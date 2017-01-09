using System.Collections.Generic;

namespace Songbook.Core
{
    public class Progression
    {
        public TimeSignature TimeSignature { get; }
        public int TempoBpm { get; set; }
        public List<ChordEvent> Chords { get; }

        public Progression()
        {
            TimeSignature = new TimeSignature();
            TempoBpm = 120;
            Chords = new List<ChordEvent>();
        }
    }
}