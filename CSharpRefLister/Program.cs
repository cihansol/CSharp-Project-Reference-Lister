using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using DotNetProjectParser;

namespace CSharpRefLister
{
    public class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("CSharpRefLister");
            Console.WriteLine("Author: Cihan");
            Console.WriteLine(string.Empty);

            if (args.Count() < 2)
            {
                Console.WriteLine("Usage: CSharpRefLister -csproj path/to/AppProject.csproj");
                Console.WriteLine("Usage: CSharpRefLister -dir path/to/folder");
                Console.WriteLine(" ");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }

            string workingDirectory = Utilities.GetApplicationDirectory();
            string inputType = args[0];

            switch (inputType)
            {
                case "-csproj":
                    HandleProjectFile(workingDirectory, args[1]);
                    break;
                case "-dir":
                    HandleDIR(workingDirectory, args[1]);
                    break;
                default:
                    Console.WriteLine($"Invalid input type: {inputType}");
                    break;
            }

            Console.ReadKey();

        }

        static void HandleProjectFile(string workingDirectory, string inputFilePath)
        {    
            string csProjFilePath = inputFilePath;
            if (!File.Exists(csProjFilePath))
            {
                //Check the working directory
                string newWorkingDirPath = Path.Combine(workingDirectory, csProjFilePath);
                if (File.Exists(newWorkingDirPath))
                    csProjFilePath = newWorkingDirPath;
                else
                {
                    Console.WriteLine($"Error Input file {inputFilePath} doesn't seem to exist!");
                    return;
                }
            }

            FileInfo file = new FileInfo(inputFilePath);
            Console.WriteLine($"Processing file {file.Name}");
            Project proj = DotNetProjectParser.ProjectFactory.GetProject(file);
            if (proj != null)
            {
                string outputFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath), $"{file.Name}_processed.log");
                using (StreamWriter sw = new StreamWriter(outputFilePath, false)) 
                {         
                    foreach (var item in proj.Items)
                    {
                        if (item.ItemType == "PackageReference")
                        {
                            sw.WriteLine($"Name: {item.ItemName} Version: {item.Version}");
                        }
                    }
                    sw.Flush();
                }
                Console.WriteLine(proj.Name.ToString());
            }
            Console.WriteLine($"Process done!");

            return;
        }


        static void HandleDIR(string workingDirectory, string inputFolderPath)
        {
            if (!Directory.Exists(inputFolderPath))
            {
                Console.WriteLine($"Error directory {inputFolderPath} doesn't seem to exist!");
                return;
            }

            string[] projFiles = Directory.GetFiles(inputFolderPath, "*.csproj", SearchOption.AllDirectories);
            if (projFiles.Length == 0)
            {
                Console.WriteLine($"No (.csproj) project files found in folder {inputFolderPath}");
                return;
            }

            foreach (var file in projFiles)
            {
                HandleProjectFile(workingDirectory, file);
            }

            Console.WriteLine($"Processing of csproj files complete in directory: '{inputFolderPath}'");
            return;
        }

    }

}