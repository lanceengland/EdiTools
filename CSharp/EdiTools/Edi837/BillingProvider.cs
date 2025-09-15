using System.Collections.Generic;

namespace EdiTools.Edi837
{
    public class BillingProvider
    {
        public List<Segment> Segments { get; set; } = new List<Segment>();
        public List<Subscriber> Subscribers { get; set; }
        public string Text
        {
            get
            {
                var firstSegment = this.Segments[0];
                var lastSegment = this.Segments[Segments.Count - 1];
                return firstSegment._fileContent.Substring(firstSegment.Start, lastSegment.Start - firstSegment.Start + lastSegment.Length);
            }
        }
        internal static List<BillingProvider> ParseBillingProviderLoop(List<Segment> segments)
        {
            var hierarchicalLevel = string.Empty;
            var billingProviders = new List<BillingProvider>();

            BillingProvider billingProvider = null;
            List<Segment> subscriberSegments = null;

            foreach (Segment segment in segments)
            {
                // billing provider
                if (segment.Name == "HL" && segment.Elements[3] == "20")
                {
                    hierarchicalLevel = "20";

                    // skip the first iteration (we collect segments and add them next iteration
                    if (billingProvider != null)
                    {
                        billingProvider.Subscribers = Subscriber.ParseSubscriberLoop(subscriberSegments);
                    }

                    // reset for next billing provider iteration
                    billingProvider = new BillingProvider();
                    billingProviders.Add(billingProvider);
                    subscriberSegments = new List<Segment>();
                }

                // subscriber
                if (segment.Name == "HL" && segment.Elements[3] == "22")
                {
                    hierarchicalLevel = "22";
                }

                // add segment to appropriate collection
                switch (hierarchicalLevel)
                {
                    case "20":
                        billingProvider.Segments.Add(segment);
                        break;
                    default:
                        subscriberSegments.Add(segment);
                        break;
                }
            }
            // add subscribers for last iteration
            billingProvider.Subscribers = Subscriber.ParseSubscriberLoop(subscriberSegments);
            return billingProviders;
        }
    }
}
