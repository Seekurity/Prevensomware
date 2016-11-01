using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using AutoUpdaterDotNET;
using Microsoft.Win32;
using Prevensomware.Dto;
using Prevensomware.Logic;
using Application = System.Windows.Application;
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
        private IEnumerable<DtoFileInfo> _fileInfoList;
        private string _searchPath;
        private string _chosenSearchPath;
        private readonly FileManager _fileManager;
        private readonly WindowsRegistryManager _windowsRegistryManager;
        private FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();
        private string _payLoad = string.Empty;
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
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                ProcessCommandFromWindowsContextMenu(args[1]);
            }
            InitializeComponent();
            _fileManager.LogDelegate = LogChanges;
            _windowsRegistryManager.LogDelegate = LogChanges;
            AddDefaultExtensionList();
            AutoUpdater.Start("http://seekurity.com/Appcast.xml");
            var backgroundWorker = new BackgroundWorker();
            SetAllButtonsEnabledState(false);
            backgroundWorker.DoWork += (s, eventArgs) =>
            {
                var appConfigurator = new AppStartupConfigurator { LogDelegate = LogChanges };
                if (appConfigurator.TestAppOnStartUp())
                    SetAllButtonsEnabledState(true);
                logList = new ObservableCollection<DtoLog>(_boLog.GetList());
                Dispatcher.Invoke(new Action(() => dataGridLogs.ItemsSource = logList));
            };
            
            backgroundWorker.RunWorkerAsync();
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
        private void AddDefaultExtensionList()
        {
            ListExtensions.Items.Add(".txt");
            ListExtensions.Items.Add(".doc");
            ListExtensions.Items.Add(".docx");
            ListExtensions.Items.Add(".bat");
            ListExtensions.Items.Add(".xlsx");
            ListExtensions.Items.Add(".xls");
            ListExtensions.Items.Add(".ppt");
            ListExtensions.Items.Add(".png");
        }
        private void ProcessCommandFromWindowsContextMenu(string filePath)
        {
            var isReverted = new BoFileInfo().RevertForPath(filePath);
            MessageBox.Show(isReverted ? "File reverted succecssfully." : "Couldn't revert the file.");
            Environment.Exit(Environment.ExitCode);
        }

        private IEnumerable<DtoFileInfo> GeneratFileInfoList()
        {
            var fileInfoList = new List<DtoFileInfo>();
            _payLoad = string.Empty;
            foreach (var extension in ListExtensions.Items)
            {
                if(!string.IsNullOrEmpty(_payLoad)) _payLoad+= ";";
                try
                {
                    var name = extension.ToString();
                    var replacement = extension+"secured";
                    fileInfoList.Add(new DtoFileInfo
                    {
                        OriginalExtension = name,
                        ReplacedExtension = replacement
                    });
                    _payLoad += $"{name}:{replacement}";
                }
                catch
                {
                    MessageBox.Show("Payload Format Error");
                }
            }
            LogChanges($"You currently have {fileInfoList.Count} extensions.", LogType.Info);
            return fileInfoList;
        }
        private void FileWorkerWorkCompleted(object sender, EventArgs e)
        {
            TxtLog.AppendText(DateTime.Now + "\tEnd.\r\n");
            TxtLog.ScrollToEnd();
            BtnStart.IsEnabled = true;
            logList.Add(_dtoLog);
        }
        private void WindowsRegistryManagerWorkCompleted(object sender, EventArgs e)
        {
            var fileWorker = new BackgroundWorker();
            fileWorker.DoWork += (s, eventArgs) => _fileManager.RenameAllFilesWithNewExtension(_fileInfoList, _searchPath, ref _dtoLog);
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
            var fileInfoList = GeneratFileInfoList();
            var dtoLog = new DtoLog { CreateDateTime = DateTime.Now, Payload = _payLoad, SearchPath = searchPath };
            new BoLog().Save(dtoLog);
            _dtoLog = dtoLog;
            _searchPath = searchPath;
            _fileInfoList = fileInfoList;
            var registryWorker = new BackgroundWorker();
            registryWorker.DoWork += (s, eventArgs) => _windowsRegistryManager.GenerateNewRegistryKeys(fileInfoList, ref dtoLog);
            registryWorker.RunWorkerCompleted += WindowsRegistryManagerWorkCompleted;
            registryWorker.RunWorkerAsync();
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
            if (ListExtensions.Items.Contains(TxtExtension.Text) || string.IsNullOrEmpty(TxtExtension.Text)) return;
            ListExtensions.Items.Insert(0, TxtExtension.Text);
            GeneratFileInfoList();
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
            var revertedChange = false;
            foreach (DtoLog dtoLog in dataGridLogs.SelectedItems)
            {
                if (dtoLog.IsReverted)
                {
                    LogChanges("Can't revert a reverted log.", LogType.Error);
                    return;
                }
                revertedChange = true;
                _boLog.Revert(dtoLog);
                logList[logList.IndexOf(logList.Single(x => x.Oid == dtoLog.Oid))].IsReverted  = true;
            }
            if(revertedChange)
                LogChanges("Changes are reverted.", LogType.Success);
            else
                LogChanges("Couldn't revert any changes.", LogType.Error);
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

        private void BtnRemoveExtension_Click(object sender, RoutedEventArgs e)
        {
            if (!ListExtensions.Items.Contains(TxtExtension.Text)) return;
            ListExtensions.Items.Remove(TxtExtension.Text);
            GeneratFileInfoList();
            TxtExtension.Clear();
        }

        private void ListExtensions_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count == 1)
            {
                TxtExtension.Text = e.AddedItems[0].ToString();
            }
        }
    }
    
}
