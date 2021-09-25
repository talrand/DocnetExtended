using System.Collections.Generic;
using System.Text;

namespace DocnetExtended.Models
{
    public class TextBlock
    {
        public List<Word> Words { get; internal set; }
        public string Text { get => ToString(); }
        public int BlockNumber { get; private set; }
        public int LineNumber { get; private set; }

        internal TextBlock(int lineNumber, int blockNumber)
        {
            Words = new List<Word>();
            LineNumber = lineNumber;
            BlockNumber = blockNumber;
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