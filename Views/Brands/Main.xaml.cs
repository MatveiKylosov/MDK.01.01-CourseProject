using MDK._01._01_CourseProject.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MDK._01._01_CourseProject.Views.Brands
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        private ObservableCollection<BrandUserControl> Brands { get; set; }
        public Main(List<Brand> brands)
        {
            InitializeComponent();
            Brands = new ObservableCollection<BrandUserControl>();
            foreach (Brand brand in brands)
                Brands.Add(new BrandUserControl(brand, this));
            
            BrandList.ItemsSource = Brands;
        }
        public void RemoveBrand(BrandUserControl brand)
        {
            Brands.Remove(brand);
        }
    }
}
