using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
