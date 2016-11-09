namespace Prevensomware.Dto
{
    public class DtoFileInfo : DtoBase
    {
        public virtual string OriginalExtension { get; set; }
        public virtual string ReplacedExtension { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DtoUserSettings UserSettings { get; set; }
        public virtual DtoLog Log { get; set; }
    }
}