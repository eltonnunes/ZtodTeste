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
    public class ExtratoController : ControllerBase
    {
        private readonly devContext _db;

        public ExtratoController(devContext context, ILogger<ExtratoController> logger)
        {
            _db = context;
            ILogger _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ExpandoObject>> Get()
        {
            IQueryCollection queryString = Request.Query;
            var result = await ExtratoService.ListAll(_db, queryString);
            return result;
        }

        [HttpGet]
        [Route("saldo")]
        public async Task<ActionResult<ExpandoObject>> GetSaldo()
        {
            IQueryCollection queryString = Request.Query;
            var result = await ExtratoService.Saldo(_db, queryString);
            return result;
        }
    }
}
