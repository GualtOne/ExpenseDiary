using System.Configuration;
using System.Data;
using System.Windows;

namespace ExpenseDiary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SQLitePCL.Batteries_V2.Init();
            base.OnStartup(e);
        }
    }

}
