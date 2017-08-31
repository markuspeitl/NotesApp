using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class CSSStyling
    {
        public Dictionary<string,string> tagStyles;
        public String toStyleTag;
        private bool isClass = false;
        private bool isID = false;

        public CSSStyling(String toStyleTag, Dictionary<string, string> tagStyles, bool isClass, bool isID)
        {
            this.tagStyles = tagStyles;
            this.toStyleTag = toStyleTag;

            this.isClass = isClass;
            this.isID = isID;
        }

    }
}
