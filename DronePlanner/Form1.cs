using DronePlanner.Models;
using System.Data;

namespace DronePlanner;

public partial class Form1 : Form
{

    private double[][]? _inputs;
    private int[]? _labels;
    private Perceptron? _model;
    private double[][] _xTrain;
    private int[] _yTrain;
    private double[][] _xTest;
    private int[] _yTest;
    private int[] _testIdx;


    private List<City> _cities = new List<City>();
    private double[,]? _costMatrix;
    private const int MapPadding = 30;


    public Form1()
    {
        InitializeComponent();
    }
    //---------------------------part1-----------------------------------------
    private void btnLoadExcel_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
            Title = "Select weather_data_linearly_separable.xlsx"
        };

        if (ofd.ShowDialog() != DialogResult.OK) return;

        try
        {
            DataLoader.LoadData(ofd.FileName, out _inputs!, out _labels!);
            lblStatus.Text = $"Loaded {_inputs.Length} rows from {Path.GetFileName(ofd.FileName)}";
            btnTrain.Enabled = _inputs.Length > 0;   // enable Train now that we have data
            btnPredict.Enabled = false;              // require training first
            lblAccuracy.Text = "Accuracy: —";
            dataGridViewResults.DataSource = null;   // clear any old table
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load Excel:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = "Failed to load Excel.";
        }
    }

    private async void btnTrain_Click(object sender, EventArgs e)
    {
        if (_inputs == null || _labels == null || _inputs.Length == 0)
        {
            lblStatus.Text = "Load data first.";
            return;
        }

        if (!double.TryParse(txtLearningRate.Text, out double lr) || lr <= 0)
        {
            MessageBox.Show("Enter a valid learning rate > 0", "Invalid Input",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        int epochs = (int)numEpochs.Value;
        if (epochs <= 0)
        {
            MessageBox.Show("Epochs must be >= 1", "Invalid Input",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 80/20 split with reproducible shuffle
        int n = _inputs.Length;
        var idx = Enumerable.Range(0, n).ToArray();
        var rng = new Random(42);
        for (int i = idx.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (idx[i], idx[j]) = (idx[j], idx[i]);
        }

        int nTrain = (int)Math.Round(n * 0.8);
        int nTest = n - nTrain;

        (_xTrain, _yTrain) = BuildSubset(_inputs, _labels, idx, 0, nTrain);
        (_xTest, _yTest) = BuildSubset(_inputs, _labels, idx, nTrain, nTest);
        _testIdx = idx.Skip(nTrain).Take(nTest).ToArray();

        // Random-init perceptron (constructor does random init if no weights passed)
        _model = new Perceptron(_xTrain[0].Length, lr);

        // Train on 80%
        _model.Train(_xTrain, _yTrain, epochs);

        lblStatus.Text = $"Trained on {nTrain} samples. Ready to predict on {nTest}.";
        btnPredict.Enabled = true;
    }

    private void btnPredict_Click(object sender, EventArgs e)
    {
        if (_model == null || _xTest == null || _yTest == null || _testIdx == null)
        {
            lblStatus.Text = "Train the model first.";
            return;
        }

        var table = new DataTable();
        table.Columns.Add("Row", typeof(int));          
        table.Columns.Add("Temperature", typeof(double));
        table.Columns.Add("Humidity", typeof(double));
        table.Columns.Add("Wind", typeof(double));
        table.Columns.Add("Actual", typeof(int));
        table.Columns.Add("Predicted", typeof(int));
        table.Columns.Add("Correct", typeof(string));   

        int tp = 0, tn = 0, fp = 0, fn = 0;

        for (int i = 0; i < _xTest.Length; i++)
        {
            int pred = _model.Predict(_xTest[i]);
            int actual = _yTest[i];

            if (pred == 1 && actual == 1) tp++;
            else if (pred == 0 && actual == 0) tn++;
            else if (pred == 1 && actual == 0) fp++;
            else if (pred == 0 && actual == 1) fn++;

            string correct = (pred == actual) ? "✓" : "✗";
            int originalRow = _testIdx[i] + 2; 

            table.Rows.Add(originalRow, _xTest[i][0], _xTest[i][1], _xTest[i][2], actual, pred, correct);
        }

        dataGridViewResults.DataSource = table;
        dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        
        dataGridViewResults.CellFormatting -= dataGridViewResults_CellFormatting;
        dataGridViewResults.CellFormatting += dataGridViewResults_CellFormatting;

        double acc = (_xTest.Length > 0) ? (double)(tp + tn) / _xTest.Length : 0.0;
        double precision = (tp + fp) > 0 ? (double)tp / (tp + fp) : 0.0; 
        double recall = (tp + fn) > 0 ? (double)tp / (tp + fn) : 0.0;

        lblAccuracy.Text = $"Test (20%) Accuracy: {acc:P2}";
        //lblStatus.Text = $"Precision: {precision:P2}, Recall: {recall:P2}";
    }

    private void dataGridViewResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        var grid = (DataGridView)sender;

        if (grid.Columns[e.ColumnIndex].Name == "Predicted" && e.Value is int pv)
        {
            e.CellStyle.BackColor = (pv == 0)
                ? System.Drawing.Color.FromArgb(220, 247, 220)   // green
                : System.Drawing.Color.FromArgb(255, 225, 225);  // red
            e.CellStyle.ForeColor = System.Drawing.Color.Black;
        }

        if (grid.Columns[e.ColumnIndex].Name == "Correct" && e.Value is string s)
        {
            if (s == "✓")
                e.CellStyle.BackColor = System.Drawing.Color.FromArgb(220, 247, 220);
            else if (s == "✗")
                e.CellStyle.BackColor = System.Drawing.Color.FromArgb(255, 225, 225);
            e.CellStyle.ForeColor = System.Drawing.Color.Black;
        }
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        _inputs = null;
        _labels = null;
        _model = null;

        dataGridViewResults.DataSource = null;
        lblStatus.Text = "Cleared.";
        lblAccuracy.Text = "Accuracy: —";


        txtLearningRate.Text = "0.1";
        numEpochs.Value = 1;


        btnTrain.Enabled = false;
        btnPredict.Enabled = false;
    }


    private static (double[][] X, int[] y) BuildSubset( double[][] X, int[] y, int[] idx, int start, int count)
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

    //----------------------------part2-----------------------------------------
    private void btnAddCity_Click(object sender, EventArgs e)
    {
        if (_model == null)
        {
            MessageBox.Show("Train the perceptron first.", "Model Not Ready",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        
        double x = (double)numX.Value;
        double y = (double)numY.Value;
        double temp = (double)numTemp.Value;
        double humidity = (double)numHumidity.Value;
        double wind = (double)numWind.Value;

        
        int prediction = _model.Predict(new double[] { temp, humidity, wind });

        
        var city = new City
        {
            Id = _cities.Count + 1,
            X = x,
            Y = y,
            Temp = temp,
            Humidity = humidity,
            Wind = wind,
            SafeToFly = prediction
        };
        _cities.Add(city);

        
        var table = new DataTable();
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("X", typeof(double));
        table.Columns.Add("Y", typeof(double));
        table.Columns.Add("Temp", typeof(double));
        table.Columns.Add("Humidity", typeof(double));
        table.Columns.Add("Wind", typeof(double));
        table.Columns.Add("SafeToFly", typeof(int));

        foreach (var c in _cities)
            table.Rows.Add(c.Id, c.X, c.Y, c.Temp, c.Humidity, c.Wind, c.SafeToFly);

        dataGridViewResults.DataSource = table;

        
        dataGridViewResults.CellFormatting -= CityCellFormatting;
        dataGridViewResults.CellFormatting += CityCellFormatting;
        pictureMap.Invalidate();

        numX.ResetText();
        numY.ResetText();
        numTemp.ResetText();
        numHumidity.ResetText();
        numWind.ResetText();
    }

    private void CityCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dataGridViewResults.Columns[e.ColumnIndex].Name == "SafeToFly" && e.Value is int v)
        {
            if (v == 0)
                e.CellStyle.BackColor = Color.FromArgb(220, 247, 220); // safe
            else
                e.CellStyle.BackColor = Color.FromArgb(255, 225, 225); // unsafe

            e.CellStyle.ForeColor = Color.Black;
        }
    }

    private void pictureMap_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        var rect = pictureMap.ClientRectangle;
        using (var borderPen = new Pen(Color.LightGray, 1))
            g.DrawRectangle(borderPen, rect.Left + 1, rect.Top + 1, rect.Width - 2, rect.Height - 2);

        if (_cities == null || _cities.Count == 0)
        {
            using var hintBrush = new SolidBrush(Color.Gray);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString("Add cities to see them here", Font, hintBrush, rect, sf);
            return;
        }

       
        double minX = _cities.Min(c => c.X);
        double maxX = _cities.Max(c => c.X);
        double minY = _cities.Min(c => c.Y);
        double maxY = _cities.Max(c => c.Y);

        
        double dx = Math.Max(1, (maxX - minX) * 0.05);
        double dy = Math.Max(1, (maxY - minY) * 0.05);
        minX -= dx; maxX += dx; minY -= dy; maxY += dy;

        
        if (minX > 0) minX = 0;
        if (maxX < 0) maxX = 0;
        if (minY > 0) minY = 0;
        if (maxY < 0) maxY = 0;

        const int pad = 40;
        int left = rect.Left + pad, right = rect.Right - pad;
        int top = rect.Top + pad, bottom = rect.Bottom - pad;

        float width = Math.Max(1, right - left);
        float height = Math.Max(1, bottom - top);

        PointF Map(double x, double y)
        {
            double sx = (x - minX) / (maxX - minX);
            double sy = (y - minY) / (maxY - minY);
            float px = left + (float)(sx * width);
            float py = bottom - (float)(sy * height); 
            return new PointF(px, py);
        }

        using var axisPen = new Pen(Color.Black, 1);

       
        if (minY <= 0 && maxY >= 0)
        {
            var y0 = Map(0, 0).Y;
            g.DrawLine(axisPen, left, y0, right, y0);
        }

        
        if (minX <= 0 && maxX >= 0)
        {
            var x0 = Map(0, 0).X;
            g.DrawLine(axisPen, x0, top, x0, bottom);
        }

        using var labelBrush = new SolidBrush(Color.Black);

       
        int xTicks = 5;
        for (int i = 0; i <= xTicks; i++)
        {
            double val = minX + i * (maxX - minX) / xTicks;
            var p = Map(val, 0);
            g.DrawLine(Pens.Black, p.X, p.Y - 3, p.X, p.Y + 3);
            var text = val.ToString("0.0");
            var size = g.MeasureString(text, Font);
            g.DrawString(text, Font, labelBrush, p.X - size.Width / 2, p.Y + 5);
        }

        
        int yTicks = 5;
        for (int i = 0; i <= yTicks; i++)
        {
            double val = minY + i * (maxY - minY) / yTicks;
            var p = Map(0, val);
            g.DrawLine(Pens.Black, p.X - 3, p.Y, p.X + 3, p.Y);
            var text = val.ToString("0.0");
            var size = g.MeasureString(text, Font);
            g.DrawString(text, Font, labelBrush, p.X - size.Width - 5, p.Y - size.Height / 2);
        }

        
        using var safeBrush = new SolidBrush(Color.FromArgb(60, 180, 75));
        using var unsafeBrush = new SolidBrush(Color.FromArgb(240, 80, 80));
        using var textBrush = new SolidBrush(Color.Black);
        const int R = 6;

        foreach (var c in _cities)
        {
            var p = Map(c.X, c.Y);
            var brush = (c.SafeToFly == 0) ? safeBrush : unsafeBrush;

            g.FillEllipse(brush, p.X - R, p.Y - R, R * 2, R * 2);
            g.DrawEllipse(Pens.Black, p.X - R, p.Y - R, R * 2, R * 2);
            g.DrawString($"C{c.Id}", this.Font, textBrush, p.X + R + 3, p.Y - (R + 3));
        }
    }

    //----------------------------part3-------------------------------------------
}
