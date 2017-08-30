using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public interface ITextFormatter
    {
        void SetStyle(string newStyle, int textStart, int textEnd);

    }
}
