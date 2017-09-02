using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public interface ISaveAndLoad
    {
        Task<System.IO.Stream> GetStreamFromPath(string path);
        List<String> GetSubDirectoryPaths(string rootPath);
        bool CheckFileExists(string rootPath);
        string GetShortDirName(string dirPath);
        void SaveText(string text, string directoryPath, string fileName);
    }
}
