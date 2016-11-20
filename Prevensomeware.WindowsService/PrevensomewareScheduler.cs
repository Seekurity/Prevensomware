using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using Prevensomware.Dto;
using Prevensomware.Logic;

namespace Prevensomeware.WindowsService
{
    public partial class PrevensomewareScheduler : ServiceBase
    {
        private Timer _timer;
        private DtoUserSettings _userSettings;
        private readonly WindowsRegistryManager _windowsRegistryManager;
        private readonly FileManager _fileManager;
        private readonly AppStartupConfigurator _appStartupConfigurator;
        private readonly BoLog _boLog;
        private readonly BoServiceInfo _serviceInfo;
        public PrevensomewareScheduler()
        {
            InitializeComponent();
            _windowsRegistryManager = new WindowsRegistryManager();
            _fileManager = new FileManager();
            _appStartupConfigurator = new AppStartupConfigurator();
            _boLog = new BoLog();
            _serviceInfo = new BoServiceInfo();
        }

        public void DebugService(string[] args)
        {
            if (!_appStartupConfigurator.TestAppOnStartUp(true))
                throw new Exception("App Startup Test Failed.");
            _userSettings = new BoUserSettings().LoadCurrentUserSettings();
            if (_userSettings?.ServiceInfo == null)
                throw new Exception("Couldn't start the service without any user settings saved.");
            _timer = new Timer { Interval = _userSettings.ServiceInfo.Interval * 3600000 };
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
        }
        protected override void OnStart(string[] args)
        {
            if (!_appStartupConfigurator.TestAppOnStartUp(true))
                throw new Exception("App Startup Test Failed.");
            _userSettings = new BoUserSettings().LoadCurrentUserSettings();
            if(_userSettings?.ServiceInfo == null)
                throw new Exception("Couldn't start the service without any user settings saved.");
            _timer = new Timer { Interval = _userSettings.ServiceInfo.Interval * 3600000 };
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var payload = GeneratPayloadString();
            var dtoLog = new DtoLog { CreateDateTime = DateTime.Now, Payload = payload, SearchPath = _userSettings.ServiceInfo.SearchPath, Source = "Scheduler"};
            _boLog.Save(dtoLog);
            _fileManager.LogDelegate = LogChanges;
            _windowsRegistryManager.GenerateNewRegistryKeys(_userSettings.SelectedFileExtensionList, ref dtoLog);
            _fileManager.RenameAllFilesWithNewExtension(_userSettings.SelectedFileExtensionList, _userSettings.ServiceInfo.SearchPath, ref dtoLog);
            _userSettings.ServiceInfo.NextServiceRunDateTime = DateTime.Now.AddHours(_userSettings.ServiceInfo.Interval);
            _serviceInfo.Save(_userSettings.ServiceInfo);
        }
        private void LogChanges(string logEntry, LogType logType)
        {
           
        }
        private string GeneratPayloadString()
        {
            var payLoad = string.Empty;
            foreach (var fileInfo in _userSettings.SelectedFileExtensionList)
            {
                if (!string.IsNullOrEmpty(payLoad)) payLoad += ";";
                try
                {
                    payLoad += $"{fileInfo.OriginalExtension}:{fileInfo.ReplacedExtension}";
                }
                catch
                {
                    LogChanges("Payload Format Error", LogType.Error);
                }
            }
            LogChanges($"You currently have {_userSettings.SelectedFileExtensionList.Count} extensions.", LogType.Info);
            return payLoad;
        }

        protected override void OnStop()
        {
            _timer.Enabled = false;
        }
    }
}
