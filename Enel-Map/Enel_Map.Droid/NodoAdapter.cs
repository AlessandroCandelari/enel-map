using System.Collections.Generic;

using Android.Content;
using Android.Views;
using Android.Widget;

namespace Enel_Map.Droid
{
    public class NodoAdapter : ArrayAdapter<Nodo>
    {

        public NodoAdapter(Context context, int textViewResourceId) : base(context, textViewResourceId) { }
        public NodoAdapter(Context context, int textViewResourceId, IList<Nodo> objects) : base(context, textViewResourceId, objects) { }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View v = convertView;

            if (v == null)
            {
                LayoutInflater vi = LayoutInflater.From(Context);
                v = vi.Inflate(Resource.Layout.fragment_nodi_item, null);
            }
            TextView txtCodice = v.FindViewById<TextView>(Resource.Id.txt_item_codice);
            TextView txtIndirizzo = v.FindViewById<TextView>(Resource.Id.txt_item_indirizzo);
            TextView txtNome = v.FindViewById<TextView>(Resource.Id.txt_item_nome);

            txtCodice.Text = "";
            txtIndirizzo.Text = "";
            txtNome.Text = "";

            Nodo nodo = GetItem(position);
            if (nodo != null)
            {
                if (txtCodice != null)
                {
                    txtCodice.Text = nodo.Codice;
                }
                if(txtIndirizzo != null)
                {
                    txtIndirizzo.Text = nodo.Indirizzo;
                }
                if(txtNome != null)
                {
                    txtNome.Text = nodo.Nome;
                }
            }
            return v;
        }
    }
}