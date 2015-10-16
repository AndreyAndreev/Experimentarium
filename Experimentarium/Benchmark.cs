using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Experimentarium
{
    public class Benchmark
    {
        public static void Run(Action action, int times = 1)
        {
            WriteStart(action);

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < times; i++)
            {
                action();
            }

            stopwatch.Stop();
            WriteEnd(action, stopwatch);
        }

        public static void RunAsync(Action action, int times = 1)
        {
            WriteStart(action);

            var stopwatch = Stopwatch.StartNew();

            var tasks = new List<Task>();

            for (int i = 0; i < times; i++)
            {
                tasks.Add(Task.Factory.StartNew(action));
            }

            tasks.ForEach(task => task.Wait());

            stopwatch.Stop();
            WriteEnd(action, stopwatch);
        }

        private static void WriteStart(Action action)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0} started...", action.Method.Name);
            Console.ForegroundColor = color;
        }

        private static void WriteEnd(Action action, Stopwatch stopwatch)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0} ended: {1}.", action.Method.Name, stopwatch.Elapsed);
            Console.ForegroundColor = color;
        }
    }
}