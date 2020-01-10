using System;
using System.Collections.Generic;
using System.Text;

namespace Alogrithms.DynamicProgramming
{
    class LevenshteinDistance
    {
        string from;
        string to;
        public int Distance;

        public LevenshteinDistance(string from, string to)
        {
            this.from = from;
            this.to = to;
            solve();
        }

        private void solve()
        {
            int fromLength = from.Length, toLength = to.Length;
            int[,] matrix = new int[fromLength + 1, toLength + 1]; // length + 1 because there we will store start values

            // initialize start values
            for (int i = 0; i < toLength + 1; i++)
            {
                matrix[0, i] = i;
            }

            for (int i = 0; i < fromLength + 1; i++)
            {
                matrix[i, 0] = i;
            }

            // store values into the table 
            for (int row = 1; row < fromLength + 1; row++)
            {
                for (int col = 1; col < toLength + 1; col++)
                {
                    int value = Math.Min(matrix[row - 1, col],
                                Math.Min(matrix[row - 1, col - 1], matrix[row, col - 1]));

                    if (!from[row - 1].Equals(to[col - 1]))
                    {
                        matrix[row, col] = value + 1; // we add 1 to the value because we do 1 operation
                    }
                    else
                    {
                        matrix[row, col] = value;
                    }

                }
            }

            Distance = matrix[fromLength, toLength]; // return the last value in the table
        }
    }

    class LevenshteinTest
    {
        static void Main(string[] args)
        {
            LevenshteinDistance editDistance = new LevenshteinDistance("kitten", "sitting");
            Console.WriteLine(editDistance.Distance);
        }

    }
}
