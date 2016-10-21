using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public static class WindowsRegistryManager
    {
        public static void GenerateNewRegistryKeys(IEnumerable<DtoFileInfo> fileInfoList)
        {
            foreach (var fileInfo in fileInfoList)
            {
                CloneClassesRootKeys(Registry.ClassesRoot, fileInfo);
                CloneClassesRootKeys(Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes",true), fileInfo);
                CloneClassesRootKeys(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes",true), fileInfo);
            }
        }

        private static void CloneClassesRootKeys(RegistryKey registryKey, DtoFileInfo dtoFileInfo)
        {
            var mainSubKey = registryKey.OpenSubKey(dtoFileInfo.OriginalExtension, true);
            if (mainSubKey == null || registryKey.GetSubKeyNames().Any(keyName => keyName == dtoFileInfo.ReplacedExtension)) return;
            var newsubKey = registryKey.CreateSubKey(dtoFileInfo.ReplacedExtension);
            CloneRegKeysAndValues(mainSubKey, newsubKey);
        }

        private static void CloneRegKeysAndValues(RegistryKey mainSubKey, RegistryKey newsubKey)
        {
            var subKeyList = mainSubKey.GetSubKeyNames();
            var subValueList = mainSubKey.GetValueNames();
            if (subValueList.Any())
            {
                CloneValueList(subValueList, mainSubKey, newsubKey);
            }
            if (subKeyList.Any())
            {
                CloneSubKeyList(subKeyList, mainSubKey, newsubKey);
            }
        }

        private static void CloneSubKeyList(IEnumerable<string> subKeyNamesList, RegistryKey mainKey, RegistryKey newKey)
        {
            foreach (var subKeyName in subKeyNamesList)
            {
                var newSubKey = newKey.CreateSubKey(subKeyName);
                var newMainSubKey = mainKey.OpenSubKey(subKeyName, true);
                CloneRegKeysAndValues(newMainSubKey, newSubKey);
            }
        }
        private static void CloneValueList(IEnumerable<string> valueNamesList, RegistryKey subKey, RegistryKey newSubKey)
        {
            foreach (var valueName in valueNamesList)
            {
                var value = subKey.GetValue(valueName);
                newSubKey.SetValue(valueName, value);
            }
        }
    }
}
