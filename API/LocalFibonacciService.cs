using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace API
{
    public sealed class LocalFibonacciService : IFibonacciService
    {
        private readonly IMemoryCache _cache;

        public LocalFibonacciService(IMemoryCache cache)
        {
            _cache = cache;
        }
        
        public Task<int> FAsync(int n)
        {
            if (n == 0 || n == 1)
                return Task.FromResult(n);
            return _cache.GetOrCreateAsync<int>(n, async entry =>
            {

                var a = FAsync(n - 1);
                var b = FAsync(n - 2);
                await Task.WhenAll(a, b);

                return a.Result + b.Result;
            });
        }
    }
}