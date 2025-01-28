using Manager.FuelingManagmentService;
using MaterialDesignDemo;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Manager
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : Page
    {
        public string tmpPassword { get; set; }
        public Setting()
        {
            InitializeComponent();
            this.DataContext = Global.CurrentUser;
            tmpPassword = EncrpytionHelper.DecodingPassword(Global.CurrentUser.LoginPassword);
            pswPassword.Password = tmpPassword;
            
            if (Global.CurrentUser.WorkspaceColor != null && Global.CurrentUser.WorkspaceColor.ToString() != "Color [Empty]")
            {
                // Converting between different types of color
                var systemDrowing_Color = Global.CurrentUser.WorkspaceColor;
                var SystemWindowsMedia_Color = System.Windows.Media.Color.FromArgb(systemDrowing_Color.A, systemDrowing_Color.R, systemDrowing_Color.G, systemDrowing_Color.B);

                colorPicker.SelectedColor = SystemWindowsMedia_Color;
            }
            else
            {
                // Convert hex to Color
                System.Drawing.Color defaultColor = System.Drawing.ColorTranslator.FromHtml(Global.DefaultColorHex);

                // Converting between different types of color
                var SystemWindowsMedia_Color = System.Windows.Media.Color.FromArgb(defaultColor.A, defaultColor.R, defaultColor.G, defaultColor.B);

                colorPicker.SelectedColor = SystemWindowsMedia_Color;
            }


        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (!CheckFullDetails())
            {
                txtErorr.Visibility = Visibility.Visible;
                return;
            }


            if (CheckConflict())
            {
                txtLoginNameErorr.Visibility = Visibility.Visible;
                txtLoginName.Text = "";
                txtLoginName.Focus();
                return;
            }

            // הצפנת הסיסמה החדשה והכנסתה לביבמת המשתמש
            Global.CurrentUser.LoginPassword = EncrpytionHelper.EncodingPassword(tmpPassword);
            // שליחת אובייקט המשתמש לעדכון
            Global.client.UpdateUser(Global.CurrentUser);


            
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new HomePage());
        }

        private bool CheckFullDetails()
        {
            // במידה ואחד השדות הנדרשים ריקים
            if (txtLoginName.Text.Trim() == ""
                || txtPhoneNumber.Text.Trim() == "")
                //|| pswPassword.Password.Trim() == "") הוא גם יכול להשאיר סיסמה ריקה
            {
                txtErorr.Text = "יש למלא את השדות החסרים";
                return false;
            }

            // Validation -במקרה שהם לא עוברים את ה
            if (Global.CurrentUser.LoginName == null || Global.CurrentUser.LoginName != txtLoginName.Text
                || Global.CurrentUser.PhoneNumber == null || Global.CurrentUser.PhoneNumber != txtPhoneNumber.Text)
            {
                txtErorr.Text = "";
                return false;
            }

            return true;
        }

        private bool CheckConflict()
        {
            List<User> lstUsers = Global.client.GetUsers();
            foreach (User user in lstUsers)
            {
                if (user.UserID == Global.CurrentUser.UserID)
                    continue;

                if (Global.CurrentUser.LoginName == user.LoginName)
                {
                    txtLoginNameErorr.Text = "שם זה כבר שמור במערכת";
                    return true;
                }


            }

            return false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new HomePage());
        }



        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            txtErorr.Visibility = Visibility.Hidden;
        }

        private void btnSave_LostFocus(object sender, RoutedEventArgs e)
        {
            txtErorr.Visibility = Visibility.Collapsed;
        }

        private void btnUploadImg_Click(object sender, RoutedEventArgs e)
        {
            // פתיחת סייר הקבצים
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
                imgProfil.Source = imageSource;

                // שמירת שם התמונה
                Global.CurrentUser.PicturePath = ofd.SafeFileName;
                // שמירת התמונה ב Byte[]
                Global.CurrentUser.Picture = File.ReadAllBytes(ofd.FileName);
            }
        }

        private void txtLoginName_KeyDown(object sender, KeyEventArgs e)
        {
            txtLoginNameErorr.Visibility = Visibility.Hidden;
        }

        private void pswPassword_KeyUp(object sender, KeyEventArgs e)
        {
            txtErorr.Visibility = Visibility.Hidden;
            tmpPassword = pswPassword.Password;
        }

        private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            colorPicker.ColorState.GetType();
            PaletteHelper paletteHelper = new MaterialDesignThemes.Wpf.PaletteHelper();
            Color selectedColor = colorPicker.SelectedColor;
            paletteHelper.ChangePrimaryColor(selectedColor);
            //paletteHelper.ChangeSecondaryColor(selectedColor);

            // Converting between different types of color
            System.Drawing.Color color = System.Drawing.Color.FromArgb(selectedColor.A, selectedColor.R, selectedColor.G, selectedColor.B);

            Global.CurrentUser.WorkspaceColor = color;
        }
    }
}
