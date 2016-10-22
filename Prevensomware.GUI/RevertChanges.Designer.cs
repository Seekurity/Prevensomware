namespace Prevensomware.GUI
{
    partial class RevertChanges
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
            this.gridLogs = new System.Windows.Forms.DataGridView();
            this.btnRevert = new System.Windows.Forms.Button();
            this.btnRevertAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // gridLogs
            // 
            this.gridLogs.AllowUserToAddRows = false;
            this.gridLogs.AllowUserToDeleteRows = false;
            this.gridLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLogs.Location = new System.Drawing.Point(0, 0);
            this.gridLogs.Name = "gridLogs";
            this.gridLogs.ReadOnly = true;
            this.gridLogs.RowTemplate.Height = 40;
            this.gridLogs.Size = new System.Drawing.Size(1213, 533);
            this.gridLogs.TabIndex = 0;
            // 
            // btnRevert
            // 
            this.btnRevert.Location = new System.Drawing.Point(8, 600);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(338, 68);
            this.btnRevert.TabIndex = 1;
            this.btnRevert.Text = "Revert Selected Logs";
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // btnRevertAll
            // 
            this.btnRevertAll.Location = new System.Drawing.Point(438, 600);
            this.btnRevertAll.Name = "btnRevertAll";
            this.btnRevertAll.Size = new System.Drawing.Size(338, 68);
            this.btnRevertAll.TabIndex = 2;
            this.btnRevertAll.Text = "Revert All";
            this.btnRevertAll.UseVisualStyleBackColor = true;
            this.btnRevertAll.Click += new System.EventHandler(this.btnRevertAll_Click);
            // 
            // RevertChanges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 730);
            this.Controls.Add(this.btnRevertAll);
            this.Controls.Add(this.btnRevert);
            this.Controls.Add(this.gridLogs);
            this.Name = "RevertChanges";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Revert Changes";
            this.Load += new System.EventHandler(this.RevertChanges_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridLogs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridLogs;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.Button btnRevertAll;
    }
}