using System.Threading.Tasks;

namespace CounterApi.Domain
{
    public interface ICounterService
    {
        int Counter { get; }
        Task Increase();
        Task Decrease();
    }
}