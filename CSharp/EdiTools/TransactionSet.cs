﻿using System.Collections.Generic;

namespace EdiTools
{
    public class TransactionSet
    {
        internal TransactionSet() {}
        internal TransactionSet(TransactionSet other)
        {
            this.Segments = other.Segments;
            this.ST = other.ST;
            this.SE = other.SE;
        }
        public Segment ST { get; private set; }
        public Segment SE { get; private set; }
        public string Text
        {
            get
            {
                return this.ST._fileContent.Substring(this.ST.Start, this.SE.Start - this.ST.Start + this.SE.Length);
            }
        }
        public string ID { get { return this.ST.Elements[1]; } }
        public string ControlNumber { get { return this.ST.Elements[2]; } }
        public string VersionIdentifier { get { return this.ST.Elements[3]; } }
        public List<Segment> Segments { get; private set; }
        static public List<TransactionSet> ParseTransactionSets(List<Segment> segments)
        {
            var transactionSets = new List<TransactionSet>();
            TransactionSet trx = null;
            List<Segment> segs = null;

            foreach (var segment in segments)
            {
                if (segment.Name == "ST")
                {
                    trx = new TransactionSet();
                    trx.ST = segment;
                    segs = new List<Segment>();
                }

                if (segment.Name == "SE")
                {
                    trx.SE = segment;
                    segs.Add(segment); // add here because resetting collection inside this block
                    trx.Segments = segs;
                    transactionSets.Add(trx);

                    // reset for next trx set
                    trx = null;
                    segs = null;
                }

                // segs is instantiated at ST
                if (segs != null)
                {
                    segs.Add(segment);
                }
            }
            return transactionSets;
        }
    }
}
