using System;
using System.Linq;
using System.Windows;
using ClothingStoreNew.ViewModels;

namespace ClothingStoreNew
{
    public partial class CartWindow : Window
    {
        public CartWindow()
        {
            InitializeComponent();

            DataContext = MainWindow.Cart;
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = MainWindow.Cart.Sum(i => i.Total);
            TotalText.Text = total.ToString("0.00");
        }

        // 🔴 УДАЛЕНИЕ ТОВАРА
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var item = (CartItem)((FrameworkElement)sender).Tag;

            if (item.Quantity > 1)
            {
                item.Quantity--;
            }
            else
            {
                MainWindow.Cart.Remove(item);
            }

            CartGrid.Items.Refresh();
            UpdateTotal();
        }

        // 🟢 ОФОРМЛЕНИЕ ЗАКАЗА
        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы!");
                return;
            }

            if (!MainWindow.Cart.Any())
            {
                MessageBox.Show("Корзина пуста!");
                return;
            }

            using (var db = new OnlineStoreDbEntities())
            {
                var order = new Orders
                {
                    UserId = App.CurrentUser.Id,
                    CreatedAt = DateTime.Now,
                    Status = "Новый"
                };

                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var item in MainWindow.Cart)
                {
                    db.OrderItems.Add(new OrderItems
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
                }

                db.SaveChanges();
            }

            MainWindow.Cart.Clear();

            MessageBox.Show("Заказ успешно оформлен!");
            this.Close();
        }
    }
}