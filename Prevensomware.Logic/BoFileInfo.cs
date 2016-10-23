using Prevensomware.DA;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class BoFileInfo: BoBase<DtoFileInfo>
    {
        public bool RevertForPath(string path)
        {
            var fileInfo = new FileInfoRepository().LoadWithPath(path);
            if (fileInfo == null) return false;
            var isReverted = FileManager.RevertOneFile(fileInfo);
            if (!isReverted) return false;
            fileInfo.Log = null;
            Remove(fileInfo);
            return true;
        }
    }
}
