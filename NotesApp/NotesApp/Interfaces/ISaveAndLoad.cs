using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public interface ISaveAndLoad
    {
        System.IO.Stream GetStreamFromPath(string path);
        List<String> GetSubDirectoryPaths(string rootPath);
        bool CheckFileExists(string rootPath);
        string GetShortDirName(string dirPath);
    }
}
