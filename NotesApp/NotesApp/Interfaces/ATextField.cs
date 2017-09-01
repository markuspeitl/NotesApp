using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Interfaces
{
    public abstract class ATextField
    {
        public event EventHandler<int> CursorPositionChanged;

        public event EventHandler<TextInsertedEventArgs> TextInsertedEvent;
        public event EventHandler<TextRemovedEventArgs> TextRemovedEvent;
        public event EventHandler<StyleChangedEventArgs> StyleChangedEvent;

        public abstract void SetText(string text);
        //public abstract void SetText(SimpleHtmlNode html);
        //public abstract void SetStyleToSection(TextStyle textStyle, int textStart, int textEnd);
        public abstract int[] GetSelectionStartEnd();

        public abstract void InsertTextSections(Dictionary<Type, List<TextSectionObject>> sections);

        public abstract void SetDefaultColors(string foregroundColor, string backgroundColor);

        /*public virtual void SetStyleToSelection(TextStyle textStyle)
        {
            int[] selection = GetSelectionStartEnd();
            SetStyleToSection(textStyle, selection[0], selection[1]);
        }*/

        public class TextInsertedEventArgs
        {
            public int position;
            public string text;
        }
        public class TextRemovedEventArgs
        {
            public int startPosition;
            public int endPosition;
        }

        public class StyleChangedEventArgs
        {
            public int startSpan;
            public int endSpan;
            public string style;
        }
    }
}
