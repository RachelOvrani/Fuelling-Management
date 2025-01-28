using Manager.FuelingServices;
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

namespace Manager
{
    /// <summary>
    /// Interaction logic for AddContactPerson.xaml
    /// </summary>
    public partial class AddContactPerson : Window
    {
        public Fleet currentFleet { get; set; }
        public User user { get; set; }
        public AddContactPerson(Fleet fleet)
        {
            InitializeComponent();
            currentFleet = fleet;
            Loaded += AddContactPerson_Loaded;
        }

        private void AddContactPerson_Loaded(object sender, RoutedEventArgs e)
        {
            
            user = new User();
            this.DataContext = user;
            this.dgContactPeople.ItemsSource = currentFleet.Users;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFullDitails())
            {
                txtErorr.Visibility = Visibility.Visible;
                return;
            }

            // במקרה שפרטי איש קשר קיים עודכנו
            if (currentFleet.Users.Contains(user))
            {
                Reset();
                return;
            }


            // במקרה של הוספת איש קשר חדש
            currentFleet.Users.Add(user);
            this.dgContactPeople.Items.Refresh();
            Reset();
            btnAdd.Visibility = Visibility.Visible;
        }

        private void Reset()
        {
            user = new User();
            this.DataContext = user;
        }

        private bool CheckFullDitails()
        {
            var FirstName = (user.FirstName == null) ? null : user.FirstName.Trim();
            var LastName = (user.LastName == null) ? null : user.LastName.Trim();
            var PhonNumber = (user.PhoneNumber == null) ? null : user.PhoneNumber.Trim();
            var ID = (user.ID == null) ? null : user.ID.Trim();


            // אם יש שדות ריקים
            if(txtFirstName.Text == txtFirstName.Watermark || txtFirstName.Text == "" 
                || txtLastName.Text == txtLastName.Watermark || txtLastName.Text == ""
                || txtPhonNumber.Text == ""
                || txtID.Text == "")
            {
                txtErorr.Text = "יש למלא את כל השדות.";
                return false;
            }



            // validation
            if (FirstName == null || txtFirstName.Text != FirstName
                || LastName == null || txtLastName.Text != LastName
                || PhonNumber == null || txtPhonNumber.Text != PhonNumber
                || ID == null || txtID.Text != ID)
            {
                txtErorr.Text = "";
                return false;
            }


            //if (Global.adminClient.GetUsers().Where(x => x.LoginName == user.ID && x.FleetID != this.currentFleet.FleetID).Count() != 0)
            //{
            //    txtPhonNumber.Text = "";
            //    txtErorr.Text = "";
            //    return false;
            //}

           

            // כשאיש קשר זה כבר קיים
            List<User> us = Global.Client.GetUsers().Where(x => x.ID == user.ID && x.FleetID == this.currentFleet.FleetID).ToList();
            if (us.Count() != 0 && user.UserID == 0)
            {
                txtErrorExistingUser.Visibility = Visibility.Visible;
                return false;
            }
            return true;
        }

        private void btnAdd_LostFocus(object sender, RoutedEventArgs e)
        {
            txtErorr.Visibility = Visibility.Hidden;
            txtErrorExistingUser.Visibility = Visibility.Hidden;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void miUpdate_Click(object sender, RoutedEventArgs e)
        {
            user = this.dgContactPeople.SelectedItem as User;
            this.DataContext = user;
            txtLastName.Focus();
            txtFirstName.Focus();
            btnAdd.Visibility = Visibility.Collapsed;
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = this.dgContactPeople.SelectedItem as User;

            User userToDelete = Global.Client.GetUsers().FirstOrDefault(x => x.UserID == selectedUser.UserID);
            if (userToDelete != null)
                Global.Client.DeleteUser(userToDelete);

            currentFleet.Users.Remove(selectedUser);
            dgContactPeople.Items.Refresh();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            user = new User();
            DataContext = user;
            btnAdd.Visibility = Visibility.Visible;
            txtLastName.Focus();
            txtFirstName.Focus();

        }
    }
}
