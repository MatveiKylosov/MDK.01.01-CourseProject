using OfficeOpenXml;
using System.Windows;
using System.Windows.Controls;

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

        private void OpenBrands_Click(object sender, RoutedEventArgs e)
        {
            TableName.Content = "Таблица: Бренды";
            Frame.Navigate(new Views.Brands.Main());
        }

        private void OpenCars_Click(object sender, RoutedEventArgs e)
        {
            TableName.Content = "Таблица: Машины";
            Frame.Navigate(new Views.Cars.Main());
        }

        private void OpenSales_Click(object sender, RoutedEventArgs e)
        {
            TableName.Content = "Таблица: Продажи";
            Frame.Navigate(new Views.CarSales.Main());
        }

        private void OpenCustomers_Click(object sender, RoutedEventArgs e)
        {
            TableName.Content = "Таблица: Клиенты";
            Frame.Navigate(new Views.Customers.Main());
        }

        private void OpenEmployees_Click(object sender, RoutedEventArgs e)
        {
            TableName.Content = "Таблица: Сотрудники";
            Frame.Navigate(new Views.Employees.Main());
        }
    }
}
