using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
