using System.Runtime.InteropServices;
using System.Text;

namespace EdiTools
{
    public sealed class Segment
    {
        public Segment(string ediText, Delimiter delimiter)
        {
            _ediText = ediText;
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
            get 
            {
                if (this._elements != null)
                {
                    var sb = new StringBuilder();
                    int oneBasedCount = 1; // used to detect not appending the element delimiter to the last element
                    foreach(var element in this._elements)
                    {
                        sb.Append(element);
                        if (oneBasedCount < this._elements.Length)
                        {
                            sb.Append(this.Delimiter.Element);
                        }
                        oneBasedCount++;
                    }
                    
                    sb.Append(this.Delimiter.Segment);
                    sb.Append(this.Delimiter.LineTerminator);
                    return sb.ToString();
                }
                    
                return _ediText.Substring(this.Start, this.Length); 
            }
        }
        public string OriginalText
        {
            get 
            {
                return this._ediText.Substring(this.Start, this.Length);
            }
        }
        public string[] Elements
        {
            get 
            {
                if (this._elements != null)
                {
                    return this._elements;
                }

                var elements = this.Text.Split(new char[] { this.Delimiter.Element });

                // strip line-endings off last element
                var lastIndex = elements.Length - 1;
                elements[lastIndex] = elements[lastIndex].Substring(0, elements[lastIndex].Length - 1 /* segment delimiter  */ - this.Delimiter.LineTerminator.Length);
                return elements;
            }

            internal set
            {
                this._elements = value;
            }
        }
        public Delimiter Delimiter
        {
            get;
            private set;
        }

        private string _ediText;
        private string[] _elements;
    }
}
