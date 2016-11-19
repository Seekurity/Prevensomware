using System;
using System.IO;
using System.Linq;
using System.Text;
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
            string originalExtension;
            try
            {
                originalExtension = Encoding.UTF8.GetString(Convert.FromBase64String(fileExtension.Remove(0,1)));
            }
            catch
            {
                return false;
            }
            return fileExtension != null && _fileManager.ChangeFileExtension("."+originalExtension, path);
        }

        public DtoFileInfo InsertNewFileInfo(string originalExtension,ref DtoUserSettings userSettings)
        {
            var fileInfoList = new FileInfoRepository().LoadWithOriginalExtension(originalExtension).ToList();
            DtoFileInfo fileInfo;
            var userSettingsRepository = new UserSettingsRepository();
            if (!fileInfoList.Any())
            {
                fileInfo = new DtoFileInfo
                {
                    CreateDateTime = DateTime.Now,
                    OriginalExtension = "." + originalExtension,
                    ReplacedExtension = "." + Convert.ToBase64String(Encoding.UTF8.GetBytes(originalExtension))
                };
                userSettings.AddFileExtension(fileInfo);
                userSettingsRepository.CreateOrUpdate(userSettings);
            }
            else
            {
                fileInfo = fileInfoList.First();
                fileInfo.IsDeleted = false;
                userSettings.AddFileExtension(fileInfo);
                new UserSettingsRepository().CreateOrUpdate(userSettings);
            }
            return fileInfo;
        }
        public void RemoveFileInfo(DtoFileInfo fileInfo, ref DtoUserSettings userSettings)
        {
            var userSettingsRepository = new UserSettingsRepository();
            fileInfo.IsDeleted= true;
            userSettings.RemoveFileExtension(fileInfo);
            userSettingsRepository.CreateOrUpdate(userSettings);
        }
    }
}
