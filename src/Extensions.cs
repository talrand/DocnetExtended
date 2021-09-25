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


        internal static void AddLine(this List<TextLine> textLines, TextLine textLine)
        {
            // As characters aren't in a readable order in the PDF structure, text lines may be fragmented
            // Check if a line with the same Y position has already been added to the collection
            TextLine existingTextLine = textLines.GetExistingTextLine(textLine.PagePosition.Y);

            if (existingTextLine != null)
            {
                existingTextLine.Join(textLine);
            }
            else
            {
                // Words may be jumbled due to PDF structure, need to sort them before adding text line  
                textLine.Words = textLine.Words.SortToReadableOrder();
                textLines.Add(textLine);
            }
        }
    }
}