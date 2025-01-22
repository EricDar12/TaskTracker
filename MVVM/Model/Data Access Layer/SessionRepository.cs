using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.MVVM.Model.Data_Access_Layer {
    internal class SessionRepository {

        public int InsertNewSession(Session session) {
            var command = @"
                INSERT INTO Session (TaskID, SessionStartTime, SessionEndTime) 
                VALUES (@TaskID, @StartTime, @EndTime)";

            using (var connection = new SqliteConnection(DatabaseInitializer.ConnectionString)) {
                connection.Open();
                int rowsAffected = connection.Execute(command, new {
                    TaskID = session.Task.TaskID,
                    // Format start and endtime using "o" to support easy parsing of the data when we read it
                    StartTime = session.StartTime.ToString("o"),
                    EndTime = session.EndTime.ToString("o")
                });
                return rowsAffected;
            }
        }

        public ObservableCollection<Session> GetSessionByTask(int taskID) {

            var sessions = new ObservableCollection<Session>();
            var command = @"
                SELECT s.*, t.*
                FROM Session s
                JOIN Task t ON s.TaskID = t.TaskID
                WHERE s.TaskID = @TaskID";

            using (var connection = new SqliteConnection(DatabaseInitializer.ConnectionString)) {
                connection.Open();

                var results = connection.Query<Session, TrackedTask, Session>(
                    command, (session, task) => {
                        session.Task = task;
                        return session;
                    },
                    new { TaskID = taskID },
                    splitOn: "TaskID");

                foreach (var session in results) {
                    sessions.Add(session);
                }
                return sessions;
            }

        }

    }
}
