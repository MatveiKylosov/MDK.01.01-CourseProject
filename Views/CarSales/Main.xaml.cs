using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Views.Cars;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace MDK._01._01_CourseProject.Views.CarSales
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        private List<CarSale> _carSales;
        private ObservableCollection<CarSaleUserControl> CarSales { get; set; }
        public Main(List<Car> cars)
        {
            InitializeComponent();
            _carSales = _carSales ?? new List<CarSale>();
            CarSales = new ObservableCollection<CarSaleUserControl>();
            InitializeCarSales();
            CarSalesList.ItemsSource = CarSales;
        }

        public void InitializeCarSales()
        {
            CarSales.Clear();

            foreach (var carSale in _carSales)
                CarSales.Add(new CarSaleUserControl(carSale, this));
        }

        private void AddCarSale_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportCarSales_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FilterCarSales_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
