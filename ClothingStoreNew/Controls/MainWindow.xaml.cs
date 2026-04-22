using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ClothingStoreNew.ViewModels;

namespace ClothingStoreNew
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Products> Products { get; set; }

        public static ObservableCollection<CartItem> Cart =
            new ObservableCollection<CartItem>();

        public int CartCount
        {
            get { return Cart.Sum(x => x.Quantity); }
        }

        public MainWindow()
        {
            InitializeComponent();

            LoadProducts();

            DataContext = this;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.IsAdmin)
            {
                HideAdminButtons(this);
            }
        }

        private void HideAdminButtons(DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                Button btn = child as Button;

                if (btn != null && btn.Name == "AdminBtn")
                {
                    btn.Visibility = Visibility.Collapsed;
                }

                HideAdminButtons(child);
            }
        }

        private void LoadProducts()
        {
            using (OnlineStoreDbEntities db = new OnlineStoreDbEntities())
            {
                Products = new ObservableCollection<Products>(db.Products.ToList());
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Products product = (Products)((FrameworkElement)sender).Tag;

            CartItem existing = Cart.FirstOrDefault(x => x.ProductId == product.Id);

            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                Cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            DataContext = null;
            DataContext = this;
        }

        private void OpenCart_Click(object sender, RoutedEventArgs e)
        {
            CartWindow w = new CartWindow();
            w.Show();
        }

        private void EditPrice_Click(object sender, RoutedEventArgs e)
        {
            if (!App.IsAdmin) return;

            Products product = (Products)((FrameworkElement)sender).Tag;

            Window win = new Window();
            win.Title = "Цена";
            win.Width = 250;
            win.Height = 120;

            StackPanel panel = new StackPanel();
            panel.Margin = new Thickness(10);

            TextBox box = new TextBox();
            box.Text = product.Price.ToString();

            Button btn = new Button();
            btn.Content = "Сохранить";

            btn.Click += delegate
            {
                decimal newPrice;

                if (decimal.TryParse(box.Text, out newPrice))
                {
                    using (OnlineStoreDbEntities db = new OnlineStoreDbEntities())
                    {
                        Products p = db.Products.First(x => x.Id == product.Id);
                        p.Price = newPrice;
                        db.SaveChanges();
                    }

                    product.Price = newPrice;

                    DataContext = null;
                    DataContext = this;

                    win.Close();
                }
            };

            panel.Children.Add(box);
            panel.Children.Add(btn);

            win.Content = panel;
            win.ShowDialog();
        }
    }
}