using System.Windows;

namespace SystemMonitor;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        // Ensure single instance
        var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
        var processes = System.Diagnostics.Process.GetProcessesByName(currentProcess.ProcessName);
        
        if (processes.Length > 1)
        {
            MessageBox.Show("程序已在运行中", "系统监控", MessageBoxButton.OK, MessageBoxImage.Information);
            Current.Shutdown();
            return;
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }
}
