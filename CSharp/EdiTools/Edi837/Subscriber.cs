using System.Collections.Generic;

namespace EdiTools.Edi837
{
    public class Subscriber
    {
        public List<Segment> Segments { get; set; } = new List<Segment>();
        public List<Claim> Claims { get; set; } = new List<Claim>();
        public List<Patient> Patients { get; set; } = new List<Patient>();
        public string Text
        {
            get
            {
                var firstSegment = this.Segments[0];
                var lastSegment = this.Segments[Segments.Count - 1];
                return firstSegment._fileContent.Substring(firstSegment.Start, lastSegment.Start - firstSegment.Start + lastSegment.Length);
            }
        }
        public static List<Subscriber> ParseSubscriberLoop(List<Segment> segments)
        {
            var loopName = string.Empty;
            var subscribers = new List<Subscriber>();

            Subscriber subscriber = null;
            List<Segment> claimSegments = null;
            List<Segment> patientSegments = null;

            foreach (Segment segment in segments)
            {
                if (segment.Name == "HL" && segment.Elements[3] == "22")
                {
                    loopName = "subscriber";

                    // if not first iteration
                    if (subscriber != null)
                    {
                        subscriber.Claims = Claim.ParseClaimLoop(claimSegments);
                        subscriber.Patients = Patient.ParsePatientLoop(patientSegments);
                    }

                    // reset for next iteration
                    subscriber = new Subscriber();
                    subscribers.Add(subscriber);
                    claimSegments = new List<Segment>();
                    patientSegments = new List<Segment>();
                }

                if (segment.Name == "HL" && segment.Elements[3] == "23")
                {
                    loopName = "patient";
                }

                // only add claims at subscriber level. Patient level claims are parsed in the Patient class
                if (segment.Name == "CLM" && loopName == "subscriber")
                {
                    loopName = "claim";
                }

                switch (loopName)
                {
                    case "subscriber":
                        subscriber.Segments.Add(segment);
                        break;
                    case "patient":
                        patientSegments.Add(segment);
                        break;
                    case "claim":
                        claimSegments.Add(segment);
                        break;
                }
            }

            // add claims and patients for final iteration
            subscriber.Claims = Claim.ParseClaimLoop(claimSegments);
            subscriber.Patients = Patient.ParsePatientLoop(patientSegments);

            return subscribers;
        }
    }
}
