using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSandbox.Models
{
    public class SubGroup
    {
        public SubGroup()
        {
            Translation = new Translation();
        }

        public string acronym { get; set; }
        public string long_name { get; set; }
        public string type { get; set; }
        public string instructions { get; set; }

        public Translation Translation { get; set; }
    }
}
