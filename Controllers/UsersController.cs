using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using src.Models;
using src.Services;

namespace src.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        

        private readonly devContext _db;

        public UsersController(devContext context, ILogger<UsersController> logger)
        {
            _db = context;
            ILogger _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ExpandoObject>> Get() //List<User> 
        {
                IQueryCollection queryString = Request.Query;
            var result = await UserService.ListAll(_db,queryString);
            return result;

        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<ExpandoObject>> AddUser([FromBody] User user)
        {
            var result = await UserService.Create(_db, user);
            return result;
        }
    }
}
