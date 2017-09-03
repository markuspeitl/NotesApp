using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public abstract class NoteContent
    {
        public NoteContent() { }

        public abstract String GetPlainText();
        public abstract List<TextSectionObject> GetStyleSections();

        public abstract void UpdatePlainText(string newText);
        public abstract void UpdateStyleSections(List<TextSectionObject> updatedSections);

        public abstract XElement ToXML();
        public abstract void FromXML(XElement toUnpack);

        public virtual void FromXML(String toUnpack)
        {
            XElement el = XDocument.Parse(toUnpack).Root;
            this.FromXML(el);
        }
    }
}
