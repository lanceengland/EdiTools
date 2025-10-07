using EdiTools.Edi837;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EdiTools 
{
    /*
     * todo: default constructor
     * init from file (path) or by contents
     */
    public sealed class  EdiFile
    {
        public EdiFile() { }
        public EdiFile(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                throw new FileNotFoundException($"The file {path} is not found.");
            }

            this.LoadFromString(File.ReadAllText(path));

            // set file properties
            var fi = new System.IO.FileInfo(path);
            this.FullPath = fi.FullName;
            this.FileName = fi.Name;
            this.DirectoryName = fi.DirectoryName;
        }
                public void LoadFromString(string ediContent)
        {
            // reset since not loading from a file
            this.FullPath = string.Empty;
            this.FileName = string.Empty;
            this.DirectoryName = string.Empty;

            this.OriginalText = ediContent;
            this.Segments = new List<Segment>();

            if (this.OriginalText.Substring(0, 3) != "ISA")
            {
                throw new System.IO.InvalidDataException("No ISA segment found. Not an EDI file.");
            }

            this.Delimiter = new Delimiter
            {
                Element = this.OriginalText[103],
                Component = this.OriginalText[104],
                Segment = this.OriginalText[105],
                LineTerminator = string.Empty // determined in next step
            };

            /* Determine if wrapped (no line breaks) or unwrapped (line breaks cr, lf, or crlf) */
            if (this.OriginalText[106] == (char)13 || this.OriginalText[106] == (char)10) /* carriage-return or new-line */
            {
                this.IsUnwrapped = true;

                if (this.OriginalText[107] == (char)10) /* if the next char is new-line, assume cr/lf */
                {
                    this.Delimiter.LineTerminator = this.OriginalText.Substring(106, 2);
                }
                else
                {
                    this.Delimiter.LineTerminator = this.OriginalText[106].ToString();
                }
            }

            // build indexes
            var sb = new StringBuilder(100);
            var segment = new EdiTools.Segment(this.OriginalText, this.Delimiter);
            int startOfSegment = 0;

            for (int i = 0; i < this.OriginalText.Length; i++)
            {
                char c = this.OriginalText[i];

                // skip eol chars
                if (c == '\r' || c == '\n')
                {
                    startOfSegment = i + 1; // to move past the EOL char
                    continue;
                }

                sb.Append(c);

                if (c == this.Delimiter.Segment)
                {
                    var elements = sb.ToString().Split(this.Delimiter.Element);
                    segment.Name = elements[0];
                    segment.Start = startOfSegment;
                    if (segment.Name == "IEA")
                    {
                        // accomodates present or missing line-break after last segment (IEA)
                        segment.Length = this.OriginalText.Length - startOfSegment;
                    }
                    else
                    {
                        // accomodates line-breaks (if present)
                        segment.Length = sb.Length + this.Delimiter.LineTerminator.Length;
                    }

                    // reset for next segment
                    startOfSegment = i + 1;

                    sb.Clear();
                    this.Segments.Add(segment);
                    segment = new EdiTools.Segment(this.OriginalText, this.Delimiter);
                }
            }

            this.Interchange = Interchange.ParseInterchange(this.Segments);
            this.FunctionalGroups = FunctionalGroup.ParseFunctionalGroups(this.Segments);
        }
        public Delimiter Delimiter { get; private set; }
        /// <summary>
        /// This will return the EDI content excluding any changes via Segment.Elements (i.e. the original text)
        /// </summary>
        public string OriginalText
        {
            get;
            private set;
        }

        /// <summary>
        /// This will return the EDI content including any changes via Segment.Elements
        /// </summary>
        public string Text
        {
            get
            {
                var sb = new StringBuilder(16 * this.Segments.Count);
                foreach (var idx in this.Segments)
                {
                    sb.Append(idx.Text);
                }
                return sb.ToString();
            }
        }
        public bool IsUnwrapped { get; private set; }
        public string FileName { get; private set; }
        public string DirectoryName { get; private set; }
        public string FullPath { get; private set; }
        public List<EdiTools.Segment> Segments { get; private set; }        
        public EdiTools.Interchange Interchange { get; private set; }
        public List<FunctionalGroup> FunctionalGroups {  get; private set; }
        public string Unwrap()
        {
           if (this.IsUnwrapped)
            {
                return this.OriginalText;
            }
           else
            {
                var sb = new StringBuilder(16 * this.Segments.Count);
                foreach (var idx in this.Segments)
                {
                    sb.Append(idx.Text);
                    sb.Append(System.Environment.NewLine);
                }
                return sb.ToString();
            }
        }
    }
}