using System.Drawing;
using System.IO;

namespace Spotlight_Checker
{
    internal static class ImageFileExtensions
    {
        public static ImageFileType ImageType(this FileInfo file)
        {
            byte[] magic;

            var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);

            using (var sr = new BinaryReader(fileStream))
            {
                if (sr.BaseStream.Length < 3)
                {
                    sr.Close();
                    return ImageFileType.Invalid;
                }

                magic = sr.ReadBytes(3);

                sr.Close();
            }

            if (magic[0] == 0xFF && magic[1] == 0xD8 && magic[2] == 0xFF)
            {
                return ImageFileType.Jpeg;
            }

            if (magic[0] == 0x89 && magic[1] == 0x50 && magic[2] == 0x4E)
            {
                return ImageFileType.Png;
            }

            return ImageFileType.Invalid;
        }

        public static bool IsPortrait(this FileInfo file)
        {
            Image image = new Bitmap(file.FullName);

            return image.Width < image.Height;
        }
    }
}