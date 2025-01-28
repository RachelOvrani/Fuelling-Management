using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class TransactionsDB : BaseDB
    {
       
        protected override BaseEntity CreatModel()
        {
            Transaction transaction = new Transaction();

            transaction.TransactionID = Convert.ToInt32(reader["TransactionID"]);
            transaction.TransactionDate = Convert.ToDateTime(reader["TransactionDate"]);
            transaction.CarID = Convert.ToInt32(reader["CarID"]);
            //transaction.FleetID = Convert.ToInt32(reader["FleetID"]);
            transaction.StationID = Convert.ToInt32(reader["StationID"]);
            transaction.ProductID = Convert.ToInt32(reader["ProductID"]);
            transaction.Price = Convert.ToDecimal(reader["Price"]);
            transaction.Volume = Convert.ToDouble(reader["Volume"]);
            transaction.TotalPayment = Convert.ToDecimal(reader["TotalPayment"]);

            return transaction;
        }

        public override void init()
        {
            this.lst.ForEach((Action<BaseEntity>)(entity =>
            {
                Transaction transaction = entity as Transaction;
                //transaction.Fleet = GlobalDB.fleetsDB.GetFleets().FirstOrDefault(x => x.FleetID == transaction.FleetID);
                transaction.Car = GlobalDB.carsDB.GetCars().FirstOrDefault(x => x.CarID == transaction.CarID);
                transaction.Product = GlobalDB.productsDB.GetProducts().FirstOrDefault(x => x.ProductID == transaction.ProductID);
                transaction.Station = GlobalDB.stationsDB.GetStations().FirstOrDefault(x => x.StationID == transaction.StationID);
            }));

        }


        protected override string GetTableName()
        {
            return "Transactions";
        }

        public List<Transaction> GetTransactions()
        {
            return lst.Cast<Transaction>().ToList();
        }
    }
}
