namespace AIEToolProject
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.newButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openButton = new System.Windows.Forms.ToolStripMenuItem();
            this.saveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.exportButton = new System.Windows.Forms.ToolStripMenuItem();
            this.editButton = new System.Windows.Forms.ToolStripMenuItem();
            this.undoButton = new System.Windows.Forms.ToolStripMenuItem();
            this.copyButton = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteButton = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolPanel = new System.Windows.Forms.Panel();
            this.actionButton = new System.Windows.Forms.Button();
            this.windowButton = new System.Windows.Forms.ToolStripMenuItem();
            this.conditionButton = new System.Windows.Forms.Button();
            this.selectorButton = new System.Windows.Forms.Button();
            this.sequenceButton = new System.Windows.Forms.Button();
            this.decoratorButton = new System.Windows.Forms.Button();
            this.MenuStrip.SuspendLayout();
            this.toolPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.BackColor = System.Drawing.SystemColors.MenuBar;
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileButton,
            this.editButton,
            this.settingsButton,
            this.windowButton});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.MdiWindowListItem = this.windowButton;
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(704, 24);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "MenuStrip";
            // 
            // fileButton
            // 
            this.fileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newButton,
            this.openButton,
            this.saveButton,
            this.exportButton});
            this.fileButton.Name = "fileButton";
            this.fileButton.Size = new System.Drawing.Size(37, 20);
            this.fileButton.Text = "File";
            // 
            // newButton
            // 
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(152, 22);
            this.newButton.Text = "New";
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // openButton
            // 
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(152, 22);
            this.openButton.Text = "Open";
            // 
            // saveButton
            // 
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(152, 22);
            this.saveButton.Text = "Save";
            // 
            // exportButton
            // 
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(152, 22);
            this.exportButton.Text = "Export";
            // 
            // editButton
            // 
            this.editButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoButton,
            this.copyButton,
            this.pasteButton});
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(39, 20);
            this.editButton.Text = "Edit";
            // 
            // undoButton
            // 
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(145, 22);
            this.undoButton.Text = "Undo (Ctrl-Z)";
            // 
            // copyButton
            // 
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(145, 22);
            this.copyButton.Text = "Copy (Ctrl-C)";
            // 
            // pasteButton
            // 
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.Size = new System.Drawing.Size(145, 22);
            this.pasteButton.Text = "Paste (Ctrl-V)";
            // 
            // settingsButton
            // 
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(61, 20);
            this.settingsButton.Text = "Settings";
            // 
            // toolPanel
            // 
            this.toolPanel.AutoSize = true;
            this.toolPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.toolPanel.BackColor = System.Drawing.SystemColors.Control;
            this.toolPanel.Controls.Add(this.decoratorButton);
            this.toolPanel.Controls.Add(this.sequenceButton);
            this.toolPanel.Controls.Add(this.selectorButton);
            this.toolPanel.Controls.Add(this.conditionButton);
            this.toolPanel.Controls.Add(this.actionButton);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolPanel.Location = new System.Drawing.Point(0, 24);
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Size = new System.Drawing.Size(40, 418);
            this.toolPanel.TabIndex = 1;
            // 
            // actionButton
            // 
            this.actionButton.Image = ((System.Drawing.Image)(resources.GetObject("actionButton.Image")));
            this.actionButton.Location = new System.Drawing.Point(5, 12);
            this.actionButton.Name = "actionButton";
            this.actionButton.Size = new System.Drawing.Size(32, 32);
            this.actionButton.TabIndex = 0;
            this.actionButton.UseVisualStyleBackColor = false;
            // 
            // windowButton
            // 
            this.windowButton.Name = "windowButton";
            this.windowButton.Size = new System.Drawing.Size(63, 20);
            this.windowButton.Text = "Window";
            // 
            // conditionButton
            // 
            this.conditionButton.Image = ((System.Drawing.Image)(resources.GetObject("conditionButton.Image")));
            this.conditionButton.Location = new System.Drawing.Point(5, 50);
            this.conditionButton.Name = "conditionButton";
            this.conditionButton.Size = new System.Drawing.Size(32, 32);
            this.conditionButton.TabIndex = 1;
            this.conditionButton.UseVisualStyleBackColor = false;
            // 
            // selectorButton
            // 
            this.selectorButton.Image = ((System.Drawing.Image)(resources.GetObject("selectorButton.Image")));
            this.selectorButton.Location = new System.Drawing.Point(5, 88);
            this.selectorButton.Name = "selectorButton";
            this.selectorButton.Size = new System.Drawing.Size(32, 32);
            this.selectorButton.TabIndex = 2;
            this.selectorButton.UseVisualStyleBackColor = false;
            // 
            // sequenceButton
            // 
            this.sequenceButton.Image = ((System.Drawing.Image)(resources.GetObject("sequenceButton.Image")));
            this.sequenceButton.Location = new System.Drawing.Point(5, 126);
            this.sequenceButton.Name = "sequenceButton";
            this.sequenceButton.Size = new System.Drawing.Size(32, 32);
            this.sequenceButton.TabIndex = 3;
            this.sequenceButton.UseVisualStyleBackColor = false;
            // 
            // decoratorButton
            // 
            this.decoratorButton.Image = ((System.Drawing.Image)(resources.GetObject("decoratorButton.Image")));
            this.decoratorButton.Location = new System.Drawing.Point(5, 164);
            this.decoratorButton.Name = "decoratorButton";
            this.decoratorButton.Size = new System.Drawing.Size(32, 32);
            this.decoratorButton.TabIndex = 4;
            this.decoratorButton.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(704, 442);
            this.Controls.Add(this.toolPanel);
            this.Controls.Add(this.MenuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "MainForm";
            this.Text = "Behaviour Tree Editor";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.toolPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileButton;
        private System.Windows.Forms.ToolStripMenuItem openButton;
        private System.Windows.Forms.ToolStripMenuItem saveButton;
        private System.Windows.Forms.ToolStripMenuItem exportButton;
        private System.Windows.Forms.ToolStripMenuItem editButton;
        private System.Windows.Forms.ToolStripMenuItem undoButton;
        private System.Windows.Forms.ToolStripMenuItem copyButton;
        private System.Windows.Forms.ToolStripMenuItem pasteButton;
        private System.Windows.Forms.ToolStripMenuItem settingsButton;
        private System.Windows.Forms.Panel toolPanel;
        private System.Windows.Forms.Button actionButton;
        private System.Windows.Forms.ToolStripMenuItem newButton;
        private System.Windows.Forms.ToolStripMenuItem windowButton;
        private System.Windows.Forms.Button conditionButton;
        private System.Windows.Forms.Button decoratorButton;
        private System.Windows.Forms.Button sequenceButton;
        private System.Windows.Forms.Button selectorButton;
    }
}

