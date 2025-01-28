using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Transaction : BaseEntity
    {

        [NoDB]
        [DataMember]
        public int TransactionID { get; set; }
        [DataMember]
        public DateTime TransactionDate { get; set; }
        public int CarID { get; set; }
        //public int FleetID { get; set; }
        public int StationID { get; set; }
        public int ProductID { get; set; }
        public decimal Price { get; set; }
        [DataMember]
        public double Volume { get; set; }
        [DataMember]
        public decimal TotalPayment { get; set; }


        /*[NoDB]
        [DataMember]
        public Fleet Fleet { get; set; }*/
        [NoDB]
        [DataMember]
        public Car Car { get; set; }
        [NoDB]
        [DataMember]
        public Rule Rule { get; set; }
        [NoDB]
        [DataMember]
        public Product Product { get; set; }
        [NoDB]
        [DataMember]
        public Station Station { get; set; }


        public override string GetTableName()
        {
            return "Transactions";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "TransactionID" };
        }


    }
}
