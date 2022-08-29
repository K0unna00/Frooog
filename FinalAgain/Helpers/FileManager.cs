using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain.Helpers
{
    public class FileManager
    {
        public static string Save(string root, string folder, IFormFile file)
        {
            string newFileName = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(root, folder, newFileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return newFileName;
        }
        public static bool Delete(string root, string folder, string fileName)
        {
            if (fileName != "pp.png")
            {
                string path = Path.Combine(root, folder, fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            return true;
        }
        public static void SaveAudio(string root,string folder , object file)
        {
            string filepath = Path.Combine(root, folder, "audio.wav");
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                writer.WriteLine(file);
            }
        }
    }
}
