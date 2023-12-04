using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUVCServiceApp.ViewModel
{
    public class ResponseRegistry
    {
        public int ID { get; set; }
        public string NameProgram { get; set; }
        public string VersionProgram { get; set; }
        public string DescriptionProgram { get; set; }
        public string Specialization { get; set; }
        public int IDSpecialization { get; set; }
    }
}
