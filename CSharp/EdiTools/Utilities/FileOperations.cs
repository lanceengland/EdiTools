using EdiTools.Edi837;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

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
            foreach(var fg in ediFile.FunctionalGroups)
            {
                var returnVal = new List<Segment>();
                returnVal.Add(ediFile.Interchange.ISA);
                returnVal.Add(fg.GS);

                foreach (var trx in fg.TransactionSets)
                {
                    var dh = new Edi837.DocumentHierarchy(trx.Segments);

                    returnVal.AddRange(dh.Segments.GetRange(0, dh.Segments.Count - 1)); // omit SE here, add it below
                    foreach(var bp in dh.BillingProviders)
                    {
                        returnVal.AddRange(bp.Segments);
                        foreach(var sub in bp.Subscribers)
                        {
                            returnVal.AddRange(sub.Segments);

                            // subscriber claims
                            foreach(var c in sub.Claims)
                            {
                                returnVal.AddRange(c.Segments);
                                returnVal.Add(dh.Segments[dh.Segments.Count - 1]); // se
                                returnVal.Add(fg.GE);
                                returnVal.Add(ediFile.Interchange.IEA);                                
                                yield return returnVal;
                            }

                            // patient claims
                            foreach(var p in sub.Patients)
                            {
                                returnVal.AddRange(p.Segments);                
                                foreach (var c in p.Claims)
                                {
                                    returnVal.AddRange(c.Segments);
                                    returnVal.Add(dh.Segments[dh.Segments.Count - 1]); // se
                                    returnVal.Add(fg.GE);
                                    returnVal.Add(ediFile.Interchange.IEA);
                                    yield return returnVal;
                                }
                            } // patient
                        } // subscriber
                    } // billing provider
                } // transaction set
            } // functional group
        }    
    }
}
