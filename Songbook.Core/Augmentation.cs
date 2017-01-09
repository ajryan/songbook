namespace Songbook.Core
{
    public class Augmentation
    {
        public Accidental Accidental { get; set; }
        public int Interval { get; set; }

        public override string ToString()
        {
            return $"{Accidental.ToDescription()}{Interval}";
        }
    }
}