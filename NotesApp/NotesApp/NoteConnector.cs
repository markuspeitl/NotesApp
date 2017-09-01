using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class NoteConnector
    {
        public Note insideNote;

        public NoteConnector(Note note)
        {
            this.insideNote = note;
        }

        public void SetNoteContent(SimpleHtmlNode contents)
        {
            this.insideNote.contents = contents;
        }
        public void SetNoteContentStyle(CSSStyleManager contentStyle)
        {
            this.insideNote.contentStyle = contentStyle;
        }


        public SimpleHtmlNode GetBodyContent()
        {
            return GetNodeOfTag(insideNote.contents, "body");
        }
        public SimpleHtmlNode GetHeadContent()
        {
            return GetNodeOfTag(insideNote.contents, "head");
        }

        private SimpleHtmlNode GetNodeOfTag(SimpleHtmlNode rootNode,string tag)
        {
            foreach (SimpleHtmlNode node in rootNode.GetChildNodes())
            {
                if (node.tag.Equals(tag))
                {
                    return node;
                }
            }
            foreach (SimpleHtmlNode node in rootNode.GetChildNodes())
            {
                SimpleHtmlNode bodyNode = GetNodeOfTag(node, tag);
                if (bodyNode != null)
                {
                    return bodyNode;
                }
            }
            return null;
        }


    }
}
