using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public class StrikeOutTextSection : TextSectionObject
    {
        public override TextSectionObject Clone()
        {
            StrikeOutTextSection section = new StrikeOutTextSection();
            section.sectionEnd = this.sectionEnd;
            section.sectionStart = this.sectionStart;
            return section;
        }

        public override void FromXML(XElement toUnpack)
        {
            XElement element = toUnpack.Element("StrikeOutTextSection");
            if (toUnpack.Name.LocalName.Equals("StrikeOutTextSection"))
                element = toUnpack;
            if (element != null)
            {
                Int32.TryParse(element.Element("Start").Value, out this.sectionStart);
                Int32.TryParse(element.Element("End").Value, out this.sectionEnd);
            }
        }

        public override XElement ToXML()
        {
            XElement element = new XElement("StrikeOutTextSection",
                new XElement("Start", sectionStart),
                new XElement("End", sectionEnd)
                );
            return element;
        }
    }
}
