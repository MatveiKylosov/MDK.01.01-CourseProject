using System.Windows;

// Filter.xaml.cs для таблицы Employees
namespace MDK._01._01_CourseProject.Views.Employees
{
    public partial class Filter : Window
    {
        public string EnteredFullName
        {
            get => FullNameTextBox.Text;
            set => FullNameTextBox.Text = value;
        }

        public int? EnteredWorkExperienceFrom
        {
            get
            {
                if (int.TryParse(WorkExperienceFrom.Text, out int value))
                    return value;
                return null;
            }
            set => WorkExperienceFrom.Text = value?.ToString();
        }

        public int? EnteredWorkExperienceTo
        {
            get
            {
                if (int.TryParse(WorkExperienceTo.Text, out int value))
                    return value;
                return null;
            }
            set => WorkExperienceTo.Text = value?.ToString();
        }

        public decimal? EnteredSalaryFrom
        {
            get
            {
                if (decimal.TryParse(SalaryFrom.Text, out decimal value))
                    return value;
                return null;
            }
            set => SalaryFrom.Text = value?.ToString();
        }

        public decimal? EnteredSalaryTo
        {
            get
            {
                if (decimal.TryParse(SalaryTo.Text, out decimal value))
                    return value;
                return null;
            }
            set => SalaryTo.Text = value?.ToString();
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
            FullNameTextBox.IsEnabled = WorkExperienceFrom.IsEnabled = WorkExperienceTo.IsEnabled =
                SalaryFrom.IsEnabled = SalaryTo.IsEnabled = ContactDetailsTextBox.IsEnabled = isEnabled;
        }
    }
}
