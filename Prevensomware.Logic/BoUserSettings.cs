using System.Linq;
using Prevensomware.DA;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class BoUserSettings : BoBase<DtoUserSettings>
    {
        public DtoUserSettings LoadCurrentUserSettings()
        {
            return new UserSettingsRepository().GetList().FirstOrDefault();
        }
    }
}
