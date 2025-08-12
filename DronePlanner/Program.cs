using DronePlanner.Models;

namespace DronePlanner
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //DataLoader.LoadData("C:\\Users\\PC\\My Files\\Files\\AI\\Project\\weather_data_linearly_separable.xlsx", out var inputs, out var labels);

            //Perceptron p = new Perceptron(3, 0.01);
            //p.Train(inputs, labels, 100);

            //Console.WriteLine("Testing first 5 samples:");
            //for (int i = 0; i < 5; i++)
            //{
            //    int predicted = p.Predict(inputs[i]);
            //    Console.WriteLine($"Sample {i + 1} → Actual: {labels[i]}, Predicted: {predicted}");
            //}

            



            
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}