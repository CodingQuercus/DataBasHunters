using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataBasHunters.Shared;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataBasHunters.Server.Controllers
{
    public class UserController : Controller
    {
        [HttpGet("GetUsers")]
        public IActionResult FetchDataUsers()
        {

            UserMethods um = new UserMethods();
            string error = "";
            var users = um.GetUsers(out error);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(new { Error = error });
            }

            return Ok(users);
        }
    }
}
