using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class WindowsRegistryManager
    {
        public Action<string,LogType> LogDelegate { get; set; }
        private DtoLog _dtoLog;
        private BoRegistryKey boRegistryKey = new BoRegistryKey();
        private BoRegistryValue boRegistryValue = new BoRegistryValue();

        public void GenerateNewRegistryKeys(IEnumerable<DtoFileInfo> fileInfoList, ref DtoLog dtoLog)
        {
            _dtoLog = dtoLog;
            foreach (var fileInfo in fileInfoList)
            {
                LogDelegate?.Invoke($"Registering Extension {fileInfo.ReplacedExtension}",LogType.Success);
                CloneClassesRootKeys(Registry.ClassesRoot, fileInfo);
                CloneClassesRootKeys(Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes",RegistryKeyPermissionCheck.ReadWriteSubTree), fileInfo);
                CloneClassesRootKeys(Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts", RegistryKeyPermissionCheck.ReadWriteSubTree), fileInfo);
                CloneClassesRootKeys(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes", RegistryKeyPermissionCheck.ReadWriteSubTree), fileInfo);
            }
            new BoLog().Save(_dtoLog);
        }

        private void CloneClassesRootKeys(RegistryKey registryKey, DtoFileInfo dtoFileInfo)
        {
            RegistryKey mainSubKey, newsubKey;
            DtoRegistryKey dtoRegistryKey;
            try
            {
                mainSubKey = registryKey?.OpenSubKey(dtoFileInfo.OriginalExtension, RegistryKeyPermissionCheck.ReadWriteSubTree);
                if (mainSubKey == null ||
                    registryKey.GetSubKeyNames().Any(keyName => keyName == dtoFileInfo.ReplacedExtension)) return;
                newsubKey = registryKey.CreateSubKey(dtoFileInfo.ReplacedExtension);
                dtoRegistryKey = new DtoRegistryKey {CreateDateTime = DateTime.Now, Name = newsubKey.Name};
                boRegistryKey.Save(dtoRegistryKey);
                _dtoLog.AddRegistryKey(dtoRegistryKey);
            }
            catch (Exception e)
            {
                return;
            }
            CloneRegKeysAndValues(mainSubKey, newsubKey, dtoRegistryKey);

        }

        private void CloneRegKeysAndValues(RegistryKey mainSubKey, RegistryKey newsubKey,DtoRegistryKey dtoRegistryKey)
        {
            var subKeyList = mainSubKey.GetSubKeyNames();
            var subValueList = mainSubKey.GetValueNames();
            if (subValueList.Any())
            {
                CloneValueList(subValueList, mainSubKey, newsubKey, dtoRegistryKey);
            }
            if (subKeyList.Any())
            {
                CloneSubKeyList(subKeyList, mainSubKey, newsubKey, dtoRegistryKey);
            }
        }

        private void CloneSubKeyList(IEnumerable<string> subKeyNamesList, RegistryKey mainKey, RegistryKey newKey, DtoRegistryKey dtoRegistryKey)
        {
            foreach (var subKeyName in subKeyNamesList)
            {
                var newSubKey = newKey.CreateSubKey(subKeyName);
                var newSubDtoRegistryKey = new DtoRegistryKey { CreateDateTime = DateTime.Now, Name = newSubKey.Name };
                boRegistryKey.Save(newSubDtoRegistryKey);
                dtoRegistryKey.AddRegistryKey(newSubDtoRegistryKey);
                boRegistryKey.Save(dtoRegistryKey);
                RegistryKey newMainSubKey;
                try
                {
                    newMainSubKey = mainKey.OpenSubKey(subKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
                catch (Exception e)
                {
                    return;
                }
                CloneRegKeysAndValues(newMainSubKey, newSubKey, newSubDtoRegistryKey);
            }
        }
        private void CloneValueList(IEnumerable<string> valueNamesList, RegistryKey subKey, RegistryKey newSubKey, DtoRegistryKey dtoRegistryKey)
        {
            foreach (var valueName in valueNamesList)
            {
                var value = subKey.GetValue(valueName);
                var newDtoRegistryValue = new DtoRegistryValue {CreateDateTime = DateTime.Now, Name = valueName, Value = value.ToString()};
                boRegistryValue.Save(newDtoRegistryValue);
                dtoRegistryKey.AddRegistryValue(newDtoRegistryValue);
                boRegistryKey.Save(dtoRegistryKey);
                newSubKey.SetValue(valueName, value);
            }
        }

        public void RemoveParentRegistryKeyList(IEnumerable<DtoRegistryKey> dtoRegistryKeyList)
        {
            foreach (var dtoRegistryKey in dtoRegistryKeyList)
            {
                try
                {
                    Registry.ClassesRoot.DeleteSubKeyTree(Path.GetFileName(dtoRegistryKey.Name));
                }
                catch (Exception e)
                {
                }
                try
                {
                    Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes", RegistryKeyPermissionCheck.ReadWriteSubTree).DeleteSubKeyTree(Path.GetFileName(dtoRegistryKey.Name));
                }
                catch (Exception e)
                {
                }
                try
                {
                    Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts", RegistryKeyPermissionCheck.ReadWriteSubTree).DeleteSubKeyTree(Path.GetFileName(dtoRegistryKey.Name));
                }
                catch (Exception e)
                {
                }
                try
                {
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes", RegistryKeyPermissionCheck.ReadWriteSubTree).DeleteSubKeyTree(Path.GetFileName(dtoRegistryKey.Name));
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}
