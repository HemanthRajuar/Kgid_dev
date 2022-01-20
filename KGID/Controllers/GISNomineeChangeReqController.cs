using System.Web.Mvc;
using BLL.NewEmployeeBLL;
using BLL.DDOMasterBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.DeptMasterBLL;
using KGID_Models.KGID_User;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using BLL.InsuredEmployeeBll;
using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDNBApplication;
using HtmlAgilityPack;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using KGID_Models.NBApplication;
using BLL.UploadEmployeeBLL;
using KGID_Models.KGID_Policy;
using System.Net;
using System.Web.Routing;
using static KGID.FilterConfig;
using KGID_Models.KGID_VerifyData;
//PDF DSC SignIn
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using iTextSharp.text.pdf.security;
using System.Security.Cryptography.X509Certificates;

using KGID_Models.KGID_Master;
using Common;
using System.Web.Configuration;
using System.Threading;
using BLL.KGIDGISNomineeChangeReqBLL;
using BLL.KGIDGroupInsuranceBLL;
using System.Threading.Tasks;
using DLL.DBConnection;
using KGID_Models.KGID_GroupInsurance;
using System.Web;
using BLL.VerifyDataBLL;


namespace KGID.Controllers
{
    public class GISNomineeChangeReqController : Controller
    {
        // GET: GrpupInsuranceNomineeChangeReq
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly INewEmployeeDetailsBLL _newemp;
        private readonly IDDOMasterBLL _ddomaster;
        private readonly IDeptMasterBLL _deptmaster;
        private readonly IInsuredEmployeeBll _InsuredEmployeebll;
        private readonly INBApplicationBll _INBApplicationbll;
        private readonly IUploadEmployeeBLL _uploadbll;
        private readonly IGroupInsuranceBLL _IGroupInsuranceBLL;
        private readonly IGISNomineeChangeReqBLL _IGISNomineeChangeReqBLL;


        public GISNomineeChangeReqController()
        {
            this._newemp = new NewEmployeeDetailsBLL();
            this._ddomaster = new DDOMasterBLL();
            this._deptmaster = new DeptMasterBLL();
            this._InsuredEmployeebll = new InsuredEmployeeBll();
            this._INBApplicationbll = new NBApplicationBll();
            this._uploadbll = new UploadEmployeeBLL();
            this._IGroupInsuranceBLL = new GroupInsuranceBLL();
            this._IGISNomineeChangeReqBLL = new GISNomineeChangeReqBLL();


        }
        public ActionResult Index()
        {
            return View();

        }
        [Route("gis-ncr-view-app")]
        public async Task<ActionResult> GIS_Ncr_ApplicationForm()
        {


            //long empId = Convert.ToInt64(Session["UID"]);
            //model1 = _IGroupInsuranceBLL.GISGetEmployeeDetails(empId);
            //VM_ApplicationDetail model = new VM_ApplicationDetail();
            //model.Emp_Id = empId;
            //model.ApplicationId = model1.kad_application_id;
            //return View("GIS_Ncr_ApplicationForm", model);


            long empId = Convert.ToInt64(Session["UID"]);
            VM_BasicDetails model1 = new VM_BasicDetails();
            model1 = _IGroupInsuranceBLL.GISGetEmployeeDetails(empId);
            VM_ApplicationDetail model = new VM_ApplicationDetail();
            model.Emp_Id = empId;
            model.ApplicationId = model1.kad_application_id;
            model.ApplicationNumber = model1.kad_kgid_application_number;
            long applstatus = await _IGroupInsuranceBLL.GetGISApplicationStatus(empId);
            long NCR_applstatus = await _IGISNomineeChangeReqBLL.GetGIS_NCRApplicationStatus(empId);


            //if (applstatus == 15)
            //{
            //    VM_GISDDOVerificationDetails verificationDetails = _IGroupInsuranceBLL.GISGetEmployeeApplicationStatusDll(Convert.ToInt64(Session["UId"]));

            //    return View("ViewGISApplicationDetails", verificationDetails);
            //    //ViewBag.applicationstatus = "Approved";
            //}


            ViewBag.ApplicationProcess = false;
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(model1.kad_kgid_application_number, QRCodeGenerator.ECCLevel.Q);
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    model1.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    model.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }

