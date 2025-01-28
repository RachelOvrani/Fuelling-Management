using Manager.FuelingManagmentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Manager
{
    /// <summary>
    /// Interaction logic for AddWorkerCar.xaml
    /// </summary>
    public partial class AddWorkerCar : Page
    {
        public Car currentCar { get; set; }

        public AddWorkerCar(Car car)
        {
            InitializeComponent();
            currentCar = car;
            this.DataContext = car;
            txtWorkerName.Focus();
            
            if (car.RuleID != 0 && car.RuleID != null)
                txtHeader.Text = "עדכון פרטי עובד";

            cmbRule.ItemsSource = Global.client.GetFleetRules(Global.CurrentFleet.FleetID);
        }

        public AddWorkerCar() : this(new Car())
        { }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new WorkersCarsList());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // לא מושלם 
            if(!CheckFullDetails())
            {
                txtErorrMassage.Visibility = Visibility.Visible;
                return;
            }


            // Add WorkerCar
            if (currentCar.CarID == 0)
            {
                currentCar.FleetID = Global.CurrentFleet.FleetID;
                Global.client.AddWorkerCar(currentCar);
            }
            // Update WorkerCar
            else
            {
                Global.client.UpdateWorkerCar(currentCar);
            }

            
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new WorkersCarsList());
        }

        private bool CheckFullDetails()
        {
            // Empty Fileds
            if (string.IsNullOrWhiteSpace(txtWorkerName.Text) ||
                string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtPlateNumber.Text) ||
                string.IsNullOrWhiteSpace(txtPhonNumber.Text) 
                //string.IsNullOrWhiteSpace(txtModel.Text) ||
                //string.IsNullOrWhiteSpace(txtMenufactor.Text) ||
                )
            {
                txtErorrMassage.Text = "יש למלא את כל השדות.";
                return false;
            }

            // Validation
            if (currentCar.OwnerID != txtID.Text ||
                currentCar.OwnerPhoneNumber != txtPhonNumber.Text)
            {
                txtErorrMassage.Text = "";
                return false;
            }

            // רכב זה כבר קיים
            if(Global.client.GetFleetCars(Global.CurrentFleet.FleetID).Exists(x=>x.PlateNumber == currentCar.PlateNumber && currentCar.PlateNumber != txtPlateNumber.Text))
            {
                txtErorrMassage.Text = "";
                txtErrorExistingUser.Visibility = Visibility.Visible;
                return false;
            }


            return true;
        }

        private void btnSave_LostFocus(object sender, RoutedEventArgs e)
        {
            txtErorrMassage.Visibility = Visibility.Collapsed;
            txtErrorExistingUser.Visibility = Visibility.Collapsed;
        }

        private void txtPlateNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            // רכב זה כבר קיים
            if (Global.client.GetFleetCars(Global.CurrentFleet.FleetID).Exists(x => x.PlateNumber == txtPlateNumber.Text && currentCar.PlateNumber != txtPlateNumber.Text))
            {
                txtErorrMassage.Text = "";
                txtErrorExistingUser.Visibility = Visibility.Visible;
               
            }
        }
    }
}
