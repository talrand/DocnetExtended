﻿using System.Collections.Generic;

namespace DocnetExtended.Models
{
    public class TextBlock
    {
        public List<Word> Words { get; internal set; }

        internal TextBlock()
        {
            Words = new List<Word>();
        }
    }
}