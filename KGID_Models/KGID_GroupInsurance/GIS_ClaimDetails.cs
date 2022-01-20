using KGID_Models.KGIDNBApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGID_Models.KGID_GroupInsurance
{
    public class GIS_CliamDetails
    {
        public string Type { get; set; }  //new,Edit,ListOfEmployeeStatus

        public string pagetype { get; set; }

        public long previousRefNo { get; set; }
        public long RenewalRefNo { get; set; }
        public long employee_id { get; set; }
        public Nullable<long> dept_employee_code { get; set; }
        public int SentBackAppliaction { get; set; }

        public bool activestatus { get; set; }

        public GIS_CliamDetails()
        {
            gis_type_of_cover_list = new List<SelectListItem>();
        }
        public long gis_referenceno { get; set; }
        public string gis_kgid_application_number { get; set; }
        public long gis_employee_id { get; set; }
        public string employee_name { get; set; }
        public string pan_number { get; set; }

        public string place_of_birth { get; set; }

        public string date_of_appointment { get; set; }

        public string spouse_name { get; set; }

        public string ewddateofjoining { get; set; }
        public string father_name { get; set; }
        public string gis_address { get; set; }
        public int gis_pincode { get; set; }
        public Nullable<long> gis_telephone_no { get; set; }
        public Nullable<long> gis_fax_no { get; set; }
        public string gis_email { get; set; }
        public string gis_occupation { get; set; }
        public string gis_type_of_cover { get; set; }
        public int gis_type_of_cover_id { get; set; }
        public string gis_Department { get; set; }
        public string gis_pagetype { get; set; }
        public string gis_policystartdate { get; set; }
        public string gis_policyenddate { get; set; }
        public string gis_policypremium { get; set; }
        public string gis_policynumber { get; set; }
        public long gis_old_application_Ref_number { get; set; }
        public string gis_kgid_renewal_application_number { get; set; }
        public string gis_category { get; set; }
        public int gis_selectedCategory { get; set; }
        //
        public string QRCode { get; set; }

        public int ewd_ddo_id { get; set; }

        //Application details
        public long gis_application_id { get; set; }
        //public string kad_kgid_application_number { get; set; }
        public string gis_date_of_submission { get; set; }

        //
        public string PolicyMonths { get; set; }

        public List<SelectListItem> gis_type_of_cover_list { get; set; }

        public Nullable<int> gender_id { get; set; }
        public string date_of_birth { get; set; }

        public Nullable<long> mobile_number { get; set; }
        public string email_id { get; set; }

        public string ewd_place_of_posting { get; set; }

        public string CLIAM_TYPE { get; set; }

        public List<SelectListItem> Genders { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> PayscaleCodes { get; set; }
        public List<SelectListItem> EmploymentTypes { get; set; }
        public List<SelectListItem> Designations { get; set; }
        public List<SelectListItem> Groups { get; set; }
        public List<SelectListItem> DDOCodes { get; set; }
        //Dropdown bound values
        public string gender { get; set; }
        public string department { get; set; }
        public string designation { get; set; }
        public string group { get; set; }
        public string emptype { get; set; }
        public string ddocode { get; set; }
        public string payscalecode { get; set; }
        public string date_of_DEATH { get; set; }
        public string CLIAM_SUB_TYPE { get; set; }


        public List<SelectListItem> ClaimTypes { get; set; }
        public List<SelectListItem> ClaimSubTypes { get; set; }

        public List<tbl_employee_nominee_details> listofnominee { get; set; }
    }
}
