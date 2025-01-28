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
    /// Interaction logic for StationsList.xaml
    /// </summary>
    public partial class StationsList : Page
    {
        public List<Station> lstStations { get; set; }
        public List<City> lstCities { get; set; }

        public StationsList()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            this.Cursor = Cursors.Arrow
                ;
            lstCities = Global.Client.GetCities();
            //var lstTags  = Global.adminClient.GetStations().Select(x=>x.Tags);

            lstStations = Global.Client.GetStations().OrderBy(x=>x.StationID).ToList();
            // Fill the city of each station by thr station.CityId property
            foreach (var station in lstStations)
            {
                station.City = lstCities.FirstOrDefault(x => x.CityID == station.CityID);
            }

            dgStations.ItemsSource = lstStations;
            cmbCities.ItemsSource = lstCities;
            cmbTags.ItemsSource = Global.Client.GetTags();
        }



        #region Add, Update, Delete

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            frm.Navigate(new AddStation());
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgStations.SelectedItems == null)
                return;

            var result = MessageBox.Show("אתה בטוח רוצה למחוק?" , "התראה", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.RtlReading);
            if (result == MessageBoxResult.No)
                return;

            Station station = dgStations.SelectedItem as Station;
            Global.Client.DeleteStation(station);
            load();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgStations.SelectedItem == null)
                return;

            var station = dgStations.SelectedItem as Station;
            frm.Navigate(new AddStation(station));
        }

        #endregion


        #region Searching

        private void txtX_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilter();
        }

        private void cmbX_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            UpdateFilter();
            //e.Handled = true;
        }

        private void UpdateFilter()
        {
            var nameFilter = txtSearchByStationName.Text.Trim();
            var cityFilter = "";
            var tagFilter = "";
            if (cmbCities.SelectedValue is City)
                cityFilter = (cmbCities.SelectedValue as City).CityName;
            
            if (cmbTags.SelectedValue is Tag)
                tagFilter = (cmbTags.SelectedValue as Tag).TagName;

            dgStations.ItemsSource = lstStations.Where(x => (string.IsNullOrWhiteSpace(nameFilter) || x.StationName.Contains(nameFilter))
                                                         && (string.IsNullOrWhiteSpace(cityFilter) || x.City.CityName == cityFilter)
                                                         && (string.IsNullOrWhiteSpace(tagFilter) || x.Tags.Where(y=>y.TagName == tagFilter).Count() != 0));

            btnClearFilter.Visibility = (nameFilter + cityFilter + tagFilter == "") ? Visibility.Collapsed : Visibility.Visible;

        }


        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtSearchByStationName.Text = "";
            cmbCities.SelectedItem = null;
            cmbTags.SelectedItem = null;
        }


        #endregion

        private void dgStations_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var station = dgStations.SelectedItem as Station;
            if(station == null) 
                return;
            frm.Navigate(new AddStation(station));
        }


        private void dgStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdate.IsEnabled = dgStations.SelectedItem != null;
            btnDelete.IsEnabled = dgStations.SelectedItem != null;
        }

    }

}






