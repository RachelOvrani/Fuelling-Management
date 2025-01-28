using Manager.FuelingManagmentService;
using MaterialDesignDemo;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Input;

namespace Manager
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            DataContext = Global.CurrentUser;


            if (Global.CurrentUser.WorkspaceColor != null && Global.CurrentUser.WorkspaceColor.ToString() != "Color [Empty]")
            {
                // Converting between different types of color
                var systemDrowing_Color = Global.CurrentUser.WorkspaceColor;
                var SystemWindowsMedia_Color = System.Windows.Media.Color.FromArgb(systemDrowing_Color.A, systemDrowing_Color.R, systemDrowing_Color.G, systemDrowing_Color.B);

                PaletteHelper paletteHelper = new MaterialDesignThemes.Wpf.PaletteHelper();
                paletteHelper.ChangePrimaryColor(SystemWindowsMedia_Color);
                //paletteHelper.ChangeSecondaryColor(SystemWindowsMedia_Color);
            }
            
            
        }


        int lastTabIndex = -1;
        private void TabControl_SelectedChange(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != sender)
                return;

            if (tabControl.SelectedIndex == lastTabIndex)
                return;
            lastTabIndex = tabControl.SelectedIndex;


            if (tabControl.SelectedIndex == 0)
            {
                int workersCarsAmount = Global.client.GetFleetCarsAmount(Global.CurrentFleet.FleetID);
                txtAmount.Text = $"{workersCarsAmount} רכבי עובדים";
                frmWorkersCars.Navigate(new WorkersCarsList());
            }

            else if (tabControl.SelectedIndex == 1)
            {
                int rulesAmount = Global.client.GetFleetRulesAmount(Global.CurrentFleet.FleetID);
                txtAmount.Text = $"{rulesAmount} חוקים";
                frmRules.Navigate(new RulesList());
            }

            else if (tabControl.SelectedIndex == 2)
            {
                int stationsAmount = Global.client.GetStationsAmount();
                txtAmount.Text = $"{stationsAmount} תחנות";
                frmStations.Navigate(new StationsList());
            }

            else if (tabControl.SelectedIndex == 3)
            {
                int transactinsAmount = Global.client.GetTransactions(Global.CurrentFleet.FleetID).Count;
                txtAmount.Text = $"{transactinsAmount} עסקות";
                frmTransactions.Navigate(new TransactionsList());
            }

            else if (tabControl.SelectedIndex == 4)
            {
                int rejectedRequestsAmount = Global.client.GetRejectedRequests(Global.CurrentFleet.FleetID).Count;
                txtAmount.Text = $"{rejectedRequestsAmount} דחיות";
                frmRejectedRequests.Navigate(new RejectedRequestsList());
            }
        }
        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            frmSetting.Navigate(new Setting());
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            frmLogout.Navigate(new LoginPage());
        }
    }

}
