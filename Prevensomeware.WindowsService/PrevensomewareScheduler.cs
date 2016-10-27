using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Timers;
using Prevensomware.Dto;
using Prevensomware.Logic;

namespace Prevensomeware.WindowsService
{
    public partial class PrevensomewareScheduler : ServiceBase
    {
        private Timer timer;
        private string payLoad;
        private string searchPath;
        public PrevensomewareScheduler()
        {
            InitializeComponent();
        }

        public void myDebug(string[] args)
        {
            payLoad = args[1];
            searchPath = args[2];
            var fileInfoList = GenerateFileInfoList(payLoad);
            var dtoLog = new DtoLog { CreateDateTime = DateTime.Now, Payload = payLoad, SearchPath = searchPath };
            new BoLog().Save(dtoLog);
            FileManager.LogDelegate = LogChanges;
            WindowsRegistryManager.GenerateNewRegistryKeys(fileInfoList, ref dtoLog);
            FileManager.RenameAllFilesWithNewExtension(fileInfoList, searchPath, ref dtoLog);

        }
        protected override void OnStart(string[] args)
        {
            payLoad = args[1];
            searchPath = args[2];
            timer = new Timer { Interval = int.Parse(args[0]) * 3600000 };
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var fileInfoList = GenerateFileInfoList(payLoad);
            var dtoLog = new DtoLog { CreateDateTime = DateTime.Now, Payload = payLoad, SearchPath = searchPath };
            new BoLog().Save(dtoLog);
            FileManager.LogDelegate = LogChanges;
            WindowsRegistryManager.GenerateNewRegistryKeys(fileInfoList, ref dtoLog);
            FileManager.RenameAllFilesWithNewExtension(fileInfoList, searchPath, ref dtoLog);
        }
        private void LogChanges(string logEntry)
        {
           
        }
        private IEnumerable<DtoFileInfo> GenerateFileInfoList(string listTxt)
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
                }
            }
            return fileInfoList;

        }

        protected override void OnStop()
        {
            timer.Enabled = false;
        }
    }
}
