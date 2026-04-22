using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ClothingStoreNew.Services;

namespace ClothingStoreNew
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Products> Products { get; set; }

        public MainWindow()
        {
            InitializeComponent();

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