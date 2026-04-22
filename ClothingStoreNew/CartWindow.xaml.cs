using ClothingStoreNew.Services;
using ClothingStoreNew.ViewModels;
using System;
using System.Linq;
using System.Windows;

namespace ClothingStoreNew
{
    public partial class CartWindow : Window
    {
        public CartWindow()
        {
            InitializeComponent();
            LoadCart();
        }

        private void LoadCart()
        {
            var cart = CartManager.GetFullCart()
                .Select(x => new CartItem
                {
                    ProductId = x.product.Id,
                    Name = x.product.Name,
                    Price = x.product.Price,
                    Quantity = x.qty
                })
                .ToList();

            CartGrid.ItemsSource = cart;

            TotalText.Text = cart.Sum(x => x.Total).ToString("0.00");
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var item = (CartItem)((FrameworkElement)sender).Tag;

            var existing = CartManager.Items.FirstOrDefault(x => x.ProductId == item.ProductId);

            if (existing != null)
            {
                if (existing.Quantity > 1)
                    existing.Quantity--;
                else
                    CartManager.Items.Remove(existing);
            }

            LoadCart();
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (!CartManager.Items.Any())
                return;

            using (var db = new OnlineStoreDbEntities1())
            {
                var order = new Orders
                {
                    UserId = App.CurrentUser.Id,
                    Status = "Новый",

                    // ✅ правильное поле из EDMX
                    CreatedAt = DateTime.Now
                };

                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var item in CartManager.Items)
                {
                    var product = db.Products.First(p => p.Id == item.ProductId);

                    db.OrderItems.Add(new OrderItems
                    {
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        Price = product.Price
                    });
                }

                db.SaveChanges();
            }

            CartManager.Items.Clear();

            MessageBox.Show("Заказ оформлен!");

            Close();
        }
    }
}