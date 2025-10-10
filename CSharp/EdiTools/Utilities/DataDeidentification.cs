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
                            var name = string.Empty;
                            switch (elements[1])
                            {
                                case "41": // submitter
                                    name = "Submitter";
                                    break;

                                case "40": // receiver
                                    name = "Receiver";
                                    break;

                                case "IL": // subscriber
                                    name = "Subscriber";
                                    break;

                                case "QC": // patient
                                    name = "Patient";
                                    break;

                                case "DN": // referring provider
                                    name = "Referring Provider";
                                    break;

                                case "P3": // Second referring provider
                                    name = "Second referring provider";
                                    break;

                                case "85": // billing provider
                                    name = "Billing Provider";
                                    break;

                                case "82": // rendering provider
                                    name = "Rendering Provider";
                                    break;

                                case "QB":
                                    name = "Purchase Service Provider";
                                    break;

                                case "DQ":
                                    name = "Supervising Physician";
                                    break;

                                case "DK":
                                    name = "Ordering Physician";
                                    break;

                                default: // all other names
                                    name = "Default";
                                    break;
                            } // switch

                            // update NM1 elements
                            elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : $"{name} Name";
                            if (elements.Length > 4) { elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : $"{name} First Name"; }
                            if (elements.Length > 5) { elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : $"{name} Middle Name"; }
                            if (elements.Length > 9) { elements[9] = $"{name.ToUpper()}-ID"; }

                            isDataChanged = true;
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
                            elements[4] = "(555) 555-5555";
                        }
                        isDataChanged = true;
                        break;

                    case "N3":
                        elements[1] = "123 Street Address";
                        if (elements.Length > 2) 
                        { 
                            elements[2] = "Address line 2"; 
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
                        elements[1] = "Patient Control Number";
                        isDataChanged = true;
                        break;

                    case "DTP":
                        elements[1] = "19000101";
                        if (elements.Length > 3) {
                            elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : "19000101"; 
                        }
                        isDataChanged = true;
                        break;

                    case "DMG":
                        elements[2] = "19000101";
                        elements[3] = "U";
                        isDataChanged = true;
                        break;

                    case "NTE":
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