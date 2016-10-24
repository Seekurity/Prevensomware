using System;
using System.Collections.Generic;

namespace Prevensomware.Dto
{
    public class DtoLog : DtoBase
    {
        public virtual IList<DtoFileInfo> FileList { get; set; }
        public virtual IList<DtoRegistryKey> RegistryKeyList { get; set; }
        public virtual string Payload { get; set; }
        public virtual bool IsReverted { get; set; }
        public virtual string SearchPath { get; set; }
        public virtual void AddRegistryKey(DtoRegistryKey dtoRegistryKey)
        {
            lock (this)
            {
                if (RegistryKeyList != null && RegistryKeyList.Contains(dtoRegistryKey)) return;
                if (RegistryKeyList == null) RegistryKeyList = new List<DtoRegistryKey>();
                RegistryKeyList.Add(dtoRegistryKey);
                dtoRegistryKey.Log = this;
            }
        }

        public virtual void AddFile(DtoFileInfo dtoFileInfo)
        {
            lock (this)
            {
                if (FileList != null && FileList.Contains(dtoFileInfo)) return;
                if (FileList == null) FileList = new List<DtoFileInfo>();
                FileList.Add(dtoFileInfo);
                dtoFileInfo.Log = this;
            }
        }
    }
}

