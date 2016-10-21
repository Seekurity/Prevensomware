using System;

namespace Prevensomware.Dto
{
    public class DtoBase : IDtoBase
    {
        public Guid Oid { get; set; }
        public DateTime CreateDateTime { get; set; }
        public override bool Equals(object obj)
        {
            return obj != null && Oid == ((DtoBase)obj).Oid;
        }

        public override int GetHashCode()
        {
            return Oid.GetHashCode();
        }
    }
}
