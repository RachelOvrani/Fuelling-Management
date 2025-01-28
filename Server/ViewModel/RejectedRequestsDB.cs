using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class RejectedRequestsDB : BaseDB
    {
        

        protected override BaseEntity CreatModel()
        {
            RejectedRequest rejectedRequest = new RejectedRequest();

            rejectedRequest.RejectedRequestID = Convert.ToInt32(reader["RejectedRequestID"]);
            rejectedRequest.RejectedRequestDate = Convert.ToDateTime(reader["RejectedRequestDate"]);
            rejectedRequest.CarID = Convert.ToInt32(reader["CarID"]);
            //rejectedRequest.FleetID = Convert.ToInt32(reader["FleetID"]);
            rejectedRequest.StationID = Convert.ToInt32(reader["StationID"]);
            rejectedRequest.ProductID = Convert.ToInt32(reader["ProductID"]);
            rejectedRequest.Reason = reader["Reason"].ToString();

            return rejectedRequest;
        }

        public override void init()
        {
            this.lst.ForEach((Action<BaseEntity>)(entity =>
            {
                RejectedRequest rejectedRequest = entity as RejectedRequest;

                //rejectedRequest.Fleet = GlobalDB.fleetsDB.GetFleets().FirstOrDefault(x => x.FleetID == rejectedRequest.FleetID);
                rejectedRequest.Car = GlobalDB.carsDB.GetCars().FirstOrDefault(x => x.CarID == rejectedRequest.CarID);
                rejectedRequest.Product = GlobalDB.productsDB.GetProducts().FirstOrDefault(x => x.ProductID == rejectedRequest.ProductID);
                rejectedRequest.Station = GlobalDB.stationsDB.GetStations().FirstOrDefault(x => x.StationID == rejectedRequest.StationID);
            }));

        }

        protected override string GetTableName()
        {
            return "RejectedRequests";
        }
        

        public List<RejectedRequest> GetRejectedRequests()
        {
            return lst.Cast<RejectedRequest>().ToList();
        }

    }
}
