using System.Linq;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class BoLog: BoBase<DtoLog>
    {
        public void Revert(DtoLog dtoLog)
        {
            if (dtoLog.RegistryKeyList != null)
            {
                WindowsRegistryManager.RemoveParentRegistryKeyList(dtoLog.RegistryKeyList);
            }
            foreach (var fileInfo in dtoLog.FileList)
            {
                var fileList = dtoLog.SearchPath == "HD" ? 
                FileManager.SearchFileListInWholeHardDrive(fileInfo.ReplacedExtension) 
                : FileManager.GetFiles(dtoLog.SearchPath, "*"+fileInfo.ReplacedExtension);
                FileManager.ChangeFileListExtensions(fileInfo.OriginalExtension, fileList);
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
