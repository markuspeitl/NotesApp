using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class TextSection
    {
        private bool hasChildText;

        int charStart;
        int charEnd;
        private string text;
        //private Style style; (textsize, font, color, etc)


        //private string beforeText;
        //private string afterText;
        //private TextSection child;

        private bool style;

        private bool hasStyle;
        private TextSection parent;
        private List<TextSection> children;

        public bool GetStyle()
        {
            if (!hasStyle)
            {
                return parent.GetStyle();
            }
            else
            {
                return style;
            }
        }

        public void AddChild(TextSection child)
        {
            if(children == null)
            {
                children = new List<TextSection>();
            }
            children.Add(child);
        }

        public void SetText(string text)
        {
            this.text = text;
        }

        private string GetChildrenText()
        {
            string fulltext = "";
            foreach(TextSection ts in children)
            {
                fulltext += ts.GetPlainText();
            }
            return fulltext;
        }

        public void SplitSection(int charStart, int charEnd)
        {
            if(ThisIsMe(charStart, charEnd))
            {
                int position = parent.children.IndexOf(this);
                //parent.children.RemoveAt(position);

                string beforetext = this.text.Substring(0,charStart - this.charStart);
                string middletext = this.text.Substring(charStart - this.charStart, charEnd - this.charEnd);
                string aftertext = this.text.Substring(charEnd - this.charEnd, this.text.Length);

                //this copy
                TextSection before = this;
                before.SetText(beforetext);

                TextSection middle = new TextSection();
                //middle.SetStyle()
                middle.SetText(middletext);

                TextSection after = new TextSection();
                after.SetText(aftertext);

                parent.children.Insert(position, before);
                parent.children.Insert(position + 1, middle);
                parent.children.Insert(position + 2, after);
            }
            else
            {
                foreach(TextSection ts in children)
                {
                    ts.SplitSection(charStart, charEnd);
                }
            }
        }

        private bool ThisIsMe(int charStart, int charEnd)
        {
            return true;
        }
        
        public string GetPlainText()
        {
            if (children == null)
            {
                return GetChildrenText();
            }
            return text;
        }
        public string GetFormattedText()
        {
            if (children == null)
            {
                return "<p "+ GetStyleString() + ">" + GetChildrenText() + "</p>";
            }
            return "<div " + GetStyleString() + ">" + text + "</div>";
        }
        private string GetStyleString()
        {
            return "";
        }


    }
}
