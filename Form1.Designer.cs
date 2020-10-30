namespace MineSweeper_ypz
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_level = new System.Windows.Forms.Label();
            this.radioButton_easy = new System.Windows.Forms.RadioButton();
            this.radioButton_median = new System.Windows.Forms.RadioButton();
            this.radioButton_hard = new System.Windows.Forms.RadioButton();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label_status = new System.Windows.Forms.Label();
            this.menuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_level
            // 
            this.label_level.AutoSize = true;
            this.label_level.Location = new System.Drawing.Point(12, 24);
            this.label_level.Name = "label_level";
            this.label_level.Size = new System.Drawing.Size(35, 12);
            this.label_level.TabIndex = 0;
            this.label_level.Text = "Level";
            // 
            // radioButton_easy
            // 
            this.radioButton_easy.AutoSize = true;
            this.radioButton_easy.Location = new System.Drawing.Point(14, 39);
            this.radioButton_easy.Name = "radioButton_easy";
            this.radioButton_easy.Size = new System.Drawing.Size(47, 16);
            this.radioButton_easy.TabIndex = 1;
            this.radioButton_easy.TabStop = true;
            this.radioButton_easy.Text = "Easy";
            this.radioButton_easy.UseVisualStyleBackColor = true;
            this.radioButton_easy.CheckedChanged += new System.EventHandler(this.radioButton_easy_CheckedChanged);
            this.radioButton_easy.Click += new System.EventHandler(this.radioButton_easy_Click);
            // 
            // radioButton_median
            // 
            this.radioButton_median.AutoSize = true;
            this.radioButton_median.Location = new System.Drawing.Point(67, 39);
            this.radioButton_median.Name = "radioButton_median";
            this.radioButton_median.Size = new System.Drawing.Size(59, 16);
            this.radioButton_median.TabIndex = 2;
            this.radioButton_median.TabStop = true;
            this.radioButton_median.Text = "Median";
            this.radioButton_median.UseVisualStyleBackColor = true;
            this.radioButton_median.CheckedChanged += new System.EventHandler(this.radioButton_median_CheckedChanged);
            this.radioButton_median.Click += new System.EventHandler(this.radioButton_median_Click);
            // 
            // radioButton_hard
            // 
            this.radioButton_hard.AutoSize = true;
            this.radioButton_hard.Location = new System.Drawing.Point(142, 39);
            this.radioButton_hard.Name = "radioButton_hard";
            this.radioButton_hard.Size = new System.Drawing.Size(47, 16);
            this.radioButton_hard.TabIndex = 2;
            this.radioButton_hard.TabStop = true;
            this.radioButton_hard.Text = "Hard";
            this.radioButton_hard.UseVisualStyleBackColor = true;
            this.radioButton_hard.CheckedChanged += new System.EventHandler(this.radioButton_hard_CheckedChanged);
            this.radioButton_hard.Click += new System.EventHandler(this.radioButton_hard_Click);
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(388, 24);
            this.menuBar.TabIndex = 3;
            this.menuBar.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Location = new System.Drawing.Point(85, 24);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(41, 12);
            this.label_status.TabIndex = 4;
            this.label_status.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 450);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.radioButton_hard);
            this.Controls.Add(this.radioButton_median);
            this.Controls.Add(this.radioButton_easy);
            this.Controls.Add(this.label_level);
            this.Controls.Add(this.menuBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuBar;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "MineSweeper";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_level;
        private System.Windows.Forms.RadioButton radioButton_easy;
        private System.Windows.Forms.RadioButton radioButton_median;
        private System.Windows.Forms.RadioButton radioButton_hard;
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label label_status;
    }
}

