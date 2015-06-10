using FifteenPuzzle.BLL;

namespace FifteenPuzzle.Client.Models
{
    public class PuzzleGameModel
    {
        private PuzzleGame _instanceGame;

        public PuzzleGameModel()
        {
            _instanceGame = new PuzzleGame();
        }

        public PuzzleGame GetInstance {
            get { return _instanceGame; }
        }
    }
}