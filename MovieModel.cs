using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    internal class MovieModel : PropertyChanger 
    {

        private string id;
        private string title;
        private string year;

        public MovieModel(string id, string title, string year)
        {
            this.id = id;
            this.title = title;
            this.year = year;
        }

        public string ID { get { return id; } set => SetField(ref id, value); }
        public string Title { get { return title; } set=> SetField(ref title, value);  }
        public string Year { get { return year; } set => SetField(ref year, value); }
    }
}
