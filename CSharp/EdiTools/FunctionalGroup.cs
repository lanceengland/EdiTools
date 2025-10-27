using EdiTools.Utilities;
using System.Collections.Generic;
using System.Text;

namespace EdiTools
{
    public sealed class FunctionalGroup
    {
        private FunctionalGroup() {} // private constructor
        public Segment GS { get; private set; }
        public Segment GE { get; private set; }
        public string Text
        {
            get
            {
                return this.Segments.ToText();
            }
        }
        public List<Segment> Segments 
        {
            get;
            private set;
        }
        public List<TransactionSet> TransactionSets
        {
            get;
            private set;
        }
        static public List<FunctionalGroup> ParseFunctionalGroups(List<Segment> segments)
        {
            var functionalGroups = new List<FunctionalGroup>();

            FunctionalGroup fg = null;
            List<Segment> segs = null;

            foreach (var segment in segments)
            {
                if (segment.Name == "GS")
                {
                    fg = new FunctionalGroup();
                    fg.GS = segment;
                    segs = new List<Segment> ();
                }

                if (segment.Name == "GE")
                {
                    fg.GE = segment;
                    segs.Add(segment); // add here because resetting collection inside this block
                    fg.Segments = segs;
                    fg.TransactionSets = TransactionSet.ParseTransactionSets(segs);                    
                    functionalGroups.Add(fg);

                    // reset for next group
                    fg = null;
                    segs = null;
                }

                // segs is instantiated at GS
                if (segs != null)
                {
                    segs.Add(segment);
                }
            }

            return functionalGroups;
        }
        public string Unwrap()
        {
            if (this.Segments[0].Delimiter.LineTerminator.Length > 0)
            {
                return this.Text;
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
