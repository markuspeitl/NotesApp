using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class SimpleNodeInterpreter
    {
        private const string paragraphTag = "p";
        private const string headingTag = "h1";


        private SimpleHtmlNode rootNode;
        public SimpleNodeInterpreter(SimpleHtmlNode rootNode)
        {
            this.rootNode = rootNode;

            currentNode = rootNode;
        }

        public string GetExtendedText()
        {
            return GetExtendedText(rootNode);
        }

        private SimpleHtmlNode currentNode;
        public string GetExtendedText(SimpleHtmlNode fromNode)
        {
            string text = "";

            if (!fromNode.IsText)
            {
                if (fromNode.tag.Equals("br"))
                {
                    text += "\n";
                }
                foreach (SimpleHtmlNode simpleNode in fromNode.ChildNodes)
                {
                    text += GetExtendedText(simpleNode);
                }

                if(fromNode.tag.Equals(paragraphTag) || fromNode.tag.Equals(headingTag))
                {
                    text += "\n";
                }
            }
            else
            {
                return fromNode.Content;
            }
            return text;
        }

        public RichText GetRichText(SimpleHtmlNode fromNode)
        {
            return null;
        }

        public class RichText
        {
            public string baseText;
            public string formattedText;
            public List<TextSectionObject> sections;
        }


    }
}
