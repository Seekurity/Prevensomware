using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Prevensomware.DA;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public static class FileManager
    {
        public static Action<string> LogDelegate { get; set; } 
        public static void RenameAllFilesWithNewExtensionForCertainPath(IEnumerable<DtoFileInfo> extensionReplacementList, string directoryPath)
        {
            //foreach (var extensionReplacement in extensionReplacementList)
            //{
            //    var allFilesArray = Directory.GetFiles(directoryPath, "*" + extensionReplacement.OriginalExtension, SearchOption.AllDirectories);
            //    LogDelegate(string.Format("Found {0} Files.", allFilesArray.Count()));
            //    RenameFileList(allFilesArray, extensionReplacement.ReplacedExtension);
            //}
            var newLog = new DtoLog
            {
                CreateDateTime =  DateTime.Now,
                Payload = "ewew:ewew",
                RegistryKeyList = new List<DtoRegistryKey>
                {
                    new DtoRegistryKey {Name = "ewew"}
                }
            };
            var logrep = new LogRepository();
            logrep.CreateOrUpdate(newLog);
            var all = logrep.GetList();
            var repo = new FileInfoRepository();
           repo.CreateOrUpdate(new DtoFileInfo());
            var ww = repo.GetList();
        }

        private static void RenameFileList(IEnumerable<string> filePathList, string newExtension)
        {
            foreach (var filePath in filePathList)
            {
                var newPath = Path.ChangeExtension(filePath, newExtension);
                File.Move(filePath, newPath);
                LogDelegate(string.Format("File {0} changed to {1}.", filePath, newPath));
            }
        }
    }
}
