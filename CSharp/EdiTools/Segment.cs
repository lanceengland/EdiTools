using System.Runtime.InteropServices;

namespace EdiTools
{
    public sealed class Segment
    {
        public Segment(string contents, Delimiter delimiter)
        {
            _fileContent = contents;
            this.Delimiter = delimiter;
        }        
        public string Name { get; internal set; }
        public int Start { get; internal set; }
        /// <summary>
        /// The Length will include any line-endings (if present)
        /// </summary>
        public int Length { get; internal set; }

        /// <summary>
        /// Returns the entire text for the segment including line-endings (if present)
        /// </summary>
        public string Text
        {
            get {  return _fileContent.Substring(this.Start, this.Length); }
        }
        public string[] Elements
        {
            get 
            {
                // todo: strip segmenet delimiter and line-endings from last element
                var elements = this.Text.Split(new char[] { this.Delimiter.Element });
                elements[elements.Length - 1] = elements[elements.Length - 1].Replace(this.Delimiter.Segment.ToString(), string.Empty).Replace(this.Delimiter.LineTerminator, string.Empty);
                return elements;
            }
        }

        // todo: iterator for elements

        public Delimiter Delimiter
        {
            get;
            private set;
        }
        internal string _fileContent;
    }
}
