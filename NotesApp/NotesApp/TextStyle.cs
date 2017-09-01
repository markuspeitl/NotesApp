using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class TextStyle
    {
        public int size = -1;
        public string color = "";
        public string backcolor = "";
        public string fontfamily = "";
        public bool isBold;
        public bool isItalic;
        public bool isUnderlined;
        public bool isStrikedOut;

        public void Inherit(TextStyle parent)
        {
            this.isBold = this.isBold || parent.isBold;
            this.isUnderlined = this.isUnderlined || parent.isUnderlined;
            this.isStrikedOut = this.isStrikedOut || parent.isStrikedOut;
            this.isItalic = this.isItalic || parent.isItalic;
            if (!parent.backcolor.Equals("") && this.backcolor.Equals(""))
            {
                this.backcolor = parent.backcolor;
            }
            if (!parent.color.Equals("") && this.color.Equals(""))
            {
                this.color = parent.color;
            }
            if (!parent.fontfamily.Equals("") && this.fontfamily.Equals(""))
            {
                this.fontfamily = parent.fontfamily;
            }
            if (parent.size != -1 && this.size != -1)
            {
                this.size = parent.size;
            }
        }

        public virtual void ApplyCSSStyling(CSSStyling cssStyler)
        {
            if (cssStyler.tagStyles.ContainsKey("color"))
            {
                this.color = cssStyler.tagStyles["color"];
            }
            if (cssStyler.tagStyles.ContainsKey("background-color"))
            {
                this.backcolor = cssStyler.tagStyles["background-color"];
            }
            if (cssStyler.tagStyles.ContainsKey("font-weight"))
            {
                if (cssStyler.tagStyles["font-weight"].Equals("bold"))
                    this.isBold = true;
            }
            if (cssStyler.tagStyles.ContainsKey("font-style"))
            {
                if (cssStyler.tagStyles["font-style"].Equals("italic"))
                    this.isItalic = true;
            }
            if (cssStyler.tagStyles.ContainsKey("text-decoration"))
            {
                if (cssStyler.tagStyles["text-decoration"].Equals("line-through"))
                    this.isStrikedOut = true;
            }
            if (cssStyler.tagStyles.ContainsKey("text-decoration"))
            {
                if (cssStyler.tagStyles["text-decoration"].Equals("underline"))
                    this.isUnderlined = true;
            }
            if (cssStyler.tagStyles.ContainsKey("font-size"))
            {
                int size = -1;
                if (Int32.TryParse(cssStyler.tagStyles["font-size"], out size))
                {
                    this.size = size;
                }  
            }
            if (cssStyler.tagStyles.ContainsKey("font-family"))
            {
                this.fontfamily = cssStyler.tagStyles["font-family"];
            }
        }

    }
}
