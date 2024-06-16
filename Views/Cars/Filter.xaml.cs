using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MDK._01._01_CourseProject.Views.Cars
{
    /// <summary>
    /// Логика взаимодействия для Filter.xaml
    /// </summary>
    public partial class Filter : Window
    {
        public int SelectedBrandID
        {
            get
            {
                if (BrandComboBox.SelectedItem is ComboBoxItem selectedItem)
                    return (int)selectedItem.Tag;
                
                return -1;
            }
            set
            {
                foreach (ComboBoxItem item in BrandComboBox.Items)
                {
                    if ((int)item.Tag == value)
                    {
                        BrandComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public int EnteredFirstDate
        {
            get => int.TryParse(FirstDate.Text, out int date) ? date : 0;
            set => FirstDate.Text = value.ToString();
        }

        public int EnteredSecondDate
        {
            get => int.TryParse(SecondDate.Text, out int date) ? date : 0;
            set => SecondDate.Text = value.ToString();
        }

        public decimal EnteredFirstPrice
        {
            get => decimal.TryParse(FirtPrice.Text, out decimal price) ? price : 0;
            set => FirtPrice.Text = value.ToString();
        }

        public decimal EnteredSecondPrice
        {
            get => decimal.TryParse(SecondPrice.Text, out decimal price) ? price : 0;
            set => SecondPrice.Text = value.ToString();
        }

        public string SelectedColor
        {
            get => ColorComboBox.SelectedItem as string;
            set => ColorComboBox.SelectedItem = value;
        }

        public string SelectedCategory
        {
            get => CategoryComboBox.SelectedItem as string;
            set => CategoryComboBox.SelectedItem = value;
        }

        public Filter(List<string> _color, List<string> _category, bool filterUse)
        {
            InitializeComponent();
            ActiveFilter.IsChecked = filterUse;
            SecondDate.IsEnabled = FirstDate.IsEnabled = ColorComboBox.IsEnabled = 
                CategoryComboBox.IsEnabled = FirtPrice.IsEnabled = SecondPrice.IsEnabled = filterUse;
            InitializeComboBoxes(_color, _category);
        }

        private void InitializeComboBoxes(List<string> _color, List<string> _category)
        {
            ColorComboBox.Items.Clear();
            CategoryComboBox.Items.Clear();
            BrandComboBox.Items.Clear();


            ColorComboBox.Items.Add("Не выбран.");
            CategoryComboBox.Items.Add("Не выбран.");
            var defaultItem = new ComboBoxItem { Content = "Не выбран.", Tag = -1 };
            BrandComboBox.Items.Add(defaultItem);

            foreach (var color in _color)      
                ColorComboBox.Items.Add(color);
            
            foreach (var category in _category)
                CategoryComboBox.Items.Add(category);

            var brands = RepositoryBrand.GetBrands();
            foreach (var brand in brands)
            {
                var brandItem = new ComboBoxItem
                {
                    Content = brand.BrandName,
                    Tag = brand.BrandID
                };
                BrandComboBox.Items.Add(brandItem);
            }
        }

        public bool ShowDialog()
        {
            base.ShowDialog();
            return ActiveFilter.IsChecked == true;
        }

        private void ActiveFilter_Checked(object sender, RoutedEventArgs e)
        {
            bool isEnabled = ActiveFilter.IsChecked == true;
            SecondDate.IsEnabled = FirstDate.IsEnabled = ColorComboBox.IsEnabled =
                CategoryComboBox.IsEnabled = FirtPrice.IsEnabled = SecondPrice.IsEnabled = isEnabled;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
