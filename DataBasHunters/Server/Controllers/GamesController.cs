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

        [HttpGet("coinflip/{gameId}")]
        public IActionResult GetCoinflip([FromRoute] int gameId)
        {
            UserMethods um = new UserMethods();
            GameMethods gm = new GameMethods();
            string error = "";
            string error2 = "";
            string error3 = "";
            string error4 = "";
            CoinFlipModel cm = um.GetCoinFlipModel(gameId, out error);
            int opponentId = (int)HttpContext.Session.GetInt32("UserId");

            User opp = um.GetUser(opponentId, out error);
            Cointoss gamect = gm.GetGameById(gameId, out error2);
            int creatorId = um.GetUserByGameId(gameId, out error3);
            User create = um.GetUser(creatorId, out error4);

            ViewModelCoinFlip vm = new ViewModelCoinFlip()
            {
                creator = create,
                opponent = opp,
                game = gamect
            };

            return Ok(vm);
        }

        [HttpPost("coinflip/{gameId}")]
        public IActionResult AcutalCoinFlip([FromBody] FinishGame finish)
        {
            GameMethods gm = new GameMethods();
            string error1 = "";
            string error2 = "";
            gm.JoinGame(finish.game, finish.joinperson, out error1);
            gm.FinishGame(finish.game, finish.winner.Id, finish.loser.Id, out error2);

            return Ok();
        }

    }
}
