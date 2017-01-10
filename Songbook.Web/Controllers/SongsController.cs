using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Songbook.Core;

namespace Songbook.Controllers
{
    [Route("api/[controller]")]
    public class SongsController : Controller
    {
        private readonly List<Song> _songList;

        public SongsController()
        {
            var song1 = new Song { Id = 1, Name = "Magazines" };
            song1.Lyrics.Add(new Lyric { Name = "Verse 1", Words = "I left home at age thirteen. Joined a crew selling magazines."});

            var song2 = new Song { Id = 2, Name = "Heavy Enough" };
            song2.Lyrics.Add(new Lyric { Name = "Verse 1", Words = "Dirt still on my hands, I came down from the fields. Dirt still on my hands, I came down from the fields. I know that rock's heavy enough. My baby won't get stealed."});

            _songList = new List<Song> { song1, song2 };
        }

        [HttpGet]
        public IEnumerable<Song> GetAll()
        {
            return _songList;
        }

        [HttpGet("{songId}", Name="GetSong")]
        public Song GetById(int songId)
        {
            return _songList.FirstOrDefault(song => song.Id == songId);
        }
    }
}
