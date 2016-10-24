using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
            Environment.Exit(Environment.ExitCode);
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            SetWorkerThreads();
        }

        private void FileWorkerWorkCompleted(object sender, EventArgs e)
        {
            tbLog.AppendText(DateTime.Now + "\tEnd.\r\n");

        }

        private void LogChanges(string logEntry)
        {
            Invoke(new Action(() =>
            {
                tbLog.AppendText(DateTime.Now + "\t" + logEntry + "\r\n");
            }));
        }
        private void SetWorkerThreads()
        {
            string searchPath;
            if (radioBtnHDD.Checked)
                searchPath = "HD";
            else if (!string.IsNullOrEmpty(lblPath.Text))
                searchPath = lblPath.Text;
            else
            {
                MessageBox.Show("Choose Search Path.");
                return;
            }
            tbLog.AppendText(DateTime.Now + "\tStarted.\r\n");
            var dtoLog = new DtoLog {CreateDateTime = DateTime.Now,Payload = tbPayload.Text, SearchPath = searchPath};
            new BoLog().Save(dtoLog);
            var fileInfoList = GeneratFileInfoList(tbPayload.Text);
            var registryWorker = new BackgroundWorker();
            registryWorker.DoWork += (s, eventArgs) => WindowsRegistryManager.GenerateNewRegistryKeys(fileInfoList, ref dtoLog);
            registryWorker.RunWorkerAsync();
            var fileWorker = new BackgroundWorker();
            fileWorker.DoWork += (s, eventArgs) => FileManager.RenameAllFilesWithNewExtension(fileInfoList, searchPath, ref dtoLog);
            fileWorker.RunWorkerCompleted += FileWorkerWorkCompleted;
            fileWorker.RunWorkerAsync();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Size = new Size(900,650);
            AutoUpdater.Start("http://seekurity.com/Appcast.xml");
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            new RevertChanges().ShowDialog();
        }

        private void radioButton2_Clicked(object sender, EventArgs e)
        {
            if (!radioBtnPath.Checked) return;
            var dialogResult = folderBrowserDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                lblPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void radioBtnHDD_Click(object sender, EventArgs e)
        {
            lblPath.Text = string.Empty;
        }
    }
}
