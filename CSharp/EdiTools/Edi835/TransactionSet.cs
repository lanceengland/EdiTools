using System;

namespace EdiTools.Edi835
{
    public sealed class TransactionSet : EdiTools.TransactionSet
    {
        public TransactionSet() : base()
        {
            if (this.ID != "835") { throw new ArgumentException("Expected an EDI transaction set of type 835."); }

            this.init();
        }
        public TransactionSet(EdiTools.TransactionSet other) : base(other)
        {
            if (this.ID != "835") { throw new ArgumentException("Expected an EDI transaction set of type 835."); }

            this.init();
        }
        private void init()
        {
            // get convenience 835 properties
            foreach (var segment in this.Segments)
            {
                //string[] elements;
                if (segment.Name == "BPR")
                {
                    System.Decimal.TryParse(segment.Elements[2], out decimal _totalActualProviderPaymentAmount);
                    this.TotalActualProviderPaymentAmount = _totalActualProviderPaymentAmount;

                    this.SenderBankAccountNumber = segment.Elements[9];
                }
                if (segment.Name == "TRN")
                {
                    this.CheckorEFTTraceNumber = segment.Elements[2];
                }
                if (segment.Name == "N1")
                {
                    if (segment.Elements[1] == "PR")
                    {
                        this.Payer = segment.Elements[2];
                    }
                    if (segment.Elements[1] == "PE")
                    {
                        this.Payee = segment.Elements[2];
                        break; // last property to extract so exit loop
                    }
                }
            }
        }

        #region properties
        public string Payer { get; private set; }
        public string Payee { get; private set; }
        public string SenderBankAccountNumber { get; private set; }
        public string CheckorEFTTraceNumber { get; private set; }
        public decimal TotalActualProviderPaymentAmount { get; private set; }
        #endregion
    }
}
