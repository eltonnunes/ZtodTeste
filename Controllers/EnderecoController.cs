using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using src.Models;
using src.Services;
using System;
using System.Linq;

namespace src.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnderecoController : ControllerBase
    {
        private readonly devContext _db;

        public EnderecoController(devContext context, ILogger<EnderecoController> logger)
        {
            _db = context;
            ILogger _logger = logger;
        }

        [HttpPost("AddEndereco")]
        public async Task<ActionResult<ExpandoObject>> AddEndereco([FromBody] Endereco endereco)
        {
            var result = await EnderecoService.Create(_db, endereco);
            return result;
        }
    }
}
