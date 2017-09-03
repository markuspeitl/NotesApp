using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public class BColorTextSection : TextSectionObject
    {
        public string color;

        public BColorTextSection(string color)
        {
            this.color = color;
        }

        public override TextSectionObject Clone()
        {
            BColorTextSection section = new BColorTextSection(color);
            section.sectionEnd = this.sectionEnd;
            section.sectionStart = this.sectionStart;
            return section;
        }

        public override void FromXML(XElement toUnpack)
        {
            XElement element = toUnpack.Element("BColorTextSection");
            if (toUnpack.Name.LocalName.Equals("BColorTextSection"))
                element = toUnpack;
            if (element != null)
            {
                Int32.TryParse(element.Element("Start").Value, out this.sectionStart);
                Int32.TryParse(element.Element("End").Value, out this.sectionEnd);
                this.color = element.Element("Color").Value;
            }
        }

        public override XElement ToXML()
        {
            XElement element = new XElement("BColorTextSection",
                new XElement("Start", sectionStart),
                new XElement("End", sectionEnd),
                new XElement("Color", color)
                );
            return element;
        }
    }
}
