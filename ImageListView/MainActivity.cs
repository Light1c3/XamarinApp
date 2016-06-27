using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.IO;
using Android.Graphics;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using WebRequestTutorial.SQLite;

namespace WebRequestTutorial
{
    [Activity(Label = "Birthday Diary ", MainLauncher = true, Icon = "@drawable/xs")]
    public class MainActivity : Activity
    {
        private ListView mListView;
        private Button mButtonMenu;
        private BaseAdapter<Contact> mAdapter;
        private List<Contact> mContacts;
        private ImageView mSelectedPic;

        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            mListView = FindViewById<ListView>(Resource.Id.listView);
            mContacts = new List<Contact>();

            Action<ImageView> action = PicSelected;

            mAdapter = new ContactListAdapter(this, Resource.Layout.row_contact, mContacts, action);
            mListView.Adapter = mAdapter;

            CreateDatabse();
            CreateContactsTable();
            GetAllDBContacts();

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.popup_menu, menu);
            return base.OnPrepareOptionsMenu(menu);
        }

        private void GetAllDBContacts()
        {
            DBRepo dbr = new DBRepo();
            mContacts.AddRange(dbr.GetAllContacts());
            mAdapter.NotifyDataSetChanged();

        }

        private void CreateContactsTable()
        {
            DBRepo dbr = new DBRepo();
            var result = dbr.CreateTable();
            Toast.MakeText(this, result, ToastLength.Short).Show();
        }

        private void CreateDatabse()
        {
            DBRepo dbr = new DBRepo();
            var result = dbr.CreateDB();
            Toast.MakeText(this, result, ToastLength.Short).Show();
        }

        private void PicSelected (ImageView selectedPic)
        {
            mSelectedPic = selectedPic;
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            this.StartActivityForResult(Intent.CreateChooser(intent, "Selecte a Photo"), 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                Stream stream = ContentResolver.OpenInputStream(data.Data);
                mSelectedPic.SetImageBitmap(DecodeBitmapFromStream(data.Data, 150, 150));
            }
        }

        private Bitmap DecodeBitmapFromStream (Android.Net.Uri data, int requestedWidth, int requestedHeight)
        {
            Stream stream = ContentResolver.OpenInputStream(data);
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeStream(stream);

            options.InSampleSize = CalculateSampleSize(options, requestedWidth, requestedHeight);

            stream = ContentResolver.OpenInputStream(data);
            options.InJustDecodeBounds = false;
            Bitmap bitmap = BitmapFactory.DecodeStream(stream, null, options);
            return bitmap;

        }

        private int CalculateSampleSize( BitmapFactory.Options options, int requestedWidth, int requestedHeight)
        {
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > requestedHeight || width > requestedWidth)
            {
                int halfHeight = height / 2;
                int halfWidth = width / 2;

                while((halfHeight / inSampleSize) > requestedHeight && (halfWidth / inSampleSize) > requestedWidth)
                {
                    inSampleSize *= 2;
                }
            }
            return inSampleSize;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.add:

                    CreateContactDialog dialog = new CreateContactDialog();
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();

                    //Subscribe to event
                    dialog.OnCreateContact += dialog_OnCreateContact;
                    dialog.Show(transaction, "create contact");
                    return true;    
                
                default:
                    return base.OnOptionsItemSelected(item);
            }
           
        }

        void dialog_OnCreateContact(object sender, CreateContactEventArgs e)
        {
            Contact newContact = new Contact() { Name = e.Name, Bithday = e.Birthday };

            mContacts.Add(newContact);
            mAdapter.NotifyDataSetChanged();

            DBRepo dbr = new DBRepo();
            string result = dbr.InsertContact(newContact);            

        }
    }
}

