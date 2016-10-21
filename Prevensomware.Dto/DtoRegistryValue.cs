using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prevensomware.Dto
{
    public class DtoRegistryValue : DtoBase
    {
        public DtoRegistryKey ParentRegistryKey { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
