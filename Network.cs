using System;
using MathNet.Numerics.LinearAlgebra;

namespace HopfieldNetwork
{
    class Network
    {
        private Matrix<double> Weights { get; set; }

        public Network(Matrix<double> input)
        {
            var matrixBuilder = Matrix<double>.Build;
            Weights = matrixBuilder.Dense(input.RowCount, input.RowCount, 0);

            Train(input);
        }

        public void Train(Matrix<double> input)
        {
            Matrix<double> oldWeights;

            // projections method
            // oldWeights = Weights.Map(f => f);
            // var t1 = Weights * input - input;
            // var t2 = t1.Transpose();
            // var coeff = 1f / (input.Transpose() * input - input.Transpose() * Weights * input)[0, 0];
            // Weights += coeff * t1 * t2;
            DeltaProjections(input);

            // do
            // {
            //     var h = 0.8;
            //     oldWeights = Weights.Map(f => f);
            //     for (int i = 0; i < input.ColumnCount; i++)
            //     {
            //         var x = input.Column(i).ToColumnMatrix();
            //         var delta = h / input.RowCount.Sqrt() * (x - oldWeights * x) * x.Transpose();
            //         Weights += delta;
            //     }

            // } while ((oldWeights - Weights).RowSums().Sum().Abs() > float.Epsilon);

            for (int i = 0; i < Weights.ColumnCount; i++)
            {
                Weights[i, i] = 0;
            }

            Console.WriteLine($"Weight matrix is:\n{Weights}");
        }

        private void DeltaProjections(Matrix<double> input)
        {
            double e = 1E-8;
            double change = 0;
            double h = 0.8;
            double delta = 0;
            double previousDelta = 0;

            do
            {
                delta = 0;
                for (int i = 0; i < input.ColumnCount; i++)
                {
                    var vec = input.Column(i).ToColumnMatrix();
                    var deltaW = (vec - Weights * vec) * vec.Transpose() * (h / input.RowCount.Sqrt());
                    Weights += deltaW;
                    delta += deltaW.RowAbsoluteSums().Sum();
                }
                change = Math.Abs(previousDelta - delta);
                previousDelta = delta;
                Console.WriteLine($"Change is: {change}");
            } while (change > e);
        }

        public void Process(Matrix<double> input)
        {
            bool isDenoised = false;
            while (!isDenoised)
            {
                isDenoised = true;
                var row = input.Transpose();
                var y = row * Weights;
                y.Activate();
                input.SetColumn(0, y.ToColumnMajorArray());
                isDenoised &= row.Compare(y);
            }

            input.ToImage("denoised");
            Console.ReadKey();
        }
    }
}