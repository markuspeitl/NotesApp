using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
