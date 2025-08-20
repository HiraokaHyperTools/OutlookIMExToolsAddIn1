namespace OutlookIMExToolsAddIn1.Forms
{
    partial class ImForm
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
            this._hint = new System.Windows.Forms.Label();
            this._progress = new System.Windows.Forms.ProgressBar();
            this._tab = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._cancel = new System.Windows.Forms.Button();
            this._log = new System.Windows.Forms.Button();
            this._tab.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _hint
            // 
            this._hint.Dock = System.Windows.Forms.DockStyle.Fill;
            this._hint.Location = new System.Drawing.Point(6, 3);
            this._hint.Name = "_hint";
            this._hint.Size = new System.Drawing.Size(478, 142);
            this._hint.TabIndex = 0;
            this._hint.Text = "...";
            // 
            // _progress
            // 
            this._progress.Dock = System.Windows.Forms.DockStyle.Top;
            this._progress.Location = new System.Drawing.Point(6, 148);
            this._progress.Maximum = 10000;
            this._progress.Name = "_progress";
            this._progress.Size = new System.Drawing.Size(478, 23);
            this._progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._progress.TabIndex = 2;
            // 
            // _tab
            // 
            this._tab.ColumnCount = 1;
            this._tab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tab.Controls.Add(this._hint, 0, 0);
            this._tab.Controls.Add(this._progress, 0, 1);
            this._tab.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this._tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tab.Location = new System.Drawing.Point(0, 0);
            this._tab.Name = "_tab";
            this._tab.Padding = new System.Windows.Forms.Padding(3);
            this._tab.RowCount = 3;
            this._tab.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tab.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this._tab.Size = new System.Drawing.Size(490, 229);
            this._tab.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this._log, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._cancel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 174);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(256, 52);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // _cancel
            // 
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(3, 3);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(150, 46);
            this._cancel.TabIndex = 2;
            this._cancel.Text = "Cancel now";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _log
            // 
            this._log.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._log.Location = new System.Drawing.Point(159, 3);
            this._log.Name = "_log";
            this._log.Size = new System.Drawing.Size(94, 46);
            this._log.TabIndex = 3;
            this._log.Text = "Notepad log";
            this._log.UseVisualStyleBackColor = true;
            // 
            // ImForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(490, 229);
            this.Controls.Add(this._tab);
            this.Name = "ImForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import in progress";
            this._tab.ResumeLayout(false);
            this._tab.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Label _hint;
        internal System.Windows.Forms.ProgressBar _progress;
        private System.Windows.Forms.TableLayoutPanel _tab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal System.Windows.Forms.Button _cancel;
        internal System.Windows.Forms.Button _log;
    }
}