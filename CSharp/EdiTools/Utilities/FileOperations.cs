﻿using EdiTools.Edi837;
using System.Collections.Generic;
using System.Text;

namespace EdiTools.Utilities
{
    public static class FileOperations
    {
        public static IEnumerable<string> Split837ByClaims(EdiFile ediFile)
        {
            return Split837ByClaims(ediFile, null);
        }
        public static IEnumerable<string> Split837ByClaims(EdiFile ediFile, string patientControlNumber)
        {
            List<Segment> headerSegments; // segments before the billing provider loop
            List<Segment> billingProviderSegments;
            List<Segment> subscriberSegments;
            List<Segment> patientSegments;

            foreach (var fg in ediFile.FunctionalGroups)
            {
                foreach (var trx in fg.TransactionSets)
                {
                    headerSegments = new List<Segment>();
                    headerSegments.Add(ediFile.Interchange.ISA);
                    headerSegments.Add(fg.GS);

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
                                if (patientControlNumber == null || c.PatientControlNumber == patientControlNumber)
                                {
                                    var sb = new StringBuilder(16 * (headerSegments.Count + billingProviderSegments.Count + subscriberSegments.Count + c.Segments.Count));
                                    sb.Append(headerSegments.ToText());
                                    sb.Append(billingProviderSegments.ToText());
                                    sb.Append(subscriberSegments.ToText());
                                    sb.Append(c.Text);

                                    var seSegment = dh.Segments[dh.Segments.Count - 1];
                                    var seElements = seSegment.Elements;
                                    var transactionSetSegmentCount =
                                        headerSegments.Count - 2 // Don't count the ISA and GS
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

                                if (c.PatientControlNumber == patientControlNumber) 
                                { 
                                    yield break; 
                                }
                            }

                            foreach (var p in sub.Patients)
                            {
                                patientSegments = new List<Segment>();
                                patientSegments.AddRange(p.Segments);

                                // patient claims
                                foreach (var c in p.Claims)
                                {
                                    if (patientControlNumber == null || c.PatientControlNumber == patientControlNumber)
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

                                    if (c.PatientControlNumber == patientControlNumber) 
                                    { 
                                        yield break; 
                                    }
                                }
                            } // patient
                        } // subscriber
                    } // billing provider
                } // transaction set
            } // functional group
        }
    }
}
