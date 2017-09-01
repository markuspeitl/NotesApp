using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class UnderLineTextSection : TextSectionObject
    {
        public override TextSectionObject Clone()
        {
            UnderLineTextSection section = new UnderLineTextSection();
            section.sectionEnd = this.sectionEnd;
            section.sectionStart = this.sectionStart;
            return section;
        }
    }
}
