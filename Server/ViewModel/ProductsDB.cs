using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ProductsDB : BaseDB
    {
        public ProductsDB() { }

        protected override BaseEntity CreatModel()
        {
            // Create a new Fleet object
            Product product = new Product();

            // Fill the information from the access to the object
            product.ProductID = Convert.ToInt32(reader["ProductID"]);
            product.ProductName = reader["ProductName"].ToString();
            product.Price = Convert.ToDecimal(reader["Price"]);
            product.Description = reader["Description"].ToString();
            
            return product;
        }

        public override void init() { }

        protected override string GetTableName()
        {
            return "Products";
        }

        public List<Product> GetProducts()
        {
            return lst.Cast<Product>().ToList();
        }

        
    }
}
