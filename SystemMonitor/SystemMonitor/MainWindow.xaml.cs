using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SystemMonitor;

public partial class MainWindow : Window
{
    private readonly HardwareMonitor _hardwareMonitor;
    private readonly DispatcherTimer _timer;

    public MainWindow()
    {
        InitializeComponent();
        
        _hardwareMonitor = new HardwareMonitor();
        _hardwareMonitor.Initialize();
        
        _timer = new DispatcherTimer
            {
            Interval = TimeSpan.FromSeconds(5)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();
        
        UpdateMonitorData();
        
        MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
    }

    private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        UpdateMonitorData();
    }

    private void UpdateMonitorData()
    {
        try
        {
            var info = _hardwareMonitor.GetSystemInfo();
            
            Dispatcher.Invoke(() =>
            {
                CpuUsageText.Text = info.CpuUsageText;
                CpuTemperatureText.Text = info.CpuTemperatureText;
                GpuUsageText.Text = info.GpuUsageText;
                GpuTemperatureText.Text = info.GpuTemperatureText;
                GpuMemoryText.Text = info.GpuMemoryText;
                MemoryText.Text = info.MemoryText;
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating monitor data: {ex.Message}");
        }
    }

    private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    protected override void OnClosed(EventArgs e)
    {
        _timer.Stop();
        _hardwareMonitor.Dispose();
        base.OnClosed(e);
    }
}