using System;
using System.Collections.Generic;

namespace KGID_Models.KGID_Report
{
    public class VM_KGIDGSTReport
    {
        public string Type { get; set; }
        public DateTime miwd_updation_datetime { get; set; }

        public string miwd_application_id { get; set; }

        public string mia_application_ref_no { get; set; }

        public string mia_created_by { get; set; }
        public string employee_name { get; set; }
        public string first_kgid_policy_no { get; set; }

        public string dm_ddo_code { get; set; }
        public string dm_ddo_office { get; set; }
        public string p_mi_policy_number { get; set; }

        public string p_mi_premium { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }

        public string tot_GST { get; set; }
        public string p_mi_premium_wo_gst { get; set; }

        public DateTime curdate { get; set; }

    }


    public class VM_KGIDGSTReports
    {
        public VM_KGIDGSTReports()
        {
            GstCollection = new List<VM_KGIDGSTReport>();
            GSTSummary = new VM_KGIDGSTReportSummary();
        }

        public List<VM_KGIDGSTReport> GstCollection { get; set; }

        public VM_KGIDGSTReportSummary GSTSummary { get; set; }
    }


    public class VM_KGIDGSTReportDetails
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class VM_KGIDGSTReportSummary
    {
        public string MONTH { get; set; }


        public string YEAR { get; set; }

        public DateTime fdate { get; set; }
        public DateTime tdate { get; set; }

        public int nPolicy { get; set; }
        public int rPolicy { get; set; }

        public Decimal totPremium { get; set; }

        public Decimal totgst { get; set; }

        public Decimal totcgst { get; set; }
        public Decimal totsgst { get; set; }

        public Decimal totAmount { get; set; }

        public DateTime curdate { get; set; }

    }
}
