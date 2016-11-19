using System.Collections.Generic;
using System.ComponentModel;
using Prevensomware.Dto.Annotations;

namespace Prevensomware.Dto
{
    public class DtoLog : DtoBase, INotifyPropertyChanged
    {
        public virtual IList<DtoFileInfo> FileList { get; set; }
        public virtual IList<DtoRegistryKey> RegistryKeyList { get; set; }
        public virtual string Payload { get; set; }
        private bool isReverted;
        public virtual bool IsReverted { get {return isReverted;} set { SetField(ref isReverted, value, "IsReverted"); } }
        public virtual string SearchPath { get; set; }
        public virtual string Source { get; set; }

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
        public virtual bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
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

        public virtual event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

