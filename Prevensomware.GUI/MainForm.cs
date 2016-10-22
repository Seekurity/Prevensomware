using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Prevensomware.Dto;
using Prevensomware.Logic;

namespace Prevensomware.GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            FileManager.LogDelegate = GetFileWorkerUpdateLog;
        }

        private void browseFileDialog_Click(object sender, EventArgs e)
        {
            var dialogResult = folderBrowserDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                lblPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private IEnumerable<DtoFileInfo> GenerateExtensionReplacementList(string listTxt)
        {
            var extensionReplacementArray = listTxt.Split(';');
            var extensionReplacementList = new List<DtoFileInfo>();
            foreach (var extensionReplacement in extensionReplacementArray)
            {
                var name = extensionReplacement.Split(':')[0];
                var replacement = extensionReplacement.Split(':')[1];
                extensionReplacementList.Add(new DtoFileInfo
                {
                    OriginalExtension = name,
                    ReplacedExtension = replacement
                });
            }
            return extensionReplacementList;

        }

        private void btnRenameExtensions_Click(object sender, EventArgs e)
        {
            tbLog.Text += DateTime.Now + " Started.\r\n";
            SetWorkerThreads();
        }

        private void FileWorkerWorkCompleted(object sender, EventArgs e)
        {
            tbLog.Text += DateTime.Now + " End.\r\n";

        }

        private void GetFileWorkerUpdateLog(string logEntry)
        {
            Invoke(new Action(() =>
            {
                tbLog.Text += DateTime.Now + " " + logEntry + "\r\n";
            }));
        }
        private void SetWorkerThreads()
        {
            var extensionReplacementList = GenerateExtensionReplacementList(textBox2.Text);
            var registryWorker = new BackgroundWorker();
            registryWorker.DoWork += (s, eventArgs) => WindowsRegistryManager.GenerateNewRegistryKeys(extensionReplacementList);
            registryWorker.RunWorkerAsync();
            var fileWorker = new BackgroundWorker();
            fileWorker.DoWork += (s, eventArgs) => FileManager.RenameAllFilesWithNewExtension(extensionReplacementList, string.IsNullOrEmpty(lblPath.Text) ? null : lblPath.Text);
            fileWorker.RunWorkerCompleted += FileWorkerWorkCompleted;
            fileWorker.RunWorkerAsync();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(700,650);
        }
    }
}
