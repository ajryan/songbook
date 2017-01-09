using System;

namespace Songbook.Core
{
public enum Pitch
    {
        C, D, E, F, G, A, B
    }

    public enum Accidental
    {
        Flat,
        Sharp
    }

    public enum Key
    {
        Major,
        Minor
    }

    public static class EnumExtensions
    {
        public static string ToDescription(this Accidental accidental) => ((Accidental?) accidental).ToDescription();
        public static string ToDescription(this Accidental? accidental) => 
            accidental == Accidental.Flat ? "b" :
            accidental == Accidental.Sharp ? "#" :
            String.Empty;

        public static string ToDescription(this Key key) => ((Key?) key).ToDescription();
        public static string ToDescription(this Key? key) =>
            key == Key.Major ? "Maj" :
            key == Key.Minor ? "Min" :
            String.Empty;
    }
}
