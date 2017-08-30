using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public interface IEditText
    {
        void SetupFullText(List<SimpleTextSectionNode> sections);
        void UpdateDirtySections(List<SimpleTextSectionNode> section);

        void SetStyle(string style);
        string GetText();
        string SetText();
        void StyleSection(int charStart, int charEnd, string style);

        /*public enum Style
        {

        }*/
    }
}
