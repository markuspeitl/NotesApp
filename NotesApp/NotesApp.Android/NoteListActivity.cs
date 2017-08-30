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

namespace NotesApp.Droid
{
    [Activity(Label = "NoteListActivity")]
    public class NoteListActivity : Activity
    {

        NoteManager manager;
        SaveAndLoad dataManager;

        private ListView noteSelectListView;
        private ArrayAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.note_list_layout);

            noteSelectListView = this.FindViewById<ListView>(Resource.Id.note_select_listview);

            // Create your application here
            dataManager = new SaveAndLoad();
            manager = new NoteManager(dataManager);
            if (manager.noteTitles != null)
            {
                adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, manager.noteTitles);

                noteSelectListView.Adapter = adapter;

                noteSelectListView.ItemSelected += OnNoteItemSelected;

                noteSelectListView.ItemClick += OnNoteItemClicked;
            }

        }

        private void OnNoteItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(EditNoteActivity));
            intent.PutExtra("SelectedNotePosition", e.Position);
            this.StartActivity(intent);
        }

        private void OnNoteItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //NoteConnector selectedNote = manager.GetNoteFromPosition(e.Position);
            Intent intent = new Intent(this, typeof(EditNoteActivity));
            intent.PutExtra("SelectedNotePosition",e.Position);
            this.StartActivity(intent);
        }
    }
}