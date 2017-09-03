using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class Bucket
    {
        private NoteMeta bucketMetaData;
        public string path;
        public string appearance;

        public List<NoteProxy> containingNotes;

    }
}
