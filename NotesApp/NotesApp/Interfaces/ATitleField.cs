using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Interfaces
{
    public abstract class ATitleField
    {
        public abstract void SetText(string text);
        public abstract void SetDefaultColors(string foregroundColor, string backgroundColor);
        public abstract void ClearText();
        public abstract string GetPlainText();

    }
}
