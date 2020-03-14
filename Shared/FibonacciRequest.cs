using System;

namespace Shared
{
    public class FibonacciRequest : FibonacciRequestV1
    {
        public Guid CorrelationId { get; set; }
        public int N { get; set; }
    }
}