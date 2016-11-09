using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Prevensomware.DA;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class AppStartupConfigurator
    {
        public Action<string,LogType> LogDelegate { get; set; }
        private static readonly Random Random = new Random();

        public DtoUserSettings GenerateInitialUserSettings()
        {
            var dtoUserSettings = new DtoUserSettings
            {
                ServiceInfo = GenerateInitialServiceInfo(),
                CreateDateTime = DateTime.Now
            };
            dtoUserSettings.SelectedFileExtensionList = GenerateInitialExtensionFileList(dtoUserSettings);
            new BoUserSettings().Save(dtoUserSettings);

            return dtoUserSettings;
        }

        private DtoServiceInfo GenerateInitialServiceInfo()
        {
            var serviceInfo = new DtoServiceInfo
            {
                CreateDateTime = DateTime.Now,
                Interval = 5,
                Name = "Prevensomware",
                NextServiceRunDateTime = DateTime.Now.AddHours(5)
            };
            new BoServiceInfo().Save(serviceInfo);
            return serviceInfo;
        }
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        private IList<DtoFileInfo> GenerateInitialExtensionFileList(DtoUserSettings dtoUserSettings)
        {
            var initialExtensionArray = new[] {"txt", "doc", "docx", "bat", "xlsx", "xls", "ppt", "png", "pps", "ppt", "mp4",
                "pot", "csv", "vcf", "zip", "rar", "avi", "pdf", "jpeg", "jpg", "bmp", "png", "chm","rtf","epub" };
            return initialExtensionArray.Select(extension => new DtoFileInfo
            {
                CreateDateTime = DateTime.Now, UserSettings = dtoUserSettings, OriginalExtension = "." + extension, ReplacedExtension = "." + RandomString(4)
            }).ToList();
        }
        public bool TestAppOnStartUp()
        {
            LogDelegate?.Invoke("Starting App Startup Test.", LogType.Info);
            var checkSucceeded = false;
            if (!IsDatabaseAccessible())
                LogDelegate?.Invoke("Startup Test: Couldn't access/create the databasse.", LogType.Error);
            else
            {
                LogDelegate?.Invoke("Startup Test: Database is accessible.", LogType.Success);
                checkSucceeded = true;
            }
            if (!IsAppPathInRegistryCorrect())
                LogDelegate?.Invoke("Startup Test: Couldn't read/write to Windows Registry.", LogType.Error);
            else
            {
                LogDelegate?.Invoke("Startup Test: Windows Registry is accessible.", LogType.Success);
                checkSucceeded = true;
            }
            if (!IsUserFilesAccessible())
                LogDelegate?.Invoke("Startup Test: Couldn't edit local Files.", LogType.Error);
            else
            {
                LogDelegate?.Invoke("Startup Test: Local Files are accessible.", LogType.Success);
                checkSucceeded = true;
            }
            return checkSucceeded;
        }

        private bool IsDatabaseAccessible()
        {
            try
            {
                var fileInfoRepository = new FileInfoRepository();
                var testFileInfoObject = new DtoFileInfo {CreateDateTime = DateTime.Now};
                fileInfoRepository.CreateOrUpdate(testFileInfoObject);
                fileInfoRepository.Remove(testFileInfoObject);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool IsAppPathInRegistryCorrect()
        {
            var currentAppPathValue = Registry.GetValue(@"HKEY_CLASSES_ROOT\*\shell\Revert File\command","",null);
            if (currentAppPathValue != null)
            {
                var appValueList = currentAppPathValue.ToString().Split(' ');
                var registerdAppExecutablePath = appValueList.First();
                if ((appValueList.Length > 1 && System.IO.Path.GetFullPath(registerdAppExecutablePath) != System.Reflection.Assembly.GetEntryAssembly().Location)
                     || appValueList.Length <= 1)
                {
                    try
                     {
                        Registry.ClassesRoot.OpenSubKey(@"*\shell\Revert File\command", true)
                            .SetValue("", System.Reflection.Assembly.GetEntryAssembly().Location + " \"%1\"");
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

            }
            else
                try
                {
                    var registryKey = Registry.ClassesRoot.OpenSubKey("*").OpenSubKey("shell", true);
                    registryKey = registryKey.CreateSubKey("Revert File");
                    registryKey = registryKey.CreateSubKey("command");
                    registryKey.SetValue("", System.Reflection.Assembly.GetEntryAssembly().Location + " \"%1\"");
                    return true;
                }
                catch
                {
                    return false;
                }
            return true;
        }
        private bool IsUserFilesAccessible()
        {
            try
            {
               System.IO.File.Create(
                    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                        "Prevensomware\\testFile.txt")).Close();
                System.IO.File.Delete(
                    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                        "Prevensomware\\testFile.txt"));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
