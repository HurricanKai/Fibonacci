using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared;

namespace Worker
{
    public class FibonacciRequestV1Consumer : IConsumer<FibonacciRequestV1>
    {
        private readonly ILogger _logger;
        private IFibonacciService _fibonacciService;

        public FibonacciRequestV1Consumer(ILogger<FibonacciRequestV1Consumer> logger, IFibonacciService fibonacciService)
        {
            _logger = logger;
            _fibonacciService = fibonacciService;
        }
        
        public async Task Consume(ConsumeContext<FibonacciRequestV1> context)
        {
            var n = context.Message.N;
            _logger.LogInformation($"Processing {n} | {context.CorrelationId}");
            var result = await _fibonacciService.FAsync(n);
            await context.RespondAsync<FibonacciResponseV1>(new {Result = result, context.CorrelationId});
        }
    }
}