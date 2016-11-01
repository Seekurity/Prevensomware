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
        private Timer _timer;
        private string _payLoad;
        private string _searchPath;
        private readonly WindowsRegistryManager _windowsRegistryManager;
        private readonly FileManager _fileManager;
        private readonly AppStartupConfigurator _appStartupConfigurator;
        public PrevensomewareScheduler()
        {
            InitializeComponent();
            _windowsRegistryManager = new WindowsRegistryManager();
            _fileManager = new FileManager();
            _appStartupConfigurator = new AppStartupConfigurator();
        }

        public void DebugService(string[] args)
        {
            _payLoad = args[1];
            _searchPath = args[2];
            var fileInfoList = GenerateFileInfoList(_payLoad);
            var dtoLog = new DtoLog { CreateDateTime = DateTime.Now, Payload = _payLoad, SearchPath = _searchPath };
            new BoLog().Save(dtoLog);
            _fileManager.LogDelegate = LogChanges;
            _windowsRegistryManager.GenerateNewRegistryKeys(fileInfoList, ref dtoLog);
            _fileManager.RenameAllFilesWithNewExtension(fileInfoList, _searchPath, ref dtoLog);

        }
        protected override void OnStart(string[] args)
        {
            if (args.Length != 3)
                throw new Exception("Invalid Service arguments number.");
            if(!_appStartupConfigurator.TestAppOnStartUp())
                throw new Exception("App Startup Test Failed.");
            _payLoad = args[1];
            _searchPath = args[2];
            _timer = new Timer { Interval = int.Parse(args[0]) * 3600000 };
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var fileInfoList = GenerateFileInfoList(_payLoad);
            var dtoLog = new DtoLog { CreateDateTime = DateTime.Now, Payload = _payLoad, SearchPath = _searchPath };
            new BoLog().Save(dtoLog);
            _fileManager.LogDelegate = LogChanges;
            _windowsRegistryManager.GenerateNewRegistryKeys(fileInfoList, ref dtoLog);
            _fileManager.RenameAllFilesWithNewExtension(fileInfoList, _searchPath, ref dtoLog);
        }
        private void LogChanges(string logEntry, LogType logType)
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
            _timer.Enabled = false;
        }
    }
}
