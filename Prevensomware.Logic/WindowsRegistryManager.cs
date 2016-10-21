using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace Prevensomware.Logic
{
    public static class WindowsRegistryManager
    {
        public static void GenerateNewRegistryKeys(IEnumerable<ExtensionReplacement> extensionReplacementList)
        {
            foreach (var extReplacement in extensionReplacementList)
            {
                CloneClassesRootKeys(Registry.ClassesRoot, extReplacement);
                CloneClassesRootKeys(Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes",true), extReplacement);
                CloneClassesRootKeys(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes",true), extReplacement);
            }
        }

        private static void CloneClassesRootKeys(RegistryKey registryKey, ExtensionReplacement extensionReplacement)
        {
            var mainSubKey = registryKey.OpenSubKey(extensionReplacement.Name, true);
            if (mainSubKey == null || registryKey.GetSubKeyNames().Any(keyName => keyName == extensionReplacement.Replacement)) return;
            var newsubKey = registryKey.CreateSubKey(extensionReplacement.Replacement);
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
