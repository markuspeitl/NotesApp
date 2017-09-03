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

        private Button addNoteButton;
        private ListView noteSelectListView;
        private ArrayAdapter adapter;

        private List<string> noteTitles;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.note_list_layout);

            noteSelectListView = this.FindViewById<ListView>(Resource.Id.note_select_listview);
            addNoteButton = this.FindViewById<Button>(Resource.Id.addNoteButton);

            addNoteButton.Click += AddNewNoteClicked;

            //Application.Context.FilesDir.AbsolutePath
            GlobalSettings.SetupRootPath(Application.Context.GetExternalFilesDir(null).AbsolutePath);
            //For Testing
            GlobalSettings.SetupRootPath("/sdcard/");

            // Create your application here

            noteSelectListView.ItemClick += OnNoteItemClicked;


        }

        protected override void OnStart()
        {
            base.OnStart();

            dataManager = new SaveAndLoad();
            manager = new NoteManager(dataManager);
            noteTitles = new List<string>();
            noteTitles = manager.LoadNoteList(GlobalSettings.GetProxysPath());

            if (noteTitles.Count > 0)
            {
                adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, manager.noteTitles);

                noteSelectListView.Adapter = adapter;
            }
            else
            {
                AddNewNoteClicked(this, null);
            }
        }

        private void AddNewNoteClicked(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(EditNoteActivity));
            intent.PutExtra("NewNote", true);
            this.StartActivity(intent);
        }

        private void OnNoteItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            StartNextActivity(e);
        }

        private async void StartNextActivity(AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(EditNoteActivity));
            intent.PutExtra("SelectedNotePosition", e.Position);
            NoteProxy toLoadNoteProxy = await manager.GetNoteFromPosition(e.Position);
            if (toLoadNoteProxy != null)
            {
                intent.PutExtra("NoteProxyMetaPath", toLoadNoteProxy.GetProxyPath());
            }
            this.StartActivity(intent);
        }
    }
}