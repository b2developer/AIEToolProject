namespace AIEToolProject
{
    partial class ExportDialog
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
            this.exportButton = new System.Windows.Forms.Button();
            this.programPanel = new System.Windows.Forms.Panel();
            this.pythonRadio = new System.Windows.Forms.RadioButton();
            this.cppRadio = new System.Windows.Forms.RadioButton();
            this.csRadio = new System.Windows.Forms.RadioButton();
            this.programLabel = new System.Windows.Forms.Label();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.filePathButton = new System.Windows.Forms.Button();
            this.filePathLabel = new System.Windows.Forms.Label();
            this.programPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(213, 144);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(102, 26);
            this.exportButton.TabIndex = 0;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // programPanel
            // 
            this.programPanel.Controls.Add(this.pythonRadio);
            this.programPanel.Controls.Add(this.cppRadio);
            this.programPanel.Controls.Add(this.csRadio);
            this.programPanel.Location = new System.Drawing.Point(38, 101);
            this.programPanel.Name = "programPanel";
            this.programPanel.Size = new System.Drawing.Size(89, 69);
            this.programPanel.TabIndex = 1;
            // 
            // pythonRadio
            // 
            this.pythonRadio.AutoSize = true;
            this.pythonRadio.Location = new System.Drawing.Point(18, 49);
            this.pythonRadio.Name = "pythonRadio";
            this.pythonRadio.Size = new System.Drawing.Size(58, 17);
            this.pythonRadio.TabIndex = 2;
            this.pythonRadio.TabStop = true;
            this.pythonRadio.Text = "Python";
            this.pythonRadio.UseVisualStyleBackColor = true;
            // 
            // cppRadio
            // 
            this.cppRadio.AutoSize = true;
            this.cppRadio.Location = new System.Drawing.Point(18, 26);
            this.cppRadio.Name = "cppRadio";
            this.cppRadio.Size = new System.Drawing.Size(44, 17);
            this.cppRadio.TabIndex = 1;
            this.cppRadio.TabStop = true;
            this.cppRadio.Text = "C++";
            this.cppRadio.UseVisualStyleBackColor = true;
            // 
            // csRadio
            // 
            this.csRadio.AutoSize = true;
            this.csRadio.Checked = true;
            this.csRadio.Location = new System.Drawing.Point(18, 3);
            this.csRadio.Name = "csRadio";
            this.csRadio.Size = new System.Drawing.Size(39, 17);
            this.csRadio.TabIndex = 0;
            this.csRadio.TabStop = true;
            this.csRadio.Text = "C#";
            this.csRadio.UseVisualStyleBackColor = true;
            // 
            // programLabel
            // 
            this.programLabel.AutoSize = true;
            this.programLabel.Location = new System.Drawing.Point(24, 85);
            this.programLabel.Name = "programLabel";
            this.programLabel.Size = new System.Drawing.Size(119, 13);
            this.programLabel.TabIndex = 2;
            this.programLabel.Text = "Programming Language";
            this.programLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(27, 40);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.ReadOnly = true;
            this.filePathTextBox.Size = new System.Drawing.Size(235, 20);
            this.filePathTextBox.TabIndex = 3;
            // 
            // filePathButton
            // 
            this.filePathButton.Location = new System.Drawing.Point(268, 38);
            this.filePathButton.Name = "filePathButton";
            this.filePathButton.Size = new System.Drawing.Size(47, 23);
            this.filePathButton.TabIndex = 4;
            this.filePathButton.Text = "...";
            this.filePathButton.UseVisualStyleBackColor = true;
            this.filePathButton.Click += new System.EventHandler(this.filePathButton_Click);
            // 
            // filePathLabel
            // 
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(139, 24);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(48, 13);
            this.filePathLabel.TabIndex = 5;
            this.filePathLabel.Text = "File Path";
            this.filePathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(333, 184);
            this.Controls.Add(this.filePathLabel);
            this.Controls.Add(this.filePathButton);
            this.Controls.Add(this.filePathTextBox);
            this.Controls.Add(this.programLabel);
            this.Controls.Add(this.programPanel);
            this.Controls.Add(this.exportButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDialog";
            this.ShowIcon = false;
            this.Text = "Export";
            this.programPanel.ResumeLayout(false);
            this.programPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Panel programPanel;
        private System.Windows.Forms.RadioButton pythonRadio;
        private System.Windows.Forms.RadioButton cppRadio;
        private System.Windows.Forms.RadioButton csRadio;
        private System.Windows.Forms.Label programLabel;
        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Button filePathButton;
        private System.Windows.Forms.Label filePathLabel;
    }
}