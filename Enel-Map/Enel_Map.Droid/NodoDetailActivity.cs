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

namespace Enel_Map.Droid
{
    [Activity(Label = "NodoDetailActivity")]
    public class NodoDetailActivity : Activity
    {
        private Nodo nodo;
        private TextView txtCodice;
        private TextView txtNome;
        private TextView txtLinea;
        private TextView txtIndirizzo;
        private TextView txtCFT;
        private TextView txtTipo;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.activity_nodo_detail);
            base.OnCreate(savedInstanceState);

            var nodoJson = Intent.GetStringExtra("nodo");
            nodo = Newtonsoft.Json.JsonConvert.DeserializeObject<Nodo>(nodoJson);

            txtCodice = FindViewById<TextView>(Resource.Id.txt_item_codice);
            txtCFT = FindViewById<TextView>(Resource.Id.txt_item_CFT);
            txtIndirizzo = FindViewById<TextView>(Resource.Id.txt_item_indirizzo);
            txtLinea = FindViewById<TextView>(Resource.Id.txt_item_linea);
            txtNome = FindViewById<TextView>(Resource.Id.txt_item_nome);
            txtTipo = FindViewById<TextView>(Resource.Id.txt_item_tipo);

            txtCodice.Text = nodo.Codice;
            txtCFT.Text = nodo.CFT;
            txtIndirizzo.Text = nodo.Indirizzo;
            txtLinea.Text = nodo.Linea;
            txtNome.Text = nodo.Nome;
            txtTipo.Text = nodo.Tipo.ToString();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var item = menu.Add("Mappa");
            item.SetShowAsAction(ShowAsAction.Always);

            item.SetOnMenuItemClickListener(new MappaListener(this, nodo));

            base.OnCreateOptionsMenu(menu);

            return true;
        }
    }

    class MappaListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
    {
        private Activity activity;
        private Nodo nodoCorrente;

        public MappaListener(Activity activity, Nodo nodoCorrente)
        {
            this.activity = activity;
            this.nodoCorrente = nodoCorrente;
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            var latitude = nodoCorrente.Latitudine.ToString().Replace(",", ".");
            var longitude = nodoCorrente.Longitudine.ToString().Replace(",", ".");
            var geoUri = Android.Net.Uri.Parse($"http://maps.google.com/maps?q={latitude},{longitude}");
            var intent = new Intent(Intent.ActionView, geoUri);
            activity.StartActivity(intent);
            return true;
        }

    }
}