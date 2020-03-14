using System;
using MassTransit;

namespace Shared
{
    public interface FibonacciRequestV1 : CorrelatedBy<Guid>
    {
        public int N { get; }
    }
}