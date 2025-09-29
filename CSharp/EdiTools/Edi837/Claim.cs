using EdiTools.Utilities;
using System.Collections.Generic;

namespace EdiTools.Edi837
{
    public class Claim
    {
        public List<Segment> Segments { get; set; } = new List<Segment>();
        public string PatientControlNumber
        {
            get
            {
                return this.Segments[0].Elements[1]; // CLM01
            }
        }
        public string Text
        {
            get
            {
                return this.Segments.ToText();
            }
        }
        public static List<Claim> ParseClaimLoop(List<Segment> segments)
        {
            var claims = new List<Claim>();
            Claim claim = null;

            foreach (var segment in segments)
            {
                if (segment.Name == "CLM")
                {
                    if (claim != null)
                    {
                        claims.Add(claim);
                    }
                    claim = new Claim();
                    claims.Add(claim);
                }

                claim.Segments.Add(segment);
            }
            return claims;
        }
    }
}
