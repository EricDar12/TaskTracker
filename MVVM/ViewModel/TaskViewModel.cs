using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.MVVM.Model.Data_Access_Layer;

namespace TaskTracker.MVVM.ViewModel {
    internal class TaskViewModel : INotifyPropertyChanged {

        private TaskRepository _taskRepository;

        private Model.Task? _selectedTask;

        public ObservableCollection<Model.Task> TaskList { get; private set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public Model.Task SelectedTask {
            get => _selectedTask!;
            set {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public TaskViewModel() {
            _taskRepository = new TaskRepository();
            TaskList = _taskRepository.GetAllTasks();
        }

        public void UpdateTaskList() {
            // Modify the existing task list on update
            TaskList.Clear();
            foreach (var task in _taskRepository.GetAllTasks()) {
                TaskList.Add(task);
            }
            OnPropertyChanged(nameof(TaskList));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
