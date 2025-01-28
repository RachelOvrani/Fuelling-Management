//using Manager.FuelingServices;
using Manager.FuelingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Collections.Specialized.BitVector32;

namespace Manager
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Page
    {
        public Product product { get; set; }

        public AddProduct(Product product)
        {
            InitializeComponent();
            this.product = product;
            this.DataContext = product;
            

            if (product.ProductID != 0)
                txtHeader.Text = "עדכון מוצר קיים";
        }
        public AddProduct() : this(new Product())
        {
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFullDitails())
            {
                txtErorrMassage.Visibility = Visibility.Visible;
                return;
            }


            
            if (product.ProductID == 0) // new product
            {
                Global.Client.AddProduct(product);
            }
            else
            {
                Global.Client.UpdateProduct(product);
            }

            
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new ProductsLits());
        }

        private bool CheckFullDitails()
        {
            // כאשר השדות אינם מלאים בשלמותם
            if (string.IsNullOrWhiteSpace(product.ProductName)
                || Convert.ToDouble(product.Price) <= 0)
                return false;


            // Validations כשלא עבר את ה  
            // Validations כשלא עבר את ה  
            decimal i;
            if (txtProductName.Text != product.ProductName
                || !decimal.TryParse(txtPrice.Text, out i) || i != product.Price)
            {
                txtErorrMassage.Text = "יש למלא את כל השדות.";
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new ProductsLits());
        }
    
    private void btnSave_LostFocus(object sender, RoutedEventArgs e)
        {
            txtErorrMassage.Visibility = Visibility.Collapsed;
        }
    
    }
}
