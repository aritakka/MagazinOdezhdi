using System.Windows;
using ClothingStoreNew.Services;

namespace ClothingStoreNew.Controls
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _auth = new AuthService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var user = _auth.Login(EmailBox.Text, PasswordBox.Password);

            if (user != null)
            {
                App.CurrentUser = user;

                MessageBox.Show("Вход успешен!");

                var main = new MainWindow();
                main.Show();

                Application.Current.MainWindow = main;
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильный Email или пароль");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var reg = new RegisterWindow();
            reg.Show();
            this.Close();
        }
    }
}