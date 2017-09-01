using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class FontFamilyTextSection : TextSectionObject
    {
        public string fontfamily;

        public FontFamilyTextSection(string fontfamily)
        {
            this.fontfamily = fontfamily;
        }
        public override TextSectionObject Clone()
        {
            FontFamilyTextSection section = new FontFamilyTextSection(fontfamily);
            section.sectionEnd = this.sectionEnd;
            section.sectionStart = this.sectionStart;
            return section;
        }
    }
}
