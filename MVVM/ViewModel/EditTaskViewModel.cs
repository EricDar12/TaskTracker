using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TaskTracker.MVVM.Model;
using TaskTracker.MVVM.Model.Data_Access_Layer;
using TaskTracker.MVVM.View;

namespace TaskTracker.MVVM.ViewModel {
    internal class EditTaskViewModel : INotifyPropertyChanged {

        private TrackedTask _selectedTask;
        private TaskRepository _taskRepository;
        private string _editingTaskName = String.Empty;

        public bool IsNewTask { get; private set; }
        public ICommand SaveTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand NewTaskCommand {  get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public static event EventHandler? TasksChanged;

        public TrackedTask SelectedTask {
            get => _selectedTask;
            set {
                _selectedTask = value;
                OnPropertyChanged(nameof(TrackedTask));
            }
        }

        public string EditingTaskName {
            get => _editingTaskName;
            set {
                _editingTaskName = value;
                OnPropertyChanged(nameof(EditingTaskName));
            }
        }

        public EditTaskViewModel(TrackedTask? task = null) {
            if (task == null) {
                _selectedTask = new TrackedTask();
                IsNewTask = true;
            }
            else {
                _selectedTask = task;
                EditingTaskName = task.TaskName;
                IsNewTask = false;
            }
            _taskRepository = new TaskRepository();
            SaveTaskCommand = new RelayCommand(SaveTask);
            DeleteTaskCommand = new RelayCommand(DeleteTask);
            NewTaskCommand = new RelayCommand(NewTask);

        }

        private void SaveTask() {

            if (EditingTaskName == String.Empty) {
                MessageBox.Show("Error", "Task Name Can Not Be Empty", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            if (IsNewTask) {

                int rowsAffected = _taskRepository.InsertNewTask(EditingTaskName);

                if (rowsAffected > 0) {
                    MessageBox.Show("Success", "Task Entered Successfully", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                    TasksChanged?.Invoke(this, EventArgs.Empty);
                    CloseWindow();
                }
                else {
                    MessageBox.Show("Error", "Task Entry Failed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }

            }

            else if (!IsNewTask) {

                int rowsAffected = _taskRepository.UpdateTask(EditingTaskName, SelectedTask.TaskID);

                if (rowsAffected > 0) {
                    MessageBox.Show("Success", "Task Updated", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                    SelectedTask.TaskName = EditingTaskName;
                    TasksChanged?.Invoke(this, EventArgs.Empty);
                    CloseWindow();
                }
                else {
                    MessageBox.Show("Error", "Task Update Failed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
        }

        // This will need to support deleting sessions, which are dependent on tasks
        private void DeleteTask() {
            if (SelectedTask.TaskName == String.Empty) {
                MessageBox.Show("Error", "No Task To Delete", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }
            int rowsAffected = _taskRepository.DeleteTask(SelectedTask.TaskID);
            if (rowsAffected > 0) {
                SelectedTask = new TrackedTask();
                TasksChanged?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Success", "Task Deleted", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                CloseWindow();
            }
            else {
                MessageBox.Show("Error", "Task Deletion Failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        private void NewTask() {
            SelectedTask = new TrackedTask();
            EditingTaskName = String.Empty;
            IsNewTask = true;
        }

        private void CloseWindow() {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is EditTaskWindow);
            window?.Close();
        }

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
