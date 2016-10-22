namespace Prevensomware.Dto
{
    public class DtoFileInfo : DtoBase
    {
        public virtual string OriginalExtension { get; set; }
        public virtual string ReplacedExtension { get; set; }
        public virtual string OriginalPath { get; set; }
        public virtual string ReplacedPath { get; set; }
        public virtual DtoLog Log { get; set; }
    }
}