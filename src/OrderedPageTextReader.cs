using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Docnet.Core.Models;
using Docnet.Core.Readers;
using DocnetExtended.Models;
using static DocnetExtended.Constants;

namespace DocnetExtended
{
    public class OrderedPageTextReader : IDisposable
    {
        private readonly int _pageIndex = 0;
        private readonly IDocReader _docReader = null;

        public OrderedPageTextReader(IDocReader docReader, int pageIndex)
        {
            if (docReader == null) throw new ArgumentNullException(nameof(docReader));

            _docReader = docReader;
            _pageIndex = pageIndex;
        }

        public void Dispose() { }

        /// <summary>Read text from the PDF page in the order it appears on screen</summary>
        public string GetTextInReadableOrder()
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<TextLine> textLines = GetTextLines();

            foreach (var line in textLines)
            {
                // Ignore lines that have no page position
                if (line.PagePosition.X.Equals(NotSet)) continue;

                stringBuilder.AppendLine(line.ToString());
            }
            return stringBuilder.ToString();
        }

        /// <summary>Splits lines of text in the PDF page into blocks of a specified size</summary>
        public List<TextBlock> GetTextBlocks(int blockWidth)
        {
            if (blockWidth == 0) throw new ArgumentException($"{nameof(blockWidth)} must be greater than 0");

            List<TextLine> lines = GetTextLines();
            List<TextBlock> textBlocks = new List<TextBlock>();
            decimal blocksPerLine = 0;
            decimal blockStartPos = 0;
            decimal blockEndPos = 0;

            using (var pageReader = _docReader.GetPageReader(_pageIndex))
            {
                // Calculate no. blocks per line
                blocksPerLine = pageReader.GetPageWidth() / blockWidth;

                foreach (var line in lines)
                {
                    for (decimal i = 0; i < blocksPerLine; i++)
                    {
                        TextBlock textBlock = new TextBlock();

                        // Set start and end position for text block
                        blockStartPos = i * blockWidth;
                        blockEndPos = blockStartPos + blockWidth;

                        // Find all words in the line that are within the bounds of the block
                        textBlock.Words = line.Words.Where(w => w.Box.Left >= blockStartPos && w.Box.Left < blockEndPos).ToList();

                        if (textBlock.Words.Count > 0)
                        {
                            textBlocks.Add(textBlock);
                        }
                    }
                }
            }

            return textBlocks;
        }

        /// <summary>Get all words from the PDF page in the order they appear on screen</summary>
        public List<Word> GetWords()
        {
            List<TextLine> lines = GetTextLines();
            List<Word> words = new List<Word>();

            // Get all words from each line
            lines.ForEach(l => l.Words.ForEach(w => words.Add(w)));

            return words;
        }

        /// <summary>Get all lines of text in the PDF page</summary>
        public List<TextLine> GetTextLines()
        {
            List<TextLine> textLines = new List<TextLine>();
            TextLine currentTextLine = new TextLine();
            Word word = new Word();

            using (var pageReader = _docReader.GetPageReader(_pageIndex))
            {
                // Read all characters on page. This will include their position on page
                List<Character> characters = pageReader.GetCharacters().ToList();

                // Loop through characters to construct lines of text in the document
                for (int i = 0; i < characters.Count; i++)
                {
                    var character = characters[i];

                    if (character.Char.Equals('\n'))
                    {
                        // As characters aren't in a readable order in the PDF structure, text lines may be fragmented
                        // Check if a line with the same Y position has already been added to the collection
                        TextLine existingTextLine = textLines.GetExistingTextLine(currentTextLine.PagePosition.Y);

                        if (existingTextLine != null)
                        {
                            existingTextLine.Join(currentTextLine);
                        }
                        else
                        {
                            // Words may be jumbled due to PDF structure, need to sort them before adding text line  
                            currentTextLine.Words = currentTextLine.Words.SortToReadableOrder();
                            textLines.Add(currentTextLine);
                        }

                        currentTextLine = new TextLine();
                    }
                    else
                    {
                        // Set start position for new line
                        if (currentTextLine.PagePosition.X == NotSet && currentTextLine.PagePosition.Y == NotSet)
                        {
                            currentTextLine.PagePosition = new Point(character.Box.Left, character.Box.Top);
                        }

                        if (String.IsNullOrWhiteSpace(character.Char.ToString()))
                        {
                            // White space detected. Add word to collection and begin a new word
                            currentTextLine.Words.Add(word);
                            word = new Word();
                        }
                        else
                        {
                            // Create a bounding box for new words
                            if (word.Box.IsEmpty())
                            {
                                word.Box = new BoundBox(character.Box.Left, character.Box.Top, character.Box.Right, character.Box.Bottom);
                            }
                            else
                            {
                                word.UpdateEndBounds(character.Box.Bottom, character.Box.Right);
                            }

                            word.Value += character.Char.ToString();
                        }
                    }
                }

                // Add last line to collection
                if (String.IsNullOrEmpty(word.Value))
                {
                    currentTextLine.Words.Add(word);
                    textLines.Add(currentTextLine);
                }
            }

            return textLines.SortToReadableOrder();
        }
    }
}