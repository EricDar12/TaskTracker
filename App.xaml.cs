using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using TaskTracker.MVVM.Model;
using TaskTracker.MVVM.Model.Data_Access_Layer;

namespace TaskTracker {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            DatabaseInitializer dbInit = new DatabaseInitializer();
            dbInit.InitializeDatabase();
        }
    }
}


