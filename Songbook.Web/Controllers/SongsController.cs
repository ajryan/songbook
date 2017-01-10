using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Songbook.Core;

namespace Songbook.Controllers
{
    [Route("api/[controller]")]
    public class SongsController : Controller
    {
        [HttpGet("[action]")]
        public IEnumerable<Song> All()
        {
            // TODO: put name on ctor of everything with a name

            var song1 = new Song { Name = "Magazines" };
            song1.Lyrics.Add(new Lyric { Name = "Verse 1", Words = "I left home at age thirteen. Joined a crew selling magazines."});

            var song2 = new Song { Name = "Heavy Enough" };
            song2.Lyrics.Add(new Lyric { Name = "Verse 1", Words = "Dirt still on my hands, I came down from the fields. Dirt still on my hands, I came down from the fields. I know that rock's heavy enough. My baby won't get stealed."});

            return new List<Song> { song1, song2 };
        }
    }
}
