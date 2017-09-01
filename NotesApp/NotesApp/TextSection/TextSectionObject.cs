using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public abstract class TextSectionObject : ITextSectionObject, IComparable
    {
        public int sectionStart;
        public int sectionEnd;

        public int GetSectionStart()
        {
            return sectionStart;
        }
        public int GetSectionEnd()
        {
            return sectionEnd;
        }

        public abstract TextSectionObject Clone();
        public abstract XElement ToXML();
        public abstract void FromXML(XElement toUnpack);

        /*public int CompareTo(object obj)
        {
            if(obj is TextSectionObject)
            {
                return -(this.sectionEnd - this.sectionStart - ((TextSectionObject)obj).sectionEnd + ((TextSectionObject)obj).sectionStart);
            }

            throw new InvalidCastException("obj is no TextSectionObject");
        }*/

        public int CompareTo(object obj)
        {
            if (obj is TextSectionObject)
            {
                return this.sectionEnd - ((TextSectionObject)obj).sectionStart;
            }

            throw new InvalidCastException("obj is no TextSectionObject");
        }
    }
}
