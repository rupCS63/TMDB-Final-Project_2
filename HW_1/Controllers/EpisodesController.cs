using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HW_1.Models;


namespace HW_1.Controllers
{
    public class EpisodesController : ApiController
    {
         //GET api/<controller>
        public IEnumerable<Episode> Get()
        {
            Episode episode = new Episode();
            List<Episode> episodeList = episode.Get();
            return episodeList;
        }

        //"../api/Episodes?id=" + user_id;
        public List<Episode> Get(string id)
        {
            Episode episode = new Episode();
            return episode.GetUserEpisodesById(id);

        }

        // GET api//api/GetEpisodeByTvName?tvName=" + tvName + "&id=" + user_id
        //[HttpGet]
        //[Route("api/Episodes/GetEpisodeByTvName")]
        public IEnumerable<Episode> Get(string tvName,string user_id)
        {
            Episode e = new Episode();
            List<Episode> sList = e.GetEpisodeByTvName(tvName,user_id);
            return sList;
        }

        // POST api/<controller>
        public int Post([FromBody] Episode episode,int id)
        {
            return episode.InsertToSQL(id);
            
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}