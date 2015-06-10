using System;
using System.Collections.Generic;
using System.Linq;

namespace FifteenPuzzle.Models
{
    public class PuzzleGameModel
    {
        private readonly List<Puzzle> _defaultPuzzles = new List<Puzzle>(16);
        private List<Puzzle> _gamePuzzles = new List<Puzzle>(16);
        private readonly Dictionary<Direction, Position> _direction = new Dictionary<Direction, Position>();
        
        public PuzzleGameModel()
        {
            InitDefault();
            InitDirection();
            Shuffle();
        }

        public Position EmptyPuzzlePosition { get; set; }
        public bool Ready { get; set; }
        public bool End { get; set; }
        public int Steps { get; set; }

        public List<Puzzle> GetPuzzles
        {
            get { return _gamePuzzles; }
        }

        public Puzzle[,] GetPuzzlesField
        {
            get { return ArrayConverter.ToMatrix(_gamePuzzles); }
        }
        
        private void InitDefault()
        {
            for (var i = 1; i <= _defaultPuzzles.Capacity; i++)
            {
                _defaultPuzzles.Add(new Puzzle
                {
                    PuzzleValue = i != 16 ? i.ToString() : ""
                });
            }
        }

        private void InitDirection()
        {
            _direction.Add(Direction.Left, new Position
            {
                X = -1,
                Y = 0
            });
            _direction.Add(Direction.Right, new Position
            {
                X = 1,
                Y = 0
            });
            _direction.Add(Direction.Up, new Position
            {
                X = 0,
                Y = -1
            });
            _direction.Add(Direction.Down, new Position
            {
                X = 0,
                Y = 1
            });
            _direction.Add(Direction.Wrong, new Position
            {
                X = 0,
                Y = 0
            });
        }

        public void Shuffle()
        {
            var random = new Random();
            do { _gamePuzzles = _defaultPuzzles.OrderBy(a => random.Next()).ToList(); }
            while (!DisorderParameter(_gamePuzzles));
            Ready = true;
        }

        private void Swap(Puzzle a, Puzzle b)
        {
            var tmp = _gamePuzzles.Last(x => x.PuzzlePosition == b.PuzzlePosition);

            _gamePuzzles.Insert(_gamePuzzles.IndexOf(b), a);
            _gamePuzzles.Insert(_gamePuzzles.IndexOf(a), tmp);

            Steps++;
        }

        public bool Verify()
        {
            var equal = _defaultPuzzles.SequenceEqual(_gamePuzzles);
            return equal;
        }

        private bool DisorderParameter(List<Puzzle> puzzles)
        {
            var emptyElementRow = 0;
            var sum = 0;

            for (var i = 0; i < puzzles.Count; i++)
            {
                if (puzzles[i].PuzzleValue == "") emptyElementRow = i/Convert.ToInt32(Math.Sqrt(puzzles.Count)) + 1;
                else
                {
                    for (var j = i; j < puzzles.Count; j++)
                    {
                        if (puzzles[j].PuzzleValue != "")
                            if (Convert.ToInt32(puzzles[i].PuzzleValue) > Convert.ToInt32(puzzles[j].PuzzleValue))
                                sum++;
                    }
                }
            }

            sum += emptyElementRow;

            return sum%2 != 1;
        }

        //i do not understand this realization
        public void MovePuzzle(Puzzle puzzle)
        {
            var direction = GetDirection(puzzle);
            var currentPuzzle = new Puzzle();
            var emptyPuzzle = new Puzzle
            {
                PuzzleValue = "",
                PuzzlePosition = new Position
                {
                    X = EmptyPuzzlePosition.X - 1,
                    Y = EmptyPuzzlePosition.Y - 1
                }
            };

            currentPuzzle.PuzzlePosition.X = emptyPuzzle.PuzzlePosition.X - _direction[direction].X;
            currentPuzzle.PuzzlePosition.Y = emptyPuzzle.PuzzlePosition.Y - _direction[direction].Y;

            Swap(emptyPuzzle, currentPuzzle);
            End=Verify();
        }

        private Direction GetDirection(Puzzle currentPuzzle)
        {
            for (var i = 0; i < GetPuzzlesField.GetUpperBound(0); i++)
                for (var j = 0; j < GetPuzzlesField.GetUpperBound(1); j++)
                {
                    if (GetPuzzlesField[i,j].PuzzleValue == "")
                    {
                        EmptyPuzzlePosition.X = j + 1;
                        EmptyPuzzlePosition.Y = i + 1;
                    }
                }

            var way = new Position
            {
                X = currentPuzzle.PuzzlePosition.X - EmptyPuzzlePosition.X,
                Y = currentPuzzle.PuzzlePosition.Y - EmptyPuzzlePosition.Y
            };

            return _direction.ContainsValue(way) ? _direction.Last(x => x.Value == way).Key : Direction.Wrong;
        }
    }
    #region helping classes

    public class Puzzle
    {
        public string PuzzleValue { get; set; }
        public Position PuzzlePosition { get; set; }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum Direction { Left, Right, Up, Down, Wrong }

    public static class ArrayConverter
    {
        public static List<T> ToArray<T>(T[,] matrix)
        {
            var array = new List<T>();

            //convert 2d array into 1d
            for (var i = 0; i <= matrix.GetUpperBound(0); i++)
                for (var j = 0; j <= matrix.GetUpperBound(1); j++)
                    array.Add(matrix[i, j]);

            return array;
        }

        public static T[,] ToMatrix<T>(List<T> array)
        {
            var rank = Convert.ToInt32(Math.Sqrt(array.Count));
            var matrix = new T[rank, rank];
            var index = 0;

            //convert 2d array into 1d
            for (var i = 0; i < rank; i++)
                for (var j = 0; j < rank; j++)
                {
                    matrix[i, j] = array[index];
                    index++;
                }

            return matrix;
        }
    }
#endregion
}