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

        [HttpPost]
        public IActionResult CreateGame(Cointoss ct)
        {
            GameMethods gm = new GameMethods();
            int i = 0;
            string error = "";
            i = gm.CreateGame(ct, out error);
            ViewBag.error = error;
            ViewBag.antal = i;

            return Ok(new { error, antal = i });
        }

        [HttpPost("InsertCustomer")]
        public IActionResult InsertCustomer([FromBody] Cointoss newCointoss)
        {
            if (ModelState.IsValid)
            {
                string error = "";
                GameMethods cm = new GameMethods();
                var success = cm.AddGame(newCointoss, out error);

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
    }
}
