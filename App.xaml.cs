using System.Configuration;
using System.Data;
using System.Windows;
using TaskTracker.MVVM.Model;

namespace TaskTracker {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            DatabaseInitializer dbInit = new DatabaseInitializer();
            dbInit.InitializeDatabase();
        }
    }
}


