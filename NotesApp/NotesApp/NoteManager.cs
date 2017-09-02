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

        private string selectedDirPath = "";

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
            selectedDirPath = rootPath;

            List<string> subDirPaths = datamanager.GetSubDirectoryPaths(rootPath);

            foreach(string dirPath in subDirPaths)
            {
                string shortName = datamanager.GetShortDirName(dirPath);
                if(datamanager.CheckFileExists(dirPath + "/" + shortName + Note.noteContentFormat))
                {
                    NoteConnector newConnector = new NoteConnector(new Note(shortName, ""));
                    noteConnectors.Add(newConnector);
                    noteTitles.Add(shortName);
                }
            }
        }

        private async Task<bool> InitializeNote(int position)
        {
            NoteConnector newConnector = noteConnectors.ElementAt(position);

            //Always parse styl before html if you want css stylings
            if (datamanager.CheckFileExists(selectedDirPath + "/" + newConnector.insideNote.title + "/" + newConnector.insideNote.title + Note.noteStyleFormat))
            {
                fileStream = await datamanager.GetStreamFromPath(selectedDirPath + "/" + newConnector.insideNote.title + "/" + newConnector.insideNote.title + Note.noteStyleFormat);
                CSSStyleManager contentStyle = parser.ParseCSS(fileStream);
                newConnector.SetNoteContentStyle(contentStyle);
            }
            if (datamanager.CheckFileExists(selectedDirPath + "/" + newConnector.insideNote.title + "/" + newConnector.insideNote.title + Note.noteContentFormat))
            {
                fileStream = await datamanager.GetStreamFromPath(selectedDirPath + "/" + newConnector.insideNote.title + "/" + newConnector.insideNote.title + Note.noteContentFormat);
                if (fileStream != null)
                {
                    SimpleHtmlNode content = parser.ParseXML(fileStream, newConnector.insideNote.contentStyle);
                    newConnector.SetNoteContent(content);
                    return true;
                }
                
            }

            return false;
        }

        public async Task<NoteConnector> GetNoteFromPosition(int position)
        {
            await InitializeNote(position);
            return noteConnectors.ElementAt(position);
        }




    }
}
