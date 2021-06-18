using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace HW_1.Models.DAL
{
    public class DataServices
    {
        static List<Episode> episodeList;
        static List<User> userList;

        public SqlDataAdapter da;
        public DataTable dt;



        public DataServices()
        {

        }

        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {
            // read the connection string from the configuration file
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        //--------------------------------------------------------------------
        // Build the Insert command String
        //--------------------------------------------------------------------
        private SqlCommand BuildInsertCommand(Object Obj , SqlConnection con)
        {
            if(Obj is User)
            {
                User temp = (User)Obj;
                // use a string builder to create the dynamic string
                string sql_insert = "Values(@username,@userlastname,@email,@password,@cellphone,@gender,@address,@genre,@yearofbirth)";
                string prefix = "INSERT INTO Users_2021 " + "(username,userlastname,email,password,cellphone,gender,address,genre,yearofbirth)";
                string CommandText = prefix + sql_insert;
                SqlCommand cmd = new SqlCommand(CommandText, con);
                cmd.Parameters.AddWithValue("@username", temp.Name);
                cmd.Parameters.AddWithValue("@userlastname", temp.LastName);
                cmd.Parameters.AddWithValue("@email", temp.Email);
                cmd.Parameters.AddWithValue("@password", temp.Password);
                cmd.Parameters.AddWithValue("@cellphone", temp.Cellphone);
                cmd.Parameters.AddWithValue("@gender", temp.Gender);
                cmd.Parameters.AddWithValue("@address", temp.Address);
                cmd.Parameters.AddWithValue("@genre", temp.Genre);
                cmd.Parameters.AddWithValue("@yearofbirth", temp.YearBirth);



                return cmd;
            }
            if (Obj is Episode)
            {
                Episode temp = (Episode)Obj;
                // use a string builder to create the dynamic string
                string sql_insert = "Values(@series_id,@episode_id,@episode_name,@air_date,@episode_overeview,@poster_path,@season_number)";
                string prefix = "INSERT INTO Episodes_2021 " + "(series_id,episode_id,episode_name,air_date,episode_overeview,poster_path,season_number)";
                string CommandText = prefix + sql_insert;
                SqlCommand cmd = new SqlCommand(CommandText, con);
                cmd.Parameters.AddWithValue("@series_id", temp.SeriesId);
                cmd.Parameters.AddWithValue("@episode_id", temp.Id);
                cmd.Parameters.AddWithValue("@episode_name", temp.Name);
                cmd.Parameters.AddWithValue("@air_date", temp.BroadcastDate);
                cmd.Parameters.AddWithValue("@episode_overeview", temp.Description);
                cmd.Parameters.AddWithValue("@poster_path", temp.Img);
                cmd.Parameters.AddWithValue("@season_number", temp.SeasonNumber);

                return cmd;
            }

            else if(Obj is Series)
            {

                Series temp = (Series)Obj;
                // use a string builder to create the dynamic string
                string sql_insert = "Values(@id,@name,@first_air_date,@origin_country,@original_language,@overview,@popularity,@poster_path)";
                string prefix = "INSERT INTO Series_2021 " + "(id, name, first_air_date,origin_country,original_language,overview,popularity,poster_path)";
                string CommandText = prefix + sql_insert;
                SqlCommand cmd = new SqlCommand(CommandText, con);
                cmd.Parameters.AddWithValue("@id",temp.Id);
                cmd.Parameters.AddWithValue("@name", temp.Name);
                cmd.Parameters.AddWithValue("@first_air_date", temp.First_air_date);
                cmd.Parameters.AddWithValue("@origin_country", temp.Origin_country);
                cmd.Parameters.AddWithValue("@original_language", temp.Original_language);
                cmd.Parameters.AddWithValue("@overview", temp.Overview);
                cmd.Parameters.AddWithValue("@popularity", temp.Popularity);
                cmd.Parameters.AddWithValue("@poster_path", temp.Poster_path.ToString());

                return cmd;
            }
            else
            {
                return new SqlCommand();
            }
            
        }

        private SqlCommand CreateCommand(SqlCommand cmd, SqlConnection con)
        {


            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }
        public List<User> GetUsers()
        {
            SqlConnection con = null;
            List<User> userList = new List<User>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Users_2021";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    User u = new User();

                    u.Name = (string)dr["username"];
                    u.LastName = (string)dr["userlastname"];
                    u.Email = (string)dr["email"];
                    u.Password = (string)dr["password"];
                    u.Cellphone = (string)dr["cellphone"];
                    u.Gender = (string)dr["gender"];
                    u.Genre = (string)dr["genre"];
                    u.Address = (string)dr["address"];
                    u.Id = (int)dr["userid"];
                    u.YearBirth = (int)dr["yearofbirth"];
                    u.IsAdmin = (bool)dr["isAdmin"];

                    userList.Add(u);
                }

                return userList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public User validLoginFromDB(string mail, string password)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Users_2021 WHERE email =" + "'" + mail + "'" + "and password = " + "'" + password + "'";       
                SqlCommand cmd = new SqlCommand(selectSTR, con);
                

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                User u = new User();

                while (dr.Read())
                {   // Read till the end of the data into a row
                    u.Id = Convert.ToInt32(dr["userid"]);
                    u.Name = (string)dr["username"];
                    u.LastName = (string)dr["userlastname"];
                    u.Email = (string)dr["email"];
                    u.Password = (string)dr["password"];
                    u.Cellphone = (string)dr["cellphone"];
                    u.Gender = (string)dr["gender"];
                    u.Address = (string)dr["address"];
                    
                }
                return u;

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }
        

        public int addToFav(Episode episode,int id)
        {
            
            SqlConnection con = new SqlConnection();

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string sql_insert = "Values(@userid1,@episode_id1)";
            string prefix = "INSERT INTO Favorites_2021 " + "(userid1,episode_id1)";
            string CommandText = prefix + sql_insert;
            SqlCommand cmd = new SqlCommand(CommandText, con);
            cmd.Parameters.AddWithValue("@userid1", id);
            cmd.Parameters.AddWithValue("@episode_id1", episode.Id);
            

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return 0;
                }
                else throw;
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int InsertToSQL<T>(T obj)
        {
            SqlCommand sendCmd = new SqlCommand();
            SqlConnection con = new SqlConnection();

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            sendCmd = BuildInsertCommand(obj,con);      // helper method to build the insert string

            try
            {
                int numEffected = sendCmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return 0;
                }
                else throw;
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int InsertUserDS(User user)
        {
            Console.WriteLine("InsertUser - dataservise.cs step 3");

            if (userList == null)
                userList = new List<User>();

            userList.Add(user);

            return 1;
        }
        public int Insert(Episode episode)
        {
            if (episodeList == null)
                episodeList = new List<Episode>();

            episodeList.Add(episode);

            return 1;
        }
        //public List<Episode> Get()
        //{
        //    return episodeList;
        //}
        public List<Episode> GetEpisodeByTvName(string tvName, string user_id)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT *"
                                    + " FROM Favorites_2021 as f"
                                    + " INNER JOIN Episodes_2021 as e"
                                    + " ON e.episode_id = f.episode_id1"
                                    + " WHERE f.userid1 = " + "'" + user_id + "'"+ " and e.episode_name ="  + tvName  ;
                SqlCommand cmd = new SqlCommand(selectSTR, con);


                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                List<Episode> listOfepisodes = new List<Episode>();
                while (dr.Read())
                {   // Read till the end of the data into a row
                    Episode e = new Episode();
                    e.Id = Convert.ToInt32(dr["episode_id"]);
                    e.SeriesId = Convert.ToInt32(dr["series_id"]);
                    e.SeasonNumber = Convert.ToInt32(dr["season_number"]);
                    e.EpisodeName = (string)dr["episode_name"];
                    e.Img = (string)dr["poster_path"];
                    e.Description = (string)dr["episode_overeview"];
                    e.BroadcastDate = Convert.ToDateTime(dr["air_date"]).ToString();

                    listOfepisodes.Add(e);

                }
                return listOfepisodes;

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)

                {
                    con.Close();
                }

            }
        }


        public List<Episode> GetUserEpisodesById(string user_id)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT *"
                                    + " FROM Favorites_2021 as f"
                                    + " INNER JOIN Episodes_2021 as e"
                                    + " ON e.episode_id = f.episode_id1"
                                    + " WHERE f.userid1 = "+"'"+user_id + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);


                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                List<Episode> listOfepisodes = new List<Episode>();
                while (dr.Read())
                {   // Read till the end of the data into a row
                    Episode e = new Episode();
                    e.Id = Convert.ToInt32(dr["episode_id"]);
                    e.SeriesId = Convert.ToInt32(dr["series_id"]);
                    e.SeasonNumber= Convert.ToInt32(dr["season_number"]);
                    e.EpisodeName = (string)dr["episode_name"];
                    e.Img = (string)dr["poster_path"];
                    e.Description = (string)dr["episode_overeview"];
                    e.BroadcastDate = Convert.ToDateTime(dr["air_date"]).ToString();
                   
                    listOfepisodes.Add(e);

                }
                return listOfepisodes;

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)

                {
                    con.Close();
                }

            }
        }

    }

}