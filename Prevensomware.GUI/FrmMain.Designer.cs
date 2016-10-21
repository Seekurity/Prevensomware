namespace Prevensomware.GUI
{
    partial class FrmMain
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
            this.btnRenameExtensions = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.lblStatue = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnRenameExtensions
            // 
            this.btnRenameExtensions.Location = new System.Drawing.Point(657, 1033);
            this.btnRenameExtensions.Name = "btnRenameExtensions";
            this.btnRenameExtensions.Size = new System.Drawing.Size(329, 44);
            this.btnRenameExtensions.TabIndex = 0;
            this.btnRenameExtensions.Text = "Rename Extensions";
            this.btnRenameExtensions.UseVisualStyleBackColor = true;
            this.btnRenameExtensions.Click += new System.EventHandler(this.btnRenameExtensions_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(69, 67);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(390, 38);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = ".doc:.blahdoc;.docx:.blahdocx";
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(242, 73);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(0, 32);
            this.lblPath.TabIndex = 2;
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Location = new System.Drawing.Point(790, 63);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(329, 44);
            this.btnChoosePath.TabIndex = 0;
            this.btnChoosePath.Text = "Choose Path";
            this.btnChoosePath.UseVisualStyleBackColor = true;
            this.btnChoosePath.Click += new System.EventHandler(this.browseFileDialog_Click);
            // 
            // lblStatue
            // 
            this.lblStatue.AutoSize = true;
            this.lblStatue.Location = new System.Drawing.Point(12, 1057);
            this.lblStatue.Name = "lblStatue";
            this.lblStatue.Size = new System.Drawing.Size(98, 32);
            this.lblStatue.TabIndex = 3;
            this.lblStatue.Text = "Statue";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 64);
            this.label1.TabIndex = 4;
            this.label1.Text = "ex: \r\n.doc:.blahdoc;.txt:.blahtxt";
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(18, 298);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(1541, 566);
            this.tbLog.TabIndex = 5;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1585, 1139);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStatue);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.btnChoosePath);
            this.Controls.Add(this.btnRenameExtensions);
            this.Name = "FrmMain";
            this.Text = "Main";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRenameExtensions;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnChoosePath;
        private System.Windows.Forms.Label lblStatue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLog;
    }
}

