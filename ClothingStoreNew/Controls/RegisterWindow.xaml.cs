using System.Windows;
using ClothingStoreNew.Services;

namespace ClothingStoreNew.Controls
{
    public partial class RegisterWindow : Window
    {
        private readonly AuthService _auth = new AuthService();

        public RegisterWindow()
        {
            InitializeComponent();
        }
            
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            bool ok = _auth.Register(
                EmailBox.Text,
                PasswordBox.Password,
                NameBox.Text);

            if (ok)
            {
                MessageBox.Show("Регистрация успешна!");

                var login = new LoginWindow();
                login.Show();

                this.Close(); // ❗ ВАЖНО
            }
            else
            {
                MessageBox.Show("Пользователь уже существует");
            }
        }
    }
}