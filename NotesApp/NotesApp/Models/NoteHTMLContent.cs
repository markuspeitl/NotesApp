using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public class NoteHTMLContent : NoteContent
    {
        public SimpleHtmlNode contents;
        public CSSStyleManager contentStyle;

        public static string noteContentFormat = ".html";
        public static string noteStyleFormat = ".css";

        public override XElement ToXML()
        {
            return XDocument.Parse(contents.ToString()).Root;
        }

        public void SetNoteContent(SimpleHtmlNode contents)
        {
            this.contents = contents;
        }
        public void SetNoteContentStyle(CSSStyleManager contentStyle)
        {
            this.contentStyle = contentStyle;
        }

        public SimpleHtmlNode GetBodyContent()
        {
            return GetNodeOfTag(this.contents, "body");
        }
        public SimpleHtmlNode GetHeadContent()
        {
            return GetNodeOfTag(this.contents, "head");
        }

        private SimpleHtmlNode GetNodeOfTag(SimpleHtmlNode rootNode, string tag)
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

        public override void FromXML(XElement toUnpack)
        {
            throw new NotImplementedException();
        }

        public override string GetPlainText()
        {
            return contents.GetInnerText();
        }

        public override List<TextSectionObject> GetStyleSections()
        {
            throw new NotImplementedException();
        }

        public override void UpdatePlainText(string newText)
        {
            throw new NotImplementedException();
        }

        public override void UpdateStyleSections(List<TextSectionObject> updatedSections)
        {
            throw new NotImplementedException();
        }
    }
}
