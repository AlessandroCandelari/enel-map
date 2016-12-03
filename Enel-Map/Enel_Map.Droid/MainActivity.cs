using System;

using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Provider;
using Android.Database;

namespace Enel_Map.Droid
{
	[Activity (Label = "Enel_Map", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        private NodoAdapter adapter;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			ListView list = FindViewById<ListView> (Resource.Id.list);
            list.ItemClick += List_ItemClick;

            List<Nodo> nodi = DbConn.GetSQLiteConnection().Table<Nodo>().ToList();

            adapter = new NodoAdapter(this, Resource.Layout.fragment_nodi_item, nodi);
            list.Adapter = adapter;
        }

        private void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Console.WriteLine("Apri dettaglio");
            var nodo = ((ListView)sender).GetItemAtPosition(e.Position).Cast<Nodo>();
            var intent = new Intent(this, typeof(NodoDetailActivity));
            var nodoJson = Newtonsoft.Json.JsonConvert.SerializeObject(nodo);
            intent.PutExtra("nodo", nodoJson);
            StartActivity(intent);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var item = menu.Add("Importa csv");
            item.SetShowAsAction(ShowAsAction.Always);

            item.SetOnMenuItemClickListener(new ImportCsvListener(this));

            base.OnCreateOptionsMenu(menu);

            return true;
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1 && resultCode == Result.Ok)
            {
                String path = GetPathFromURI(this, data.Data);
                CsvImporter importer = new CsvImporter();
                importer.Import();
                adapter.NotifyDataSetChanged();
            }
        }


        internal static String GetPathFromURI(Context context, Android.Net.Uri uri)
        {
            bool isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;

            // DocumentProvider
            if (isKitKat && DocumentsContract.IsDocumentUri(context, uri))
            {
                // ExternalStorageProvider
                if (IsExternalStorageDocument(uri))
                {
                    String docId = DocumentsContract.GetDocumentId(uri);
                    String[] split = docId.Split(':');
                    String type = split[0];

                    if ("primary".Equals(type, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Android.OS.Environment.ExternalStorageDirectory + "/" + split[1];
                    }
                }
                // DownloadsProvider
                else if (IsDownloadsDocument(uri))
                {

                    String id = DocumentsContract.GetDocumentId(uri);
                    Android.Net.Uri contentUri = ContentUris.WithAppendedId(Android.Net.Uri.Parse("content://downloads/public_downloads"), (long)Convert.ToDouble(id));

                    return GetDataColumn(context, contentUri, null, null);
                }
                // MediaProvider
                else if (IsMediaDocument(uri))
                {
                    String docId = DocumentsContract.GetDocumentId(uri);
                    String[] split = docId.Split(':');
                    String type = split[0];

                    Android.Net.Uri contentUri = null;
                    if ("image".Equals(type))
                    {
                        contentUri = MediaStore.Images.Media.ExternalContentUri;
                    }
                    else if ("video".Equals(type))
                    {
                        contentUri = MediaStore.Video.Media.ExternalContentUri;
                    }
                    else if ("audio".Equals(type))
                    {
                        contentUri = MediaStore.Audio.Media.ExternalContentUri;
                    }

                    String selection = "_id=?";
                    String[] selectionArgs = new String[] { split[1] };

                    return GetDataColumn(context, contentUri, selection, selectionArgs);
                }
            }
            // MediaStore (and general)
            else if ("content".Equals(uri.Scheme, StringComparison.CurrentCultureIgnoreCase))
            {
                return GetDataColumn(context, uri, null, null);
            }
            // File
            else if ("file".Equals(uri.Scheme, StringComparison.CurrentCultureIgnoreCase))
            {
                return uri.Path;
            }

            return null;
        }

        private static String GetDataColumn(Context context, Android.Net.Uri uri, String selection, String[] selectionArgs)
        {
            ICursor cursor = null;
            String column = "_data";
            String[] projection = { column };

            try
            {
                cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs, null);
                if (cursor != null && cursor.MoveToFirst())
                {
                    int column_index = cursor.GetColumnIndexOrThrow(column);
                    return cursor.GetString(column_index);
                }
            }
            finally
            {
                if (cursor != null) { cursor.Close(); }
            }
            return null;
        }
        private static bool IsExternalStorageDocument(Android.Net.Uri uri)
        {
            return "com.android.externalstorage.documents".Equals(uri.Authority);
        }
        private static bool IsDownloadsDocument(Android.Net.Uri uri)
        {
            return "com.android.providers.downloads.documents".Equals(uri.Authority);
        }
        private static bool IsMediaDocument(Android.Net.Uri uri)
        {
            return "com.android.providers.media.documents".Equals(uri.Authority);
        }

    }
    class ImportCsvListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
    {
        private Activity activity;

        public ImportCsvListener(Activity activity)
        {
            this.activity = activity;
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            Intent intent = null;
            intent = new Intent(Intent.ActionGetContent);
            intent.SetType("*/*");
            // intent.PutExtra(Intent.ExtraLocalOnly, true); // disabilita i provider tipo dropbox e googledrive

            activity.StartActivityForResult(intent, 1);
            return true;
            
        }
    }
    internal static class ObjectTypeHelper
    {
        public static T Cast<T>(this Java.Lang.Object obj) where T : class
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
        }
    }
}


