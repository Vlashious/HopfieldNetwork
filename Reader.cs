using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace HopfieldNetwork
{
    static class Reader
    {
        public static Matrix<double>[] ReadInput(params string[] inputFiles)
        {
            var matrixBuilder = Matrix<double>.Build;
            List<Matrix<double>> matrices = new();

            foreach (var filePath in inputFiles)
            {
                var rawInput = File.ReadAllLines(filePath);
                var inputAsNum = rawInput.Select(row => row.Split(" ").Select(stringNum => double.Parse(stringNum)).ToArray()).ToArray();
                var matrix = matrixBuilder.DenseOfRowArrays(inputAsNum);
                matrix.ToImage($"{filePath}");
                matrix = matrixBuilder.Dense(matrix.ColumnCount * matrix.RowCount, 1, matrix.ToRowMajorArray());
                matrices.Add(matrix);
            }

            return matrices.ToArray();
        }
    }
}