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
using Android.Text.Style;
using Android.Graphics;
using Java.Lang;

namespace NotesApp.Droid.CustomSpans
{
    public class MarginSpan : Java.Lang.Object, ILineHeightSpan
    {
        private int margin;

        public MarginSpan(int margin)
        {
            this.margin = margin;
        }

        public void ChooseHeight(ICharSequence text, int start, int end, int spanstartv, int v, Paint.FontMetricsInt fm)
        {
            
            fm.Top += margin;
            fm.Ascent += margin;

            fm.Bottom += margin;
            fm.Descent += margin;
            
        }
    }
}