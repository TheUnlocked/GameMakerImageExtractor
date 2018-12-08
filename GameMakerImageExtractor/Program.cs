using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;

namespace GameMakerImageExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug();
    }
}
