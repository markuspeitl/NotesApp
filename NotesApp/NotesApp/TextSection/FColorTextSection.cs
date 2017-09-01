using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public class FColorTextSection : TextSectionObject
    {
        public string color;

        public FColorTextSection(string color)
        {
            this.color = color;
        }

        public override TextSectionObject Clone()
        {
            FColorTextSection section = new FColorTextSection(color);
            section.sectionEnd = this.sectionEnd;
            section.sectionStart = this.sectionStart;
            return section;
        }

        public override void FromXML(XElement toUnpack)
        {
            XElement element = toUnpack.Element("FColorTextSection");
            if (element != null)
            {
                Int32.TryParse(element.Element("Start").Value, out this.sectionStart);
                Int32.TryParse(element.Element("End").Value, out this.sectionStart);
                this.color = element.Element("Color").Value;
            }
        }

        public override XElement ToXML()
        {
            XElement element = new XElement("FColorTextSection",
                new XElement("Start", sectionStart),
                new XElement("End", sectionStart),
                new XElement("Color", color)
                );
            return element;
        }
    }
}
