using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using DevSandbox.Utils;

namespace DevSandbox.Tests
{
    [TestClass]
    public class FileUtilitiesTests
    {
        [TestMethod]
        public void Archive_File()
        {
            var folder = "C:\\Test";
            var file = "TestArchive.xlsx";
            var archiveDirectory = $"{folder}\\ArchiveTest";

            Directory.CreateDirectory(folder);
            var fileStream = File.Create($"{folder}\\{file}");
            fileStream.Close();
            if (Directory.Exists(archiveDirectory)) Directory.GetFiles(archiveDirectory, "TestArchive*.xlsx", SearchOption.TopDirectoryOnly).ToList().ForEach(f => File.Delete(f));

            var util = new FileUtilities();
            util.ArchiveFile($"{folder}\\{file}", archiveDirectory);

            var files = Directory.GetFiles(archiveDirectory, "TestArchive_*.xlsx", SearchOption.TopDirectoryOnly);
            Assert.AreEqual(1, files.Count());
        }

        [TestMethod]
        public void StageFile()
        {
            var folder = "C:\\Test";
            var file = "StageTest.xlsx";
            var processingDirectory = $"{folder}\\ProcessingTest";

            Directory.CreateDirectory(folder);
            var fileStream = File.Create($"{folder}\\{file}");
            fileStream.Close();
            if (Directory.Exists(processingDirectory)) Directory.GetFiles(processingDirectory, "StageTest*.xlsx", SearchOption.TopDirectoryOnly).ToList().ForEach(f => File.Delete(f));

            var util = new FileUtilities();
            util.StageFile($"{folder}\\{file}", processingDirectory);

            var files = Directory.GetFiles(processingDirectory, "StageTest*.xlsx", SearchOption.TopDirectoryOnly);
            Assert.AreEqual(1, files.Count());
        }

        [TestMethod]
        public void StageFile_When_File_Does_Not_Exist_Returns_Staged_File_Path()
        {
            var folder = "C:\\Test";
            var file = $"{folder}\\NotExistStageTest.xlsx";
            var processingDirectory = $"{folder}\\ProcessingTest";

            Directory.CreateDirectory(folder);
            var util = new FileUtilities();

            file = util.StageFile(file, processingDirectory);

            Assert.AreEqual(file, $"{processingDirectory}\\NotExistStageTest.xlsx");
        }

        [TestMethod]
        public void CleanFolder()
        {
            var folder = "C:\\Test\\ArchiveTest";
            var file1 = $"{folder}\\TestDelete1.xlsx";
            var file2 = $"{folder}\\TestDelete2.xlsx";
            var file3 = $"{folder}\\TestDelete3.xlsx";

            var daysOld = 5;

            Directory.CreateDirectory(folder);
            if (Directory.Exists(folder)) Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly).ToList().ForEach(f => File.Delete(f));

            var fileStream1 = File.Create(file1);
            var fileStream2 = File.Create(file2);
            var fileStream3 = File.Create(file3);
            fileStream1.Close();
            fileStream2.Close();
            fileStream3.Close();

            File.SetLastWriteTime(file1, DateTime.Now.AddDays(-3));
            File.SetLastWriteTime(file2, DateTime.Now.AddDays(-4));
            File.SetLastWriteTime(file3, DateTime.Now.AddDays(-5));

            var util = new FileUtilities();

            util.CleanFolder(folder, daysOld);

            var files = Directory.GetFiles(folder);
            Assert.AreEqual(files.First(), file1);
            Assert.AreEqual(files.Last(), file2);
            Assert.IsFalse(files.Contains(file3));
        }

    }

}
