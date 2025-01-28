using Manager.FuelingServices;
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
        public User CurrentUser { get; set; }
        public HomePage(User user)
        {
            InitializeComponent();
            this.CurrentUser = user;
            DataContext = user;


            if (CurrentUser.WorkspaceColor != null && CurrentUser.WorkspaceColor.ToString() != "Color [Empty]")
            {
                // Converting between different types of color
                var systemDrowing_Color = CurrentUser.WorkspaceColor;
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
                int stationsAmount = Global.Client.GetStationsAmount();
                txtAmount.Text = $"{stationsAmount} תחנות";
                frmStations.Navigate(new StationsList());
            }

            else if (tabControl.SelectedIndex == 1)
            {
                int productsAmount = Global.Client.GetProductsAmount();
                txtAmount.Text = $"{productsAmount} מוצרים";
                frmProducts.Navigate(new ProductsLits());
            }

            else if (tabControl.SelectedIndex == 2)
            {
                int fleetsAmount = Global.Client.GetFleetsAmount();
                txtAmount.Text = $"{fleetsAmount} ציים";
                frmFleets.Navigate(new FleetsList());
            }
        }
        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            frmSetting.Navigate(new Setting(CurrentUser));
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            frmLogout.Navigate(new LoginPage());
        }
    }

}
