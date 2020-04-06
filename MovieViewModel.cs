using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SearchEngine
{
    class MovieViewModel : PropertyChanger
    {
        SQLQueryGenerator sql = new SQLQueryGenerator();
        private string input = "Sta";
        private ObservableCollection<MovieModel> movieCollection = new ObservableCollection<MovieModel>();
        public string Input
        {           
            get
            {
                return input;
            }
            set
            {
                ParseInput(value);
                SetField(ref input, value);
            }
        }
        public ObservableCollection<MovieModel> MovieCollection
        {
            get => movieCollection; set => SetField(ref movieCollection, value);
        }
        
        public void ParseInput(string value)
        {
            if (String.IsNullOrEmpty(value))
                return;
            value = value.Trim();
            NpgsqlCommand command = new NpgsqlCommand(default, sql.Connection);
            if (value.All(x => Char.IsDigit(x)))
            {
                command.CommandText = sql.SearchYear(value);
            }
            else if(!(value.Any(x => Char.IsDigit(x))))
            {
                //MessageBox.Show("Not4Digits");
                command.CommandText = sql.SearchName(value);
            }
            else
            {
                command.CommandText = sql.SearchBoth(value);
            }
            movieCollection.Clear();
            var reader = command.ExecuteReader();
            while (reader.Read() && reader.HasRows)
            {
                var id = reader.GetValue(0).ToString();
                var title = reader.GetValue(2).ToString();
                var year = reader.GetValue(1).ToString();
                var movie = new MovieModel(id, title, year);
                if (!movieCollection.Contains(movie))
                    movieCollection.Add(movie);
            }
            reader.Close();
        }
    }
}
