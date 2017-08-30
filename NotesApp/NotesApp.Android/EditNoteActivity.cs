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
using Android.Text;
using Android.Text.Style;
using Android.Graphics;

namespace NotesApp.Droid
{
    [Activity(Label = "EditNoteActivity")]
    public class EditNoteActivity : Activity
    {

        NoteManager manager;
        SaveAndLoad dataManager;

        private EditText noteEditText;

        Button boldButton;
        Button italicButton;
        Button underlineButton;
        Button redButton;
        Button size18Button;
        Button size24Button;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.note_editor_layout);

            noteEditText = this.FindViewById<EditText>(Resource.Id.note_editor_edittext);

            boldButton = this.FindViewById<Button>(Resource.Id.boldStyleButton);
            italicButton = this.FindViewById<Button>(Resource.Id.italicStyleButton);
            underlineButton = this.FindViewById<Button>(Resource.Id.underlinedStyleButton);
            redButton = this.FindViewById<Button>(Resource.Id.redStyleButton);
            size18Button = this.FindViewById<Button>(Resource.Id.size18StyleButton);
            size24Button = this.FindViewById<Button>(Resource.Id.size24StyleButton);

            boldButton.Click += OnBoldButtonClicked;
            italicButton.Click += OnItalicButtonClicked;
            underlineButton.Click += OnUnderlineButtonClicked;
            redButton.Click += OnRedButtonClicked;
            size18Button.Click += OnSize18ButtonClicked;
            size24Button.Click += OnSize24ButtonClicked;

            // Create your application here
            dataManager = new SaveAndLoad();
            manager = new NoteManager(dataManager);
        }

        private void OnSize24ButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnSize18ButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnRedButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnUnderlineButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnItalicButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnBoldButtonClicked(object sender, EventArgs e)
        {
            int start = noteEditText.SelectionStart;
            int end = noteEditText.SelectionEnd;
            SpannableString spanRange = new SpannableString(noteEditText.Text);
            TextAppearanceSpan appearance = new TextAppearanceSpan("New Times Roman", TypefaceStyle.Bold, 18, Android.Content.Res.ColorStateList.ValueOf(Color.Red), Android.Content.Res.ColorStateList.ValueOf(Color.Red));
            spanRange.SetSpan(appearance, start, end, SpanTypes.ExclusiveExclusive);
            noteEditText.TextFormatted = spanRange;
        }
    }
}