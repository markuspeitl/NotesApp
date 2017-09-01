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

namespace NotesApp.Droid
{
    public class EditTextManager : ATextField
    {
        private EditText noteEditText;
        private Context sourceContext;
        SpannableString spanRange;

        public EditTextManager(EditText noteEditText, Context sourceContext)
        {
            this.noteEditText = noteEditText;
            this.sourceContext = sourceContext;
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
                this.spanRange = new SpannableString(noteEditText.Text);
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
                        Color color = Color.ParseColor(((FontFamilyTextSection)section).fontfamily);
                        if (color != null)
                        {
                            RemoveOverlappingSpans(typeof(TypefaceSpan), section);
                            spanRange.SetSpan(new TypefaceSpan(((FontFamilyTextSection)section).fontfamily),
                                section.GetSectionStart(),
                                section.GetSectionEnd(),
                                SpanTypes.ExclusiveExclusive);
                        }
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
            this.noteEditText.Text = text;
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
    }
}