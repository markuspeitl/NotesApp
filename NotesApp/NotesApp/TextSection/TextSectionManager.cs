using NotesApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesApp
{
    public class TextSectionManager : TextSectionObject
    {

        private Dictionary<Type, List<TextSectionObject>> insertedTextSections;
        private SortedSet<TextSectionObject> allSectionsSorted;


        //public List<TextSectionObject> insertedTextSections;

        private ATextField displayTextField;

        public TextSectionManager(ATextField displayTextField)
        {
            this.displayTextField = displayTextField;
            //insertedTextSections = new List<TextSectionObject>();
            insertedTextSections = new Dictionary<Type, List<TextSectionObject>>();
            allSectionsSorted = new SortedSet<TextSectionObject>();
        }

        public void InsertTextSection(TextSectionObject insertSection)
        {
            if (!insertedTextSections.ContainsKey(insertSection.GetType()))
            {
                insertedTextSections.Add(insertSection.GetType(), new List<TextSectionObject>());
            }
            insertedTextSections[insertSection.GetType()].Add(insertSection);
            allSectionsSorted.Add(insertSection);
            //insertedTextSections.Add(insertSection);
        }

        public void RemoveTextSection(TextSectionObject removeSection)
        {
            if (insertedTextSections.ContainsKey(removeSection.GetType()))
            {
                insertedTextSections[removeSection.GetType()].Remove(removeSection);
                allSectionsSorted.Remove(removeSection);
            }
        }

        public void InsertCharactersUpdateMetrics(int start, int lenght)
        {
            foreach(TextSectionObject section in allSectionsSorted)
            {
                /*if(section.sectionEnd < start)
                {
                    do nothing
                }
                if(section.sectionEnd > start && section.sectionStart < start)
                {
                    between increase end
                }
                if(section.sectionStart > start)
                {
                    
                }*/
                if (section.sectionStart > start)
                {
                    section.sectionStart = section.sectionStart + lenght;
                    section.sectionEnd = section.sectionEnd + lenght;
                }
                else if (section.sectionEnd > start)
                {
                    section.sectionEnd = section.sectionEnd + lenght;
                }
            }
        }
        public void RemoveCharactersUpdateMetrics(int start, int lenght)
        {
            foreach (TextSectionObject section in allSectionsSorted)
            {
                if (section.sectionStart > start)
                {
                    section.sectionStart = section.sectionStart - lenght;
                    section.sectionEnd = section.sectionEnd - lenght;
                }
                else if (section.sectionEnd > start)
                {
                    section.sectionEnd = section.sectionEnd - lenght;
                }
            }
        }

        public List<TextSectionObject> GetOverLappingSections(int start, int end,Type textSectionType)
        {
            List<TextSectionObject> overlapping = new List<TextSectionObject>();
            if (insertedTextSections.ContainsKey(textSectionType))
            {
                foreach(TextSectionObject toCheck in insertedTextSections[textSectionType])
                {
                    if (Overlap(start, end, toCheck))
                    {
                        overlapping.Add(toCheck);
                    }
                }

                return overlapping;
            }
            return null;
        }

        private bool Overlap(int start,int end, TextSectionObject second)
        {
            if(start > second.GetSectionEnd() || second.GetSectionStart() > end)
            {
                return false;
            }
            return true;
        }

        public override TextSectionObject Clone()
        {
            throw new NotImplementedException();
        }

        /*private SortedSet<TextSectionObject> sortedSet = new SortedSet<TextSectionObject>();
        public SimpleHtmlNode GenerateHtml()
        {
            SimpleHtmlNode rootNode = new SimpleHtmlNode("html",null);

            SimpleHtmlNode currentNode = new SimpleHtmlNode("head", null);
            rootNode.AddChild(currentNode);

            currentNode = new SimpleHtmlNode("body", null);
            rootNode.AddChild(currentNode);

            SimpleHtmlNode lastNode = new SimpleHtmlNode("p", null);
            currentNode.AddChild(lastNode);
            currentNode = lastNode;


            foreach (Type t in insertedTextSections.Keys)
            {
                foreach(TextSectionObject section in insertedTextSections[t])
                {
                    sortedSet.Add(section);
                }
            }

            SimpleHtmlNode superNode = currentNode;
            TextSectionObject superBucket = sortedSet.ElementAt(0);

            foreach (TextSectionObject section in sortedSet)
            {
                if(section != superBucket)
                {
                    if (Inside(section.sectionStart, section.sectionEnd, superBucket))
                    {

                        
                    }
                    else if (Overlap(section.sectionStart, section.sectionEnd, superBucket))
                    {

                    }
                }
            }
            return rootNode;
        }*/
        
        public List<TextSectionObject> GetElementsList()
        {
            List<TextSectionObject> allElements = new List<TextSectionObject>();
            foreach (Type t in insertedTextSections.Keys)
            {
                foreach (TextSectionObject section in insertedTextSections[t])
                {
                    allElements.Add(section);
                }
            }

            return allElements;
        }

        private string GetTextSectionTag(TextSectionObject toGet)
        {
            return toGet.ToString();
        }

        private bool Inside(int start, int end, TextSectionObject second)
        {
            if (start > second.GetSectionStart() && second.GetSectionEnd() > end)
            {
                return true;
            }
            return false;
        }

        public override void FromXML(XElement toUnpack)
        {
            XElement element = toUnpack.Element("TextSectionManager");

            foreach(XElement el in element.DescendantNodes())
            {
                TextSectionObject toInsert = null;
                if (el.Name.Equals("BColorTextSection"))
                {
                    toInsert = new BColorTextSection("");
                }
                else if (el.Name.Equals("BreakLineTextSection"))
                {
                    toInsert = new BreakLineTextSection();
                }
                else if (el.Name.Equals("FColorTextSection"))
                {
                    toInsert = new FColorTextSection("");
                }
                else if (el.Name.Equals("FontFamilyTextSection"))
                {
                    toInsert = new FontFamilyTextSection("");
                }
                else if (el.Name.Equals("SizeTextSection"))
                {
                    toInsert = new SizeTextSection(0);
                }
                else if (el.Name.Equals("StrikeOutTextSection"))
                {
                    toInsert = new StrikeOutTextSection();
                }
                else if (el.Name.Equals("StyleTextSection"))
                {
                    toInsert = new StyleTextSection(false,false);
                }
                else if (el.Name.Equals("UnderLineTextSection"))
                {
                    toInsert = new UnderLineTextSection();
                }

                if (toInsert != null)
                {
                    toInsert.FromXML(el);
                    this.InsertTextSection(toInsert);
                }
            }
        }

        public override XElement ToXML()
        {
            XElement element = new XElement("TextSectionManager");

            foreach (Type t in insertedTextSections.Keys)
            {
                foreach (TextSectionObject section in insertedTextSections[t])
                {
                    element.Add(section.ToXML());
                }
            }
            
            return element;
        }

    }
}
