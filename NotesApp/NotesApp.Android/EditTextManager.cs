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

        private const string defaultFont = "Times New Roman";
        private const int defaultSize = 20;
        private ColorStateList defaultColor = ColorStateList.ValueOf(Color.White);
        private Color defaultBackColor = Color.LightBlue;
        private Color defaultFrontColor = Color.Red;

        public override void SetStyleToSection(TextStyle textStyle, int textStart, int textEnd)
        {
            if(this.spanRange == null)
            {
                this.spanRange = new SpannableString(noteEditText.Text);
                noteEditText.TextFormatted = spanRange;
            }

            string fontfamily = defaultFont;
            if(textStyle.fontfamily != "")
            {
                fontfamily = textStyle.fontfamily;
            }
            
            TypefaceStyle typeStyle = TypefaceStyle.Normal;
            if (textStyle.isBold)
            {
                typeStyle = TypefaceStyle.Bold;
            }
            else if (textStyle.isItalic)
            {
                typeStyle = TypefaceStyle.Italic;
            }

            int textSize = defaultSize;
            if(textStyle.size != -1)
            {
                textSize = textStyle.size;
            }

            ColorStateList color = defaultColor;
            if(textStyle.color != "")
            {
                color = ColorStateList.ValueOf(Color.ParseColor(textStyle.color));

                if(color == null)
                    color = defaultColor;
            }

            //TextAppearanceSpan appearance = new TextAppearanceSpan(fontfamily, typeStyle, textSize, color, color);
            //spanRange.SetSpan(appearance, start, end, SpanTypes.ExclusiveExclusive);

            List<Java.Lang.Object> allSpannables = new List<Java.Lang.Object>();

            if (textStyle.size != -1)
            {
                AbsoluteSizeSpan aSpan = new AbsoluteSizeSpan(textSize, true);
                allSpannables.Add(aSpan);
            }
            if (textStyle.isUnderlined)
            {
                UnderlineSpan uSpan = new UnderlineSpan();
                allSpannables.Add(uSpan);
            }
            if (!textStyle.fontfamily.Equals(""))
            {
                TypefaceSpan tSpan = new TypefaceSpan(fontfamily);
                allSpannables.Add(tSpan);
            }
            if (!textStyle.backcolor.Equals(""))
            {
                BackgroundColorSpan bSpan = new BackgroundColorSpan(defaultBackColor);
                allSpannables.Add(bSpan);
            }
            if (!textStyle.color.Equals(""))
            {
                ForegroundColorSpan fSpan = new ForegroundColorSpan(defaultFrontColor);
                allSpannables.Add(fSpan);
            }
            if (textStyle.isStrikedOut)
            {
                StrikethroughSpan strikeSpan = new StrikethroughSpan();
                allSpannables.Add(strikeSpan);
            }
            if (typeStyle != TypefaceStyle.Normal)
            {
                StyleSpan styleSpan = new StyleSpan(typeStyle);
                allSpannables.Add(styleSpan);
            }

            SetAllSpans(allSpannables, textStart, textEnd, SpanTypes.ExclusiveExclusive);
            //URLSpan urlSpan = new URLSpan("www.google.com");
            //spanRange.SetSpan(urlSpan, start, end, SpanTypes.ExclusiveExclusive);
            //ImageSpan iSpan = new ImageSpan(;
            //spanRange.SetSpan(uSpan, start, end, SpanTypes.ExclusiveExclusive);
        }

        private void SetAllSpans(List<Java.Lang.Object> allSpannables, int start, int end, SpanTypes type)
        {
            foreach(Java.Lang.Object o in allSpannables)
            {
                if (o.GetType().Equals(typeof(TypefaceSpan)) || o.GetType().Equals(typeof(AbsoluteSizeSpan))){
                    SetSpanOfType(o, start, end, type, o.GetType(), true);
                }
                else
                {
                    SetSpanOfType(o, start, end, type, o.GetType(), false);
                }
            }
        }

        private void SetSpanOfType(Java.Lang.Object spannable,int start, int end, SpanTypes type, Type spanType, bool overwrite)
        {
            Java.Lang.Object[] overlappingSpans = (Java.Lang.Object[])spanRange.GetSpans(start,
                end, Java.Lang.Class.FromType(spanType));

            if (overlappingSpans.Length == 0)
            {
                spanRange.SetSpan(spannable, start, end, SpanTypes.ExclusiveExclusive);
            }
            else if (overlappingSpans.Length == 1 && IsInside(start,end, overlappingSpans[0]))
            {
                int oldStart = spanRange.GetSpanStart(overlappingSpans[0]);
                int oldEnd = spanRange.GetSpanEnd(overlappingSpans[0]);
                //Remove old span
                spanRange.RemoveSpan(overlappingSpans[0]);
                //Reuse Object if needed
                if(oldStart != start)
                    spanRange.SetSpan(overlappingSpans[0], oldStart, start, SpanTypes.ExclusiveExclusive);
                if (end != oldEnd)
                    spanRange.SetSpan(CopyFactory(overlappingSpans[0],spannable), end, oldEnd, SpanTypes.ExclusiveExclusive);
                if (overwrite)
                {
                    spanRange.SetSpan(spannable, start, end, SpanTypes.ExclusiveExclusive);
                }
            }
            else
            {
                int[] minmax = GetMinMaxPos(overlappingSpans, start, end);
                RemoveAll(overlappingSpans);
                spanRange.SetSpan(spannable, minmax[0], minmax[1], SpanTypes.ExclusiveExclusive);
            }
        }

        private void RemoveOverlappingSpans(Type ty, TextSectionObject section)
        {
            Java.Lang.Object[] overlappingSpans = (Java.Lang.Object[])spanRange.GetSpans(section.GetSectionStart(),
                section.GetSectionEnd(), Java.Lang.Class.FromType(ty));
            RemoveAll(overlappingSpans);
        }

        public override void InsertTextSections(List<TextSectionObject> insertSections)
        {
            foreach (TextSectionObject section in insertSections)
            {
                if (section.GetType().Equals(typeof(SizeTextSection)))
                {
                    if (((SizeTextSection)section).Size > 0)
                    {
                        RemoveOverlappingSpans(typeof(AbsoluteSizeSpan), section);
                        spanRange.SetSpan(new AbsoluteSizeSpan(((SizeTextSection)section).Size, true), 
                            section.GetSectionStart(),
                            section.GetSectionStart(),
                            SpanTypes.ExclusiveExclusive);
                    }
                }
                else if (section.GetType().Equals(typeof(BColorTextSection)))
                {
                    if (((BColorTextSection)section).color != "")
                    {
                        Color color = Color.ParseColor(((BColorTextSection)section).color);
                        if (color != null)
                        {
                            RemoveOverlappingSpans(typeof(BackgroundColorSpan), section);
                            spanRange.SetSpan(new BackgroundColorSpan(Color.ParseColor(((BColorTextSection)section).color)),
                                section.GetSectionStart(),
                                section.GetSectionStart(),
                                SpanTypes.ExclusiveExclusive);
                        }
                    }
                }
                else if (section.GetType().Equals(typeof(FColorTextSection)))
                {
                    if (((FColorTextSection)section).color != "")
                    {
                        Color color = Color.ParseColor(((FColorTextSection)section).color);
                        if (color != null)
                        {
                            RemoveOverlappingSpans(typeof(ForegroundColorSpan), section);
                            spanRange.SetSpan(new ForegroundColorSpan(Color.ParseColor(((FColorTextSection)section).color)),
                                section.GetSectionStart(),
                                section.GetSectionStart(),
                                SpanTypes.ExclusiveExclusive);
                        }
                    }
                }
            }
        }

        

        private Java.Lang.Object CopyFactory(Java.Lang.Object toCopyObject, Java.Lang.Object alternative)
        {
            if (toCopyObject.GetType().Equals(typeof(AbsoluteSizeSpan)))
            {
                if (((AbsoluteSizeSpan)toCopyObject).Size >0)
                {
                    return new AbsoluteSizeSpan(((AbsoluteSizeSpan)toCopyObject).Size, true);
                }
            }
            else if (toCopyObject.GetType().Equals(typeof(TypefaceSpan)))
            {
                if (((TypefaceSpan)toCopyObject).Family != null)
                {
                    return new TypefaceSpan(((TypefaceSpan)toCopyObject).Family);
                }
            }
            return alternative;
        }

        private void RemoveAll(Java.Lang.Object[] spans)
        {
            foreach (Java.Lang.Object obj in spans) {
                spanRange.RemoveSpan(obj);
            }
        }
        private bool IsInside(int newStart, int newEnd, Java.Lang.Object otherSpan)
        {
            int otherStart = spanRange.GetSpanStart(otherSpan);
            int otherEnd = spanRange.GetSpanEnd(otherSpan);

            if (newStart >= otherStart && newEnd <= otherEnd)
            {
                return true;
            }
            return false;
        }

        private int[] GetMinMaxPos(Java.Lang.Object[] spans, int currentStart, int currentEnd)
        {
            int[] minmax = new int[2];
            minmax[0] = currentStart;
            minmax[1] = currentEnd;

            foreach (Java.Lang.Object o in spans)
            {
                if (spanRange.GetSpanStart(o) < minmax[0])
                {
                    minmax[0] = spanRange.GetSpanStart(o);
                }
                if (spanRange.GetSpanEnd(o) > minmax[1])
                {
                    minmax[1] = spanRange.GetSpanEnd(o);
                }
            }
            return minmax;
        }

        public override void SetText(string text)
        {
            this.noteEditText.Text = text;
        }

        public override void SetText(SimpleHtmlNode html)
        {
            this.noteEditText.TextFormatted = Html.FromHtml(html.ToString());
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