using System;
using System.IO;
using System.Linq;
using System.Text;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class BoLog: BoBase<DtoLog>
    {
        private readonly WindowsRegistryManager _windowsRegistryManager;
        private readonly FileManager _fileManager;
        public BoLog()
        {
            _windowsRegistryManager = new WindowsRegistryManager();
            _fileManager = new FileManager();
        }
        public void Revert(DtoLog dtoLog)
        {
            if (dtoLog.RegistryKeyList != null)
            {
                _windowsRegistryManager.RemoveParentRegistryKeyList(dtoLog.RegistryKeyList);
            }
            foreach (var fileInfo in dtoLog.FileList)
            {
                var fileList = dtoLog.SearchPath == "HD" ?
                _fileManager.SearchFileListInWholeHardDrive(fileInfo.ReplacedExtension) 
                : _fileManager.GetFiles(dtoLog.SearchPath, "*"+fileInfo.ReplacedExtension);
                foreach (var filePath in fileList)
                {
                    var fileExtension = Path.GetExtension(filePath);
                    string originalExtension;
                    try
                    {
                        originalExtension = Encoding.UTF8.GetString(Convert.FromBase64String(fileExtension.Remove(0,1)));
                    }
                    catch
                    {
                        continue;
                    }
                    _fileManager.ChangeFileExtension("."+originalExtension, filePath);
                }
            }
            dtoLog.IsReverted = true;
            Save(dtoLog);
        }
        public void RevertAll()
        {
            var dtoLogList = Repository.GetList().Where(log=>!log.IsReverted);
            foreach (var dtoLog in dtoLogList)
            {
                Revert(dtoLog);
            }
        }
    }
}
