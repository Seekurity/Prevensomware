using System;

namespace Prevensomware.Dto
{
    public interface IDtoBase
    {
        DateTime CreateDateTime { get; set; }
        int? Oid { get; set; }
    }
}