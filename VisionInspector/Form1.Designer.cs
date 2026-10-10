namespace VisionInspector
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dgvHistory = new DataGridView();
            picOriginal = new PictureBox();
            picResult = new PictureBox();
            btnOpen = new Button();
            btnInspect = new Button();
            lblResult = new Label();
            lblTime = new Label();
            numMinArea = new NumericUpDown();
            numMaxCount = new NumericUpDown();
            lblMinArea = new Label();
            lblMaxCount = new Label();
            ((System.ComponentModel.ISupportInitialize)picOriginal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picResult).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinArea).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxCount).BeginInit();
            SuspendLayout();

            // btnOpen
            btnOpen.Location = new Point(12, 12);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(110, 32);
            btnOpen.Text = "이미지 열기";
            btnOpen.UseVisualStyleBackColor = true;
            btnOpen.Click += btnOpen_Click;

            // btnInspect
            btnInspect.Location = new Point(130, 12);
            btnInspect.Name = "btnInspect";
            btnInspect.Size = new Size(110, 32);
            btnInspect.Text = "검사";
            btnInspect.UseVisualStyleBackColor = true;
            btnInspect.Click += btnInspect_Click;

            // lblMinArea (캡션)
            lblMinArea.AutoSize = true;
            lblMinArea.Location = new Point(270, 20);
            lblMinArea.Name = "lblMinArea";
            lblMinArea.Text = "최소 면적:";

            // numMinArea
            numMinArea.Location = new Point(340, 16);
            numMinArea.Name = "numMinArea";
            numMinArea.Size = new Size(80, 27);

            // lblMaxCount (캡션)
            lblMaxCount.AutoSize = true;
            lblMaxCount.Location = new Point(445, 20);
            lblMaxCount.Name = "lblMaxCount";
            lblMaxCount.Text = "허용 개수:";

            // numMaxCount
            numMaxCount.Location = new Point(515, 16);
            numMaxCount.Name = "numMaxCount";
            numMaxCount.Size = new Size(60, 27);

            // picOriginal
            picOriginal.BorderStyle = BorderStyle.FixedSingle;
            picOriginal.Location = new Point(12, 60);
            picOriginal.Name = "picOriginal";
            picOriginal.Size = new Size(470, 360);
            picOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            picOriginal.TabStop = false;

            // picResult
            picResult.BorderStyle = BorderStyle.FixedSingle;
            picResult.Location = new Point(500, 60);
            picResult.Name = "picResult";
            picResult.Size = new Size(470, 360);
            picResult.SizeMode = PictureBoxSizeMode.Zoom;
            picResult.TabStop = false;

            // lblResult
            lblResult.AutoSize = true;
            lblResult.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblResult.Location = new Point(12, 435);
            lblResult.Name = "lblResult";
            lblResult.Text = "-";

            // lblTime
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTime.Location = new Point(14, 520);
            lblTime.Name = "lblTime";
            lblTime.Text = "검사 시간: - ms";

            // dgvHistory
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistory.Location = new Point(250, 435);
            dgvHistory.Name = "dgvHistory";
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersVisible = false;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistory.Size = new Size(720, 225);

            // Form1
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(985, 680);
            Controls.Add(btnOpen);
            Controls.Add(btnInspect);
            Controls.Add(lblMinArea);
            Controls.Add(numMinArea);
            Controls.Add(lblMaxCount);
            Controls.Add(numMaxCount);
            Controls.Add(picOriginal);
            Controls.Add(picResult);
            Controls.Add(lblResult);
            Controls.Add(lblTime);
            Controls.Add(dgvHistory);
            Name = "Form1";
            Text = "Vision Inspector";
            ((System.ComponentModel.ISupportInitialize)picOriginal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picResult).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinArea).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxCount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picOriginal;
        private PictureBox picResult;
        private Button btnOpen;
        private Button btnInspect;
        private Label lblResult;
        private Label lblTime;
        private NumericUpDown numMinArea;
        private NumericUpDown numMaxCount;
        private Label lblMinArea;
        private Label lblMaxCount;

        private DataGridView dgvHistory;
    }
}