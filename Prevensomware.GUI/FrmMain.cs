using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Prevensomware.Dto;
using Prevensomware.Logic;

namespace Prevensomware.GUI
{
    public partial class FrmMain : Form
    {
        public FrmMain()
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
                    OriginalExtension =  name,
                    ReplacedExtension = replacement
                });
            }
            return extensionReplacementList;
            
        }

        private void btnRenameExtensions_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblPath.Text))
            {
                MessageBox.Show("Choose Path");
                return;
            };
            lblStatue.Text = "Working";
           SetWorkerThreads();
        }

        private void FileWorkerWorkCompleted(object sender, EventArgs e)
        {
            lblStatue.Text = "Done";
        }

        private void GetFileWorkerUpdateLog(string logEntry)
        {
            Invoke(new Action(() =>
            {
                tbLog.Text += logEntry + "\n";
            }));
        }
        private void SetWorkerThreads()
        {
            var extensionReplacementList = GenerateExtensionReplacementList(textBox2.Text);
            var registryWorker = new BackgroundWorker();
            registryWorker.DoWork += (s, eventArgs) => WindowsRegistryManager.GenerateNewRegistryKeys(extensionReplacementList);
            registryWorker.RunWorkerAsync();
            var fileWorker = new BackgroundWorker();
            fileWorker.DoWork += (s, eventArgs) => FileManager.RenameAllFilesWithNewExtensionForCertainPath(extensionReplacementList, lblPath.Text);
            fileWorker.RunWorkerCompleted += FileWorkerWorkCompleted;
            fileWorker.RunWorkerAsync();

        }
    }
}
