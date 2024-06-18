using System;
using System.Windows;

// Filter.xaml.cs для таблицы Customers
namespace MDK._01._01_CourseProject.Views.Customers
{
    public partial class Filter : Window
    {
        public string EnteredFullName
        {
            get => FullNameTextBox.Text;
            set => FullNameTextBox.Text = value;
        }

        public string EnteredPassportData
        {
            get => PassportDataTextBox.Text;
            set => PassportDataTextBox.Text = value;
        }

        public string EnteredAddress
        {
            get => AddressTextBox.Text;
            set => AddressTextBox.Text = value;
        }

        public DateTime? EnteredBirthDateFrom
        {
            get => BirthDateFrom.SelectedDate;
            set => BirthDateFrom.SelectedDate = value;
        }

        public DateTime? EnteredBirthDateTo
        {
            get => BirthDateTo.SelectedDate;
            set => BirthDateTo.SelectedDate = value;
        }

        public bool? SelectedGender
        {
            get => GenderComboBox.SelectedIndex == 0 ? null : (bool?)(GenderComboBox.SelectedIndex == 1);
            set => GenderComboBox.SelectedIndex = value == null ? 0 : (bool)value ? 1 : 2;
        }

        public string EnteredContactDetails
        {
            get => ContactDetailsTextBox.Text;
            set => ContactDetailsTextBox.Text = value;
        }

        public Filter(bool filterUse)
        {
            InitializeComponent();
            ActiveFilter.IsChecked = filterUse;
            ToggleFilterControls(filterUse);
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            GenderComboBox.Items.Clear();
            GenderComboBox.Items.Add("Не выбран");
            GenderComboBox.Items.Add("Мужской");
            GenderComboBox.Items.Add("Женский");
            GenderComboBox.SelectedIndex = 0;
        }

        public bool ShowDialog()
        {
            base.ShowDialog();
            return ActiveFilter.IsChecked == true;
        }

        private void ActiveFilter_Checked(object sender, RoutedEventArgs e)
        {
            ToggleFilterControls(ActiveFilter.IsChecked == true);
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = ActiveFilter.IsChecked.Value;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ToggleFilterControls(bool isEnabled)
        {
            FullNameTextBox.IsEnabled = PassportDataTextBox.IsEnabled = AddressTextBox.IsEnabled =
                BirthDateFrom.IsEnabled = BirthDateTo.IsEnabled = GenderComboBox.IsEnabled =
                ContactDetailsTextBox.IsEnabled = isEnabled;
        }
    }
}
