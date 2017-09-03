using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public class NoteXMLContent : NoteContent
    {
        public string plainText = "";
        public List<TextSectionObject> styleSections;

        public static string noteContentFormat = ".css";

        public override XElement ToXML()
        {
            XElement element = new XElement("NoteContent");

            //element.Add(new XElement("Title", metaData.title));
            //element.Add(new XElement("Date", metaData.createdDate));
            element.Add(new XElement("PlainText", plainText));

            if (styleSections != null)
            {
                foreach (TextSectionObject section in styleSections)
                {
                    element.Add(section.ToXML());
                }
            }

            return element;
        }

        public override void FromXML(XElement toUnpack)
        {
            XElement element;

            if (toUnpack.Name.LocalName.Equals("NoteContent"))
            {
                element = toUnpack;
            }
            else
            {
                element = toUnpack.Element("NoteContent");
            }

            styleSections = new List<TextSectionObject>();

            //this.title = element.Element("Title").Value;
            //this.date = element.Element("Date").Value;
            this.plainText = element.Element("PlainText").Value;

            IEnumerable<XNode> decendants = element.Elements();
            foreach (XElement el in decendants)
            {
                TextSectionObject toInsert = null;
                if (el.Name.LocalName.Equals("BColorTextSection"))
                {
                    toInsert = new BColorTextSection("");
                }
                else if (el.Name.LocalName.Equals("BreakLineTextSection"))
                {
                    toInsert = new BreakLineTextSection();
                }
                else if (el.Name.LocalName.Equals("FColorTextSection"))
                {
                    toInsert = new FColorTextSection("");
                }
                else if (el.Name.LocalName.Equals("FontFamilyTextSection"))
                {
                    toInsert = new FontFamilyTextSection("");
                }
                else if (el.Name.LocalName.Equals("SizeTextSection"))
                {
                    toInsert = new SizeTextSection(0);
                }
                else if (el.Name.LocalName.Equals("StrikeOutTextSection"))
                {
                    toInsert = new StrikeOutTextSection();
                }
                else if (el.Name.LocalName.Equals("StyleTextSection"))
                {
                    toInsert = new StyleTextSection(false, false);
                }
                else if (el.Name.LocalName.Equals("UnderLineTextSection"))
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

        public override string GetPlainText()
        {
            return plainText;
        }

        public override List<TextSectionObject> GetStyleSections()
        {
            return styleSections;
        }

        public override void UpdatePlainText(string newText)
        {
            plainText = newText;
        }

        public override void UpdateStyleSections(List<TextSectionObject> updatedSections)
        {
            styleSections = updatedSections;
        }
    }
}
