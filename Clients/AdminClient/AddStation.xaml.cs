using Manager.FuelingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddStation.xaml
    /// </summary>
    public partial class AddStation : Page
    {
        public Station station { get; set; }

        public AddStation(Station station)
        {
            InitializeComponent();

            this.station = station;
            this.DataContext = station;
            
            this.Loaded += AddStation_Loaded;
        }

        private void AddStation_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.cmbCities.ItemsSource = Global.Client.GetCities();

            var list = Global.Client.GetTags();
            this.lstbTags.ItemsSource = list;

            if (station.StationID != 0)
            {
                txtHeader.Text = "עדכון תחנה קיימת";
                var selectedTags = list.Where(x => station.Tags.Exists(y => y.TagID == x.TagID));


                foreach (Tag tag in selectedTags)
                {
                    this.lstbTags.SelectedItems.Add(tag);
                }
            }
        }

        public AddStation() : this(new Station()) { }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFullDitails())
            {
                txtErorrMassage.Visibility = Visibility.Visible;
                return;
            }


            if (station.StationID == 0) // new station
            {
                Global.Client.AddStation(station);

                int new_stationId = Global.Client.GetStations().Max(x => x.StationID);
                var tags = new List<Tag>(lstbTags.SelectedItems.Cast<Tag>());

                if (tags.Count != 0)
                    Global.Client.AddStationsTags(new_stationId, tags);

            }
            else // update station
            {
                var tags = new List<Tag>(lstbTags.SelectedItems.Cast<Tag>());
                
                if (tags == null)
                    tags = new List<Tag>();

                Global.Client.UpdateStationsTags(station.StationID, tags);

                Global.Client.UpdateStation(station);
            }

            
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new StationsList());
        }

        private bool CheckFullDitails()
        {
            // כאשר השדות אינם מלאים בשלמותם
            if (string.IsNullOrWhiteSpace(station.StationName)
                || string.IsNullOrWhiteSpace(station.Address)
                || cmbCities.SelectedItem == null)
                return false;


            // Validations כשלא עבר את ה  
            if (txtStationName.Text != station.StationName
                || txtAddress.Text != station.Address)
            {
                txtErorrMassage.Text = "יש למלא את כל השדות.";
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new StationsList());
        }

        private void btnSave_LostFocus(object sender, RoutedEventArgs e)
        {
            txtErorrMassage.Visibility = Visibility.Collapsed;
        }

    }
}
