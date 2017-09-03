using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.SingleTons
{
    public static class TextStyleTextSectionConverter
    {
        //private const string defaultFont = "Times New Roman";
        //private const int defaultSize = 20;
        //private string defaultColor = "white";
        //private string defaultBackColor = "blue";
        //private string defaultFrontColor = "red";
        public static List<TextSectionObject> SetupSectionList(TextStyle newSectionStyle)
        {
            List<TextSectionObject> toInsertSection = new List<TextSectionObject>();

            if (newSectionStyle.fontsize.size != -1)
            {
                SizeTextSection aSpan = new SizeTextSection(newSectionStyle.fontsize.size);
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

    }
}
