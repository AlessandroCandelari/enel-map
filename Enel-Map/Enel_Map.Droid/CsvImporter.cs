using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;

namespace Enel_Map.Droid
{
    class CsvImporter
    {
        public void Import(String path)
        {
            var nodi = this.GetNodiDaCsv(path);

            var conn = DbConn.GetSQLiteConnection();
            conn.DropTable<Nodo>();
            conn.CreateTable<Nodo>();
            conn.InsertAll(nodi);
            conn.Close();
        }
        private List<Nodo> GetNodiDaCsv(string path)
        {
            var nodo1 = new Nodo
            {
                ID = 1,
                Codice = "2",
                Tipo = 1,
                Nome = "nodo ajkfhdjsgdj",
                Longitudine = 13.382664M,
                Latitudine = 43.563909M
            };
            var nodo2 = new Nodo
            {
                ID = 2,
                Tipo = 2,
                Nome = "gianpeppo",
                Codice = "3",
                Longitudine = 13.239818M,
                Latitudine = 43.520070M
            };
            List<Nodo> nodi = new List<Nodo> { nodo1, nodo2 };

            return nodi;
        }
    }
}