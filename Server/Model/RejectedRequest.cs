using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class RejectedRequest : BaseEntity
    {
        [NoDB]
        [DataMember]
        public int RejectedRequestID { get; set; }
        [DataMember]
        public DateTime RejectedRequestDate { get; set; }
        public int CarID { get; set; }
        //public int FleetID { get; set; }
        public int StationID { get; set; }
        public int ProductID { get; set; }
        [DataMember] 
        public string Reason { get; set; }
        


        
        /*[DataMember]
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
            return "RejectedRequests";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "RejectedRequestsID" };
        }


    }
}
