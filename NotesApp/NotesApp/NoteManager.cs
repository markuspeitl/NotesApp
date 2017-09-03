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

        public List<NoteProxy> noteConnectors;
        public List<string> noteTitles;

        private CustomHtmlParser parser;
        private Stream fileStream;

        private string selectedDirPath = "";

        //private SimpleNodeAccessor accessor;

        public NoteManager(ISaveAndLoad datamanager)
        {
            this.datamanager = datamanager;
            this.parser = new CustomHtmlParser();

            noteConnectors = new List<NoteProxy>();
            noteTitles = new List<string>();
        }

        public List<string> LoadNoteList(string rootPath)
        {
            selectedDirPath = rootPath;

            /*List<string> subDirPaths = datamanager.GetSubDirectoryPaths(rootPath);
            foreach(string dirPath in subDirPaths)
            {
                string shortName = datamanager.GetShortDirName(dirPath);
                if(datamanager.CheckFileExists(dirPath + "/" + shortName + Note.noteContentFormat))
                {
                    NoteConnector newConnector = new NoteConnector(new Note(shortName, ""));
                    noteConnectors.Add(newConnector);
                    noteTitles.Add(shortName);
                }
            }*/
            List<string> fileNameList = datamanager.GetSubFilePaths(rootPath);
            foreach (string filePath in fileNameList)
            {
                string shortName = StripExtension(GetNameOfFile(filePath));
                if (datamanager.CheckFileExists(filePath))
                {
                    NoteProxy newConnector = new NoteProxy(filePath, datamanager);
                    noteConnectors.Add(newConnector);

                    if (newConnector.GetMetaData().title != "")
                    {
                        noteTitles.Add(newConnector.GetMetaData().title);
                    }
                    else
                    {
                        noteTitles.Add(newConnector.GetMetaData().createdDate);
                    }
                }
            }
            return noteTitles;
        }
        private string GetNameOfFile(string path)
        {
            string[] splitpath = path.Split('\\','/');
            return splitpath[splitpath.Length - 1];
        }
        private string StripExtension(string path)
        {
            string[] splitpath = path.Split('.');
            string shortname = "";
            for(int i = 0; i < splitpath.Length - 1; i++)
            {
                shortname += splitpath[i];
            }
            return shortname;
        }

        public async void SaveNoteToXML(ISaveAndLoad dataManager,NoteProxy currentNote)
        {
            currentNote.SaveMetaData();
            NoteContent insideNote = currentNote.GetContent();
            string noteRepresentation = insideNote.ToXML().ToString();
            NoteMeta meta = currentNote.GetMetaData();

            currentNote.SaveMetaData();
            dataManager.SaveText(noteRepresentation, meta.ContentPath, meta.title + meta.extension);
        }

        private async Task<NoteProxy> InitializeNote(int position)
        {
            NoteProxy newConnector = noteConnectors.ElementAt(position);

            //Always parse styl before html if you want css stylings
            /*if (datamanager.CheckFileExists(selectedDirPath + "/" + newConnector.insideNote.title + "/" + newConnector.insideNote.title + Note.noteStyleFormat))
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
                    SimpleHtmlNode content = parser.ParseXML2(fileStream, newConnector.insideNote.contentStyle);
                    newConnector.SetNoteContent(content);
                    return true;
                }
            }
            return false;*/
            return newConnector;
        }

        public async Task<NoteProxy> GetNoteFromPosition(int position)
        {
            return await InitializeNote(position);
        }




    }
}
