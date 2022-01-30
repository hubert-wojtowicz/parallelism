using System.Threading;
using System.Threading.Tasks;

namespace CounterApi.Domain
{
    public class ThreadSafeCounterService2 : ICounterService
    {
        public ThreadSafeCounterService2()
        {
            Counter = 0;
        }

        private int _counter;

        public int Counter
        {
            get { return _counter; }
            protected set { _counter = value; }
        }

        public async Task Increase()
        {
            Interlocked.Increment(ref _counter);
        }

        public async Task Decrease()
        {
            Interlocked.Decrement(ref _counter);
        }
    }
}
