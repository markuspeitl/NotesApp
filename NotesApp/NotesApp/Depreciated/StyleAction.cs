using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class StyleAction : ITextAction
    {
        private string newTextStyle;
        private string oldTextStyle;

        private int textStart;
        private int textEnd;

        private ITextFormatter formatter;

        public StyleAction(string newStyle, int textStart, int textEnd)
        {
            DoAction();
        }

        public void UndoAction()
        {
            formatter.SetStyle(oldTextStyle, textStart, textEnd);
        }

        public void DoAction()
        {
            formatter.SetStyle(newTextStyle, textStart, textEnd);
        }
    }
}
