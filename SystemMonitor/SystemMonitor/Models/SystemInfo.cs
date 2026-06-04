namespace SystemMonitor.Models;

public class SystemInfo
{
    public float CpuUsage { get; set; }
    public float CpuTemperature { get; set; }
    public float GpuUsage { get; set; }
    public float GpuTemperature { get; set; }
    public float GpuMemoryUsed { get; set; }
    public float GpuMemoryTotal { get; set; }
    public float MemoryUsed { get; set; }
    public float MemoryTotal { get; set; }
    
    public string CpuUsageText => $"{CpuUsage:F1}%";
    public string CpuTemperatureText => $"{CpuTemperature:F0}°C";
    public string GpuUsageText => $"{GpuUsage:F1}%";
    public string GpuTemperatureText => $"{GpuTemperature:F0}°C";
    public string GpuMemoryText => FormatMemory(GpuMemoryUsed, GpuMemoryTotal);
    public string MemoryText => FormatMemory(MemoryUsed, MemoryTotal);
    
    private static string FormatMemory(float used, float total)
    {
        if (total >= 1024)
            return $"{used / 1024:F1}/{total / 1024:F1} GB";
        return $"{used:F0}/{total:F0} MB";
    }
}
