using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DronePlanner.Models;

internal class Perceptron
{
    private double[] weights { get; set; }
    private double bias { get; set; }
    private double learningRate { get; set; }

    public Perceptron(int inputSize, double learningRate = 0.1)
    {
        this.learningRate = learningRate;
        weights = new double[inputSize];
        bias = 0;
    }

    public int Predict(double[] inputs)
    {
        double sum = bias;
        for (int i = 0; i < weights.Length; i++)
        {
            sum += weights[i] * inputs[i];
        }
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

                //update weights and bias
                for (int j = 0; j < weights.Length; j++)
                {
                    weights[j] += learningRate * error * trainingInputs[i][j];
                }
                bias += learningRate * error;
            }
        }
    }
}
