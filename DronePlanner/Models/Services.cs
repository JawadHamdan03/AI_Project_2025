using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DronePlanner.Models;

public class Services
{
    internal static (double[][] X, int[] y) BuildSubset(double[][] X, int[] y, int[] idx, int start, int count)
    {
        var subX = new double[count][];
        var subY = new int[count];
        for (int k = 0; k < count; k++)
        {
            int src = idx[start + k];
            var row = X[src];
            var copy = new double[row.Length];
            Array.Copy(row, copy, row.Length);
            subX[k] = copy;
            subY[k] = y[src];
        }
        return (subX, subY);
    }


    internal static void ShowCostMatrixPreview(double[,] m, DataGridView dataGridViewResults)
    {
        int n = m.GetLength(0);
        var t = new DataTable();

        t.Columns.Add("From\\To", typeof(string));
        for (int j = 0; j < n; j++)
            t.Columns.Add($"C{j + 1}", typeof(double));

        for (int i = 0; i < n; i++)
        {
            var row = t.NewRow();
            row[0] = $"C{i + 1}";
            for (int j = 0; j < n; j++)
                row[j + 1] = Math.Round(m[i, j], 2);
            t.Rows.Add(row);
        }

        dataGridViewResults.DataSource = t;
        dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }


    internal static double Distance(double x1, double y1, double x2, double y2)
    {
        double dx = x1 - x2, dy = y1 - y2;
        return Math.Sqrt(dx * dx + dy * dy);
    }


    internal static double[,] BuildCostMatrix(List<City> cities, double penalty)
    {
        int n = cities.Count;
        var m = new double[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                {
                    m[i, j] = 0;
                    continue;
                }

                double d = Distance(cities[i].X, cities[i].Y, cities[j].X, cities[j].Y);
                double p = (cities[j].SafeToFly == 1) ? penalty : 0.0;
                m[i, j] = d + p;
            }
        }
        return m;
    }


    internal static void Shuffle(int[] a, Random rng)
    {
       
        for (int i = a.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (a[i], a[j]) = (a[j], a[i]);
        }
    }

    internal static double RouteCost(int[] route, double[,] cost, bool returnToStart = true)
    {
        double sum = 0.0;
        for (int k = 0; k < route.Length - 1; k++)
            sum += cost[route[k], route[k + 1]];
        if (returnToStart && route.Length > 1)
            sum += cost[route[^1], route[0]];
        return sum;
    }


}
