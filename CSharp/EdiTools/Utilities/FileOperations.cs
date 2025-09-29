using EdiTools.Edi837;
using System.Collections.Generic;
using System.Text;

namespace EdiTools.Utilities
{
    public static class FileOperations
    {
        //TODO: DELETE commented code
        //public static List<Segment> GetEdi837SegmentsForPatientControlNumber(EdiFile ediFile, string patientControlNumber)
        //{
        //    bool isMatchFound = false;

        //    // used to capture the all levels when matched
        //    FunctionalGroup matchedFunctionalGroup = null;
        //    Edi837.DocumentHierarchy matchedDocumentHierarchy = null;
        //    BillingProvider matchedBillingProvider = null;
        //    Subscriber matchedSubscriber = null;
        //    Patient matchedPatient = null;
        //    Claim matchedClaim = null;

        //    foreach (var functionalGroup in ediFile.FunctionalGroups)
        //    {
        //        matchedFunctionalGroup = functionalGroup;
        //        foreach (var transactionSet in functionalGroup.TransactionSets)
        //        {
        //            matchedDocumentHierarchy = new Edi837.DocumentHierarchy(transactionSet.Segments);
        //            foreach (var billingProvider in matchedDocumentHierarchy.BillingProviders)
        //            {
        //                matchedBillingProvider = billingProvider;
        //                foreach (var subscriber in billingProvider.Subscribers)
        //                {
        //                    matchedSubscriber = subscriber;
        //                    foreach (var claim in subscriber.Claims)
        //                    {
        //                        matchedClaim = claim;
        //                        if (claim.PatientControlNumber == patientControlNumber)
        //                        {
        //                            isMatchFound = true;
        //                            break; // claim
        //                        }
        //                    }

        //                    // patient loop
        //                    foreach (var patient in subscriber.Patients)
        //                    {
        //                        matchedPatient = patient;
        //                        foreach (var claim in patient.Claims)
        //                        {
        //                            matchedClaim = claim;
        //                            if (claim.PatientControlNumber == patientControlNumber)
        //                            {
        //                                isMatchFound = true;
        //                                break; // claims
        //                            }
        //                        }
        //                        if (isMatchFound) break; // patient
        //                        matchedPatient = null; // reset to indicate inside patient or sub loop on match
        //                    }
        //                    if (isMatchFound) break; // subscriber      
        //                }
        //                if (isMatchFound) break; // billing provider
        //            }
        //            if (isMatchFound) break; // transaction set
        //        }
        //        if (isMatchFound) break; // functional group
        //    }

        //    if (isMatchFound)
        //    {
        //        var segments = new List<Segment>();
        //        segments.Add(ediFile.Interchange.ISA);
        //        segments.Add(matchedFunctionalGroup.GS);

        //        // Get all segments before the matched billing provider and also not the trailing 'SE' segment
        //        segments.AddRange(matchedDocumentHierarchy.Segments.GetRange(0, matchedDocumentHierarchy.Segments.Count - 1));

        //        segments.AddRange(matchedBillingProvider.Segments);
        //        segments.AddRange(matchedSubscriber.Segments);

        //        // determine if the matched claim is in the patient loop or subscriber loop
        //        if (matchedPatient != null)
        //        {
        //            segments.AddRange(matchedPatient.Segments);
        //            segments.AddRange(matchedClaim.Segments);
        //        }
        //        else
        //        {
        //            segments.AddRange(matchedClaim.Segments);
        //        }

        //        // get last transaction set segment 'SE'
        //        segments.Add(matchedDocumentHierarchy.Segments[matchedDocumentHierarchy.Segments.Count - 1]);
        //        segments.Add(matchedFunctionalGroup.GE);
        //        segments.Add(ediFile.Interchange.IEA);
        //        return segments;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //// todo: I cant return mutable segments
        //public static IEnumerable<List<Segment>> SplitEdi837ByPatientControlNumber(EdiFile ediFile)
        //{
        //    foreach(var fg in ediFile.FunctionalGroups)
        //    {
        //        var returnVal = new List<Segment>();
        //        returnVal.Add(ediFile.Interchange.ISA);
        //        returnVal.Add(fg.GS);

        //        foreach (var trx in fg.TransactionSets)
        //        {
        //            var dh = new Edi837.DocumentHierarchy(trx.Segments);

        //            returnVal.AddRange(dh.Segments.GetRange(0, dh.Segments.Count - 1)); // omit SE here, add it below

        //            var countOfSegmentsBeforeBillingProviders = returnVal.Count;
        //            foreach(var bp in dh.BillingProviders)
        //            {
        //                returnVal.AddRange(bp.Segments);
        //                foreach(var sub in bp.Subscribers)
        //                {
        //                    returnVal.AddRange(sub.Segments);
        //                    var countOfSegmentsBeforeClaims = returnVal.Count;

        //                    // subscriber claims
        //                    foreach(var c in sub.Claims)
        //                    {
        //                        returnVal.AddRange(c.Segments);
        //                        returnVal.Add(dh.Segments[dh.Segments.Count - 1]); // se
        //                        returnVal.Add(fg.GE);
        //                        returnVal.Add(ediFile.Interchange.IEA);                                
        //                        yield return returnVal;

        //                        // remove claim segments for next iteration
        //                        returnVal.RemoveRange(countOfSegmentsBeforeClaims, returnVal.Count - countOfSegmentsBeforeClaims - 1);
        //                    }

        //                    // todo: clear subscriber claims 
        //                    // patient claims
        //                    foreach(var p in sub.Patients)
        //                    {
        //                        countOfSegmentsBeforeClaims = returnVal.Count;
        //                        returnVal.AddRange(p.Segments);

        //                        foreach (var c in p.Claims)
        //                        {
        //                            returnVal.AddRange(c.Segments);
        //                            returnVal.Add(dh.Segments[dh.Segments.Count - 1]); // se
        //                            returnVal.Add(fg.GE);
        //                            returnVal.Add(ediFile.Interchange.IEA);
        //                            yield return returnVal;

        //                            // remove claim segments for next iteration
        //                            returnVal.RemoveRange(countOfSegmentsBeforeClaims, returnVal.Count - countOfSegmentsBeforeClaims - 1);
        //                        }
        //                    } // patient
        //                } // subscriber
        //            } // billing provider
        //        } // transaction set
        //    } // functional group
        //} // method
        
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
                                    headerSegments.Count
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
                                        headerSegments.Count
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
