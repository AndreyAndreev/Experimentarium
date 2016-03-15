using System;
using System.Threading;
using System.Threading.Tasks;

namespace Experimentarium.Concurrency
{
    public class Volatililty
    {
        public void Test()
        {
            var waiter = new Waiter();

            waiter.Start();
            Console.WriteLine("Waiter started");

            Thread.Sleep(5000);

            waiter.Stop();
            Console.WriteLine("Waiter stopped");
        }


        public class Waiter
        {
            private volatile bool stop = false;
            private Task task;

            public void Start()
            {
                task = Task.Run(new Action(Loop));
            }

            private void Loop()
            {
                while (!stop)
                {
                    ;
                }
            }

            public void Stop()
            {
                stop = true;
                task.Wait();
            }
        }
    }
}