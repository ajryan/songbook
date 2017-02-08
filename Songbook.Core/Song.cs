using System.Collections.Generic;
using Newtonsoft.Json;

namespace Songbook.Core
{
    public class Song
    {
        public string id { get; set; }

        public string Name { get; set; }
        public List<Lyric> Lyrics { get; }
        public List<Progression> Progressions { get;  }

        public Song()
        {
            Lyrics = new List<Lyric>();
            Progressions = new List<Progression>();
        }
    }
}
