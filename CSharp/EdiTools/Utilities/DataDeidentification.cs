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
                                    elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : "Submitter Last Name";
                                    elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Submitter First Name";
                                    elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Submitter Middle Name";
                                    elements[9] = "SUBMITTER-ID";
                                    break;

                                case "40": // receiver
                                    elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : "Receiver Last Name";
                                    elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Receiver First Name";
                                    elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Receiver Middle Name";
                                    elements[9] = "RECEIVER-ID";
                                    break;

                                case "IL": // subscriber
                                    elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : "Subscriber Last Name";
                                    elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Subscriber First Name";
                                    elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Subscriber Middle Name";
                                    elements[9] = "SUBSCRIBER-ID";
                                    break;

                                case "QC": // patient
                                    elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : "Patient Last Name";
                                    if (elements.Length > 4) { elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Patient First Name"; }
                                    if (elements.Length > 5) { elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Patient Middle Name"; }
                                    if (elements.Length > 9) { elements[9] = "PATIENT-ID"; }
                                    break;

                                case "DN": // referring provider
                                    elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : "Referring Provider Name";
                                    if (elements.Length > 4) { elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Referring First Name"; }
                                    if (elements.Length > 5) { elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Referring Middle Name"; }
                                    if (elements.Length > 9) { elements[9] = "Referring-ID"; }
                                    break;

                                case "P3": // Second referring provider
                                    elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : "Referring2 Provider Name";
                                    if (elements.Length > 4) { elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Referring2 First Name"; }
                                    if (elements.Length > 5) { elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Referring2 Middle Name"; }
                                    if (elements.Length > 9) { elements[9] = "Referring2-ID"; }
                                    break;

                                case "85": // billing provider
                                    elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : "Billing Provider Name";
                                    if (elements.Length > 4) { elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Provider First Name"; }
                                    if (elements.Length > 5) { elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Provider Middle Name"; }
                                    if (elements.Length > 9) { elements[9] = "PROVIDER-ID"; }
                                    break;

                                case "82": // rendering provider
                                    elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : "Rendering Provider Name";
                                    if (elements.Length > 4) { elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Rendering First Name"; }
                                    if (elements.Length > 5) { elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Rendering Middle Name"; }
                                    if (elements.Length > 9) { elements[9] = "RENDERING-ID"; }
                                    break;

                                default: // all other names
                                    elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : "Default Name";
                                    if (elements.Length > 4) { elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "Default First Name"; }
                                    if (elements.Length > 5) { elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "Default Middle Name"; }
                                    if (elements.Length > 9) { elements[9] = "DEFAULT-ID"; }
                                    break;
                            }
                        }

                        // update both person and non-person entity 


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
                            elements[4] = "(555) 555-5555";
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