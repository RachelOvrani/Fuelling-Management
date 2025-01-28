using Manager.FuelingManagmentService;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System;
using System.Data.Common;

namespace Manager
{

    public partial class RejectedRequestsList : Page
    {
        public List<RejectedRequest> lstRejectedRequests { get; set; }
        public RejectedRequestsList()
        {
            InitializeComponent();

            lstRejectedRequests = Global.client.GetRejectedRequests(Global.CurrentFleet.FleetID);
            foreach (var trn in lstRejectedRequests)
            {
                trn.Car.Rule = Global.client.GetRuleByID(trn.Car.RuleID);
            }
            dgRejectedRequests.ItemsSource = lstRejectedRequests.OrderByDescending(x => x.RejectedRequestDate);
                                                                

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
            lstRejectedRequests = Global.client.GetRejectedRequests(Global.CurrentFleet.FleetID);
            foreach (var trn in lstRejectedRequests)
            {
                trn.Car.Rule = Global.client.GetRuleByID(trn.Car.RuleID);
            }
            dgRejectedRequests.ItemsSource = lstRejectedRequests.OrderByDescending(x => x.RejectedRequestDate); ;
        }

        private void dpSearchByDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? dateTimeFilter = dpSearchByDateFilter.SelectedDate;
            btnClearFilter.Visibility = (dateTimeFilter == null) ? Visibility.Collapsed : Visibility.Visible;


            dgRejectedRequests.ItemsSource = lstRejectedRequests.Where(x => dateTimeFilter == null ||
                                                                    x.RejectedRequestDate.ToString("yyyy-MM-dd") ==
                                                                    ((DateTime)dateTimeFilter).ToString("yyyy-MM-dd"));
        }
    }
}
