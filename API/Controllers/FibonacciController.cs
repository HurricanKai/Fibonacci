using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FibonacciController : ControllerBase
    {
        private readonly ILogger<FibonacciController> _logger;
        private readonly IRequestClient<FibonacciRequestV1> _requestClient;

        public FibonacciController(ILogger<FibonacciController> logger, IRequestClient<FibonacciRequestV1> requestClient)
        {
            _logger = logger;
            _requestClient = requestClient;
        }

        [HttpGet("{n}")]
        public async Task<int> Get([FromRoute] int n)
        {
            var response = await _requestClient.GetResponse<FibonacciResponseV1>(new {N = n, CorrelationId = NewId.NextGuid()});
            return response.Message.Result;
        }
    }
}