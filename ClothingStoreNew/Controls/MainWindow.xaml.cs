using ClothingStoreNew.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

namespace ClothingStoreNew
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Products> Products { get; set; }
        public Visibility AdminVisibility { get; set; }

        // 🔥 ДОБАВЛЕНО
        private List<Products> AllProducts;

        public MainWindow()
        {
            InitializeComponent();

            AdminVisibility = App.IsAdmin ? Visibility.Visible : Visibility.Collapsed;

            LoadProducts();

            DataContext = this;
        }

        private void LoadProducts()
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                // 🔥 сохраняем полный список
                AllProducts = db.Products.ToList();

                // отображаем
                Products = new ObservableCollection<Products>(AllProducts);

                // 🔥 категории в фильтр
                CategoryFilter.ItemsSource = db.Categories.ToList();
            }
        }

        // 🔥 ФИЛЬТР
        private void ApplyFilter()
        {
            if (AllProducts == null) return;

            string search = SearchBox.Text?.ToLower();
            var selectedCategory = CategoryFilter.SelectedItem as Categories;

            decimal.TryParse(MinPriceBox.Text, out decimal minPrice);
            decimal.TryParse(MaxPriceBox.Text, out decimal maxPrice);

            var filtered = AllProducts.Where(p =>
                (string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search)) &&
                (selectedCategory == null || p.CategoryId == selectedCategory.Id) &&
                (minPrice == 0 || p.Price >= minPrice) &&
                (maxPrice == 0 || p.Price <= maxPrice)
            ).ToList();

            Products.Clear();
            foreach (var item in filtered)
                Products.Add(item);
        }

        // 🔥 ОДИН ОБРАБОТЧИК ДЛЯ ВСЕГО
        private void Filter_Changed(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var p = (Products)((FrameworkElement)sender).Tag;
            CartManager.Add(p);

            MessageBox.Show("Добавлено в корзину");
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            new CartWindow().Show();
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            if (!App.IsAdmin)
            {
                MessageBox.Show("Нет доступа");
                return;
            }

            new AdminWindow().Show();
        }
    }
}