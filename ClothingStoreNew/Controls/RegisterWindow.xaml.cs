using System.Text.RegularExpressions;
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
            string email = EmailBox.Text?.Trim();
            string fullName = NameBox.Text?.Trim();
            string password = PasswordBox.Password;

            // ================= EMPTY VALIDATION =================
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Введите email");
                return;
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Введите ФИО");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            // ================= EMAIL VALIDATION =================
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Некорректный email");
                return;
            }

            // ================= PASSWORD VALIDATION =================
            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов");
                return;
            }

            if (!HasLetterAndDigit(password))
            {
                MessageBox.Show("Пароль должен содержать буквы и цифры");
                return;
            }

            // ================= REGISTER =================
            bool ok = _auth.Register(email, password, fullName);

            if (ok)
            {
                MessageBox.Show("Регистрация успешна!");

                var login = new LoginWindow();
                login.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Пользователь уже существует");
            }
        }

        // ================= HELPERS =================

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            );
        }

        private bool HasLetterAndDigit(string password)
        {
            bool hasLetter = false;
            bool hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsLetter(c)) hasLetter = true;
                if (char.IsDigit(c)) hasDigit = true;
            }

            return hasLetter && hasDigit;
        }
    }
}