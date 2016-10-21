using System.Collections.Generic;

namespace Prevensomware.Dto
{
    public class DtoRegistryKey : DtoBase
    {
        public DtoRegistryKey ParentRegistryKey { get; set; }
        public string Name { get; set; }
        public IList<DtoRegistryValue> RegistryValueList { get; set; }
        public IList<DtoRegistryKey>  RegistryKeyList { get; set; }

        public void AddRegistryKey(DtoRegistryKey dtoRegistryKey)
        {
            lock (this)
            {
                if (RegistryKeyList != null && RegistryKeyList.Contains(dtoRegistryKey)) return;
                if (RegistryKeyList == null) RegistryKeyList = new List<DtoRegistryKey>();
                RegistryKeyList.Add(dtoRegistryKey);
                dtoRegistryKey.ParentRegistryKey = this;
            }
        }
        public void AddRegistryValue(DtoRegistryValue dtoRegistryValue)
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
