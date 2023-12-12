using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUVCServiceApp.ViewModel
{
    public class ResponseRequests
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime DateCreateRequest { get; set; }
        public DateTime DateExecuteRequest { get; set; }
        public string UserRequestName { get; set; }
        public string UserExecutorName { get; set; }
        public string StatusName { get; set; }
        public string PriorityName { get; set; }
        public string EquipmentName { get; set; }
        public int IDUserRequest { get; set; }
        public int IDExecutorRequest { get; set; }
        public int IDStatus { get; set; }
        public int IDPriority { get; set; }
        public int IDEquipment { get; set; }
    }
}
