//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NotesApp
//{
//    public class SimpleNodeAccessor
//    {
//        private const string textName = "#text";
//        private const string heaingFirstChar = "h";
//        private const string paragraphChar = "p";

//        SimpleHtmlNode toAccessNode;
//        List<SimpleTextSectionNode> textSections;

//        public SimpleNodeAccessor(SimpleHtmlNode toAccessNode)
//        {
//            this.toAccessNode = toAccessNode;
//            textSections = new List<SimpleTextSectionNode>();
//            SetUpBodyTextSections();
//        }

//        private void SetUpBodyTextSections()
//        {
//            this.textPos = 0;
//            SetupTextSections(toAccessNode);
//        }
//        private int textPos = 0;
//        private void SetupTextSections(SimpleHtmlNode currentNode)
//        {
//            if (currentNode.IsText)
//            {
//                textSections.Add(new SimpleTextSectionNode(currentNode, textPos, textPos + currentNode.GetInnerText().Length));
//                textPos = textPos + currentNode.GetInnerString().Length;
//            }
//            else
//            {
//                foreach (SimpleHtmlNode child in currentNode.GetChildNodes())
//                {
//                    SetupTextSections(child);
//                }
//            }
//        }

//        public void InsertCharacter(int position, string insertString)
//        {
//            foreach (SimpleTextSectionNode extNode in textSections)
//            {
//                if (position >= extNode.textStart)
//                {
//                    if (position < extNode.textEnd)
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
//            SimpleHtmlNode childNode;
//            childNode = new SimpleHtmlNode(style, null);

//            foreach (SimpleTextSectionNode extNode in textSections)
//            {
//                //leftopen cut section
//                if (startPosition < extNode.textEnd && startPosition >= extNode.textStart)
//                {
//                    extNode.SetPartialLeftNewMasterNode(style, startPosition, endPosition);
//                }
//                //rightopen cut section
//                else if (endPosition > extNode.textStart && endPosition < extNode.textEnd)
//                {
//                    extNode.SetPartialRightNewMasterNode(style, startPosition, endPosition);
//                }
//                //crossing section no cut
//                else if (endPosition > extNode.textEnd && startPosition < extNode.textStart)
//                {
//                    extNode.SetNewMasterNode(style);
//                }
//                //in other child no cut (create 3 children)
//            }
//        }

//    }
//}
