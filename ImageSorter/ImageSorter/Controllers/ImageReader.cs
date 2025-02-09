using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using Image = SixLabors.ImageSharp.Image;
using ImageSorter.Model;

namespace ImageSorter.Controllers
{
    internal class ImageReader
    {
        DateTime _startDate;
        DateTime _endDate;
        public List<FileInfo> SortImages(string sourcePath, DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;

            List<FileInfo> sortedImages = new List<FileInfo>();
            DirectoryInfo directoryInfo = new DirectoryInfo(sourcePath);
                directoryInfo.EnumerateFiles();
                foreach (FileInfo file in directoryInfo.EnumerateFiles())
                {
                    try
                    {
                        switch (file.Extension.ToUpper())
                        {
                            case ".JPG":
                                if (ReadImage(file))
                                {
                                    sortedImages.Add(file);
                                }
                                break;
                            case ".JPEG":
                                if (ReadImage(file))
                                {
                                    sortedImages.Add(file);
                                }
                                break;
                            case ".PNG":
                                if (ReadImage(file))
                                {
                                    sortedImages.Add(file);
                                }
                                break;
                            default:
                                continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                return sortedImages;
        }
        private bool ReadImage(FileInfo fileImage)
        {
            using (Image<Rgba32> image = Image.Load<Rgba32>(fileImage.FullName))
            {
                ImageMetadata metadata = image.Metadata;
                var exifProfile = metadata.ExifProfile;

                if (exifProfile != null)
                {
                    if (exifProfile.TryGetValue(SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.DateTimeOriginal, out var dateTimeOriginal))
                    {
                        if (DateTime.TryParseExact(dateTimeOriginal.ToString(), "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime imageDT))
                        {
                            Console.WriteLine($"Date and Time Original: {imageDT}");
                            if(_startDate < imageDT && imageDT < _endDate)
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Failed to parse DateTimeOriginal.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("DateTimeOriginal tag not found.");
                    }
                }
                else
                {
                    Console.WriteLine("No EXIF profile found.");
                }
            }
            return false;
        }
    }
}
