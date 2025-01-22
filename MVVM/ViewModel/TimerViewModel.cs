using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using System.Timers;
using CommunityToolkit.Mvvm.Input;
using TaskTracker.MVVM.Model;
using TaskTracker.MVVM.Model.Data_Access_Layer;
using System.Windows;

namespace TaskTracker.MVVM.ViewModel {
    internal class TimerViewModel : INotifyPropertyChanged {

        private readonly System.Timers.Timer _timer;
        private readonly TaskViewModel _taskViewModel;
        private DateTime _startTime;
        private SessionRepository _sessionRepository;
        private TimeSpan _elapsedTime;
        private bool _isRunning = false;

        public string TimerDisplay => _elapsedTime.ToString(@"hh\:mm\:ss");

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand StartTimerCommand { get; }
        public ICommand StopTimerCommand { get; }

        public TimerViewModel(TaskViewModel taskViewModel) {
            _timer = new System.Timers.Timer(1000);

            _timer.Elapsed += UpdateTimer_Tick!;

            _taskViewModel = taskViewModel;
            _sessionRepository = new SessionRepository();

            StartTimerCommand = new RelayCommand(StartTimer);
            StopTimerCommand = new RelayCommand(StopTimer);
        }

        public void StartTimer() {
            if (_isRunning || _taskViewModel.SelectedTask == null) return;
            _startTime = DateTime.Now;
            _isRunning = true;
            _elapsedTime = TimeSpan.Zero;
            _timer.Start();
        }

        public void StopTimer() {
            if (!_isRunning) return;
            _isRunning = false;
            _timer.Stop();

            var session = new Session(_startTime, _taskViewModel.SelectedTask);
            session.EndSession(DateTime.Now);
            int rowsAffected = _sessionRepository.InsertNewSession(session);

            if (rowsAffected > 0) {
                MessageBox.Show("Success", "Session Entered Successfully", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            } else {
                MessageBox.Show("Error", "Session Entry Failed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

        }

        private void UpdateTimer_Tick(object sender, EventArgs e) {
            _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
            // Marshal the update to the UI thread to ensure thread safety
            App.Current.Dispatcher.Invoke(() => {
                OnPropertyChanged(nameof(TimerDisplay));
            });
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
