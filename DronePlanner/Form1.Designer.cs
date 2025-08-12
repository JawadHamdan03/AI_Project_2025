namespace DronePlanner
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnLoadExcel = new Button();
            txtLearningRate = new TextBox();
            numEpochs = new NumericUpDown();
            btnTrain = new Button();
            btnPredict = new Button();
            dataGridViewResults = new DataGridView();
            lblStatus = new Label();
            lblAccuracy = new Label();
            btnClear = new Button();
            ((System.ComponentModel.ISupportInitialize)numEpochs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewResults).BeginInit();
            SuspendLayout();
            // 
            // btnLoadExcel
            // 
            btnLoadExcel.Location = new Point(12, 19);
            btnLoadExcel.Name = "btnLoadExcel";
            btnLoadExcel.Size = new Size(166, 29);
            btnLoadExcel.TabIndex = 0;
            btnLoadExcel.Text = "Load Data";
            btnLoadExcel.UseVisualStyleBackColor = true;
            btnLoadExcel.Click += btnLoadExcel_Click;
            // 
            // txtLearningRate
            // 
            txtLearningRate.Location = new Point(223, 22);
            txtLearningRate.Name = "txtLearningRate";
            txtLearningRate.Size = new Size(125, 27);
            txtLearningRate.TabIndex = 1;
            // 
            // numEpochs
            // 
            numEpochs.Location = new Point(394, 19);
            numEpochs.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numEpochs.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numEpochs.Name = "numEpochs";
            numEpochs.Size = new Size(150, 27);
            numEpochs.TabIndex = 2;
            numEpochs.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnTrain
            // 
            btnTrain.Location = new Point(719, 17);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(94, 29);
            btnTrain.TabIndex = 3;
            btnTrain.Text = "Train";
            btnTrain.UseVisualStyleBackColor = true;
            btnTrain.Click += btnTrain_Click;
            // 
            // btnPredict
            // 
            btnPredict.Location = new Point(835, 17);
            btnPredict.Name = "btnPredict";
            btnPredict.Size = new Size(94, 29);
            btnPredict.TabIndex = 4;
            btnPredict.Text = "Predict ";
            btnPredict.UseVisualStyleBackColor = true;
            btnPredict.Click += btnPredict_Click;
            // 
            // dataGridViewResults
            // 
            dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewResults.Location = new Point(-3, 100);
            dataGridViewResults.Name = "dataGridViewResults";
            dataGridViewResults.RowHeadersVisible = false;
            dataGridViewResults.RowHeadersWidth = 51;
            dataGridViewResults.Size = new Size(1393, 294);
            dataGridViewResults.TabIndex = 5;
            dataGridViewResults.CellFormatting += dataGridViewResults_CellFormatting;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(154, 433);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(50, 20);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "Ready";
            // 
            // lblAccuracy
            // 
            lblAccuracy.AutoSize = true;
            lblAccuracy.Location = new Point(745, 433);
            lblAccuracy.Name = "lblAccuracy";
            lblAccuracy.Size = new Size(68, 20);
            lblAccuracy.TabIndex = 7;
            lblAccuracy.Text = "Accuracy";
            // 
            // btnClear
            // 
            btnClear.Location = new Point(960, 20);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(94, 29);
            btnClear.TabIndex = 8;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1394, 473);
            Controls.Add(btnClear);
            Controls.Add(lblAccuracy);
            Controls.Add(lblStatus);
            Controls.Add(dataGridViewResults);
            Controls.Add(btnPredict);
            Controls.Add(btnTrain);
            Controls.Add(numEpochs);
            Controls.Add(txtLearningRate);
            Controls.Add(btnLoadExcel);
            Name = "Form1";
            Text = "Drone Planner";
            ((System.ComponentModel.ISupportInitialize)numEpochs).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewResults).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLoadExcel;
        private TextBox txtLearningRate;
        private NumericUpDown numEpochs;
        private Button btnTrain;
        private Button btnPredict;
        private DataGridView dataGridViewResults;
        private Label lblStatus;
        private Label lblAccuracy;
        private Button btnClear;
    }
}
