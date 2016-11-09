using System.Collections.Generic;
using System.ComponentModel;
using Prevensomware.Dto.Annotations;

namespace Prevensomware.Dto
{
    public class DtoUserSettings : DtoBase, INotifyPropertyChanged
    {
        private IList<DtoFileInfo> selectedFileExtensionList;

        public virtual IList<DtoFileInfo> SelectedFileExtensionList
        {
            get
            {
                return selectedFileExtensionList;
            }
            set { SetField(ref selectedFileExtensionList, value, "SelectedFileExtensionList"); }
        }

        public virtual string SearchPath { get; set; }
        public virtual DtoServiceInfo ServiceInfo { get; set; }
        public virtual bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        public virtual void AddFileExtension(DtoFileInfo dtoFileInfo)
        {
            lock (this)
            {
                if (SelectedFileExtensionList != null && SelectedFileExtensionList.Contains(dtoFileInfo)) return;
                if (SelectedFileExtensionList == null) SelectedFileExtensionList = new List<DtoFileInfo>();
                SelectedFileExtensionList.Insert(0,dtoFileInfo);
                dtoFileInfo.UserSettings = this;
            }
        }
        public virtual void RemoveFileExtension(DtoFileInfo dtoFileInfo)
        {
            lock (this)
            {
                if ((SelectedFileExtensionList != null && !SelectedFileExtensionList.Contains(dtoFileInfo)) || SelectedFileExtensionList == null) return;
                SelectedFileExtensionList.Remove(dtoFileInfo);
                dtoFileInfo.UserSettings = null;
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

