using System.IO;
using Prevensomware.DA;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class BoFileInfo: BoBase<DtoFileInfo>
    {
        public bool RevertForPath(string path)
        {
            var fileExtension = Path.GetExtension(path);
            var fileInfo = new FileInfoRepository().LoadWithExtension(fileExtension);
            return fileInfo != null && FileManager.ChangeFileExtension(fileInfo.OriginalExtension, path);
        }
    }
}
