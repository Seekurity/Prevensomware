using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.ServiceProcess;
using System.Windows.Forms;
using AutoUpdaterDotNET;
using Prevensomware.Dto;
using Prevensomware.Logic;

namespace Prevensomware.GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                ProcessCommandFromWindowsContextMenu(args[1]);
            }
            InitializeComponent();
            FileManager.LogDelegate = LogChanges;
           
        }

        private void ProcessCommandFromWindowsContextMenu(string filePath)
        {
            var isReverted = new BoFileInfo().RevertForPath(filePath);
            if (isReverted) MessageBox.Show("File reverted succecssfully.");
            else MessageBox.Show("Couldn't revert the file.");
            this.Close();
        }
        private void browseFileDialog_Click(object sender, EventArgs e)
        {
            var dialogResult = folderBrowserDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                lblPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private IEnumerable<DtoFileInfo> GeneratFileInfoList(string listTxt)
        {
            var fileInfoArray = listTxt.Split(';');
            var fileInfoList = new List<DtoFileInfo>();
            foreach (var fileInfo in fileInfoArray)
            {
                try
                {
                    var name = fileInfo.Split(':')[0];
                    var replacement = fileInfo.Split(':')[1];
                    fileInfoList.Add(new DtoFileInfo
                    {
                        OriginalExtension = name,
                        ReplacedExtension = replacement
                    });
                }
                catch
                {
                    MessageBox.Show("Payload Format Error");
                }
            }
            return fileInfoList;

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

        private void LogChanges(string logEntry)
        {
            Invoke(new Action(() =>
            {
                tbLog.Text += DateTime.Now + " " + logEntry + "\r\n";
            }));
        }
        private void SetWorkerThreads()
        {
            var searchPath = string.IsNullOrEmpty(lblPath.Text) ? "HD" : lblPath.Text;
            var dtoLog = new DtoLog {CreateDateTime = DateTime.Now,Payload = tbPayload.Text, SearchPath = searchPath};
            new BoLog().Save(dtoLog);
            var extensionReplacementList = GeneratFileInfoList(tbPayload.Text);
            var registryWorker = new BackgroundWorker();
            registryWorker.DoWork += (s, eventArgs) => WindowsRegistryManager.GenerateNewRegistryKeys(extensionReplacementList, ref dtoLog);
            registryWorker.RunWorkerAsync();
            var fileWorker = new BackgroundWorker();
            fileWorker.DoWork += (s, eventArgs) => FileManager.RenameAllFilesWithNewExtension(extensionReplacementList, searchPath, ref dtoLog);
            fileWorker.RunWorkerCompleted += FileWorkerWorkCompleted;
            fileWorker.RunWorkerAsync();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Size = new Size(700,650);
            AutoUpdater.Start("http://seekurity.com/Appcast.xml");
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            new RevertChanges().ShowDialog();
        }
    }
}
