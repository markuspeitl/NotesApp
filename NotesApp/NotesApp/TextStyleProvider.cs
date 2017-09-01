using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class TextStyleProvider
    {

        public static TextExtendedStyle GetTagStyle(string tag)
        {
            switch (tag)
            {
                case "p":
                    return GetParagraphStyle();
                case "u":
                    return GetUnderLinedStyle();
                case "i":
                    return GetItalicStyle();
                case "b":
                    return GetBoldStyle();
                case "s":
                    return GetStrikeThroughStyle();
                case "br":
                    return GetLineBreakStyle();
                case "h1":
                    return GetHeader1Style();
                case "h2":
                    return GetHeader2Style();
                case "h3":
                    return GetHeader3Style();
                case "h4":
                    return GetHeader4Style();
                case "h5":
                    return GetHeader5Style();
                case "h6":
                    return GetHeader6Style();
                default:
                    return null;
            }
        }

        private static TextExtendedStyle GetUnderLinedStyle()
        {
            return new TextExtendedStyle() { isUnderlined = true };
        }

        private static TextExtendedStyle GetItalicStyle()
        {
            return new TextExtendedStyle() { isItalic = true };
        }

        private static TextExtendedStyle GetBoldStyle()
        {
            return new TextExtendedStyle() { isBold = true };
        }

        private static TextExtendedStyle GetStrikeThroughStyle()
        {
            return new TextExtendedStyle() { isStrikedOut = true };
        }

        
        private static TextExtendedStyle GetLineBreakStyle()
        {
            TextExtendedStyle style = new TextExtendedStyle();
            return new TextExtendedStyle() { lineBreak = true };
        }

        //Text Format Styles
        private static TextExtendedStyle GetParagraphStyle()
        {
            TextExtendedStyle style = new TextExtendedStyle();
            return new TextExtendedStyle() { marginBottom = 10, marginTop = 10, lineBreak = true, setMargin = true };
        }
        
        private static TextExtendedStyle GetHeader1Style()
        {
            return new TextExtendedStyle() { marginBottom = 20, marginTop = 20, lineBreak = true, size = 50, setMargin = true };
        }
        private static TextExtendedStyle GetHeader2Style()
        {
            return new TextExtendedStyle() { marginBottom = 16, marginTop = 16, lineBreak = true, size = 40, setMargin = true };
        }
        private static TextExtendedStyle GetHeader3Style()
        {
            return new TextExtendedStyle() { marginBottom = 14, marginTop = 14, lineBreak = true, size = 32, setMargin = true };
        }
        private static TextExtendedStyle GetHeader4Style()
        {
            return new TextExtendedStyle() { marginBottom = 12, marginTop = 12, lineBreak = true, size = 25, setMargin = true };
        }
        private static TextExtendedStyle GetHeader5Style()
        {
            return new TextExtendedStyle() { marginBottom = 10, marginTop = 10, lineBreak = true, size = 20, setMargin = true };
        }
        private static TextExtendedStyle GetHeader6Style()
        {
            return new TextExtendedStyle() { marginBottom = 10, marginTop = 10, lineBreak = true, size = 15, setMargin = true };
        }

    }
}
