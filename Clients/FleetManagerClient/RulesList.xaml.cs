using Manager.FuelingManagmentService;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for RulesList.xaml
    /// </summary>
    public partial class RulesList : Page
    {
        public List<FuelingManagmentService.Rule> lstRules { get; set; }
        public RulesList()
        {
            InitializeComponent();
            this.lstRules = Global.fleetRules;
            dgLimitedProducts.ItemsSource = lstRules;
        }

        #region Searching

        private void txtX_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilter();
        }

        void UpdateFilter()
        {
            var ruleNameFilter = txtSearchByRuleName.Text.Trim();

            dgLimitedProducts.ItemsSource = lstRules.Where(x => (string.IsNullOrWhiteSpace(ruleNameFilter) || x.RuleName.Contains(ruleNameFilter)));

            btnClearFilter.Visibility = (ruleNameFilter == "") ? Visibility.Collapsed : Visibility.Visible;
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtSearchByRuleName.Text = "";
            btnClearFilter.Visibility = Visibility.Collapsed;
        }

        #endregion


        #region Add Update Delete

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            frmRule.Navigate(new AddRule());
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (dgLimitedProducts.SelectedItem == null)
                return;

            var p = dgLimitedProducts.SelectedItem as FuelingManagmentService.Rule;

            frmRule.Navigate(new AddRule(p));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            FuelingManagmentService.Rule r = dgLimitedProducts.SelectedItem as FuelingManagmentService.Rule;


           int relatedCars = Global.client.GetFleetCars(Global.CurrentFleet.FleetID).
                                   Where(x => x.RuleID == r.RuleID).Count();

            string message = $"לחוק זה מחוברים {relatedCars} רכבים\nאם תמחק חוק זה כל אותם רכבים ישארו ללא חוק.\nהאם בכל זאת אתה רוצה למחוק?";

                var result = MessageBox.Show(message, "התראה", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.RtlReading);
            if (result == MessageBoxResult.No)
                return;

            if (dgLimitedProducts.SelectedItem == null)
                return;


            Global.client.DeleteRule(r);


            // אחרי המחיקה של חוק יש גם לעדכן את רשימת החוקים הגלובלית בצד זה
            Global.fleetRules = Global.client.GetFleetRules(Global.CurrentFleet.FleetID);


            lstRules = Global.fleetRules;
            dgLimitedProducts.ItemsSource = lstRules;
        }

        #endregion

        private void dgLimitedProducts_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgLimitedProducts.SelectedItem == null)
                return;

            FuelingManagmentService.Rule rule = dgLimitedProducts.SelectedItem as FuelingManagmentService.Rule;
            frmRule.Navigate(new AddRule(rule));
        }

        private void dgLimitedProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdate.IsEnabled = dgLimitedProducts.SelectedItem != null;
            btnDelete.IsEnabled = dgLimitedProducts.SelectedItem != null;
        }

        
    }
}