            if (applstatus == 15)
            {

                if (applstatus == 2 || NCR_applstatus == 2)
                {
                    // allow to make changes
                    ViewBag.ApplicationProcess = false;
                }
                else if (applstatus == 3 || applstatus == 4 || NCR_applstatus == 3 || NCR_applstatus == 4)
                {
                    // restrict access
                    ViewBag.ApplicationProcess = true;
                }
            }
            else { ViewBag.AccessToNomineeChange = true; }



            Session["UID"] = empId;
            //return View(applicationDetails);

            return View("GIS_Ncr_ApplicationForm", model);
        }
        public ActionResult GIS_NCR_NomineeDetails()
        {
            VM_BasicDetails model = new VM_BasicDetails();
            long empId = Convert.ToInt64(Session["UID"]);
            model = _IGroupInsuranceBLL.GISGetEmployeeDetails(empId);
            ViewBag.ismarried = model.eod_emp_married;
            ViewBag.isOrphan = model.eod_emp_orphan;
            VM_NomineeDetails _NomineeData = new VM_NomineeDetails();
            _NomineeData.NomineeDetails = _IGroupInsuranceBLL.GISNomineeDetailsDll(empId);
            _NomineeData.EmployeeId = Convert.ToInt64(Session["UID"]); ;
            _NomineeData.ddlNomineeList = _IGroupInsuranceBLL.GetNomineelist(empId);
            return PartialView("GIS_NCR_NomineeDetails", _NomineeData);
        }

        public Task<long> GIS_NR_InsertNomineeDetails(VM_NomineeDetail objNomineeDetails)
        {
            //// var result = _IGISNomineeChangeReqBLL.GIS_NR_SaveNominee(objNomineeDetails);
            ////return result;
            //return null;

            string path = UploadDocumentNominee(objNomineeDetails.SonDaughterAdoption_doc, (long)objNomineeDetails.EmpId, "AdoptionDocument");
            objNomineeDetails.SonDaughterAdoption_doc_path = path;
            //  var result = _IGroupInsuranceBLL.GISSaveNBNominee(objNomineeDetails);
            var result = _IGISNomineeChangeReqBLL.GISSaveNBNomineeChangeRequest(objNomineeDetails);
            return result;
        }

