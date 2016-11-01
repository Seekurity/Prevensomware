using System.IO;
using System.Linq;
using Prevensomware.DA;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class BoFileInfo: BoBase<DtoFileInfo>
    {
        private readonly FileManager _fileManager;
        public BoFileInfo()
        {
            _fileManager = new FileManager();
        }
        public bool RevertForPath(string path)
        {
            var fileExtension = Path.GetExtension(path);
            var fileInfoList = new FileInfoRepository().LoadWithExtension(fileExtension);
            if (fileInfoList == null) return false;
            return fileInfoList != null && _fileManager.ChangeFileExtension(fileInfoList.First().OriginalExtension, path);
        }
    }
}
