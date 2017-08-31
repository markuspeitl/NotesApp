using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class Note
    {
        //public static string noteRootPath = "/data/data/NotesApp.Droid/files/Notes";
        public static string noteRootPath = "/sdcard/Notes/";
        public static string noteContentFormat = ".html";
        public static string noteStyleFormat = ".css";

        public string title;
        public string date;
        public string wordcount;
        public string path;
        public SimpleHtmlNode contents;
        public CSSStyleManager contentStyle;

        public Note() { }

        public Note(string title, string date)
        {
            this.title = title;
            this.date = date;
        }
    }
}
