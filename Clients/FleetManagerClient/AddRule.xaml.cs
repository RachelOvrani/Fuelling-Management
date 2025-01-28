using Manager.FuelingManagmentService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Collections.Specialized.BitVector32;

namespace Manager
{
    /// <summary>
    /// Interaction logic for AddRule.xaml
    /// </summary>
    public partial class AddRule : Page
    {
        public List<ProductLimit> lstProductsLimit { get; set; }
        public Rule currentRule { get; set; }


        public int RelatedCarsCount
        {
            get { return (int)GetValue(RelatedCarsCountProperty); }
            set { SetValue(RelatedCarsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RelatedCarsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RelatedCarsCountProperty =
            DependencyProperty.Register("RelatedCarsCount", typeof(int), typeof(AddRule), new PropertyMetadata(0));




        public int RelatedStationsCount
        {
            get { return (int)GetValue(RelatedStationsCountProperty); }
            set { SetValue(RelatedStationsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RelatedStationsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RelatedStationsCountProperty =
            DependencyProperty.Register("RelatedStationsCount", typeof(int), typeof(AddRule), new PropertyMetadata(0));




        public AddRule(Rule rule)
        {
            InitializeComponent();
            currentRule = rule;
            this.DataContext = currentRule;
            
            if (currentRule.LimitedProducts == null)
                lstProductsLimit = new List<ProductLimit>();
            else
                lstProductsLimit = currentRule.LimitedProducts;

            dgLimitedProducts.ItemsSource = lstProductsLimit;
            
            productsColumn.ItemsSource = Global.client.GetProducts();

            Loaded += AddRule_Loaded;
            

        }

        public AddRule() : this(new Rule()) { }


        private void AddRule_Loaded(object sender, RoutedEventArgs e)
        {
            txtRuleName.Focus();

            var list = Global.client.GetTags();
            this.lstbTags.ItemsSource = list;

            if (currentRule.RuleID != 0)
            {
                txtHeader.Text = "עדכון חוק קיים ";
                var selectedTags = list.Where(x => currentRule.Tags.Exists(y => y.TagID == x.TagID));

                foreach (Tag tag in selectedTags)
                {
                    this.lstbTags.SelectedItems.Add(tag);
                }

                lstbTags_SelectionChanged(new object(), null);

                RelatedCarsCount = Global.client.GetFleetCars(Global.CurrentFleet.FleetID).
                                   Where(x => x.RuleID == currentRule.RuleID).Count();

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            lstProductsLimit.Add(new ProductLimit());
            dgLimitedProducts.ItemsSource = null;
            dgLimitedProducts.ItemsSource = lstProductsLimit;

        }

        private void btnDeleteLimitedProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductLimit toDelete = (dgLimitedProducts.SelectedItem as ProductLimit);
            lstProductsLimit.Remove(toDelete);
            dgLimitedProducts.ItemsSource = null;
            dgLimitedProducts.ItemsSource = lstProductsLimit;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new RulesList());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var tags = new List<Tag>(lstbTags.SelectedItems.Cast<Tag>());
            currentRule.Tags = tags;

            currentRule.LimitedProducts =(dgLimitedProducts.ItemsSource as List<ProductLimit>);


            // new Rule
            if (currentRule.RuleID == 0)
            {
                currentRule.FleetID = Global.CurrentFleet.FleetID;
                Global.client.AddRule(currentRule);
            }
            // update Rule
            else
            {
                Global.client.UpdateRule(currentRule);
            }

            // אחרי הוספה או עידכון של חוק יש גם לעדכן את רשימת החוקים הגלובלית בצד זה
            Global.fleetRules = Global.client.GetFleetRules(Global.CurrentFleet.FleetID);

            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new RulesList());
        }

        private void lstbTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstbTags.SelectedItems == null || lstbTags.SelectedItems.Count == 0)
            {
                RelatedStationsCount = 0;
                return;
            }

            List<string> lstSelectedTags = new List<string>();
            foreach (Tag tag in lstbTags.SelectedItems)
            {
                lstSelectedTags.Add(tag.TagName);
            }


            RelatedStationsCount = Global.client.GetStations().Where(x=> Contain(x, lstSelectedTags)).Count();
        }

        private bool Contain(Station x , List<string> lstSelectedTags)
        {
            List<string> x_namesTags = x.Tags.Select(y => y.TagName).ToList();

            foreach (string tagName in lstSelectedTags)
            {
                if (!x_namesTags.ToList().Contains(tagName))
                    return false;
            }
            return true;
        }
    }
}
