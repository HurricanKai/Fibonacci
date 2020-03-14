using System.Threading.Tasks;

namespace Worker
{
    public interface IFibonacciService
    {
        Task<int> FAsync(int n);
    }
}