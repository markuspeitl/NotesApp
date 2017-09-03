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

        private Dictionary<Type,List<TextSectionObject>> transferSections;

        public NoteDisplayer(NoteConnector noteToDisplay, ATextField displayTextField)
        {
            this.noteToDisplay = noteToDisplay;
            this.displayTextField = displayTextField;

            transferSections = new Dictionary<Type, List<TextSectionObject>>();
            sectionManager = new TextSectionManager(displayTextField);
            
            DisplayNote(this.noteToDisplay);
        }

        private const string defaultforground = "black";
        private const string defaultbackground = "white";
        private void SetupDisplayerColors()
        {
            string forgroundColor = defaultforground;
            string backgoundColor = defaultbackground;

            if (noteToDisplay.insideNote.contentStyle != null)
            {
                CSSStyling styleOptions = noteToDisplay.insideNote.contentStyle.GetStyle("body");

                if (styleOptions != null)
                {
                    if (styleOptions.tagStyles.ContainsKey("color"))
                    {
                        forgroundColor = styleOptions.tagStyles["color"];
                    }
                    if (styleOptions.tagStyles.ContainsKey("background-color"))
                    {
                        backgoundColor = styleOptions.tagStyles["background-color"];
                    }
                }
            }

            displayTextField.SetDefaultColors(forgroundColor, backgoundColor);

            displayTextField.TextInsertedEvent += TextWasInserted;
            displayTextField.TextRemovedEvent += TextWasRemoved;
        }

        private void TextWasRemoved(object sender, ATextField.TextChangeEventArgs e)
        {
            sectionManager.RemoveCharactersUpdateMetrics(e.startPosition, e.endPosition - e.startPosition);
        }

        private void TextWasInserted(object sender, ATextField.TextChangeEventArgs e)
        {
            sectionManager.InsertCharactersUpdateMetrics(e.startPosition, e.endPosition - e.startPosition);
        }

        private SimpleNodeInterpreter nodeInterperter;
        public void DisplayNote(NoteConnector noteToDisplay)
        {
            nodeInterperter = new SimpleNodeInterpreter(noteToDisplay.insideNote.contents);

            bodyNode = noteToDisplay.GetBodyContent();
            headNode = noteToDisplay.GetHeadContent();

            SimpleNodeInterpreter.RichText richText = nodeInterperter.GetRichText();
            displayTextField.SetText(richText.baseText);
            InsertAllSections(richText.sections);
            SendSectionsToEditText();

            //displayTextField.SetText(nodeInterperter.GetExtendedText());


            //sectionManager.GenerateHtml();

            //displayTextField.SetText(bodyNode.GetInnerText());
            //displayTextField.SetText(noteToDisplay.insideNote.contents);

            SetupDisplayerColors();

            displayTextField.IsSetUp(true);
        }

        public void SaveCurrentNote(ISaveAndLoad dataManager)
        {
            this.noteToDisplay.insideNote.plainText = displayTextField.GetPlainText();
            this.noteToDisplay.insideNote.styleSections = sectionManager.GetElementsList();
            string noteRepresentation = this.noteToDisplay.insideNote.ToXML().ToString();
            dataManager.SaveText(noteRepresentation,"/sdcard/Notes/", "thisTextNote.xml");
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
            return toInsertSection;
        }

        private void InsertAllSections(List<TextSectionObject> insertSections, int start, int end)
        {
            foreach (TextSectionObject o in insertSections)
            {
                o.sectionStart = start;
                o.sectionEnd = end;
            }
            InsertAllSections(insertSections);
        }

        private void InsertAllSections(List<TextSectionObject> insertSections)
        {
            foreach (TextSectionObject o in insertSections)
            {
                if (o.GetType().Equals(typeof(FontFamilyTextSection)) || o.GetType().Equals(typeof(SizeTextSection)))
                {
                    InsertSectionOfType(o, o.sectionStart, o.sectionEnd, o.GetType(), true);
                }
                else
                {
                    InsertSectionOfType(o, o.sectionStart, o.sectionEnd, o.GetType(), false);
                }
            }
        }


        private void SendSectionsToEditText()
        {
            displayTextField.InsertTextSections(transferSections);
            transferSections = new Dictionary<Type, List<TextSectionObject>>();
        }
        private void PutItemIntoDictionary(TextSectionObject section)
        {
            if (!transferSections.ContainsKey(section.GetType()))
            {
                transferSections[section.GetType()] = new List<TextSectionObject>();
            }

            transferSections[section.GetType()].Add(section);
        }

        private void InsertSectionOfType(TextSectionObject section, int start, int end, Type spanType, bool overwrite)
        {
            List<TextSectionObject> overlappingSections = sectionManager.GetOverLappingSections(start,end,section.GetType());

            if (overlappingSections == null)
            {
                overlappingSections = new List<TextSectionObject>();
            }
            
            //Three cases for overlapping spanSections
            if (overlappingSections.Count == 0)
            {
                sectionManager.InsertTextSection(section);
                PutItemIntoDictionary(section);
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
                    PutItemIntoDictionary(otherSection);
                }
                if (end != oldEnd)
                {
                    TextSectionObject otherClone = otherSection.Clone();
                    otherClone.sectionStart = end;
                    otherClone.sectionEnd = oldEnd;
                    sectionManager.InsertTextSection(otherClone);
                    PutItemIntoDictionary(otherClone);
                }
                if (overwrite)
                {
                    sectionManager.InsertTextSection(section);
                    PutItemIntoDictionary(section);
                }
            }
            else
            {
                int[] minmax = GetMinMaxPos(overlappingSections, start, end);
                RemoveAll(overlappingSections);
                section.sectionStart = minmax[0];
                section.sectionEnd = minmax[1];
                sectionManager.InsertTextSection(section);
                PutItemIntoDictionary(section);
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
