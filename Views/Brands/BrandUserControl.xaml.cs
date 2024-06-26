﻿using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Repository;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MDK._01._01_CourseProject.Views.Brands
{
    /// <summary>
    /// Логика взаимодействия для BrandUserControl.xaml
    /// </summary>
    public partial class BrandUserControl : UserControl
    {
        public Brand brand;
        Main main;
        bool edit
        {
            get
            {
                return BrandName.IsEnabled;
            }
            set
            {
                BrandName.IsEnabled = value;
                Address.IsEnabled = value;
                Manufacturer.IsEnabled = value;
                Country.IsEnabled = value;
                DeleteButton.IsEnabled = value;
                EditButton.Content = value ? "Сохранить" : "Изменить";
            }
        }

        public BrandUserControl(bool UserMode,Brand brand, Main main)
        {
            InitializeComponent();
            this.brand = brand;
            this.main = main;
            this.edit = false;

            if (brand == null)
                return;

            BrandName.Text = brand.BrandName;
            Address.Text = brand.Address;
            Manufacturer.Text = brand.Manufacturer;
            Country.SelectedValue = brand.Country;

            if(UserMode)
                DeleteButton.Visibility = EditButton.Visibility = Visibility.Hidden;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!edit)
            {
                edit = true;
                EditButton.Content = "Сохранить";
                return;
            }

            if (String.IsNullOrEmpty(BrandName.Text))
            {
                MessageBox.Show("Название бренда не должно быть пустым.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (String.IsNullOrEmpty(Manufacturer.Text))
            {
                MessageBox.Show("Название завода не должно быть пустым.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (String.IsNullOrEmpty(Address.Text))
            {
                MessageBox.Show("Адрес не должно быть пустым.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (Country.SelectedIndex == -1)
            {
                MessageBox.Show("Должна быть выбрана страна.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            brand.BrandName = this.BrandName.Text;
            brand.Manufacturer = this.Manufacturer.Text;
            brand.Country = this.Country.SelectedValue.ToString();
            brand.Address = this.Address.Text;

            RepositoryBrand.UpdateBrand(brand);
            edit = false;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasRelatedCars = Repository.RepositoryCar.GetCars().Any(x => x.BrandID == brand.BrandID);
            MessageBoxResult result;

            if (hasRelatedCars)
            {
                result = MessageBox.Show(
                    "Данная запись участвует в других таблицах. Удалить эту запись вместе с другими?",
                    "Внимание!",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                    Repository.RepositoryBrand.DeleteAllEntries(brand);

                else if (result == MessageBoxResult.No)
                    Repository.RepositoryBrand.DeleteBrand(brand);
            }
            else
            {
                result = MessageBox.Show(
                    "Продолжить удаление этой записи?",
                    "Внимание!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                    Repository.RepositoryBrand.DeleteBrand(brand);
                else
                    result = MessageBoxResult.Cancel;
            }

            if (result == MessageBoxResult.Yes || result == MessageBoxResult.No)
                main.RemoveBrand(this);
        }
    }
}
