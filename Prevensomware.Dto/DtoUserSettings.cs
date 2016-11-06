using System.Collections.Generic;

namespace Prevensomware.Dto
{
    public class DtoUserSettings : DtoBase
    {
        public virtual IList<DtoFileInfo> SelectedFileExtensionList { get; set; }
        public virtual string SearchPath { get; set; }
        public virtual DtoServiceInfo ServiceInfo { get; set; }
        public virtual void AddFile(DtoFileInfo dtoFileInfo)
        {
            lock (this)
            {
                if (SelectedFileExtensionList != null && SelectedFileExtensionList.Contains(dtoFileInfo)) return;
                if (SelectedFileExtensionList == null) SelectedFileExtensionList = new List<DtoFileInfo>();
                SelectedFileExtensionList.Add(dtoFileInfo);
                dtoFileInfo.UserSettings = this;
            }
        }
    }
}

