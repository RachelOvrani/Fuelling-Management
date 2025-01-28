using Manager.FuelingManagmentService;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Manager
{
    /// <summary>
    /// Interaction logic for CarsList.xaml
    /// </summary>
    public partial class WorkersCarsList : Page
    {
        public Car Workercar { get; set; }
        public List<Car> lstWorkersCars { get; set; }
        public WorkersCarsList()
        {            
            InitializeComponent();
            Workercar = new Car();
            this.DataContext = Workercar;
            
            
            lstWorkersCars = Global.client.GetFleetCars(Global.CurrentFleet.FleetID);
            foreach (var workerCar in lstWorkersCars)
            {
                workerCar.Rule = Global.fleetRules.FirstOrDefault(x => x.RuleID == workerCar.RuleID);
            }
            dgWorkersCars.ItemsSource = lstWorkersCars;

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            frmWorker.Navigate(new AddWorkerCar());
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgWorkersCars.SelectedItem == null)
                return;

            var workerCar = dgWorkersCars.SelectedItem as Car;

            frmWorker.Navigate(new AddWorkerCar(workerCar));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("אתה בטוח רוצה למחוק?", "התראה", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.RtlReading);
            if (result == MessageBoxResult.No)
                return;

            if (dgWorkersCars.SelectedItem == null)
                return;

            Car c = dgWorkersCars.SelectedItem as Car;

            Global.client.DeleteWorkerCar(c);

            lstWorkersCars = Global.client.GetFleetCars(Global.CurrentFleet.FleetID);
            dgWorkersCars.ItemsSource = lstWorkersCars;
        }




        void UpdateFilter()
        {
            var workerNameFilter = txtSearchByWorkerName.Text.Trim();
            var PlateNumberFilter = txtSearchByPlateNumber.Text.Trim();


            dgWorkersCars.ItemsSource = lstWorkersCars.Where(x => (string.IsNullOrWhiteSpace(workerNameFilter) || x.OwnerName.Contains(workerNameFilter))
                                && (string.IsNullOrWhiteSpace(PlateNumberFilter) || x.PlateNumber.Contains(PlateNumberFilter)));


            btnClearFilter.Visibility = (workerNameFilter + PlateNumberFilter == "") ? Visibility.Collapsed : Visibility.Visible;
        }

        private void txtX_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilter();
        }

        private void dgWorkersCar_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var workerCar = dgWorkersCars.SelectedItem as Car;
            if (workerCar == null)
                return;

            frmWorker.Navigate(new AddWorkerCar(workerCar));
        }
        private void dgWorkersCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdate.IsEnabled = dgWorkersCars.SelectedItem != null;
            btnDelete.IsEnabled = dgWorkersCars.SelectedItem != null;
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtSearchByWorkerName.Text = "";
            txtSearchByPlateNumber.Text = "";
        }

        private void MenuItem_ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            var workerCar = dgWorkersCars.SelectedItem as Car;
            if (workerCar == null)
                return;

            frmWorker.Navigate(new WokerUseDetails(workerCar));
        }

        private void MenuItem_Rule_Click(object sender, RoutedEventArgs e)
        {
            var workerCar = dgWorkersCars.SelectedItem as Car;
            if (workerCar == null)
                return;

            var workerRule = Global.client.GetRuleByID(workerCar.RuleID);
            frmWorker.Navigate(new AddRule(workerRule));
        }
    }
}
