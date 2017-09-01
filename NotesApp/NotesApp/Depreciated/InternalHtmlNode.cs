//using HtmlAgilityPack;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NotesApp
//{
//    public class InternalHtmlNode
//    {

//        private const string textName = "#text";
//        private const string heaingFirstChar = "h";
//        private const string paragraphChar = "p";

//        public List<InternalHtmlNode> ChildNodes;
//        public bool HasChildNodes = false;

//        public String Name;
//        public String InnerHtml;
//        public String OuterHtml;

//        public int OuterStartIndex;
//        public int InnerStartIndex;

//        public InternalHtmlNode(HtmlNode toCopy)
//        {
//            this.InnerHtml = toCopy.InnerHtml;
//            this.OuterHtml = toCopy.OuterHtml;
//            this.Name = toCopy.Name;

//            this.InnerStartIndex = toCopy.LinePosition;
//            this.OuterStartIndex = InnerStartIndex - OuterHtml.IndexOf('>');

//            if (toCopy.HasChildNodes)
//            {
//                if (ChildNodes == null)
//                {
//                    ChildNodes = new List<InternalHtmlNode>();
//                    HasChildNodes = true;
//                }

//                foreach (HtmlNode child in toCopy.ChildNodes)
//                {
//                    ChildNodes.Add(new InternalHtmlNode(child));
//                }
//            }
//        }
        
//        public String InnerText()
//        {
//            if (this.Name == textName)
//                return InnerHtml;

//            /*if (_nodetype == HtmlNodeType.Comment)
//                return ((HtmlCommentNode)this).Comment;*/

//            // note: right now, this method is *slow*, because we recompute everything.
//            // it could be optimized like innerhtml
//            if (!HasChildNodes)
//                return string.Empty;

//            string s = null;
//            foreach (InternalHtmlNode node in ChildNodes)
//                s += node.InnerText();
//            return s;
//        }

//    }
//}
