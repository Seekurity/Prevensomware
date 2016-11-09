using System;
using System.IO;
using System.Linq;
using Prevensomware.DA;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class BoFileInfo: BoBase<DtoFileInfo>
    {
        private readonly FileManager _fileManager;
        private static readonly Random Random = new Random();
        public BoFileInfo()
        {
            _fileManager = new FileManager();
        }
        public bool RevertForPath(string path)
        {
            var fileExtension = Path.GetExtension(path);
            var fileInfoList = new FileInfoRepository().LoadWithReplacedExtension(fileExtension);
            if (fileInfoList == null) return false;
            return fileInfoList != null && _fileManager.ChangeFileExtension(fileInfoList.First().OriginalExtension, path);
        }
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
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
                    OriginalExtension = !originalExtension.StartsWith(".") ? "." + originalExtension : originalExtension,
                    ReplacedExtension = "." + RandomString(4)
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
