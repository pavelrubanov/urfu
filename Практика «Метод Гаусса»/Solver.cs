using System;
using System.Collections.Generic;
using System.Linq;

namespace GaussAlgorithm
{
    public class Solver
    {
        public double[] Solve(double[][] matrix, double[] freeMembers)
        {
            var result = new double[matrix[0].Length];
            var usedLines = new Dictionary<int, int>();

            for (int column = 0; column < matrix[0].Length; column++)
            {
                var allNules = true;
                for (int line = 0; line < matrix.Length; line++)
                {
                    if (!usedLines.ContainsKey(line) && matrix[line][column] != 0)
                    {
                        allNules = false;
                        usedLines.Add(line, column);
                        Normalize(matrix, freeMembers, column, line);
                        MakeTriangleMatrix(matrix, freeMembers, column, line);
                    }
                }
                if (allNules)
                    result[column] = 0;
            }

            for (int line = 0; line < matrix.Length; line++)
            {
                if (matrix[line].All(a => a == 0) && Math.Round(freeMembers[line], 3) != 0)
                    throw new NoSolutionException("System has no solution!");
            }

            foreach (var line in usedLines)
            {
                result[line.Value] = Math.Round(freeMembers[line.Key], 3);
            }
            return result;
        }

        private static void MakeTriangleMatrix(double[][] matrix, double[] freeMembers, int column, int line)
        {
            for (int internalLine = 0; internalLine < matrix.Length; internalLine++)
            {
                if (internalLine != line)
                {
                    var multiplexor = -1 * matrix[internalLine][column];
                    freeMembers[internalLine] = Math.Round(freeMembers[internalLine] + multiplexor * freeMembers[line], 3);
                    for (int internalColumn = 0; internalColumn < matrix[0].Length; internalColumn++)
                    {
                        matrix[internalLine][internalColumn] = Math.Round(matrix[internalLine][internalColumn]
                            + multiplexor * matrix[line][internalColumn], 3);
                    }
                }
            }
        }

        private static void Normalize(double[][] matrix, double[] freeMembers, int column, int line)
        {
            var devider = matrix[line][column];
            freeMembers[line] = freeMembers[line] / devider;
            for (int i = 0; i < matrix[0].Length; i++)
            {
                matrix[line][i] = matrix[line][i] / devider;
            }
        }
    }
}