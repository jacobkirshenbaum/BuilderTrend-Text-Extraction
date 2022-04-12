using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace Capstone.UI;

public partial class MainWindow : Window
{
    private ElasticAccess elastic = new();
    private FileProcessor processor = new();
    
    public MainWindow()
    {
        List<File> files = elastic.SearchAll();
        List<String> fileNames = new();
        foreach (var file in files)
        {
            fileNames.Add(file.FileName);
        }
        InitializeComponent();
        dataBinding.ItemsSource = fileNames;
    }

    private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        
        List<String> fileNames = new();
        if (filter.Text.Equals("File Name"))
        {
            List<File> filesByName = elastic.SearchByName(txtNameToSearch.Text);
            foreach (var file in filesByName)
            {
                fileNames.Add(file.FileName);
            }
        }
        else if (filter.Text.Equals("File Contents"))
        {
            List<File> filesByText = elastic.SearchByText(txtNameToSearch.Text);
            foreach (var file in filesByText)
            {
                if (!fileNames.Contains(file.FileName))
                {
                    fileNames.Add(file.FileName);
                }
            }
        }
        else
        {
            List<File> filesByName = elastic.SearchByName(txtNameToSearch.Text);
            foreach (var file in filesByName)
            {
                fileNames.Add(file.FileName);
            }
            List<File> filesByText = elastic.SearchByText(txtNameToSearch.Text);
            foreach (var file in filesByText)
            {
                if (!fileNames.Contains(file.FileName))
                {
                    fileNames.Add(file.FileName);
                }
            }
        }
        dataBinding.ItemsSource = fileNames;
    }

    private void openFileExplorer(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new();
        openFileDialog.Multiselect = true;
        if (openFileDialog.ShowDialog() == true)
        {
            var files = processor.ReadFile(openFileDialog.FileNames);
            elastic.IndexDocuments(files);
        }
        MessageBox.Show("File(s) added");
    }
}