using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class NoteManager
    {
        private ISaveAndLoad datamanager;

        public List<NoteConnector> noteConnectors;
        public List<string> noteTitles;

        private CustomHtmlParser parser;
        private Stream fileStream;

        //private SimpleNodeAccessor accessor;

        public NoteManager(ISaveAndLoad datamanager)
        {
            this.datamanager = datamanager;
            this.parser = new CustomHtmlParser();

            noteConnectors = new List<NoteConnector>();
            noteTitles = new List<string>();

            LoadNotes(Note.noteRootPath);
        }

        public void LoadNotes(string rootPath)
        {
            List<string> subDirPaths = datamanager.GetSubDirectoryPaths(rootPath);

            foreach(string dirPath in subDirPaths)
            {
                string shortName = datamanager.GetShortDirName(dirPath);
                if(datamanager.CheckFileExists(dirPath + "/" + shortName + Note.noteContentFormat))
                {
                    NoteConnector newConnector = new NoteConnector(new Note(shortName, ""));
                    
                    //Always parse styl before html if you want css stylings
                    if (datamanager.CheckFileExists(dirPath + "/" + shortName + Note.noteStyleFormat))
                    {
                        fileStream = datamanager.GetStreamFromPath(dirPath + "/" + shortName + Note.noteStyleFormat);
                        CSSStyleManager contentStyle = parser.ParseCSS(fileStream);
                        newConnector.SetNoteContentStyle(contentStyle);
                    }

                    fileStream = datamanager.GetStreamFromPath(dirPath + "/" + shortName + Note.noteContentFormat);
                    SimpleHtmlNode content = parser.ParseXML(fileStream, newConnector.insideNote.contentStyle);
                    newConnector.SetNoteContent(content);

                    noteConnectors.Add(newConnector);
                    noteTitles.Add(shortName);
                    
                }
            }
        }

        public NoteConnector GetNoteFromPosition(int position)
        {
            return noteConnectors.ElementAt(position);
        }




    }
}
