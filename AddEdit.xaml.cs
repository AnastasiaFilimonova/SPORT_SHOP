using SPORT_SHOP.классы;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace SPORT_SHOP
{
    /// <summary>
    /// Логика взаимодействия для AddEdit.xaml
    /// </summary>
    public partial class AddEdit : Window
    {
        private readonly string _connectionString = "Data Source=ANASTASIA;Initial Catalog=SportShop;Integrated Security=True";
        private string _productArticleNumber;

        // Конструктор для добавления нового продукта
        public AddEdit()
        {
            InitializeComponent();
            LoadCategories(); // Загружаем категории при открытии окна
        }

        // Конструктор для редактирования существующего продукта
        public AddEdit(string productArticleNumber) : this()
        {
            _productArticleNumber = productArticleNumber;
            LoadProductDetails();
        }

        private void LoadProductDetails()
        {
            if (string.IsNullOrEmpty(_productArticleNumber)) return;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Product WHERE ProductArticleNumber = @ProductArticleNumber";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductArticleNumber", _productArticleNumber);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ArticleNumberTextBox.Text = reader["ProductArticleNumber"].ToString();
                                NameTextBox.Text = reader["ProductName"].ToString();
                                DescriptionTextBox.Text = reader["ProductDescription"].ToString();
                                CategoryComboBox.SelectedItem = reader["ProductCategory"].ToString();
                                //PhotoTextBox.Text = reader["ProductPhoto"].ToString();
                                ManufacturerTextBox.Text = reader["ProductManufacturer"].ToString();
                                CostTextBox.Text = reader["ProductCost"].ToString();
                                DiscountTextBox.Text = reader["ProductDiscountAmount"].ToString();
                                QuantityTextBox.Text = reader["ProductQuantityInStock"].ToString();
                                StatusTextBox.Text = reader["ProductStatus"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Определяем запрос: если артикул пустой, это добавление, иначе обновление
                    string query = string.IsNullOrEmpty(_productArticleNumber)
                        ? "INSERT INTO Product (ProductArticleNumber, ProductName, ProductDescription, ProductCategory, ProductManufacturer, ProductCost, ProductDiscountAmount, ProductQuantityInStock, ProductStatus) VALUES (@ProductArticleNumber, @ProductName, @ProductDescription, @ProductCategory, @ProductManufacturer, @ProductCost, @ProductDiscountAmount, @ProductQuantityInStock, @ProductStatus)"
                        : "UPDATE Product SET ProductName = @ProductName, ProductDescription = @ProductDescription, ProductCategory = @ProductCategory, ProductManufacturer = @ProductManufacturer, ProductCost = @ProductCost, ProductDiscountAmount = @ProductDiscountAmount, ProductQuantityInStock = @ProductQuantityInStock, ProductStatus = @ProductStatus WHERE ProductArticleNumber = @ProductArticleNumber";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductArticleNumber", string.IsNullOrEmpty(_productArticleNumber) ? ArticleNumberTextBox.Text : _productArticleNumber);
                        command.Parameters.AddWithValue("@ProductName", NameTextBox.Text);
                        command.Parameters.AddWithValue("@ProductDescription", DescriptionTextBox.Text);
                        command.Parameters.AddWithValue("@ProductCategory", CategoryComboBox.SelectedItem?.ToString());
                        //command.Parameters.AddWithValue("@ProductPhoto", PhotoTextBox.Text);
                        command.Parameters.AddWithValue("@ProductManufacturer", ManufacturerTextBox.Text);
                        command.Parameters.AddWithValue("@ProductCost", decimal.Parse(CostTextBox.Text));
                        command.Parameters.AddWithValue("@ProductDiscountAmount", string.IsNullOrEmpty(DiscountTextBox.Text) ? 0 : int.Parse(DiscountTextBox.Text));
                        command.Parameters.AddWithValue("@ProductQuantityInStock", int.Parse(QuantityTextBox.Text));
                        command.Parameters.AddWithValue("@ProductStatus", StatusTextBox.Text);

                        command.ExecuteNonQuery();
                    }
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }


        private void LoadCategories()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT ProductCategory FROM Product WHERE ProductCategory IS NOT NULL AND ProductCategory <> ''";
                    using (var command = new SqlCommand(query, connection))
                    {
                        var reader = command.ExecuteReader();
                        var categories = new List<string>();
                        while (reader.Read())
                        {
                            categories.Add(reader["ProductCategory"].ToString());
                        }
                        CategoryComboBox.ItemsSource = categories;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}");
            }
        }
    }
}