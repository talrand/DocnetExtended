using Docnet.Core.Models;

namespace DocnetExtended.Models
{
    public class Word
    {
        public string Value { get; internal set; }
        public BoundBox Box { get; internal set; }

        internal Word() { }

        internal Word(string value)
        {
            Value = value;
        }

        internal void UpdateEndBounds(int bottom, int right)
        {
            // Don't update end bounds if passed values are invalid
            if (right < Box.Left || bottom < Box.Top) return;

            Box = new BoundBox(Box.Left, Box.Top, right, bottom);
        }
    }
}