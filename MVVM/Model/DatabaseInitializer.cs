using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.MVVM.Model {
    public class DatabaseInitializer {

        public static string ConnectionString { get; private set; } = String.Empty;
        private bool _isInitalized = false;

        public void InitializeDatabase() {

            // Only initialize the database once
            if (_isInitalized) return;
            _isInitalized = true;

            // Retrieve the users app data folder path
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Create a subfolder for the task tracker within app data directory
            string appFolder = Path.Combine(appDataFolder, "TaskTracker");

            // Create the directory if it does not already exist
            Directory.CreateDirectory(appFolder);

            // Define the database file path
            string dbFilePath = Path.Combine(appFolder, "localDB.db");

            // Use the dbFilePath to create the connection string
            string connectionString = $@"Data Source={dbFilePath};";

            // Assign the connection string to be read elsewhere in the program
            ConnectionString = connectionString;

            using (var connection = new SqliteConnection(connectionString)) {
                connection.Open();
                CreateTaskTable(connection);
                CreateSessionTable(connection);
            }
        }

        private void CreateTaskTable(SqliteConnection connection) {
            string createTaskTable = @"
                CREATE TABLE IF NOT EXISTS 
                Task (TaskID INTEGER PRIMARY KEY AUTOINCREMENT, 
                TaskName TEXT NOT NULL);";
            using (var taskCommand = new SqliteCommand(createTaskTable, connection)) {
                taskCommand.ExecuteNonQuery();
            }
        }

        private void CreateSessionTable(SqliteConnection connection) {
            string createSessionTable = @"
                CREATE TABLE IF NOT EXISTS 
                Session (SessionID INTEGER PRIMARY KEY AUTOINCREMENT,
                TaskID INTEGER NOT NULL, 
                SessionStartTime TEXT NOT NULL, 
                SessionEndTime TEXT NOT NULL,
                FOREIGN KEY (TaskID) REFERENCES Task(TaskID));";
            using (var sessionCommand = new SqliteCommand(createSessionTable, connection)) {
                sessionCommand.ExecuteNonQuery();
            }
        }
    }
}
