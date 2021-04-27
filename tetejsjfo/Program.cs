using System;
using System.Threading;
using System.Threading.Tasks;

namespace tetejsjfo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var task1 = new Task(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine(i);
                }
            });
            var task2 = new Task(() =>
            {
                for (int i = 20; i < 40; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine(i);
                }
            });

            task1.Start(); task2.Start();
            await Task.WhenAll(task1, task2);

            Console.WriteLine("Hello World!");
        }
    }
}
