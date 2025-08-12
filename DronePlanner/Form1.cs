using DronePlanner.Models;
using System.Data;

namespace DronePlanner;

public partial class Form1 : Form
{

    private double[][]? _inputs;
    private int[]? _labels;
    private Perceptron? _model;
    private List<City> _cities = new();
    private double[,]? _costMatrix;
    private const int MapPadding = 30;
    public Form1()
    {
        InitializeComponent();
    }

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
            lblStatus.Text = "Please load the Excel file first.";
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

        try
        {
            UseWaitCursor = true;
            lblStatus.Text = "Training perceptron...";

            _model = new Perceptron(3, lr);

            // run training in background so UI stays responsive
            await Task.Run(() => _model.Train(_inputs, _labels, epochs));

            lblStatus.Text = "Training complete.";
            btnPredict.Enabled = true; // allow prediction
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Training failed:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = "Training failed.";
        }
        finally
        {
            UseWaitCursor = false;
        }
    }

    private void btnPredict_Click(object sender, EventArgs e)
    {
        if (_inputs == null || _labels == null || _inputs.Length == 0 || _model == null)
        {
            lblStatus.Text = "Load data and train first.";
            return;
        }

        var table = new DataTable();
        table.Columns.Add("Temperature", typeof(double));
        table.Columns.Add("Humidity", typeof(double));
        table.Columns.Add("Wind", typeof(double));
        table.Columns.Add("Actual", typeof(int));
        table.Columns.Add("Predicted", typeof(int));

        int correct = 0;

        for (int i = 0; i < _inputs.Length; i++)
        {
            int pred = _model.Predict(_inputs[i]);
            if (pred == _labels[i]) correct++;

            table.Rows.Add(_inputs[i][0], _inputs[i][1], _inputs[i][2], _labels[i], pred);
        }

        dataGridViewResults.DataSource = table;

        dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


        dataGridViewResults.CellFormatting -= dataGridViewResults_CellFormatting;
        dataGridViewResults.CellFormatting += dataGridViewResults_CellFormatting;

        double acc = (double)correct / _inputs.Length;
        lblAccuracy.Text = $"Accuracy: {acc:P2}  ({correct}/{_inputs.Length})";
        lblStatus.Text = "Prediction complete.";
    }

    private void dataGridViewResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dataGridViewResults.Columns[e.ColumnIndex].Name == "Predicted" && e.Value is int v)
        {

            if (v == 0)
                e.CellStyle.BackColor = Color.FromArgb(220, 247, 220); // light green
            else
                e.CellStyle.BackColor = Color.FromArgb(255, 225, 225); // light red

            e.CellStyle.ForeColor = Color.Black;
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


        txtLearningRate.Text = "0.01";
        numEpochs.Value = 100;


        btnTrain.Enabled = false;
        btnPredict.Enabled = false;
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

        // Gather input values
        double x = (double)numX.Value;
        double y = (double)numY.Value;
        double temp = (double)numTemp.Value;
        double humidity = (double)numHumidity.Value;
        double wind = (double)numWind.Value;

        // Predict Safe/Unsafe using the perceptron
        int prediction = _model.Predict(new double[] { temp, humidity, wind });

        // Add city to list
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

        // Refresh grid
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

        // Color unsafe rows red
        dataGridViewResults.CellFormatting -= CityCellFormatting;
        dataGridViewResults.CellFormatting += CityCellFormatting;
        pictureMap.Invalidate();
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

    //----------------drawing cities--------------
    private (double minX, double maxX, double minY, double maxY) GetWorldBounds()
    {
        if (_cities.Count == 0) return (0, 100, 0, 100); // default view

        double minX = _cities.Min(c => c.X);
        double maxX = _cities.Max(c => c.X);
        double minY = _cities.Min(c => c.Y);
        double maxY = _cities.Max(c => c.Y);

        // Add a little margin
        double dx = Math.Max(1, (maxX - minX) * 0.05);
        double dy = Math.Max(1, (maxY - minY) * 0.05);
        return (minX - dx, maxX + dx, minY - dy, maxY + dy);
    }

    private PointF WorldToScreen(double x, double y, Rectangle clientRect)
    {
        var (minX, maxX, minY, maxY) = GetWorldBounds();

       
        int left = clientRect.Left + MapPadding;
        int right = clientRect.Right - MapPadding;
        int top = clientRect.Top + MapPadding;
        int bottom = clientRect.Bottom - MapPadding;

        float width = Math.Max(1, right - left);
        float height = Math.Max(1, bottom - top);

        double sx = (maxX - minX) == 0 ? 0.5 : (x - minX) / (maxX - minX);
        double sy = (maxY - minY) == 0 ? 0.5 : (y - minY) / (maxY - minY);

        
        float px = left + (float)(sx * width);
        float py = bottom - (float)(sy * height);

        return new PointF(px, py);
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

        // Get bounds
        double minX = _cities.Min(c => c.X);
        double maxX = _cities.Max(c => c.X);
        double minY = _cities.Min(c => c.Y);
        double maxY = _cities.Max(c => c.Y);
        double dx = Math.Max(1, (maxX - minX) * 0.05);
        double dy = Math.Max(1, (maxY - minY) * 0.05);
        minX -= dx; maxX += dx; minY -= dy; maxY += dy;

        const int pad = 40;
        int left = rect.Left + pad, right = rect.Right - pad;
        int top = rect.Top + pad, bottom = rect.Bottom - pad;

        float width = Math.Max(1, right - left);
        float height = Math.Max(1, bottom - top);

        PointF Map(double x, double y)
        {
            double sx = (maxX - minX) == 0 ? 0.5 : (x - minX) / (maxX - minX);
            double sy = (maxY - minY) == 0 ? 0.5 : (y - minY) / (maxY - minY);
            float px = left + (float)(sx * width);
            float py = bottom - (float)(sy * height); // invert Y
            return new PointF(px, py);
        }

        using (var axisPen = new Pen(Color.Black, 1))
        {
            // Draw X-axis at minY
            var xAxisY = Map(0, minY).Y; // baseline bottom
            g.DrawLine(axisPen, left, bottom, right, bottom);

            // Draw Y-axis at minX
            var yAxisX = Map(minX, 0).X; // baseline left
            g.DrawLine(axisPen, left, top, left, bottom);
        }

        using var labelBrush = new SolidBrush(Color.Black);

        // X-axis ticks & labels
        int xTicks = 5;
        for (int i = 0; i <= xTicks; i++)
        {
            double val = minX + i * (maxX - minX) / xTicks;
            var p = Map(val, minY);
            g.DrawLine(Pens.Black, p.X, bottom - 3, p.X, bottom + 3);
            var text = val.ToString("0.0");
            var size = g.MeasureString(text, Font);
            g.DrawString(text, Font, labelBrush, p.X - size.Width / 2, bottom + 5);
        }

        // Y-axis ticks & labels
        int yTicks = 5;
        for (int i = 0; i <= yTicks; i++)
        {
            double val = minY + i * (maxY - minY) / yTicks;
            var p = Map(minX, val);
            g.DrawLine(Pens.Black, left - 3, p.Y, left + 3, p.Y);
            var text = val.ToString("0.0");
            var size = g.MeasureString(text, Font);
            g.DrawString(text, Font, labelBrush, left - size.Width - 5, p.Y - size.Height / 2);
        }

        // Draw cities
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

    //----------------part3----------------------------
}
