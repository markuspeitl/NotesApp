using NotesApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace NotesApp.UWP
{
    public class RichTextManager : ATextField
    {
        private RichEditBox noteEditText;
        public RichTextManager(RichEditBox noteEditText)
        {
            this.noteEditText = noteEditText;
        }

        public override void ClearStyles(int start, int end)
        {

        }

        public override void ClearText()
        {
            noteEditText.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
        }

        public override string GetPlainText()
        {
            string plainText = "";
            noteEditText.Document.GetText(Windows.UI.Text.TextGetOptions.None, out plainText);
            return plainText;
        }

        public override int[] GetSelectionStartEnd()
        {
            int[] startend = new int[2];
            startend[0] = noteEditText.Document.Selection.StartPosition;
            startend[1] = noteEditText.Document.Selection.EndPosition;

            return startend;
        }

        public override void InsertTextSections(Dictionary<Type, List<TextSectionObject>> insertSections)
        {
            if (insertSections.ContainsKey(typeof(SizeTextSection)))
            {
                foreach (SizeTextSection section in insertSections[typeof(SizeTextSection)])
                {
                    if (section.Size > 0)
                    {
                        //RemoveOverlappingSpans(typeof(AbsoluteSizeSpan), section);
                        ITextCharacterFormat charFormatting = this.GetCharFormatting(section.GetSectionStart(), section.GetSectionEnd());
                        charFormatting.Size = section.Size;
                        this.SetCharFormatting(charFormatting);
                    }
                }
            }
            if (insertSections.ContainsKey(typeof(BColorTextSection)))
            {
                foreach (BColorTextSection section in insertSections[typeof(BColorTextSection)])
                {
                    if (((BColorTextSection)section).color != "")
                    {
                        string color = ((BColorTextSection)section).color;
                        //Color color = Color.ParseColor(((BColorTextSection)section).color);
                        if (color != null)
                        {
                            //RemoveOverlappingSpans(typeof(BackgroundColorSpan), section);
                            ITextCharacterFormat charFormatting = this.GetCharFormatting(section.GetSectionStart(), section.GetSectionEnd());
                            charFormatting.BackgroundColor = ParseStringToColor(color);
                            this.SetCharFormatting(charFormatting);
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
                        string color = ((FColorTextSection)section).color;
                        //Color color = Color.ParseColor(((FColorTextSection)section).color);
                        if (color != null)
                        {
                            //RemoveOverlappingSpans(typeof(ForegroundColorSpan), section);
                            ITextCharacterFormat charFormatting = this.GetCharFormatting(section.GetSectionStart(), section.GetSectionEnd());
                            charFormatting.ForegroundColor = ParseStringToColor(color);
                            this.SetCharFormatting(charFormatting);
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
                        //RemoveOverlappingSpans(typeof(TypefaceSpan), section);
                        ITextCharacterFormat charFormatting = this.GetCharFormatting(section.GetSectionStart(), section.GetSectionEnd());
                        charFormatting.Name = ((FontFamilyTextSection)section).fontfamily;
                        this.SetCharFormatting(charFormatting);
                    }
                }
            }
            if (insertSections.ContainsKey(typeof(UnderLineTextSection)))
            {
                foreach (UnderLineTextSection section in insertSections[typeof(UnderLineTextSection)])
                {
                    if (((UnderLineTextSection)section) != null)
                    {
                        //RemoveOverlappingSpans(typeof(UnderlineSpan), section);
                        ITextCharacterFormat charFormatting = this.GetCharFormatting(section.GetSectionStart(), section.GetSectionEnd());
                        charFormatting.Underline = UnderlineType.Single;
                        this.SetCharFormatting(charFormatting);
                    }
                }
            }
            if (insertSections.ContainsKey(typeof(StrikeOutTextSection)))
            {
                foreach (StrikeOutTextSection section in insertSections[typeof(StrikeOutTextSection)])
                {
                    if (((StrikeOutTextSection)section) != null)
                    {
                        //RemoveOverlappingSpans(typeof(StrikethroughSpan), section);
                        ITextCharacterFormat charFormatting = this.GetCharFormatting(section.GetSectionStart(), section.GetSectionEnd());
                        charFormatting.Strikethrough = FormatEffect.On;
                        this.SetCharFormatting(charFormatting);
                    }
                }
            }
            if (insertSections.ContainsKey(typeof(StyleTextSection)))
            {
                foreach (StyleTextSection section in insertSections[typeof(StyleTextSection)])
                {
                    if (((StyleTextSection)section).isBold || ((StyleTextSection)section).isItalic)
                    {
                        //RemoveOverlappingSpans(typeof(StyleSpan), section);
                        ITextCharacterFormat charFormatting = this.GetCharFormatting(section.GetSectionStart(), section.GetSectionEnd());
                        if (((StyleTextSection)section).isBold)
                        {
                            charFormatting.Bold = FormatEffect.On;
                        }
                        if (((StyleTextSection)section).isItalic)
                        {
                            charFormatting.Italic = FormatEffect.On;
                        }
                        this.SetCharFormatting(charFormatting);
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
                    noteEditText.Document.Selection.StartPosition = section.sectionEnd;
                    noteEditText.Document.Selection.EndPosition = section.sectionEnd;
                    noteEditText.Document.Selection.Text = "\n";
                }
            }
        }

        public override void SetDefaultColors(string foregroundColor, string backgroundColor)
        {
            provider.AddToDic("yellow",         Colors.Yellow.ToString());
            provider.AddToDic("yellowgreen",    Colors.YellowGreen.ToString());
            provider.AddToDic("black",          Colors.Black.ToString());
            provider.AddToDic("gray",           Colors.Gray.ToString());
            provider.AddToDic("darkgray",       Colors.DarkGray.ToString());
            provider.AddToDic("orange",         Colors.Orange.ToString());
            Xamarin.Forms.Platform.UWP.ColorConverter conv = new Xamarin.Forms.Platform.UWP.ColorConverter();

            noteEditText.Background = new SolidColorBrush(ParseStringToColor(backgroundColor));
            noteEditText.Foreground = new SolidColorBrush(ParseStringToColor(foregroundColor));

            //ITextCharacterFormat format = noteEditText.Document.GetDefaultCharacterFormat();
            //format.ForegroundColor = Windows.UI.Colors.DarkBlue;
            //format.BackgroundColor = Windows.UI.Colors.YellowGreen;
            //noteEditText.Document.SetDefaultCharacterFormat(format);

            //noteEditText.UpdateLayout();
        }

        private Color ParseStringToColor(string colorRep)
        {
            byte[] colorBytes = provider.ParseColorStringToBytes(colorRep);
            if (colorBytes.Length.Equals(4))
            {
                return ColorHelper.FromArgb(colorBytes[0], colorBytes[1], colorBytes[2], colorBytes[3]);
            }
            else
            {
                return Colors.Red;
            }
        }

        public override void SetText(string text)
        {
            noteEditText.Document.SetText(Windows.UI.Text.TextSetOptions.None, text);
        }

        private ITextSelection selectedText;
        private int previouseSelectionStart;
        private int previouseSelectionEnd;

        private ITextCharacterFormat GetCharFormatting(int start, int end)
        {
            previouseSelectionStart = noteEditText.Document.Selection.StartPosition;
            previouseSelectionEnd = noteEditText.Document.Selection.EndPosition;

            // || Selection.SetRange(x,y)
            noteEditText.Document.Selection.StartPosition = start;
            noteEditText.Document.Selection.EndPosition = end;

            selectedText = noteEditText.Document.Selection;
            if (selectedText != null)
            {
                ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                return charFormatting;
            }
            return null;
        }
        private void SetCharFormatting(ITextCharacterFormat charFormatting)
        {
            selectedText.CharacterFormat = charFormatting;

            noteEditText.Document.Selection.StartPosition = previouseSelectionStart;
            noteEditText.Document.Selection.EndPosition = previouseSelectionEnd;
        }
    }
}
