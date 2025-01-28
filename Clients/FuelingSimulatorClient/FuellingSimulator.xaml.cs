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
using System.Windows.Shapes;
using FuelingSimulatorClient.FuelingAuthorizationService;
using MaterialDesignThemes.Wpf;

namespace Manager
{
    /// <summary>
    /// Interaction logic for FuellingSimulator.xaml
    /// </summary>
    public partial class FuellingSimulator : Page
    {
        public FuellingRequest fuellingRequest = new FuellingRequest();
        public FuellingSimulator()
        {
            InitializeComponent();
            this.DataContext = fuellingRequest;

            cmbStations.ItemsSource = Global.client.GetStations();
            cmbProducts.ItemsSource = Global.client.GetProducts();


        }

        private void btnRequest_Click(object sender, RoutedEventArgs e)
        {
            FuellingRespond respond = Global.client.FuellingRequest(fuellingRequest);

            if (respond.Amount <= 0)
            { 
                txtResult.Text = respond.Explination;
                btnNewRequest.Visibility = Visibility.Visible;
            }
            else
            {
                btnNewRequest.Visibility = Visibility.Hidden;

                txtResult.Text = $"את/ה יכול/ה לצרוך מוצר זה עד {respond.Amount}.";
                txtCar.Text = fuellingRequest.PlateNumber;
                txtStation.Text = fuellingRequest.GasStation.StationName;
                txtProduct.Text = fuellingRequest.ChosenProduct.ProductName;
                HintAssist.SetHelperText(txtAmount, $"{respond.Amount}/0");
                txtAmount.IsEnabled = true;
            }


        }

        private void cmbStations_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            CheckFullDitails();
        }

        private void cmbProducts_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            CheckFullDitails();
        }


        private void txtPlateNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

            CheckFullDitails();
        }

        private void CheckFullDitails()
        {
            ResetReportDetails();

            btnRequest.IsEnabled = false;
            txtError.Visibility = Visibility.Hidden;

            if (txtPlateNumber.Text.Length != 9) return;

            if (!Global.client.CheckExistingCarByPlateNumber(txtPlateNumber.Text))
            {
                txtError.Visibility = Visibility.Visible;
                return;
            }


            if (cmbStations.SelectedItem == null) return;

            if (cmbProducts.SelectedItem == null) return;


            btnRequest.IsEnabled = true;
        }

        private void ResetReportDetails()
        {
            txtCar.Text = "";
            txtStation.Text = "";
            txtProduct.Text = "";
            txtAmount.Text = "";
            HintAssist.SetHelperText(txtAmount, "0/0");
            txtResult.Text = "";
            btnReport.IsEnabled = false;
        }

        private void txtAmount_KeyUp(object sender, KeyEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                txtAmount.Text = "";
                return;
            }

            if (Convert.ToInt32(txtAmount.Text) > 0)
                btnReport.IsEnabled = true;



            string tmp = HintAssist.GetHelperText(txtAmount).ToString();
            string[] arr = tmp.Split('/');
            
            if (Convert.ToInt32(arr[0]) - Convert.ToInt32(txtAmount.Text) < 0)
            {
                txtAmount.Text = txtAmount.Text.Substring(0, txtAmount.Text.Length-1);
                txtAmount.Select(txtAmount.Text.Length, 0); 
            }
            
            HintAssist.SetHelperText(txtAmount, $"{arr[0]}/{txtAmount.Text}");
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAmount.Text)) return;
            if (Convert.ToInt32(txtAmount.Text) <= 0) return;

            Global.client.FuellingReport(fuellingRequest, Convert.ToInt32(txtAmount.Text));

            ResetAll();
        }

        private void ResetAll()
        {
            ResetRequestDetails();
            ResetReportDetails();
        }

        private void ResetRequestDetails()
        {
            txtPlateNumber.Text = "";
            txtError.Visibility = Visibility.Hidden;
            cmbStations.SelectedItem = null;
            cmbProducts.SelectedItem = null;
            txtResult.Text = "";
            btnNewRequest.Visibility = Visibility.Hidden;
        }

        private void btnNewRequest_Click(object sender, RoutedEventArgs e)
        {
            ResetAll();
        }
    }
}
