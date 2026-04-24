using ClothingStoreNew.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ClothingStoreNew
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Products> Products { get; set; }
        public Visibility AdminVisibility { get; set; }

        private List<Products> AllProducts;

        public MainWindow()
        {
            InitializeComponent();

            AdminVisibility = App.IsAdmin ? Visibility.Visible : Visibility.Collapsed;

            Products = new ObservableCollection<Products>();

            DataContext = this;

            LoadProducts();
        }

        // ================= LOAD =================
        private void LoadProducts()
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                AllProducts = db.Products.ToList();

                Products.Clear();
                foreach (var p in AllProducts)
                    Products.Add(p);

                CategoryFilter.ItemsSource = db.Categories.ToList();
            }
        }

        // ================= FILTER =================
        private void ApplyFilter()
        {
            if (AllProducts == null) return;

            string search = SearchBox.Text?.ToLower()?.Trim();

            var category = CategoryFilter.SelectedItem as Categories;

            decimal min = 0;
            decimal max = 0;

            decimal.TryParse(MinPriceBox.Text, out min);
            decimal.TryParse(MaxPriceBox.Text, out max);

            var filtered = AllProducts.Where(p =>
                (string.IsNullOrWhiteSpace(search) || p.Name.ToLower().Contains(search)) &&
                (category == null || p.CategoryId == category.Id) &&
                (min == 0 || p.Price >= min) &&
                (max == 0 || p.Price <= max)
            );

            // ================= SORT =================
            switch (SortComboBox.SelectedIndex)
            {
                case 0:
                    filtered = filtered.OrderBy(p => p.Price);
                    break;
                case 1:
                    filtered = filtered.OrderByDescending(p => p.Price);
                    break;
                case 2:
                    filtered = filtered.OrderBy(p => p.Name);
                    break;
                case 3:
                    filtered = filtered.OrderByDescending(p => p.Name);
                    break;
            }

            Products.Clear();

            foreach (var item in filtered.ToList())
                Products.Add(item);
        }

        // ================= RESET FILTERS =================
        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            MinPriceBox.Text = "";
            MaxPriceBox.Text = "";
            CategoryFilter.SelectedItem = null;
            SortComboBox.SelectedIndex = 0;

            ApplyFilter();
        }

        // ================= EVENTS =================
        private void Filter_Changed(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var product = (Products)((FrameworkElement)sender).Tag;

            CartManager.Add(product);

            MessageBox.Show("Добавлено в корзину");
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            new CartWindow().Show();
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
        }

        // ================= EDIT PRODUCT =================
        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!App.IsAdmin)
            {
                MessageBox.Show("Нет доступа");
                return;
            }

            var product = (sender as FrameworkElement)?.Tag as Products;

            if (product == null) return;

            var win = new ProductEditWindow(product);
            win.ShowDialog();

            LoadProducts();
        }

        // ================= ADD PRODUCT =================
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!App.IsAdmin)
            {
                MessageBox.Show("Нет доступа");
                return;
            }

            var win = new ProductEditWindow(null);
            win.ShowDialog();

            LoadProducts();
        }
    }
}