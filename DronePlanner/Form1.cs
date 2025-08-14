using DocumentFormat.OpenXml.EMMA;
using DronePlanner.Models;
using System.Data;

namespace DronePlanner;

public partial class Form1 : Form
{

    private double[][]? Inputs;
    private int[]? Labels;
    private Perceptron? Classifier;
    private double[][] _xTrain;
    private int[] _yTrain;
    private double[][] _xTest;
    private int[] _yTest;
    private int[] _testIdx;


    private List<City> _cities = new List<City>();
    private double[,]? _costMatrix;
    private const int MapPadding = 30;


    private int[]? _bestRoute;
    private double _bestCost;
    private const double DEFAULT_UNSAFE_PENALTY = 50.0;
    private readonly Pen _routePen = new(Color.RoyalBlue, 2f);

    private readonly Random _rng = new Random();

    private int[]? _route;     // current route (random first)
    private double _routeCost; // cost of current route

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
            DataLoader.LoadData(ofd.FileName, out Inputs!, out Labels!);
            lblStatus.Text = $"Loaded {Inputs.Length} rows from {Path.GetFileName(ofd.FileName)}";
            btnTrain.Enabled = Inputs.Length > 0;
            btnPredict.Enabled = false;
            lblAccuracy.Text = "Accuracy: —";
            dataGridViewResults.DataSource = null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load Excel:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = "Failed to load Excel.";
        }
    }

    private void btnTrain_Click(object sender, EventArgs e)
    {
        if (Inputs == null || Labels == null || Inputs.Length == 0)
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
        int n = Inputs.Length;
        var idx = Enumerable.Range(0, n).ToArray();
        var rng = new Random(42);
        for (int i = idx.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (idx[i], idx[j]) = (idx[j], idx[i]);
        }

        int nTrain = (int)Math.Round(n * 0.8);
        int nTest = n - nTrain;

        (_xTrain, _yTrain) = Services.BuildSubset(Inputs, Labels, idx, 0, nTrain);
        (_xTest, _yTest) = Services.BuildSubset(Inputs, Labels, idx, nTrain, nTest);
        _testIdx = idx.Skip(nTrain).Take(nTest).ToArray();

        // Random-init perceptron (constructor does random init if no weights passed)
        Classifier = new Perceptron(_xTrain[0].Length, lr);

        // Train on 80%
        Classifier.Train(_xTrain, _yTrain, epochs);

        Console.WriteLine($"after training : \n w1={Classifier.Weights[0]},  w2={Classifier.Weights[1]}, w3={Classifier.Weights[2]}");


        lblStatus.Text = $"Trained on {nTrain} samples. Ready to predict on {nTest}.";
        btnPredict.Enabled = true;
    }

    private void btnPredict_Click(object sender, EventArgs e)
    {
        if (Classifier == null || _xTest == null || _yTest == null || _testIdx == null)
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
            int pred = Classifier.Predict(_xTest[i]);
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
        Inputs = null;
        Labels = null;
        Classifier = null;

        dataGridViewResults.DataSource = null;
        lblStatus.Text = "Cleared.";
        lblAccuracy.Text = "Accuracy: —";


        txtLearningRate.Text = "0.1";
        numEpochs.Value = 1;


        btnTrain.Enabled = false;
        btnPredict.Enabled = false;
    }

    //----------------------------part2-----------------------------------------
    private void btnAddCity_Click(object sender, EventArgs e)
    {
        if (Classifier == null)
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


        int prediction = Classifier.Predict(new double[] { temp, humidity, wind });


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

    //----------------------------part3-------------------------------------------
    private void btnCostMatrix_Click(object sender, EventArgs e)
    {
        if (_cities.Count < 2)
        {
            MessageBox.Show("Add at least 2 cities first.", "Not enough cities",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        const double PENALTY = 50.0;      
        bool returnToStart = true;        

        _costMatrix = Services.BuildCostMatrix(_cities, PENALTY);

        int n = _cities.Count;
        _route = Enumerable.Range(0, n).ToArray();
        Services.Shuffle(_route);                   

        _routeCost = Services.RouteCost(_route, _costMatrix, returnToStart);
        lblAccuracy.Text = $"Random route cost: {_routeCost:F2}";
        lblStatus.Text = "Random route generated.";
        pictureMap.Invalidate();           
    }

    //-----------------part 4---------------------------
    private void btnOptimizeRoute_Click(object sender, EventArgs e)
    {
        if (_costMatrix == null || _route == null || _route.Length < 2)
        {
            MessageBox.Show("Build the cost matrix first to create a random route.",
                            "No route yet", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        double before = Services.RouteCost(_route, _costMatrix, returnToStart: true);

        var (bestRoute, bestCost) = SimulatedAnnealing(
            _costMatrix,
            _route,                
            iterMax: 12_000,       
            t0: 800.0,             
            alpha: 0.995,          
            returnToStart: true,
            seed: null             
        );

        _route = bestRoute;        
        _routeCost = bestCost;

        double improvement = (before > 0) ? (before - bestCost) / before * 100.0 : 0.0;
        lblAccuracy.Text = $"Before: {before:F2}  →  After: {bestCost:F2}  ({improvement:F1}% better)";
        lblStatus.Text = "Simulated annealing complete.";
        pictureMap.Invalidate();
    }

    private static double CalcTemp(int i, double t0, double alpha) => t0 * Math.Pow(alpha, i);

    private (int[] route, double cost) SimulatedAnnealing(
        double[,] costMatrix,
        int[] initialRoute,     
        int iterMax = 10_000,   
        double t0 = 1000.0,     
        double alpha = 0.995,   
        bool returnToStart = true,
        int? seed = null)
    {
        var rng = seed.HasValue ? new Random(seed.Value) : new Random();

        var x_curr = (int[])initialRoute.Clone();
        double f_curr = Services.RouteCost(x_curr, costMatrix, returnToStart);

        var x_best = (int[])x_curr.Clone();
        double f_best = f_curr;

        for (int i = 1; i <= iterMax; i++)
        {
            double Tc = CalcTemp(i, t0, alpha);
            if (Tc <= 1e-12) break; 

            var x_next = (int[])x_curr.Clone();
            int a = rng.Next(x_next.Length);
            int b = rng.Next(x_next.Length);
            if (a != b) (x_next[a], x_next[b]) = (x_next[b], x_next[a]);

            double f_next = Services.RouteCost(x_next, costMatrix, returnToStart);
            double delta = f_curr - f_next;  

            if (delta > 0)
            {
                x_curr = x_next;
                f_curr = f_next;

                if (f_next < f_best)
                {
                    x_best = (int[])x_next.Clone();
                    f_best = f_next;
                }
            }
            else
            {
                double acceptProb = Math.Exp(delta / Tc); 
                if (rng.NextDouble() < acceptProb)
                {
                    x_curr = x_next;
                    f_curr = f_next;
                }
            }
        }

        return (x_best, f_best);
    }


    //----------------maping paint------------
    private void pictureMap_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        var rect = pictureMap.ClientRectangle;
        using (var borderPen = new Pen(Color.LightGray, 1))
            g.DrawRectangle(borderPen, rect.Left + 1, rect.Top + 1, rect.Width - 2, rect.Height - 2);

        // empty state
        if (_cities == null || _cities.Count == 0)
        {
            using var hintBrush = new SolidBrush(Color.Gray);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString("Add cities to see them here", Font, hintBrush, rect, sf);
            return;
        }

        // ----- bounds (+margin) -----
        double minX = _cities.Min(c => c.X);
        double maxX = _cities.Max(c => c.X);
        double minY = _cities.Min(c => c.Y);
        double maxY = _cities.Max(c => c.Y);

        double dx = Math.Max(1, (maxX - minX) * 0.05);
        double dy = Math.Max(1, (maxY - minY) * 0.05);
        minX -= dx; maxX += dx; minY -= dy; maxY += dy;

        // include (0,0) so axes can show
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
            double rx = (maxX - minX);
            double ry = (maxY - minY);
            double sx = rx == 0 ? 0.5 : (x - minX) / rx;
            double sy = ry == 0 ? 0.5 : (y - minY) / ry;
            float px = left + (float)(sx * width);
            float py = bottom - (float)(sy * height); // invert Y
            return new PointF(px, py);
        }

        // ----- axes -----
        using (var axisPen = new Pen(Color.Black, 1))
        {
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
        }

        // ----- ticks & labels -----
        using (var labelBrush = new SolidBrush(Color.Black))
        {
            int xTicks = 5;
            double tickY = (minY <= 0 && maxY >= 0) ? 0 : minY;
            for (int i = 0; i <= xTicks; i++)
            {
                double val = minX + i * (maxX - minX) / xTicks;
                var p = Map(val, tickY);
                g.DrawLine(Pens.Black, p.X, p.Y - 3, p.X, p.Y + 3);
                var text = val.ToString("0.0");
                var size = g.MeasureString(text, Font);
                g.DrawString(text, Font, labelBrush, p.X - size.Width / 2, p.Y + 5);
            }

            int yTicks = 5;
            double tickX = (minX <= 0 && maxX >= 0) ? 0 : minX;
            for (int i = 0; i <= yTicks; i++)
            {
                double val = minY + i * (maxY - minY) / yTicks;
                var p = Map(tickX, val);
                g.DrawLine(Pens.Black, p.X - 3, p.Y, p.X + 3, p.Y);
                var text = val.ToString("0.0");
                var size = g.MeasureString(text, Font);
                g.DrawString(text, Font, labelBrush, p.X - size.Width - 5, p.Y - size.Height / 2);
            }
        }

        // ----- draw city dots FIRST (no text yet) -----
        using var safeBrush = new SolidBrush(Color.FromArgb(60, 180, 75));
        using var unsafeBrush = new SolidBrush(Color.FromArgb(240, 80, 80));
        const int R = 6;

        foreach (var c in _cities)
        {
            var p = Map(c.X, c.Y);
            var brush = (c.SafeToFly == 0) ? safeBrush : unsafeBrush;
            g.FillEllipse(brush, p.X - R, p.Y - R, R * 2, R * 2);
            g.DrawEllipse(Pens.Black, p.X - R, p.Y - R, R * 2, R * 2);
        }

        // ===== draw CURRENT route (_route) so numbers/labels can be on top =====
        if (_route != null && _route.Length >= 2)
        {
            using var routePen = new Pen(Color.RoyalBlue, 2f);
            PointF Pt(int idx) => Map(_cities[idx].X, _cities[idx].Y);

            for (int k = 0; k < _route.Length - 1; k++)
            {
                var a = Pt(_route[k]);
                var b = Pt(_route[k + 1]);
                g.DrawLine(routePen, a, b);
            }
            // close the loop
            var first = Pt(_route[0]);
            var last = Pt(_route[^1]);
            g.DrawLine(routePen, last, first);

            // ----- step numbers ON TOP of route -----
            using var stepFont = new Font(Font.FontFamily, Math.Max(8f, Font.Size - 1f), FontStyle.Bold);
            using var stepBg = new SolidBrush(Color.FromArgb(230, Color.White));
            using var stepFg = new SolidBrush(Color.RoyalBlue);

            for (int step = 0; step < _route.Length; step++)
            {
                var p = Pt(_route[step]);
                var label = (step + 1).ToString();
                var size = g.MeasureString(label, stepFont);

                // small offset above node center
                var rectLbl = new RectangleF(p.X - size.Width / 2, p.Y - size.Height - 14, size.Width + 4, size.Height);
                g.FillRectangle(stepBg, rectLbl);
                g.DrawString(label, stepFont, stepFg, rectLbl.X + 2, rectLbl.Y);
            }
        }

        // ----- city labels LAST so they’re always readable -----
        using var textBrush = new SolidBrush(Color.Black);
        foreach (var c in _cities)
        {
            var p = Map(c.X, c.Y);
            g.DrawString($"C{c.Id}", this.Font, textBrush, p.X + R + 3, p.Y - (R + 3));
        }
    }

    private void btnAddRandomCity_Click(object sender, EventArgs e)
    {
        AddRandomCity();
    }
    private void AddRandomCity()
    {
        double minX = -50, maxX = 50;
        double minY = -50, maxY = 50;
        double minTemp = 0, maxTemp = 40;
        double minHum = 10, maxHum = 90;
        double minWind = 0, maxWind = 40;

        double Rand(double a, double b) => a + _rng.NextDouble() * (b - a);

        var city = new City
        {
            Id = _cities.Count + 1,
            X = Rand(minX, maxX),
            Y = Rand(minY, maxY),
            Temp = Rand(minTemp, maxTemp),
            Humidity = Rand(minHum, maxHum),
            Wind = Rand(minWind, maxWind),
            SafeToFly = 0 
        };

        if (Classifier != null)
            city.SafeToFly = Classifier.Predict(new[] { city.Temp, city.Humidity, city.Wind });

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

        lblStatus.Text = (Classifier != null)
            ? "Random city added (classified by Perceptron)."
            : "Random city added (train the Perceptron to classify).";

        pictureMap.Invalidate();
    }

}
