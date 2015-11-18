using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotlight_Checker
{
    internal class ImageHelper
    {
        public static ImageFileType ExamineImage(string file)
        {
            byte[] magic;

            using (var sr = new BinaryReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)))
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
                return ImageFileType.Jpeg;

            if (magic[0] == 0x89 && magic[1] == 0x50 && magic[2] == 0x4E)
                return ImageFileType.Png;

            return ImageFileType.Invalid;
        }
    }
}
