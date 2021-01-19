using System;
using System.Collections.Generic;
using System.Text;

namespace TempMail.Models
{
    public class TempMailAttachment
    {
        public string filename { get; set; }
        public string contentType { get; set; }
        public int size { get; set; }
    }
}
