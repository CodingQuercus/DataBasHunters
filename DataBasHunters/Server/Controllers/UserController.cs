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

        [HttpGet("GetUser")]
        public IActionResult UserProfile()
        {
            UserMethods um = new UserMethods();
            string error = "";

            var users = um.GetUsers(out error);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(new { Error = error });
            }

            // Hämta användar-ID från sessionsvariabeln
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                // Hitta användaren med det angivna ID:et
                var user = users.FirstOrDefault(u => u.Id == userId.Value);

                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound(new { Error = "Användare med det angivna ID:et hittades inte." });
                }
            }
            else
            {
                return BadRequest(new { Error = "Sessionsvariabeln 'UserId' saknas eller är ogiltig." });
            }
        }


        [HttpPost("InsertUser")]
        public IActionResult InserUser([FromBody] User newUser)
        {
            if (ModelState.IsValid)
            {
                string error = "";
                UserMethods um = new UserMethods();
                var success = um.AddUser(newUser, out error);

                if (success == 1)
                {
                    return Ok(newUser);
                }
                else
                {
                    return BadRequest(new { Error = error });
                }
            }
            else
            {
                return BadRequest(new { Error = "Invalid data submitted." });
            }
        }

        [HttpPost("LoginUser")]
        public IActionResult LoginUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                string error = "";
                UserMethods um = new UserMethods();
                var success = um.LoginUser(user, out error);

                if (success == 1)
                {
                    int id = um.GetUserId(user.Username, out error);
                    HttpContext.Session.SetInt32("UserId", id);
                    return Ok(user);
                }
                else
                {
                    return BadRequest(new { Error = error });
                }
            }
            else
            {
                return BadRequest(new { Error = "Invalid data submitted." });
            }
        }


    }
}
