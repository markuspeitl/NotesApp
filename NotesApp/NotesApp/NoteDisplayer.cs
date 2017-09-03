using NotesApp.Interfaces;
using NotesApp.SingleTons;
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
        private ATitleField displayTitleField;
        private NoteProxy noteToDisplay;

        private TextSectionManager sectionManager;

        private Dictionary<Type,List<TextSectionObject>> transferSections;

        public NoteDisplayer(ATextField displayTextField, ATitleField displayTitleField)
        {
            this.displayTextField = displayTextField;
            this.displayTitleField = displayTitleField;

            transferSections = new Dictionary<Type, List<TextSectionObject>>();
            sectionManager = new TextSectionManager(displayTextField);
        }

        private const string defaultforground = "black";
        private const string defaultbackground = "white";
        private async void SetupDisplayerColors()
        {
            System.Diagnostics.Debug.WriteLine("SetupDisplayerColors Started");
            string forgroundColor = defaultforground;
            string backgoundColor = defaultbackground;

            NoteContent displayNote = noteToDisplay.GetContent();

            if (displayNote is NoteHTMLContent)
            {
                NoteHTMLContent htmlNodeContent = (NoteHTMLContent)displayNote;
                if (htmlNodeContent.contentStyle != null)
                {
                    CSSStyling styleOptions = htmlNodeContent.contentStyle.GetStyle("body");
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
            }

            if (!noteToDisplay.GetMetaData().backgroundColor.Equals(""))
            {
                backgoundColor = noteToDisplay.GetMetaData().backgroundColor;
            }
            if (!noteToDisplay.GetMetaData().frontColor.Equals(""))
            {
                forgroundColor = noteToDisplay.GetMetaData().frontColor;
            }

            displayTextField.SetDefaultColors(forgroundColor, backgoundColor);
            displayTitleField.SetDefaultColors("black","white");

            displayTextField.TextInsertedEvent += TextWasInserted;
            displayTextField.TextRemovedEvent += TextWasRemoved;

            System.Diagnostics.Debug.WriteLine("SetupDisplayerColors Ended");
        }

        public async Task<NoteProxy> GetFormattedNote()
        {
            System.Diagnostics.Debug.WriteLine("GetFormattedNote Started");
            NoteContent displayNote = noteToDisplay.GetContent();
            displayNote.UpdatePlainText(this.displayTextField.GetPlainText());

            noteToDisplay.GetMetaData().title = displayTitleField.GetPlainText();

            displayNote.UpdateStyleSections(this.sectionManager.GetElementsList());
            return this.noteToDisplay;
            System.Diagnostics.Debug.WriteLine("GetFormattedNote Ended");
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
        public async void DisplayNote(NoteProxy noteProxyToDisplay)
        {
            System.Diagnostics.Debug.WriteLine("DisplayNote Started");

            this.noteToDisplay = noteProxyToDisplay;

            NoteContent displayNote = noteProxyToDisplay.GetContent();

            /*nodeInterperter = new SimpleNodeInterpreter(noteToDisplay.insideNote.contents);
            bodyNode = noteToDisplay.GetBodyContent();
            headNode = noteToDisplay.GetHeadContent();
            SimpleNodeInterpreter.RichText richText = nodeInterperter.GetRichText();
            displayTextField.SetText(richText.baseText);
            InsertAllSections(richText.sections);
            SendSectionsToEditText();*/
            displayTextField.SetText(displayNote.GetPlainText());
            displayTitleField.SetText(noteProxyToDisplay.GetMetaData().title);
            InsertAllSections(displayNote.GetStyleSections());
            SendSectionsToEditText();

            SetupDisplayerColors();

            displayTextField.IsSetUp(true);

            System.Diagnostics.Debug.WriteLine("DisplayNote Ended");
        }
        
        public void ExecuteStyleChange(TextStyle newSectionStyle)
        {
            //displayTextField.SetStyleToSelection(newSectionStyle);
            System.Diagnostics.Debug.WriteLine("ExecuteStyleChange Started");
            List<TextSectionObject> todo = TextStyleTextSectionConverter.SetupSectionList(newSectionStyle);
            int[] startEnd = displayTextField.GetSelectionStartEnd();
            InsertAllSections(todo, startEnd[0], startEnd[1]);
            SendSectionsToEditText();

            System.Diagnostics.Debug.WriteLine("ExecuteStyleChange Ended");
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
            if (insertSections != null)
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
