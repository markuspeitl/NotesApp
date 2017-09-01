using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class TextExtendedStyle : TextStyle
    {

        //Check inheritance of margins needed
        public bool setMargin = false;
        public int marginRight = -1;
        public int marginLeft = -1;
        public int marginTop = -1;
        public int marginBottom = -1;
        public bool lineBreak = false;
    }
}
