using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class ProductLimit : BaseEntity
    {
        [NoDB]
        [DataMember]
        public int LimitID { get; set; }
        public int RuleID { get; set; }
        [DataMember] 
        public int ProductID { get; set; }
        [DataMember]
        public double DailyVolume { get; set; }
        [DataMember]
        public double MonthlyVolume { get; set; }


        

        public override string GetTableName()
        {
            return "ProductsLimit";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "LimitID" };
        }
    }
}
