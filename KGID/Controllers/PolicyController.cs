using BLL.KGIDPolicyBLL;
using BLL.NewEmployeeBLL;
using iTextSharp.text.pdf;
using KGID.Models;
using KGID_Models.KGID_Policy;
using KGID_Models.NBApplication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static KGID.FilterConfig;

namespace KGID.Controllers
{
    [RoutePrefix("Policy")]
    public class PolicyController : Controller
    {
        private readonly IPolicyBLL policyBLL;
        private readonly INBApplicationBll appBLL;

        public PolicyController()
        {
            policyBLL = new PolicyBLL();
            appBLL = new NBApplicationBll();
        }

        // GET: Policy
        public ActionResult Index()
        {
            return View();
        }

        [Route("FacingSheet")]
        public ActionResult GetFacingSheet(long applicationId,long empid)
        {
            //VM_FacingSheet facingSheet = new VM_FacingSheet();
            //facingSheet = policyBLL.GetFacingSheetDetails(applicationId, empid);
            //return View("FacingSheet", facingSheet);
            ///
            VM_FacingSheet facingSheet1 = new VM_FacingSheet();
            facingSheet1 = policyBLL.GetFacingSheetDetails(applicationId, empid);
            string filepath = FillFacingSheet(facingSheet1, applicationId);
            string result = policyBLL.NBBondFacingDocUploadBLL(applicationId,empid,filepath, "FacingSheet");
            //return View("FacingSheet", facingSheet1);
            return File(filepath, "application/pdf");

        }
        #region Facing Sheet Generation
        private string FillFacingSheet(VM_FacingSheet facingSheet, long result)
        {
            /////Verification Details
            if (facingSheet.CWName == "" && facingSheet.SIName == "" && facingSheet.DIOName == "" && facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.CWName = "";
                facingSheet.CWVDate = "";
                facingSheet.SIName = "";
                facingSheet.SIVDate = "";
                facingSheet.DIOName = "";
                facingSheet.DIOVDate = "";
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.SIName == "" && facingSheet.DIOName == "" && facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.SIName = "";
                facingSheet.SIVDate = "";
                facingSheet.DIOName = "";
                facingSheet.DIOVDate = "";
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DIOName == "" && facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.DIOName = "";
                facingSheet.DIOVDate = "";
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DDName == "" && facingSheet.DName == "")
            {
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DName == "")
            {
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            //////////
            if (facingSheet.DIOName == facingSheet.DDName && facingSheet.DDName == facingSheet.DName)
            {
                facingSheet.DDName = "";
                facingSheet.DDVDate = "";
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            else if (facingSheet.DDName == facingSheet.DName)
            {
                facingSheet.DName = "";
                facingSheet.DVDate = "";
            }
            ////////////////////////////////////////////
            string pdfTemplate = Server.MapPath("~/PdfTemplate/FacingSheet/Facingsheet_Template_Kannada.pdf");
            //string newFile = Server.MapPath("~/PdfTemplate/FacingSheet/" + result + "UnSigned" + ".pdf");
            //string newFile = @"C:/Documents/PdfTemplate/FacingSheet/" + result + "UnSigned" + ".pdf";
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"PdfTemplate\FacingSheet\" + result + "UnSigned" + ".pdf";
            }
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            AcroFields fields = pdfStamper.AcroFields;
            {//Facing Sheet Details
                var date1 = facingSheet.DateOfIssue?.ToString("dd-MM-yyyy");
                string date = DateTime.Now.ToShortDateString();
                fields.SetField("ApplicationRefNo", facingSheet.ApplicationNumber.ToString().Trim().ToUpper());
                fields.SetField("PolicyNumber", facingSheet.PolicyNumber);
                fields.SetFieldProperty("DIO", "textsize", 10.5f, null);
                fields.SetField("DIO", facingSheet.DistrictInsuranceOfficeAddress);
                fields.SetField("EmployeeName", facingSheet.InsurerName.ToString().Trim().ToUpper());
                //fields.SetField("DepositDate", facingSheet.InitialDeposit.ToString().Trim().ToUpper());
                fields.SetField("SubmittedBy", facingSheet.InsurerName.ToString().Trim().ToUpper());
                fields.SetField("DepositAmount", facingSheet.MonthlyPremium.ToString().Trim().ToUpper());
                //fields.SetField("CWDate", facingSheet.DateOfLiability);
                //fields.SetField("SIDDDate", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy"));
                //fields.SetField("KeyPoints", KeyPoints);
                //fields.SetField("OrderofPassage", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy").Trim().ToUpper());
                fields.SetField("ChallanRefNumber", facingSheet.ChallanRefNo);
                fields.SetField("ChallanDate", facingSheet.ChallanDate);
                fields.SetField("DateofRisk", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy"));
                fields.SetField("DOB", facingSheet.DateOfBirth.ToString("dd-MM-yyyy"));
                fields.SetFieldProperty("InsurerAge", "textsize", 10.5f, null);
                fields.SetField("InsurerAge", facingSheet.Age.ToString().Trim().ToUpper());
                fields.SetField("MonthlyPremium", facingSheet.MonthlyPremium.ToString().Trim().ToUpper());
                fields.SetField("SumAssuredAmount", facingSheet.InsuranceAmount.ToString().Trim().ToUpper());
                fields.SetField("MaturityMonthYear", facingSheet.EffectiveMonthYear);
                fields.SetField("DateofIssuePB", facingSheet.DateOfIssue?.ToString("dd-MM-yyyy"));
                //fields.SetField("DIODate", date);
                //Verification Details
                fields.SetField("CWName", facingSheet.CWName.ToString());
                fields.SetField("CWVDate", facingSheet.CWVDate.ToString());

                fields.SetField("SIName", facingSheet.SIName.ToString());
                fields.SetField("SIVDate", facingSheet.SIVDate.ToString());

                fields.SetField("DIOName", facingSheet.DIOName.ToString());
                fields.SetField("DIOVDate", facingSheet.DIOVDate.ToString());

                fields.SetField("DDName", facingSheet.DDName.ToString());
                fields.SetField("DDVDate", facingSheet.DDVDate.ToString());

                fields.SetField("DName", facingSheet.DName.ToString());
                fields.SetField("DVDate", facingSheet.DVDate.ToString());
            }
            if(facingSheet.Policies.Count==1)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
            }
            if (facingSheet.Policies.Count == 2)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
            }
            if (facingSheet.Policies.Count == 3)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
            }
            if (facingSheet.Policies.Count == 4)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
            }
            if (facingSheet.Policies.Count == 5)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
            }
            if (facingSheet.Policies.Count == 6)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
                fields.SetField("Policy6", facingSheet.Policies[5]);
            }
            if (facingSheet.Policies.Count == 7)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
                fields.SetField("Policy6", facingSheet.Policies[5]);
                fields.SetField("Policy7", facingSheet.Policies[6]);
            }
            if (facingSheet.Policies.Count == 8)
            {
                fields.SetField("Policy1", facingSheet.Policies[0]);
                fields.SetField("Policy2", facingSheet.Policies[1]);
                fields.SetField("Policy3", facingSheet.Policies[2]);
                fields.SetField("Policy4", facingSheet.Policies[3]);
                fields.SetField("Policy5", facingSheet.Policies[4]);
                fields.SetField("Policy6", facingSheet.Policies[5]);
                fields.SetField("Policy7", facingSheet.Policies[6]);
                fields.SetField("Policy8", facingSheet.Policies[7]);
            }
            //if(facingSheet.PolicyNumberDetails.Count>=1)
            //foreach (var row in facingSheet.PolicyNumberDetails)
            //{
            //        //row.PolicyNumber=
            //}
            pdfStamper.Close();
            return newFile;
        }
        #endregion

        public ActionResult KhajaneIIGateway()
        {
            VM_PaymentDetails obj = appBLL.NBChallanDetailsDll(Convert.ToInt64(Session["UID"]));
            return View("KhajaneIIGateway", obj);
        }
        public ActionResult GetIntimationLetter(long applicationId)
        {
            VM_InitimationLetter initimationLetter = new VM_InitimationLetter();
            initimationLetter = policyBLL.GetIntimationLetter(applicationId);
            return View("IntimationLetter", initimationLetter);
        }
    }
}