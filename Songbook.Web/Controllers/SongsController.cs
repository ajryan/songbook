using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Songbook.Core;

namespace Songbook.Controllers
{
    [Route("api/[controller]")]
    public class SongsController : Controller
    {
        private static readonly List<Song> _songList;

        static SongsController()
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
            System.Threading.Thread.Sleep(500);
            return _songList;
        }

        [HttpGet("{songId}", Name="GetSong")]
        public IActionResult GetById(int songId)
        {
            var song = _songList.FirstOrDefault(s => s.Id == songId);

            return song == null ? (IActionResult) NotFound()
                                : new ObjectResult(song);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Song song)
        {
            if (song == null)
                return BadRequest();

            song.Id = _songList.Count + 1;
            _songList.Add(song);

            return CreatedAtRoute("GetSong", new { songId = song.Id }, song);
        }

        [HttpPut("{songId}")]
        public IActionResult Update(int songId, [FromBody] Song song)
        {
            if (song?.Id != songId)
                return BadRequest();

            var existingSong = _songList.FirstOrDefault(s => s.Id == songId);

            if (existingSong == null)
                return NotFound();

            _songList.Remove(existingSong);
            _songList.Insert(song.Id - 1, song);

            return new NoContentResult();
        }

        [HttpDelete("{songId}")]
        public IActionResult Delete(int songId)
        {
            var deleteSong = _songList.FirstOrDefault(s => s.Id == songId);
            _songList.Remove(deleteSong);

            return new NoContentResult();
        }
    }
}
