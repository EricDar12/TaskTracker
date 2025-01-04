using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.MVVM.Model.Data_Access_Layer {
    internal class TaskRepository {

        public ObservableCollection<Task> GetAllTasks() {

            var tasks = new ObservableCollection<Task>();

            using (var connection = new SqliteConnection(DatabaseInitializer.ConnectionString)) {
                connection.Open();
                var result = connection.Query<Task>("SELECT * FROM Task").ToList();
                foreach (var task in result) {
                    tasks.Add(task);
                }
            }
            return tasks;
        }

        public void InsertNewTask(string taskName) {

            var command = "INSERT INTO Task (TaskName) VALUES (@TaskName)";

            using (var connection = new SqliteConnection(DatabaseInitializer.ConnectionString)) {
                connection.Open();
                int rowsAffected = connection.Execute(command, new {TaskName = taskName});
            }
            
        }

    }
}
