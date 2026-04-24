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
            // ================= VALIDATION =================
            if (string.IsNullOrWhiteSpace(EmailBox.Text))
            {
                MessageBox.Show("Введите email");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            // ================= LOGIN =================
            var user = _auth.Login(EmailBox.Text, PasswordBox.Password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            App.CurrentUser = user;

            new MainWindow().Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();
            this.Close();
        }
    }
}