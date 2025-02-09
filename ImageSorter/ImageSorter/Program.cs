using ImageSorter.Controllers;
using ImageSorter.Model;
using System;
using System.Globalization;

var dateFormat = "yyyy-MM-dd";
var dateTimeFormat = "yyyy-MM-dd/hh:mm:ss";

Console.WriteLine("Welcome to the Image&Video Sorter.");
Console.WriteLine("This program will sort images and videos based on the date taken.");
Console.WriteLine($"DateTime formats can be either {dateTimeFormat} or {dateFormat}.");
Console.WriteLine("enter as many date ranges as you want to sort the images and videos by. Type 'n' when all lines are entered.");
Console.WriteLine("Enter the files in either of the following formats:SourcePath yyyy-MM-dd-hh:mm:ss/yyyy-MM-dd-hh:mm:ss DestinationPath");

CultureInfo provider = CultureInfo.InvariantCulture;
List<Instruction> sortInstructions = new List<Instruction>();
List<string> inputLines = new List<string>();
while (true)
{
    string line = Console.ReadLine();
    if (line == "n")
    {
        break;
    }

    string[] lineArray = line.Split(' ');
    string sourcePath = lineArray[0];
    string[] dates = lineArray[1].Split('/');
    string startDate = dates[0];
    string endDate = dates[1];
    string destinationPath = lineArray[2];
    if (sourcePath == null || startDate == null || endDate == null || destinationPath == null)
    {
        Console.WriteLine($"Invalid input {line}. Please try again.");
        continue;
    }
    string inputDateFormatStart = string.Empty;
    string inputDateFormatEnd = string.Empty;
    switch (startDate.Length)
    {
        case 10:
            inputDateFormatStart = dateFormat;
            break;
        case 19:
            inputDateFormatStart = dateTimeFormat;
            break;
        default:
            Console.WriteLine($"Invalid input {line}. Please try again.");
            continue;
    }
    switch (endDate.Length)
    {
        case 10:
            inputDateFormatEnd = dateFormat;
            continue;
        case 19:
            inputDateFormatEnd = dateTimeFormat;
            break;
        default:
            Console.WriteLine($"Invalid input {line}. Please try again.");
            continue;
    }
    Instruction instruction = new Instruction(sourcePath
        , DateTime.ParseExact(startDate, inputDateFormatStart, provider)
        , DateTime.ParseExact(endDate, inputDateFormatEnd, provider)
        , destinationPath);
    sortInstructions.Add(instruction);
}

foreach (var instructions in sortInstructions)
{
    Console.WriteLine("The Following Instructions have been entered:");
    Console.WriteLine($"SourcePath: {instructions.SourcePath} StartDate: {instructions.StartDate} EndDate: {instructions.EndDate} DestinationPath: {instructions.DestinationPath}");

}
Console.WriteLine("Continue? y/n");
if(Console.ReadLine() == "y")
{
    Console.WriteLine("Continuing");
}
else
{
    Console.WriteLine("Exiting");
    Environment.Exit(0);
}
ImageReader imageReader = new ImageReader();
VideoReader videoReader = new VideoReader();
FileMover fileMover = new FileMover();
foreach (var instruction in sortInstructions)
{
    instruction.Files = imageReader.SortImages(instruction.SourcePath,instruction.StartDate,instruction.EndDate);
    Console.WriteLine("Images sorted");
    Console.WriteLine("Now Sorting Videos");
    instruction.Files.AddRange(videoReader.VideoList(instruction.SourcePath,instruction.StartDate, instruction.EndDate));
    Console.WriteLine("Videos sorted");
    
    fileMover.SortFiles(instruction.DestinationPath,instruction.Files);
}

