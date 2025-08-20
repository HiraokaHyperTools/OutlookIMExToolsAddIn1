namespace OutlookIMExToolsAddIn1.Forms
{
    partial class ImTbForm
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
            this._import = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._toOutlookFolder = new System.Windows.Forms.TextBox();
            this._selectOutlookFolder = new System.Windows.Forms.Button();
            this._selectPopup = new System.Windows.Forms.Button();
            this._tree = new System.Windows.Forms.TreeView();
            this._popup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._ofd = new System.Windows.Forms.OpenFileDialog();
            this._checkOnRecur = new System.Windows.Forms.Button();
            this._checkOffRecur = new System.Windows.Forms.Button();
            this._onAll = new System.Windows.Forms.Button();
            this._offAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _import
            // 
            this._import.Location = new System.Drawing.Point(12, 405);
            this._import.Name = "_import";
            this._import.Size = new System.Drawing.Size(125, 46);
            this._import.TabIndex = 10;
            this._import.Text = "Import";
            this._import.UseVisualStyleBackColor = true;
            this._import.Click += new System.EventHandler(this._import_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Import mails from this Thunderbird profile";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 324);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Import to this Outlook folder";
            // 
            // _toOutlookFolder
            // 
            this._toOutlookFolder.Location = new System.Drawing.Point(27, 357);
            this._toOutlookFolder.Name = "_toOutlookFolder";
            this._toOutlookFolder.ReadOnly = true;
            this._toOutlookFolder.Size = new System.Drawing.Size(417, 19);
            this._toOutlookFolder.TabIndex = 8;
            // 
            // _selectOutlookFolder
            // 
            this._selectOutlookFolder.Location = new System.Drawing.Point(450, 357);
            this._selectOutlookFolder.Name = "_selectOutlookFolder";
            this._selectOutlookFolder.Size = new System.Drawing.Size(75, 19);
            this._selectOutlookFolder.TabIndex = 9;
            this._selectOutlookFolder.Text = "...";
            this._selectOutlookFolder.UseVisualStyleBackColor = true;
            this._selectOutlookFolder.Click += new System.EventHandler(this._selectOutlookFolder_Click);
            // 
            // _selectPopup
            // 
            this._selectPopup.Location = new System.Drawing.Point(27, 68);
            this._selectPopup.Name = "_selectPopup";
            this._selectPopup.Size = new System.Drawing.Size(125, 23);
            this._selectPopup.TabIndex = 1;
            this._selectPopup.Text = "Select profile or...";
            this._selectPopup.UseVisualStyleBackColor = true;
            this._selectPopup.Click += new System.EventHandler(this._selectPopup_Click);
            // 
            // _tree
            // 
            this._tree.CheckBoxes = true;
            this._tree.Location = new System.Drawing.Point(27, 97);
            this._tree.Name = "_tree";
            this._tree.Size = new System.Drawing.Size(498, 204);
            this._tree.TabIndex = 6;
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
            // _checkOnRecur
            // 
            this._checkOnRecur.Location = new System.Drawing.Point(191, 68);
            this._checkOnRecur.Name = "_checkOnRecur";
            this._checkOnRecur.Size = new System.Drawing.Size(125, 23);
            this._checkOnRecur.TabIndex = 4;
            this._checkOnRecur.Text = "Check on recursively";
            this._checkOnRecur.UseVisualStyleBackColor = true;
            this._checkOnRecur.Click += new System.EventHandler(this._checkOnRecur_Click);
            // 
            // _checkOffRecur
            // 
            this._checkOffRecur.Location = new System.Drawing.Point(322, 68);
            this._checkOffRecur.Name = "_checkOffRecur";
            this._checkOffRecur.Size = new System.Drawing.Size(125, 23);
            this._checkOffRecur.TabIndex = 5;
            this._checkOffRecur.Text = "Check off recursively";
            this._checkOffRecur.UseVisualStyleBackColor = true;
            this._checkOffRecur.Click += new System.EventHandler(this._checkOffRecur_Click);
            // 
            // _onAll
            // 
            this._onAll.Location = new System.Drawing.Point(191, 39);
            this._onAll.Name = "_onAll";
            this._onAll.Size = new System.Drawing.Size(125, 23);
            this._onAll.TabIndex = 2;
            this._onAll.Text = "Check on all";
            this._onAll.UseVisualStyleBackColor = true;
            this._onAll.Click += new System.EventHandler(this._onAll_Click);
            // 
            // _offAll
            // 
            this._offAll.Location = new System.Drawing.Point(322, 39);
            this._offAll.Name = "_offAll";
            this._offAll.Size = new System.Drawing.Size(125, 23);
            this._offAll.TabIndex = 3;
            this._offAll.Text = "Check off all";
            this._offAll.UseVisualStyleBackColor = true;
            this._offAll.Click += new System.EventHandler(this._offAll_Click);
            // 
            // ImTbForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(572, 477);
            this.Controls.Add(this._offAll);
            this.Controls.Add(this._onAll);
            this.Controls.Add(this._checkOffRecur);
            this.Controls.Add(this._checkOnRecur);
            this.Controls.Add(this._tree);
            this.Controls.Add(this._selectPopup);
            this.Controls.Add(this._selectOutlookFolder);
            this.Controls.Add(this._toOutlookFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._import);
            this.Name = "ImTbForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import from Thunderbird";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImTbForm_FormClosed);
            this.Load += new System.EventHandler(this.ImTbForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _import;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _toOutlookFolder;
        private System.Windows.Forms.Button _selectOutlookFolder;
        private System.Windows.Forms.Button _selectPopup;
        private System.Windows.Forms.TreeView _tree;
        private System.Windows.Forms.ContextMenuStrip _popup;
        private System.Windows.Forms.OpenFileDialog _ofd;
        private System.Windows.Forms.Button _checkOnRecur;
        private System.Windows.Forms.Button _checkOffRecur;
        private System.Windows.Forms.Button _onAll;
        private System.Windows.Forms.Button _offAll;
    }
}