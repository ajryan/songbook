using System;
using System.Collections.Generic;

namespace Songbook.Core
{
    public class Chord
    {
        public Note Note { get; }
        public Key? Key { get; set; }
        public bool IsSeventh { get; set; }
        public Note BaseNote { get; set; }
        public List<Augmentation> Augmentations { get; }

        public Chord()
        {
            Note = new Note();
            Augmentations = new List<Augmentation>();
        }

        public override string ToString()
        {
            string baseDesc = BaseNote == null ? null : $"/{BaseNote}";

            string augmentationDesc = null;

            if (Augmentations.Count > 0)
            {
                string augmentations = String.Join(",", Augmentations);
                augmentationDesc = $"({augmentations})";
            }

            return $"{Note}{Key}{(IsSeventh ? "7th" : null)}{augmentationDesc}{baseDesc}";
        }
    }
}