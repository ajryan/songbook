namespace Songbook.Core
{
    public class Note
    {
        public Pitch Pitch { get; set; }
        public Accidental? Accidental { get; set; }

        public override string ToString()
        {
            return $"{Pitch}{Accidental.ToDescription()}";
        }
    }
}