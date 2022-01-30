using System.Threading;
using System.Threading.Tasks;

namespace CounterApi.Domain
{
    public class ThreadSafeCounterService1 : ICounterService
    {
        private object padlock = new object();

        public ThreadSafeCounterService1()
        {
            Counter = 0;
        }

        public int Counter { get; protected set; }

        public async Task Increase()
        {
            lock (padlock)
            {
                Counter++;
            }
        }

        public async Task Decrease()
        {
            lock (padlock)
            {
                Counter--;
            }
        }
    }
}
