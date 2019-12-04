using System;
using System.Collections.Generic;

namespace DevSandbox.Models
{
    public class Company
    {
        public string name { get; set; }
        public string code { get; set; }
        public string email { get; set; }
        public bool need_report { get; set; }
        public bool need_approval { get; set; }
        public bool no_email_notif { get; set; }
        public string region { get; set; }
        public string document_type { get; set; }

        public ICollection<SubGroup> SubGroups { get; set; }
    }
}
