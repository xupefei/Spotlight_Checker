using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotlight_Checker
{
    internal class Program
    {
        private static string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                                    @"AppData\Local\Packages\" +
                                                    @"Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets");

        private static string destFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                                           + "\\Spotlight Wallpapers";

        private static void Main(string[] args)
        {
            int thresholdKB = 200;

            if (args.Length != 0)
                Int32.TryParse(args[0], out thresholdKB);

            foreach (var file in Directory.GetFiles(folder, "*", SearchOption.AllDirectories))
            {
                var type = ImageHelper.ExamineImage(file);

                if (type == ImageFileType.Invalid)
                    continue;

                if (new FileInfo(file).Length >= thresholdKB * 1024)
                    CopyImage(file, type);
            }
        }

        private static void CopyImage(string file, ImageFileType type)
        {
            string newFileFullPath = Path.Combine(destFolder, Path.GetFileName(file));
            if (type == ImageFileType.Jpeg)
                newFileFullPath += ".jpg";
            else if (type == ImageFileType.Png)
                newFileFullPath += ".png";

            if (File.Exists(newFileFullPath))
                return;

            if (ImageHelper.IsMobileSizeImage(file))
                return;

            try
            {
                if (!Directory.Exists(destFolder))
                    Directory.CreateDirectory(destFolder);

                File.Copy(file, newFileFullPath);
            }
            catch (Exception)
            {
            }
        }
    }
}
