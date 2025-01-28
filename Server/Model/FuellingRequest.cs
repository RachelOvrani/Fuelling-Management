using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class FuellingRequest
    {
        [DataMember]
        public string PlateNumber { get; set; }
        [DataMember]
        public Station GasStation { get; set; }
        [DataMember]
        public Product ChosenProduct { get; set; }

        public FuellingRequest() { }

        public FuellingRequest(string plateNumber, Station gasStation, Product chosenProduct)
        {
            this.PlateNumber = plateNumber;
            this.GasStation = gasStation;
            this.ChosenProduct = chosenProduct;
        }
    }
}
