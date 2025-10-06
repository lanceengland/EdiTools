using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiTools.Utilities
{
    internal static class DataDeidentification
    {
        internal static void Deidentify837(List<Segment> segments)
        {
            foreach (var segment in segments)
            {
                switch (segment.Name)
                {
                    case "NM1":
                        break;

                    case "N3":
                        break;

                    case "N4":
                        break;

                    case "PER":
                        break;

                    case "REF":
                        break;

                    case "CLM":
                        break;


                }
            }
        }

    }
}