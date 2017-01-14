using System;
using System.IO;

namespace Spotlight_Checker
{
    internal class Program
    {
        private static readonly string Folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            @"AppData\Local\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets");

        private static readonly string DestFolderPath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) +
            @"\Spotlight Wallpapers";

        private static void Main(string[] args)
        {
            var thresholdKb = 200;

            if (args.Length != 0)
            {
                int.TryParse(args[0], out thresholdKb);
            }

            foreach (var filePath in Directory.GetFiles(Folder, "*", SearchOption.AllDirectories))
            {
                var fileInfo = new FileInfo(filePath);
                var type = fileInfo.ImageType();

                if (type != ImageFileType.Invalid && fileInfo.Length >= thresholdKb*1024)
                {
                    CopyImage(filePath, type);
                }
            }
        }

        private static void CopyImage(string filePath, ImageFileType type)
        {
            var fileInfo = new FileInfo(filePath);
            var directoryInfo = new DirectoryInfo(DestFolderPath);

            if (string.IsNullOrWhiteSpace(fileInfo.Name))
            {
                Console.WriteLine("Filename not found.");
                return;
            }

            var destFilePath = Path.Combine(directoryInfo.FullName, fileInfo.Name);
            var destFileInfo = new FileInfo(destFilePath);

            switch (type)
            {
                case ImageFileType.Jpeg:
                    destFilePath += ".jpg";
                    break;
                case ImageFileType.Png:
                    destFilePath += ".png";
                    break;
            }

            if (destFileInfo.Exists || fileInfo.IsPortrait())
            {
                return;
            }

            try
            {
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }

                File.Copy(fileInfo.FullName, destFilePath);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}