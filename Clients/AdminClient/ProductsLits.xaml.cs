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

namespace Manager
{
    /// <summary>
    /// Interaction logic for ProductsLits.xaml
    /// </summary>
    public partial class ProductsLits : Page
    {
        public List<Product> lstProducts { get; set; }
        public ProductsLits()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            

            lstProducts = Global.Client.GetProducts().OrderBy(x => x.ProductID).ToList(); ;
            dgProducts.ItemsSource = lstProducts;
        }


        #region Add, Update, Delete

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            
            frm.Navigate(new AddProduct());
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem == null)
                return;

            var result = MessageBox.Show("אתה בטוח רוצה למחוק?", "התראה", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.RtlReading);
            if (result == MessageBoxResult.No)
                return;

            Global.Client.DeleteProduct(dgProducts.SelectedItem as Product);
            load();
            
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(dgProducts.SelectedItem == null) 
                return;

            
            var product = dgProducts.SelectedItem as Product;
            frm.Navigate(new AddProduct(product));
        }

        #endregion


        #region Searching

        void UpdateFilter()
        {
            var productFilter = txtSearchByProductName.Text.Trim();
            var priceFilter = txtSearchByPrice.Text.Trim();


            dgProducts.ItemsSource =
                lstProducts.Where(checkFilter);
            
            //lstProducts.Where(x => (string.IsNullOrWhiteSpace(productFilter) || x.ProductName.StartsWith(productFilter))
            //                    && (string.IsNullOrWhiteSpace(priceFilter) || x.Price == (double.Parse(priceFilter))));

            bool checkFilter(Product product)
            {
                // לא עבר סינון שם
                if (!string.IsNullOrWhiteSpace(productFilter) && !product.ProductName.Contains(productFilter))
                    return false;

                // לא עבר סינון שם
                if (!string.IsNullOrWhiteSpace(priceFilter) && product.Price.ToString() != priceFilter)
                    return false;

                return true;
            }

            btnClearFilter.Visibility = (productFilter + priceFilter == "") ? Visibility.Collapsed : Visibility.Visible;
        }

        private void txtX_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilter();
        }

        private void dgProducts_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            

            var product = dgProducts.SelectedItem as Product;
            if(product == null) 
                return;
            frm.Navigate(new AddProduct(product));
        }

        #endregion

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdate.IsEnabled = dgProducts.SelectedItem != null;
            btnDelete.IsEnabled = dgProducts.SelectedItem != null;
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtSearchByPrice.Text = "";
            txtSearchByProductName.Text = "";
        }
    }
}
