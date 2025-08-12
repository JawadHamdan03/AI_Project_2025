using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;

namespace DronePlanner.Models;

internal class DataLoader
{
    public static void LoadData(string filePath, out double[][] inputs, out int[] labels)
    {
        var inputList = new List<double[]>();
        var labelList = new List<int>();

        using (var workbook = new XLWorkbook(filePath))
        {
            var worksheet = workbook.Worksheet(1); // First sheet
            var rowCount = worksheet.LastRowUsed().RowNumber();

            for (int row = 2; row <= rowCount; row++) // skip header
            {
                double temp = worksheet.Cell(row, 1).GetDouble();
                double humidity = worksheet.Cell(row, 2).GetDouble();
                double wind = worksheet.Cell(row, 3).GetDouble();
                int safeToFly = (int)worksheet.Cell(row, 4).GetDouble();

                inputList.Add(new double[] { temp, humidity, wind });
                labelList.Add(safeToFly);
            }
        }

        inputs = inputList.ToArray();
        labels = labelList.ToArray();
    }
}