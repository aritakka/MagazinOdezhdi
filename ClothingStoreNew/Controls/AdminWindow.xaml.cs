using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClothingStoreNew
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                ProductsGrid.ItemsSource = db.Products.ToList();
                UsersGrid.ItemsSource = db.Users.ToList();
                OrdersGrid.ItemsSource = db.Orders.ToList(); // 👈 добавили
            }
        }

        // PRODUCTS
        private void AddProduct(object sender, RoutedEventArgs e)
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                db.Products.Add(new Products
                {
                    Name = NameBox.Text,
                    Price = decimal.Parse(PriceBox.Text)
                });

                db.SaveChanges();
            }

            LoadData();
        }

        private void UpdateProduct(object sender, RoutedEventArgs e)
        {
            var p = (Products)ProductsGrid.SelectedItem;

            using (var db = new OnlineStoreDbEntities1())
            {
                var prod = db.Products.First(x => x.Id == p.Id);

                prod.Name = NameBox.Text;
                prod.Price = decimal.Parse(PriceBox.Text);

                db.SaveChanges();
            }

            LoadData();
        }

        private void DeleteProduct(object sender, RoutedEventArgs e)
        {
            var p = (Products)ProductsGrid.SelectedItem;

            using (var db = new OnlineStoreDbEntities1())
            {
                var prod = db.Products.First(x => x.Id == p.Id);

                db.Products.Remove(prod);
                db.SaveChanges();
            }

            LoadData();
        }

        // USERS
        private void AddUser(object sender, RoutedEventArgs e)
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                var exists = db.Users.Any(u => u.Email == NewEmailBox.Text);

                if (exists)
                {
                    MessageBox.Show("Пользователь уже существует");
                    return;
                }

                db.Users.Add(new Users
                {
                    Email = NewEmailBox.Text,
                    FullName = NewNameBox.Text,
                    PasswordHash = NewPasswordBox.Text,
                    Role = "User"
                });

                db.SaveChanges();
            }

            LoadData();
        }

        private void ChangeRole(object sender, RoutedEventArgs e)
        {
            var u = (Users)UsersGrid.SelectedItem;

            using (var db = new OnlineStoreDbEntities1())
            {
                var user = db.Users.First(x => x.Id == u.Id);

                user.Role = ((ComboBoxItem)RoleBox.SelectedItem).Content.ToString();

                db.SaveChanges();
            }

            LoadData();
        }

        // 🆕 ORDERS

        private void UpdateOrderStatus(object sender, RoutedEventArgs e)
        {
            var o = (Orders)OrdersGrid.SelectedItem;

            if (o == null)
                return;

            using (var db = new OnlineStoreDbEntities1())
            {
                var order = db.Orders.First(x => x.Id == o.Id);

                order.Status = ((ComboBoxItem)OrderStatusBox.SelectedItem).Content.ToString();

                db.SaveChanges();
            }

            LoadData();
        }
    }
}