using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using static NotesApp.TextStyle;

namespace NotesApp
{
    public static class TextStyleCSSProvider
    {

        public static void ApplyCSSValuesToTextStyle(TextStyle textStyle, CSSStyling cssStyler)
        {
            if (cssStyler.tagStyles.ContainsKey("color"))
            {
                textStyle.color = cssStyler.tagStyles["color"];
            }
            if (cssStyler.tagStyles.ContainsKey("background-color"))
            {
                textStyle.backcolor = cssStyler.tagStyles["background-color"];
            }
            if (cssStyler.tagStyles.ContainsKey("font-weight"))
            {
                if (cssStyler.tagStyles["font-weight"].Equals("bold"))
                    textStyle.isBold = true;
            }
            if (cssStyler.tagStyles.ContainsKey("font-style"))
            {
                if (cssStyler.tagStyles["font-style"].Equals("italic"))
                    textStyle.isItalic = true;
            }
            if (cssStyler.tagStyles.ContainsKey("text-decoration"))
            {
                if (cssStyler.tagStyles["text-decoration"].Equals("line-through"))
                    textStyle.isStrikedOut = true;
            }
            if (cssStyler.tagStyles.ContainsKey("text-decoration"))
            {
                if (cssStyler.tagStyles["text-decoration"].Equals("underline"))
                    textStyle.isUnderlined = true;
            }
            if (cssStyler.tagStyles.ContainsKey("font-size"))
            {
                textStyle.fontsize = GetSize(cssStyler.tagStyles["font-size"]);
            }
            if (cssStyler.tagStyles.ContainsKey("font-family"))
            {
                textStyle.fontfamily = cssStyler.tagStyles["font-family"];
            }
        }

        private static SizeAble GetSize(string size)
        {
            SizeAble sizeAble = new SizeAble();
            if (size[size.Length - 1] == 'p')
            {
                sizeAble.inDP = true;
                while (!Char.IsNumber(size[size.Length - 1]))
                {
                    size = size.Remove(size.Length - 1);
                }
            }
            else if (size[size.Length - 1] == 'x')
            {
                sizeAble.inPx = true;
                while (!Char.IsNumber(size[size.Length - 1]))
                {
                    size = size.Remove(size.Length - 1);
                }
            }
            Int32.TryParse(size, out sizeAble.size);

            return sizeAble;
        }
    }
}