using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchEngine
{
    public class SQLQueryGenerator
    {
        private NpgsqlConnection connection = new NpgsqlConnection("Server=91.245.227.59;Port=5432;User ID=Searcher;Password=qwerty;Database=SearchingMethods;");
        public NpgsqlConnection Connection
        {
            get=>connection;
            private set => connection = value;
        }
        StringBuilder sb = new StringBuilder();
        public SQLQueryGenerator()
        {
            connection.Open();
        }
        ~SQLQueryGenerator()
        {
            connection.Close();
        }
        private static string baseString = $@"
            SELECT *
            FROM public.movies
            WHERE ";
        public string SearchName(string input) 
        {
            sb = new StringBuilder(baseString + "name" + " ILIKE " + $"'%{input}%'");
            return sb.ToString();
        }
        public string SearchYear(string year)
        {
            sb = new StringBuilder(baseString + "CAST(year AS TEXT) ILIKE " + $"'%{year}%'");
            return sb.ToString();
        }
        public string SearchBoth(string input)
        {
            var words = input.Trim().Split(' ');
            words = words.Where(x=>!String.IsNullOrEmpty(x)).Select(x => x.Trim()).ToArray();   
            string year = words.Where(x=>x.All(y=>Char.IsDigit(y))).First();
            string title = String.Join(" ",words).Replace(year,String.Empty).Trim();
            sb = new StringBuilder(baseString + "name" + " ILIKE " + $"'%{title}%'" + " AND " + "CAST(year AS TEXT) ILIKE " + $"'%{year}%'");
            return sb.ToString();
        }
        string SearchTsVector(string input)
        {
            throw new NotImplementedException();
        }
    }
}
