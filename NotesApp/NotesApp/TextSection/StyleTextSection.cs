using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class StyleTextSection : TextSectionObject
    {
        public bool isItalic = false;
        public bool isBold = false;

        public StyleTextSection(bool isBold, bool isItalic)
        {
            this.isItalic = isItalic;
            this.isBold = isBold;
        }
    }
}
