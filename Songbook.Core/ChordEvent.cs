namespace Songbook.Core
{
    public class BaseEvent
    {
        public int StartBeat { get; set; }
        public int DurationBeats { get; set; }
    }

    public class ChordEvent : BaseEvent
    {
        public Chord Chord { get; }

        public ChordEvent()
        {
            Chord = new Chord();
        }
    }
}