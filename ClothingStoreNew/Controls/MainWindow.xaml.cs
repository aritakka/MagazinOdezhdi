using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ClothingStoreNew.ViewModels;

namespace ClothingStoreNew
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Products> Products { get; set; }

        public static ObservableCollection<CartItem> Cart = new ObservableCollection<CartItem>();

        public int CartCount => Cart.Count;

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
                Products = new ObservableCollection<Products>(
                    db.Products.ToList()
                );
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var product = (Products)((FrameworkElement)sender).Tag;

            Cart.Add(new CartItem
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            });

            MessageBox.Show("Добавлено в корзину!");
        }

        private void OpenCart_Click(object sender, RoutedEventArgs e)
        {
            new CartWindow().Show();
        }
    }
}