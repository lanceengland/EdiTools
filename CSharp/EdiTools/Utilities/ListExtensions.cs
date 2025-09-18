﻿using System.Collections.Generic;
using System.Text;

namespace EdiTools.Utilities
{
    public static class ListExtensions
    {
        public static string GetTextFromFirstToLastSegment(this List<Segment> segments)
        {
            var firstSegment = segments[0];
            var lastSegment = segments[segments.Count - 1];

            return firstSegment._fileContent.Substring(firstSegment.Start, lastSegment.Start - firstSegment.Start + lastSegment.Length);
        }
        public static string CombineSegmentText(this List<Segment> segments)
        {
            var sb = new StringBuilder(100);
            foreach (var segment in segments)
            {
                sb.Append(segment.Text);
            }
            return sb.ToString();
        }
    }
}

