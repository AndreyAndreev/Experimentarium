using System;
using Experimentarium.Bitcoin;
using Experimentarium.Concurrency;

namespace Experimentarium
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("App started");

            new Volatililty().Test();
            

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

    }
}
