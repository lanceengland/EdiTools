using EdiTools.Edi837;
using System.Collections.Generic;
using System.Text;

namespace EdiTools.Utilities
{
    public static class FileOperations
    {      
        public static string Extract837ForPatientControlNumber(string patientControlNumber)
        {
            return "todo";
        }
        public static IEnumerable<string> Split837ByClaims(EdiFile ediFile)
        {
            List<Segment> headerSegments = new List<Segment>(); // segments before the billing provider loop
            List<Segment> billingProviderSegments = null;
            List<Segment> subscriberSegments = null;
            List<Segment> patientSegments = null;

            foreach (var fg in ediFile.FunctionalGroups)
            {
                headerSegments.Add(ediFile.Interchange.ISA);
                headerSegments.Add(fg.GS);

                foreach (var trx in fg.TransactionSets)
                {
                    var dh = new Edi837.DocumentHierarchy(trx.Segments);
                    headerSegments.AddRange(dh.Segments.GetRange(0, dh.Segments.Count - 1)); // omit SE here, add it below

                    foreach (var bp in dh.BillingProviders)
                    {
                        billingProviderSegments = new List<Segment>();
                        billingProviderSegments.AddRange(bp.Segments);

                        foreach (var sub in bp.Subscribers)
                        {
                            subscriberSegments = new List<Segment>();
                            subscriberSegments.AddRange(sub.Segments);

                            // subscriber claims
                            foreach (var c in sub.Claims)
                            {
                                var sb = new StringBuilder(16 * (headerSegments.Count + billingProviderSegments.Count + subscriberSegments.Count + c.Segments.Count));
                                sb.Append(headerSegments.ToText());
                                sb.Append(billingProviderSegments.ToText());
                                sb.Append(subscriberSegments.ToText());
                                sb.Append(c.Text);

                                var seSegment = dh.Segments[dh.Segments.Count - 1];
                                var seElements = seSegment.Elements;
                                var transactionSetSegmentCount =
                                    headerSegments.Count -  2 // Don't count the ISA and GS
                                    + billingProviderSegments.Count
                                    + subscriberSegments.Count
                                    + c.Segments.Count
                                    + 1; // include the SE segment

                                seElements[1] = transactionSetSegmentCount.ToString(); // update SE01 (segment count)
                                seSegment.Elements = seElements; // write back to segment
                                sb.Append(seSegment.Text); // SE

                                var geElements = fg.GE.Elements;
                                geElements[1] = "1"; // only 1 transactio set per functional group
                                fg.GE.Elements = geElements;
                                sb.Append(fg.GE.Text);
                                sb.Append(ediFile.Interchange.IEA.Text);

                                yield return sb.ToString();
                            }

                            foreach (var p in sub.Patients)
                            {
                                patientSegments = new List<Segment>();
                                patientSegments.AddRange(p.Segments);

                                // patient claims
                                foreach (var c in p.Claims)
                                {
                                    var sb = new StringBuilder(16 * (headerSegments.Count + billingProviderSegments.Count + subscriberSegments.Count + patientSegments.Count + c.Segments.Count));
                                    sb.Append(headerSegments.ToText());
                                    sb.Append(billingProviderSegments.ToText());
                                    sb.Append(subscriberSegments.ToText());
                                    sb.Append(p.Segments.ToText());
                                    sb.Append(c.Text);

                                    var seSegment = dh.Segments[dh.Segments.Count - 1];
                                    var seElements = seSegment.Elements;
                                    var transactionSetSegmentCount =
                                        headerSegments.Count - 2 // Don't count the ISA and GS
                                        + billingProviderSegments.Count
                                        + subscriberSegments.Count
                                        + patientSegments.Count
                                        + c.Segments.Count
                                        + 1; // include the SE segment

                                    seElements[1] = transactionSetSegmentCount.ToString(); // update SE01 (segment count)
                                    seSegment.Elements = seElements; // write back to segment
                                    sb.Append(seSegment.Text); // SE

                                    var geElements = fg.GE.Elements;
                                    geElements[1] = "1"; // only 1 transactio set per functional group
                                    fg.GE.Elements = geElements;
                                    sb.Append(fg.GE.Text);
                                    sb.Append(ediFile.Interchange.IEA.Text);

                                    yield return sb.ToString();
                                }
                            } // patient
                        } // subscriber
                    } // billing provider
                } // transaction set
            } // functional group
        }
    }
}
