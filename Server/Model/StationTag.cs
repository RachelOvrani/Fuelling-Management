using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class StationTag : BaseEntity
    {
        public int StationID { get; set; }
        public int TagID { get; set; }


        [NoDB]
        [DataMember]
        public Station Station { get; set; }
        
        [NoDB]
        [DataMember]
        public Tag Tag { get; set; }

        public override string GetTableName()
        {
            return "StationsTags";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "StationID", "TagID" };
        }
    }
}
