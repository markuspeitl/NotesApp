using NotesApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class TextSectionManager : TextSectionObject
    {

        private Dictionary<Type, List<TextSectionObject>> insertedTextSections;

        //public List<TextSectionObject> insertedTextSections;

        private ATextField displayTextField;

        public TextSectionManager(ATextField displayTextField)
        {
            this.displayTextField = displayTextField;
            //insertedTextSections = new List<TextSectionObject>();
            insertedTextSections = new Dictionary<Type, List<TextSectionObject>>();
        }

        public void InsertTextSection(TextSectionObject insertSection)
        {
            if (!insertedTextSections.ContainsKey(insertSection.GetType()))
            {
                insertedTextSections.Add(insertSection.GetType(), new List<TextSectionObject>());
            }
            insertedTextSections[insertSection.GetType()].Add(insertSection);
            //insertedTextSections.Add(insertSection);
        }

        public void RemoveTextSection(TextSectionObject removeSection)
        {
            if (insertedTextSections.ContainsKey(removeSection.GetType()))
            {
                insertedTextSections[removeSection.GetType()].Remove(removeSection);
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
    }
}
