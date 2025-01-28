using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Rule : BaseEntity
    {
        [NoDB]
        [DataMember]
        public int RuleID { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public int FleetID { get; set; }



        [NoDB]
        [DataMember]
        public List<Tag> Tags { get; set; }
        [NoDB]
        [DataMember]
        public List<ProductLimit> LimitedProducts { get; set; }


        public override string GetTableName()
        {
            return "Rules";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "RuleID" };
        }
    }
}
