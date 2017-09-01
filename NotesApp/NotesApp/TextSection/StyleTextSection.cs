using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public class StyleTextSection : TextSectionObject
    {
        public bool isItalic = false;
        public bool isBold = false;

        public StyleTextSection(bool isBold, bool isItalic)
        {
            this.isItalic = isItalic;
            this.isBold = isBold;
        }

        public override TextSectionObject Clone()
        {
            StyleTextSection section = new StyleTextSection(isBold, isItalic);
            section.sectionEnd = this.sectionEnd;
            section.sectionStart = this.sectionStart;
            return section;
        }

        public override void FromXML(XElement toUnpack)
        {
            XElement element = toUnpack.Element("StyleTextSection");
            if (element != null)
            {
                Int32.TryParse(element.Element("Start").Value, out this.sectionStart);
                Int32.TryParse(element.Element("End").Value, out this.sectionStart);
                Boolean.TryParse(element.Element("IsItalic").Value, out this.isItalic);
                Boolean.TryParse(element.Element("IsBold").Value, out this.isBold);
            }
        }

        public override XElement ToXML()
        {
            XElement element = new XElement("StyleTextSection",
                new XElement("Start", sectionStart),
                new XElement("End", sectionStart),
                new XElement("IsItalic", isItalic),
                new XElement("IsBold", isBold)
                );
            return element;
        }
    }
}
