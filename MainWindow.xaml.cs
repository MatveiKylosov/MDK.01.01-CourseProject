using MDK._01._01_CourseProject.Models;
using OfficeOpenXml;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MDK._01._01_CourseProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Customer customer = null;
        public MainWindow()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void OpenBrands_Click(object sender, RoutedEventArgs e)
        {
            TableName.Content = "Таблица: Марки";
            Frame.Navigate(new Views.Brands.Main(customer != null));
        }

        private void OpenCars_Click(object sender, RoutedEventArgs e)
        {
            TableName.Content = "Таблица: Автомобили";
            Frame.Navigate(new Views.Cars.Main(customer != null));
        }

        private void OpenSales_Click(object sender, RoutedEventArgs e)
        {
            TableName.Content = "Таблица: Продажи";
            Frame.Navigate(new Views.CarSales.Main(customer));
        }

        private void OpenCustomers_Click(object sender, RoutedEventArgs e)
        {
            TableName.Content = "Таблица: Клиенты";
            Frame.Navigate(new Views.Customers.Main(customer));
        }

        private void OpenEmployees_Click(object sender, RoutedEventArgs e)
        {
            TableName.Content = "Таблица: Сотрудники";
            Frame.Navigate(new Views.Employees.Main());
        }
        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            var employee = Repository.RepositoryEmployee.GetEmployees().FirstOrDefault(x => x.FullName == Login.Text && x.Password == Password.Password);
            var customer = Repository.RepositoryCustomer.GetCustomers().FirstOrDefault(x => x.FullName == Login.Text && x.Password == Password.Password);

            if (employee == null && customer == null)
            {
                MessageBox.Show($"Ошибка при авторизации.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (employee != null && customer != null)
                if (MessageBox.Show($"Вы хотите зайти под сотрудником?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    this.customer = customer;

            if (employee == null && customer != null)
                this.customer = customer;

            if (this.customer != null)
            {
                OpenEmployees.Height = OpenEmployees.Width = 0;
                OpenCustomers.Content = "Профиль";
            }
            else
            {
                OpenEmployees.Height = 25;
                OpenEmployees.Width = 70;
                OpenCustomers.Content = "Клиенты";
            }

            AuthGrid.Visibility = Visibility.Hidden;
        }

        private void ReturnAuth_Click(object sender, RoutedEventArgs e)
        {
            customer = null;
            this.Frame.Navigate(null);
            AuthGrid.Visibility = Visibility.Visible;
        }
    }
}
