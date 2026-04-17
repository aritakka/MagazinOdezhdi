using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace ClothingStoreNew
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<ProductVM> Products { get; set; }
        public ObservableCollection<Products> Cart { get; set; } = new ObservableCollection<Products>();

        private int _cartCount;
        public int CartCount
        {
            get => _cartCount;
            set
            {
                _cartCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CartCount)));
            }
        }

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
                Products = new ObservableCollection<ProductVM>(
                    db.Products.ToList().Select(p => new ProductVM
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Size = p.Size,
                        IsInCart = false
                    })
                );
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var product = (ProductVM)((FrameworkElement)sender).Tag;

            if (!product.IsInCart)
            {
                product.IsInCart = true;
                product.ButtonText = "Добавлено ✔";

                using (var db = new OnlineStoreDbEntities())
                {
                    var entity = db.Products.First(x => x.Id == product.Id);
                    Cart.Add(entity);
                }

                CartCount = Cart.Count;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ProductVM : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }

        private string _buttonText = "В корзину";
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            }
        }

        public bool IsInCart { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}