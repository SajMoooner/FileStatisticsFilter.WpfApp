using System.Text.RegularExpressions;
using FileStatisticsFilter.CommonLibrary;
class Program
{
    static void Main(string[] args)
    {
        // Default values
        string inputCsv = "windows.csv";
        string outputCsv = "windows1.csv";
        string regexPattern = null;
        bool recursive = false;

        // Process arguments
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--input":
                    if (i + 1 < args.Length) // Check if there's another argument after this
                        inputCsv = args[++i]; // Increment i to get the value
                    break;
                case "--output":
                    if (i + 1 < args.Length) // Check if there's another argument after this
                        outputCsv = args[++i]; // Increment i to get the value
                    break;
                case "--regex":
                    if (i + 1 < args.Length) // Check if there's another argument after this
                        regexPattern = args[++i]; // Increment i to get the value
                    break;
                case "--recursive":
                    recursive = true;
                    break;
            }
        }

        // Check that required arguments were provided
        if (string.IsNullOrEmpty(inputCsv) || string.IsNullOrEmpty(outputCsv))
        {
            Console.WriteLine("Missing required arguments --input and/or --output");
            return;
        }

        // Check if inputCsv file exists
        if (!File.Exists(inputCsv))
        {
            Console.WriteLine($"Input CSV file does not exist: {inputCsv}");
            return;
        }

        // Now you can use the variables inputCsv, outputCsv, regexPattern, and recursive in your program
        // For example:

        var searchedFiles = new SearchedFiles();

        if (string.IsNullOrEmpty(regexPattern) || Regex.IsMatch(inputCsv, regexPattern))
        {
            searchedFiles.LoadFromCsv(new FileInfo(inputCsv)); // Load CSV file
        }

        // print searched files
        foreach (var file in searchedFiles.Files)
        {
          
        }

        //save searched files to csv file
        searchedFiles.SaveToCsv(new FileInfo(outputCsv));

    }
}
