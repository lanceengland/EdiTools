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
            get
            {
                switch (this.VersionIdentifier)
                {
                    case "005010X222A1":
                        return "P";
                    case "005010X222A2":
                        return "I";
                    case "005010X2224A2":
                        return "D";
                    default:
                        return "Unknown";
                }
            }
        }
    }
}
