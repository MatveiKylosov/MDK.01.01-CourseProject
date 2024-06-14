﻿using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Repository;
using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Windows;
using System.Windows.Controls;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace MDK._01._01_CourseProject.Views.Cars
{
    public partial class CarUserControl : UserControl
    {
        public Car Car;
        Main main;
        bool edit
        {
            get { return CarName.IsEnabled; }
            set
            {
                CarName.IsEnabled = value;
                YearOfProduction.IsEnabled = value;
                Color.IsEnabled = value;
                Category.IsEnabled = value;
                Price.IsEnabled = value;
                BrandComboBox.IsEnabled = value;
                DeleteButton.IsEnabled = value;
                EditButton.Content = value ? "Сохранить" : "Изменить";
            }
        }

        public CarUserControl(Car car, Main main)
        {
            InitializeComponent();
            this.Car = car;
            this.main = main;
            this.edit = false;

            if (car == null) return;

            CarName.Text = car.CarName;
            YearOfProduction.Text = car.YearOfProduction?.ToString();
            Color.Text = car.Color;
            Category.Text = car.Category;
            Price.Text = car.Price?.ToString();

            BrandComboBox.Items.Clear();
            var brands = RepositoryBrand.GetBrands();

            foreach (var brand in brands)
            {
                var comboBoxItem = new ComboBoxItem
                {
                    Content = brand.BrandName,
                    Tag = brand.BrandID
                };
                BrandComboBox.Items.Add(comboBoxItem);
            }

            foreach (ComboBoxItem item in BrandComboBox.Items)
            {
                if ((int)item.Tag == car.BrandID)
                {
                    BrandComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!edit)
            {
                edit = true;
                EditButton.Content = "Сохранить";
                return;
            }

            if (String.IsNullOrEmpty(CarName.Text))
            {
                MessageBox.Show("Название автомобиля не должно быть пустым.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(YearOfProduction.Text) || !int.TryParse(YearOfProduction.Text, out int year))
            {
                MessageBox.Show("Год производства должен быть числом.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(Color.Text))
            {
                MessageBox.Show("Цвет не должен быть пустым.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(Category.Text))
            {
                MessageBox.Show("Категория не должна быть пустой.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(Price.Text) || !decimal.TryParse(Price.Text, out decimal price))
            {
                MessageBox.Show("Цена должна быть числом.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (BrandComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Должен быть выбран бренд.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Car.CarName = this.CarName.Text;
            Car.YearOfProduction = year;
            Car.Color = this.Color.Text;
            Car.Category = this.Category.Text;
            Car.Price = price;
            Car.BrandID = (int)((ComboBoxItem)BrandComboBox.SelectedItem).Tag;

            RepositoryCar.UpdateCar(Car);
            edit = false;
            EditButton.Content = "Редактировать";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Продолжить удаление этой записи?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                RepositoryCar.DeleteCar(Car.CarID);
                main.RemoveCar(this);
            }
        }
    }
}