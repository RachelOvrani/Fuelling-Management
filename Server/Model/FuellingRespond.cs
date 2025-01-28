using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class FuellingRespond
    {
        [DataMember]
        public bool IsAllowed { get; set; }

        [DataMember]
        public double Amount { get; set; }

        [DataMember]
        public string Explination { get; set; }
    }
}
