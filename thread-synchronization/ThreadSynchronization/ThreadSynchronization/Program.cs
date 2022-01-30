using System;
using System.Threading;

namespace ThreadSynchronization
{
    internal class Program
    {
        private static object _padlock = new object();
        private static int result = 0;
        public static EventWaitHandle readyForResult = new AutoResetEvent(false);
        public static EventWaitHandle setRedult = new AutoResetEvent(false);


        public static void DoWork()
        {
            while (true)
            {
                // signal to main thread to wait for shared variable to be changed
                readyForResult.WaitOne();

                int i = result;
                Thread.Sleep(1);
                lock (_padlock)
                {
                    result = i + 1;
                }

                // signal to main thread shared variable changed
                setRedult.Set();
            }
        }

        static void Main(string[] args)
        {
            Thread t = new Thread(DoWork);
            t.Start();

            for (int i = 0; i < 100; i++)
            {
                // tell thread we are ready for result
                readyForResult.Set();
                // wait until result will be set
                setRedult.WaitOne();

                lock (_padlock)
                {
                    Console.WriteLine(result);
                }
                
                Thread.Sleep(10);
            }
            Console.WriteLine("Hello World!");
        }
    }
}
