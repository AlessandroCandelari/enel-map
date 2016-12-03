using System.Collections.Generic;
using System.Linq;

namespace Enel_Map.Droid
{
    static class NodoRepo
    {
        internal static List<Nodo> List(string query)
        {
            query = "%" + query + "%";
            return DbConn.GetSQLiteConnection().Query<Nodo>(
                " select * from Nodo " +
                $" where {nameof(Nodo.Codice)} like ? or " +
                $" {nameof(Nodo.Nome)} like ? or " +
                $" {nameof(Nodo.Indirizzo)} like ?" +
                " limit 200"
                , query, query, query).ToList();
        }

        internal static List<Nodo> List()
        {
            return DbConn.GetSQLiteConnection()
                .Query<Nodo>("select * from Nodo limit 200").ToList();
        }
    }
}