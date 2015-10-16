using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Experimentarium.Concurrency
{
    public class Concurrency
    {
        private const int Count = 10;
        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        public void Main()
        {
            Benchmark.Run(UseThrottlingQueue);
        }

        private void DoSomeStuff()
        {
            Thread.Sleep(_random.Next(100, 500));

            if (_random.Next(100) >= 60)
            {
                throw new Exception("The task is eventually throws an exception");
            }
        }

        private async void UseThrottlingQueue()
        {
            var action = new Action(DoSomeStuff);

            var tasks = new List<Task>();

            for (int i = 0; i < Count; i++)
            {
                Task task;

                //Thread.Sleep(1000);

                while ((task = ThrottlingQueue.Instance.Queue(action)) == null)
                {
                    Thread.Sleep(1000);
                }
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            var stat = ThrottlingQueue.Instance.GetStatistic();
            Console.WriteLine("Statistics:");
            Console.WriteLine("Total queued: {0}\nTotal executed: {1}\nAverage time in queue: {2}\nAverage execution time: {3}",
                stat.TotalQueued, stat.TotalExecuted, stat.AverageTimeInQueue, stat.AverageExecutionTime);
        }
    }
}