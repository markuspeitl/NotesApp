using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class SimpleHtmlAttribute
    {
        public string Name;
        public string Value;

        private bool parenthesis = false;

        public SimpleHtmlAttribute(string representation)
        {
            string[] splitstring = representation.Split('=');

            Name = splitstring[0];

            if (splitstring[1].Contains("\""))
            {
                parenthesis = true;
            }

            Value = splitstring[1].Replace("\"","");
        }

        public override string ToString()
        {
            if (!parenthesis)
            {
                return Name + "=" + Value;
            }
            return Name + "=" +"\""+ Value + "\"";
        }

    }
}
