using System;

namespace Prevensomware.Dto
{
    public interface IDtoBase
    {
        DateTime CreateDateTime { get; set; }
        Guid Oid { get; set; }
    }
}