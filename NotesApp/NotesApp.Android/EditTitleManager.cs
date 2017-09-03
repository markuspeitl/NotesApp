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

namespace NotesApp.Droid
{
    public class EditTitleManager : ATitleField
    {
        private EditText title;
        public EditTitleManager(EditText title)
        {
            this.title = title;
        }

        public override void ClearText()
        {
            title.Text = "";
        }

        public override string GetPlainText()
        {
            return title.Text;
        }

        public override void SetDefaultColors(string foregroundColor, string backgroundColor)
        {
            this.title.SetBackgroundColor(Android.Graphics.Color.ParseColor(backgroundColor));
            this.title.SetTextColor(Android.Graphics.Color.ParseColor(foregroundColor));
        }

        public override void SetText(string text)
        {
            title.Text = text;
        }
    }
}