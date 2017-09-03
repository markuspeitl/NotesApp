using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NotesApp
{
    
    public class NoteProxy
    {
        private string metaDataPath;
        private NoteMeta metaData;

        private bool contentLoaded = false;
        private NoteContent noteContent;

        private ISaveAndLoad dataManager;

        public NoteProxy(string readMetaDataPath, ISaveAndLoad dataManager)
        {
            this.dataManager = dataManager;
            this.metaDataPath = readMetaDataPath;
            this.LoadMetaData(this.metaDataPath);
        }

        public NoteProxy(string createMetaDataDirectory,NoteMeta metaData, ISaveAndLoad dataManager)
        {
            this.dataManager = dataManager;
            this.metaDataPath = createMetaDataDirectory + System.Guid.NewGuid().ToString() + ".xml";
            this.metaData = metaData;
            this.SaveMetaData();
        }

        public NoteMeta GetMetaData()
        {
            return metaData;
        }

        public string GetProxyPath()
        {
            return this.metaDataPath;
        }

        private async void LoadMetaData(string path)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Loading Meta Data Started");
                DataContractSerializer dcs = new DataContractSerializer(typeof(NoteMeta));
                System.Diagnostics.Debug.WriteLine("Loading Meta Data Started Line1");
                Stream readStream = await dataManager.GetReadStreamFromPath(path);
                System.Diagnostics.Debug.WriteLine("Loading Meta Data Started Line2");
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(readStream, XmlDictionaryReaderQuotas.Max);
                System.Diagnostics.Debug.WriteLine("Loading Meta Data Started Line3");
                metaData = (NoteMeta)dcs.ReadObject(reader);
                System.Diagnostics.Debug.WriteLine("Loading Meta Data Finished");
                readStream.Dispose();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to load metadata - Reason: " + e.Message);
            }
        }
        public async void SaveMetaData()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Saving Meta Data Started");
                DataContractSerializer dcs = new DataContractSerializer(metaData.GetType());
                System.Diagnostics.Debug.WriteLine("Saving Meta Data Started Line1");
                Stream writeStream = await dataManager.GetWriteStreamFromPath(metaDataPath);
                System.Diagnostics.Debug.WriteLine("Saving Meta Data Started Line2");
                //XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateTextWriter(writeStream, Encoding.UTF8);
                System.Diagnostics.Debug.WriteLine("Saving Meta Data Started Line3");
                dcs.WriteObject(writeStream, metaData);
                System.Diagnostics.Debug.WriteLine("Saving Meta Data Finished");
                writeStream.Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to save metadata - Reason: " + e.Message);
            }
        }

        //Loads the note contents from a file or returns them if already loaded
        public /*async Task<NoteContent>*/NoteContent GetContent()
        {
            System.Diagnostics.Debug.WriteLine("NoteProxy GetContent called");
            if (!contentLoaded)
            {
                if (!this.metaData.ContentPath.Equals(""))
                {
                    string content = dataManager.LoadText(this.metaData.ContentPath);
                    if (!content.Equals(""))
                    {
                        if (this.metaData.contentType == NoteMeta.DOCTYPE.HTML)
                        {
                            noteContent = new NoteHTMLContent();
                            noteContent.FromXML(content);
                            contentLoaded = true;
                        }
                        else if (this.metaData.contentType == NoteMeta.DOCTYPE.XML)
                        {
                            noteContent = new NoteXMLContent();
                            noteContent.FromXML(content);
                            contentLoaded = true;
                        }
                    }
                    else
                    {
                        SaveProxyMetaAndContent();
                        contentLoaded = true;
                    }
                }
                else
                {
                    SaveProxyMetaAndContent();
                    contentLoaded = true;
                }
                
            }
            System.Diagnostics.Debug.WriteLine("NoteProxy GetContent ended");
            return noteContent;
        }

        public void SaveProxyMetaAndContent()
        {
            System.Diagnostics.Debug.WriteLine("SaveProxyMetaAndContent called");
            if (noteContent == null)
            {
                noteContent = new NoteXMLContent();
                System.Diagnostics.Debug.WriteLine("Content is null -> make new");
            }
            if (this.metaData.ContentPath.Equals(""))
            {
                System.Diagnostics.Debug.WriteLine("Set new Content Path from uuid");
                //Set Path
                string newNoteUUID = System.Guid.NewGuid().ToString();
                this.metaData.ContentPath = GlobalSettings.GetNotesPath() + newNoteUUID + ".xml";
            }

            System.Diagnostics.Debug.WriteLine(this.metaData.ContentPath);
            System.Diagnostics.Debug.WriteLine(GetNameOfFile(this.metaData.ContentPath) + ".xml");

            System.Diagnostics.Debug.WriteLine("XML Parsing");
            string noteText = noteContent.ToXML().ToString();
            System.Diagnostics.Debug.WriteLine("GetPath");
            string noteDir = GlobalSettings.GetNotesPath();
            System.Diagnostics.Debug.WriteLine("GetName");
            string filename = GetNameOfFile(this.metaData.ContentPath);

            System.Diagnostics.Debug.WriteLine("Save content file");
            //Create or Save Content file
            dataManager.SaveText(noteText, noteDir, filename);

            System.Diagnostics.Debug.WriteLine("Save meta");
            //Update Meta Data File
            this.SaveMetaData();

            System.Diagnostics.Debug.WriteLine("SaveProxyMetaAndContent ended");
        }
        private string GetNameOfFile(string path)
        {
            string[] splitpath = path.Split('\\', '/');
            System.Diagnostics.Debug.WriteLine("Name of file");
            return splitpath[splitpath.Length - 1];
        }

    }
}
