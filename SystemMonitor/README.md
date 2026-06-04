# 系统监控悬浮窗口

一个轻量级的 Windows 桌面悬浮窗口应用程序，用于实时监控系统硬件状态。

## 功能特性

- CPU 占用率监控
- CPU 温度监控
- GPU 占用率监控
- GPU 温度监控
- 显存占用监控
- 内存占用监控

## 系统要求

- Windows 10/11
- .NET 8.0 Runtime
- 管理员权限（用于访问硬件信息）

## 安装与运行

### 方式一：从源码构建

1. 确保已安装 [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. 克隆或下载项目
3. 打开命令行，进入项目目录
4. 运行以下命令：

```bash
cd SystemMonitor
dotnet build
dotnet run --project SystemMonitor
```

### 方式二：发布为可执行文件

```bash
cd SystemMonitor
dotnet publish -c Release -r win-x64 --self-contained
```

生成的可执行文件位于 `SystemMonitor/bin/Release/net8.0-windows/win-x64/publish/`

## 使用说明

1. 启动程序后，会出现一个半透明的悬浮窗口
2. 窗口默认显示在屏幕左上角
3. 可以拖拽窗口到任意位置
4. 窗口始终显示在最上层
5. 数据每 5 秒自动更新

## 窗口特性

- **始终置顶**：窗口始终显示在其他窗口之上
- **半透明**：窗口背景半透明，可看到后面的内容
- **可拖拽**：按住标题栏可拖拽窗口
- **跟随系统主题**：自动适应 Windows 深色/浅色主题

## 项目结构

```
SystemMonitor/
├── SystemMonitor.sln           # 解决方案文件
├── SystemMonitor/
│   ├── SystemMonitor.csproj    # 项目文件
│   ├── App.xaml                # 应用入口配置
│   ├── App.xaml.cs             # 应用逻辑
│   ├── MainWindow.xaml         # 主窗口 XAML
│   ├── MainWindow.xaml.cs      # 主窗口逻辑
│   ├── HardwareMonitor.cs      # 硬件监控服务
│   └── Models/
│       └── SystemInfo.cs       # 数据模型
└── README.md                   # 项目说明
```

## 技术栈

- C# 12
- .NET 8.0
- WPF (Windows Presentation Foundation)
- LibreHardwareMonitorLib

## 常见问题

### Q: 为什么需要管理员权限？

A: LibreHardwareMonitor 需要访问系统硬件信息，这需要管理员权限才能正常工作。

### Q: 如何设置开机自启动？

A: 可以将程序快捷方式放入 Windows 启动文件夹：
1. 按 `Win + R`，输入 `shell:startup`
2. 将程序可执行文件的快捷方式放入该文件夹

### Q: 如何修改刷新频率？

A: 修改 `MainWindow.xaml.cs` 中的 `TimeSpan.FromSeconds(5)` 值。

## 许可证

MIT License
