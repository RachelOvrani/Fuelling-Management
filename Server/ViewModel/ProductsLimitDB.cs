using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ProductsLimitDB : BaseDB
    {
        protected override BaseEntity CreatModel()
        {
            ProductLimit productLimit = new ProductLimit();
            productLimit.LimitID = Convert.ToInt32(reader["LimitID"]);
            productLimit.ProductID = Convert.ToInt32(reader["ProductID"]);
            productLimit.RuleID = Convert.ToInt32(reader["RuleID"]);
            productLimit.DailyVolume = (reader["DailyVolume"].ToString() == "")? 0: Convert.ToDouble(reader["DailyVolume"]);
            productLimit.MonthlyVolume = Convert.ToDouble(reader["MonthlyVolume"]);

            return productLimit;
        }

        public override void init()
        {
            this.lst.ForEach(entity =>
            {
                ProductLimit productLimit = entity as ProductLimit;
                //productLimit.Product = GlobalDB.productsDB.GetProducts().FirstOrDefault(x => x.ProductID == productLimit.ProductID);
            });
        }

        protected override string GetTableName()
        {
            return "ProductsLimit";
        }

        public List<ProductLimit> GetProductsLimits()
        {
            return lst.Cast<ProductLimit>().ToList();   
        }            



    }
}
