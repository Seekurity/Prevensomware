using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public static class WindowsRegistryManager
    {
        private static DtoLog _dtoLog;
        private static BoRegistryKey boRegistryKey = new BoRegistryKey();
        private static BoRegistryValue boRegistryValue = new BoRegistryValue();

        public static void GenerateNewRegistryKeys(IEnumerable<DtoFileInfo> fileInfoList, ref DtoLog dtoLog)
        {
            _dtoLog = dtoLog;
            foreach (var fileInfo in fileInfoList)
            {
                CloneClassesRootKeys(Registry.ClassesRoot, fileInfo);
                CloneClassesRootKeys(Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes",true), fileInfo);
                CloneClassesRootKeys(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes",true), fileInfo);
            }
            new BoLog().Save(_dtoLog);
        }

        private static void CloneClassesRootKeys(RegistryKey registryKey, DtoFileInfo dtoFileInfo)
        {
            var mainSubKey = registryKey.OpenSubKey(dtoFileInfo.OriginalExtension, true);
            if (mainSubKey == null || registryKey.GetSubKeyNames().Any(keyName => keyName == dtoFileInfo.ReplacedExtension)) return;
            var newsubKey = registryKey.CreateSubKey(dtoFileInfo.ReplacedExtension);
            var dtoRegistryKey = new DtoRegistryKey {CreateDateTime = DateTime.Now, Name = newsubKey.Name};
            boRegistryKey.Save(dtoRegistryKey);
            _dtoLog.AddRegistryKey(dtoRegistryKey);
            CloneRegKeysAndValues(mainSubKey, newsubKey, dtoRegistryKey);
        }

        private static void CloneRegKeysAndValues(RegistryKey mainSubKey, RegistryKey newsubKey,DtoRegistryKey dtoRegistryKey)
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

        private static void CloneSubKeyList(IEnumerable<string> subKeyNamesList, RegistryKey mainKey, RegistryKey newKey, DtoRegistryKey dtoRegistryKey)
        {
            foreach (var subKeyName in subKeyNamesList)
            {
                var newSubKey = newKey.CreateSubKey(subKeyName);
                var newSubDtoRegistryKey = new DtoRegistryKey { CreateDateTime = DateTime.Now, Name = newSubKey.Name };
                boRegistryKey.Save(newSubDtoRegistryKey);
                dtoRegistryKey.AddRegistryKey(newSubDtoRegistryKey);
                boRegistryKey.Save(dtoRegistryKey);
                var newMainSubKey = mainKey.OpenSubKey(subKeyName, true);
                CloneRegKeysAndValues(newMainSubKey, newSubKey, newSubDtoRegistryKey);
            }
        }
        private static void CloneValueList(IEnumerable<string> valueNamesList, RegistryKey subKey, RegistryKey newSubKey, DtoRegistryKey dtoRegistryKey)
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

        public static void RemoveParentRegistryKeyList(IEnumerable<DtoRegistryKey> dtoRegistryKeyList)
        {
            foreach (var dtoRegistryKey in dtoRegistryKeyList)
            {
                try
                {
                    Registry.ClassesRoot.DeleteSubKeyTree(Path.GetFileName(dtoRegistryKey.Name));
                }catch{}
                try
                {
                    Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes",true).DeleteSubKeyTree(Path.GetFileName(dtoRegistryKey.Name));
                }catch{}
                try
                {
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes",true).DeleteSubKeyTree(Path.GetFileName(dtoRegistryKey.Name));
                }catch{}
            }
        }
    }
}
