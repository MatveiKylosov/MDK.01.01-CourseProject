using System;
using System.Collections.Generic;
using System.Windows;

namespace MDK._01._01_CourseProject.Views.Brands
{
    public partial class Filter : Window
    {
        public string SelectedCountry { get; private set; }
        public string SelectedManufacturer { get; private set; }
        public string SelectedAddress { get; private set; }

        private List<string> _countries;
        private List<string> _manufacturers;
        private List<string> _addresses;

        public Filter()
        {
            InitializeComponent();
            ActiveFilter.IsChecked = false;
            CountryComboBox.IsEnabled = ManufacturerComboBox.IsEnabled = AddressComboBox.IsEnabled = false;
        }

        private void InitializeComboBoxes(string selectedCountry, string selectedManufacturer, string selectedAddress)
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

            CountryComboBox.SelectedItem = selectedCountry;
            ManufacturerComboBox.SelectedItem = selectedManufacturer;
            AddressComboBox.SelectedItem = selectedAddress;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            SelectedCountry = CountryComboBox.SelectedItem as string;
            SelectedManufacturer = ManufacturerComboBox.SelectedItem as string;
            SelectedAddress = AddressComboBox.SelectedItem as string;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public bool ShowDialog(bool filterUse, List<string> countries, List<string> manufacturers, List<string> addresses, string selectedCountry, string selectedManufacturer, string selectedAddress)
        {
            ActiveFilter.IsChecked = filterUse;
            _countries = countries;
            _manufacturers = manufacturers;
            _addresses = addresses;
            InitializeComboBoxes(selectedCountry, selectedManufacturer, selectedAddress);
            base.ShowDialog();
            return ActiveFilter.IsChecked.HasValue && ActiveFilter.IsChecked.Value;
        }

        private void ActiveFilter_Checked(object sender, RoutedEventArgs e)
        {
            CountryComboBox.IsEnabled = ManufacturerComboBox.IsEnabled = AddressComboBox.IsEnabled = ActiveFilter.IsChecked.Value;
        }
    }
}