        private string UploadDocumentNominee(HttpPostedFileBase document, long empId, string docType)
        {
            //string path = string.Empty;
            //if (document != null && document.ContentLength > 0)
            //{
            //    string fileName = Path.GetFileName(document.FileName);
            //    string subPath = string.Empty;// = "~/EmployeeDocuments/" + empId.ToString() + "/" + docType;
            //    if (docType == "AdoptionDocument")
            //    {
            //        //subPath = @"C:/Users/CSG/Documents/Adoption/";// + empId.ToString() + "/" + docType;
            //        if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            //        {
            //            //subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\Adoption\";
            //            subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"Adoption\";
            //        }
            //    }
            //    string FileNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
            //    path = subPath + empId.ToString() + FileNo + fileName;
            //    document.SaveAs(path);
            //}
            //return path;
            string path = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);
                string subPath = string.Empty;// = "~/EmployeeDocuments/" + empId.ToString() + "/" + docType;
                if (docType == "AdoptionDocument")
                {
                    // subPath = @"C:/Users/CSG/Documents/Adoption/";// + empId.ToString() + "/" + docType;
                    if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    {
                        if (!Directory.Exists(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\Adoption\" + empId))
                            Directory.CreateDirectory(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\Adoption\" + empId);
                        subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\Adoption\" + empId + "\\";
                        //subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"Adoption\";
                    }
                }
                string FileNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                path = subPath + empId.ToString() + FileNo + fileName;
                document.SaveAs(path);
            }
            return path;
        }

        public ActionResult GIS_NCR_EmployeeBasicDetailsToView()
        {
            VM_BasicDetails model = new VM_BasicDetails();
            long empId = Convert.ToInt64(Session["UID"]);
            model = _IGroupInsuranceBLL.GISGetEmployeeDetails(empId);
            DateTime f = (DateTime)model.dateofbirth;
            DateTime f1 = (DateTime)model.dateofappointment;
            DateTime f2 = (DateTime)model.ewddateofjoining;
            // model.ead_pincode = int.Parse(model.ead_pincode1);
            model.date_of_birth = f.ToShortDateString();
            model.ewd_date_of_joining = f2.ToShortDateString();
            model.date_of_appointment = f1.ToShortDateString();
            return PartialView("GIS_NCR_EmployeeBasicDetails", model);
        }
        public ActionResult GIS_NCR_Declaration()
        {
            return this.PartialView();

        }
        //[Route("GIS-NomineeChange--Declaration")]
        public Task<int> GIS_NR_SaveDeclaration(long EmpId, long AppId)
        {
            var result = _IGISNomineeChangeReqBLL.GISSaveDeclaration(EmpId, AppId);
            //return result;
            return result;
        }

        ///// UPLOAD AND DOWNLOAD OF FILES (DOCUMENTS)
        /////
        //[Route("kgid-upload-emp-data")]
         [Route("gis-ncr_upload-emp-data")]
        public ActionResult GIS_NCR_UploadForm()
        {

            DbConnectionKGID _db = new DbConnectionKGID();
            VM_GIS_Upload_EmployeeForm objUEData = new VM_GIS_Upload_EmployeeForm();

            objUEData.App_Employee_Code = Convert.ToInt64(Session["UID"]);
            if (objUEData.App_Employee_Code != 0)
            {
                //objUEData = _Objemployee.GetUploadDocBll(objUEData.App_Employee_Code);
                objUEData = _IGISNomineeChangeReqBLL.GetUploadDoc(objUEData.App_Employee_Code);
            }
            objUEData.App_Employee_Code = Convert.ToInt64(Session["UID"]);
            return this.PartialView(objUEData);
        }
        [Route("gis-noiminee-change-upload-form")]
        public async Task<ActionResult>    GISUploadEmployeeData(VM_GIS_Upload_EmployeeForm objUF)
        {
            var result = 0;
            tbl_GIS_NR_upload_form objEmpForm = new tbl_GIS_NR_upload_form();
            //if (ModelState.IsValid)
            //{
                //string path = Server.MapPath("~/Uploads/");
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                // string AppPath = UploadDocument(objUF.ApplicationFormDoc, objUF.App_Employee_Code, "ApplicationForm");
                string Form6Path = UploadDocument(objUF.Form6Doc, objUF.App_Employee_Code, "Form6");
                string Form7Path = UploadDocument(objUF.Form7Doc, objUF.App_Employee_Code, "Form7");

                objUF.App_Employee_Code = Convert.ToInt64(Session["UID"]);
                objEmpForm.App_Employee_Code = Convert.ToInt64(Session["UID"]);
                //  objEmpForm.App_Application_Form = AppPath;
                objEmpForm.App_Form6 = Form6Path;
                objEmpForm.App_Form7 = Form7Path;
                //string SubPathForApp = "/EmployeeDocuments/" + objEmpForm.App_Employee_Code.ToString() + "/ApplicationForm/";
                //string SubPathForMed = "/EmployeeDocuments/" + objEmpForm.App_Employee_Code.ToString() + "/MedicalForm/";

                //objEmpForm.App_Application_Form = SubPathForApp + objUF.ApplicationFormDoc.FileName;
                // objEmpForm.App_Application_Form = AppPath;
                objEmpForm.App_Form6 = Form6Path;
                objEmpForm.App_Form7 = Form7Path;

                //objEmpForm.App_Medical_Form = SubPathForMed + objUF.MedicalFormDoc.FileName;
                //    objEmpForm.App_Form6 = Form6Path;
                //objEmpForm.App_Form7 = Form7Path;


                int result1 =await _IGISNomineeChangeReqBLL.GIS_NR_UploadForms(objEmpForm);
            // result = _IGroupInsuranceBLL.GISSaveEmployeeForm(objEmpForm);
            if (result1 == 1)
                {
                    //TempData["EmpForm"] = objEmpForm;
                    ViewBag.EmpForm = objEmpForm;

                    //try
                    //{
                    //    var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587481508452);

                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587481508452" + msg);
                    //    var ApprefNo = (from ad in _dbVerify.tbl_kgid_application_details where ad.kad_emp_id == objEmpForm.App_Employee_Code select ad.kad_kgid_application_number).ToList().LastOrDefault();
                    //    var mobile = (from eb in _dbVerify.tbl_employee_basic_details where eb.employee_id == objEmpForm.App_Employee_Code select eb).FirstOrDefault();
                    //    var ddoempid = (from eb in _dbVerify.tbl_employee_work_details where eb.ewd_emp_id == objEmpForm.App_Employee_Code select eb.ewd_ddo_id).FirstOrDefault();

                    //    var ddomobileno = (from ebd in _dbVerify.tbl_employee_basic_details
                    //                       join wd in _dbVerify.tbl_employee_work_details on ebd.employee_id equals wd.ewd_emp_id
                    //                       where ebd.user_category_id.Contains("1,2") && wd.ewd_ddo_id == ddoempid
                    //                       select ebd.mobile_number).FirstOrDefault();
                    //    // var challandate = (from ch in _dbVerify.tbl_challan_details where ch.cd_system_emp_code == objEmpForm.App_Employee_Code select ch.cd_updation_datetime).FirstOrDefault();
                    //    var challandate = _dbVerify.tbl_challan_details.Where(a => a.cd_application_id == objEmpForm.App_ApplicationID).Select(a => a.cd_updation_datetime).FirstOrDefault();

                    //    msg = msg.Replace("#var1#", ApprefNo);
                    //    msg = msg.Replace("#var2#", challandate.ToString());
                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587481508452 replace of values" + msg);
                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData obile.mobile_number.ToString()  " + mobile.mobile_number.ToString());
                    //    //string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + ApprefNo + " ಯು ದಿನಾಂಕ " + challandate + " ದಂದು ಯಶಸ್ವಿಯಾಗಿ ನಿಮ್ಮ ವೇತನ ಬಟವಾಡೆ ಮಾಡುವ ಅಧಿಕಾರಿಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ,"
                    //    //    + "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                    //    AllCommon.sendUnicodeSMS(mobile.mobile_number.ToString(), msg, "1107161587481508452");
                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData SMS Sent ");
                    //    var ddomsg = _INBApplicationbll.GetEmailSMSTemplate(1107161587541292075);

                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587541292075 " + msg);

                    //    ddomsg = ddomsg.Replace("#var1#", mobile.employee_name);
                    //    ddomsg = ddomsg.Replace("#var2#", ApprefNo);

                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData  ddo replace of values " + msg);
                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData ddomobileno.ToString()  " + ddomobileno.ToString());


                    //    //                string ddomsg = "ವೇತನ ಬಟವಾಡೆ ಅಧಿಕಾರಿಗಳೇ, ಶ್ರೀ/ಶ್ರೀಮತಿ " + mobile.employee_name + " ರವರ " + ApprefNo + "  ಸಂಖ್ಯೆಯ ವಿಮಾ ಪ್ರಸ್ತಾವನೆಯು ಪರಿಶೀಲನೆಗಾಗಿ ನಿಮ್ಮ ಲಾಗಿನ್‌ಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ. https://kgidonline.karnataka.gov.in ಗೆ ಲಾಗಿನ್ ಆಗಿ ಮುಂದಿನ ಕ್ರಮ ಕೈಗೊಳ್ಳಲು ಕೋರಿದೆ."
                    //    //+ "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                    //    AllCommon.sendUnicodeSMS(ddomobileno.ToString(), ddomsg, "1107161587541292075");
                    //    Logger.LogMessage(TracingLevel.INFO, "SMS Sent ");
                    //}
                    //catch (Exception ex)
                    //{
                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData" + ex.Message.ToString());
                    //}

                    // return RedirectToAction("ViewApplicationDetails", "VerifyDetails");
                    return Json(new { RedirectUrl = "/gis-ncr-view-app/" }, JsonRequestBehavior.AllowGet);
                }
                else if (result1 == 2)
                {
                    // TempData["EmpForm"] = objEmpForm;
                    ViewBag.EmpForm = objEmpForm;
                    //try
                    //{
                    //    var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587481508452);

                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587481508452" + msg);
                    //    var ApprefNo = (from ad in _dbVerify.tbl_kgid_application_details where ad.kad_emp_id == objEmpForm.App_Employee_Code select ad.kad_kgid_application_number).ToList().LastOrDefault();
                    //    var mobile = (from eb in _dbVerify.tbl_employee_basic_details where eb.employee_id == objEmpForm.App_Employee_Code select eb).FirstOrDefault();
                    //    var ddoempid = (from eb in _dbVerify.tbl_employee_work_details where eb.ewd_emp_id == objEmpForm.App_Employee_Code select eb.ewd_ddo_id).FirstOrDefault();

                    //    var ddomobileno = (from ebd in _dbVerify.tbl_employee_basic_details
                    //                       join wd in _dbVerify.tbl_employee_work_details on ebd.employee_id equals wd.ewd_emp_id
                    //                       where ebd.user_category_id.Contains("1,2") && wd.ewd_ddo_id == ddoempid
                    //                       select ebd.mobile_number).FirstOrDefault();
                    //    var challandate = (from ch in _dbVerify.tbl_challan_details where ch.cd_application_id == objEmpForm.App_ApplicationID select ch.cd_updation_datetime).FirstOrDefault();


                    //    msg = msg.Replace("#var1#", ApprefNo);
                    //    msg = msg.Replace("#var2#", challandate.ToString());
                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587481508452 replace of values" + msg);
                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData obile.mobile_number.ToString()  " + mobile.mobile_number.ToString());
                    //    //string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + ApprefNo + " ಯು ದಿನಾಂಕ " + challandate + " ದಂದು ಯಶಸ್ವಿಯಾಗಿ ನಿಮ್ಮ ವೇತನ ಬಟವಾಡೆ ಮಾಡುವ ಅಧಿಕಾರಿಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ,"
                    //    //    + "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                    //    AllCommon.sendUnicodeSMS(mobile.mobile_number.ToString(), msg, "1107161587481508452");
                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData SMS Sent ");
                    //    var ddomsg = _INBApplicationbll.GetEmailSMSTemplate(1107161587541292075);

                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData 1107161587541292075 " + msg);

                    //    ddomsg = ddomsg.Replace("#var1#", mobile.employee_name);
                    //    ddomsg = ddomsg.Replace("#var2#", ApprefNo);

                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData  ddo replace of values " + msg);
                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData ddomobileno.ToString()  " + ddomobileno.ToString());


                    //    //                string ddomsg = "ವೇತನ ಬಟವಾಡೆ ಅಧಿಕಾರಿಗಳೇ, ಶ್ರೀ/ಶ್ರೀಮತಿ " + mobile.employee_name + " ರವರ " + ApprefNo + "  ಸಂಖ್ಯೆಯ ವಿಮಾ ಪ್ರಸ್ತಾವನೆಯು ಪರಿಶೀಲನೆಗಾಗಿ ನಿಮ್ಮ ಲಾಗಿನ್‌ಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ. https://kgidonline.karnataka.gov.in ಗೆ ಲಾಗಿನ್ ಆಗಿ ಮುಂದಿನ ಕ್ರಮ ಕೈಗೊಳ್ಳಲು ಕೋರಿದೆ."
                    //    //+ "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                    //    AllCommon.sendUnicodeSMS(ddomobileno.ToString(), ddomsg, "1107161587541292075");
                    //    Logger.LogMessage(TracingLevel.INFO, "SMS Sent ");
                    //}
                    //catch (Exception ex)
                    //{
                    //    Logger.LogMessage(TracingLevel.INFO, "UploadEmployeeData" + ex.Message.ToString());
                    //}

                    //return RedirectToAction("ViewApplicationDetails", "VerifyDetails");
                    return Json(new { RedirectUrl = "/gis-ncr-view-app/" }, JsonRequestBehavior.AllowGet);
                }
            //  }
            //devika
            //VM_GISDeptVerificationDetails workflow = new VM_GISDeptVerificationDetails();
            //workflow.ApplicationId = objUF.App_Employee_Code;
            //Task<int> workflowResult = _IGISNomineeChangeReqBLL.GIS_NR_UpdateWorkFlow(workflow);


            //devika
            //return View(objEmpForm);
            //return RedirectToAction("UploadEmployeeData", "VerifyData");


            //30nov changed
            //return Json(new { RedirectUrl = "/gis-ncr_upload-emp-data/", Result = result }, JsonRequestBehavior.AllowGet);
            //return Json(new { RedirectUrl = "/gis-ncr-view-app/" }, JsonRequestBehavior.AllowGet);
            return Json(new { Result = result1 }, JsonRequestBehavior.AllowGet);
        }

        private string UploadDocument(HttpPostedFileBase document, long empId, string docType)
        {
            string path = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);




                string subPath = string.Empty;// = "~/EmployeeDocuments/" + empId.ToString() + "/" + docType;

                if (docType == "Form6")
                {
                    if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    {
                        if (!Directory.Exists(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\NCR_Form6\" + empId))
                            Directory.CreateDirectory(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\NCR_Form6\" + empId);
                        subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\NCR_Form6\" + empId + "\\";
                      
                        
                        
                        //subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"NCR_Form6\";
                    }
                    // subPath = @"C:/Users/CSG/Documents/Challans/";// + empId.ToString() + "/" + docType;
                    //if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    //{
                    //    subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"EmployeeDocuments\ApplicationForm\";
                    //}
                }
                else if (docType == "Form7")
                {

                    if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    {

                        if (!Directory.Exists(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\NCR_Form7\" + empId))
                            Directory.CreateDirectory(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\NCR_Form7\" + empId);
                        subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\NCR_Form7\" + empId + "\\";


                        // subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"NCR_Form7\";
                    }
                    //subPath = @"C:/Users/CSG/Documents/Challans/";
                    //subPath = @"F:/Documents/EmployeeDocuments/MedicalForm/";
                    //if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    //{
                    //    subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"EmployeeDocuments\MedicalForm\";
                    //}
                }
                //bool exists = Directory.Exists(Server.MapPath(subPath));
                //if (!exists)
                //{
                //    Directory.CreateDirectory(Server.MapPath(subPath));
                //}

                //path = Path.Combine(Server.MapPath(subPath), fileName);
                string FileNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                path = subPath + empId.ToString() + FileNo + fileName;

                document.SaveAs(path);
            }

            return path;
        }


        [Route("GIS-NomineeChange-ddo")]
        public async Task<ActionResult> DetailsForDDOVerification()
        {
            VM_GISDDOVerificationDetails verificationDetails = await _IGISNomineeChangeReqBLL.GetEmployeeNomineeDetailsForDDOVerification(Convert.ToInt64(Session["UID"]));
            Thread.Sleep(2000);
            //Logger.LogMessage(TracingLevel.INFO, "Pending Issues DDO " + verificationDetails.PendingApplications.ToString());
            //Logger.LogMessage(TracingLevel.INFO, "Total Issues DDO " + verificationDetails.TotalReceived.ToString());
            return View("GIS_NCR_DDO", verificationDetails);

        }

        public ActionResult GIS_NCR_DDOVerification(long empId = 0, long applicationId = 0)
        {
            if (Session["UID"] != null)
            {

                VM_GISDeptVerificationDetails verificationDetails = new VM_GISDeptVerificationDetails();
                if (empId != 0 && applicationId != 0)
                {
                    TempData["empId"] = empId;
                    TempData["applicationId"] = applicationId;
                }
                if (TempData["empId"] != null && TempData["applicationId"] != null)
                {
                    empId = Convert.ToInt32(TempData.Peek("empId"));
                    applicationId = Convert.ToInt32(TempData.Peek("applicationId"));
                    TempData.Keep("empId");
                    TempData.Keep("applicationId");
                    if (empId != 0)
                    {
                        Session["RUID"] = empId;
                        //verificationDetails = _INBApplicationbll.GetMedicalLeaveDetails(empId, applicationId);
                        //VM_DeptVerificationDetails policyCalculationDetails = _INBApplicationbll.GetPolicyCalculations(empId, applicationId);
                        //if (policyCalculationDetails != null)
                        //{
                        //    verificationDetails.LoadFactor = policyCalculationDetails.LoadFactor;
                        //    verificationDetails.SumAssured = policyCalculationDetails.SumAssured;
                        //    verificationDetails.DeductionLoadFactors = policyCalculationDetails.DeductionLoadFactors;
                        //    verificationDetails.DeductionLoadFactor = policyCalculationDetails.DeductionLoadFactor;
                        //}
                        verificationDetails = _IGISNomineeChangeReqBLL.GetUploadedDocuments(empId, applicationId);//_IGroupInsuranceBLL.GetUploadedDocuments(empId ,applicationId);

                        verificationDetails.WorkFlowDetails = _IGISNomineeChangeReqBLL.GetWorkFlowDetails(applicationId);
                        verificationDetails.listUploadDocuments = (List<KGID_Models.KGID_GroupInsurance.UploadedDocuments>)_IGroupInsuranceBLL.GetUploadedAdoptionFile(empId, applicationId);

                        //Session["RUID"] = empId;
                        //verificationDetails.WorkFlowDetails = null;

                    }
                }
                else
                {
                    return RedirectToAction("DetailsForDDOVerification", "GISNomineeChangeReq");
                }
                return View(verificationDetails);
            }
            else
            {
                return RedirectToAction("DetailsForDDOVerification", "GISNomineeChangeReq");
            }
        }

        [Route("GIS_NCR_SaveDDOVData")]
        public string GIS_NCR_InsertVerifyDetails(VM_GISDeptVerificationDetails objVerifyDetails)
        {
            string result = "";
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);

            //  result = _INBApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
            result = _IGISNomineeChangeReqBLL.GISSaveVerifiedDetails(objVerifyDetails);
            if (Convert.ToInt32(result) == 1)
            {
                //if (objVerifyDetails.ApplicationStatus == 2)
                //{

                //}
                //if (objVerifyDetails.ApplicationStatus == 5)
                //{
                //    var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == objVerifyDetails.EmpCode select eb.mobile_number).FirstOrDefault();
                //    var appRefNo = (from ka in _db.tbl_kgid_application_details where ka.kad_application_id == objVerifyDetails.ApplicationId select ka.kad_kgid_application_number).FirstOrDefault();
                //    var assignedto = (from kw in _db.tbl_kgid_application_workflow_details where kw.kawt_application_id == objVerifyDetails.ApplicationId && kw.kawt_active_status == true select kw.kawt_assigned_to).FirstOrDefault();
                //    var DistrictOffice = (from ew in _db.tbl_employee_work_details
                //                          join ddo in _db.tbl_ddo_master on ew.ewd_ddo_id equals ddo.dm_ddo_id
                //                          where ew.ewd_emp_id == assignedto
                //                          select ddo.dm_ddo_office).FirstOrDefault();


                //    string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + appRefNo + " ಯು ದಿನಾಂಕ " + DateTime.Now + " ದಂದು ಜಿಲ್ಲಾ ವಿಮಾ ಕಛೇರಿ, " + DistrictOffice + " ಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ."
                //     + "– ವಿಮಾ ಇಲಾಖೆ(KGID).";
                //    //AllCommon.sendOTPMSG(mobile.ToString(), msg);                   
                //    // TempData["VerifyDetails"] = objVerifyDetails;
                //    ViewBag.VerifyDetails = objVerifyDetails;


                // }
                if (objVerifyDetails.ApplicationStatus == 2)
                {

                }
                //APPROVAL OF NOMINEE CHANGE
                if (objVerifyDetails.ApplicationStatus == 15)
                {
                    //var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == objVerifyDetails.EmpCode select eb.mobile_number).FirstOrDefault();
                    //var appRefNo = (from ka in _db.tbl_GIS_application_details where ka.gad_application_id == objVerifyDetails.ApplicationId select ka.gad_application_no).FirstOrDefault();
                    //var assignedto = (from kw in _db.tbl_GIS_NomineeReq_workflow_details where kw.gnwt_application_id == objVerifyDetails.ApplicationId && kw.gnwt_active_status == 1 select kw.gnwt_assigned_to).FirstOrDefault();
                    //var DistrictOffice = (from ew in _db.tbl_employee_work_details
                    //                      join ddo in _db.tbl_ddo_master on ew.ewd_ddo_id equals ddo.dm_ddo_id
                    //                      where ew.ewd_emp_id == assignedto
                    //                      select ddo.dm_ddo_office).FirstOrDefault();


                    //string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + appRefNo + " ಯು ದಿನಾಂಕ " + DateTime.Now + " ದಂದು ಜಿಲ್ಲಾ ವಿಮಾ ಕಛೇರಿ, " + DistrictOffice + " ಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ."
                    // + "– ವಿಮಾ ಇಲಾಖೆ(KGID).";
                    //// AllCommon.sendOTPMSG(mobile.ToString(), msg);
                    //AllCommon.sendUnicodeSMS(mobile.ToString(), msg, "1107161587541292075");
                    //TempData["VerifyDetails"] = objVerifyDetails;
                    ViewBag.VerifyDetails = objVerifyDetails;

                }

                // return result;
            }
            
               //return RedirectToAction("DetailsForDDOVerification", "GISNomineeChangeReq");
           // return RedirectToRoute("/GIS-NomineeChange-ddo/", new { area = "" });
            return result;
        }

        public async Task<int> DeleteNomineeDetails(VM_NomineeDetail objNominee)
        {
            var result = await _IGroupInsuranceBLL.GISDeleteNominee(objNominee);
            return result;
        }
    }
    }