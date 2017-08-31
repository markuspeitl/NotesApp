using NotesApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class NoteDisplayer
    {
        private ATextField displayTextField;
        private NoteConnector noteToDisplay;
        private SimpleHtmlNode bodyNode;
        private SimpleHtmlNode headNode;

        private TextSectionManager sectionManager;

        private List<TextSectionObject> transferSections;

        public NoteDisplayer(NoteConnector noteToDisplay, ATextField displayTextField)
        {
            this.noteToDisplay = noteToDisplay;
            this.displayTextField = displayTextField;

            transferSections = new List<TextSectionObject>();
            sectionManager = new TextSectionManager(displayTextField);
            
            DisplayNote(this.noteToDisplay);
        }

        private const string defaultforground = "black";
        private const string defaultbackground = "white";
        private void SetupDisplayerColors()
        {
            CSSStyling styleOptions = noteToDisplay.insideNote.contentStyle.GetStyle("body");
            string forgroundColor = defaultforground;
            string backgoundColor = defaultbackground;

            if (styleOptions != null)
            {
                if(styleOptions.tagStyles.ContainsKey("color"))
                {
                    forgroundColor = styleOptions.tagStyles["color"];
                }
                if (styleOptions.tagStyles.ContainsKey("background-color"))
                {
                    backgoundColor = styleOptions.tagStyles["background-color"];
                }
            }
            displayTextField.SetDefaultColors(forgroundColor, backgoundColor);

        }

        public void DisplayNote(NoteConnector noteToDisplay)
        {
            bodyNode = noteToDisplay.GetBodyContent();
            headNode = noteToDisplay.GetHeadContent();
            displayTextField.SetText(bodyNode.GetInnerText());
            //displayTextField.SetText(noteToDisplay.insideNote.contents);

            SetupDisplayerColors();
        }

        public void ExecuteStyleChange(TextStyle newSectionStyle)
        {
            //displayTextField.SetStyleToSelection(newSectionStyle);

            List<TextSectionObject> todo = SetupSectionList(newSectionStyle);
            int[] startEnd = displayTextField.GetSelectionStartEnd();
            InsertAllSections(todo, startEnd[0], startEnd[1]);
            SendSectionsToEditText();
        }

        //private const string defaultFont = "Times New Roman";
        //private const int defaultSize = 20;
        //private string defaultColor = "white";
        //private string defaultBackColor = "blue";
        //private string defaultFrontColor = "red";
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
            return toInsertSection;
        }

        private void InsertAllSections(List<TextSectionObject> insertSections, int start, int end)
        {
            foreach (TextSectionObject o in insertSections)
            {
                o.sectionStart = start;
                o.sectionEnd = end;

                if (o.GetType().Equals(typeof(FontFamilyTextSection)) || o.GetType().Equals(typeof(SizeTextSection)))
                {
                    InsertSectionOfType(o, start, end, o.GetType(), true);
                }
                else
                {
                    InsertSectionOfType(o, start, end, o.GetType(), false);
                }
            }
        }
        
        private void SendSectionsToEditText()
        {
            displayTextField.InsertTextSections(transferSections);
            transferSections = new List<TextSectionObject>();
        }

        private void InsertSectionOfType(TextSectionObject section, int start, int end, Type spanType, bool overwrite)
        {
            List<TextSectionObject> overlappingSections = sectionManager.GetOverLappingSections(start,end,section.GetType());

            if (overlappingSections.Count == 0)
            {
                sectionManager.InsertTextSection(section);
                transferSections.Add(section);
            }
            else if (overlappingSections.Count == 1 && IsInside(start, end, overlappingSections.ElementAt(0)))
            {
                TextSectionObject otherSection = overlappingSections.ElementAt(0);

                int oldStart = otherSection.sectionStart;
                int oldEnd = otherSection.sectionEnd;
                //Remove old span
                sectionManager.RemoveTextSection(otherSection);
                if (oldStart != start)
                {
                    otherSection.sectionStart = oldStart;
                    otherSection.sectionEnd = start;
                    sectionManager.InsertTextSection(otherSection);
                    transferSections.Add(otherSection);
                }
                if (end != oldEnd)
                {
                    TextSectionObject otherClone = otherSection.Clone();
                    otherClone.sectionStart = end;
                    otherClone.sectionEnd = oldEnd;
                    sectionManager.InsertTextSection(otherClone);
                    transferSections.Add(otherClone);
                }
                if (overwrite)
                {
                    sectionManager.InsertTextSection(section);
                    transferSections.Add(section);
                }
            }
            else
            {
                int[] minmax = GetMinMaxPos(overlappingSections, start, end);
                RemoveAll(overlappingSections);
                section.sectionStart = minmax[0];
                section.sectionEnd = minmax[1];
                sectionManager.InsertTextSection(section);
                transferSections.Add(section);
            }
        }

        private void RemoveAll(IEnumerable<TextSectionObject> sections)
        {
            foreach (TextSectionObject obj in sections)
            {
                sectionManager.RemoveTextSection(obj);
            }
        }
        private bool IsInside(int newStart, int newEnd, TextSectionObject otherSection)
        {
            if (newStart >= otherSection.GetSectionStart() && newEnd <= otherSection.GetSectionEnd())
            {
                return true;
            }
            return false;
        }
        private int[] GetMinMaxPos(IEnumerable<TextSectionObject> sections, int currentStart, int currentEnd)
        {
            int[] minmax = new int[2];
            minmax[0] = currentStart;
            minmax[1] = currentEnd;

            foreach (TextSectionObject o in sections)
            {
                if (o.GetSectionStart() < minmax[0])
                {
                    minmax[0] = o.GetSectionStart();
                }
                if (o.GetSectionEnd() > minmax[1])
                {
                    minmax[1] = o.GetSectionEnd();
                }
            }
            return minmax;
        }

    }
}
