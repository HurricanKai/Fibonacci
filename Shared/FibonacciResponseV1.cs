using System;
using MassTransit;

namespace Shared
{
    public interface FibonacciResponseV1 : CorrelatedBy<Guid>
    {
        public int Result { get; }
    }
}