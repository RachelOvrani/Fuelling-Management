using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Product : BaseEntity
    {
        [NoDB]
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public string Description { get; set; }



        public override string GetTableName()
        {
            return "Products";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "ProductID" };
        }
    }
}
