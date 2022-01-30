using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CounterApi.Domain;

namespace CounterApi.Console.Consumer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var counterApi1 = new CounterService();
            RunConcurrent(counterApi1);
            System.Console.WriteLine(counterApi1.Counter);

            var counterApi2 = new ThreadSafeCounterService1();
            RunConcurrent(counterApi2);
            System.Console.WriteLine(counterApi2.Counter);

            var counterApi3 = new ThreadSafeCounterService2();
            RunConcurrent(counterApi3);
            System.Console.WriteLine(counterApi3.Counter);

            var counterApi4 = new CounterService();
            RunConcurrentWithSpinLock(counterApi4);
            System.Console.WriteLine(counterApi4.Counter);
        }

        private static void RunConcurrent(ICounterService counterApi)
        {
            var t1 = Task.Factory.StartNew(async () =>
            {
                for (int j = 0; j < 1000; j++)
                {
                    await counterApi.Increase();
                }
            });
            var t2 = Task.Factory.StartNew(async () =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    await counterApi.Decrease();
                }
            });
            Task.WaitAll(t1, t2);
        }

        private static void RunConcurrentWithSpinLock(ICounterService counterApi)
        {
            SpinLock sl = new SpinLock();
            var t1 = Task.Factory.StartNew(async () =>
            {
                for (int j = 0; j < 1000; j++)
                {
                    var lockTaken = false;
                    try
                    {
                        sl.Enter(ref lockTaken);
                        await counterApi.Increase();
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            sl.Exit();
                        }
                    }
                }
            });
            var t2 = Task.Factory.StartNew(async () =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var lockTaken = false;
                    try
                    {
                        sl.Enter(ref lockTaken);
                        await counterApi.Decrease();
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            sl.Exit();

                        }
                    }
                }
            });
            Task.WaitAll(t1, t2);
        }
    }
}
