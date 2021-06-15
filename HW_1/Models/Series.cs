using HW_1.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HW_1.Models
{
    public class Series
    {
        int id;
        string name;
        string first_air_date;
        string origin_country;
        string original_language;
        string overview;
        float popularity;
        string poster_path;


        public Series()
        {

        }
        public Series(int id, string name, string first_air_date, string origin_country, string original_language, string overview, float popularity, string poster_path)
        {
            this.id = id;
            this.name = name;
            this.first_air_date = first_air_date;
            this.origin_country = origin_country;
            this.original_language = original_language;
            this.overview = overview;
            this.popularity = popularity;
            this.poster_path = poster_path;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string First_air_date { get => first_air_date; set => first_air_date = value; }
        public string Origin_country { get => origin_country; set => origin_country = value; }
        public string Original_language { get => original_language; set => original_language = value; }
        public string Overview { get => overview; set => overview = value; }
        public float Popularity { get => popularity; set => popularity = value; }
        public string Poster_path { get => poster_path; set => poster_path = value; }


        public int InsertToSQL()
        {
            DataServices ds = new DataServices();
            ds.InsertToSQL(this);
            return 1;
        }








    }




}