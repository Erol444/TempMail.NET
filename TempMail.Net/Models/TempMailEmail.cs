using System;
using System.Collections.Generic;
using System.Text;

namespace TempMail.Models
{
    public class TempMailEmail
    {
        public int id { get; set; }
        public string from { get; set; }
        public string subject { get; set; }
        public string date { get; set; }
        public List<TempMailAttachment> attachments { get; set; }
        public string body { get; set; }
        public string textBody { get; set; }
        public string htmlBody { get; set; }

        public DateTime GetDate() => DateTime.Parse(this.date);
    }
}
