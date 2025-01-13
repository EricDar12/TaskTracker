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

        public ObservableCollection<TrackedTask> GetAllTasks() {

            var tasks = new ObservableCollection<TrackedTask>();

            using (var connection = new SqliteConnection(DatabaseInitializer.ConnectionString)) {
                connection.Open();
                var result = connection.Query<TrackedTask>("SELECT * FROM Task").ToList();
                foreach (var task in result) {
                    tasks.Add(task);
                }
            }
            return tasks;
        }

        public int InsertNewTask(string taskName) {

            var command = "INSERT INTO Task (TaskName) VALUES (@TaskName)";

            using (var connection = new SqliteConnection(DatabaseInitializer.ConnectionString)) {
                connection.Open();
                int rowsAffected = connection.Execute(command, new {TaskName = taskName});
                return rowsAffected;
            }
            
        }

        public int UpdateTask(string newTaskName, int taskID) {

            var command = "UPDATE Task SET TaskName = @NewTaskName WHERE TaskID = @TaskID";

            using (var connection = new SqliteConnection(DatabaseInitializer.ConnectionString)) {
                connection.Open();
                int rowsAffected = connection.Execute(command, new { NewTaskName = newTaskName, TaskID = taskID });
                return rowsAffected;
            }

        }

        public int DeleteTask(int taskID) {

            var command = "DELETE FROM Task WHERE TaskID = @TaskID";

            using (var connection = new SqliteConnection(DatabaseInitializer.ConnectionString)) {
                connection.Open();
                int rowsAffected = connection.Execute(command, new {TaskID = taskID});
                return rowsAffected;
            }

        }

    }
}
