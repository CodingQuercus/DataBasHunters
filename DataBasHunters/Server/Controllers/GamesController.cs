using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataBasHunters.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpGet("GetMyGames")]
        public IActionResult FetchDataMyGames()
        {

            GameMethods gm = new GameMethods();
            string error = "";
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            var games = gm.GetMyGames(userId, out error);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(new { Error = error });
            }

            return Ok(games);
        }
        [HttpPost("DeleteGame/{gameId}")]
        public IActionResult DeleteGame([FromRoute] int gameId)
        {
            if (ModelState.IsValid)
            {
                string error = "";
                GameMethods cm = new GameMethods();

                int userId = (int)HttpContext.Session.GetInt32("UserId");

                var success = cm.DeleteGame(userId, gameId, out error);

                if (success == 1)
                {
                    return Ok(gameId);
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

        [HttpGet("Coinflip/{gameId}")]
        public IActionResult GetCoinflip([FromRoute] int gameId)
        {
            try
            {
                Console.WriteLine($"Trying to get Coinflip for GameId: {gameId}");
                string error = "";
                UserMethods um = new UserMethods();
                CoinFlipModel cm = um.GetCoinFlipModel(gameId, out error);

                ViewModelCoinFlip vm = new ViewModelCoinFlip
                {
                    opponent = um.GetUser((int)HttpContext.Session.GetInt32("UserId"), out error),
                    creator = cm.creator,
                    game = cm.game
                };

                if (vm == null)
                {
                    Console.WriteLine($"Error in GetCoinflip: {error}");
                    return BadRequest(new { Error = error });
                }

                Console.WriteLine("Successfully retrieved Coinflip data.");
                return Ok(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetCoinflip: {ex.Message}");
                return BadRequest(new { Error = "Error fetching ViewModelCoinFlip" });
            }
        }


    }
}
