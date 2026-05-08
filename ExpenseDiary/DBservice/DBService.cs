using System.IO;
using Dapper;
using Microsoft.Data.Sqlite;
using static ExpenseDiary.Modules.Transaction;

namespace ExpenseDiary.DBservice
{
    public class DBService
    {
        private static readonly string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        static readonly string folder = "ExpenseDiary";
        private static readonly string file = "userdata.db";

        private static readonly string folderpath = Path.Combine(docPath, folder);
        private static readonly string filepath = Path.Combine(folderpath, file);

        private static readonly string connectionString = $"Data Source={filepath}";
        private static readonly string tablename = "Transactions";

        static DBService()
        {
            if (!Directory.Exists(folderpath))
                Directory.CreateDirectory(folderpath);
        }
        public static void Connection()
        {
            try
            {
                if (!Directory.Exists(folderpath))
                    Directory.CreateDirectory(folderpath);

                if (DataBaseExists())
                {
                    using SqliteConnection connection = new(connectionString);
                    connection.Open();
                    if (!TableExists(connection))
                    {
                        CreateTable(connection);
                    }
                }
                else
                {

                    File.Create(filepath).Close();
                    using var connection = new SqliteConnection(connectionString);
                    connection.Open();
                    CreateTable(connection);
                }
            }
            catch (SqliteException ex)
            {
                Error.ShowError($"Ошибка при при подсоединение к SQL серверу {ex.Message}");
            }
        }

        public static bool DataBaseExists()
        {
            if (File.Exists(filepath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void CreateTable(SqliteConnection connection)
        {
            string createQuery = @"
                CREATE TABLE IF NOT EXISTS Transactions (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                    Date TEXT NOT NULL,
                    Summ REAL NOT NULL,
                    Type TEXT NOT NULL,
                    Commentary TEXT
                )";
            using var createCmd = new SqliteCommand(createQuery, connection);
            createCmd.ExecuteNonQuery();
        }

        public static bool TableExists(SqliteConnection connection)
        {
            string query = "SELECT name FROM sqlite_master WHERE type='table' AND name=@name";
            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@name", tablename);
            var result = command.ExecuteScalar();
            return result != null;
        }

        public static IEnumerable<Modules.Transaction> GetAll()
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                return connection.Query<Modules.Transaction>("SELECT * FROM Transactions");
            }
            catch (SqliteException ex)
            {
                Error.ShowError(ex.Message);
                return [];
            }
            catch (Exception ex)
            {
                Error.ShowError(ex.Message);
                return [];
            }
        }

        public static void Delete(int Id)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "DELETE FROM Transactions WHERE Id = @id;";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", Id);
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Error.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                Error.ShowError(ex.Message);
            }
        }

        public static void Add(DateTime date, string commentary, TransactionType type, decimal summ)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "INSERT INTO Transactions (Date, Summ, Type, Commentary) VALUES (@date, @summ, @type, @commentary)";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@summ", summ);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@commentary", commentary);
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Error.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                Error.ShowError(ex.Message);
            }
        }

        public static void Update(int Id ,DateTime date, string commentary, TransactionType type, decimal summ)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = $"UPDATE Transactions SET Date = @date, Summ = @summ, Type = @type, Commentary = @commentary WHERE Id = @id";
                using var command = new SqliteCommand( query, connection);
                command.Parameters.AddWithValue("id", Id);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@summ", summ);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@commentary", commentary);
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Error.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                Error.ShowError(ex.Message);
            }
        }


    }
}
