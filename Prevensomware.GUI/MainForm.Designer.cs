namespace Prevensomware.GUI
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
            this.tbLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPath = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.btnRenameExtensions = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(37, 472);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(1315, 944);
            this.tbLog.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(416, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 64);
            this.label1.TabIndex = 11;
            this.label1.Text = "ex: \r\n.doc:.blahdoc;.txt:.blahtxt";
            // 
            // lblPath
            // 
            this.lblPath.Location = new System.Drawing.Point(416, 255);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(936, 40);
            this.lblPath.TabIndex = 9;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(422, 54);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(930, 65);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = ".doc:.blahdoc;.docx:.blahdocx";
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Location = new System.Drawing.Point(1388, 54);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(329, 65);
            this.btnChoosePath.TabIndex = 6;
            this.btnChoosePath.Text = "Choose Path";
            this.btnChoosePath.UseVisualStyleBackColor = true;
            this.btnChoosePath.Click += new System.EventHandler(this.browseFileDialog_Click);
            // 
            // btnRenameExtensions
            // 
            this.btnRenameExtensions.Location = new System.Drawing.Point(1388, 145);
            this.btnRenameExtensions.Name = "btnRenameExtensions";
            this.btnRenameExtensions.Size = new System.Drawing.Size(329, 71);
            this.btnRenameExtensions.TabIndex = 7;
            this.btnRenameExtensions.Text = "Rename Extensions";
            this.btnRenameExtensions.UseVisualStyleBackColor = true;
            this.btnRenameExtensions.Click += new System.EventHandler(this.btnRenameExtensions_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(388, 32);
            this.label2.TabIndex = 10;
            this.label2.Text = "Extensions And Replacement";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1741, 1452);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.btnChoosePath);
            this.Controls.Add(this.btnRenameExtensions);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnChoosePath;
        private System.Windows.Forms.Button btnRenameExtensions;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
    }
}