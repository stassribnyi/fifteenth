using System.Web.Mvc;
using FifteenPuzzle.Client.Models;

namespace FifteenPuzzle.Client.Controllers
{
    public class PuzzleGameController : Controller
    {
        [HttpGet]
        public ActionResult Index(string value)
        {
            var instanse = Session["Game"] as PuzzleGameModel;

            if (instanse == null)
            {
                instanse = new PuzzleGameModel();
                Session["Game"] = instanse;

                return View(instanse);
            }

             if (!instanse.GetInstance.Ready) return View(instanse);

             var currentPuzzle = instanse.GetInstance.GetByValue(value);
             instanse.GetInstance.Go(currentPuzzle);
             instanse.GetInstance.Verify();
             Session["Game"] = instanse;

             return View(instanse);
        }

        [HttpGet]
        public ActionResult NewGame(bool reset = false)
        {
            var instanse = Session["Game"] as PuzzleGameModel;

            if (!reset) return View("Index", instanse);
            if (instanse!=null)
                instanse.GetInstance.Shuffle();

            return View("Index", instanse);
        }

    }
}