using System.IO;
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
            var fileInfo = new FileInfoRepository().LoadWithExtension(fileExtension);
            return fileInfo != null && _fileManager.ChangeFileExtension(fileInfo.OriginalExtension, path);
        }
    }
}
