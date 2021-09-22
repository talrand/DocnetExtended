using System.Collections.Generic;
using System.Drawing;
using static DocnetExtended.Constants;

namespace DocnetExtended.Models
{
    public class TextLine
    {
        public Point PagePosition { get; internal set; }
        public string Value { get => ToString();}
        public List<Word> Words { get; internal set; }

        internal TextLine() 
        {
            Words = new List<Word>();
            PagePosition = new Point(NotSet, NotSet);
        }

        internal List<Word> Join(TextLine textLine)
        {
            foreach (var word in textLine.Words)
            {
                Words.Add(word);
            }

            return Words;
        }

        public override string ToString()
        {
            if (Words.Count == 0) return "";

            string value = "";
            Words.ForEach((Word w) => value += $"{w.Value} ");
            return value.Trim();
        }
    }
}