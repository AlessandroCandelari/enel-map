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
        private const int serieIndex = 0;
        private const int numeroIndex = 1;
        private const int nomeIndex = 2;
        private const int indirizzoIndex = 3;
        private const int cftIndex = 4;
        private const int costruzioneIndex = 5;
        private const int tipoIndex = 6;
        private const int lineaIndex = 7;
        private const int dataIndex = 8;
        private const int idoneaIndex = 9;
        private const int coordinateIndex = 10;
        private const int noteIndex = 11;

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
                var lat = line[coordinateIndex].Split(',')[0];
                var lng = line[coordinateIndex].Split(',')[1];
                var data = DateTime.ParseExact(line[dataIndex], "dd/MM/yyyy",new CultureInfo("it-IT")); 

                var nodo = new Nodo
                {
                    Codice = line[numeroIndex],
                    Nome = line[nomeIndex],
                    Latitudine = decimal.Parse(lat, new CultureInfo("en-US")),
                    Longitudine = decimal.Parse(lng, new CultureInfo("en-US")),
                    CFT = line[cftIndex],
                    Costruzione = line[costruzioneIndex],
                    Indirizzo = line[indirizzoIndex],
                    Linea = line[lineaIndex],
                    Tipo = line[tipoIndex],
                    Idonea = line[idoneaIndex],
                    Data = data
                };
                nodi.Add(nodo);
            }
            return nodi;
        }
    }
}