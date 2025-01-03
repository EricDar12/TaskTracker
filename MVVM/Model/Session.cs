using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.MVVM.Model {
    internal class Session {

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Task Task { get; set; }
        public TimeSpan Duration => EndTime - StartTime;

        public Session(DateTime startTime, Task task) {
            StartTime = startTime;
            Task = task ?? throw new ArgumentNullException(nameof(task));
        }

        public void EndSession(DateTime endTime) {
            EndTime = endTime;
        }

    }
}
