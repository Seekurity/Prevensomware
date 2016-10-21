using System;
using System.Collections.Generic;

namespace Prevensomware.Dto
{
    public class DtoLog
    {
        public List<DtoFileInfo> FileList{ get; set; }
        public string Payload { get; set; }
        public DateTime GenerateDate { get; set; }
    }
}
