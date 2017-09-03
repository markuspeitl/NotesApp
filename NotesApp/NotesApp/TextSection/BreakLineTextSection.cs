using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public class BreakLineTextSection : TextSectionObject
    {
        public override TextSectionObject Clone()
        {
            BreakLineTextSection section = new BreakLineTextSection();
            section.sectionEnd = this.sectionEnd;
            section.sectionStart = this.sectionStart;
            return section;
        }

        public override void FromXML(XElement toUnpack)
        {
            XElement element = toUnpack.Element("BreakLineTextSection");
            if (toUnpack.Name.LocalName.Equals("BreakLineTextSection"))
                element = toUnpack;
            if (element != null)
            {
                Int32.TryParse(element.Element("Start").Value, out this.sectionEnd);
                Int32.TryParse(element.Element("End").Value, out this.sectionEnd);
            }
        }

        public override XElement ToXML()
        {
            XElement element = new XElement("BreakLineTextSection",
                new XElement("Start", sectionEnd),
                new XElement("End", sectionEnd)
                );
            return element;
        }
    }
}
