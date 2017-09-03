using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NotesApp.Interfaces;
using Android.Text;
using Android.Text.Style;
using Android.Graphics;
using Android.Content.Res;
using NotesApp.Droid.CustomSpans;

namespace NotesApp.Droid
{
    public class EditTextManager : ATextField
    {
        private EditText noteEditText;
        private Context sourceContext;
        SpannableStringBuilder spanRange;

        public EditTextManager(EditText noteEditText, Context sourceContext)
        {
            this.noteEditText = noteEditText;
            this.sourceContext = sourceContext;

            noteEditText.TextChanged += TextChanged;
            noteEditText.BeforeTextChanged += BeforeTextChanged;
        }

        private void BeforeTextChanged(object sender, TextChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            //throw new NotImplementedException();
            if (this.wasSetup)
            {
                if (e.AfterCount < e.BeforeCount)
                {
                    TextChangeEventArgs removeArgs = new TextChangeEventArgs() { startPosition = e.Start, endPosition = e.Start - e.BeforeCount };
                    //spanRange.Replace(e.Start, e.Start + e.BeforeCount, "");
                    spanRange.Replace(e.Start, e.Start + e.BeforeCount - e.AfterCount, "");
                    this.InvokeTextRemovedEvent(removeArgs);
                }
                else if (e.AfterCount > e.BeforeCount)
                {
                    TextChangeEventArgs insertArgs = new TextChangeEventArgs() { startPosition = e.Start, endPosition = e.Start + e.AfterCount };
                    string insertString = ("" + e.Text).Substring(e.Start, e.AfterCount - e.BeforeCount);
                    spanRange.Insert(e.Start, insertString);
                    this.InvokeTextInsertedEvent(insertArgs);
                }
            }
        }

        private void RemoveOverlappingSpans(Type ty, TextSectionObject section)
        {
            Java.Lang.Object[] overlappingSpans = (Java.Lang.Object[])spanRange.GetSpans(section.GetSectionStart(),
                section.GetSectionEnd(), Java.Lang.Class.FromType(ty));
            RemoveAll(overlappingSpans);
        }

