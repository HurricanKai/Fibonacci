using System;

namespace Shared
{
    public class FibonacciResponse : FibonacciResponseV1
    {
        public Guid CorrelationId { get; set; }
        public int Result { get; set; }
    }
}