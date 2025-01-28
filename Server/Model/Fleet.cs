using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Fleet : BaseEntity
    {
        [NoDB]
        [DataMember]
        public int FleetID { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public double Credit { get; set; }
        [DataMember]
        public double Discount { get; set; }
        [DataMember]
        public string LogoPath { get; set; }
        [DataMember]
        public bool Active { get; set; }



        //[NoDB]
        //[DataMember]
        //public List<Rule> lstRules { get; set; }

        [NoDB]
        [DataMember]
        public List<User> Users { get; set; }

        [NoDB]
        [DataMember]
        public byte[] Logo { get; set; }

        [NoDB]
        public List<Car> Cars { get; set; }


        
        public override string GetTableName()
        {
            return "Fleets";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "FleetID" };
        }


    }

}
