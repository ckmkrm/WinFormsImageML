namespace WinFormsImageML
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.Button btnPredict;
        private System.Windows.Forms.Button btnGray;
        private System.Windows.Forms.Button btnEdges;
        private System.Windows.Forms.PictureBox picOriginal;
        private System.Windows.Forms.PictureBox picProcessed;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.Label lblPrediction;
        private System.Windows.Forms.Label lblProjectRoot;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnTrain = new Button();
            btnLoadImage = new Button();
            btnPredict = new Button();
            btnGray = new Button();
            btnEdges = new Button();
            picOriginal = new PictureBox();
            picProcessed = new PictureBox();
            txtLog = new TextBox();
            txtImagePath = new TextBox();
            lblPrediction = new Label();
            lblProjectRoot = new Label();
            ((System.ComponentModel.ISupportInitialize)picOriginal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picProcessed).BeginInit();
            SuspendLayout();
            // 
            // btnTrain
            // 
            btnTrain.Location = new Point(12, 12);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(120, 30);
            btnTrain.TabIndex = 0;
            btnTrain.Text = "Train Model";
            btnTrain.UseVisualStyleBackColor = true;
            btnTrain.Click += btnTrain_Click;
            // 
            // btnLoadImage
            // 
            btnLoadImage.Location = new Point(12, 58);
            btnLoadImage.Name = "btnLoadImage";
            btnLoadImage.Size = new Size(120, 30);
            btnLoadImage.TabIndex = 1;
            btnLoadImage.Text = "Load Image...";
            btnLoadImage.UseVisualStyleBackColor = true;
            btnLoadImage.Click += btnLoadImage_Click;
            // 
            // btnPredict
            // 
            btnPredict.Location = new Point(12, 104);
            btnPredict.Name = "btnPredict";
            btnPredict.Size = new Size(120, 30);
            btnPredict.TabIndex = 2;
            btnPredict.Text = "Predict";
            btnPredict.UseVisualStyleBackColor = true;
            btnPredict.Click += btnPredict_Click;
            // 
            // btnGray
            // 
            btnGray.Location = new Point(12, 150);
            btnGray.Name = "btnGray";
            btnGray.Size = new Size(120, 30);
            btnGray.TabIndex = 3;
            btnGray.Text = "To Grayscale";
            btnGray.UseVisualStyleBackColor = true;
            btnGray.Click += btnGray_Click;
            // 
            // btnEdges
            // 
            btnEdges.Location = new Point(12, 196);
            btnEdges.Name = "btnEdges";
            btnEdges.Size = new Size(120, 30);
            btnEdges.TabIndex = 4;
            btnEdges.Text = "Sobel Edges";
            btnEdges.UseVisualStyleBackColor = true;
            btnEdges.Click += btnEdges_Click;
            // 
            // picOriginal
            // 
            picOriginal.BorderStyle = BorderStyle.FixedSingle;
            picOriginal.Location = new Point(150, 12);
            picOriginal.Name = "picOriginal";
            picOriginal.Size = new Size(320, 240);
            picOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            picOriginal.TabIndex = 5;
            picOriginal.TabStop = false;
            // 
            // picProcessed
            // 
            picProcessed.BorderStyle = BorderStyle.FixedSingle;
            picProcessed.Location = new Point(486, 12);
            picProcessed.Name = "picProcessed";
            picProcessed.Size = new Size(320, 240);
            picProcessed.SizeMode = PictureBoxSizeMode.Zoom;
            picProcessed.TabIndex = 6;
            picProcessed.TabStop = false;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(12, 285);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(794, 125);
            txtLog.TabIndex = 7;
            // 
            // txtImagePath
            // 
            txtImagePath.Location = new Point(150, 262);
            txtImagePath.Name = "txtImagePath";
            txtImagePath.ReadOnly = true;
            txtImagePath.Size = new Size(656, 31);
            txtImagePath.TabIndex = 8;
            // 
            // lblPrediction
            // 
            lblPrediction.AutoSize = true;
            lblPrediction.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPrediction.Location = new Point(150, 234);
            lblPrediction.Name = "lblPrediction";
            lblPrediction.Size = new Size(116, 28);
            lblPrediction.TabIndex = 9;
            lblPrediction.Text = "Tahmin: ---";
            // 
            // lblProjectRoot
            // 
            lblProjectRoot.AutoSize = true;
            lblProjectRoot.Location = new Point(12, 420);
            lblProjectRoot.Name = "lblProjectRoot";
            lblProjectRoot.Size = new Size(126, 25);
            lblProjectRoot.TabIndex = 10;
            lblProjectRoot.Text = "Project root: ...";
            // 
            // Form1
            // 
            ClientSize = new Size(818, 450);
            Controls.Add(lblProjectRoot);
            Controls.Add(lblPrediction);
            Controls.Add(txtImagePath);
            Controls.Add(txtLog);
            Controls.Add(picProcessed);
            Controls.Add(picOriginal);
            Controls.Add(btnEdges);
            Controls.Add(btnGray);
            Controls.Add(btnPredict);
            Controls.Add(btnLoadImage);
            Controls.Add(btnTrain);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Text = "WinForms Image ML Demo";
            ((System.ComponentModel.ISupportInitialize)picOriginal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picProcessed).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
