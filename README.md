# DocNetExtended
DocNetExtended is a small extension library built upon the [DocNet library](https://github.com/GowenGit/docnet/), designed to extract text in a readable order from PDFs. 

Features
---
* Get text
* Get lines of text
* Get words
* Split lines of text into blocks

Usage
---

**Extracting all text**
```cs
using (var docReader = DocLib.Instance.GetDocReader(pdfFileName, new PageDimensions(2480, 3508)))
{
    using (var pageReader = new OrderedPageTextReader(docReader, 0))
    {
        Console.WriteLine(pageReader.GetTextInReadableOrder());
    }
}
```

**Extracting lines of text**

```cs
using (var docReader = DocLib.Instance.GetDocReader(pdfFileName, new PageDimensions(2480, 3508)))
{
    using (var pageReader = new OrderedPageTextReader(docReader, 0))
    {
        var textLines = pageReader.GetTextLines();

        foreach (var textLine in textLines)
        {
            Console.WriteLine(textLine.Text);
        }

    }
}
```

**Extracting all words**

```cs
using (var docReader = DocLib.Instance.GetDocReader(pdfFileName, new PageDimensions(2480, 3508)))
{
    using (var pageReader = new OrderedPageTextReader(docReader, 0))
    {
        var words = pageReader.GetWords();

        foreach (var word in words)
        {
            Console.WriteLine(word.Value);
        }
    }
}
```

**Extracting blocks of text**

When extracting text from a PDF, you may only be interested in a certain section of the page. 

 The **GetTextBlocks** method will split lines of text into blocks of text by dividing the page width by the block size, and then checking the position of each word to determine which block it should be in.
 
 Note: Blocks are currently calculated per TextLine.
 
 ```cs
using (var docReader = DocLib.Instance.GetDocReader(pdfFileName, new PageDimensions(2480, 3508)))
{
    using (var pageReader = new OrderedPageTextReader(docReader, 0))
    {
        var textBlocks = pageReader.GetTextBlocks(300);

        foreach (var textBlock in textBlocks)
        {
            Console.WriteLine(textBlock.Text);
        }
    }
}
 ```


Disclaimer
---
Whilst every attempt is made to extract data in the order it appears in the PDF, this is very much a work in progress and may not support the structure of all PDFs.


Credit
---
This project wouldn't be possible without the work done by the [DocNet team](https://github.com/GowenGit/docnet/)
