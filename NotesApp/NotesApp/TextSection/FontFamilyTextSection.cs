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

    }
}
