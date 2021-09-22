using System.Collections.Generic;
using System.Linq;
using Docnet.Core.Models;
using DocnetExtended.Models;

namespace DocnetExtended
{
    internal static class Extensions
    {
        internal static bool IsEmpty(this BoundBox box)
        {
            return box.Top == 0 && box.Left == 0 && box.Bottom == 0 && box.Right == 0; 
        }

        internal static List<TextLine> SortToReadableOrder(this List<TextLine> textLines)
        {
            return textLines.OrderBy(l => l.PagePosition.Y).ThenBy(l => l.PagePosition.X).ToList();
        }

        internal static List<Word> SortToReadableOrder(this List<Word> words)
        {
            return words.OrderBy(w => w.Box.Left).ToList();
        }

        internal static TextLine GetExistingTextLine(this List<TextLine> textLines, int yPos)
        {
            return textLines.FirstOrDefault((l) => l.PagePosition.Y == yPos);
        }
    }
}