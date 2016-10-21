using System;
using System.Collections.Generic;

namespace Prevensomware.Dto
{
    public class DtoLog : DtoBase
    {
        public IList<DtoFileInfo> FileList { get; set; }
        public IList<DtoRegistryKey> RegistryKeyList { get; set; }
        public string Payload { get; set; }
        public string SearchPath { get; set; }

        public void AddRegistryKey(DtoRegistryKey dtoRegistryKey)
        {
            lock (this)
            {
                if (RegistryKeyList != null && RegistryKeyList.Contains(dtoRegistryKey)) return;
                if (RegistryKeyList == null) RegistryKeyList = new List<DtoRegistryKey>();
                RegistryKeyList.Add(dtoRegistryKey);
            }
        }

        public void AddFile(DtoFileInfo dtoFileInfo)
        {
            lock (this)
            {
                if (FileList != null && FileList.Contains(dtoFileInfo)) return;
                if (FileList == null) FileList = new List<DtoFileInfo>();
                FileList.Add(dtoFileInfo);
            }
        }
    }
}

