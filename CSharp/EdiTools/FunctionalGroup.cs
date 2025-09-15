using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiTools
{
    public sealed class FunctionalGroup
    {

        private FunctionalGroup() 
        {
        }
        public Segment GS { get; private set; }
        public Segment GE { get; private set; }
        public string Text
        {
            get
            {
                return this.GS._fileContent.Substring(this.GS.Start, this.GE.Start - this.GS.Start + this.GE.Length);
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
    }
}
