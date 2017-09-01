using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public string plainText;
        public List<TextSectionObject> styleSections;

        public Note() { }

        public Note(string title, string date)
        {
            this.title = title;
            this.date = date;
        }

        public XElement ToXML()
        {
            XElement element = new XElement("Note");

            element.Add(new XElement("Title", title));
            element.Add(new XElement("Date", date));
            element.Add(new XElement("PlainText", plainText));

            foreach (TextSectionObject section in styleSections)
            {
                element.Add(section.ToXML());
            }

            return element;
        }

        public void FromXML(XElement toUnpack)
        {
            XElement element = toUnpack.Element("Note");

            styleSections = new List<TextSectionObject>();

            this.title = element.Element("Title").Value;
            this.date = element.Element("Date").Value;
            this.plainText = element.Element("PlainText").Value;

            foreach (XElement el in element.DescendantNodes())
            {
                TextSectionObject toInsert = null;
                if (el.Name.Equals("BColorTextSection"))
                {
                    toInsert = new BColorTextSection("");
                }
                else if (el.Name.Equals("BreakLineTextSection"))
                {
                    toInsert = new BreakLineTextSection();
                }
                else if (el.Name.Equals("FColorTextSection"))
                {
                    toInsert = new FColorTextSection("");
                }
                else if (el.Name.Equals("FontFamilyTextSection"))
                {
                    toInsert = new FontFamilyTextSection("");
                }
                else if (el.Name.Equals("SizeTextSection"))
                {
                    toInsert = new SizeTextSection(0);
                }
                else if (el.Name.Equals("StrikeOutTextSection"))
                {
                    toInsert = new StrikeOutTextSection();
                }
                else if (el.Name.Equals("StyleTextSection"))
                {
                    toInsert = new StyleTextSection(false, false);
                }
                else if (el.Name.Equals("UnderLineTextSection"))
                {
                    toInsert = new UnderLineTextSection();
                }

                if (toInsert != null)
                {
                    toInsert.FromXML(el);
                    styleSections.Add(toInsert);
                }
            }
        }
    }
}
