//using HtmlAgilityPack;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NotesApp
//{
//    public class HtmlNodeAccessor
//    {
//        private const string textName = "#text";
//        private const string heaingFirstChar = "h";
//        private const string paragraphChar = "p";

//        HtmlNode toAccessNode;
//        List<TextExtendedNode> textSections;

//        public HtmlNodeAccessor(HtmlNode toAccessNode)
//        {
//            this.toAccessNode = toAccessNode;
//            textSections = new List<TextExtendedNode>();
//            SetUpBodyTextSections();
//        }



//        private void SetUpBodyTextSections()
//        {
//            this.textPos = 0;
//            SetupTextSections(toAccessNode);
//        }
//        private int textPos = 0;
//        private void SetupTextSections(HtmlNode currentNode)
//        {
//            if(currentNode.NodeType == HtmlNodeType.Text)
//            {
//                textSections.Add(new TextExtendedNode(currentNode, textPos, textPos + currentNode.InnerText.Length));
//                textPos = textPos + currentNode.InnerText.Length;
//            }
//            else
//            {
//                foreach(HtmlNode child in currentNode.ChildNodes)
//                {
//                    SetupTextSections(child);
//                }
//            }
//        }

//        public void InsertCharacter(int position, string insertString)
//        {
//            foreach (TextExtendedNode extNode in textSections)
//            {
//                if(position >= extNode.textStart)
//                {
//                    if(position < extNode.textEnd)
//                    {
//                        extNode.InsertString(position, insertString);
//                    }
//                    else
//                    {
//                        extNode.UpdateTextPosition(insertString.Length);
//                    }
//                }
//            }
//        }

//        public void ChangeStyle(int startPosition, int endPosition, string style)
//        {
//            HtmlNode masterNode;
//            masterNode = new HtmlNode(HtmlNodeType.Element, toAccessNode.OwnerDocument, 0);
//            masterNode.Name = "bold";

//            HtmlNode childNode;
//            childNode = new HtmlNode(HtmlNodeType.Element, toAccessNode.OwnerDocument, 1);
//            childNode.Name = "bold";

//            foreach (TextExtendedNode extNode in textSections)
//            {
//                //leftopen cut section
//                if (startPosition < extNode.textEnd && startPosition >= extNode.textStart)
//                {

//                }
//                //rightopen cut section
//                else if (endPosition > extNode.textStart && endPosition < extNode.textEnd)
//                {

//                }
//                //crossing section no cut
//                else if (endPosition > extNode.textEnd && startPosition < extNode.textStart)
//                {
//                    extNode.SetNewMasterNode(masterNode);
//                }
//            }
//        }

//        public String GetText()
//        {
//            return this.GetText(toAccessNode);
//        }

//        public String GetTextAlt()
//        {
//            string text = "";

//            foreach(TextExtendedNode extNode in textSections)
//            {
//                text += extNode.thisHtmlTextNode.InnerText;
//            }

//            return text;
//        }

//        public string GetText(HtmlNode fromNode)
//        {
//            string text = "";

//            if (fromNode.Name.Equals(textName))
//            {
//                return fromNode.InnerHtml;
//            }
//            else if (HasLineBreak(textName))
//            {
//                return fromNode.InnerHtml + "\n";
//            }
//            else
//            {
//                foreach (HtmlNode childNode in fromNode.ChildNodes)
//                {
//                    text += this.GetText(childNode);
//                }
//            }
//            return text;
//        }

//        private bool HasLineBreak(String nodeName)
//        {
//            if (nodeName != null)
//            {
//                if (nodeName.Substring(0, 1).Equals(heaingFirstChar) || nodeName.Equals(paragraphChar))
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
//    }
//}
