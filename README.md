# Game Maker Image Extractor

This program runs on [Avalonia](http://avaloniaui.net/), so it's cross-platform and uses .NET Core 2 and all that stuff.

This program was specifically built to extract PNG files from Game Maker games, though it would theoretically work for any file that contains embedded PNGs. It basically searches for PNG signatures with Regex and then loads them into files for your viewing and extracting. The work isn't hard, but it is annoying to do without a program like this. If you don't feel like using this program though, the regular expression I use populates `pngExpression` in MainWindow.xaml.cs, or `\x89PNG\x0d\x0a\x1a\x0a[\s\S]*?IEND\xae\x42\x60\x82`.