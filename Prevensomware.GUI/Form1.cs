using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Prevensomware.Logic;

namespace Prevensomware.GUI
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void browseFileDialog_Click(object sender, EventArgs e)
        {
            var dialogResult = folderBrowserDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                lblPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }
       
        private IEnumerable<ExtensionReplacement> GenerateExtensionReplacementList(string listTxt)
        {
            var extensionReplacementArray = listTxt.Split(';');
            var extensionReplacementList = new List<ExtensionReplacement>();
            foreach (var extensionReplacement in extensionReplacementArray)
            {
                var name = extensionReplacement.Split(':')[0];
                var replacement = extensionReplacement.Split(':')[1];
                extensionReplacementList.Add(new ExtensionReplacement
                {
                    Name =  name,
                    Replacement = replacement
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

        private void GetFileWorkerUpdateLog()
        {
        }
        private void SetWorkerThreads()
        {
            var extensionReplacementList = GenerateExtensionReplacementList(textBox2.Text);
            var registryWorker = new BackgroundWorker();
            registryWorker.DoWork += (s, eventArgs) => WindowsRegistryManager.GenerateNewRegistryKeys(extensionReplacementList);
            registryWorker.RunWorkerAsync();
            var fileWorker = new BackgroundWorker();
            fileWorker.DoWork += (s, eventArgs) => FileManager.RenameAllFilesWithNewExtension(extensionReplacementList, lblPath.Text);
            fileWorker.RunWorkerCompleted += FileWorkerWorkCompleted;
            fileWorker.RunWorkerAsync();

        }
    }
}
