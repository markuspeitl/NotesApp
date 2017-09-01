using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
