using Manager.FuelingServices;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for FleetsList.xaml
    /// </summary>
    public partial class FleetsList : Page
    {
        public List<Fleet> lstFleets { get; set; }
        public FleetsList()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            lstFleets = Global.Client.GetFleets().OrderBy(x => x.FleetID).ToList();

            dgFleets.ItemsSource = lstFleets;
            rdbAll.IsChecked = true;
            
        }

        
        #region Add, Update, Delete
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            
            frm.Navigate(new AddFleet());
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("אתה בטוח רוצה למחוק?", "התראה", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.RtlReading);
            if (result == MessageBoxResult.No)
                return;

            if (dgFleets.SelectedItem == null)
                return;

            Fleet f = dgFleets.SelectedItem as Fleet;
            
            if(f.Users != null && f.Users.Count()!=0)
                Global.Client.DeleteUser(f.Users[0]);
            
            Global.Client.DeleteFleet(f);
            load();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgFleets.SelectedItem == null)
                return;

            var fleet = dgFleets.SelectedItem as Fleet;
            
            frm.Navigate(new AddFleet(fleet));
        }

        #endregion


        #region Searching

        private void txtX_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilter();

        }
        private void rdb_checked(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }
        void UpdateFilter()
        {
            var companyNameFilter = txtSearchByFleetName.Text.Trim();
                      
            
            bool activeFilter = true;

            if (rdbActive.IsChecked == true)
                activeFilter = true;
            else if (rdbUnactive.IsChecked == true)
                activeFilter = false;

            dgFleets.ItemsSource = lstFleets.Where(x => (rdbAll.IsChecked == true || x.Active == activeFilter) 
                                                   && (string.IsNullOrWhiteSpace(companyNameFilter) || x.CompanyName.Contains(companyNameFilter)));
        }

        #endregion

        private void dgFleets_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var fleet = dgFleets.SelectedItem as Fleet;
            if (fleet == null)
                return;

            
            frm.Navigate(new AddFleet(fleet));
        }

        private void MenuItem_ShowCars_Click(object sender, RoutedEventArgs e)
        {
            
            var fleet = dgFleets.SelectedItem as Fleet;
            frm.Navigate(new CarsList(fleet));
        }

        private void dgFleets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdate.IsEnabled = dgFleets.SelectedItem != null;
            btnDelete.IsEnabled = dgFleets.SelectedItem != null;

        }
    }
}
