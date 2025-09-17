using System.Collections.Generic;

namespace EdiTools.Edi837
{
    public static class FileOperations
    {
        static public List<Segment> GetEdi837SegmentsForPatientControlNumber(EdiFile ediFile, string patientControlNumber)
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
    }
}
