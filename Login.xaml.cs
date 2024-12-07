using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace SPORT_SHOP
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateWatermark();
        }

        private void UpdateWatermark()
        {
            watermark.Visibility = string.IsNullOrWhiteSpace(LoginTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void TextBox_TextChanged1(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateWatermark1();
        }
        private void UpdateWatermark1()
        {
            watermark1.Visibility = string.IsNullOrWhiteSpace(PasswordTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=ANASTASIA;Initial Catalog=SportShop;Integrated Security=True"; // Замените на ваши данные
            string login = LoginTextBox.Text; // TextBox для логина
            string password = PasswordTextBox.Text; // TextBox для пароля

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Расширенный запрос для получения дополнительных данных
                    string query = @"SELECT UserRole, UserSurname, UserName, UserPatronymic FROM [User] WHERE UserLogin = @login AND UserPassword = @password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@login", login.Trim());
                    command.Parameters.AddWithValue("@password", password.Trim());

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read()) // Если пользователь найден
                        {
                            int role = reader.GetInt32(reader.GetOrdinal("UserRole"));
                            string surname = reader.GetString(reader.GetOrdinal("UserSurname"));
                            string name = reader.GetString(reader.GetOrdinal("UserName"));
                            string patronymic = reader.GetString(reader.GetOrdinal("UserPatronymic"));

                            MessageBox.Show($"Добро пожаловать, {surname} {name} {patronymic}!");
                           
                            // Открытие окна в зависимости от роли
                            switch (role)
                            {
                                case 1: // Администратор
                                    AdminWindow adminWindow = new AdminWindow();
                                    adminWindow.Show();
                                    break;

                                case 2: // Менеджер
                                    ManagerWindow managerWindow = new ManagerWindow();
                                    managerWindow.Show();
                                    break;

                                case 3: // Авторизованный клиент
                                    AutClientWindow autclientWindow = new AutClientWindow();
                                    autclientWindow.Show();
                                    break;
                            }
                            this.Close(); // Закрытие текущего окна
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}