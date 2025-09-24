using EdiTools.Edi837;
using System.Collections.Generic;

namespace EdiTools.Utilities
{
    public static class FileOperations
    {
        public static List<Segment> GetEdi837SegmentsForPatientControlNumber(EdiFile ediFile, string patientControlNumber)
        {
            bool isMatchFound = false;

            // used to capture the all levels when matched
            FunctionalGroup matchedFunctionalGroup = null;
            Edi837.DocumentHierarchy matchedDocumentHierarchy = null;
            BillingProvider matchedBillingProvider = null;
            Subscriber matchedSubscriber = null;
            Patient matchedPatient = null;
            Claim matchedClaim = null;

            foreach (var functionalGroup in ediFile.FunctionalGroups)
            {
                matchedFunctionalGroup = functionalGroup;
                foreach (var transactionSet in functionalGroup.TransactionSets)
                {
                    matchedDocumentHierarchy = new Edi837.DocumentHierarchy(transactionSet.Segments);
                    foreach (var billingProvider in matchedDocumentHierarchy.BillingProviders)
                    {
                        matchedBillingProvider = billingProvider;
                        foreach (var subscriber in billingProvider.Subscribers)
                        {
                            matchedSubscriber = subscriber;
                            foreach (var claim in subscriber.Claims)
                            {
                                matchedClaim = claim;
                                if (claim.PatientControlNumber == patientControlNumber)
                                {
                                    isMatchFound = true;
                                    break; // claim
                                }
                            }

                            // patient loop
                            foreach (var patient in subscriber.Patients)
                            {
                                matchedPatient = patient;
                                foreach (var claim in patient.Claims)
                                {
                                    matchedClaim = claim;
                                    if (claim.PatientControlNumber == patientControlNumber)
                                    {
                                        isMatchFound = true;
                                        break; // claims
                                    }
                                }
                                if (isMatchFound) break; // patient
                                matchedPatient = null; // reset to indicate inside patient or sub loop on match
                            }
                            if (isMatchFound) break; // subscriber      
                        }
                        if (isMatchFound) break; // billing provider
                    }
                    if (isMatchFound) break; // transaction set
                }
                if (isMatchFound) break; // functional group
            }

            if (isMatchFound)
            {
                var segments = new List<Segment>();
                segments.Add(ediFile.Interchange.ISA);
                segments.Add(matchedFunctionalGroup.GS);

                // Get all segments before the matched billing provider and also not the trailing 'SE' segment
                segments.AddRange(matchedDocumentHierarchy.Segments.GetRange(0, matchedDocumentHierarchy.Segments.Count - 1));

                segments.AddRange(matchedBillingProvider.Segments);
                segments.AddRange(matchedSubscriber.Segments);

                // determine if the matched claim is in the patient loop or subscriber loop
                if (matchedPatient != null)
                {
                    segments.AddRange(matchedPatient.Segments);
                    segments.AddRange(matchedClaim.Segments);
                }
                else
                {
                    segments.AddRange(matchedClaim.Segments);
                }

                // get last transaction set segment 'SE'
                segments.Add(matchedDocumentHierarchy.Segments[matchedDocumentHierarchy.Segments.Count - 1]);
                segments.Add(matchedFunctionalGroup.GE);
                segments.Add(ediFile.Interchange.IEA);
                return segments;
            }
            else
            {
                return null;
            }
        }
        public static IEnumerable<List<Segment>> SplitEdi837ByPatientControlNumber(EdiFile ediFile)
        {
            // todo: isa,gs, trx envelope & claim split logic
            foreach(var fg in ediFile.FunctionalGroups)
            {
                foreach(var trx in fg.TransactionSets)
                {
                    var dh = new Edi837.DocumentHierarchy(trx.Segments);
                    foreach(var bp in dh.BillingProviders)
                    {
                        foreach(var sub in bp.Subscribers)
                        {
                            // subscriber claims
                            foreach(var c in sub.Claims)
                            {

                            }

                            // patient claims
                            foreach(var p in sub.Patients)
                            {
                                foreach (var c in p.Claims)
                                {

                                }
                            }
                        }
                    }                    

                    yield return trx.Segments;
                }
            }
        }
        public static IEnumerable<List<Segment>> Test()
        {
            var fileContents = "file contents";
            Delimiter del = new Delimiter();
            Segment seg = null;
            List<Segment> innerList = null;

            for (int i = 0; i < 3; i++)
            {
                innerList = new List<Segment>();
                fileContents = fileContents + i.ToString();

                // represents 'split' transaction set
                for (int k = 0; k < 4; k++)
                {
                    seg = new Segment(fileContents, del);
                    seg.Start = 0;
                    seg.Length = 4;
                    innerList.Add(seg);
                }
                yield return innerList;
            }
        }        
    }
}
