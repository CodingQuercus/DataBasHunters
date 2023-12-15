using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataBasHunters.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Hosting;

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

        [HttpGet("GetUserId")]
        public IActionResult GetUserId()
        {
            // Hämta användar-ID från sessionsvariabeln
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                int userId = (int)HttpContext.Session.GetInt32("UserId");
                return Ok(userId);
            }

            return Ok();
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
        public async Task<IActionResult> UpdateImage([FromForm] byte[] imageFile, [FromForm] string fileName, [FromForm] int userId)
        {
            try
            {
                if (imageFile.Length == 0 || string.IsNullOrEmpty(fileName))
                {
                    return BadRequest("Invalid file data");
                }

                if (imageFile.Length > 0 && !string.IsNullOrEmpty(fileName))
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                await System.IO.File.WriteAllBytesAsync(filePath, imageFile);

                string error = "";
                UserMethods um = new UserMethods();

                // Use the userId to update the user's profile picture
                var success = um.UpdateUserImage(userId, filePath, out error);

                if (success == 1)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(error);
                }
            }
            return BadRequest("Invalid file data");
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        public class UserImageUpdateModel
        {
            public int Id { get; set; }
            public string Url { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserImage([FromBody] UserImageUpdateModel model)
        {
            // Call your data access layer method to update the image path
            string error;

            UserMethods um = new UserMethods();

            int result = um.UpdateUserImage(model.Id, model.Url, out error);

            if (result == 1)
            {
                return Ok();
            }
            else
            {
                return BadRequest(error);
            }
        }
      
    }
}
