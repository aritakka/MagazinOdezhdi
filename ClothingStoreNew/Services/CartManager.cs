using System.Collections.Generic;
using System.Linq;

namespace ClothingStoreNew.Services
{
    public static class CartManager
    {
        public static List<CartItems> Items = new List<CartItems>();

        public static void Add(Products product)
        {
            var item = Items.FirstOrDefault(x => x.ProductId == product.Id);

            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                Items.Add(new CartItems
                {
                    ProductId = product.Id,
                    Quantity = 1
                });
            }
        }

        public static List<(Products product, int qty)> GetFullCart()
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                return Items
                    .Select(i => (
                        product: db.Products.First(p => p.Id == i.ProductId),
                        qty: i.Quantity
                    ))
                    .ToList();
            }
        }
    }
}