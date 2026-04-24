using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClothingStoreNew
{
    public partial class AdminWindow : Window
    {
        private string selectedImagePath;

        public AdminWindow()
        {
            InitializeComponent();
            LoadData();
        }

        // ================= LOAD =================
        private void LoadData()
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                ProductsGrid.ItemsSource = db.Products.ToList();
                UsersGrid.ItemsSource = db.Users.ToList();
                OrdersGrid.ItemsSource = db.Orders.ToList();

                CategoryBox.ItemsSource = db.Categories.ToList();
                BrandBox.ItemsSource = db.Brands.ToList();
            }
        }

        // ================= IMAGE =================
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Images|*.jpg;*.png;*.jpeg"
            };

            if (dlg.ShowDialog() == true)
            {
                selectedImagePath = dlg.FileName;
                ImagePathText.Text = selectedImagePath;
            }
        }

        // ================= PRODUCTS =================
        private void AddProduct(object sender, RoutedEventArgs e)
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                db.Products.Add(new Products
                {
                    Name = NameBox.Text,
                    Price = decimal.Parse(PriceBox.Text),
                    Description = DescriptionBox.Text,
                    Stock = int.Parse(StockBox.Text),
                    ImagePath = selectedImagePath,
                    CategoryId = (CategoryBox.SelectedItem as Categories)?.Id ?? 0,
                    BrandId = (BrandBox.SelectedItem as Brands)?.Id ?? 0
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
                var p = db.Products.First(x => x.Id == selected.Id);

                p.Name = NameBox.Text;
                p.Price = decimal.Parse(PriceBox.Text);
                p.Description = DescriptionBox.Text;
                p.Stock = int.Parse(StockBox.Text);
                p.ImagePath = selectedImagePath;

                db.SaveChanges();
            }

            LoadData();
        }

        private void DeleteProduct(object sender, RoutedEventArgs e)
        {
            var selected = ProductsGrid.SelectedItem as Products;
            if (selected == null) return;

            using (var db = new OnlineStoreDbEntities1())
            {
                db.Products.Remove(db.Products.First(x => x.Id == selected.Id));
                db.SaveChanges();
            }

            LoadData();
        }

        private void ProductsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var p = ProductsGrid.SelectedItem as Products;
            if (p == null) return;

            NameBox.Text = p.Name;
            PriceBox.Text = p.Price.ToString();
            DescriptionBox.Text = p.Description;
            StockBox.Text = p.Stock.ToString();
        }

        // ================= USERS =================
        private void AddUser(object sender, RoutedEventArgs e)
        {
            using (var db = new OnlineStoreDbEntities1())
            {
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
            var u = UsersGrid.SelectedItem as Users;
            if (u == null) return;

            using (var db = new OnlineStoreDbEntities1())
            {
                var user = db.Users.First(x => x.Id == u.Id);
                user.Role = ((ComboBoxItem)RoleBox.SelectedItem).Content.ToString();
                db.SaveChanges();
            }

            LoadData();
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            var u = UsersGrid.SelectedItem as Users;
            if (u == null) return;

            using (var db = new OnlineStoreDbEntities1())
            {
                db.Users.Remove(db.Users.First(x => x.Id == u.Id));
                db.SaveChanges();
            }

            LoadData();
        }

        // ================= ORDERS =================
        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var o = OrdersGrid.SelectedItem as Orders;
            if (o == null) return;

            using (var db = new OnlineStoreDbEntities1())
            {
                var items = db.OrderItems
                    .Where(x => x.OrderId == o.Id)
                    .Select(x => new
                    {
                        x.Products.Name,
                        x.Quantity,
                        x.Price
                    }).ToList();

                OrderItemsGrid.ItemsSource = items;
            }
        }

        private void UpdateOrderStatus(object sender, RoutedEventArgs e)
        {
            var o = OrdersGrid.SelectedItem as Orders;
            if (o == null) return;

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