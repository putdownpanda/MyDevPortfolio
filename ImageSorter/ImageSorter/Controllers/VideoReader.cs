using ImageSorter.Model;
using MediaInfo;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSorter.Controllers
{
                            
    internal class VideoReader
    {
        DateTime _startDate;
        DateTime _endDate;
        public List<FileInfo> VideoList(string sourcePath, DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;

            List<FileInfo> sortedVideos = new List<FileInfo>();
            DirectoryInfo directoryInfo = new DirectoryInfo(sourcePath);
            directoryInfo.EnumerateFiles();
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                try
                {
                    switch (file.Extension.ToUpper())
                    {
                        case ".MP4":
                            if (ReadVideo(file))
                            {
                                sortedVideos.Add(file);
                            }
                            break;
                        case ".HEVC":
                            if (ReadVideo(file))
                            {
                                sortedVideos.Add(file);
                            }
                            break;
                        default:
                            continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to read {file.Name}");
                }
            }
            return sortedVideos;
        }
        private bool ReadVideo(FileInfo file)
        {
            var mediaInfo = new MediaInfoWrapper(file.FullName);
            var dateTaken = mediaInfo.Tags.RecordedDate;
            if (_startDate < dateTaken && dateTaken < _endDate)
                return true;
            return false;
        }

    }
}
