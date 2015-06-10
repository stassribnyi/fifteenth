using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FifteenPuzzle.Models;

namespace FifteenPuzzle.Controllers
{
    public class PuzzleGameController : Controller
    {
        // GET: PuzzleGame
        [HttpGet]
        public ActionResult FifteenGame(int? x, int? y, bool isReset = false)
        {
            var model = Session["gameModel"] as PuzzleGameModel;
           
            if (isReset)
            {
                model = new PuzzleGameModel();
                //model.Shuffle();
                Session["GameModel"] = model;
                return View(model);
            }
            if (x != null && y != null)
            {
                var _x = (int) x;
                var _y = (int) y;
                var currentPuzzle = new Puzzle
                {
                    PuzzlePosition = new Position
                    {
                        X = _x,
                        Y = _y
                    }
                };

                if (!model.Ready)
                    model.Shuffle();

                model.MovePuzzle(currentPuzzle);
                model.Verify();

                Session["GameModel"] = model;
            }
            return View(model);
        }
    }
}