using ClothingStoreNew.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ClothingStoreNew
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Products> Products { get; set; }

        public Visibility AdminVisibility { get; set; }

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
                Products = new ObservableCollection<Products>(
                    db.Products.ToList()
                );
            }
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
            if (!App.IsAdmin)
            {
                MessageBox.Show("Нет доступа");
                return;
            }

            new AdminWindow().Show();
        }
    }
}