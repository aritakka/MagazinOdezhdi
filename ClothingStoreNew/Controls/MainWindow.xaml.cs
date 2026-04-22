using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ClothingStoreNew.ViewModels;

namespace ClothingStoreNew
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Products> Products { get; set; }

        public static ObservableCollection<CartItem> Cart { get; set; } = new ObservableCollection<CartItem>();

        public int CartCount => Cart.Sum(x => x.Quantity);

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
            DataContext = this;
        }

        private void LoadProducts()
        {
            using (var db = new OnlineStoreDbEntities())
            {
                Products = new ObservableCollection<Products>(db.Products.ToList());
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var product = (Products)((FrameworkElement)sender).Tag;

            var existing = Cart.FirstOrDefault(p => p.ProductId == product.Id);

            if (existing != null)
                existing.Quantity++;
            else
                Cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });

            MessageBox.Show("Добавлено в корзину!");

            DataContext = null;
            DataContext = this;
        }

        private void OpenCart_Click(object sender, RoutedEventArgs e)
        {
            new CartWindow().Show();
        }
    }
}