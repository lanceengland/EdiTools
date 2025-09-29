using EdiTools.Edi837;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EdiTools 
{
    public sealed class  EdiFile
    {
        public EdiFile(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                throw new FileNotFoundException($"The file {path} is not found.");
            }

            this.Segments = new List<Segment>();

            var fi = new System.IO.FileInfo(path);
            this.FullPath = fi.FullName;
            this.FileName = fi.Name;
            this.DirectoryName = fi.DirectoryName;

            this.Text = File.ReadAllText(path);

            if (this.Text.Substring(0, 3) != "ISA")
            {
                throw new System.IO.InvalidDataException("No ISA segment found. Not an EDI file.");
            }

            this.Delimiter = new Delimiter
            {
                Element = this.Text[103],
                Component = this.Text[104],
                Segment = this.Text[105],
                LineTerminator = string.Empty // determined in next step
            };

            /* Determine if wrapped (no line breaks) or unwrapped (line breaks cr, lf, or crlf) */
            if (this.Text[106] == (char)13 || this.Text[106] == (char)10) /* carriage-return or new-line */
            {
                this.IsUnwrapped = true;

                if (this.Text[107] == (char)10) /* if the next char is new-line, assume cr/lf */
                {
                    this.Delimiter.LineTerminator = this.Text.Substring(106, 2);
                }
                else
                {
                    this.Delimiter.LineTerminator = this.Text[106].ToString();
                }
            }

            // build indexes
            var sb = new StringBuilder(100);
            var segment = new EdiTools.Segment(this.Text, this.Delimiter);
            int startOfSegment = 0;

            for (int i = 0; i < this.Text.Length; i++)
            {
                char c = this.Text[i];

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
                        segment.Length = this.Text.Length - startOfSegment;
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
                    segment = new EdiTools.Segment(this.Text, this.Delimiter);
                }
            }

            this.Interchange = Interchange.ParseInterchange(this.Segments);
            this.FunctionalGroups = FunctionalGroup.ParseFunctionalGroups(this.Segments);
        }
        public Delimiter Delimiter { get; private set; }
        public string Text
        {
            get;
            private set;
        }
        public bool IsUnwrapped { get; private set; }
        public string FileName { get; private set; }
        public string DirectoryName { get; private set; }
        public string FullPath { get; private set; }
        public List<EdiTools.Segment> Segments { get; private set; }        
        public EdiTools.Interchange Interchange { get; private set; }
        public List<FunctionalGroup> FunctionalGroups {  get; private set; }
                
        // todo: make a property?
        public string Unwrap()
        {
           if (this.IsUnwrapped)
            {
                return this.Text;
            }
           else
            {
                var sb = new StringBuilder();
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