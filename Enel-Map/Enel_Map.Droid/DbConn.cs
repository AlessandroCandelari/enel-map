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
    static class DbConn
    {
        public static SQLiteConnection GetSQLiteConnection()
        {
            var path = GetFilePath();
            var esiste = File.Exists(path);
            var result = new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
            if (!esiste)
            {
                result.CreateTable<Nodo>();
            }
            return result;
        }
        private static string GetFilePath()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            return Path.Combine(documentsPath, "db.db");
        }
        public static void Close(SQLiteConnection conn)
        {
            conn.Close();
        }
    }
}