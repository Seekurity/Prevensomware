using System;
using System.Collections.Generic;
using Prevensomware.Dto;

namespace Prevensomware.DA
{
    public class FileInfoRepository : RepositoryBase<DtoFileInfo>
    {
        public IEnumerable<DtoFileInfo> LoadWithReplacedExtension(string replacedExtension)
        {
            return Session.QueryOver<DtoFileInfo>().Where(file => file.ReplacedExtension == replacedExtension).List();
        }
        public IEnumerable<DtoFileInfo> LoadWithOriginalExtension(string originalExtension)
        {
            return Session.QueryOver<DtoFileInfo>().Where(file => file.OriginalExtension == originalExtension).List();
        }
       
        public override IEnumerable<DtoFileInfo> GetList()
        {
            return Session.QueryOver<DtoFileInfo>().Where(x=>!x.IsDeleted).List();
        }
    }
}
