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
                foreach (SimpleHtmlNode simpleNode in fromNode.GetChildNodes())
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

        public RichText GetRichText()
        {
            return this.GetRichText(rootNode);
        }

        public RichText GetRichText(SimpleHtmlNode fromNode)
        {
            RichText richText = new RichText();

            List<SimpleHtmlNode.TextStyleSection> nodeStyleSections = new List<SimpleHtmlNode.TextStyleSection>();
            nodeStyleSections = fromNode.GetStylingSections();
            richText.baseText = fromNode.GetInnerText();
            richText.formattedText = this.GetExtendedText();
            foreach (SimpleHtmlNode.TextStyleSection nodeStyle in nodeStyleSections)
            {
                foreach(TextSectionObject o in SetupSectionList(nodeStyle.nodeStyle))
                {
                    o.sectionStart = nodeStyle.sectionStart;
                    o.sectionEnd = nodeStyle.sectionEnd;
                    richText.sections.Add(o);
                }
            }

            return richText;
        }

        private List<TextSectionObject> SetupSectionList(TextStyle newSectionStyle)
        {
            List<TextSectionObject> toInsertSection = new List<TextSectionObject>();

            if (newSectionStyle.size != -1)
            {
                SizeTextSection aSpan = new SizeTextSection(newSectionStyle.size);
                toInsertSection.Add(aSpan);
            }
            if (newSectionStyle.isUnderlined)
            {
                UnderLineTextSection uSpan = new UnderLineTextSection();
                toInsertSection.Add(uSpan);
            }
            if (!newSectionStyle.fontfamily.Equals(""))
            {
                FontFamilyTextSection tSpan = new FontFamilyTextSection(newSectionStyle.fontfamily);
                toInsertSection.Add(tSpan);
            }
            if (!newSectionStyle.backcolor.Equals(""))
            {
                BColorTextSection bSpan = new BColorTextSection(newSectionStyle.backcolor);
                toInsertSection.Add(bSpan);
            }
            if (!newSectionStyle.color.Equals(""))
            {
                FColorTextSection fSpan = new FColorTextSection(newSectionStyle.color);
                toInsertSection.Add(fSpan);
            }
            if (newSectionStyle.isStrikedOut)
            {
                StrikeOutTextSection strikeSpan = new StrikeOutTextSection();
                toInsertSection.Add(strikeSpan);
            }
            if (newSectionStyle.isBold || newSectionStyle.isItalic)
            {
                StyleTextSection styleSpan = new StyleTextSection(newSectionStyle.isBold, newSectionStyle.isItalic);
                toInsertSection.Add(styleSpan);
            }

            if (newSectionStyle is TextExtendedStyle)
            {
                if (((TextExtendedStyle)newSectionStyle).setMargin)
                {
                    TextExtendedStyle marginStyle = ((TextExtendedStyle)newSectionStyle);

                    MarginTextSection styleSpan = new MarginTextSection(marginStyle.marginTop, marginStyle.marginBottom,
                        marginStyle.marginLeft, marginStyle.marginRight);

                    toInsertSection.Add(styleSpan);
                }
                //Always insert BreaklineSections last
                if (((TextExtendedStyle)newSectionStyle).lineBreak)
                {
                    BreakLineTextSection styleSpan = new BreakLineTextSection();
                    toInsertSection.Add(styleSpan);
                    //Linebreak must be a point or is inserted after TextExtendedStyle element -> has no overlappings
                    styleSpan.sectionEnd = styleSpan.sectionStart;
                }
            }

            return toInsertSection;
        }

        public class RichText
        {
            public string baseText;
            public string formattedText;
            public List<TextSectionObject> sections;
            public RichText()
            {
                sections = new List<TextSectionObject>();
            }
        }


    }
}
