using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FibonacciController : ControllerBase
    {
        private readonly ILogger<FibonacciController> _logger;
        private readonly IFibonacciService _fibonacciService;

        public FibonacciController(ILogger<FibonacciController> logger, IFibonacciService fibonacciService)
        {
            _logger = logger;
            _fibonacciService = fibonacciService;
        }

        [HttpGet("{n}")]
        public Task<int> Get([FromRoute] int n)
        {
            return _fibonacciService.FAsync(n);
        }
    }
}