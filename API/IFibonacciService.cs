using System.Threading.Tasks;

namespace API
{
    public interface IFibonacciService
    {
        Task<int> FAsync(int n);
    }
}