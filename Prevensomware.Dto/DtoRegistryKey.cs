using System.Collections.Generic;

namespace Prevensomware.Dto
{
    public class DtoRegistryKey : DtoBase
    {
        public virtual DtoRegistryKey ParentRegistryKey { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<DtoRegistryValue> RegistryValueList { get; set; }
        public virtual IList<DtoRegistryKey>  RegistryKeyList { get; set; }
        public virtual DtoLog Log { get; set; }
        public virtual void AddRegistryKey(DtoRegistryKey dtoRegistryKey)
        {
            lock (this)
            {
                if (RegistryKeyList != null && RegistryKeyList.Contains(dtoRegistryKey)) return;
                if (RegistryKeyList == null) RegistryKeyList = new List<DtoRegistryKey>();
                RegistryKeyList.Add(dtoRegistryKey);
                dtoRegistryKey.ParentRegistryKey = this;
            }
        }
        public virtual void AddRegistryValue(DtoRegistryValue dtoRegistryValue)
        {
            lock (this)
            {
                if (RegistryValueList != null && RegistryValueList.Contains(dtoRegistryValue)) return;
                if (RegistryValueList == null) RegistryValueList = new List<DtoRegistryValue>();
                RegistryValueList.Add(dtoRegistryValue);
                dtoRegistryValue.ParentRegistryKey = this;
            }
        }

    }
}
