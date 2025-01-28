using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Car : BaseEntity
    {
        [NoDB]
        [DataMember]
        public int CarID { get; set; }
        [DataMember]
        public string PlateNumber { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string Menufactor { get; set; }
        [DataMember]
         public string OwnerID { get; set; }
        [DataMember]
        public string OwnerName { get; set; }
        [DataMember]
        public string OwnerPhoneNumber { get; set; }
        [DataMember]
        public int FleetID { get; set; }

        [DataMember]
        public int? RuleID { get; set; }


        [NoDB]
        [DataMember]
        public Fleet Fleet { get; set; }


        [NoDB]
        [DataMember]
        public Rule Rule { get; set; }




        public override string GetTableName()
        {
            return "Cars";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "CarID" };
        }
    }

}
