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

        private NoteManager manager;
        private SaveAndLoad dataManager;

        private NoteDisplayer noteDisplay;
        private EditTextManager editManager;

        private EditText noteEditText;
        private Button boldButton;
        private Button italicButton;
        private Button underlineButton;
        private Button redButton;
        private Button size18Button;
        private Button size24Button;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
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


            int selectedNotePos = this.Intent.GetIntExtra("SelectedNotePosition",0);
            NoteConnector selectedNote = manager.GetNoteFromPosition(selectedNotePos);

            editManager = new EditTextManager(noteEditText,this);
            noteDisplay = new NoteDisplayer(selectedNote, editManager);
        }

        private void OnSize24ButtonClicked(object sender, EventArgs e)
        {
            noteDisplay.ExecuteStyleChange(new TextStyle() { size = 29 });
        }

        private void OnSize18ButtonClicked(object sender, EventArgs e)
        {
            noteDisplay.ExecuteStyleChange(new TextStyle() { size = 22 });
        }

        private void OnRedButtonClicked(object sender, EventArgs e)
        {
            noteDisplay.ExecuteStyleChange(new TextStyle() { color = "red" });
        }

        private void OnUnderlineButtonClicked(object sender, EventArgs e)
        {
            noteDisplay.ExecuteStyleChange(new TextStyle() { isUnderlined = true });
        }

        private void OnItalicButtonClicked(object sender, EventArgs e)
        {
            noteDisplay.ExecuteStyleChange(new TextStyle() { isItalic = true });
        }

        private void OnBoldButtonClicked(object sender, EventArgs e)
        {
            noteDisplay.ExecuteStyleChange(new TextStyle() { isBold = true });
        }
    }
}