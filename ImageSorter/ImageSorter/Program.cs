using System;
using System.Globalization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.PixelFormats;

Console.WriteLine("Enter the path to the images");
string imagesPath = Console.ReadLine();
Console.WriteLine($"The path you entered is {imagesPath}");

Console.WriteLine("Enter the path to the folder you want to move the images to");
Console.WriteLine("If the folder does not exist it will be created");
string destinationPath = Console.ReadLine();
Console.WriteLine($"The path you entered is {destinationPath}");
// Ensure the destination directory exists
Directory.CreateDirectory(destinationPath);

var dateFormat = "yyyy-MM-dd";
CultureInfo provider = CultureInfo.InvariantCulture;

var startDate = new DateTime();
var endDate = new DateTime();

while (true)
{
    try {
        Console.WriteLine("Now Enter the date range you want to sort the images by");
        Console.WriteLine("Enter the start date in the format yyyy-MM-dd");
        string start = Console.ReadLine();
        startDate = DateTime.ParseExact(start, dateFormat, provider);

        Console.WriteLine("Enter the end date in the format yyyy-MM-dd");
        string end = Console.ReadLine();
        endDate = DateTime.ParseExact(end, dateFormat, provider);
        break;
    }
    catch (Exception ex)
    {        
        Console.WriteLine("The date you entered is not in the correct format");
        Console.WriteLine("Please try again");
    }
    continue;
}
Console.WriteLine($"The date range you entered is {startDate} to {endDate}");

//"C:\\Users\\matth\\Pictures\\Namibia 2024 November\\IMG_1540.JPG";
//now i need to go to the folder and iterate through the folder
DirectoryInfo directoryInfo = new DirectoryInfo(imagesPath);
directoryInfo.EnumerateFiles();
foreach (FileInfo file in directoryInfo.EnumerateFiles())
{
    //now a switch statement to check the file extension
    switch (file.Extension)
    {
        case ".jpg":
            break;
        case ".JPG":
            break;
        case ".jpeg":
            break;
        case ".JPEG":
            break;
        case ".png":
            break;
        case ".PNG":
            break;
        default:
            continue;
    }

    using (Image<Rgba32> image = Image.Load<Rgba32>(file.FullName))
    {
        ImageMetadata metadata = image.Metadata;
        var exifProfile = metadata.ExifProfile;

        if (exifProfile != null)
        {
            if (exifProfile.TryGetValue(SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.DateTimeOriginal, out var dateTimeOriginal))
            {
                string dateTimeString = dateTimeOriginal.ToString();
                if (DateTime.TryParseExact(dateTimeString, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    Console.WriteLine($"Date and Time Original: {dateTime}");
                    if (startDate <= dateTime && dateTime <= endDate)
                    {
                        // Move the file to the correct folder
                        string destinationFilePath = Path.Combine(destinationPath, file.Name);
                        File.Move(file.FullName, destinationFilePath);
                        Console.WriteLine($"Moved {file.Name} to {destinationPath}");
                    }
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
}