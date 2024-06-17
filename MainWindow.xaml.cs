using OfficeOpenXml;
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

namespace MDK._01._01_CourseProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Views.Brands.Main());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Views.Cars.Main());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Views.CarSales.Main());
        } 

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Views.Customers.Main());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Views.Employees.Main());
        }
    }
}
