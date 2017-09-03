using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NotesApp.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        NoteManager noteManager;

        public MainPage()
        {
            this.InitializeComponent();

            GlobalSettings.SetupRootPath(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
            
            noteManager = new NoteManager(new SaveAndLoad());
            List<string> notes = noteManager.LoadNoteList(GlobalSettings.GetNotesPath());
            //noteManager.LoadNotes("C:\\Notes\\");

            SetupNoteDisplay();
        }

        public async void SetupNoteDisplay()
        {
            NoteConnector note = await noteManager.GetNoteFromPosition(0);

            RichTextManager display = new RichTextManager(noteEditText);

            NoteDisplayer noteDisplay = new NoteDisplayer(display);

            noteDisplay.DisplayNote(note);
        }
    }
}