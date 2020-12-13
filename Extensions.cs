using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using MathNet.Numerics.LinearAlgebra;

namespace HopfieldNetwork
{
    static public class Extensions
    {
        public static void Randomize(this Matrix<double> m)
        {
            var rand = new Random();

            for (int i = 0; i < m.RowCount; i++)
            {
                for (int j = 0; j < m.ColumnCount; j++)
                {
                    m[i, j] = Math.Clamp(m.At(i, j) + rand.Next(-1, 1), -1, 1);
                }
            }
        }
        public static void Randomize(this Matrix<double>[] m)
        {
            var rand = new Random();

            foreach (var matrix in m)
            {
                for (int i = 0; i < matrix.RowCount; i++)
                {
                    for (int j = 0; j < matrix.ColumnCount; j++)
                    {
                        matrix[i, j] = Math.Clamp(matrix.At(i, j) + rand.Next(-1, 1), -1, 1);
                    }
                }
            }
        }

        public static void Activate(this Matrix<double> m)
        {
            m.Map(elem => elem > 0 ? 1 : -1, m);
        }

        public static bool Compare(this Matrix<double> matrix, Matrix<double> other)
        {
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    if (matrix[i, j] != other[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void ToImage(this Matrix<double> m, string filePath)
        {
            var size = (int)Math.Sqrt(m.ColumnCount * m.RowCount);
            Bitmap image = new Bitmap(size, size);
            m = Matrix<double>.Build.DenseOfRowMajor(size, size, m.ToRowMajorArray());

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var val = 255 - (int)m.At(i, j) * 255;
                    val = Math.Clamp(val, 0, 255);
                    Color col = Color.FromArgb(val, val, val);
                    image.SetPixel(j, i, col);
                }
            }
            image.Save($"{filePath}.bmp", ImageFormat.Bmp);
        }

        public static void ToImage(this Matrix<double>[] m, string filePath)
        {
            var size = (int)Math.Sqrt(m[0].ColumnCount * m[0].RowCount);
            var index = 0;

            foreach (var matrix in m)
            {
                Bitmap image = new Bitmap(size, size);
                var matrix1 = Matrix<double>.Build.DenseOfRowMajor(size, size, matrix.ToRowMajorArray());
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        var val = 255 - (int)matrix1.At(i, j) * 255;
                        val = Math.Clamp(val, 0, 255);
                        Color col = Color.FromArgb(val, val, val);
                        image.SetPixel(j, i, col);
                    }
                }
                image.Save($"{filePath}_{index++}.bmp", ImageFormat.Bmp);
            }
        }

        public static Matrix<double> Join(this Matrix<double>[] matrices)
        {
            var m = matrices[0];
            var vecBuilder = Vector<double>.Build;
            for (int i = 1; i < matrices.Length; i++)
            {
                m = m.InsertColumn(m.ColumnCount, vecBuilder.DenseOfArray(matrices[i].ToColumnMajorArray()));
            }

            return m;
        }

        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }

        public static double Sqrt(this int value)
        {
            return Math.Sqrt(value);
        }
    }

}
