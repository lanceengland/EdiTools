using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiTools.Edi837
{
    public class Patient
    {
        public List<Segment> Segments { get; set; } = new List<Segment>();
        public string Text
        {
            get
            {
                return this.Segments.GetTextFromFirstToLastSegment();
            }
        }
        public List<Claim> Claims { get; set; } = new List<Claim>();
        public static List<Patient> ParsePatientLoop(List<Segment> segments)
        {
            var loopName = string.Empty;
            var patients = new List<Patient>();

            Patient patient = null;
            List<Segment> claimSegments = null;

            foreach (Segment segment in segments)
            {
                // patient
                if (segment.Name == "HL")
                {
                    loopName = "patient";

                    if (patient != null)
                    {
                        patient.Claims = Claim.ParseClaimLoop(claimSegments);
                    }

                    // reset for next iteration
                    patient = new Patient();
                    patients.Add(patient);
                    claimSegments = new List<Segment>();
                }

                if (segment.Name == "CLM")
                {
                    loopName = "claim";
                }

                if (loopName == "patient")
                {
                    patient.Segments.Add(segment);
                }
                else
                {
                    claimSegments.Add(segment);
                }                
            }

            // add claims from final iteration
            if (claimSegments != null)
            {
                patient.Claims = Claim.ParseClaimLoop(claimSegments);
            }

            return patients;
        }
    }
}
