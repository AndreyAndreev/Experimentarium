using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Experimentarium.Concurrency
{
    public class ThrottlingQueue
    {
        private const int ParallelTasksMaxCount = 10;
        private const int QueueMaxLenght = 100;
        private readonly ConcurrentQueue<TaskWrapper> _queue = new ConcurrentQueue<TaskWrapper>();
        private readonly LinkedList<TaskWrapper> _inProcessList = new LinkedList<TaskWrapper>();

        private long _counterQueued = 0;
        private long _counterExecuted = 0;

        private readonly ConcurrentBag<TimeSpan> _timesInQueue = new ConcurrentBag<TimeSpan>();
        private readonly ConcurrentBag<TimeSpan> _timesFromStartProcessing = new ConcurrentBag<TimeSpan>();


        private static ThrottlingQueue _instance;
        private static readonly object _lockObject = new object();

        private ThrottlingQueue()
        {
        }

        public static ThrottlingQueue Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        return _instance ?? (_instance = new ThrottlingQueue());
                    }
                }
                return _instance;
            }
        }

        public Task Queue(Action action, string name = null)
        {
            TaskWrapper taskWrapper;
            lock (_queue)
            {
                if (_queue.Count >= QueueMaxLenght)
                {
                    Console.WriteLine("Queue is full, can't add.");
                    return null;
                }

                taskWrapper = new TaskWrapper()
                {
                    Action = action,
                    Name = name,
                    Task = new Task(action)
                };

                taskWrapper.Task.ContinueWith(taskWrapper.OnCompleted, TaskContinuationOptions.OnlyOnRanToCompletion);
                taskWrapper.Task.ContinueWith(taskWrapper.OnFault, TaskContinuationOptions.OnlyOnFaulted);

                //TaskAwaiter awaiter = taskWrapper.Task.GetAwaiter();
                //awaiter.OnCompleted(taskWrapper.OnCompleted);

                long counter = Interlocked.Increment(ref _counterQueued);
                taskWrapper.Name = counter.ToString();
                _queue.Enqueue(taskWrapper);

                Console.WriteLine("Action {0} was added. \tIn Queue: {1}\tIn Process: {2}",
                    taskWrapper.Name, _queue.Count, _inProcessList.Count);
            }

            PlaceNextToInProcess();

            return taskWrapper.Task;
        }

        private bool PlaceNextToInProcess()
        {
            lock (_inProcessList)
            {
                if (_inProcessList.Count >= ParallelTasksMaxCount)
                {
                    Console.WriteLine("Too many process in executing, can't start");
                    return false;
                }

                TaskWrapper taskWrapper;
                if (!_queue.TryDequeue(out taskWrapper))
                {
                    Console.WriteLine("The queue is empty, nothing to start");
                    return false;
                }

                var linkedListNode = _inProcessList.AddLast(taskWrapper);
                taskWrapper.LinkedListNode = linkedListNode;

                _timesInQueue.Add(taskWrapper.Stopwatch.Elapsed);
                taskWrapper.Stopwatch.Restart();

                taskWrapper.Task.Start();

                Console.WriteLine("Action {0} was started. \tIn Queue: {1}\tIn Process: {2}", taskWrapper.Name, _queue.Count, _inProcessList.Count);
                return true;
            }
        }

        private void RemoveEndedTask(TaskWrapper taskWrapper)
        {
            lock (_inProcessList)
            {
                _timesFromStartProcessing.Add(taskWrapper.Stopwatch.Elapsed);
                _inProcessList.Remove(taskWrapper.LinkedListNode);

                Interlocked.Increment(ref _counterExecuted);

                Console.WriteLine("Action {0} was ended. \tIn Queue: {1}\tIn Process: {2}", taskWrapper.Name, _queue.Count, _inProcessList.Count);
            }

            PlaceNextToInProcess();
        }

        private void HandleFaultTask(TaskWrapper taskWrapper)
        {
            lock (_inProcessList)
            {
                _inProcessList.Remove(taskWrapper.LinkedListNode);
            }

            taskWrapper.Task.Exception?.Flatten().Handle(ex =>
            {
                Console.WriteLine("Action {0} throws an exception: {1}", taskWrapper.Name, ex.Message);
                return true;
            });

            PlaceNextToInProcess();
        }

        public ThrottlingQueueStatistic GetStatistic()
        {
            return new ThrottlingQueueStatistic()
            {
                TotalQueued = _counterQueued,
                TotalExecuted = _counterExecuted,
                AverageTimeInQueue = TimeSpan.FromTicks((long)_timesInQueue.Average(x => x.Ticks)),
                AverageExecutionTime = TimeSpan.FromTicks((long)_timesFromStartProcessing.Average(x => x.Ticks))
            };
        }

        private class TaskWrapper
        {
            public string Name { get; set; }
            public Action Action { get; set; }
            public Task Task { get; set; }
            public LinkedListNode<TaskWrapper> LinkedListNode { get; set; }

            public readonly Stopwatch Stopwatch;

            public TaskWrapper()
            {
                Stopwatch = Stopwatch.StartNew();
            }

            public void OnCompleted(Task task)
            {
                Stopwatch.Stop();
                ThrottlingQueue.Instance.RemoveEndedTask(this);
            }

            public void OnFault(Task task)
            {
                Stopwatch.Stop();
                ThrottlingQueue.Instance.HandleFaultTask(this);
            }
        }
    }

    public class ThrottlingQueueStatistic
    {
        public long TotalQueued { get; set; }
        public long TotalExecuted { get; set; }
        public TimeSpan AverageTimeInQueue { get; set; }
        public TimeSpan AverageExecutionTime { get; set; }
    }
}