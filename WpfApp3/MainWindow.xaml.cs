using System;
using System.Data;
using System.Linq;
using System.Windows;
using Npgsql;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        private readonly string connectionString = "Host=localhost;Database=library;Username=postgres;Password=admin";

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var booksAdapter = new NpgsqlDataAdapter("SELECT * FROM books", connection);
                var booksTable = new DataTable();
                booksAdapter.Fill(booksTable);
                BooksDataGrid.ItemsSource = booksTable.DefaultView;

                var peopleAdapter = new NpgsqlDataAdapter("SELECT * FROM people", connection);
                var peopleTable = new DataTable();
                peopleAdapter.Fill(peopleTable);
                PersonsDataGrid.ItemsSource = peopleTable.DefaultView;

                var tabelAdapter = new NpgsqlDataAdapter(
                    "SELECT * FROM tabels",
                    connection
                );
                var tabelTable = new DataTable();
                tabelAdapter.Fill(tabelTable);
                TabelDataGrid.ItemsSource = tabelTable.DefaultView;
            }
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение значений 
            string bookidText = BookTitleTextBox1.Text;
            string title = BookTitleTextBox2.Text;

            // Проверка корректности
            if (string.IsNullOrWhiteSpace(bookidText) || string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Пожалуйста, заполните оба поля.");
                return;
            }

            if (!int.TryParse(bookidText, out int bookid))
            {
                MessageBox.Show("ID книги должно быть числом.");
                return;
            }

            try
            {
                // SQL-запрос для вставки
                using (var connection = new NpgsqlConnection("Host=localhost;Database=library;Username=postgres;Password=admin"))
                {
                    connection.Open();
                    string query = "INSERT INTO books (bookid, title) VALUES (@bookid, @title)";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@bookid", bookid);
                        command.Parameters.AddWithValue("@title", title);
                        command.ExecuteNonQuery();
                    }
                }

                // Обновление данных в DataGrid
                LoadData();
                MessageBox.Show("Книга успешно добавлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении книги: {ex.Message}");
            }
        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            
            string bookidText = BookTitleTextBox1.Text;


            if (string.IsNullOrWhiteSpace(bookidText))
            {
                MessageBox.Show("Пожалуйста, введите ID книги.");
                return;
            }

            if (!int.TryParse(bookidText, out int bookid))
            {
                MessageBox.Show("ID книги должно быть числом.");
                return;
            }

            try
            {
                // SQL-запрос для удаления
                using (var connection = new NpgsqlConnection("Host=localhost;Database=library;Username=postgres;Password=admin"))
                {
                    connection.Open();
                    string query = "DELETE FROM books WHERE bookid = @bookid";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@bookid", bookid);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Книга успешно удалена!");
                        }
                        else
                        {
                            MessageBox.Show("Книга с указанным ID не найдена.");
                        }
                    }
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении книги: {ex.Message}");
            }
        }
        private void UpdateBookButton_Click(object sender, RoutedEventArgs e)
        {
            string bookidText = BookTitleTextBox1.Text;
            string newTitle = BookTitleTextBox2.Text;

            if (string.IsNullOrWhiteSpace(bookidText) || string.IsNullOrWhiteSpace(newTitle))
            {
                MessageBox.Show("Пожалуйста, введите ID книги и новое название.");
                return;
            }

            if (!int.TryParse(bookidText, out int bookid))
            {
                MessageBox.Show("ID книги должно быть числом.");
                return;
            }

            try
            {
                // SQL-запрос для обновления названия книги по ID
                using (var connection = new NpgsqlConnection("Host=localhost;Database=library;Username=postgres;Password=admin"))
                {
                    connection.Open();
                    string query = "UPDATE books SET title = @newTitle WHERE bookid = @bookid";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@bookid", bookid);
                        command.Parameters.AddWithValue("@newTitle", newTitle);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Название книги успешно обновлено!");
                        }
                        else
                        {
                            MessageBox.Show("Книга с указанным ID не найдена.");
                        }
                    }
                }

                // Обновление данных в DataGrid
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении книги: {ex.Message}");
            }
        }

        private void AddPersonButton_Click(object sender, RoutedEventArgs e)
        {
            string personIdText = PersonIdTextBox.Text;
            string personName = PersonNameTextBox.Text;

            if (string.IsNullOrWhiteSpace(personIdText) || string.IsNullOrWhiteSpace(personName))
            {
                MessageBox.Show("Пожалуйста, введите ID и имя.");
                return;
            }

            if (!int.TryParse(personIdText, out int personId))
            {
                MessageBox.Show("ID должно быть числом.");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Database=library;Username=postgres;Password=admin"))
                {
                    connection.Open();
                    string query = "INSERT INTO people (personid, name) VALUES (@personid, @name)";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@personid", personId);
                        command.Parameters.AddWithValue("@name", personName);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Человек успешно добавлен!");
                    }
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}");
            }
        }
        private void UpdatePersonButton_Click(object sender, RoutedEventArgs e)
        {
            string personIdText = PersonIdTextBox.Text;
            string newPersonName = PersonNameTextBox.Text;

            if (string.IsNullOrWhiteSpace(personIdText) || string.IsNullOrWhiteSpace(newPersonName))
            {
                MessageBox.Show("Пожалуйста, введите ID и новое имя.");
                return;
            }

            if (!int.TryParse(personIdText, out int personId))
            {
                MessageBox.Show("ID должно быть числом.");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Database=library;Username=postgres;Password=admin"))
                {
                    connection.Open();
                    string query = "UPDATE people SET name = @name WHERE personid = @personid";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@personid", personId);
                        command.Parameters.AddWithValue("@name", newPersonName);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Имя успешно обновлено!");
                        }
                        else
                        {
                            MessageBox.Show("Человек с указанным ID не найден.");
                        }
                    }
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}");
            }
        }
        private void DeletePersonButton_Click(object sender, RoutedEventArgs e)
        {
            string personIdText = PersonIdTextBox.Text;

            if (string.IsNullOrWhiteSpace(personIdText))
            {
                MessageBox.Show("Пожалуйста, введите ID.");
                return;
            }

            if (!int.TryParse(personIdText, out int personId))
            {
                MessageBox.Show("ID должно быть числом.");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Database=library;Username=postgres;Password=admin"))
                {
                    connection.Open();
                    string query = "DELETE FROM people WHERE personid = @personid";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@personid", personId);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Человек успешно удалён!");
                        }
                        else
                        {
                            MessageBox.Show("Человек с указанным ID не найден.");
                        }
                    }
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            }
        }

        private void AddTabelButton_Click(object sender, RoutedEventArgs e)
        {
            string bookIdText = ReturnBookIdTextBox.Text;
            string personIdText = ReturnPersonIdTextBox.Text;
            string returnDateText = ReturnDate.Text;

            if (string.IsNullOrWhiteSpace(bookIdText) || string.IsNullOrWhiteSpace(personIdText) || string.IsNullOrWhiteSpace(returnDateText))
            {
                MessageBox.Show("Пожалуйста, введите ID книги, ID человека и дату возврата.");
                return;
            }

            if (!int.TryParse(bookIdText, out int bookId) || !int.TryParse(personIdText, out int personId))
            {
                MessageBox.Show("ID книги и ID человека должны быть числами.");
                return;
            }

            if (!DateTime.TryParseExact(returnDateText, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime returnDate))
            {
                MessageBox.Show("Введите дату в формате ДД.ММ.ГГГГ.");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Database=library;Username=postgres;Password=admin"))
                {
                    connection.Open();
                    string query = "INSERT INTO tabels (bookid, personid, returndate) VALUES (@bookid, @personid, @returndate)";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@bookid", bookId);
                        command.Parameters.AddWithValue("@personid", personId);
                        command.Parameters.AddWithValue("@returndate", returnDate);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Запись успешно добавлена!");
                    }
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}");
            }
        }
        private void UpdateTabelButton_Click(object sender, RoutedEventArgs e)
        {
            string bookIdText = ReturnBookIdTextBox.Text;
            string personIdText = ReturnPersonIdTextBox.Text;
            string newReturnDateText = ReturnDate.Text;

            if (string.IsNullOrWhiteSpace(bookIdText) || string.IsNullOrWhiteSpace(personIdText) || string.IsNullOrWhiteSpace(newReturnDateText))
            {
                MessageBox.Show("Пожалуйста, введите ID книги, ID человека и новую дату возврата.");
                return;
            }

            if (!int.TryParse(bookIdText, out int bookId) || !int.TryParse(personIdText, out int personId))
            {
                MessageBox.Show("ID книги и ID человека должны быть числами.");
                return;
            }

            if (!DateTime.TryParseExact(newReturnDateText, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime newReturnDate))
            {
                MessageBox.Show("Введите новую дату в формате ДД.ММ.ГГГГ.");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Database=library;Username=postgres;Password=admin"))
                {
                    connection.Open();
                    string query = "UPDATE tabels SET returndate = @returndate WHERE bookid = @bookid AND personid = @personid";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@bookid", bookId);
                        command.Parameters.AddWithValue("@personid", personId);
                        command.Parameters.AddWithValue("@returndate", newReturnDate);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Дата возврата успешно обновлена!");
                        }
                        else
                        {
                            MessageBox.Show("Запись с указанными ID не найдена.");
                        }
                    }
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}");
            }
        }

        private void DeleteTabelButton_Click(object sender, RoutedEventArgs e)
        {
            string bookIdText = ReturnBookIdTextBox.Text;
            string personIdText = ReturnPersonIdTextBox.Text;

            if (string.IsNullOrWhiteSpace(bookIdText) || string.IsNullOrWhiteSpace(personIdText))
            {
                MessageBox.Show("Пожалуйста, введите ID книги и ID человека.");
                return;
            }

            if (!int.TryParse(bookIdText, out int bookId) || !int.TryParse(personIdText, out int personId))
            {
                MessageBox.Show("ID книги и ID человека должны быть числами.");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Database=library;Username=postgres;Password=admin"))
                {
                    connection.Open();
                    string query = "DELETE FROM tabels WHERE bookid = @bookid AND personid = @personid";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@bookid", bookId);
                        command.Parameters.AddWithValue("@personid", personId);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись успешно удалена!");
                        }
                        else
                        {
                            MessageBox.Show("Запись с указанными ID не найдена.");
                        }
                    }
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            }
        }

        private void BooksDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}