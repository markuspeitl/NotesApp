using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public class FontFamilyTextSection : TextSectionObject
    {
        public string fontfamily;

        public FontFamilyTextSection(string fontfamily)
        {
            this.fontfamily = fontfamily;
        }
        public override TextSectionObject Clone()
        {
            FontFamilyTextSection section = new FontFamilyTextSection(fontfamily);
            section.sectionEnd = this.sectionEnd;
            section.sectionStart = this.sectionStart;
            return section;
        }

        public override void FromXML(XElement toUnpack)
        {
            XElement element = toUnpack.Element("FontFamilyTextSection");
            if (element != null)
            {
                Int32.TryParse(element.Element("Start").Value, out this.sectionStart);
                Int32.TryParse(element.Element("End").Value, out this.sectionStart);
                this.fontfamily = element.Element("FontFamily").Value;
            }
        }

        public override XElement ToXML()
        {
            XElement element = new XElement("FontFamilyTextSection",
                new XElement("Start", sectionStart),
                new XElement("End", sectionStart),
                new XElement("FontFamily", fontfamily)
                );
            return element;
        }
    }
}
