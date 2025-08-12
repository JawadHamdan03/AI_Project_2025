using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DronePlanner.Models;

internal class Perceptron
{
    private double[] weights;
    private double bias;
    private readonly double learningRate;
    private static readonly Random rng = new Random(); 

    public Perceptron(int inputSize, double learningRate = 0.1, double[]? initialWeights = null, double? initialBias = null)
    {
        this.learningRate = learningRate;
        weights = new double[inputSize];

        if (initialWeights != null && initialWeights.Length == inputSize)
        {
            Array.Copy(initialWeights, weights, inputSize); 
        }
        else
        {
            
            for (int i = 0; i < inputSize; i++)
                weights[i] = rng.NextDouble() - 0.5;
        }

        if (initialBias.HasValue)
        {
            bias = initialBias.Value;
        }
        else
        {
            
            bias = rng.NextDouble() - 0.5;
        }
    }

    public int Predict(double[] inputs)
    {
        double sum = bias;
        for (int i = 0; i < weights.Length; i++)
            sum += weights[i] * inputs[i];
        return sum >= 0 ? 1 : 0;
    }

    public void Train(double[][] trainingInputs, int[] labels, int epochs)
    {
        for (int e = 0; e < epochs; e++)
        {
            for (int i = 0; i < trainingInputs.Length; i++)
            {
                int prediction = Predict(trainingInputs[i]);
                int error = labels[i] - prediction;

                for (int j = 0; j < weights.Length; j++)
                    weights[j] += learningRate * error * trainingInputs[i][j];

                bias += learningRate * error;
            }
        }
    }
}
