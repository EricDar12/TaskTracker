using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaskTracker.MVVM.Model.Data_Access_Layer;
using TaskTracker.MVVM.View;
using TaskTracker.MVVM.Model;

namespace TaskTracker.MVVM.ViewModel {
    internal class TaskViewModel : INotifyPropertyChanged {

        private TaskRepository _taskRepository;

        private TrackedTask? _selectedTask;
        public ICommand OpenEditCommand { get; }

        public ObservableCollection<TrackedTask> TaskList { get; private set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public TaskViewModel() {
            _taskRepository = new TaskRepository();
            OpenEditCommand = new RelayCommand(OpenEditTaskWindow);
            TaskList = _taskRepository.GetAllTasks();
            EditTaskViewModel.TasksChanged += (sender, args) => UpdateTaskList();
        }

        public TrackedTask SelectedTask {
            get => _selectedTask!;
            set {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public void OpenEditTaskWindow() {
            var editTaskViewModel = new EditTaskViewModel(SelectedTask);
            var editTaskWindow = new EditTaskWindow();
            editTaskWindow.DataContext = editTaskViewModel;
            editTaskWindow.ShowDialog();
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
