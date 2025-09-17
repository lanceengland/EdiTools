using System.Collections.Generic;

// todo: better namespace
namespace EdiTools.Edi837
{
    public class DocumentHierarchy
    {
        public DocumentHierarchy(List<Segment> segments)
        {
            // 20 billing provider - 2000A loop
            // 22 subscriber - 2000B loop
            // 23 patient - 2000C loop
            // end of sub/pat loops is HL or SE

            var billingProviderSegments = new List<Segment>();
            var inBillingProviderLoop = false;

            foreach (Segment segment in segments)
            {
                switch (segment.Name)
                {
                    case "HL":
                        inBillingProviderLoop = true;
                        break;
                    case "SE":
                        inBillingProviderLoop = false;
                        break;
                }

                if (inBillingProviderLoop)
                {
                    billingProviderSegments.Add(segment);
                }
                else
                {
                    this.Segments.Add(segment);
                }
            }

            this.BillingProviders = BillingProvider.ParseBillingProviderLoop(billingProviderSegments);
        }
        public List<Segment> Segments { get; set; } = new List<Segment>();
        public List<BillingProvider> BillingProviders { get; set; } = new List<BillingProvider>();
        public string Text
        {
            get
            {
                return this.Segments.GetTextFromFirstToLastSegment();
            }
        }
    }
}
