using System;

namespace EdiTools.Edi837
{
    public sealed class TransactionSet : EdiTools.TransactionSet
    {
        public TransactionSet() : base()
        {
            if (this.ID != "837") { throw new ArgumentException("Expected an EDI transaction set of type 837."); }
        }
        public TransactionSet(EdiTools.TransactionSet other) : base(other)
        {
            if (this.ID != "837") { throw new ArgumentException("Expected an EDI transaction set of type 837."); }
        }
        public string VersionShortId
        {
            /*
             P: 005010X222A1 Standards Approved for Publication by ASC X12 Procedures Review Board through October 2003 
             I: 005010X223A2 Standards Approved for Publication by ASC X12 Procedures Review Board through October 2003 
             D: 005010X224A2 Standards Approved for Publication by ASC X12 Procedures Review Board through October 2003 
             */

            get
            {
                switch (this.VersionIdentifier)
                {
                    case "005010X222A1":
                        return "P";

                    case "005010X223A2":
                        return "I";

                    case "005010X224A2":
                        return "D";

                    default:
                        return "Unknown";
                }
            }
        }
    }
}
