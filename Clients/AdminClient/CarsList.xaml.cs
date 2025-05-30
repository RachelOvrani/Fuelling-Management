﻿using Manager.FuelingServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Manager
{
    /// <summary>
    /// Interaction logic for CarsList.xaml
    /// </summary>
    public partial class CarsList : Page
    {
        public Car car { get; set; }
        public CarsList(Fleet fleet)
        {            
            InitializeComponent();
            car = new Car();
            this.DataContext = car;
            dgCars.ItemsSource = Global.Client.GetFleetCars(fleet.FleetID);
            txtHeader.Text = "הרכבים בחברת " + fleet.CompanyName;
            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new FleetsList());
        }
    }
}
