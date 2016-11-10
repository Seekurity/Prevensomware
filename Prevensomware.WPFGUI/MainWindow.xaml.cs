using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using AutoUpdaterDotNET;
using Microsoft.Win32;
using Prevensomware.Dto;
using Prevensomware.Logic;
using Application = System.Windows.Application;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace Prevensomware.WPFGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private DtoLog _dtoLog;
        private readonly BoLog _boLog = new BoLog();
        public ObservableCollection<DtoLog> logList;
        public ObservableCollection<DtoFileInfo> selectedUserextensionList;
        private string _searchPath;
        private string _chosenSearchPath;
        private readonly FileManager _fileManager;
        private readonly WindowsRegistryManager _windowsRegistryManager;
        private FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();
        private string _payLoad = string.Empty;
        private readonly AppStartupConfigurator _appConfigurator = new AppStartupConfigurator();
        public DtoUserSettings userSettings;
        private readonly BoFileInfo _boFileInfo;
        private readonly BoUserSettings _boUserSettings;
        private readonly WindowsServiceManager _windowsServiceManager;
        private Timer _timerTxtServiceUpdater;
        public void RegisterAppInWindowsRegistry()
        {
            if (Registry.GetValue(@"HKEY_CLASSES_ROOT\*\shell\Revert File\command", "", null) != null) return;
            try
            {
                var registryKey = Registry.ClassesRoot.OpenSubKey("*").OpenSubKey("shell", true);
                registryKey = registryKey.CreateSubKey("Revert File");
                registryKey = registryKey.CreateSubKey("command");
                registryKey.SetValue("", System.Reflection.Assembly.GetExecutingAssembly().Location + " \"%1\"");
            }
            catch { }
        }
        public MainWindow()
        {
            _fileManager = new FileManager();
            _windowsRegistryManager = new WindowsRegistryManager();
            _boFileInfo = new BoFileInfo();
            _windowsServiceManager = new WindowsServiceManager();
            _boUserSettings = new BoUserSettings();
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                ProcessCommandFromWindowsContextMenu(args[1]);
            }
            InitializeComponent();
            _fileManager.LogDelegate = LogChanges;
            _windowsRegistryManager.LogDelegate = LogChanges;
            _appConfigurator.LogDelegate = LogChanges;
            _windowsServiceManager.LogDelegate = LogChanges;
            AutoUpdater.Start("http://seekurity.com/Appcast.xml");
            var backgroundWorker = new BackgroundWorker();
            SetAllButtonsEnabledState(false);
            _timerTxtServiceUpdater = new Timer {Interval = 60000 };
            _timerTxtServiceUpdater.Tick += _timerTxtServiceUpdater_Tick;
            _timerTxtServiceUpdater.Enabled = true;
            backgroundWorker.DoWork += (s, eventArgs) =>
            {
                SetAllButtonsEnabledState(_appConfigurator.TestAppOnStartUp());
                LoadUserSettingsInfo();

                logList = new ObservableCollection<DtoLog>(_boLog.GetList());
                Dispatcher.Invoke(new Action(() => dataGridLogs.ItemsSource = logList));
            };

            backgroundWorker.RunWorkerAsync();
        }

        private void _timerTxtServiceUpdater_Tick(object sender, EventArgs e)
        {
            var timeDiffInMinutes = userSettings.ServiceInfo.NextServiceRunDateTime.Subtract(DateTime.Now).TotalMinutes;
            if (timeDiffInMinutes < 0)
            {
                userSettings = _boUserSettings.LoadCurrentUserSettings();
                timeDiffInMinutes = userSettings.ServiceInfo.NextServiceRunDateTime.Subtract(DateTime.Now).TotalMinutes;
            }
            Dispatcher.Invoke(new Action(() =>
            {
                if (LabelServiceStatus.Content.ToString() == ServiceState.Stopped.ToString())
                    LabelServiceCurrentInterval.Content = 0;
                LabelNextServiceRun.Content = timeDiffInMinutes < 0 ? 0 : (int) timeDiffInMinutes;
            }));
        }

        private void LoadUserSettingsInfo()
        {
            LogChanges("Loading user settings.", LogType.Info);
            try
            {
                var currentUserSettings = new BoUserSettings().LoadCurrentUserSettings();
                if (currentUserSettings == null)
                {
                    LogChanges("No user settings were found. Generating default user settings.", LogType.Info);
                    currentUserSettings = _appConfigurator.GenerateInitialUserSettings();
                }
                userSettings = currentUserSettings;
                selectedUserextensionList = new ObservableCollection<DtoFileInfo>(userSettings.SelectedFileExtensionList);
                Dispatcher.Invoke(new Action(() =>
                {
                    ListExtensions.ItemsSource = selectedUserextensionList;
                    SetServiceLabels();
                }));
            }
            catch
            {
                    LogChanges("Failed to load user settings.", LogType.Error);
            }
            LogChanges("End", LogType.Info);
        }
        private void SetAllButtonsEnabledState(bool isEnabled)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                BtnStart.IsEnabled = isEnabled;
                BtnAddExtension.IsEnabled = isEnabled;
                BtnClearLog.IsEnabled = isEnabled;
                BtnEnd.IsEnabled = isEnabled;
                BtnRemoveExtension.IsEnabled = isEnabled;
                BtnRevertAll.IsEnabled = isEnabled;
                BtnRevertSelected.IsEnabled = isEnabled;
                BtnSetServiceInterval.IsEnabled = isEnabled;
            }));
        }
        private void ProcessCommandFromWindowsContextMenu(string filePath)
        {
            var isReverted = new BoFileInfo().RevertForPath(filePath);
            MessageBox.Show(isReverted ? "File reverted succecssfully." : "Couldn't revert the file.");
            Environment.Exit(Environment.ExitCode);
        }

        private void GeneratPayloadString()
        {
            _payLoad = string.Empty;
            foreach (var fileInfo in ListExtensions.Items)
            {
                if(!string.IsNullOrEmpty(_payLoad)) _payLoad+= ";";
                try
                {
                    _payLoad += $"{((DtoFileInfo)fileInfo).OriginalExtension}:{((DtoFileInfo)fileInfo).ReplacedExtension}";
                }
                catch
                {
                    MessageBox.Show("Payload Format Error");
                }
            }
            LogChanges($"You currently have {userSettings.SelectedFileExtensionList.Count} extensions.", LogType.Info);
        }
       
        private void FileWorkerWorkCompleted(object sender, EventArgs e)
        {
            TxtLog.AppendText(DateTime.Now + "\tEnd.\r\n");
            TxtLog.ScrollToEnd();
            BtnStart.IsEnabled = true;
            logList.Add(_dtoLog);
            Dispatcher.Invoke(new Action(SetServiceLabels));
        }
        private void WindowsRegistryManagerWorkCompleted(object sender, EventArgs e)
        {
            var fileWorker = new BackgroundWorker();
            fileWorker.DoWork += (s, eventArgs) => _fileManager.RenameAllFilesWithNewExtension(userSettings.SelectedFileExtensionList, _searchPath, ref _dtoLog);
            fileWorker.RunWorkerCompleted += FileWorkerWorkCompleted;
            fileWorker.RunWorkerAsync();
        }
        private void LogChanges(string logEntry,LogType logType)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                string color;
                switch (logType)
                {
                    case LogType.Success:
                        color = "green";
                        break;
                    case LogType.Error:
                        color = "red";
                        break;
                    default:
                        color = "white";
                        break;
                }
                TxtLog.AppendText(DateTime.Now + "\t" + logEntry + "\r\n", color);
                TxtLog.ScrollToEnd();
            }));
        }
        
        private void SetWorkerThreads()
        {
            string searchPath;
            if (RadioBtnHdd.IsChecked.Value)
                searchPath = "HD";
            else if (!string.IsNullOrEmpty(_chosenSearchPath))
                searchPath = _chosenSearchPath;
            else
            {
                MessageBox.Show("Choose Search Path.");
                return;
            }
            BtnStart.IsEnabled = false;
            TxtLog.AppendText(DateTime.Now + "\tStarted.\r\n");
            GeneratPayloadString();
            var dtoLog = new DtoLog { CreateDateTime = DateTime.Now, Payload = _payLoad, SearchPath = searchPath };
            new BoLog().Save(dtoLog);
            _dtoLog = dtoLog;
            _searchPath = searchPath;
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (s, eventArgs) =>
            {
                userSettings.SearchPath = searchPath;
                _boUserSettings.Save(userSettings);
                _windowsServiceManager.StartService(userSettings.ServiceInfo);
                _windowsRegistryManager.GenerateNewRegistryKeys(userSettings.SelectedFileExtensionList, ref dtoLog);
            };
            backgroundWorker.RunWorkerCompleted += WindowsRegistryManagerWorkCompleted;
            backgroundWorker.RunWorkerAsync();
        }

        private void SetServiceLabels()
        {
            var serviceState = _windowsServiceManager.GetServiceState(userSettings.ServiceInfo);
            LabelServiceStatus.Content = serviceState.ToString();
            LabelServiceStatus.Foreground = serviceState == ServiceState.Running ?
            new SolidColorBrush(Color.FromRgb(0, 153, 0)) : new SolidColorBrush(Color.FromRgb(204, 0, 0));
            LabelServiceCurrentInterval.Content = userSettings.ServiceInfo.Interval;
            var timeDiffInMinutes =
                userSettings.ServiceInfo.NextServiceRunDateTime.Subtract(DateTime.Now).TotalMinutes;
            LabelNextServiceRun.Content = timeDiffInMinutes < 0 ? 0 : (int)timeDiffInMinutes;
            _timerTxtServiceUpdater.Start();
        }
        private void MainFrm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void BtnEnd_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var numericRegex = new Regex("[^0-9]+");
            e.Handled = numericRegex.IsMatch(e.Text);
        }

        private void RadioBtnHdd_Click(object sender, RoutedEventArgs e)
        {
            _chosenSearchPath = string.Empty;
            LogChanges("Chosen search path: Hard Drive.", LogType.Info);
        }

        private void RadioBtnPath_Click(object sender, RoutedEventArgs e)
        {
            if (!RadioBtnPath.IsChecked.Value) return;
            var dialogResult = _folderBrowserDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                _chosenSearchPath = _folderBrowserDialog.SelectedPath;
                LogChanges($"Chosen search path: {_chosenSearchPath}.", LogType.Info);
            }
            else
                RadioBtnPath.IsChecked = false;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            SetWorkerThreads();
        }

        private void BtnAddExtension_Click(object sender, RoutedEventArgs e)
        {
            AddNewExtension();
        }

        private void AddNewExtension()
        {
            if (userSettings.SelectedFileExtensionList.Any(x => string.Equals(x.OriginalExtension, TxtExtension.Text, StringComparison.CurrentCultureIgnoreCase)) || string.IsNullOrEmpty(TxtExtension.Text)) return;
            if (!TxtExtension.Text.StartsWith(".") || TxtExtension.Text.Count(c => c == '.') > 1) return;
            var insertedFile = _boFileInfo.InsertNewFileInfo(TxtExtension.Text, ref userSettings);
            selectedUserextensionList.Insert(0, insertedFile);
            GeneratPayloadString();
            TxtExtension.Clear();
        }
        private void BtnRemoveExtension_Click(object sender, RoutedEventArgs e)
        {
            if (!userSettings.SelectedFileExtensionList.Any(x => string.Equals(x.OriginalExtension, TxtExtension.Text, StringComparison.CurrentCultureIgnoreCase)) || string.IsNullOrEmpty(TxtExtension.Text)) return;
            var extensionToRemove = userSettings.SelectedFileExtensionList.Single(x=>string.Equals(x.OriginalExtension,TxtExtension.Text,StringComparison.InvariantCultureIgnoreCase));
            _boFileInfo.RemoveFileInfo(extensionToRemove, ref userSettings);
            selectedUserextensionList.Remove(extensionToRemove);
            GeneratPayloadString();
            TxtExtension.Clear();
        }
        private void BtnClearLog_Click(object sender, RoutedEventArgs e)
        {
            TxtLog.SelectAll();
            TxtLog.Selection.Text = string.Empty;
            TxtLog.Document.PageWidth = 5000;
            var newParagraph = new Paragraph {Margin = new Thickness(0,0,0,0)};
            TxtLog.Document.Blocks.Add(newParagraph);
        }

        private void BtnRevertSelected_Click(object sender, RoutedEventArgs e)
        {
            var backgroundWorker = new BackgroundWorker();
            var gridSelectedItems = dataGridLogs.SelectedItems;
            backgroundWorker.DoWork += (s, eventArgs) =>
            {
                var revertedChange = false;
                foreach (DtoLog dtoLog in gridSelectedItems)
                {
                    if (dtoLog.IsReverted)
                    {
                        LogChanges("Can't revert a reverted log.", LogType.Error);
                        return;
                    }
                    revertedChange = true;
                    _boLog.Revert(dtoLog);
                    logList[logList.IndexOf(logList.Single(x => x.Oid == dtoLog.Oid))].IsReverted = true;
                }
                if (revertedChange)
                    LogChanges("Changes are reverted.", LogType.Success);
                else
                    LogChanges("Couldn't revert any changes.", LogType.Error);
            };
            backgroundWorker.RunWorkerAsync();
        }

        private void BtnRevertAll_Click(object sender, RoutedEventArgs e)
        {
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (s, eventArgs) =>
            {
                _boLog.RevertAll();
            };
            backgroundWorker.RunWorkerCompleted += ChangesRevertedSucessfully;
            backgroundWorker.RunWorkerAsync();
        }

        private void ChangesRevertedSucessfully(object sender, EventArgs e)
        {
            foreach (var dtoLog in logList)
                dtoLog.IsReverted = true;
            Dispatcher.Invoke(new Action(() => LogChanges("Changes are reverted.", LogType.Success)));
        }

        private void dataGridLogs_LostFocus(object sender, RoutedEventArgs e)
        {
            dataGridLogs.RowBackground = new SolidColorBrush(Color.FromArgb(255,34,34,36));
        }

        private void ListExtensions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count == 1)
            {
                TxtExtension.Text = ((DtoFileInfo)e.AddedItems[0]).OriginalExtension;
            }
        }

        private void TxtExtension_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddNewExtension();
            }
        }

        private void BtnSetServiceInterval_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtServiceInterval.Text)) return;
            var serviceInterval = int.Parse(TxtServiceInterval.Text);
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (o, args) =>
            {
                userSettings.ServiceInfo.Interval = serviceInterval;
                userSettings.ServiceInfo.NextServiceRunDateTime = DateTime.Now.AddHours(serviceInterval);
                _boUserSettings.Save(userSettings);
                LogChanges($"Setting Prevensomware Scheduler Hour Interval To: {serviceInterval}", LogType.Info);
                _windowsServiceManager.StartService(userSettings.ServiceInfo);
                Dispatcher.Invoke(new Action(SetServiceLabels));
            };
            backgroundWorker.RunWorkerAsync();
            TxtServiceInterval.Text = string.Empty;
        }
    }
    
}
