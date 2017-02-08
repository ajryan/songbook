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
        private readonly SongRepository _repository;

        public SongsController(SongRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Song>> GetAll()
        {
            return await _repository.GetAll();
        }

        [HttpGet("{songId}", Name="GetSong")]
        public async Task<IActionResult> GetById(string songId)
        {
            var song = await _repository.GetSong(songId);

            return song == null ? (IActionResult) NotFound()
                                : new ObjectResult(song);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Song song)
        {
            if (song == null)
                return BadRequest();

            song.id = Guid.NewGuid().ToString();
            await _repository.CreateSong(song);

            return CreatedAtRoute("GetSong", new { songId = song.id }, song);
        }

        [HttpPut("{songId}")]
        public async Task<IActionResult> Update(string songId, [FromBody] Song song)
        {
            if (song?.id != songId)
                return BadRequest();

            await _repository.UpdateSong(songId, song);

            return new NoContentResult();
        }

        [HttpDelete("{songId}")]
        public async Task<IActionResult> Delete(string songId)
        {
            await _repository.DeleteSong(songId);

            return new NoContentResult();
        }
    }
}
