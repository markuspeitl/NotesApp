using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class TextStyle
    {
        //public int size = -1;
        public SizeAble fontsize = new SizeAble() { size = -1 };
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
            if (parent.fontsize.size != -1 && this.fontsize.size != -1)
            {
                this.fontsize = parent.fontsize;
            }
        }

        public struct SizeAble
        {
            public int size;
            public bool inPx;
            public bool inDP;
        }
    }
}
