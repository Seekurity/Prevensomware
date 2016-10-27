using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class FileManager
    {
        public Action<string> LogDelegate { get; set; }
        private DtoLog _dtoLog;
        public void RenameAllFilesWithNewExtension(IEnumerable<DtoFileInfo> fileInfoList, string directoryPath, ref DtoLog dtoLog)
        {
            _dtoLog = dtoLog;
            foreach (var fileInfo in fileInfoList)
            {
                if (directoryPath == "HD") RenameFileListInWholeHardDrive(fileInfo);
                else RenameFileListForCertainPath(fileInfo,directoryPath);
            }
            new BoLog().Save(_dtoLog);
        }

        private void RenameFileListInWholeHardDrive(DtoFileInfo fileInfo)
        {
            ChangeFileListExtensions(fileInfo.ReplacedExtension, SearchFileListInWholeHardDrive(fileInfo.OriginalExtension, LogDelegate));
        }
        public IEnumerable<string> SearchFileListInWholeHardDrive(string extension, Action<string> logDelegate = null)
        {
            var fileList = new List<string>();
            foreach (var drive in DriveInfo.GetDrives().Where(x => x.IsReady))
            {
                logDelegate?.Invoke($"Attempting to Get files from {drive.RootDirectory.FullName}");
                try
                {
                    fileList.AddRange(GetFiles(drive.RootDirectory.FullName, "*" + extension, LogDelegate));
                }
                catch
                {
                    logDelegate?.Invoke($"Couldn't load files from {drive.RootDirectory.FullName}");
                }
            }
            return fileList;
        }
        public IEnumerable<string> GetFiles(string root, string searchPattern, Action<string> logDelegate = null)
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
                    logDelegate?.Invoke($"Attempting to Get Files from {path}");
                    next = Directory.GetFiles(path, searchPattern);
                }
                catch
                {
                    logDelegate?.Invoke("Couldn't Get Files in " + path);
                }
                if (next != null && next.Length != 0)
                    files.AddRange(next);
                try
                {
                    logDelegate?.Invoke($"Attempting to Get Subdirectories from {path}");
                    next = Directory.GetDirectories(path);
                    foreach (var subdir in next) pending.Push(subdir);
                }
                catch
                {
                    logDelegate?.Invoke("Couldn't Get Directories in  " + path);
                }
            }
            logDelegate?.Invoke(files.Count + " File/s Found.");
            return files;
        }
        private void RenameFileListForCertainPath(DtoFileInfo fileInfo, string directoryPath)
        {
            var allFilesArray = GetFiles(directoryPath , "*" + fileInfo.OriginalExtension, LogDelegate);
            ChangeFileListExtensions(fileInfo.ReplacedExtension, allFilesArray, LogDelegate);
            _dtoLog.AddFile(fileInfo);
        }

        public void ChangeFileListExtensions(string extension, IEnumerable<string> allFilesArray, Action<string> logDelegate = null)
        {
            foreach (var filePath in allFilesArray)
            {
                ChangeFileExtension(extension, filePath, logDelegate);
            }
        }

        public bool ChangeFileExtension(string extension, string filePath, Action<string> logDelegate = null)
        {
            try
            {
                var newPath = Path.ChangeExtension(filePath, extension);
                File.Move(filePath, newPath);
                logDelegate?.Invoke($"Securing File {filePath}.");
                return true;
            }
            catch
            {
                logDelegate?.Invoke($"Couldn't Rename File {filePath}");
                return false;
            }
        }
    }
}
