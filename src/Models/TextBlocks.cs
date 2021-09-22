using System.Collections.Generic;
using System.Text;

namespace DocnetExtended.Models
{
    public class TextBlock
    {
        public List<Word> Words { get; internal set; }

        internal TextBlock()
        {
            Words = new List<Word>();
        }

        public override string ToString()
        {
            if (Words.Count == 0) return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            Words.ForEach(w => stringBuilder.Append(w.Value + " "));
            return stringBuilder.ToString().Trim();
        }
    }
}