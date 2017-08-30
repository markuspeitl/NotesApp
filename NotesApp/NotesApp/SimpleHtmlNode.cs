using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class SimpleHtmlNode
    {

        public bool IsText = false;

        public List<SimpleHtmlNode> ChildNodes;
        public SimpleHtmlNode Parent;
        public string tag;
        public bool wasClosed = false;

        private String content;
        public String Content { get { return content; } set { IsText = true; content = value; } }

        public List<SimpleHtmlAttribute> attributes;

        public SimpleHtmlNode(string tag, SimpleHtmlNode parent)
        {
            this.tag = tag;
            ChildNodes = new List<SimpleHtmlNode>();
            attributes = new List<SimpleHtmlAttribute>();
        }

        public string GetOuterString()
        {
            string text = "";

            if (!IsText)
            {
                text += "<" + this.tag + GetAttributeString() + ">";
                foreach (SimpleHtmlNode simpleNode in ChildNodes)
                {
                    text += simpleNode.ToString();
                }
                text += "</" + this.tag + ">";
            }
            else
            {
                return Content;
            }

            return text;
        }

        //Text between Tags
        public string GetInnerString()
        {
            string text = "";

            if (!IsText)
            {
                foreach (SimpleHtmlNode simpleNode in ChildNodes)
                {
                    text += simpleNode.ToString();
                }
            }
            else
            {
                return Content;
            }

            return text;
        }

        //Text nodes only
        public string GetInnerText()
        {
            string text = "";

            if (!IsText)
            {
                foreach (SimpleHtmlNode simpleNode in ChildNodes)
                {
                    text += simpleNode.GetInnerText();
                }
            }
            else
            {
                return Content;
            }

            return text;
        }

        public override String ToString()
        {
            return GetOuterString();
        }

        private string GetAttributeString()
        {
            string text = "";
            if (attributes.Count > 0)
            {
                text = " ";
                foreach (SimpleHtmlAttribute s in attributes)
                {
                    text += s;
                }
            }

            return text;
        }

    }
}
