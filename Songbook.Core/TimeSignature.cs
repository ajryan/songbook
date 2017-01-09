namespace Songbook.Core
{
    public class TimeSignature
    {
        public int Beats { get; set; }
        public int Note { get; set; }

        public TimeSignature()
        {
            Beats = 4;
            Note = 4;
        }
    }
}