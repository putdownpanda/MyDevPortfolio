using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSorter.Controllers
{
    internal class FileMover
    {
        protected List<FileInfo> ErrorFiles;
        public FileMover()
        {
            ErrorFiles = new List<FileInfo>();
        }

        public void SortFiles(string destinationPath,List<FileInfo> files) {
            foreach (var file in files)
            {
                try
                {
                    MoveFile(file.FullName, destinationPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to move {file.FullName} to {destinationPath}");
                    ErrorFiles.Add(file);
                }
            }
        }

        private void MoveFile(string originPath, string destinationPath)
        {
            // Ensure the destination directory exists
            Directory.CreateDirectory(destinationPath);

            // Move the file to the correct folder
            File.Move(originPath, destinationPath);
            Console.WriteLine($"Moved {originPath} to {destinationPath}");
        }
    }
}
