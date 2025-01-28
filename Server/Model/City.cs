using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class City : BaseEntity
    {
        [NoDB]
        [DataMember]
        public int CityID { get; set; }
        [DataMember]
        public string CityName { get; set; }


        public override string GetTableName()
        {
            return "Cities";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "CityID" };
        }
    }
}
