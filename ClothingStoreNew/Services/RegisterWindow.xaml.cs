using System.Windows;
using ClothingStoreNew.Services;

namespace ClothingStoreNew.Controls
{
    public partial class RegisterWindow : Window
    {
        private AuthService _auth = new AuthService();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            bool ok = _auth.Register(EmailBox.Text, PasswordBox.Password, NameBox.Text);
            MessageBox.Show(ok ? "Регистрация успешна!" : "Пользователь с таким Email уже существует");
        }
    }
}