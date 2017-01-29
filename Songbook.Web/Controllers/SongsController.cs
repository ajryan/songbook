using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Songbook.Core;
using Songbook.Web.Services;

namespace Songbook.Controllers
{
    [Route("api/[controller]")]
    public class SongsController : Controller
    {
        static SongsController()
        {
            var song1 = new Song { Id = Guid.NewGuid().ToString(), Name = "Magazines" };
            song1.Lyrics.Add(new Lyric { Name = "Verse 1", Words = "I left home at age thirteen. Joined a crew selling magazines."});

            var song2 = new Song { Id = Guid.NewGuid().ToString(), Name = "Heavy Enough" };
            song2.Lyrics.Add(new Lyric { Name = "Verse 1", Words = "Dirt still on my hands, I came down from the fields. Dirt still on my hands, I came down from the fields. I know that rock's heavy enough. My baby won't get stealed."});

            var repo = new SongRepository();

            repo.CreateSong(song1).Wait();
            repo.CreateSong(song2).Wait();
        }

        [HttpGet]
        public async Task<IEnumerable<Song>> GetAll()
        {
            return await new SongRepository().GetAll();
        }

        [HttpGet("{songId}", Name="GetSong")]
        public IActionResult GetById(string songId)
        {
            var song = new SongRepository().GetSong(songId);

            return song == null ? (IActionResult) NotFound()
                                : new ObjectResult(song);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Song song)
        {
            if (song == null)
                return BadRequest();

            song.Id = Guid.NewGuid().ToString();
            await new SongRepository().CreateSong(song);

            return CreatedAtRoute("GetSong", new { songId = song.Id }, song);
        }

        [HttpPut("{songId}")]
        public async Task<IActionResult> Update(string songId, [FromBody] Song song)
        {
            if (song?.Id != songId)
                return BadRequest();

            await new SongRepository().UpdateSong(songId, song);

            return new NoContentResult();
        }

        [HttpDelete("{songId}")]
        public async Task<IActionResult> Delete(string songId)
        {
            await new SongRepository().DeleteSong(songId);

            return new NoContentResult();
        }
    }
}
