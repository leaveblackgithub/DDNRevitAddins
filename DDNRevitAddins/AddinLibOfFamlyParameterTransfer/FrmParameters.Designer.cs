namespace DDNRevitAddins.AddinLibOfFamlyParameterTransfer
{
    partial class FrmParameters
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
            this.btnOK = new System.Windows.Forms.Button();
            this.cmbDocs = new System.Windows.Forms.ComboBox();
            this.lstParameters = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(0, 237);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(283, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmbDocs
            // 
            this.cmbDocs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDocs.FormattingEnabled = true;
            this.cmbDocs.Location = new System.Drawing.Point(0, 1);
            this.cmbDocs.Name = "cmbDocs";
            this.cmbDocs.Size = new System.Drawing.Size(283, 20);
            this.cmbDocs.TabIndex = 2;
            this.cmbDocs.SelectedIndexChanged += new System.EventHandler(this.cmbDocs_SelectedIndexChanged);
            // 
            // lstParameters
            // 
            this.lstParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstParameters.FormattingEnabled = true;
            this.lstParameters.ItemHeight = 12;
            this.lstParameters.Location = new System.Drawing.Point(0, 24);
            this.lstParameters.Name = "lstParameters";
            this.lstParameters.ScrollAlwaysVisible = true;
            this.lstParameters.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstParameters.Size = new System.Drawing.Size(283, 208);
            this.lstParameters.TabIndex = 0;
            // 
            // FrmParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.cmbDocs);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lstParameters);
            this.Name = "FrmParameters";
            this.Text = "Parameter List";
            this.Load += new System.EventHandler(this.FrmParameter_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmbDocs;
        private System.Windows.Forms.ListBox lstParameters;
    }
}