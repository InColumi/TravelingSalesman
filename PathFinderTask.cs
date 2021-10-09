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
        private static int[,] _matrixDistance;
        private static int _sizeRow;
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

        private static Cell FindMaxAssessment(int[,] matrix)
        {
            int minValueInRow = int.MaxValue;
            int maxAssessment = 0;
            int assessmentI = 0;
            int assessmentJ = 0;
            int cell;
            for (int i = 0; i < _sizeRow; i++)
            {
                for (int j = 0; j < _sizeRow; j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        for (int k = 0; k < _sizeRow; k++)
                        {
                            cell = matrix[i, k];
                            if (cell != -1 && j != k && minValueInRow > cell)
                            {
                                minValueInRow = cell;
                                assessmentI = i;
                                assessmentJ = k;
                            }
                        }

                        if (maxAssessment < minValueInRow)
                        {
                            maxAssessment = minValueInRow;
                        }
                        minValueInRow = 0;
                    }
                }
            }
            return new Cell(maxAssessment, assessmentI, assessmentJ);
        }

        private static int[] GetMinPath(Point[] checkpoints)
        {
            _sizeRow = checkpoints.Length;
            SetMatrixDistance(checkpoints);
            ShowMatrix(_matrixDistance);
            if (_sizeRow == 2)
            {
                return new int[] { 0, 1 };
            }

            int[] minValuesByRow = GetSumInRowOrCol(_matrixDistance, true);
            int[] minValuesByCol = GetSumInRowOrCol(_matrixDistance, false);
            
            int sumMinValuesByRow = 0;
            int sumMinValuesByCol = 0;
            
            for (int i = 0; i < _sizeRow; i++)
            {
                sumMinValuesByRow += minValuesByRow[i];
                sumMinValuesByCol += minValuesByCol[i];
            }

            int[,] matrixReduced = GetMatrixMinusValueByRowOrCol(_matrixDistance, minValuesByRow, true);
            matrixReduced = GetMatrixMinusValueByRowOrCol(_matrixDistance, minValuesByCol, false);
            ShowMatrix(matrixReduced);

            Cell assessment = FindMaxAssessment(matrixReduced);

            return new int[0];
        }

        private static int[,] GetMatrixMinusValueByRowOrCol(int[,] matrix, int[] value, bool isRow)
        {
            int[,] newMatrix = new int[_sizeRow, _sizeRow];
            int cell;
            for (int i = 0; i < _sizeRow; i++)
            {
                for (int j = 0; j < _sizeRow; j++)
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
            int[] minValues = new int[_sizeRow];
            int cell;
            for (int i = 0; i < _sizeRow; i++)
            {
                for (int j = 0; j < _sizeRow; j++)
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

        private static void SetMatrixDistance(Point[] checkpoints)
        {
            _matrixDistance = new int[_sizeRow, _sizeRow];
            int indexI = 0;
            int indexJ = 0;
            for (int i = 0; i < _sizeRow; i++)
            {
                for (int j = 0; j < _sizeRow; j++)
                {
                    _matrixDistance[i, j] = (i != j) ? GetSize(checkpoints[i], checkpoints[j]) : -1;
                    ++indexJ;
                }
                indexJ = 0;
                ++indexI;
            }
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