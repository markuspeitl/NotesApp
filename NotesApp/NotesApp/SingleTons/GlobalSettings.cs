using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public static class GlobalSettings
    {
        private static string rootPath;
        private const string relativeNotesPath = "Notes/Notes/";
        private const string relativeNoteProxyPath = "Notes/Proxys/";
        private const string relativeNoteBucketPath = "Notes/Buckets/";

        public static string GetNotesPath()
        {
            return rootPath + relativeNotesPath;
        }

        public static string GetProxysPath()
        {
            return rootPath + relativeNoteProxyPath;
        }

        public static string GetBucketsPath()
        {
            return rootPath + relativeNoteBucketPath;
        }

        public static void SetupRootPath(string workingDirectoryPath)
        {
            rootPath = workingDirectoryPath;
        }

    }
}
