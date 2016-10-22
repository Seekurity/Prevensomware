using System.Linq;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class BoLog: BoBase<DtoLog>
    {
        public void Revert(DtoLog dtoLog)
        {
            WindowsRegistryManager.RemoveParentRegistryKeyList(dtoLog.RegistryKeyList);
            FileManager.RevertFileList(dtoLog.FileList);
            Remove(dtoLog);
        }
        public void RevertAll()
        {
            var dtoLogList = Repository.GetList();
            WindowsRegistryManager.RemoveParentRegistryKeyList(dtoLogList.SelectMany(x=>x.RegistryKeyList));
            FileManager.RevertFileList(dtoLogList.SelectMany(x=>x.FileList));
            RemoveList(dtoLogList);
        }
    }
}
