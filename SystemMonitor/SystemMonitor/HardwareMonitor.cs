using System.Runtime.InteropServices;
using LibreHardwareMonitor.Hardware;
using SystemMonitor.Models;

namespace SystemMonitor;

public class HardwareMonitor : IDisposable
{
    private Computer? _computer;
    private bool _disposed;

    [StructLayout(LayoutKind.Sequential)]
    private struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    public void Initialize()
    {
        _computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true
        };
        _computer.Open();
    }

    public SystemInfo GetSystemInfo()
    {
        var info = new SystemInfo();
        
        if (_computer == null) return info;
        
        foreach (var hardware in _computer.Hardware)
        {
            hardware.Update();
            
            foreach (var sensor in hardware.Sensors)
            {
                ProcessSensor(hardware, sensor, info);
            }
            
            foreach (var subHardware in hardware.SubHardware)
            {
                subHardware.Update();
                foreach (var sensor in subHardware.Sensors)
                {
                    ProcessSensor(subHardware, sensor, info);
                }
            }
        }
        
        if (info.MemoryTotal == 0)
        {
            GetMemoryFromWin32(info);
        }
        
        return info;
    }

    private static void GetMemoryFromWin32(SystemInfo info)
    {
        var memStatus = new MEMORYSTATUSEX { dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>() };
        if (GlobalMemoryStatusEx(ref memStatus))
        {
            info.MemoryTotal = memStatus.ullTotalPhys / 1024 / 1024;
            info.MemoryUsed = (memStatus.ullTotalPhys - memStatus.ullAvailPhys) / 1024 / 1024;
        }
    }

    private void ProcessSensor(IHardware hardware, ISensor sensor, SystemInfo info)
    {
        if (!sensor.Value.HasValue) return;
        
        float value = sensor.Value.Value;
        
        switch (hardware.HardwareType)
        {
            case HardwareType.Cpu:
                ProcessCpuSensor(sensor, value, info);
                break;
            case HardwareType.GpuNvidia:
            case HardwareType.GpuAmd:
            case HardwareType.GpuIntel:
                ProcessGpuSensor(sensor, value, info);
                break;
            case HardwareType.Memory:
                ProcessMemorySensor(sensor, value, info);
                break;
        }
    }

    private void ProcessCpuSensor(ISensor sensor, float value, SystemInfo info)
    {
        switch (sensor.SensorType)
        {
            case SensorType.Load when sensor.Name.Contains("CPU Total"):
                info.CpuUsage = value;
                break;
            case SensorType.Temperature when sensor.Name.Contains("CPU Package"):
                info.CpuTemperature = value;
                break;
        }
    }

    private void ProcessGpuSensor(ISensor sensor, float value, SystemInfo info)
    {
        switch (sensor.SensorType)
        {
            case SensorType.Load when sensor.Name.Contains("GPU Core"):
                info.GpuUsage = value;
                break;
            case SensorType.Temperature when sensor.Name.Contains("GPU Core"):
                info.GpuTemperature = value;
                break;
            case SensorType.SmallData when sensor.Name.Contains("GPU Memory Used"):
                info.GpuMemoryUsed = value;
                break;
            case SensorType.SmallData when sensor.Name.Contains("GPU Memory Total"):
                info.GpuMemoryTotal = value;
                break;
        }
    }

    private void ProcessMemorySensor(ISensor sensor, float value, SystemInfo info)
    {
        switch (sensor.SensorType)
        {
            case SensorType.Data when sensor.Name.Contains("Used"):
                info.MemoryUsed = value * 1024;
                break;
            case SensorType.Data when sensor.Name.Contains("Available"):
                info.MemoryTotal = (info.MemoryUsed / 1024 + value) * 1024;
                break;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _computer?.Close();
            _disposed = true;
        }
    }
}
