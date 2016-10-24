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
            this.tbPayload = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRevert = new System.Windows.Forms.Button();
            this.radioBtnHDD = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioBtnPath = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.Color.Black;
            this.tbLog.ForeColor = System.Drawing.Color.White;
            this.tbLog.Location = new System.Drawing.Point(37, 472);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(2200, 944);
            this.tbLog.TabIndex = 12;
            this.tbLog.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(1367, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(378, 32);
            this.label1.TabIndex = 11;
            this.label1.Text = "ex: .doc:.blahdoc;.txt:.blahtxt";
            // 
            // lblPath
            // 
            this.lblPath.Location = new System.Drawing.Point(1033, 197);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(696, 45);
            this.lblPath.TabIndex = 9;
            // 
            // tbPayload
            // 
            this.tbPayload.Location = new System.Drawing.Point(422, 54);
            this.tbPayload.Multiline = true;
            this.tbPayload.Name = "tbPayload";
            this.tbPayload.Size = new System.Drawing.Size(930, 65);
            this.tbPayload.TabIndex = 8;
            this.tbPayload.Text = ".doc:.blahdoc;.docx:.blahdocx";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.White;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.ForeColor = System.Drawing.Color.Black;
            this.btnStart.Location = new System.Drawing.Point(1908, 298);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(329, 71);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(388, 32);
            this.label2.TabIndex = 10;
            this.label2.Text = "Extensions And Replacement";
            // 
            // btnRevert
            // 
            this.btnRevert.BackColor = System.Drawing.Color.White;
            this.btnRevert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRevert.ForeColor = System.Drawing.Color.Black;
            this.btnRevert.Location = new System.Drawing.Point(1908, 395);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(329, 71);
            this.btnRevert.TabIndex = 13;
            this.btnRevert.Text = "Revert Changes";
            this.btnRevert.UseVisualStyleBackColor = false;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // radioBtnHDD
            // 
            this.radioBtnHDD.AutoSize = true;
            this.radioBtnHDD.BackColor = System.Drawing.Color.White;
            this.radioBtnHDD.Location = new System.Drawing.Point(6, 57);
            this.radioBtnHDD.Name = "radioBtnHDD";
            this.radioBtnHDD.Size = new System.Drawing.Size(186, 36);
            this.radioBtnHDD.TabIndex = 14;
            this.radioBtnHDD.TabStop = true;
            this.radioBtnHDD.Text = "Hard Drive";
            this.radioBtnHDD.UseVisualStyleBackColor = false;
            this.radioBtnHDD.Click += new System.EventHandler(this.radioBtnHDD_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.radioBtnPath);
            this.groupBox1.Controls.Add(this.radioBtnHDD);
            this.groupBox1.Location = new System.Drawing.Point(422, 137);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(605, 142);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Path";
            // 
            // radioBtnPath
            // 
            this.radioBtnPath.AutoSize = true;
            this.radioBtnPath.BackColor = System.Drawing.Color.White;
            this.radioBtnPath.Location = new System.Drawing.Point(265, 57);
            this.radioBtnPath.Name = "radioBtnPath";
            this.radioBtnPath.Size = new System.Drawing.Size(210, 36);
            this.radioBtnPath.TabIndex = 14;
            this.radioBtnPath.TabStop = true;
            this.radioBtnPath.Text = "Certain Path";
            this.radioBtnPath.UseVisualStyleBackColor = false;
            this.radioBtnPath.Click += new System.EventHandler(this.radioButton2_Clicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(276, 32);
            this.label3.TabIndex = 10;
            this.label3.Text = "Choose Search Path";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(2413, 1452);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRevert);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.tbPayload);
            this.Controls.Add(this.btnStart);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Prevensomeware";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox tbPayload;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.RadioButton radioBtnHDD;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioBtnPath;
        private System.Windows.Forms.Label label3;
    }
}