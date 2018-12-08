using Avalonia;
using Avalonia.Markup.Xaml;

namespace GameMakerImageExtractor
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}