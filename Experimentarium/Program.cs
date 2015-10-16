using System;

namespace Experimentarium
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("App started");

            new Concurrency.Concurrency().Main();

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
