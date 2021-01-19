using System;
using System.Collections.Generic;
using System.Text;

namespace TempMail.Models
{
    public class TempMailMessage
    {
        public int id { get; set; }
        public string from { get; set; }
        public string subject { get; set; }
        public string date { get; set; }

        public DateTime GetDate() => DateTime.Parse(this.date);
    }
}
