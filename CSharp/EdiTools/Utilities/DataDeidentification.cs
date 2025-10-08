using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiTools.Utilities
{
    public static class DataDeidentification
    {
        public static string Deidentify837(string ediText)
        {
            var f = new EdiFile();
            f.LoadFromString(ediText);

            var isDataChanged = false;
            foreach (var segment in f.Segments)
            {
                var elements = segment.Elements;
                switch (segment.Name)
                {
                    case "NM1":
                        if (elements[2] == "1") // individual
                        {
                            switch (elements[1])
                            {
                                case "41": // submitter
                                    elements[3] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Submitter Last Name";
                                    elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Submitter First Name";
                                    elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Submitter Middle Name";
                                    elements[9] = "SUBMITTER-ID";
                                    break;

                                case "40": // receiver
                                    elements[3] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Receiver Last Name";
                                    elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Receiver First Name";
                                    elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Receiver Middle Name";
                                    elements[9] = "RECEIVER-ID";
                                    break;

                                case "IL": // subscriber
                                    elements[3] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Subscriber Last Name";
                                    elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Subscriber First Name";
                                    elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Subscriber Middle Name";
                                    elements[9] = "SUBSCRIBER-ID";
                                    break;
                            }
                        }
                        isDataChanged = true;
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
                        isDataChanged = true;
                        break;

                    case "N3":
                        elements[1] = "123 Street Address";
                        if (elements.Length > 2) 
                        { 
                            elements[2] = "line 1"; 
                        }
                        isDataChanged = true;
                        break;

                    case "N4":
                        elements[1] = "City Name";
                        elements[2] = "MN";
                        elements[3] = "00000";
                        isDataChanged = true;
                        break;

                    case "SBR":
                        if (!String.IsNullOrEmpty(elements[3]))
                        {
                            elements[3] = "policy num";
                        }
                        if (!String.IsNullOrEmpty(elements[4]))
                        {
                            elements[4] = "Subscriber Name";
                        }
                        isDataChanged = true;
                        break;

                    case "PAT":
                        if (elements.Length > 6)
                        {
                            elements[6] = String.IsNullOrEmpty(elements[6]) ? String.Empty : "19000101";
                        }
                        isDataChanged = true;
                        break;

                    case "REF":
                        isDataChanged = true;
                        break;

                    case "CLM":
                        isDataChanged = true;
                        break;

                    case "DMG":
                        isDataChanged = true;
                        break;
                } // switch

                // write back to element data to segment if updated
                if (isDataChanged)
                {
                    segment.Elements = elements;
                }
                isDataChanged = false; // reset for next iteration
            } // foreach

            return f.Text;
        } // method

    } // class
} // namespace