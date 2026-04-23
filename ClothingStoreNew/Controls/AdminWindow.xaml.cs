using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;

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
                Title = "Выберите изображение",
                Filter = "Images|*.jpg;*.jpeg;*.png",
                Multiselect = false
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
                var cat = CategoryBox.SelectedItem as Categories;
                var brand = BrandBox.SelectedItem as Brands;

                if (cat == null || brand == null)
                {
                    MessageBox.Show("Выберите категорию и бренд");
                    return;
                }

                db.Products.Add(new Products
                {
                    Name = NameBox.Text,
                    Price = ParseDecimal(PriceBox.Text),
                    Description = DescriptionBox.Text,
                    Stock = ParseInt(StockBox.Text),
                    ImagePath = selectedImagePath,
                    CategoryId = cat.Id,
                    BrandId = brand.Id
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

                var cat = CategoryBox.SelectedItem as Categories;
                var brand = BrandBox.SelectedItem as Brands;

                p.Name = NameBox.Text;
                p.Price = ParseDecimal(PriceBox.Text);
                p.Description = DescriptionBox.Text;
                p.Stock = ParseInt(StockBox.Text);
                p.ImagePath = selectedImagePath;

                if (cat != null) p.CategoryId = cat.Id;
                if (brand != null) p.BrandId = brand.Id;

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
                var p = db.Products.First(x => x.Id == selected.Id);
                db.Products.Remove(p);
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

            selectedImagePath = p.ImagePath;
            ImagePathText.Text = p.ImagePath ?? "нет файла";

            using (var db = new OnlineStoreDbEntities1())
            {
                CategoryBox.SelectedItem = db.Categories.FirstOrDefault(x => x.Id == p.CategoryId);
                BrandBox.SelectedItem = db.Brands.FirstOrDefault(x => x.Id == p.BrandId);
            }
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
                var user = db.Users.First(x => x.Id == u.Id);
                db.Users.Remove(user);
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
                        Product = x.Products.Name,
                        x.Quantity,
                        x.Price,
                        Total = x.Quantity * x.Price
                    }).ToList();

                OrderItemsGrid.ItemsSource = items;
                OrderTotalText.Text = items.Sum(x => x.Total).ToString("0.00");

                var user = db.Users.FirstOrDefault(x => x.Id == o.UserId);
                OrderUserText.Text = user?.Email ?? "-";
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

        // ================= HELPERS =================
        private decimal ParseDecimal(string v)
            => decimal.TryParse(v, out var r) ? r : 0;

        private int ParseInt(string v)
            => int.TryParse(v, out var r) ? r : 0;
    }
}