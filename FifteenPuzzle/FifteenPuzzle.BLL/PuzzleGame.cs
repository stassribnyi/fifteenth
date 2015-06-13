using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FifteenPuzzle.BLL.Classes;
using FifteenPuzzle.BLL.Converters;

namespace FifteenPuzzle.BLL
{
    public enum Direction { Left, Right, Up, Down, Wrong }

    public class PuzzleGame
    {
        private List<Puzzle> _gamePuzzles = new List<Puzzle>(16);
        private readonly Dictionary<Direction, Position> _direction = new Dictionary<Direction, Position>();

        public PuzzleGame()
        {
            InitDefault();
            InitDirection();
            Shuffle();
        }

        public bool Ready { get; set; }
        public int Steps { get; set; }
        
        public Puzzle[,] GetPuzzlesField
        {
            get { return ArrayConverter.ToMatrix(_gamePuzzles); }
        }

        private void InitDefault()
        {
            for (var i = 1; i <= _gamePuzzles.Capacity; i++)
            {
                _gamePuzzles.Add(new Puzzle
                {
                    PuzzleValue = i != 16 ? i.ToString(CultureInfo.InvariantCulture) : " ",
                    PuzzlePosition = new Position()
                });
            }
        }

        private void InitPosition()
        {
            for (var i = 0; i <= GetPuzzlesField.GetUpperBound(0); i++)
                for (var j = 0; j <= GetPuzzlesField.GetUpperBound(1); j++)
                {
                    GetPuzzlesField[i, j].PuzzlePosition = new Position { X = i, Y = j };
                }
        }

        private void InitDirection()
        {
            _direction.Add(Direction.Wrong, new Position
            {
                X = 0,
                Y = 0
            });
            _direction.Add(Direction.Left, new Position
            {
                X = 0,
                Y = -1
            });
            _direction.Add(Direction.Right, new Position
            {
                X = 0,
                Y = 1
            });
            _direction.Add(Direction.Up, new Position
            {
                X = -1,
                Y = 0
            });
            _direction.Add(Direction.Down, new Position
            {
                X = 1,
                Y = 0
            });

        }

        public void Shuffle()
        {
            var random = new Random();
            do { _gamePuzzles = _gamePuzzles.OrderBy(a => random.Next()).ToList(); }
            while (!DisorderParameter(_gamePuzzles));

            InitPosition();

            Steps = 0;
            Ready = true;
        }

        public bool Verify()
        {
            if (_gamePuzzles[15].PuzzleValue != " ") return false;

            var i = 1 + _gamePuzzles.Where((t, j) => t.PuzzleValue != " " && (j + 1) == Convert.ToInt16(t.PuzzleValue)).Count();

            if (i != 16) return false;

            Ready = false;

            return true;
        }

        private static bool DisorderParameter(List<Puzzle> puzzles)
        {
            var emptyElementRow = 0;
            var sum = 0;

            for (var i = 0; i < puzzles.Count; i++)
            {
                if (puzzles[i].PuzzleValue == " ") emptyElementRow = i / Convert.ToInt32(Math.Sqrt(puzzles.Count)) + 1;
                else
                {
                    for (var j = i; j < puzzles.Count; j++)
                    {
                        if (puzzles[j].PuzzleValue == " ") continue;
                        if (Convert.ToInt32(puzzles[i].PuzzleValue) > Convert.ToInt32(puzzles[j].PuzzleValue))
                            sum++;
                    }
                }
            }

            sum += emptyElementRow;

            return sum % 2 != 1;
        }

        public void Go(Puzzle currentPuzzle)
        {
            var emptyPuzzle = GetEmptyPuzzle();
            if (!CanMove(emptyPuzzle, currentPuzzle)) return;
            if (emptyPuzzle.PuzzleValue != currentPuzzle.PuzzleValue)
                Swap(currentPuzzle, emptyPuzzle);
        }

        private void Swap(Puzzle a, Puzzle b)
        {
            var tmp = b.PuzzleValue;

            b.PuzzleValue = a.PuzzleValue;
            a.PuzzleValue = tmp;

            Steps++;
        }
        
        private bool CanMove(Puzzle a, Puzzle b)
        {
            var way = new Position();
            if (a.PuzzlePosition != null && b.PuzzlePosition != null)
            {
                way = new Position
                {
                    X = a.PuzzlePosition.X - b.PuzzlePosition.X,
                    Y = a.PuzzlePosition.Y - b.PuzzlePosition.Y
                };
            }
            var dir = _direction.FirstOrDefault(x => x.Value.X == way.X && x.Value.Y == way.Y);
            return _direction.Contains(dir);
        }

        public Puzzle GetByValue(string value)
        {
            return _gamePuzzles.FirstOrDefault(x => x.PuzzleValue == value);
        }
        
        private Puzzle GetEmptyPuzzle()
        {
            return _gamePuzzles.FirstOrDefault(x => x.PuzzleValue == " ");
        }
    }
}