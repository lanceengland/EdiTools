using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
                            var entityIdName = string.Empty;
                            switch (elements[1])
                            {
                                case "41": // submitter
                                    entityIdName = "Submitter";
                                    break;

                                case "40": // receiver
                                    entityIdName = "Receiver";
                                    break;

                                case "IL": // subscriber
                                    entityIdName = "Subscriber";
                                    break;

                                case "QC": // patient
                                    entityIdName = "Patient";
                                    break;

                                case "DN": // referring provider
                                    entityIdName = "Referring Provider";
                                    break;

                                case "P3": // Second referring provider
                                    entityIdName = "Second referring provider";
                                    break;

                                case "85": // billing provider
                                    entityIdName = "Billing Provider";
                                    break;

                                case "82": // rendering provider
                                    entityIdName = "Rendering Provider";
                                    break;

                                case "QB":
                                    entityIdName = "Purchase Service Provider";
                                    break;

                                case "DQ":
                                    entityIdName = "Supervising Physician";
                                    break;

                                case "DK":
                                    entityIdName = "Ordering Physician";
                                    break;

                                 case "71":
                                    entityIdName = "Attending Physician";
                                    break;

                                case "72":
                                    entityIdName = "Operating Physician";
                                    break;

                                case "73":
                                    entityIdName = "Other Physician";
                                    break;

                                case "ZZ":
                                    entityIdName = "Other Operating Physician";
                                    break;

                                case "AO":
                                    entityIdName = "Account Of";
                                    break;

                                default: // all other names
                                    entityIdName = "Default";
                                    break;
                            } // switch

                            // update NM1 elements
                            elements[3] = String.IsNullOrEmpty(elements[3]) ? String.Empty : $"{entityIdName} Name";
                            if (elements.Length > 4) { elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : $"{entityIdName} First Name"; }
                            if (elements.Length > 5) { elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : $"{entityIdName} Middle Name"; }
                            if (elements.Length > 9) { elements[9] = $"{entityIdName.ToUpper()}-ID"; }

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
                            switch (elements[1])
                        {
                            case "SY": // SSN
                                elements[2] = "000000000";
                                break;

                            case "8U": // Bank assigned security identifier
                                elements[2] = "bank1234";
                                break;

                            case "EM": // Electronic Payment Reference Number
                                elements[2] = "eref-pay1234";
                                break;

                            case "Y4": // Property and Casualty clai number
                                elements[2] = "agencyclaim1234";
                                break;

                            case "BB": // Credit/Debit Card Information
                                elements[2] = "auth1234";
                                break;

                            case "1W": // Member identificationn
                                elements[2] = "member1234";
                                break;

                            case "IG": // Insurance policy number
                                elements[2] = "policy1234";
                                break;

                            case "23": // claim number
                                elements[2] = "claim1234";
                                break;

                            case "9F": // referral number
                                elements[2] = "referral1234";
                                break;

                            case "G1": // prior authorization number
                                elements[2] = "priorauth1234";
                                break;

                            case "F8": // payer claim control number
                                elements[2] = "payerclaimctrl1234";
                                break;

                            case "9A": // repriced claim number
                                elements[2] = "repriced1234";
                                break;

                            case "9C": // adjusted repriced claim number
                                elements[2] = "adjustedrepriced1234";
                                break;

                            case "9D": // claim number
                                elements[2] = "claimnumber1234";
                                break;

                            case "EA": // medical record number
                                elements[2] = "medicalrecordnumber1234";
                                break;

                            case "EI": // identification number
                                elements[2] = "employerid1234";
                                break;

                            case "4N":
                                elements[2] = "specialpayment1234";
                                break;

                            case "72": // other physician
                                elements[3] = "other physican last";
                                if (elements.Length > 4) 
                                { 
                                    elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "other physican first"; 
                                }
                                if (elements.Length > 5)
                                {
                                    elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "other physican first";
                                }
                                if (elements.Length > 6)
                                {
                                    elements[6] = String.IsNullOrEmpty(elements[6]) ? String.Empty : "other physican middle";
                                }
                                break;

                            case "ZZ": // other physician
                                elements[3] = "other operating physican";
                                if (elements.Length > 4)
                                {
                                    elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "other operating first";
                                }
                                if (elements.Length > 5)
                                {
                                    elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "other operating first";
                                }
                                if (elements.Length > 6)
                                {
                                    elements[6] = String.IsNullOrEmpty(elements[6]) ? String.Empty : "other operating middle";
                                }
                                break;

                            case "82":
                                elements[3] = "rendering provider";
                                if (elements.Length > 4)
                                {
                                    elements[4] = String.IsNullOrEmpty(elements[4]) ? String.Empty : "rendering first";
                                }
                                if (elements.Length > 5)
                                {
                                    elements[5] = String.IsNullOrEmpty(elements[5]) ? String.Empty : "rendering first";
                                }
                                if (elements.Length > 6)
                                {
                                    elements[6] = String.IsNullOrEmpty(elements[6]) ? String.Empty : "rendering middle";
                                }
                                break;

                            case "0B": // State License Number
                                elements[2] = "state license number";
                                break;

                            case "1G": // Provider UPIN Number
                                elements[2] = "X00000";
                                break;

                            case "G2": // Provider Commercial Number
                                elements[2] = "commercial number";
                                break;

                            case "LU": // Location Number
                                elements[2] = "Location Number";
                                break;
                        }
                        isDataChanged = true;
                        break;

                    case "CLM":
                        elements[1] = "Patient Ctl Num 1234";
                        isDataChanged = true;
                        break;

                    case "DTP":
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
                        elements[2] = "scrubbed text";
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