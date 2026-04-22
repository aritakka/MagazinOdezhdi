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
            var email = EmailBox.Text;
            var password = PasswordBox.Password;

            var user = _auth.Login(email, password);

            if (user != null)
            {
                App.CurrentUser = user;

                // 🔴 ПРОВЕРКА НА АДМИНА
                if (email == "1@gmail.com" && password == "1")
                    App.IsAdmin = true;
                else
                    App.IsAdmin = false;

                MessageBox.Show(App.IsAdmin ? "Вы вошли как админ" : "Вход успешен!");

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