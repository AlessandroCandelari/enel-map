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
using System.Globalization;

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
            var lines = File.ReadAllLines(path).Select(a => a.Split(';'));
            
            List<Nodo> nodi = new List<Nodo>();
            foreach (var line in lines)
            {
                var lat = line[2].Split(',')[0];
                var lng = line[2].Split(',')[1];

                var nodo = new Nodo
                {
                    Codice = line[0],
                    Nome = line[1],
                    Latitudine = decimal.Parse(lat, new CultureInfo("en-US")),
                    Longitudine = decimal.Parse(lng, new CultureInfo("en-US"))
                };
                nodi.Add(nodo);
            }
            return nodi;
        }
    }
}