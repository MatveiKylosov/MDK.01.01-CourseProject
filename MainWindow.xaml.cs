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
            Frame.Navigate(new Views.Brands.Main(Repository.RepositoryBrand.GetBrands()));
            Frame.Navigate(new Views.Cars.Main(Repository.RepositoryCar.GetCars()));
            Frame.Navigate(new Views.CarSales.Main(Repository.RepositoryCarSale.GetCarSales()));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
    }
}
