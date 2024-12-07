using SPORT_SHOP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>

    public partial class AdminWindow : Window
    {
        private readonly string _connectionString = "Data Source=ANASTASIA;Initial Catalog=SportShop;Integrated Security=True";

        public AdminWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        // Загрузка данных из базы
        private void LoadProducts()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT ProductArticleNumber, ProductName, ProductDescription, ProductManufacturer, ProductCost, ProductQuantityInStock FROM Product";
                    var command = new SqlCommand(query, connection);
                    var reader = command.ExecuteReader();

                    var productList = new List<Product>();
                    while (reader.Read())
                    {
                        productList.Add(new Product
                        {
                            ProductArticleNumber = reader["ProductArticleNumber"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            ProductManufacturer = reader["ProductManufacturer"].ToString(),
                            ProductCost = decimal.Parse(reader["ProductCost"].ToString()),
                            ProductQuantityInStock = int.Parse(reader["ProductQuantityInStock"].ToString()),
                        });
                    }

                    ProductListBox.ItemsSource = productList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        // Добавление нового продукта
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddEdit(); // Новое окно для добавления
            if (addProductWindow.ShowDialog() == true)
            {
                LoadProducts(); // Обновляем список
            }
        }

        // Редактирование выбранного продукта
        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListBox.SelectedItem is Product selectedProduct)
            {
                var editProductWindow = new AddEdit(selectedProduct.ProductArticleNumber); // Передаем идентификатор товара
                if (editProductWindow.ShowDialog() == true)
                {
                    LoadProducts(); // Обновляем список
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Удаление продукта
        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListBox.SelectedItem is Product selectedProduct)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этот товар?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    DeleteProductFromDatabase(selectedProduct.ProductArticleNumber);
                    LoadProducts(); // Обновляем список
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Удаление из базы данных
        private void DeleteProductFromDatabase(string productArticleNumber)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM Product WHERE ProductArticleNumber = @ProductArticleNumber";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductArticleNumber", productArticleNumber);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления товара: {ex.Message}");
            }
        }
    }
}

