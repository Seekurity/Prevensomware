using System;

namespace Prevensomware.Dto
{
    public class DtoBase : IDtoBase
    {
        public virtual int? Oid { get; set; }
        public virtual DateTime CreateDateTime { get; set; }
        public override bool Equals(object obj)
        {
            return obj != null && ((DtoBase)obj).Oid != null && Oid == ((DtoBase)obj).Oid;
        }

        public override int GetHashCode()
        {
            return Oid.GetHashCode();
        }
    }
}
