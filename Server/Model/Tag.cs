using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Tag : BaseEntity
    {
        [NoDB]
        [DataMember]
        public int TagID { get; set; }
        [DataMember]
        public string TagName { get; set; }


        public override string GetTableName()
        {
            return "Tags";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "TagID" };
        }

    }
}
