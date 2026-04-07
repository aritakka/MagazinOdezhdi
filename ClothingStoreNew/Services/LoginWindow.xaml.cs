using System.Windows;
using ClothingStoreNew.Services;

namespace ClothingStoreNew.Controls
{
    public partial class LoginWindow : Window
    {
        private AuthService _auth = new AuthService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var user = _auth.Login(EmailBox.Text, PasswordBox.Password);
            if (user != null)
            {
                new MainWindow().Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неправильный Email или пароль");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();
        }
    }
}