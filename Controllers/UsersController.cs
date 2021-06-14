using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using src.database;
using src.database.Models;

namespace src.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        

        public ApplicationDBContext _db;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<User> Get()
        {


            List<User> list = _db.Users.Select( e => e ).ToList<User>();
            return list;
    

        }
    }
}
