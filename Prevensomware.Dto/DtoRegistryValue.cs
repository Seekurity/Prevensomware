namespace Prevensomware.Dto
{
    public class DtoRegistryValue : DtoBase
    {
        public virtual DtoRegistryKey ParentRegistryKey { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
    }
}
