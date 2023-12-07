using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataBasHunters.Shared;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataBasHunters.Server.Controllers
{
    public class GamesControllers : Controller
    {
        [HttpGet("GetGames")]
        public IActionResult FetchDataUsers()
        {

            GameMethods gm = new GameMethods();
            string error = "";
            var games = gm.GetGames(out error);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(new { Error = error });
            }

            return Ok(games);
        }

        [HttpPost("InsertGame")]
        public IActionResult InsertGame([FromBody] Cointoss newCointoss)
        {
            if (ModelState.IsValid)
            {
                string error = "";
                GameMethods cm = new GameMethods();
                int userId = (int)HttpContext.Session.GetInt32("UserId");
                var success = cm.AddGame(newCointoss, userId, out error);

                if (success == 1)
                {
                    return Ok(newCointoss);
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

        [HttpGet("Coinflip")]
        public IActionResult Coinflip(int id)
        {
            GameMethods gm = new GameMethods();
            string error = "";

            Cointoss cointoss = gm.GetGameById(id, out error);

            if (cointoss == null)
            {
                ViewBag.error = error;
                return RedirectToAction("Coinflip");
            }

            return View(cointoss);
        }


    }
}
