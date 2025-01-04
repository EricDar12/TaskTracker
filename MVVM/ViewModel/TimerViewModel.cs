using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Input;
using TaskTracker.MVVM.Model;

namespace TaskTracker.MVVM.ViewModel {
    internal class TimerViewModel : INotifyPropertyChanged {

        private readonly DispatcherTimer _timer;
        private TimeSpan _elapsedTime;
        private bool _isRunning = false;

        public string TimerDisplay => _elapsedTime.ToString(@"hh\:mm\:ss");

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand StartTimerCommand { get; }
        public ICommand StopTimerCommand { get; }

        public TimerViewModel() {
            _timer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick += UpdateTimer_Tick!;

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
            _isRunning = false;
            _timer.Stop();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e) {
            _elapsedTime = _elapsedTime.Add(_timer.Interval);
            OnPropertyChanged(nameof(TimerDisplay));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
