using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public static class FileManager
    {
        public static Action<string> LogDelegate { get; set; }
        private static DtoLog _dtoLog;
        public static void RenameAllFilesWithNewExtension(IEnumerable<DtoFileInfo> fileInfoList, string directoryPath, ref DtoLog dtoLog)
        {
            _dtoLog = dtoLog;
            foreach (var fileInfo in fileInfoList)
            {
                if (directoryPath == "HD") RenameFileListInWholeHardDrive(fileInfo);
                else RenameFileListForCertainPath(fileInfo,directoryPath);
            }
            new BoLog().Save(_dtoLog);
        }

        private static void RenameFileListInWholeHardDrive(DtoFileInfo fileInfo)
        {
            var fileList = new List<string>();
            foreach (var drive in DriveInfo.GetDrives().Where(x => x.IsReady))
            {
                try
                {
                    fileList.AddRange(GetFiles(drive.RootDirectory.FullName, "*"+fileInfo.OriginalExtension));
                }
                catch (Exception)
                {
                    LogDelegate("Couldn't change files in Drive "+ drive.Name);
                }
            }
            LogDelegate(fileList.Count + " File/s Found.");
            ChangeFileListExtensions(fileInfo, fileList);

        }
        public static IEnumerable<string> GetFiles(string root, string searchPattern)
        {
            var pending = new Stack<string>();
            var files = new List<string>();
            pending.Push(root);
            while (pending.Count != 0)
            {
                var path = pending.Pop();
                string[] next = null;
                try
                {
                    next = Directory.GetFiles(path, searchPattern);
                }
                catch
                {
                    LogDelegate("Couldn't Get Files in "+ path);
                }
                if (next != null && next.Length != 0)
                    foreach (var file in next) files.Add(file);
                try
                {
                    next = Directory.GetDirectories(path);
                    foreach (var subdir in next) pending.Push(subdir);
                }
                catch
                {
                    LogDelegate("Couldn't Get Directories in  "+ path);
                }
            }
            return files;
        }
        private static void RenameFileListForCertainPath(DtoFileInfo fileInfo, string directoryPath)
        {
            var allFilesArray = GetFiles(directoryPath , "*" + fileInfo.OriginalExtension);
            ChangeFileListExtensions(fileInfo, allFilesArray);
        }

        private static void ChangeFileListExtensions(DtoFileInfo fileInfo, IEnumerable<string> allFilesArray)
        {
            foreach (var filePath in allFilesArray)
            {
                var newPath = Path.ChangeExtension(filePath, fileInfo.ReplacedExtension);
                File.Move(filePath, newPath);
                fileInfo.OriginalPath = filePath;
                fileInfo.ReplacedPath = newPath;
                _dtoLog.AddFile(fileInfo);
                LogDelegate(string.Format("File {0} changed to {1}.", filePath, newPath));
            }
        }
    }
}
