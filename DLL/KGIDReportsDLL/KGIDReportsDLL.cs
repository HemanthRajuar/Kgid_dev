using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DLL.DBConnection;
using KGID_Models.KGID_Report;

namespace DLL.KGIDReportsDLL
{
    public class KGIDReportsDLL : IKGIDReportsDLL
    {
        private readonly Common_Connection _Conn = new Common_Connection();
        private readonly AllCommon _commnobj = new AllCommon();

        public VM_KGIDApplicationReports GetKGIDApplicationsReport(long loggedInUserId, VM_KGIDApplicationReportDetails applicationReportDetails, int empType)
        {
            VM_KGIDApplicationReports applicationReports = null;

            DataSet dsClaims = new DataSet();
            //if (applicationReportDetails.FromDate != null)
            //{
            //    string FDate = _commnobj.DateConversion(Convert.ToDateTime(applicationReportDetails.FromDate).ToShortDateString());
            //    applicationReportDetails.FromDate = Convert.ToDateTime(FDate);
            //}
            //if (applicationReportDetails.ToDate != null)
            //{
            //    string TDate = _commnobj.DateConversion(Convert.ToDateTime(applicationReportDetails.ToDate).ToShortDateString());
            //    applicationReportDetails.ToDate = Convert.ToDateTime(TDate);
            //}

            SqlParameter[] sqlparam =
            {
                new SqlParameter("@loggedInUserId", loggedInUserId),
                new SqlParameter("@empType", empType),
                new SqlParameter("@fromDate", applicationReportDetails.FromDate),
                new SqlParameter("@toDate", applicationReportDetails.ToDate)
            };

            dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_report_kgidApplications");
            if (dsClaims.Tables[0].Rows.Count > 0)
            {
                applicationReports = new VM_KGIDApplicationReports();
                foreach (DataRow row in dsClaims.Tables[0].Rows)
                {
                    VM_KGIDApplicationReport applicationReport = new VM_KGIDApplicationReport();
                    applicationReport.Name = row["Name"].ToString();
                    applicationReport.ApplicationRefNumber = row["ApplicationRefNumber"].ToString();
                    applicationReport.ApplicationDate = Convert.ToDateTime(row["ApplicationDate"].ToString());
                    applicationReport.District = row["District"].ToString();
                    applicationReport.Department = row["Department"].ToString();
                    applicationReport.Priority = row["Priority"].ToString();
                    applicationReport.Status = row["Status"].ToString();

                    applicationReports.ApplicationReports.Add(applicationReport);
                }
            }

            return applicationReports;
        }


        //ICT 11-11-2021
        public VM_KGIDGSTReports GetKGIDGSTReport(long loggedInUserId, VM_KGIDGSTReportDetails applicationReportDetails, int empType)
        {
            VM_KGIDGSTReports applicationReports = null;

            DataSet dsClaims = new DataSet();


            SqlParameter[] sqlparam =
            {
                new SqlParameter("@loggedInUserId", loggedInUserId),
                new SqlParameter("@empType", empType),
                new SqlParameter("@fromDate", applicationReportDetails.FromDate),
                new SqlParameter("@toDate", applicationReportDetails.ToDate)
            };

            dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_report_kgidMIGSTDATA");
            if (dsClaims.Tables[0].Rows.Count > 0)
            {
                applicationReports = new VM_KGIDGSTReports();
                foreach (DataRow row in dsClaims.Tables[0].Rows)
                {
                    VM_KGIDGSTReport applicationReport = new VM_KGIDGSTReport();

                    applicationReport.Type = row["Type"].ToString();
                    applicationReport.miwd_updation_datetime = Convert.ToDateTime(row["miwd_updation_datetime"].ToString());
                    applicationReport.miwd_application_id = row["miwd_application_id"].ToString();
                    applicationReport.mia_application_ref_no = row["mia_application_ref_no"].ToString();
                    applicationReport.mia_created_by = row["mia_created_by"].ToString();
                    applicationReport.employee_name = row["employee_name"].ToString();
                    applicationReport.first_kgid_policy_no = row["first_kgid_policy_no"].ToString();
                    applicationReport.dm_ddo_code = row["dm_ddo_code"].ToString();
                    applicationReport.dm_ddo_office = row["dm_ddo_office"].ToString();
                    applicationReport.p_mi_policy_number = row["p_mi_policy_number"].ToString();
                    applicationReport.p_mi_premium = row["p_mi_premium"].ToString();
                    applicationReport.CGST = row["CGST"].ToString();
                    applicationReport.SGST = row["SGST"].ToString();
                    applicationReport.tot_GST = row["tot_GST"].ToString();
                    applicationReport.p_mi_premium_wo_gst = row["p_mi_premium_wo_gst"].ToString();


                    applicationReports.GstCollection.Add(applicationReport);


                }
            }

            if (dsClaims.Tables[1].Rows.Count > 0)
            {
                applicationReports.GSTSummary.MONTH = dsClaims.Tables[1].Rows[0]["mon"].ToString();
                applicationReports.GSTSummary.YEAR = dsClaims.Tables[1].Rows[0]["yr"].ToString();
                applicationReports.GSTSummary.nPolicy = Convert.ToInt32(dsClaims.Tables[1].Rows[0]["nPolicy"].ToString());
                applicationReports.GSTSummary.rPolicy = Convert.ToInt32(dsClaims.Tables[1].Rows[0]["rPolicy"].ToString());
                applicationReports.GSTSummary.totPremium = Convert.ToInt32(dsClaims.Tables[1].Rows[0]["totPremium"].ToString());
                applicationReports.GSTSummary.totgst = Convert.ToInt32(dsClaims.Tables[1].Rows[0]["totgst"].ToString());
                applicationReports.GSTSummary.totAmount = Convert.ToInt32(dsClaims.Tables[1].Rows[0]["totAmount"].ToString());
                applicationReports.GSTSummary.curdate = Convert.ToDateTime(dsClaims.Tables[1].Rows[0]["printdate"].ToString());
                applicationReports.GSTSummary.fdate = Convert.ToDateTime(dsClaims.Tables[1].Rows[0]["fdate"].ToString());
                applicationReports.GSTSummary.tdate = Convert.ToDateTime(dsClaims.Tables[1].Rows[0]["tdate"].ToString());
            }



            return applicationReports;
        }
    }
}
