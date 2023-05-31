using System.Globalization;

namespace FileStatisticsFilter.CommonLibrary
{
    public class SearchedFile
    {
   
        public string Directory { get; set; }
        public string FileNameWithoutExtension { get; set; }
        public string Extension { get; set; }
        public string FileName => $"{FileNameWithoutExtension}{Extension}";
        public string FullName => $"{Directory}\\{FileName}";
        public long Size { get; set; }
        public string SizeReadable { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public bool isReadOnly { get; set; }

        public SearchedFile(FileInfo fileInfo)
        {
            CreatedTime = fileInfo.CreationTime;
            Directory = fileInfo.DirectoryName;
            Extension = fileInfo.Extension;
            FileNameWithoutExtension = fileInfo.Name.Replace(fileInfo.Extension, "");
            isReadOnly = fileInfo.IsReadOnly;
            ModifiedTime = fileInfo.LastWriteTime;
            Size = fileInfo.Length;
            SizeReadable = (fileInfo.Length / 1024 / 1024).ToString();
        }

        public SearchedFile(string csvLine, char delimiter = ';')
        {
            var fields = csvLine.Split(delimiter);
            
            if (fields.Length < 6)
            {
      
                throw new ArgumentException("CSV line does not have enough fields.");
            }

            Directory = fields[0];
            FileNameWithoutExtension = fields[1];
            Extension = fields[2];
            Size = long.Parse(fields[3]);  // Use long.Parse instead of int.Parse
            CreatedTime = DateTime.ParseExact(fields[4], "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            ModifiedTime = DateTime.ParseExact(fields[5], "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            isReadOnly = bool.Parse(fields[6]);
        }



        public static string ToCsvHeaderLine(char delimiter = '\t')
        {
            return $"Directory{delimiter};FileNameWithoutExtension{delimiter};Extension{delimiter};Size{delimiter};CreatedTime{delimiter};ModifiedTime{delimiter}IsReadOnly";
        }

        public string ToCsvLine(char delimiter = '\t')
        {
            return $"{Directory}{delimiter};{FileNameWithoutExtension}{delimiter};{Extension};{delimiter}{Size};{delimiter}{CreatedTime:O};{delimiter}{ModifiedTime:O};{delimiter}{isReadOnly};";
        }
    }

    public class SearchedFiles
    {
        private List<SearchedFile> files;

        public List<SearchedFile> Files
        {
            get { return files; }
        }

        public SearchedFiles()
        {
            this.files = new List<SearchedFile>();
        }

        //konštruktor, ktorý podľa zadaného reťazca reprezentujúceho jeden riadok CSV súboru nastaví vlastnosti inštancie SearchedFile
        public SearchedFiles(IEnumerable<FileInfo> fileInfos)
        {
            this.files = fileInfos.Select(file => new SearchedFile(file)).ToList();
        }

        public void LoadFromCsv(FileInfo file)
        {
            using (StreamReader sr = new StreamReader(file.FullName))
            {
                // Read and discard the first line (header line)
                sr.ReadLine();

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var searchedFile = new SearchedFile(line);
                    this.files.Add(searchedFile);
                }
            }
        }

        public void SaveToCsv(FileInfo file)
        {
            using (StreamWriter sw = file.CreateText())
            {
                sw.WriteLine(SearchedFile.ToCsvHeaderLine());
                foreach (var searchedFile in this.Files)
                {
                    sw.WriteLine(searchedFile.ToCsvLine());
                }
            }
        }
    }
} 