using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public class MarginTextSection : TextSectionObject
    {
        public int marginTop = -1;
        public int marginBottom = -1;
        public int marginLeft = -1;
        public int marginRight = -1;

        public MarginTextSection(int margin)
        {
            this.SetUp(margin, margin, margin, margin);
        }

        public MarginTextSection(int marginTop, int marginBottom, int marginLeft, int marginRight)
        {
            this.SetUp(marginTop, marginBottom, marginLeft, marginRight);
        }

        private void SetUp(int marginTop, int marginBottom, int marginLeft, int marginRight)
        {
            this.marginTop = marginTop;
            this.marginBottom = marginBottom;
            this.marginLeft = marginLeft;
            this.marginRight = marginRight;
        }

        public override TextSectionObject Clone()
        {
            MarginTextSection section = new MarginTextSection(marginTop, marginBottom, marginLeft, marginRight);
            section.sectionEnd = this.sectionEnd;
            section.sectionStart = this.sectionStart;
            return section;
        }

        public override XElement ToXML()
        {
            return null;

            //throw new NotImplementedException();
        }

        public override void FromXML(XElement toUnpack)
        {
            throw new NotImplementedException();
        }
    }
}
