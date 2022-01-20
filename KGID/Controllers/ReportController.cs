using BLL.KGIDReportsBLL;
using Common;
using KGID.Models;
using KGID_Models.KGID_Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static KGID.FilterConfig;
using BLL.NewEmployeeBLL;
using KGID_Models;
using DLL.DBConnection;
namespace KGID.Controllers
{
    [NoCache]    
    public class ReportController : Controller
    {
        private readonly IKGIDReportsBLL reportsBLL;
        private readonly DbConnectionKGID _db = new DbConnectionKGID();

        public ReportController()
        {
            reportsBLL = new KGIDReportsBLL();
        }

        [Route("APP-REP")]
        public ActionResult GetKGIDApplicationReport()
        {
            return View("ApplicationReports");
        }

        [HttpPost]
        [Route("APP-REP-DET")]
        public ActionResult GetKGIDApplications(VM_KGIDApplicationReportDetails applicationReportDetails)
        {
            //int empType = 1;
            //long loggedInUserId = Convert.ToInt64(Session["UID"]);
            //if (applicationReportDetails.KGIDNO != "" && applicationReportDetails.KGIDNO != null && applicationReportDetails.KGIDNO != "undefined")
            //{
            //    long temp = System.Convert.ToInt32(applicationReportDetails.KGIDNO);
            //    var UserDtls = (from n in _db.tbl_employee_basic_details
            //                    where n.first_kgid_policy_no == temp
            //                    select n).FirstOrDefault();
            //    loggedInUserId = UserDtls.employee_id;
            //    empType = Convert.ToInt32(applicationReportDetails.value);
            //}
            VM_KGIDApplicationReports applicationReports = new VM_KGIDApplicationReports();
            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            int empType = 1;
            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DIO)))
            {
                empType = 2;
            }
            else if (Session["SelectedCategory"] != null && (Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DEPUTYDIRECTOR)) || Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DIRECTOR))))
            {
                empType = 3;
            }
            else if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DDO)))
            {
                empType = 4;
            }
            applicationReports = reportsBLL.GetKGIDApplicationsReport(loggedInUserId, applicationReportDetails, empType);

            return PartialView("_KGIDApplications", applicationReports);
        }
        [Route("APP-MIGST-REP")]
        public ActionResult GetMIGSTReport()
        {
            return View("GSTReport");
        }

        [HttpPost]
        [Route("APP-MIGST-REP-DET")]
        public ActionResult GetMIGSTCollection(VM_KGIDGSTReportDetails applicationReportDetails)
        {

            VM_KGIDGSTReports applicationReports = new VM_KGIDGSTReports();

            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            int empType = 1;
            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DIO)))
            {
                empType = 2;
            }
            else if (Session["SelectedCategory"] != null && (Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DEPUTYDIRECTOR)) || Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DIRECTOR))))
            {
                empType = 3;
            }
            else if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DDO)))
            {
                empType = 4;
            }
            else if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.ASSITANTDIRECTOR)))
            {
                empType = 15;
            }

            applicationReports = reportsBLL.GetKGIDGSTReport(loggedInUserId, applicationReportDetails, empType);

            return PartialView("_KGIDGSTData", applicationReports);
        }

        [HttpPost]
        [Route("APP-MIGST-REP-PRINT")]
        public ActionResult GetMIGSTPRINT(VM_KGIDGSTReportDetails applicationReportDetails)
        {

            VM_KGIDGSTReports applicationReports = new VM_KGIDGSTReports();

            long loggedInUserId = Convert.ToInt64(Session["UID"]);
            int empType = 1;
            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DIO)))
            {
                empType = 2;
            }
            else if (Session["SelectedCategory"] != null && (Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DEPUTYDIRECTOR)) || Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DIRECTOR))))
            {
                empType = 3;
            }
            else if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.DDO)))
            {
                empType = 4;
            }
            else if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Equals(Convert.ToString((int)UserCategories.ASSITANTDIRECTOR)))
            {
                empType = 15;
            }

            applicationReports = reportsBLL.GetKGIDGSTReport(loggedInUserId, applicationReportDetails, empType);

            return PartialView("_KGIDGSTDataPrint", applicationReports.GSTSummary);
        }
    }
}