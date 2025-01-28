using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class RuleTag : BaseEntity
    {
        public int RuleID { get; set; }
        public int TagID { get; set; }


        [NoDB]
        [DataMember]
        public Rule Rule { get; set; }
        
        [NoDB]
        [DataMember]
        public Tag Tag { get; set; }


        public override string GetTableName()
        {
            return "RulesTags";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "RuleID", "TagID" };
        }
    }
}
