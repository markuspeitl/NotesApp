using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{

    [DataContract(Name = "MetaData")]
    public class NoteMeta
    {
        //[DataMember]
        //public string uuid;
        [DataMember()]
        public string title = "";
        [DataMember()]
        public string createdDate = "";
        [DataMember()]
        public string lastModifiedDate = "";
        [DataMember()]
        public string backgroundColor = "";
        [DataMember()]
        public string frontColor = "";
        [DataMember()]
        private string contentPath = "";
        [DataMember()]
        public int wordcount;
        [DataMember]
        public DOCTYPE contentType;
        [DataMember()]
        public string extension = "";

        public string ContentPath
        {
            get
            {
                return contentPath;
            }
            set
            {
                this.contentType = GetExtensionType(value);
                this.contentPath = value;
            }
        }

        public enum DOCTYPE
        {
            NONE,
            HTML,
            XML
        }

        private DOCTYPE GetExtensionType(string path)
        {
            string[] splitPath = path.Split('.');

            if (splitPath[splitPath.Length - 1].Equals("html"))
            {
                return DOCTYPE.HTML;
            }
            else if (splitPath[splitPath.Length - 1].Equals("xml"))
            {
                return DOCTYPE.XML;
            }

            this.extension = splitPath[splitPath.Length - 1];
            return DOCTYPE.NONE;
        }
    }
}
