using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public abstract class TextSectionObject : ITextSectionObject
    {
        public int sectionStart;
        public int sectionEnd;

        public int GetSectionStart()
        {
            return sectionStart;
        }
        public int GetSectionEnd()
        {
            return sectionEnd;
        }

        public abstract TextSectionObject Clone();
    }
}
