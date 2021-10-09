using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoutePlanning
{
    class Cell
    {
        public int Value { get; private set; }
        public int I { get; private set; }
        public int J { get; private set; }

        public Cell()
        {
            Value = 0;
            I = 0;
            J = 0;
        }

        public Cell(int value, int i, int j)
        {
            Value = value;
            I = i;
            J = j;
        }
    }

    public static class PathFinderTask
    {
        public static int[] FindBestCheckpointsOrder(Point[] p)
        {
            int[] minPath = GetMinPath(p);
            return MakeTrivialPermutation(p.Length);
        }

        private static int[] MakeTrivialPermutation(int size)
        {
            var bestOrder = new int[size];
            for (int i = 0; i < bestOrder.Length; i++)
                bestOrder[i] = i;
            return bestOrder;
        }

        private static int[] GetMinPath(Point[] checkpoints)
        {
            int countPoints = checkpoints.Length;
            int[,] matrDistance = GetMatrixDistance(checkpoints);
            ShowMatrix(matrDistance);
            if (countPoints == 2)
            {
                return new int[] { 0, 1 };
            }

            int[] minValuesByRow = GetSumInRowOrCol(matrDistance, true);
            int[] minValuesByCol = GetSumInRowOrCol(matrDistance, false);
            
            int sumMinValuesByRow = 0;
            int sumMinValuesByCol = 0;
            
            for (int i = 0; i < countPoints; i++)
            {
                sumMinValuesByRow += minValuesByRow[i];
                sumMinValuesByCol += minValuesByCol[i];
            }

            int[,] matrixReduced = GetMatrixMinusValueByRowOrCol(matrDistance, minValuesByRow, true);
            matrixReduced = GetMatrixMinusValueByRowOrCol(matrDistance, minValuesByCol, false);
            ShowMatrix(matrixReduced);

            return new int[0];
        }

        private static int[,] GetMatrixMinusValueByRowOrCol(int[,] matrix, int[] value, bool isRow)
        {
            int row = matrix.GetLength(0);
            int col = matrix.GetLength(1);
            int[,] newMatrix = new int[row, col];
            int cell;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    cell = (isRow) ? matrix[i, j] : matrix[j, i];
                    newMatrix[i, j] = (cell != -1) ? cell - value[j] : cell;                   
                }
            }
            return newMatrix;
        }

        private static int[] GetSumInRowOrCol(int[,] matrix, bool isRow)
        {
            int min = int.MaxValue;
            int[] minValues = new int[matrix.GetLength(0)];
            int cell;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    cell = (isRow) ? matrix[i, j] : matrix[j, i];
                    if (cell < min && cell != -1)
                    {
                        min = cell;
                    }
                }
                minValues[i] = min;
                min = int.MaxValue;
            }
            return minValues;
        }

        private static int[,] GetMatrixDistance(Point[] checkpoints)
        {
            int sizeCheckPoints = checkpoints.Length;
            int[,] distance = new int[sizeCheckPoints, sizeCheckPoints];
            int indexI = 0;
            int indexJ = 0;
            for (int i = 0; i < sizeCheckPoints; i++)
            {
                for (int j = 0; j < sizeCheckPoints; j++)
                {
                    distance[i, j] = (i != j) ? GetSize(checkpoints[i], checkpoints[j]) : -1;
                    ++indexJ;
                }
                indexJ = 0;
                ++indexI;
            }

            return distance;
        }

        private static int GetSize(Point a, Point b)
        {
            return (b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y);
        }

        private static void ShowMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j]} \t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("----------------------------------------");
        }
    }
}