using System;
using System.Collections.Generic;

namespace FifteenPuzzle.BLL.Converters
{
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
}
