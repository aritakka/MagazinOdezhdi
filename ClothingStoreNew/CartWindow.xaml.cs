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
        }
    }
}