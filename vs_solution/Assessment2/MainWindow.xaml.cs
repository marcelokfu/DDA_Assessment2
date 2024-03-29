﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Assessment2
{
    /// <summary>
    /// INTERACTION LOGIC FOR MAINWINDOW.XAML
    /// </summary>
    public partial class MainWindow
    {
        public static bool bDatabaseStatus = false;
        public static long statusCount = 0;
        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            UpdateList();

            VehicleIC.Title = "Vehicles";
            VehicleIC.BackgroundColour = "Black";

            RentalIC.Title = "Rentals";
            RentalIC.BackgroundColour = "Black";

            TotalRentalIC.Title = "Total Revenue";
            TotalRentalIC.BackgroundColour = "Black";

            AvailableVehiclesIC.Title = "Avaiable Vehicles";
            AvailableVehiclesIC.BackgroundColour = "Black";
        }
        /// <summary>
        /// UPDATE THE LIST VIEW ACCORDING TO THE FILTER
        /// </summary>
        public void UpdateList()
        {
            VehicleIC.ValueProperty = Vehicle.vehicleList.Count.ToString();
            RentalIC.ValueProperty = Rental.rentalList.Count.ToString();
            AvailableVehiclesIC.ValueProperty = Rental.GetAvailableVehicles().Count.ToString();
            TotalRentalIC.ValueProperty = Rental.GetTotalRevenue().ToString("C");
        }
        /// <summary>
        /// ON BTN VEHICLE CLICK OPEN THE VEHICLE LIST FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiVehicles_Click(object sender, RoutedEventArgs e)
        {
            if (lockDBAccess())
            {
                Form_VehicleList form_VehicleList = new Form_VehicleList();
                form_VehicleList.ShowDialog();
                UpdateList();
            }
        }

        /// <summary>
        /// ON BTN RENT CLICK OPEN THE RENTAL LIST FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiRents_Click(object sender, RoutedEventArgs e)
        {
            if (lockDBAccess())
            {
                Form_RentalList form_RentalList = new Form_RentalList();
                form_RentalList.ShowDialog();
                UpdateList();
            }
        }

        /// <summary>
        /// ON THE BTN CANCEL CLICK CLOSE THE FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// MENU ITEM ABOUT CLICK EVENT - OPEN THE ABOUT PAGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }
        /// <summary>
        /// VEHICE DOUBLE INFORMATION CARD CLICK EVENT - OPEN THE VEHICE PAGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VehicleIC_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lockDBAccess())
            {
                Form_VehicleList form_VehicleList = new Form_VehicleList();
                form_VehicleList.ShowDialog();
                UpdateList();
            }
        }
        /// <summary>
        /// RENTAL DOUBLE INFORMATION CARD CLICK EVENT - OPEN THE RENTAL PAGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RentalIC_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lockDBAccess())
            {
                Form_RentalList form_RentalList = new Form_RentalList();
                form_RentalList.ShowDialog();
                UpdateList();
            }
        }
        /// <summary>
        /// AVAILABLE VEHICLE INFORMATION CARD DOUBLE CLICK EVENT - OPEN THE AVAILABLE VEHICLE PAGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AvailableVehiclesIC_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lockDBAccess())
            {
                Form_AvailableVehicles form_AvailableVehicles = new Form_AvailableVehicles();
                form_AvailableVehicles.ShowDialog();
                UpdateList();
            }
        }
        /// <summary>
        /// METHOD CHECK IF THE DATABASE IS AVAILALBE, OTHERWISE WARN THE USER AND DON'T LET HIM OPEN ANY OTHER PAGE
        /// </summary>
        /// <returns></returns>
        public bool lockDBAccess()
        {
            if (!bDatabaseStatus)
            {
                MessageBox.Show("Database is unavailable, please contact support", "Database", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }
            return true;
        }
        /// <summary>
        /// WINDOW MOUSE MOVE EVENT - UPDATE THE MOUSE POINT TEXT AND THE DATABASE STATUS TEXT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            txtCursorPositionLabel.Text = string.Format("{0}x{1}", e.GetPosition(DashboardWindow).Y, e.GetPosition(DashboardWindow).X);

            // don't check every mouse move
            if (statusCount % 100 == 0)
            {
                bDatabaseStatus = Sql.TestConnection();

                if (bDatabaseStatus)
                {
                    txtDatabaseStatus.Text = "DB Status: Online";
                    txtDatabaseStatus.Foreground = Brushes.GreenYellow;
                }
                else
                {
                    txtDatabaseStatus.Text = "DB Status: Offiline";
                    txtDatabaseStatus.Foreground = Brushes.Red;
                }
            }
            statusCount++;
        }
    }
}
