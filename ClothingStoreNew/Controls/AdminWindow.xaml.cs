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

        // 🔄 загрузка всех данных
        private void LoadData()
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                ProductsGrid.ItemsSource = db.Products.ToList();
                UsersGrid.ItemsSource = db.Users.ToList();
                OrdersGrid.ItemsSource = db.Orders.ToList();
            }
        }

        // =========================
        // 🛍 PRODUCTS
        // =========================

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
            var selected = ProductsGrid.SelectedItem as Products;

            if (selected == null) return;

            using (var db = new OnlineStoreDbEntities1())
            {
                var product = db.Products.First(x => x.Id == selected.Id);

                product.Name = NameBox.Text;
                product.Price = decimal.Parse(PriceBox.Text);

                db.SaveChanges();
            }

            LoadData();
        }

        private void DeleteProduct(object sender, RoutedEventArgs e)
        {
            var selected = ProductsGrid.SelectedItem as Products;

            if (selected == null)
            {
                MessageBox.Show("Выберите товар");
                return;
            }

            using (var db = new OnlineStoreDbEntities1())
            {
                var product = db.Products.First(x => x.Id == selected.Id);

                db.Products.Remove(product);
                db.SaveChanges();
            }

            LoadData();
        }

        // =========================
        // 👤 USERS
        // =========================

        private void AddUser(object sender, RoutedEventArgs e)
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                if (db.Users.Any(x => x.Email == NewEmailBox.Text))
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
            var selected = UsersGrid.SelectedItem as Users;

            if (selected == null) return;

            using (var db = new OnlineStoreDbEntities1())
            {
                var user = db.Users.First(x => x.Id == selected.Id);

                user.Role = ((ComboBoxItem)RoleBox.SelectedItem).Content.ToString();

                db.SaveChanges();
            }

            LoadData();
        }

        // ❌ УДАЛЕНИЕ ПОЛЬЗОВАТЕЛЯ (простое)
        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            var selected = UsersGrid.SelectedItem as Users;

            if (selected == null)
            {
                MessageBox.Show("Выберите пользователя");
                return;
            }

            var result = MessageBox.Show(
                "Удалить пользователя?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            using (var db = new OnlineStoreDbEntities1())
            {
                var user = db.Users.FirstOrDefault(x => x.Id == selected.Id);

                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
            }

            LoadData();

            MessageBox.Show("Пользователь удалён");
        }

        // =========================
        // 📦 ORDERS
        // =========================

        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = OrdersGrid.SelectedItem as Orders;

            if (selected == null)
                return;

            using (var db = new OnlineStoreDbEntities1())
            {
                var items = db.OrderItems
                    .Where(x => x.OrderId == selected.Id)
                    .Select(x => new
                    {
                        x.Id,
                        Product = x.Products.Name,
                        x.Quantity,
                        x.Price,
                        Total = x.Quantity * x.Price
                    })
                    .ToList();

                OrderItemsGrid.ItemsSource = items;

                OrderTotalText.Text = items.Sum(x => x.Total).ToString("0.00");

                var user = db.Users.FirstOrDefault(x => x.Id == selected.UserId);
                OrderUserText.Text = user != null ? user.Email : "-";

                foreach (ComboBoxItem item in OrderStatusBox.Items)
                {
                    if (item.Content.ToString() == selected.Status)
                    {
                        OrderStatusBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void UpdateOrderStatus(object sender, RoutedEventArgs e)
        {
            var selected = OrdersGrid.SelectedItem as Orders;

            if (selected == null)
                return;

            using (var db = new OnlineStoreDbEntities1())
            {
                var order = db.Orders.First(x => x.Id == selected.Id);

                order.Status = ((ComboBoxItem)OrderStatusBox.SelectedItem).Content.ToString();

                db.SaveChanges();
            }

            LoadData();
        }
    }
}