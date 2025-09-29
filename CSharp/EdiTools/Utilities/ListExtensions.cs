using System.Collections.Generic;
using System.Text;

namespace EdiTools.Utilities
{
    public static class ListExtensions
    {
        public static string ToText(this List<Segment> segments)
        {
            var sb = new StringBuilder(segments.Count * 16); // 16 is the default
            foreach (var segment in segments)
            {
                sb.Append(segment.Text);
            }
            return sb.ToString();
        }
    }
}

