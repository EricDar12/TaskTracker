using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using System.Timers;
using CommunityToolkit.Mvvm.Input;
using TaskTracker.MVVM.Model;

namespace TaskTracker.MVVM.ViewModel {
    internal class TimerViewModel : INotifyPropertyChanged {

        private readonly System.Timers.Timer _timer;
        private TimeSpan _elapsedTime;
        private bool _isRunning = false;

        public string TimerDisplay => _elapsedTime.ToString(@"hh\:mm\:ss");

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand StartTimerCommand { get; }
        public ICommand StopTimerCommand { get; }

        public TimerViewModel() {
            _timer = new System.Timers.Timer(1000);

            _timer.Elapsed += UpdateTimer_Tick!;

            StartTimerCommand = new RelayCommand(StartTimer);
            StopTimerCommand = new RelayCommand(StopTimer);
        }

        public void StartTimer() {
            if (_isRunning) return;
            _isRunning = true;
            _elapsedTime = TimeSpan.Zero;
            _timer.Start();
        }

        public void StopTimer() {
            if (!_isRunning) return;
            _isRunning = false;
            _timer.Stop();
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
