using System;
using System.Collections.Generic;
using Prevensomware.Dto;

namespace Prevensomware.DA
{
    public class FileInfoRepository : RepositoryBase<DtoFileInfo>
    {
        public DtoFileInfo LoadWithPath(string replacedPath)
        {
            return Session.QueryOver<DtoFileInfo>().Where(file => file.ReplacedPath == replacedPath).SingleOrDefault();
        }   
    }
}
