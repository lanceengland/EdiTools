using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiTools.Utilities
{
    public static class DataDeidentification
    {
        //todo: take text string, convert to EdiFile, etc then return string
        //todo: therefore, need to be able to init EdiFile via content string
        public static string Deidentify837(string ediText)
        {
            var f = new EdiFile();
            f.LoadFromString(ediText);

            foreach (var segment in f.Segments)
            {
                var elements = segment.Elements;

                switch (segment.Name)
                {
                    case "NM1":
                        // 1000A/B submitter/receiver (only if individual)
                        if (elements[2] == "1")
                        {
                            switch (elements[1])
                            {
                                case "41": // submitter
                                    elements[3] = "SUBMITTER";
                                    elements[4] = string.Empty;
                                    elements[5] = string.Empty;
                                    elements[9] = "SUBMITTER-ID";
                                    break;

                                case "40": // receiver
                                    elements[3] = "RECEIVER";
                                    elements[4] = string.Empty;
                                    elements[5] = string.Empty;
                                    elements[9] = "RECEIVER-ID";
                                    break;
                            }
                            segment.Elements = elements; // write back to segment
                        }
                        break;
                    
                    case "PER":
                        elements[2] = "Contact Name";
                        if (elements[3] == "EM")
                        {
                            elements[4] = "email@email.com";
                        }
                        else
                        {
                            elements[4] = "5555555555";
                        }
                        segment.Elements = elements; // write back to segment
                        break;

                    case "N3":
                        break;

                    case "N4":
                        break;



                    case "REF":
                        break;

                    case "CLM":
                        break;

                    case "DMG":
                        break;


                } // switch
            } // foreach

            return f.Text;
        }

    } // class
} // namespace