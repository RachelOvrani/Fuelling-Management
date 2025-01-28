using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Station : BaseEntity
    {
        [NoDB]
        [DataMember]
        public int StationID { get; set; }
        [DataMember]
        public string StationName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public int CityID { get; set; }

               
        [NoDB]
        [DataMember]
        public City City { get; set; }
        

        [NoDB]
        [DataMember]
        public List<Tag> Tags { get; set; }


        public Station()
        {
            Tags = new List<Tag>();
        }

        public override string GetTableName()
        {
            return "Stations";
            
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "StationID" };
        }
    }
}
