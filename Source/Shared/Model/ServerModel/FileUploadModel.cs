using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model.ServerModel
{
    public class FileUploadModel
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public Shared.Enum.FileType FileType { get; set; }
        public int PeriodId { get; set; }
    }
}
