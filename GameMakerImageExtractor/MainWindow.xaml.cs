using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameMakerImageExtractor
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static Regex pngExpression = new Regex(@"\x89PNG\x0d\x0a\x1a\x0a[\s\S]*?IEND\xae\x42\x60\x82", RegexOptions.Compiled);
        const string tempFilename = "__tmp.png";

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            imageList = this.FindControl<ListBox>("ImageList");
            loadingProgress = this.FindControl<ProgressBar>("LoadingProgress");
            imageList.SelectionChanged += ImageList_SelectionChanged;
            this.Find<MenuItem>("Open").Click += Open_ClickAsync;
            this.Find<MenuItem>("ExtractOne").Click += async (sender, args) =>
            {
                if (imageList.ItemCount == 0 || imageList.SelectedIndex == -1)
                {
                    // Add message box when available
                    return;
                }

                var dialog = new SaveFileDialog
                {
                    InitialFileName = imageList.SelectedItem as string ?? ""
                };
                var file = await dialog.ShowAsync(this);

                if (file == "")
                    return;

                File.WriteAllBytes(file, Images[imageList.SelectedIndex].ToCharArray().Select(x => (byte)x).ToArray());
            };
            this.Find<MenuItem>("ExtractAll").Click += async (sender, args) =>
            {
                if (imageList.ItemCount == 0)
                {
                    // Add message box when available
                    return;
                }

                var dialog = new OpenFolderDialog();
                var folder = await dialog.ShowAsync();

                if (folder == "")
                    return;

                for (int i = 0; i < Images.Length; i++)
                {
                    var filenames = imageList.Items.OfType<string>().Select(x => Path.Combine(folder, x)).ToArray();
                    File.WriteAllBytes(filenames[i], Images[i].ToCharArray().Select(x => (byte)x).ToArray());
                }
            };

            Closed += (sender, args) => File.Delete(tempFilename);
        }

        private async void Open_ClickAsync(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            var files = await dialog.ShowAsync();

            List<string> images = new List<string>();
            List<string> imageNames = new List<string>();

            if (files.Length == 0)
                return;

            loadingProgress.IsVisible = true;

            foreach (string filename in files)
            {
                var matches = pngExpression.Matches(new string((await File.ReadAllBytesAsync(filename))
                                                .Select(b => (char)b)
                                                .ToArray()))
                    .Select(x => x.Value ?? "")
                    .Where(x => x != "");

                int i = 0;
                images.AddRange(matches);
                imageNames.AddRange(matches.Select(x => $"{Path.GetFileNameWithoutExtension(filename)}_{i++}.png"));
            }

            Images = images.ToArray();
            imageList.Items = imageNames;

            loadingProgress.IsVisible = false;
        }

        private static string[] Images { get; set; }
        private ListBox imageList;
        private ProgressBar loadingProgress;

        private void ImageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (imageList.SelectedIndex == -1)
                return;
            PopulateImage(Images[imageList.SelectedIndex]);
        }

        private void PopulateImage(string data)
        {
            File.WriteAllBytes(tempFilename, data.ToCharArray().Select(x => (byte)x).ToArray());
            this.Find<Image>("ImageBox").Source = new Bitmap(tempFilename);
        }
    }
}