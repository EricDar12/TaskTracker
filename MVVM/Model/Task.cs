using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskTracker.MVVM.Model {
    internal class Task {

        public int TaskID { get; private set; }
        public string TaskName { get; set; } = string.Empty;

        // For user created tasks
        public Task(string taskName) {
            TaskName = taskName;
        }
        // For constructing tasks from the database
        public Task(int taskId, string taskName) {
            TaskID = taskId;
            TaskName = taskName;
        }

    }
}
