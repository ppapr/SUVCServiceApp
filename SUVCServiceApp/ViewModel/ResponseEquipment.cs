using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUVCServiceApp.ViewModel
{
    public class ResponseEquipment
    {
        public int ID { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentDescription { get; set; }
        public string NetworkName { get; set; }
        public string InventoryName { get; set; }
        public string OwnerName { get; set; }
        public int IDOwner { get; set; }
        public string StatusName { get; set; }
        public int IDStatus { get; set; }
        public string Location { get; set; }
        public int IDLocation { get; set; }
        public string FullNameEquipment => NetworkName + " " + EquipmentName + " " + EquipmentDescription + " " + InventoryName;
    }
}