        public override void InsertTextSections(Dictionary<Type, List<TextSectionObject>> insertSections)
        {
            if (this.spanRange == null)
            {
                this.spanRange = new SpannableStringBuilder(noteEditText.Text);
            }

            if (insertSections.ContainsKey(typeof(SizeTextSection)))
            {
                foreach (SizeTextSection section in insertSections[typeof(SizeTextSection)])
                {
                    if (section.Size > 0)
                    {
                        RemoveOverlappingSpans(typeof(AbsoluteSizeSpan), section);
                        spanRange.SetSpan(new AbsoluteSizeSpan(section.Size, true),
                            section.GetSectionStart(),
                            section.GetSectionEnd(),
                            SpanTypes.ExclusiveExclusive);
                    }
                }
            }
            if (insertSections.ContainsKey(typeof(BColorTextSection)))
            {
                foreach (BColorTextSection section in insertSections[typeof(BColorTextSection)])
                {
                    if (((BColorTextSection)section).color != "")
                    {
                        Color color = Color.ParseColor(((BColorTextSection)section).color);
                        if (color != null)
                        {
                            RemoveOverlappingSpans(typeof(BackgroundColorSpan), section);
                            spanRange.SetSpan(new BackgroundColorSpan(Color.ParseColor(((BColorTextSection)section).color)),
                                section.GetSectionStart(),
                                section.GetSectionEnd(),
                                SpanTypes.ExclusiveExclusive);
                        }
                    }
                }
            }
            if (insertSections.ContainsKey(typeof(FColorTextSection)))
            {
                foreach (FColorTextSection section in insertSections[typeof(FColorTextSection)])
                {
                    if (((FColorTextSection)section).color != "")
                    {
                        Color color = Color.ParseColor(((FColorTextSection)section).color);
                        if (color != null)
                        {
                            RemoveOverlappingSpans(typeof(ForegroundColorSpan), section);
                            spanRange.SetSpan(new ForegroundColorSpan(Color.ParseColor(((FColorTextSection)section).color)),
                                section.GetSectionStart(),
                                section.GetSectionEnd(),
                                SpanTypes.ExclusiveExclusive);
                        }
                    }
                }
            }
            if (insertSections.ContainsKey(typeof(FontFamilyTextSection)))
            {
                foreach (FontFamilyTextSection section in insertSections[typeof(FontFamilyTextSection)])
                {
                    if (((FontFamilyTextSection)section).fontfamily != "")
                    {
                        RemoveOverlappingSpans(typeof(TypefaceSpan), section);
                        spanRange.SetSpan(new TypefaceSpan(((FontFamilyTextSection)section).fontfamily),
                            section.GetSectionStart(),
                            section.GetSectionEnd(),
                            SpanTypes.ExclusiveExclusive);
                    }
                }
            }
            if (insertSections.ContainsKey(typeof(UnderLineTextSection)))
            {
                foreach (UnderLineTextSection section in insertSections[typeof(UnderLineTextSection)])
                {
                    if (((UnderLineTextSection)section) != null)
                    {
                        RemoveOverlappingSpans(typeof(UnderlineSpan), section);
                        spanRange.SetSpan(new UnderlineSpan(),
                            section.GetSectionStart(),
                            section.GetSectionEnd(),
                            SpanTypes.ExclusiveExclusive);
                    }
                }
            }
            if (insertSections.ContainsKey(typeof(StrikeOutTextSection)))
            {
                foreach (StrikeOutTextSection section in insertSections[typeof(StrikeOutTextSection)])
                {
                    if (((StrikeOutTextSection)section) != null)
                    {
                        RemoveOverlappingSpans(typeof(StrikethroughSpan), section);
                        spanRange.SetSpan(new StrikethroughSpan(),
                            section.GetSectionStart(),
                            section.GetSectionEnd(),
                            SpanTypes.ExclusiveExclusive);
                    }
                }
            }
            if (insertSections.ContainsKey(typeof(StyleTextSection)))
            {
                foreach (StyleTextSection section in insertSections[typeof(StyleTextSection)])
                {
                    if (((StyleTextSection)section).isBold || ((StyleTextSection)section).isItalic)
                    {
                        TypefaceStyle style = TypefaceStyle.Normal;
                        if (((StyleTextSection)section).isBold)
                        {
                            style = TypefaceStyle.Bold;
                        }
                        if (((StyleTextSection)section).isItalic)
                        {
                            style = TypefaceStyle.Italic;
                        }

                        RemoveOverlappingSpans(typeof(StyleSpan), section);
                        spanRange.SetSpan(new StyleSpan(style),
                            section.GetSectionStart(),
                            section.GetSectionEnd(),
                            SpanTypes.ExclusiveExclusive);
                    }
                }
            }
            if (insertSections.ContainsKey(typeof(MarginTextSection)))
            {
                foreach (MarginTextSection section in insertSections[typeof(MarginTextSection)])
                {
                    /*section.sectionStart = section.sectionEnd;
                    RemoveOverlappingSpans(typeof(MarginSpan), section);
                    spanRange.SetSpan(new MarginSpan(20),
                            section.GetSectionStart(),
                            section.GetSectionEnd(),
                            SpanTypes.ExclusiveExclusive);*/

                }
            }
            if (insertSections.ContainsKey(typeof(BreakLineTextSection)))
            {
                //Always go from back to front
                insertSections[typeof(BreakLineTextSection)].Reverse();
                foreach (BreakLineTextSection section in insertSections[typeof(BreakLineTextSection)])
                {
                    spanRange.Insert(section.sectionEnd, "\n");
                }
            }

            noteEditText.TextFormatted = spanRange;
        }

        private void RemoveAll(Java.Lang.Object[] spans)
        {
            foreach (Java.Lang.Object obj in spans) {
                spanRange.RemoveSpan(obj);
            }
        }
        public override void SetText(string text)
        {
            if (text == null)
                text = "";
            this.noteEditText.Text = text;
            this.spanRange = new SpannableStringBuilder(noteEditText.Text);
        }

        public override int[] GetSelectionStartEnd()
        {
            return new int[2] {noteEditText.SelectionStart,noteEditText.SelectionEnd };
        }

        public override void SetDefaultColors(string foregroundColor, string backgroundColor)
        {
            this.noteEditText.SetBackgroundColor(Color.ParseColor(backgroundColor));
            this.noteEditText.SetTextColor(Color.ParseColor(foregroundColor));
        }

        public override void ClearStyles(int start, int end)
        {
            Java.Lang.Object[] overlappingSpans = (Java.Lang.Object[])spanRange.GetSpans(start,
                end, Java.Lang.Class.FromType(typeof(Java.Lang.Object)));
            RemoveAll(overlappingSpans);
        }

        public override void ClearText()
        {
            spanRange = null;
            noteEditText.Text = "";
        }

        public override string GetPlainText()
        {
            return noteEditText.Text;
        }
    }
}