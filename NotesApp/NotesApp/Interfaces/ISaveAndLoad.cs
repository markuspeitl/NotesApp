using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public interface ISaveAndLoad
    {
        Task<System.IO.Stream> GetReadStreamFromPath(string path);
        Task<System.IO.Stream> GetWriteStreamFromPath(string path);

        List<String> GetSubDirectoryPaths(string rootPath);
        bool CheckFileExists(string rootPath);
        string GetShortDirName(string dirPath);
        void SaveText(string text, string directoryPath, string fileName);

        /*Task<string>*/string LoadText(string filePath);
        /*Task<string>*/string LoadText(string directoryPath, string fileName);

        List<String> GetSubFilePaths(string directoryPath);
    }
}
