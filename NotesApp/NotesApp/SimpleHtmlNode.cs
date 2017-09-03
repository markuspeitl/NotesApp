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

        private List<SimpleHtmlNode> ChildNodes;
        private SimpleHtmlNode parent;
        public SimpleHtmlNode Parent { get { return parent; }}
        public string tag;
        private string inheritanceTag = "";
        public bool wasClosed = false;

        private String content;
        public String Content { get { return content; } set { IsText = true; content = value; } }

        public Dictionary<string,SimpleHtmlAttribute> attributes;

        private TextStyle nodeStyle;
        private CSSStyleManager cssStyler;
        public CSSStyling htmlCssStyle;

        public int textSectionStart = 0;
        public int textSectionEnd = 0;

        public SimpleHtmlNode(string tag, CSSStyling htmlCssStyle, CSSStyleManager cssStyler)
        {
            this.tag = tag;
            this.ChildNodes = new List<SimpleHtmlNode>();
            this.attributes = new Dictionary<string, SimpleHtmlAttribute>();
            this.cssStyler = cssStyler;
            this.htmlCssStyle = htmlCssStyle;
            this.nodeStyle = TextStyleHTMLProvider.GetTagStyle(tag);
        }

        private void SetParent(SimpleHtmlNode parent)
        {
            inheritanceTag = parent.inheritanceTag + " " +this.tag;
            this.parent = parent;
            UpdateNodeStyling();
        }


        private void UpdateNodeStyling()
        {
            //Reset NodeStyle before inheriting new Style from parent
            nodeStyle = TextStyleHTMLProvider.GetTagStyle(tag);

            //If nodeStyle is null it is not supported and wont be set
            if (nodeStyle != null)
            {
                //Inherit Parent Styling when setting new Parent
                if (this.parent != null)
                {

                    if (this.parent.nodeStyle != null)
                        nodeStyle.Inherit(this.parent.nodeStyle);
                }

                if (cssStyler != null)
                {
                    CSSStyling ccsStyle = null;
                    if (cssStyler.HasStyle(inheritanceTag))
                    {
                        ccsStyle = cssStyler.GetStyle(inheritanceTag);
                    }
                    else if (cssStyler.HasStyle(tag))
                    {
                        ccsStyle = cssStyler.GetStyle(tag);
                    }

                    if (ccsStyle != null)
                    {
                        //OverWrite Relevant Attributes
                        TextStyleCSSProvider.ApplyCSSValuesToTextStyle(nodeStyle, ccsStyle);
                    }
                }
                if (htmlCssStyle != null)
                {
                    //OverWrite Relevant Attributes (inline style attribute)
                    TextStyleCSSProvider.ApplyCSSValuesToTextStyle(nodeStyle, htmlCssStyle);
                }
            }
        }
        public List<SimpleHtmlNode> GetChildNodes()
        {
            return ChildNodes;
        }

        public void UpdateNodeStylingBranch()
        {
            this.UpdateNodeStyling();
            foreach (SimpleHtmlNode simpleNode in ChildNodes)
            {
                simpleNode.UpdateNodeStylingBranch();
            }
        }
        
        private int GetStart()
        {
            return this.textSectionStart;
        }
        private int GetEnd()
        {
            if (!IsText)
            {
                return this.textSectionEnd;
            }
            else
            {
                return content.Length;
            }
        }

        //Tranverses Tree Brach recursively in reverse and update span ends
        private void UpdateNodeEnds(int addToEnd)
        {
            this.textSectionEnd = this.textSectionEnd + addToEnd;
            if(this.parent != null)
            {
                this.parent.UpdateNodeEnds(addToEnd);
            }
        }

        public void AddChild(SimpleHtmlNode child)
        {
            this.ChildNodes.Add(child);
            child.SetParent(this);

            //Update Parent endings to what was added (shift to right by insertion lenght)
            UpdateNodeEnds(child.GetEnd());

            child.textSectionStart = this.GetStart() + (this.GetEnd() - this.GetStart());
            child.textSectionEnd = child.textSectionStart + child.GetEnd();
        }

        public List<TextStyleSection> GetStylingSections()
        {
            List<TextStyleSection> sections = new List<TextStyleSection>();
            if (!IsText)
            {
                //Do not add section if node styling not supported for this node
                if(nodeStyle != null)
                    sections.Add(new TextStyleSection(nodeStyle, textSectionStart, textSectionEnd));

                foreach (SimpleHtmlNode simpleNode in ChildNodes)
                {
                    sections.AddRange(simpleNode.GetStylingSections());
                }
            }
            return sections;
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
                foreach (KeyValuePair<string,SimpleHtmlAttribute> s in attributes)
                {
                    text += s.Value;
                }
            }

            return text;
        }

        public class TextStyleSection
        {
            public TextStyle nodeStyle;
            public int sectionStart;
            public int sectionEnd;
            public TextStyleSection(TextStyle nodeStyle, int sectionStart,int sectionEnd)
            {
                this.nodeStyle = nodeStyle;
                this.sectionStart = sectionStart;
                this.sectionEnd = sectionEnd;
            }
        }

    }
}
