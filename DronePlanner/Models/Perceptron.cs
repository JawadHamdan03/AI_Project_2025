using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DronePlanner.Models;

internal class Perceptron
{
    public double[] Weights { get; set; }
    public double theta { get; set; }
    private readonly double alpha;
    private static  Random rng = new Random(); 

    public Perceptron(int inputSize, double learningRate = 0.1, double[]? initialWeights = null, double? initialBias = null)
    {
        this.alpha = learningRate;
        Weights = new double[inputSize];

        if (initialWeights != null && initialWeights.Length == inputSize)
        {
            Array.Copy(initialWeights, Weights, inputSize); 
        }
        else
        {
            
            for (int i = 0; i < inputSize; i++)
                Weights[i] = rng.NextDouble() - 0.5;
        }

        if (initialBias.HasValue)
        {
            theta = initialBias.Value;
        }
        else
        {
            
            theta = rng.NextDouble() - 0.5;
        }
    }

    public int Predict(double[] inputs)
    {
        double sum = theta;
        for (int i = 0; i < Weights.Length; i++)
            sum += Weights[i] * inputs[i];
        return sum >= 0 ? 1 : 0;
    }

    public void Train(double[][] trainingInputs, int[] labels, int epochs)
    {
        Console.WriteLine("Training Weights :");
        for (int e = 0; e < epochs; e++)
        {
            Console.WriteLine($"\nEpoch :{e}");
            for (int i = 0; i < trainingInputs.Length; i++)
            {
                Console.WriteLine($"Inputs: Temp={trainingInputs[i][0]} , Humidity={trainingInputs[i][1]} , WindSpeed={trainingInputs[i][2]}. Weights: W1={Weights[0]}, W2={Weights[1]}, W3={Weights[2]}");
                int prediction = Predict(trainingInputs[i]);
                int error = labels[i] - prediction;

                for (int j = 0; j < Weights.Length; j++)
                    Weights[j] += alpha * error * trainingInputs[i][j];


                

                theta += alpha * error;
            }
        }
    }
}
