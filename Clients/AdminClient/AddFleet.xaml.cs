using Manager.FuelingServices;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Manager
{
    /// <summary>
    /// Interaction logic for AddFleet.xaml
    /// </summary>
    public partial class AddFleet : Page
    {
        public List<Fleet> lstFleets { get; set; }
        public Fleet fleet { get; set; }
        public AddFleet(Fleet fleet)
        {
            InitializeComponent();
            this.fleet = fleet;
            this.DataContext = fleet;
            

            if (fleet.FleetID != 0)
                txtHeader.Text = "עדכון צי קיים";

            lstFleets = Global.Client.GetFleets();
        }
        public AddFleet() : this(new Fleet())
        {
            this.fleet.Users = new List<User>();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (!CheckFullDitails())
            {
                txtErorrMassage.Visibility = Visibility.Visible;
                return;
            }

            if (fleet.FleetID == 0) // new fleet
            {
                Global.Client.AddFleet(fleet);
                List<User> lstUsers = new List<User>();
                foreach (var user in fleet.Users)
                {
                    lstUsers.Add(CreateUser(user));
                }

                Global.Client.AddUsersToFleet(lstUsers);
            }
            else // update fleet
            {
                List<User> to_add = new List<User>();

                foreach (var user in fleet.Users)
                {
                    // איש קשר חדש שנוסף
                    if (user.UserID == 0)
                    {
                        user.FleetID = fleet.FleetID;
                        user.Role = 2;
                        user.LoginName = user.ID;
                        to_add.Add(user);
                    }
                    // איש קשר קיים שהתעדכן
                    else
                    {
                        Global.Client.UpdateUser(user);
                    }
                }
                if (to_add.Count != 0)
                    Global.Client.AddUsersToFleet(to_add);

                Global.Client.UpdateFleet(fleet);

            }

            
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new FleetsList());
        }



        private bool CheckFullDitails()
        {

            // כשצי זה כבר קיים
            if (txtExistCompanyErorr.Visibility == Visibility.Visible)
                return false;


            // כשלא כל השדות מלאים
            if (string.IsNullOrWhiteSpace(fleet.CompanyName)
                || Convert.ToInt32(fleet.Amount) <= 0)

            {
                txtErorrMassage.Text = "יש למלא את כל השדות.";
                return false;
            }

            // Validations כשלא עבר את ה  
            double i;
            if (!double.TryParse(txtAmount.Text, out i) || i != fleet.Amount
                || !double.TryParse(txtDiscount.Text, out i) || i != fleet.Discount
                || !double.TryParse(txtCredit.Text, out i) || i != fleet.Credit)
            {
                txtErorrMassage.Text = "יש למלא את כל השדות.";
                return false;
            }

            if (fleet.Users.Count == 0)
            {
                txtErorrMassage.Text = "חובה להוסיף לפחות איש קשר אחד.";
                return false;
            }


            return true;
        }

        private User CreateUser(User user)
        {
            int newFleet_ID = Global.Client.GetFleets().Max(x => x.FleetID);
            user.FleetID = newFleet_ID;
            user.Role = 2;
            user.LoginName = user.ID;
            user.IsActive = true;
            return user;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new FleetsList());
        }

        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "All Images | *.jpg;*.jpeg;*.tif;*.tiff;*.bmp;*.png;" +
                "|JPEG Files (*.jpeg)|*.jpeg" +
                "|PNG Files (*.png)|*.png" +
                "|JPG Files (*.jpg)|*.jpg" +
                "|GIF Files (*.gif)|*.gif";

            ofd.Multiselect = false;
            var result = ofd.ShowDialog();

            if (result == true)
            {
                ImageSource imageSource = new BitmapImage(new Uri(ofd.FileName));
                imgLogo.Source = imageSource;

                fleet.LogoPath = ofd.SafeFileName;

                fleet.Logo = File.ReadAllBytes(ofd.FileName);
            }
        }

        private void btnSave_LostFocus(object sender, RoutedEventArgs e)
        {
            txtErorrMassage.Visibility = Visibility.Collapsed;
        }


        private void btnAddContactPersons_Click(object sender, RoutedEventArgs e)
        {

            AddContactPerson addContactPerson = new AddContactPerson(this.DataContext as Fleet);

            addContactPerson.Owner = Application.Current.MainWindow;
            addContactPerson.ShowInTaskbar = false;
            addContactPerson.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addContactPerson.ShowDialog();


            this.txtCountUsers.Text = fleet.Users.Count.ToString();
        }

        private void txtCompanyName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (lstFleets.Where(x => x.CompanyName == txtCompanyName.Text && x.FleetID != fleet.FleetID).Count() != 0)
                txtExistCompanyErorr.Visibility = Visibility.Visible;
        }

        private void txtCompanyName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtExistCompanyErorr.Visibility = Visibility.Hidden;
        }
    }
}
