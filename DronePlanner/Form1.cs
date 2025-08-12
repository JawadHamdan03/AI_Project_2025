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
}
