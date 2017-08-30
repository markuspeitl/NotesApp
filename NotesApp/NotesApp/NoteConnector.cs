using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class NoteConnector
    {
        private Note insideNote;

        public NoteConnector(Note note)
        {
            this.insideNote = note;
        }

        public void SetNoteContent(SimpleHtmlNode contents)
        {
            this.insideNote.contents = contents;
        }


        public SimpleHtmlNode GetBodyContent()
        {
            return GetBodyNode(insideNote.contents);
        }
        private SimpleHtmlNode GetBodyNode(SimpleHtmlNode rootNode)
        {
            foreach (SimpleHtmlNode node in rootNode.ChildNodes)
            {
                if (node.tag.Equals("body"))
                {
                    return node;
                }
            }
            foreach (SimpleHtmlNode node in rootNode.ChildNodes)
            {
                SimpleHtmlNode bodyNode = GetBodyNode(node);
                if (bodyNode != null)
                {
                    return bodyNode;
                }
            }
            return null;
        }


    }
}
