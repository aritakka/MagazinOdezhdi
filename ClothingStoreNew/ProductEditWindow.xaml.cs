using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ClothingStoreNew
{
    public partial class ProductEditWindow : Window
    {
        private Products _product;
        private string _imagePath;

        public ProductEditWindow(Products product)
        {
            InitializeComponent();

            using (var db = new OnlineStoreDbEntities1())
            {
                CategoryBox.ItemsSource = db.Categories.ToList();
                BrandBox.ItemsSource = db.Brands.ToList();
            }

            _product = product;

            if (product != null)
            {
                NameBox.Text = product.Name;
                PriceBox.Text = product.Price.ToString();

                _imagePath = product.ImagePath;
                LoadImage(_imagePath);
            }
        }

        // ================= IMAGE SELECT =================
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Images|*.jpg;*.png;*.jpeg",
                Title = "Выберите изображение"
            };

            if (dlg.ShowDialog() == true)
            {
                _imagePath = dlg.FileName;
                LoadImage(_imagePath);
            }
        }

        // ================= IMAGE REMOVE =================
        private void RemoveImage_Click(object sender, RoutedEventArgs e)
        {
            _imagePath = null;
            PreviewImage.Source = null;
            ImagePathText.Text = "Нет изображения";
        }

        // ================= IMAGE LOAD =================
        private void LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                PreviewImage.Source = null;
                ImagePathText.Text = "Нет изображения";
                return;
            }

            try
            {
                PreviewImage.Source = new BitmapImage(new Uri(path));
                ImagePathText.Text = path;
            }
            catch
            {
                ImagePathText.Text = "Ошибка загрузки изображения";
            }
        }

        // ================= SAVE =================
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                if (_product == null)
                {
                    var p = new Products
                    {
                        Name = NameBox.Text,
                        Price = decimal.Parse(PriceBox.Text),
                        ImagePath = _imagePath
                    };

                    db.Products.Add(p);
                }
                else
                {
                    var p = db.Products.First(x => x.Id == _product.Id);

                    p.Name = NameBox.Text;
                    p.Price = decimal.Parse(PriceBox.Text);
                    p.ImagePath = _imagePath;
                }

                db.SaveChanges();
            }

            Close();
        }

        // ================= DELETE =================
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (_product == null) return;

            using (var db = new OnlineStoreDbEntities1())
            {
                var p = db.Products.First(x => x.Id == _product.Id);

                db.Products.Remove(p);
                db.SaveChanges();
            }

            Close();
        }
    }
}