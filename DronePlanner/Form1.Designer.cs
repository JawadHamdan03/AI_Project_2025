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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            numX = new NumericUpDown();
            numY = new NumericUpDown();
            numHumidity = new NumericUpDown();
            numTemp = new NumericUpDown();
            label5 = new Label();
            label6 = new Label();
            numWind = new NumericUpDown();
            label7 = new Label();
            btnAddCity = new Button();
            pictureMap = new PictureBox();
            btnOptimizeRoute = new Button();
            btnCostMatrix = new Button();
            btnAddRandomCity = new Button();
            ((System.ComponentModel.ISupportInitialize)numEpochs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewResults).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numHumidity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTemp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numWind).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureMap).BeginInit();
            SuspendLayout();
            // 
            // btnLoadExcel
            // 
            btnLoadExcel.BackColor = SystemColors.InactiveCaption;
            btnLoadExcel.FlatStyle = FlatStyle.Flat;
            btnLoadExcel.Font = new Font("Segoe UI Historic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLoadExcel.Location = new Point(350, 41);
            btnLoadExcel.Name = "btnLoadExcel";
            btnLoadExcel.Size = new Size(166, 29);
            btnLoadExcel.TabIndex = 0;
            btnLoadExcel.Text = "Load Data";
            btnLoadExcel.UseVisualStyleBackColor = false;
            btnLoadExcel.Click += btnLoadExcel_Click;
            // 
            // txtLearningRate
            // 
            txtLearningRate.Location = new Point(118, 43);
            txtLearningRate.Name = "txtLearningRate";
            txtLearningRate.Size = new Size(150, 27);
            txtLearningRate.TabIndex = 1;
            // 
            // numEpochs
            // 
            numEpochs.Location = new Point(118, 131);
            numEpochs.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            numEpochs.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numEpochs.Name = "numEpochs";
            numEpochs.Size = new Size(150, 27);
            numEpochs.TabIndex = 2;
            numEpochs.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnTrain
            // 
            btnTrain.BackColor = SystemColors.InactiveCaption;
            btnTrain.FlatStyle = FlatStyle.Flat;
            btnTrain.Location = new Point(350, 131);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(166, 29);
            btnTrain.TabIndex = 3;
            btnTrain.Text = "Train";
            btnTrain.UseVisualStyleBackColor = false;
            btnTrain.Click += btnTrain_Click;
            // 
            // btnPredict
            // 
            btnPredict.BackColor = SystemColors.Info;
            btnPredict.Enabled = false;
            btnPredict.FlatStyle = FlatStyle.Popup;
            btnPredict.Location = new Point(546, 132);
            btnPredict.Name = "btnPredict";
            btnPredict.Size = new Size(94, 29);
            btnPredict.TabIndex = 4;
            btnPredict.Text = "Test";
            btnPredict.UseVisualStyleBackColor = false;
            btnPredict.Click += btnPredict_Click;
            // 
            // dataGridViewResults
            // 
            dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewResults.Location = new Point(12, 237);
            dataGridViewResults.Name = "dataGridViewResults";
            dataGridViewResults.RowHeadersVisible = false;
            dataGridViewResults.RowHeadersWidth = 51;
            dataGridViewResults.Size = new Size(682, 407);
            dataGridViewResults.TabIndex = 5;
            dataGridViewResults.CellFormatting += dataGridViewResults_CellFormatting;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 205);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(50, 20);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "Ready";
            // 
            // lblAccuracy
            // 
            lblAccuracy.AutoSize = true;
            lblAccuracy.Location = new Point(441, 205);
            lblAccuracy.Name = "lblAccuracy";
            lblAccuracy.Size = new Size(75, 20);
            lblAccuracy.TabIndex = 7;
            lblAccuracy.Text = "Accuracy :";
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.Red;
            btnClear.FlatStyle = FlatStyle.Popup;
            btnClear.Location = new Point(546, 41);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(94, 29);
            btnClear.TabIndex = 8;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 46);
            label1.Name = "label1";
            label1.Size = new Size(100, 20);
            label1.TabIndex = 9;
            label1.Text = "Learning Rate";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 133);
            label2.Name = "label2";
            label2.Size = new Size(65, 20);
            label2.TabIndex = 10;
            label2.Text = "#Epochs";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(728, 39);
            label3.Name = "label3";
            label3.Size = new Size(29, 20);
            label3.TabIndex = 11;
            label3.Text = "X : ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(728, 129);
            label4.Name = "label4";
            label4.Size = new Size(24, 20);
            label4.TabIndex = 12;
            label4.Text = "Y :";
            // 
            // numX
            // 
            numX.Location = new Point(763, 39);
            numX.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            numX.Name = "numX";
            numX.Size = new Size(150, 27);
            numX.TabIndex = 13;
            numX.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // numY
            // 
            numY.Location = new Point(763, 129);
            numY.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            numY.Name = "numY";
            numY.Size = new Size(150, 27);
            numY.TabIndex = 14;
            numY.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // numHumidity
            // 
            numHumidity.Location = new Point(1008, 129);
            numHumidity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numHumidity.Name = "numHumidity";
            numHumidity.Size = new Size(150, 27);
            numHumidity.TabIndex = 18;
            numHumidity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // numTemp
            // 
            numTemp.Location = new Point(1008, 39);
            numTemp.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numTemp.Name = "numTemp";
            numTemp.Size = new Size(150, 27);
            numTemp.TabIndex = 17;
            numTemp.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(973, 129);
            label5.Name = "label5";
            label5.Size = new Size(27, 20);
            label5.TabIndex = 16;
            label5.Text = "H :";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(973, 39);
            label6.Name = "label6";
            label6.Size = new Size(24, 20);
            label6.TabIndex = 15;
            label6.Text = "T :";
            // 
            // numWind
            // 
            numWind.Location = new Point(1216, 39);
            numWind.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numWind.Name = "numWind";
            numWind.Size = new Size(150, 27);
            numWind.TabIndex = 20;
            numWind.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(1181, 39);
            label7.Name = "label7";
            label7.Size = new Size(30, 20);
            label7.TabIndex = 19;
            label7.Text = "W :";
            // 
            // btnAddCity
            // 
            btnAddCity.BackColor = Color.OliveDrab;
            btnAddCity.FlatStyle = FlatStyle.Popup;
            btnAddCity.Location = new Point(1216, 125);
            btnAddCity.Name = "btnAddCity";
            btnAddCity.Size = new Size(150, 29);
            btnAddCity.TabIndex = 21;
            btnAddCity.Text = "Add City";
            btnAddCity.UseVisualStyleBackColor = false;
            btnAddCity.Click += btnAddCity_Click;
            // 
            // pictureMap
            // 
            pictureMap.Location = new Point(728, 237);
            pictureMap.Name = "pictureMap";
            pictureMap.Size = new Size(654, 407);
            pictureMap.TabIndex = 22;
            pictureMap.TabStop = false;
            pictureMap.Paint += pictureMap_Paint;
            // 
            // btnOptimizeRoute
            // 
            btnOptimizeRoute.BackColor = Color.Chartreuse;
            btnOptimizeRoute.FlatStyle = FlatStyle.Popup;
            btnOptimizeRoute.Location = new Point(728, 695);
            btnOptimizeRoute.Name = "btnOptimizeRoute";
            btnOptimizeRoute.Size = new Size(157, 29);
            btnOptimizeRoute.TabIndex = 23;
            btnOptimizeRoute.Text = "Optimize Route";
            btnOptimizeRoute.UseVisualStyleBackColor = false;
            btnOptimizeRoute.Click += btnOptimizeRoute_Click;
            // 
            // btnCostMatrix
            // 
            btnCostMatrix.BackColor = Color.Chartreuse;
            btnCostMatrix.FlatStyle = FlatStyle.Popup;
            btnCostMatrix.Location = new Point(12, 695);
            btnCostMatrix.Name = "btnCostMatrix";
            btnCostMatrix.Size = new Size(187, 29);
            btnCostMatrix.TabIndex = 24;
            btnCostMatrix.Text = "Build Cost Matrix";
            btnCostMatrix.UseVisualStyleBackColor = false;
            btnCostMatrix.Click += btnCostMatrix_Click;
            // 
            // btnAddRandomCity
            // 
            btnAddRandomCity.BackColor = Color.OliveDrab;
            btnAddRandomCity.FlatStyle = FlatStyle.Popup;
            btnAddRandomCity.Location = new Point(1216, 178);
            btnAddRandomCity.Name = "btnAddRandomCity";
            btnAddRandomCity.Size = new Size(150, 29);
            btnAddRandomCity.TabIndex = 25;
            btnAddRandomCity.Text = "Add Random City";
            btnAddRandomCity.UseVisualStyleBackColor = false;
            btnAddRandomCity.Click += btnAddRandomCity_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1394, 786);
            Controls.Add(btnAddRandomCity);
            Controls.Add(btnCostMatrix);
            Controls.Add(btnOptimizeRoute);
            Controls.Add(pictureMap);
            Controls.Add(btnAddCity);
            Controls.Add(numWind);
            Controls.Add(label7);
            Controls.Add(numHumidity);
            Controls.Add(numTemp);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(numY);
            Controls.Add(numX);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
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
            ((System.ComponentModel.ISupportInitialize)numX).EndInit();
            ((System.ComponentModel.ISupportInitialize)numY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numHumidity).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTemp).EndInit();
            ((System.ComponentModel.ISupportInitialize)numWind).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureMap).EndInit();
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
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown numX;
        private NumericUpDown numY;
        private NumericUpDown numHumidity;
        private NumericUpDown numTemp;
        private Label label5;
        private Label label6;
        private NumericUpDown numWind;
        private Label label7;
        private Button btnAddCity;
        private PictureBox pictureMap;
        private Button btnOptimizeRoute;
        private Button btnCostMatrix;
        private Button btnAddRandomCity;
    }
}
