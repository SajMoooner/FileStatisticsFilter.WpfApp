using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileStatisticsFilter.CommonLibrary;

namespace FileStatisticsFilter.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SearchedFiles searchedFiles;  // Make searchedFiles a class-level variable

        public MainWindow()
        {
            InitializeComponent();
        }


        private void OpenFileExplorer_Click(object sender, RoutedEventArgs e)
        {
            //---------------Otvor okno------------------//
            // Create an instance of OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Set filter options and filter index
            openFileDialog.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";

            // Call the ShowDialog method to show the dialog box
            bool? userClickedOK = openFileDialog.ShowDialog();

            //------------Ulož subor --------------------//
            // Process input if the user clicked OK
            if (userClickedOK == true)
            {
                // Create an instance of SearchedFiles
                searchedFiles = new SearchedFiles();

                // Load the selected CSV file
                searchedFiles.LoadFromCsv(new FileInfo(openFileDialog.FileName));

                //---------Vyplne listview------------------//
                // Set the ItemsSource property of the ListView to the list of loaded files
                FilesListView.ItemsSource = searchedFiles.Files;


                //---------Label nastav------------------//
                // set label to show the number of files loaded
                FilesCount.Content = "Files Loaded: " + searchedFiles.Files.Count.ToString("N0");

                // create sum of all file sizes with LINQ
                var totalSize = searchedFiles.Files.Sum(f => f.Size);
                Suma.Content = "Total Size: " + totalSize.ToString("N0") + " bytes";

                //-------Nacitaj drodown----------------//
                // Get unique directories
                var uniqueDirectories = searchedFiles.Files.Select(f => f.Directory).Distinct();

                // Clear the ComboBox (in case this is not the first time loading files)
                DirectoryComboBox.Items.Clear();

                // Add unique directories to the ComboBox
                foreach (var directory in uniqueDirectories)
                {
                    DirectoryComboBox.Items.Add(directory);
                }

            }
        }

        private void DirectoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected directory
            string selectedDirectory = (string)DirectoryComboBox.SelectedItem;

            // Filter the ListView based on the selected directory
            FilesListView.ItemsSource = searchedFiles.Files.Where(f => f.Directory == selectedDirectory).ToList();
        }
    }
}
