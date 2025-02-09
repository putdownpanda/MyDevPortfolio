using ImageSorter.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSorter.Model
{
    public class Instruction
    {
        public Instruction(string sourcePath, DateTime startDate, DateTime endDate, string destPath) {
            SourcePath = sourcePath;
            StartDate = startDate;
            EndDate = endDate;
            DestinationPath = destPath;
        }
        public string SourcePath { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get;  }
        public string DestinationPath { get; }
        public List<FileInfo> Files { get; set; }
        public List<string> ErrorFiles { get; set; }

    }
}
