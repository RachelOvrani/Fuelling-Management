using Manager.FuelingManagmentService;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System;

namespace Manager
{
    /// <summary>
    /// Interaction logic for TransactionsTable.xaml
    /// </summary>
    public partial class TransactionsList : Page
    {
        public List<Transaction> lstTransactions { get; set; }
        public TransactionsList()
        {
            InitializeComponent();

            lstTransactions = Global.client.GetTransactions(Global.CurrentFleet.FleetID);
            foreach (var trn in lstTransactions)
            {
                trn.Car.Rule = Global.client.GetRuleByID(trn.Car.RuleID);
            }
            dgTransactions.ItemsSource = lstTransactions.OrderByDescending(x => x.TransactionDate);


            dpSearchByDateFilter.DisplayDateStart = DateTime.Now.AddYears(-5);
            dpSearchByDateFilter.DisplayDateEnd = DateTime.Now;
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            btnClearFilter.Visibility = Visibility.Collapsed;
            dpSearchByDateFilter.SelectedDate = null;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            lstTransactions = Global.client.GetTransactions(Global.CurrentFleet.FleetID);
            foreach (var trn in lstTransactions)
            {
                trn.Car.Rule = Global.client.GetRuleByID(trn.Car.RuleID);
            }
            dgTransactions.ItemsSource = lstTransactions.OrderByDescending(x => x.TransactionDate);
        }

        private void dpSearchByDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? dateTimeFilter = dpSearchByDateFilter.SelectedDate;
            btnClearFilter.Visibility = (dateTimeFilter == null)? Visibility.Collapsed :Visibility.Visible;
            

            dgTransactions.ItemsSource = lstTransactions.Where(x => dateTimeFilter == null ||
                                                                    x.TransactionDate.ToString("yyyy-MM-dd") == 
                                                                    ((DateTime)dateTimeFilter).ToString("yyyy-MM-dd")) ;


        }
    }
}
