//using HtmlAgilityPack;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;

//namespace NotesApp
//{
//    public class XMLText
//    {
//        //private TextSection textRepresentation;

//        // private List<Styles> styles

//        public void SaveToXML()
//        {
//            //XElement rootText = XElement.Parse(textRepresentation.GetFormattedText());
//        }

//        public void LoadFromXML(ISaveAndLoad dataManager)
//        {
//            /*XElement rootElement = XElement.Parse(textRepresentation.GetFormattedText());
//            IEnumerable<XAttribute> attributes = rootElement.Attributes();
//            //Parse Attributes and create root
//            TextSection root = new TextSection();
//            RestoreTextSectionTree(rootElement, root);*/

//            HtmlDocument rootDocument = new HtmlDocument();

//            string path = "/sdcard/test1.html";

//            Stream htmlStream = dataManager.GetStreamFromPath(path);

//            CustomHtmlParser node = new CustomHtmlParser();
//            SimpleHtmlNode rootNode = node.ParseXML(htmlStream);
//            //node.ParseXML(htmlStream);


//            SimpleHtmlNode bodyNode = GetBodyNode(rootNode);

//            if (bodyNode != null)
//            {
//                //CustomHtmlNode customNode = (CustomHtmlNode)bodyNode;
//                //customNode.AddCustomBehaviourChildren();

//                //string fullText = customNode.GetText();

//                SimpleNodeAccessor nodeAccessor = new SimpleNodeAccessor(bodyNode);

//                string fullText = rootNode.GetInnerString();

//                string altText = rootNode.GetOuterString();

//                nodeAccessor.ChangeStyle(9, 40, "bold");
//            }

//            /*if (htmlStream != null)
//            {
//                rootDocument.Load(htmlStream);

//                HtmlNode bodyNode = GetBodyNode(rootDocument);

//                if (bodyNode != null)
//                {
//                    //CustomHtmlNode customNode = (CustomHtmlNode)bodyNode;
//                    //customNode.AddCustomBehaviourChildren();

//                    //string fullText = customNode.GetText();

//                    HtmlNodeAccessor nodeAccessor = new HtmlNodeAccessor(bodyNode);

//                    string fullText = nodeAccessor.GetText();

//                    string altText = nodeAccessor.GetTextAlt();

//                    nodeAccessor.ChangeStyle(20, 140, "bold");
//                }
//            }*/
//        }

//        private HtmlNode GetBodyNode(HtmlDocument rootDocument)
//        {
//            foreach(HtmlNode node in rootDocument.DocumentNode.ChildNodes)
//            {
//                HtmlNode bodyNode = GetBodyNode(node);
//                if (bodyNode != null)
//                {
//                    return bodyNode;
//                }
//            }
//            return null;
//        }

//        private HtmlNode GetBodyNode(HtmlNode rootNode)
//        {
//            foreach(HtmlNode node in rootNode.ChildNodes)
//            {
//                if (node.Name.Equals("body"))
//                {
//                    return node;
//                }
//            }
//            foreach (HtmlNode node in rootNode.ChildNodes)
//            {
//                HtmlNode bodyNode = GetBodyNode(node);
//                if(bodyNode != null)
//                {
//                    return bodyNode;
//                }
//            }

//            return null;
//        }

//        private SimpleHtmlNode GetBodyNode(SimpleHtmlNode rootNode)
//        {
//            foreach (SimpleHtmlNode node in rootNode.ChildNodes)
//            {
//                if (node.tag.Equals("body"))
//                {
//                    return node;
//                }
//            }
//            foreach (SimpleHtmlNode node in rootNode.ChildNodes)
//            {
//                SimpleHtmlNode bodyNode = GetBodyNode(node);
//                if (bodyNode != null)
//                {
//                    return bodyNode;
//                }
//            }

//            return null;
//        }

//        /*private void RestoreTextSectionTree(XElement currentElement, TextSection currentSection)
//        {
//            IEnumerable<XAttribute> attributes;

//            foreach (XElement child in currentElement.Descendants())
//            {
//                //child.Value
//                attributes = child.Attributes();
//                //Parse Attributes and create child
//                TextSection textchild = new TextSection();
//                textchild.SetText(child.Value);

//                currentSection.AddChild(textchild);

//                RestoreTextSectionTree(child, textchild);
//            }
//        }*/

//    }
//}
