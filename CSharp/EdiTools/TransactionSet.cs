using EdiTools.Utilities;
using System.Collections.Generic;
using System.Text;

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
                return this.Segments.ToText();
            }
        }
        public string ID { get { return this.ST.Elements[1]; } }
        public string ControlNumber { get { return this.ST.Elements[2]; } }
        public string VersionIdentifier { get { return this.ST.Elements[3]; } }
        public List<Segment> Segments { get; private set; }
        public string Unwrap()
        {
            if (this.ST.Delimiter.LineTerminator.Length > 0)
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
