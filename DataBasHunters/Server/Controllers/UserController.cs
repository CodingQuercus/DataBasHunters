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
            TransactionMethods tm = new TransactionMethods();
            string error = "";
            
            // Hämta användar-ID från sessionsvariabeln
            if (HttpContext.Session.GetInt32("UserId").HasValue) {

             int userId = (int)HttpContext.Session.GetInt32("UserId");

                ViewModelProfile vm = new ViewModelProfile()
                {
                    user = um.GetUser(userId, out error),
                    history = tm.GetTransactions(userId, out error)
                };

                return Ok(vm);
            }

            return BadRequest();
            
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

        [HttpPost("AddFundsToUser")]
        public IActionResult AddFunds([FromBody] int funds)
        {
            if (ModelState.IsValid)
            {
                string error = "";
                UserMethods um = new UserMethods();
                var success = um.AddFunds((int)HttpContext.Session.GetInt32("UserId"), funds, out error);

                return Ok();

            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("UpdateProfileImage")]
        public IActionResult UpdateImage([FromBody] User user) {
            if(ModelState.IsValid) {
                string error = "";
                UserMethods um = new UserMethods();
                var success = um.UpdateUserImage(user.Id, user.Profilepicture, out error);

                return Ok();

            } else
            {
                return BadRequest();
            }

        }



    }
}
