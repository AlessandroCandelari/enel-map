using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Enel_Map.Droid
{
    public class Nodo
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public String Codice { get; set; }
        public String Tipo { get; set; } 
        public String Nome { get; set; }
        public String Indirizzo { get; set; }
        public String CFT { get; set; }
        public String Costruzione { get; set; }
        public String Linea { get; set; }
        public String Idonea { get; set; }

        public DateTime Data { get; set; }

        public Decimal Latitudine { get; set; }
        public Decimal Longitudine { get; set; }
    }
}