using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NotesApp.Droid
{
    public class SaveAndLoad : ISaveAndLoad
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
            if(info != null)
            {
                return info.Name;
            }
            return null;
        }

        public async Task<Stream> GetStreamFromPath(string path)
        {
            if (File.Exists(path))
            {
                RemoveBreakLinesAndTab(path);
                FileStream fileStream = new FileStream(path, FileMode.Open);
                return fileStream;
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

        private void RemoveBreakLinesAndTab(string path)
        {
            string fileText = File.ReadAllText(path);
            fileText = Regex.Replace(fileText, @"(\r\n|\n|\t)", String.Empty);
            File.WriteAllText(path,fileText);
        }

        public void SaveText(string text, string directoryPath, string fileName)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(directoryPath + fileName, text);

        }
    }
}