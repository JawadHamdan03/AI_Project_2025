using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;

namespace DronePlanner.Models;

internal class DataLoader
{
    public static void LoadData(string filePath, out double[][] inputs, out int[] labels, string? sheetName = null)
    {
        var inputList = new List<double[]>();
        var labelList = new List<int>();

        using var wb = new XLWorkbook(filePath);
        var ws = sheetName is null ? wb.Worksheet(1) : wb.Worksheet(sheetName);

        
        var lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;
        if (lastRow < 2)
            throw new InvalidDataException("Worksheet appears to be empty or missing a header row.");

        for (int row = 2; row <= lastRow; row++)
        {
            
            if (ws.Row(row).IsEmpty()) continue;

            
            double temp = ws.Cell(row, 1).GetDouble();
            double humidity = ws.Cell(row, 2).GetDouble();
            double wind = ws.Cell(row, 3).GetDouble();

            // Label: 0 = Safe, 1 = Unsafe 
            var labelCell = ws.Cell(row, 4);
            if (labelCell.IsEmpty()) continue; 
            int safeToFly = (int)labelCell.GetDouble();

            inputList.Add(new[] { temp, humidity, wind });
            labelList.Add(safeToFly);
        }

        if (inputList.Count == 0)
            throw new InvalidDataException("No data rows were read. Check sheet name/columns.");

        inputs = inputList.ToArray();
        labels = labelList.ToArray();
    }
}