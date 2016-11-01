using System;
using System.Collections.Generic;
using Prevensomware.Dto;

namespace Prevensomware.DA
{
    public class FileInfoRepository : RepositoryBase<DtoFileInfo>
    {
        public IEnumerable<DtoFileInfo> LoadWithExtension(string replacedExtension)
        {
            return Session.QueryOver<DtoFileInfo>().Where(file => file.ReplacedExtension == replacedExtension).List();
        }   
    }
}
