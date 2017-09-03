using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public class SizeTextSection : TextSectionObject
    {
        public int Size;

        public SizeTextSection(int Size)
        {
            this.Size = Size;
        }
        public override TextSectionObject Clone()
        {
            SizeTextSection section = new SizeTextSection(Size);
            section.sectionEnd = this.sectionEnd;
            section.sectionStart = this.sectionStart;
            return section;
        }

        public override void FromXML(XElement toUnpack)
        {
            XElement element = toUnpack.Element("SizeTextSection");
            if (toUnpack.Name.LocalName.Equals("SizeTextSection"))
                element = toUnpack;

            if (element != null)
            {
                Int32.TryParse(element.Element("Start").Value, out this.sectionStart);
                Int32.TryParse(element.Element("End").Value, out this.sectionEnd);
                Int32.TryParse(element.Element("Size").Value, out this.Size);
            }
        }

        public override XElement ToXML()
        {
            XElement element = new XElement("SizeTextSection",
                new XElement("Start", sectionStart),
                new XElement("End", sectionEnd),
                new XElement("Size", Size)
                );
            return element;
        }
    }
}
