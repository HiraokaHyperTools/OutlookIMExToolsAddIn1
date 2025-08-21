namespace OutlookIMExToolsAddIn1.Forms
{
    partial class ImTbContactsForm
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
            this.components = new System.ComponentModel.Container();
            this._tree = new System.Windows.Forms.TreeView();
            this._selectPopup = new System.Windows.Forms.Button();
            this._selectOutlookFolder = new System.Windows.Forms.Button();
            this._toOutlookFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._import = new System.Windows.Forms.Button();
            this._popup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._ofd = new System.Windows.Forms.OpenFileDialog();
            this._offAll = new System.Windows.Forms.Button();
            this._onAll = new System.Windows.Forms.Button();
            this._ofdAbook = new System.Windows.Forms.OpenFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this._overwrite = new System.Windows.Forms.ComboBox();
            this._importer = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _tree
            // 
            this._tree.CheckBoxes = true;
            this._tree.Location = new System.Drawing.Point(27, 76);
            this._tree.Name = "_tree";
            this._tree.Size = new System.Drawing.Size(498, 204);
            this._tree.TabIndex = 17;
            // 
            // _selectPopup
            // 
            this._selectPopup.Location = new System.Drawing.Point(27, 47);
            this._selectPopup.Name = "_selectPopup";
            this._selectPopup.Size = new System.Drawing.Size(125, 23);
            this._selectPopup.TabIndex = 12;
            this._selectPopup.Text = "Select profile or...";
            this._selectPopup.UseVisualStyleBackColor = true;
            this._selectPopup.Click += new System.EventHandler(this._selectPopup_Click);
            // 
            // _selectOutlookFolder
            // 
            this._selectOutlookFolder.Location = new System.Drawing.Point(450, 336);
            this._selectOutlookFolder.Name = "_selectOutlookFolder";
            this._selectOutlookFolder.Size = new System.Drawing.Size(75, 19);
            this._selectOutlookFolder.TabIndex = 20;
            this._selectOutlookFolder.Text = "...";
            this._selectOutlookFolder.UseVisualStyleBackColor = true;
            this._selectOutlookFolder.Click += new System.EventHandler(this._selectOutlookFolder_Click);
            // 
            // _toOutlookFolder
            // 
            this._toOutlookFolder.Location = new System.Drawing.Point(27, 336);
            this._toOutlookFolder.Name = "_toOutlookFolder";
            this._toOutlookFolder.ReadOnly = true;
            this._toOutlookFolder.Size = new System.Drawing.Size(417, 19);
            this._toOutlookFolder.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 303);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "Import to this Outlook folder";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "Import contacts from this Thunderbird profile";
            // 
            // _import
            // 
            this._import.Location = new System.Drawing.Point(12, 482);
            this._import.Name = "_import";
            this._import.Size = new System.Drawing.Size(125, 46);
            this._import.TabIndex = 25;
            this._import.Text = "Import";
            this._import.UseVisualStyleBackColor = true;
            this._import.Click += new System.EventHandler(this._import_Click);
            // 
            // _popup
            // 
            this._popup.Name = "_popup";
            this._popup.Size = new System.Drawing.Size(61, 4);
            // 
            // _ofd
            // 
            this._ofd.CheckFileExists = false;
            this._ofd.FileName = "(DIR)";
            // 
            // _offAll
            // 
            this._offAll.Location = new System.Drawing.Point(322, 47);
            this._offAll.Name = "_offAll";
            this._offAll.Size = new System.Drawing.Size(125, 23);
            this._offAll.TabIndex = 14;
            this._offAll.Text = "Check off all";
            this._offAll.UseVisualStyleBackColor = true;
            this._offAll.Click += new System.EventHandler(this._offAll_Click);
            // 
            // _onAll
            // 
            this._onAll.Location = new System.Drawing.Point(191, 47);
            this._onAll.Name = "_onAll";
            this._onAll.Size = new System.Drawing.Size(125, 23);
            this._onAll.TabIndex = 13;
            this._onAll.Text = "Check on all";
            this._onAll.UseVisualStyleBackColor = true;
            this._onAll.Click += new System.EventHandler(this._onAll_Click);
            // 
            // _ofdAbook
            // 
            this._ofdAbook.CheckFileExists = false;
            this._ofdAbook.Filter = "abook.sqlite|abook.sqlite";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 369);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "Overwrite?";
            // 
            // _overwrite
            // 
            this._overwrite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._overwrite.FormattingEnabled = true;
            this._overwrite.Location = new System.Drawing.Point(27, 384);
            this._overwrite.Name = "_overwrite";
            this._overwrite.Size = new System.Drawing.Size(417, 20);
            this._overwrite.TabIndex = 22;
            // 
            // _importer
            // 
            this._importer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._importer.FormattingEnabled = true;
            this._importer.Location = new System.Drawing.Point(27, 433);
            this._importer.Name = "_importer";
            this._importer.Size = new System.Drawing.Size(417, 20);
            this._importer.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 418);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "Importer?";
            // 
            // ImTbContactsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(599, 540);
            this.Controls.Add(this._importer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._overwrite);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._tree);
            this.Controls.Add(this._selectPopup);
            this.Controls.Add(this._selectOutlookFolder);
            this.Controls.Add(this._toOutlookFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._import);
            this.Controls.Add(this._offAll);
            this.Controls.Add(this._onAll);
            this.Name = "ImTbContactsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import contacts from Thunderbird";
            this.Load += new System.EventHandler(this.ImTbContactsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView _tree;
        private System.Windows.Forms.Button _selectPopup;
        private System.Windows.Forms.Button _selectOutlookFolder;
        private System.Windows.Forms.TextBox _toOutlookFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _import;
        private System.Windows.Forms.ContextMenuStrip _popup;
        private System.Windows.Forms.OpenFileDialog _ofd;
        private System.Windows.Forms.Button _offAll;
        private System.Windows.Forms.Button _onAll;
        private System.Windows.Forms.OpenFileDialog _ofdAbook;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox _overwrite;
        private System.Windows.Forms.ComboBox _importer;
        private System.Windows.Forms.Label label4;
    }
}