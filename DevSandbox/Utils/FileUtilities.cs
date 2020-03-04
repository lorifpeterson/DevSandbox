using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DevSandbox.Utils
{
    public class FileUtilities
    {
        public string MoveFile(string fileName, string destinationFilePath, string destinationFileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            string destFileName = "";
            destFileName = Path.Combine(destinationFilePath, destinationFileName);

            if (!Directory.Exists(destinationFilePath))
                Directory.CreateDirectory(destinationFilePath);

            fileInfo.MoveTo(destFileName);
            return destFileName;
        }

        public string MoveFile(string fileName, string destinationFilePath)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            string destinationFileName = "";

            destinationFileName = Path.Combine(destinationFilePath, fileInfo.Name);

            if (!Directory.Exists(destinationFilePath))
                Directory.CreateDirectory(destinationFilePath);

            fileInfo.MoveTo(destinationFileName);
            return destinationFileName;
        }

        public string CopyFile(string fileName, string destinationFilePath, string destinationFileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            string destFileName = "";
            destFileName = Path.Combine(destinationFilePath, destinationFileName);

            if (!Directory.Exists(destinationFilePath))
                Directory.CreateDirectory(destinationFilePath);

            fileInfo.CopyTo(destFileName);
            return destFileName;
        }
        public string CopyFile(string fileName, string destinationFilePath)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            string destinationFileName = "";

            destinationFileName = Path.Combine(destinationFilePath, fileInfo.Name);

            if (!Directory.Exists(destinationFilePath))
                Directory.CreateDirectory(destinationFilePath);

            fileInfo.CopyTo(destinationFileName);
            return destinationFileName;
        }

        public string StageFile(string file, string destinationFilePath)
        {
            if (File.Exists(file)) return MoveFile(file, destinationFilePath);
            return Path.Combine(destinationFilePath, Path.GetFileName(file));
        }

        public string ArchiveFile(string file, string archiveDirectory)
        {
            var archiveDate = DateTime.Now;
            var fileName = $"{Path.GetFileNameWithoutExtension(file)}_{archiveDate.ToString("yyyyddMMhhmmss")}{Path.GetExtension(file)}";
            return MoveFile(file, archiveDirectory, fileName);
        }

        public void CleanFolder(string directory, int daysOld)
        {
            var keepDate = DateTime.Now.AddDays(daysOld * -1);
            if (Directory.Exists(directory))
            {
                new DirectoryInfo(directory).GetFiles()
                    .Where(f => f.LastWriteTime < keepDate)
                    .ToList()
                    .ForEach(fileInfo =>
                    {
                        fileInfo.Delete();
                    });
            }
        }


    }
}
