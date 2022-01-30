using System.Threading.Tasks;

namespace CounterApi.Domain
{
    public class CounterService : ICounterService
    {
        public CounterService()
        {
            Counter = 0;
        }

        public int Counter { get; protected set; }

        public async Task Increase()
        {
            Counter++;
        }

        public async Task Decrease()
        {
            Counter--;
        }
    }
}
