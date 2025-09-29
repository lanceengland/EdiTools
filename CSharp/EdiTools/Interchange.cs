using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdiTools.Utilities;

namespace EdiTools
{
    public sealed class Interchange
    {
        public Segment ISA { get; private set; }
        public Segment IEA { get; private set; }
        public string Sender { get; private set; }
        public string Receiver { get; private set; }
        public string InterchangeControlNumber { get; private set; }
        public DateTime InterchangeDate { get; private set; }
        private Interchange() {}
        public string Text
        {
            get
            {
                // interchange is essentially the entire file
                return this.Segments.ToText();
            }
        }
        public List<Segment> Segments { get; private set; }
        public static Interchange ParseInterchange(List<Segment> segments)
        {
            var interchange = new Interchange();
            interchange.Segments = segments;

            if (segments[0].Name == "ISA")
            {
                interchange.ISA = segments[0];
                var elements = interchange.ISA.Elements;

                interchange.Sender = elements[6].Trim();
                interchange.Receiver = elements[8].Trim();
                interchange.InterchangeControlNumber = elements[13].Trim();

                DateTime.TryParseExact(elements[9] + elements[10], "yyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt);
                interchange.InterchangeDate = dt;
            }

            if (segments[segments.Count - 1].Name == "IEA")
            {
                interchange.IEA = segments[segments.Count - 1];
            }

            return interchange;
        }
    }
}
