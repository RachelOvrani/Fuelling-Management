using Manager.FuelingManagmentService;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
    /// Interaction logic for WokerUseDetails.xaml
    /// </summary>
    public partial class WokerUseDetails : Page
    {
        public Car currentCar { get; set; }
        public List<Transaction> lstTransactions { get; set; }
        public WokerUseDetails(Car workerCar)
        {
            InitializeComponent();
            
            currentCar = workerCar;
            lstTransactions = Global.client.GetTransactionsByCarID(currentCar.CarID);
            foreach (var trn in lstTransactions)
            {
                trn.Car.Rule = Global.client.GetRuleByID(trn.Car.RuleID);
            }
            dgTransactions.ItemsSource = lstTransactions;
            txtHeader.Text = "פרטי השימוש : " + currentCar.PlateNumber;
            dpSearchByDate.DisplayDateEnd = DateTime.Today;
            dpSearchByDate.DisplayDateStart = DateTime.Now.AddYears(-10);
        }


        private void txtX_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgTransactions_DoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new WorkersCarsList());
        }
    }
}
