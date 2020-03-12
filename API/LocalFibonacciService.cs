using System.Threading.Tasks;

namespace API
{
    public sealed class LocalFibonacciService : IFibonacciService
    {
        public Task<int> FAsync(int n)
        {
            return Task.FromResult(F(n));
        }

        private int F(int n)
        {
            if (n == 0 || n == 1)
                return n;

            return F(n - 1) + F(n - 2);
        }
    }
}