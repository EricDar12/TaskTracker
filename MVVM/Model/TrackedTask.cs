using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskTracker.MVVM.Model {
    internal class TrackedTask {

        public int TaskID { get; private set; }
        public string TaskName { get; set; } = string.Empty;

        public TrackedTask() {

        }

        // For constructing tasks from the database
        public TrackedTask(int taskId, string taskName) {
            TaskID = taskId;
            TaskName = taskName;
        }

    }
}
