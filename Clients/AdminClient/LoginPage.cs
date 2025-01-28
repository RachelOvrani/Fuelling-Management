using Manager.FuelingServices;
using MaterialDesignDemo;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Manager
{
    public partial class LoginPage : Page
    {
        public List<User> lstUsers { get; set; }
        public LoginPage()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            lstUsers = Global.Client.GetUsers();
            this.DataContext = lstUsers;
            setTimeMessage();
            txtLoginName.Focus();
            

            // Convert hex to Color
            System.Drawing.Color defaultColor = System.Drawing.ColorTranslator.FromHtml(Global.DefaultColorHex);

            // Converting between different types of color
            var SystemWindowsMedia_Color = System.Windows.Media.Color.FromArgb(defaultColor.A, defaultColor.R, defaultColor.G, defaultColor.B);

            var paletteHelper = new PaletteHelper();
            paletteHelper.ChangePrimaryColor(SystemWindowsMedia_Color);
           

            //Task.Run(() =>
            //{
            //    Thread.Sleep(1000);
            //    Application.Current.Dispatcher.Invoke(() =>
            //    {
            //        NavigationService nav = NavigationService.GetNavigationService(this);
            //        nav.Navigate(new HomePage(lstUsers.First()));
            //    });
            //});

        }




        private void setTimeMessage()
        {
            var time = DateTime.Now;
            if (time.Hour >= 5 && time.Hour < 13)
                txtTimeMessage.Text = "בוקר טוב";

            else if (time.Hour >= 13 && time.Hour < 19)
                txtTimeMessage.Text = "צהרים טובים";

            else if (time.Hour >= 19 && time.Hour <= 23)
                txtTimeMessage.Text = "ערב טוב";

            else if (time.Hour >= 0 && time.Hour < 5)
                txtTimeMessage.Text = "לילה טוב";

        }




        private void CheckLoginIsValid()
        {
            if (string.IsNullOrWhiteSpace(txtLoginName.Text))
                return;


            User currentUser = lstUsers.FirstOrDefault(x => x.LoginName == txtLoginName.Text);

            // כשאין כזה משתמש
            if (currentUser == null)
            {
                txtMessage.Text = "לא קיים משתמש כזה במערכת.";
                return;
            }

            // האם המשתמש פעיל
            if (!currentUser.IsActive)
            {
                txtMessage.Text = "משתמש זה לא פעיל.";
                return;
            }
            // האם המשתמש הוא בתפקיד מנהל
            if (currentUser.Role != 1)
            {
                txtMessage.Text = "משתמש זה לא מוגדר כמנהל.";
                return;
            }

            // האם הסיסמה שהוקשה נכונה
            if (currentUser.LoginPassword != Global.EncodingPassword(pswLogin.Password))
            {
                txtMessage.Text = "סיסמה זו אינה נכונה";
                pswLogin.Password = "";
                pswLogin.Focus();
                return;
            }


            NavigationService nav = NavigationService.GetNavigationService(this); ;
            nav.Navigate(new HomePage(currentUser));

        }



        private void LoginName_KeyDown(object sender, KeyEventArgs e)
        {
            txtMessage.Text = "";

            if (!string.IsNullOrWhiteSpace(txtLoginName.Text))
                CheckUserProfil();

            if (e.Key == Key.Enter)
                CheckLoginIsValid();

        }

        private void CheckUserProfil()
        {
            User user = lstUsers.FirstOrDefault(x => x.LoginName == txtLoginName.Text);
            ImageSource imageSource;

            if (user == null || user.Picture == null)
            {
                FileInfo fi = new FileInfo(@"..\\..\\Images\\LoginProfil.png");
                imageSource = new BitmapImage(new Uri(fi.FullName));
                ProfilImg.Source = imageSource;
                return;
            }


            imageSource = ToImage(user.Picture);
            ProfilImg.Source = imageSource;

        }
        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }



        private void pswLogin_KeyDown(object sender, KeyEventArgs e)
        {
            txtMessage.Text = "";

            if (e.Key == Key.Enter)
                CheckLoginIsValid();
        }

        private void LoginName_KeyUp(object sender, KeyEventArgs e)
        {
            txtMessage.Text = "";

            if (!string.IsNullOrWhiteSpace(txtLoginName.Text))
                CheckUserProfil();

            if (e.Key == Key.Enter)
                CheckLoginIsValid();

        }
    }
}
