using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskTracker.MVVM.ViewModel;

namespace TaskTracker {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            var taskViewModel = new TaskViewModel();
            stackPanelTask.DataContext = taskViewModel;
            stackPanelTimer.DataContext = new TimerViewModel(taskViewModel);
        }
    }
}