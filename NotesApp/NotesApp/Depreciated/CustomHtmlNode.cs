using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class CustomHtmlNode : HtmlNode
    {
        private const string textName = "#text";
        private const string heaingFirstChar = "h";
        private const string paragraphChar = "p";

        public CustomHtmlNode(HtmlNodeType type, HtmlDocument ownerdocument, int index) : base(type, ownerdocument, index)
        {
        }

        public string GetText()
        {
            string text = "";

            if (this.Name.Equals(textName))
            {
                return this.InnerHtml;
            }
            else if (HasLineBreak(textName))
            {
                return this.InnerHtml + "\n";
            }
            else
            {
                foreach(CustomHtmlNode childNode in this.ChildNodes)
                {
                    text += childNode.GetText();
                }
            }
            return text;
        }

        private bool HasLineBreak(String nodeName)
        {
            if(nodeName != null)
            {
                if (nodeName.Substring(0, 1).Equals(heaingFirstChar) || nodeName.Equals(paragraphChar))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
