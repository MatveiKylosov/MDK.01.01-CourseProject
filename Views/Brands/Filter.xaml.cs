using System;
using System.Collections.Generic;
using System.Windows;

namespace MDK._01._01_CourseProject.Views.Brands
{
    public partial class Filter : Window
    {
        public string SelectedCountry { 
            get
            {
                return CountryComboBox.SelectedItem as string;
            } 
            set
            {
                CountryComboBox.SelectedItem = value;
            } 
        }
        public string SelectedManufacturer { 
            get
            {
                return ManufacturerComboBox.SelectedItem as string;
            } 
            set
            {
                ManufacturerComboBox.SelectedItem = value;
            } 
        }
        public string SelectedAddress {
            get
            {
                return AddressComboBox.SelectedItem as string;
            }
            set 
            {
                AddressComboBox.SelectedItem = value;
            }
        }

        public Filter(List<string> countries, List<string> manufacturers, List<string> addresses, bool filterUse)
        {
            InitializeComponent();
            ActiveFilter.IsChecked = filterUse;
            CountryComboBox.IsEnabled = ManufacturerComboBox.IsEnabled = AddressComboBox.IsEnabled = filterUse;
            InitializeComboBoxes(countries, manufacturers, addresses);
        }

        private void InitializeComboBoxes(
            List<string> _countries,
            List<string> _manufacturers,
            List<string> _addresses)
        {
            CountryComboBox.Items.Clear();
            ManufacturerComboBox.Items.Clear();
            AddressComboBox.Items.Clear();

            CountryComboBox.Items.Add("Не выбран.");
            ManufacturerComboBox.Items.Add("Не выбран.");
            AddressComboBox.Items.Add("Не выбран.");

            foreach (var item in _countries)
                CountryComboBox.Items.Add(item);
            foreach (var item in _manufacturers)
                ManufacturerComboBox.Items.Add(item);
            foreach (var item in _addresses)
                AddressComboBox.Items.Add(item);
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

        public bool ShowDialog()
        {
            base.ShowDialog();
            return ActiveFilter.IsChecked.HasValue && ActiveFilter.IsChecked.Value;
        }

        private void ActiveFilter_Checked(object sender, RoutedEventArgs e)
        {
            CountryComboBox.IsEnabled = ManufacturerComboBox.IsEnabled = AddressComboBox.IsEnabled = ActiveFilter.IsChecked.Value;
        }
    }
}
