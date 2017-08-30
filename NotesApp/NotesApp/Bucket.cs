using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class Bucket
    {
        public static string bucketsRootFolder = "/data/data/NotesApp.Droid/files/Buckets";

        public string title;
        public string path;
        public string appearance;
        public List<NoteConnector> containingNotes;

    }
}
