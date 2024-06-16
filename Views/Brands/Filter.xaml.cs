using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MDK._01._01_CourseProject.Views.Brands
{
    public partial class Filter : Window
    {
        // Свойства для выбранных значений фильтра
        public string SelectedCountry
        {
            get => CountryComboBox.SelectedItem as string;
            set => CountryComboBox.SelectedItem = value;
        }

        public string SelectedManufacturer
        {
            get => ManufacturerComboBox.SelectedItem as string;
            set => ManufacturerComboBox.SelectedItem = value;
        }

        public string SelectedAddress
        {
            get => AddressComboBox.SelectedItem as string;
            set => AddressComboBox.SelectedItem = value;
        }

        // Конструктор окна фильтрации
        public Filter(List<string> countries, List<string> manufacturers, List<string> addresses, bool filterUse)
        {
            InitializeComponent();
            ActiveFilter.IsChecked = filterUse;
            SetComboBoxesEnabled(filterUse);
            InitializeComboBoxes(countries, manufacturers, addresses);
        }

        // Инициализация ComboBox значениями
        private void InitializeComboBoxes(List<string> countries, List<string> manufacturers, List<string> addresses)
        {
            AddItemsToComboBox(CountryComboBox, countries);
            AddItemsToComboBox(ManufacturerComboBox, manufacturers);
            AddItemsToComboBox(AddressComboBox, addresses);
        }

        // Добавление элементов в ComboBox
        private void AddItemsToComboBox(ComboBox comboBox, List<string> items)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("Не выбран.");
            foreach (var item in items)
            {
                comboBox.Items.Add(item);
            }
        }

        // Обработчик нажатия на кнопку "Применить"
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        // Обработчик нажатия на кнопку "Отменить"
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // Обработчик изменения состояния CheckBox для активации фильтра
        private void ActiveFilter_Checked(object sender, RoutedEventArgs e)
        {
            SetComboBoxesEnabled(ActiveFilter.IsChecked.GetValueOrDefault());
        }

        // Установка доступности ComboBox в зависимости от состояния фильтра
        private void SetComboBoxesEnabled(bool isEnabled)
        {
            CountryComboBox.IsEnabled = ManufacturerComboBox.IsEnabled = AddressComboBox.IsEnabled = isEnabled;
        }
    }
}
