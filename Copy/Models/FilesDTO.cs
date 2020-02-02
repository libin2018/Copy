using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Copy
{
    public class FilesDTO
    {
        public string file_name { get; set; }
        public string file_ext { get; set; }
        public string file_key { get; set; }
        public DateTime? file_timestamp { get; set; }
        public byte[] file_bytes { get; set; }
    }
}