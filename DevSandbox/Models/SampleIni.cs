using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DevSandbox.Models
{
    [Serializable] // needed for SimpleIniFormatter
    public class SampleIni
    {
        public string name;
        public string email;
        public bool approved;
        public bool notify;
    }
}
