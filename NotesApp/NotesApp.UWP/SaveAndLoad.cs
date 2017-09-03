using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace NotesApp.UWP
{
    public class SaveAndLoad
    {
        public bool CheckFileExists(string rootPath)
        {
            try
            {
                if (File.Exists(rootPath))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("There has been an error Checking File Existance:" + e.Message);
            }
            return false;
        }

        public string GetShortDirName(string dirPath)
        {
            DirectoryInfo info = new DirectoryInfo(dirPath);
            if (info != null)
            {
                return info.Name;
            }
            return null;
        }

        public async Task<Stream> GetStreamFromPath(string path)
        {
            if (File.Exists(path))
            {

                //Windows.Storage.ApplicationData.Current.LocalFolder.Path
                string filePath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "/" + path;
                filePath = filePath.Replace("/", "\\");
                string filePath2 = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "/" + path;
                filePath2 = filePath2.Replace("/", "\\");

                if (File.Exists(filePath))
                {

                    StorageFile file = await StorageFile.GetFileFromPathAsync(filePath);

                    //StorageFile newLocalfile = await RemoveBreakLinesAndTab(file, filePath2);

                    //FileStream fileStream = new FileStream(path, FileMode.Open);

                    return await file.OpenStreamForReadAsync();
                }
            }
            return null;
        }

        public List<string> GetSubDirectoryPaths(string rootPath)
        {
            List<String> subDirPaths = new List<string>();
            try
            {
                if (Directory.Exists(rootPath))
                {
                    DirectoryInfo info = new DirectoryInfo(rootPath);
                    DirectoryInfo[] subDirectories = info.GetDirectories().ToArray();
                    foreach (DirectoryInfo singleDir in subDirectories)
                    {
                        subDirPaths.Add(singleDir.FullName);
                    }
                    return subDirPaths;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("There has been an error acquiring subdir Paths:" + e.Message);
            }
            return subDirPaths;
        }

        private async Task<StorageFile> RemoveBreakLinesAndTab(StorageFile file, string destination)
        {

            string fileText = await Windows.Storage.FileIO.ReadTextAsync(file);

            fileText = Regex.Replace(fileText, @"(\r\n|\n|\t)", String.Empty);

            StorageFile newFile = await StorageFile.GetFileFromPathAsync(destination);
            
            await Windows.Storage.FileIO.WriteTextAsync(newFile, fileText);

            return newFile;
        }

        public void SaveText(string text, string directoryPath, string fileName)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(directoryPath + fileName, text);

        }

        public async Task<string> LoadText(string filePath)
        {
            StorageFile readFile = await StorageFile.GetFileFromPathAsync(filePath);
            string fileText = await FileIO.ReadTextAsync(readFile);
            return fileText;
        }

        public async Task<string> LoadText(string directoryPath, string fileName)
        {
            return await LoadText(directoryPath + fileName);
        }

        public List<string> GetSubFilePaths(string directoryPath)
        {
            List<String> filePaths = new List<string>();
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    DirectoryInfo info = new DirectoryInfo(directoryPath);
                    FileInfo[] files = info.GetFiles().ToArray();
                    foreach (FileInfo singleFile in files)
                    {
                        filePaths.Add(singleFile.FullName);
                    }
                    return filePaths;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("There has been an error acquiring subdir Paths:" + e.Message);
            }
            return filePaths;
        }
    }
}
