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

using Common;
using System.Web.Configuration;
using System.Threading;
using BLL.KGIDGroupInsuranceBLL;
using System.Threading.Tasks;
using DLL.DBConnection;
using KGID_Models.KGID_GroupInsurance;
using System.Web;
using BLL.VerifyDataBLL;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using java.io;
using javax.xml.stream;
using javax.xml.stream.events;
using com.sun.org.apache.xml.@internal.security.c14n;
using java.security;
using javax.crypto;
using javax.crypto.spec;
using com.sun.org.apache.xerces.@internal.impl.dv.util;
using log4net;
using KGID.Models;

namespace KGID.Controllers
{
    public class GroupInsuranceController : Controller
    {
        // GET: GroupInsurance
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly INewEmployeeDetailsBLL _newemp;
        private readonly IDDOMasterBLL _ddomaster;
        private readonly IDeptMasterBLL _deptmaster;

        private readonly IInsuredEmployeeBll _InsuredEmployeebll;
        private readonly INBApplicationBll _INBApplicationbll;
        private readonly IUploadEmployeeBLL _uploadbll;
        private readonly IGroupInsuranceBLL _IGroupInsuranceBLL;
        private readonly IVerificationdetailsbll _Objemployee;
        private static readonly ILog Log = LogManager.GetLogger(typeof(KIIController));
        public GroupInsuranceController()
        {
            this._newemp = new NewEmployeeDetailsBLL();
            this._ddomaster = new DDOMasterBLL();
            this._deptmaster = new DeptMasterBLL();

            this._InsuredEmployeebll = new InsuredEmployeeBll();
            this._INBApplicationbll = new NBApplicationBll();
            this._uploadbll = new UploadEmployeeBLL();
            this._IGroupInsuranceBLL = new GroupInsuranceBLL();
            this._Objemployee = new Verificationdetailsbll();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GISAddEmployeeDetails()
        {
            VM_BasicDetails employeeDetails = _uploadbll.GetDDODetailsById(Convert.ToInt64(Session["UID"]));
            return View(employeeDetails);
        }

        public JsonResult AddEmployeeBasicDetails(VM_BasicDetails employeeDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            employeeDetails.created_by = Convert.ToInt32(Session["UID"]);
            string result = _IGroupInsuranceBLL.AddEmployeeBasicDetails(employeeDetails);

            if (result != "2" && result != "1")
            {
                string[] duplicate = result.Split(',');

                if (duplicate.Length > 0)
                {
                    if (duplicate.Contains("Mobile number") || duplicate.Contains("Pan number") || duplicate.Contains("Email id"))
                    {
                        result = result.TrimEnd(',');
                        message = "Provided " + result + " for the employee code is already registerd.";
                    }

                    isSuccess = false;
                }
            }

            else
            {

                try
                {
                    var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587991448971);

                    Logger.LogMessage(TracingLevel.INFO, "AddEmployeeBasicDetails 1107161587991448971" + msg.ToString());
                    Logger.LogMessage(TracingLevel.INFO, "AddEmployeeBasicDetails mobile no " + employeeDetails.mobile_number.ToString());

                    //AllCommon.sendUnicodeSMS(employeeDetails.mobile_number.ToString(), msg, "1107161587991448971");
                    Logger.LogMessage(TracingLevel.INFO, "AddEmployeeBasicDetails SMS sent ");
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "AddEmployeeBasicDetails" + ex.Message.ToString());
                }
                isSuccess = true;
                message = "Employee details saved successfully";
            }
            //else
            //{
            //    isSuccess = false;
            //    message = "Provided Mobile number/Pan number/Email id for the employee code is already registerd.";
            //}

            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }

        [Route("gis-view-app")]
        public async Task<ActionResult> GISApplicationForEmployee()
        {
            // VM_ApplicationDetail model = new VM_ApplicationDetail();
            //VM_BasicDetails model = new VM_BasicDetails();
            long empId = Convert.ToInt64(Session["UID"]);
            //long refNum = _IGroupInsuranceBLL.GenerateGISApplicationNumber(empId);
            VM_ApplicationDetail applicationDetails = await _IGroupInsuranceBLL.GenerateGISApplicationNumber(empId);
            // model.ApplicationNumber = refNum.ToString();
            // // VM_BasicDetails model1 = new VM_BasicDetails();   
            //  model = await _IGroupInsuranceBLL.GISGetEmployeeDetails(empId);

            long applstatus = await _IGroupInsuranceBLL.GetGISApplicationStatus(empId);
            ViewBag.paymentStatus = applicationDetails.PaymentStatus;

            if (applstatus == 15)
            {
                VM_GISDDOVerificationDetails verificationDetails;
                //  verificationDetails = _IGroupInsuranceBLL.GISGetSubscriptionDetails(empId);
                verificationDetails = _IGroupInsuranceBLL.GISGetEmployeeApplicationStatusDll(Convert.ToInt64(Session["UId"]));
                if (verificationDetails.IEmployeeVerificationDetails.Count > 0)
                {
                    foreach (var i in verificationDetails.IEmployeeVerificationDetails)
                    { i.amtinwords = ConvertToRupee(i.SavingInsuranceAmt.ToString()); }
                }



                return View("ViewGISApplicationDetails", verificationDetails);
                //ViewBag.applicationstatus = "Approved";
            }
            if (applicationDetails.ApplicationCount == 0)
            {
                if (applicationDetails.RestrictApplyingPolicy > 50)
                {
                    ViewBag.RestrictApplyingPolicy = true;
                }
                else
                {
                    ViewBag.RestrictApplyingPolicy = false;
                    ViewBag.ApplicationProcess = false;
                    if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationNumber))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(applicationDetails.ApplicationNumber, QRCodeGenerator.ECCLevel.Q);
                            using (Bitmap bitMap = qrCode.GetGraphic(20))
                            {
                                bitMap.Save(ms, ImageFormat.Png);
                                applicationDetails.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                            }
                        }
                    }
                }
            }
            else
            {

                //if (applstatus == 15) 
                //{
                //    VM_GISDDOVerificationDetails verificationDetails = _IGroupInsuranceBLL.GISGetEmployeeApplicationStatusDll(Convert.ToInt64(Session["UId"]));

                //    return View(verificationDetails);
                //    //ViewBag.applicationstatus = "Approved";
                //}
                //else

                if (applstatus == 2)
                {
                    // allow to make changes
                    ViewBag.ApplicationProcess = false;
                    ViewBag.EditApplication = true;
                }
                else if (applstatus == 3 || applstatus == 4)
                {
                    // restrict access
                    ViewBag.ApplicationProcess = true;
                    ViewBag.Refnumber = applicationDetails.ApplicationNumber;
                    //now  VM_GISDeptVerificationDetails
                    VM_GISDeptVerificationDetails verificationDetails = new VM_GISDeptVerificationDetails();
                    verificationDetails.WorkFlowDetails = _IGroupInsuranceBLL.GetWorkFlowDetails(applicationDetails.ApplicationId);
                    return View("GISViewApplStatusforEmployee", verificationDetails);
                }



            }
            Session["UID"] = empId;
            return View(applicationDetails);
        }

        public ActionResult GISEmployeeBasicDetails()
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
            return PartialView("GISEmployeeBasicDetails", model);
        }


        public ActionResult GISNomineeDetails()
        {
            VM_BasicDetails model = new VM_BasicDetails();
            long empId = Convert.ToInt64(Session["UID"]);
            model = _IGroupInsuranceBLL.GISGetEmployeeDetails(empId);
            if (model.spouse_name != null)
            { ViewBag.ismarried = true; }
            else
            { ViewBag.ismarried = false; }
            // ViewBag.ismarried = model.eod_emp_married;
            ViewBag.isOrphan = model.eod_emp_orphan;
            //bool ismarried = model.eod_emp_married;

            VM_NomineeDetails _NomineeData = new VM_NomineeDetails();
            _NomineeData.NomineeDetails = _IGroupInsuranceBLL.GISNomineeDetailsDll(empId);
            _NomineeData.EmployeeId = Convert.ToInt64(Session["UID"]); ;
            _NomineeData.ddlNomineeList = _IGroupInsuranceBLL.GetNomineelist(empId);


            //
            //if (_NomineeData == null) 
            //{
            //    _NomineeData = new VM_NomineeDetails();
            //}
            //   float ip = _IGroupInsuranceBLL.GisGetIntialPaymentDetails(empId);  -- not used check once 
            return PartialView("GISNomineeDetails", _NomineeData);
        }

        public JsonResult GISNomineeDetailsList()
        {
            VM_BasicDetails model = new VM_BasicDetails();
            long empId = Convert.ToInt64(Session["UID"]);
            model = _IGroupInsuranceBLL.GISGetEmployeeDetails(empId);
            ViewBag.ismarried = model.eod_emp_married;
            ViewBag.isOrphan = model.eod_emp_orphan;
            //bool ismarried = model.eod_emp_married;

            VM_NomineeDetails _NomineeData = new VM_NomineeDetails();
            _NomineeData.NomineeDetails = _IGroupInsuranceBLL.GISNomineeDetailsDll(empId);
            _NomineeData.EmployeeId = Convert.ToInt64(Session["UID"]); ;
            //_NomineeData.ddlNomineeList = _IGroupInsuranceBLL.GetNomineelist(empId);
            //

            //
            //if (_NomineeData == null) 
            //{
            //    _NomineeData = new VM_NomineeDetails();
            //}
            //   float ip = _IGroupInsuranceBLL.GisGetIntialPaymentDetails(empId);  -- not used check once 
            return Json(_NomineeData, JsonRequestBehavior.AllowGet); ;
        }
        //public JsonResult GetNomineeNameList(long empId, string type)
        //{
        //    BindDropDownModel result = new BindDropDownModel();
        //    result = _INBApplicationbll.GetNomineeNameListBll(empId, type);
        //    return Json(result, JsonRequestBehavior.AllowGet); ;
        //}

        public async Task<long> GISInsertBasicDetails(VM_BasicDetails objBasicDetails)
        {
            long empId = Convert.ToInt64(Session["UID"]);
            objBasicDetails.employee_id = empId;
            long result = await _IGroupInsuranceBLL.GISSaveEmpBasicDetails(objBasicDetails);
            //long result = _INBApplicationbll.SaveNBBasicBll(objBasicDetails);
            //BasicDetailsToView();

            Session["AppRefNumber"] = result;

            return result;
        }

        public Task<long> GISInsertNomineeDetails(VM_NomineeDetail objNomineeDetails)
        {
          
            string path = UploadDocumentNominee(objNomineeDetails.SonDaughterAdoption_doc, (long)objNomineeDetails.EmpId, "AdoptionDocument");
            objNomineeDetails.SonDaughterAdoption_doc_path = path;
            var result = _IGroupInsuranceBLL.GISSaveNBNominee(objNomineeDetails);


            return result;
        }
        //UPLOAD DOCUMENT FOR ADOPTION SON/ DAUGHTER
        private string UploadDocumentNominee(HttpPostedFileBase document, long empId, string docType)
        {
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
                    }
                }
                string FileNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                path = subPath + empId.ToString() + FileNo + fileName;
                document.SaveAs(path);
            }
            return path;
        }
        public async Task<int> DeleteNomineeDetails(VM_NomineeDetail objNominee)
        {
            var result = await _IGroupInsuranceBLL.GISDeleteNominee(objNominee);
            return result;
        }

        public ActionResult GISInitialPayment()
        {
            long empId = Convert.ToInt64(Session["UID"]);
            // float ip= _IGroupInsuranceBLL.GisGetIntialPaymentDetails(empId);
            VM_GISPaymentDetails obj = new VM_GISPaymentDetails();
            obj = _IGroupInsuranceBLL.GISPaymentDll(empId);
            if (obj.gcd_date_of_generation != null)
            {
                DateTime temp = DateTime.Parse(obj.gcd_date_of_generation );
                obj.gcd_date_of_generation = temp.ToShortDateString();
            }
            // return null;
            if (obj.gcd_savings_amount == 0)
            { return this.PartialView("GISInitialPayment", obj); }
            else 
            { return this.PartialView("GISInitialPayment2", obj); }
            
        }


        public JsonResult GISInsertChallanDetails(VM_GISPaymentDetails objChalnDetails)
        {
            //int result = _INBApplicationbll.SaveNBPayscaleBll(objKGIDDetails);
            long result = _IGroupInsuranceBLL.GISSavePaymentDll(objChalnDetails);
            //if(objChalnDetails.cd_amount == null)
            //{
            //    objChalnDetails.cd_amount = 0;
            //}
            //int result = 0;
            //   int result = _INBApplicationbll.SaveChallanBll(objChalnDetails);
            if (result == 1)
            {
                //TempData["ChallanDetails"] = objChalnDetails;
                ViewBag.chaldetails = objChalnDetails;
            }

            bool isSuccess = false;
            string message = string.Empty;
            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
        }


        public long InsertChalanStatusDetails(VM_GISPaymentDetails objPaymentDetails)
        {
            //long result = _INBApplicationbll.SaveNBChallanStatusDll(objPaymentDetails);
            long result = _IGroupInsuranceBLL.GISSaveChallanStatusDll(objPaymentDetails);
            return result;
        }
        //public ActionResult PaymentDetailsToView()
        //{
        //    VM_PaymentDetails obj = _INBApplicationbll.NBPaymentDll(Convert.ToInt64(Session["UID"]));
        //    return this.PartialView("_PaymentDetails", obj);
        //}



        public ActionResult GISDeclaration()
        {
            return this.PartialView();

        }

        //  public JsonResult GISSaveDeclaration(long EmpId, long AppId)
        public int GISSaveDeclaration(long EmpId, long AppId)
        {
            int result = 1;
            string message = string.Empty;
            result = _IGroupInsuranceBLL.GISSaveDeclaration(EmpId, AppId);
            //  return View();
            //  return Json(new { result = result, Message = message }, JsonRequestBehavior.AllowGet);
            return result;
        }

        //
        [Route("GIS-ddo")]
        public ActionResult DetailsForDDOVerification()
        {


            VM_GISDDOVerificationDetails verificationDetails = _IGroupInsuranceBLL.GetEmployeeDetailsForDDOVerification(Convert.ToInt64(Session["UID"]));
            Thread.Sleep(2000);
            Logger.LogMessage(TracingLevel.INFO, "Pending Issues DDO " + verificationDetails.PendingApplications.ToString());
            Logger.LogMessage(TracingLevel.INFO, "Total Issues DDO " + verificationDetails.TotalReceived.ToString());
            return View("GISDDO", verificationDetails);

        }

        //[Route("gis_ddo_va")]
        [Route("gis_ddo_va/{empId}/{applicationId}")]
        public ActionResult GISDDOVerification(long empId = 0, long applicationId = 0)
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

                        verificationDetails = _IGroupInsuranceBLL.GetUploadedDocuments(empId, applicationId);//_IGroupInsuranceBLL.GetUploadedDocuments(empId ,applicationId);
                        verificationDetails.WorkFlowDetails = _IGroupInsuranceBLL.GetWorkFlowDetails(applicationId);
                        verificationDetails.listUploadDocuments = (List<KGID_Models.KGID_GroupInsurance.UploadedDocuments>)_IGroupInsuranceBLL.GetUploadedAdoptionFile(empId, applicationId);



                        Session["RUID"] = empId;
                        // verificationDetails.WorkFlowDetails = null;

                    }
                }
                else
                {
                    return RedirectToAction("DetailsForDDOVerification", "GroupInsurance");
                }
                return View(verificationDetails);
            }
            else
            {
                return RedirectToAction("DetailsForDDOVerification", "GroupInsurance");
            }
        }



        // DETAILS  TO VIEW BY DDO  --  VIEW DATA TO VERIFY -- DDO 
        #region

        public ActionResult GISBasicDetailsToView()
        {


            VM_BasicDetails _BasicData = _IGroupInsuranceBLL.GISGetEmployeeDetails(Convert.ToInt64(Session["RUID"]));
            if (_BasicData == null)
            {
                _BasicData = new VM_BasicDetails();
            }
            else
            {
                //if (_BasicData.kad_kgid_application_number != null && _BasicData.kad_kgid_application_number != "")
                //{
                //    using (MemoryStream ms = new MemoryStream())
                //    {
                //        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                //        QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(_BasicData.kad_kgid_application_number.ToString(), QRCodeGenerator.ECCLevel.Q);
                //        using (Bitmap bitMap = qrCode.GetGraphic(20))
                //        {
                //            bitMap.Save(ms, ImageFormat.Png);
                //            _BasicData.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                //        }
                //    }
                //}
            }
            return this.PartialView("GISBasicDetailsToView", _BasicData);
        }
        public ActionResult KGIDDetailsToView()
        {
            VM_PolicyDetails _KGIDData = _INBApplicationbll.KGIDDetailsBll(Convert.ToInt64(Session["RUID"]));
            if (_KGIDData == null)
            {
                _KGIDData = new VM_PolicyDetails();
            }

            return this.PartialView("_KGIDDetails", _KGIDData);
        }
        public ActionResult GISNomineeDetailsToView()
        {
            //  VM_NomineeDetails _NomineeData = _INBApplicationbll.NomineeDetailsBll(Convert.ToInt64(Session["RUID"]));
            //VM_NomineeDetails _NomineeData = _IGroupInsuranceBLL.GISNomineeDetailsDll(Convert.ToInt64(Session["RUID"]));

            VM_NomineeDetails _NomineeData = new VM_NomineeDetails();
            _NomineeData.NomineeDetails = _IGroupInsuranceBLL.GISNomineeDetailsDll(Convert.ToInt64(Session["RUID"]));

            if (_NomineeData == null)
            {
                _NomineeData = new VM_NomineeDetails();
            }

            return this.PartialView("GISNomineeDetailsToView", _NomineeData);
        }
        public ActionResult GISPaymentDetailsToView(int EmpId, int AppId)
        {
            long EmpId1 = Convert.ToInt64(Session["RUID"]);


            VM_ChallanPrintDetails ChallanDetails = new VM_ChallanPrintDetails();
            ChallanDetails = _IGroupInsuranceBLL.ChallanprintDetails(EmpId1, AppId);

            ChallanDetails.LastUpdatedDateTime = Convert.ToDateTime(ChallanDetails.LastUpdatedDateTime);
            ChallanDetails.challan_date = (ChallanDetails.challan_date);

            //return this.PartialView("_GISPaymentDetails", ChallanDetails);

            return this.PartialView("GISPaymentDetailsToView", ChallanDetails);

            //VM_ChallanPrintDetails NBChallanDetails = new VM_ChallanPrintDetails();
            //NBChallanDetails = _newemp.ChallanprintDetailsBLL(EmpId, AppId);
            ////NBChallanDetails.challan_ref_no = "CHN" + DateTime.Now.ToShortDateString() + "_" + EmpId + "_" + AppId;
            //return this.PartialView("_PaymentDetails", NBChallanDetails);
        }


        public ActionResult DeclarationDetailsToView()
        {
            tbl_nb_declaration_master _DeclarationData = _INBApplicationbll.DeclarationDetailsBll(Convert.ToInt64(Session["RUID"]));
            if (_DeclarationData == null)
            {
                _DeclarationData = new tbl_nb_declaration_master();
            }

            return this.PartialView("_DeclarationDetails", _DeclarationData);
        }
        #endregion


        [Route("GISSaveDDOVData")]
        public string GISInsertVerifyDetails(VM_GISDeptVerificationDetails objVerifyDetails)
        {
            string result = "";
            objVerifyDetails.CreatedBy = Convert.ToInt32(Session["UID"]);
            objVerifyDetails.EmpCode = Convert.ToInt32(Session["UID"]);

            //  result = _INBApplicationbll.SaveVerifiedDetailsBll(objVerifyDetails);
            result = _IGroupInsuranceBLL.GISSaveVerifiedDetails(objVerifyDetails);
            if (Convert.ToInt32(result) == 1)
            {
                if (objVerifyDetails.ApplicationStatus == 2)
                {

                }
                if (objVerifyDetails.ApplicationStatus == 15)
                {
                    //var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == objVerifyDetails.EmpCode select eb.mobile_number).FirstOrDefault();
                    //var appRefNo = (from ka in _db.tbl_GIS_application_details where ka.gad_application_id == objVerifyDetails.ApplicationId select ka.gad_application_no).FirstOrDefault();
                    //var assignedto = (from kw in _db.tbl_GIS_application_workflow_details where kw.gawt_application_id == objVerifyDetails.ApplicationId && kw.gawt_active_status == 1 select kw.gawt_assigned_to).FirstOrDefault();
                    //var DistrictOffice = (from ew in _db.tbl_employee_work_details
                    //                      join ddo in _db.tbl_ddo_master on ew.ewd_ddo_id equals ddo.dm_ddo_id
                    //                      where ew.ewd_emp_id == assignedto
                    //                      select ddo.dm_ddo_office).FirstOrDefault();


                    //string msg = "ವಿಮಾ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ " + appRefNo + " ಯು ದಿನಾಂಕ " + DateTime.Now + " ದಂದು ಜಿಲ್ಲಾ ವಿಮಾ ಕಛೇರಿ, " + DistrictOffice + " ಗೆ ಸಲ್ಲಿಕೆಯಾಗಿದೆ."
                    // + "– ವಿಮಾ ಇಲಾಖೆ(KGID).";
                    //// AllCommon.sendOTPMSG(mobile.ToString(), msg);
                    //AllCommon.sendUnicodeSMS(mobile.ToString(), msg, "1107161587541292075");
                    //// TempData["VerifyDetails"] = objVerifyDetails;
                    ViewBag.VerifyDetails = objVerifyDetails;


                }
            }
            //return RedirectToRoute("/kgid-ddo/", new { area = "" });
            return result;
        }







        /////////////////////////
        ///

        public ActionResult PBasicDetailsToView()
        {
            //VM_BasicDetails _BasicData = _INBApplicationbll.BasicDetailsBll(Convert.ToInt64(Session["UID"]));
            //if (_BasicData == null)
            //{
            //    _BasicData = new VM_BasicDetails();
            //}

            VM_BasicDetails _BasicData = new VM_BasicDetails();
            long empId = Convert.ToInt64(Session["UID"]);
            _BasicData = _IGroupInsuranceBLL.GISGetEmployeeDetails(empId);
            if (_BasicData == null)
            {
                _BasicData = new VM_BasicDetails();
            }
            else
            {
                if (_BasicData.kad_kgid_application_number != null || _BasicData.kad_kgid_application_number != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(_BasicData.kad_kgid_application_number.ToString(), QRCodeGenerator.ECCLevel.Q);
                        using (Bitmap bitMap = qrCode.GetGraphic(20))
                        {
                            bitMap.Save(ms, ImageFormat.Png);
                            _BasicData.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            return this.PartialView("PrintBasicDetails", _BasicData);
        }


        public ActionResult PNomineeDetailsToView()
        {
            long empid = (long)Convert.ToInt64(Session["UID"]);
            VM_NomineeDetails _NomineeData = new VM_NomineeDetails();
            _NomineeData.NomineeDetails = _IGroupInsuranceBLL.GISNomineeDetailsDll(empid);
            if (_NomineeData == null)
            {
                _NomineeData = new VM_NomineeDetails();
            }

            return this.PartialView("PrintNomineeDetails", _NomineeData);
        }



        // ChallanprintDetails


        public ActionResult PrintChallanDetails(long EmpId, long AppId, long challanNo = 0)
        {
            long EID = Convert.ToInt64(Session["UID"]);
            if (EID == EmpId)
            {
                VM_ChallanPrintDetails NBChallanDetails = new VM_ChallanPrintDetails();
                NBChallanDetails = _IGroupInsuranceBLL.ChallanprintDetails(EmpId, AppId);
                //  NBChallanDetails = _newemp.ChallanprintDetailsBLL(EmpId, AppId);
                string filepath = FillNBChallan(NBChallanDetails, AppId);
                //return View("FacingSheet", facingSheet1);
                return File(filepath, "application/pdf");
            }
            else
            {
                return View();
            }
            //return NBChallanDetails;
        }
        private string FillNBChallan(VM_ChallanPrintDetails NBChallanDetails, long result)
        {
            //DateTime dt = DateTime.Now;
            string amtwords = ConvertToWords(Convert.ToString(NBChallanDetails.p_premium));
            NBChallanDetails.Category = "Government";
            NBChallanDetails.GrandTotal = Convert.ToString(NBChallanDetails.p_premium);
            //KG MONTH YEAR 8011 01-8digits
            //string my = dt.ToString("MMyy");
            //string timestamp = dt.ToString("hhmmssff");
            string challanrefno = NBChallanDetails.challan_ref_no;
            //   string challanrefno = "SDDSADADA";

            //NBChallanDetails.challan_ref_no = challanrefno;
            ////////////////////////////////////////////////////////////////
            string pdfTemplate = Server.MapPath("~/Challans/NB/Challan_NB_Test.pdf");
            //string newFile = Server.MapPath("~/Challans/NB/" + challanrefno + ".pdf");
            //string newFile = @"C:/Documents/Challans/NB/" + challanrefno + ".pdf";
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\Challans\NB\" + challanrefno + ".pdf";
            }
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create, FileAccess.ReadWrite));
            AcroFields fields = pdfStamper.AcroFields;
            {//Facing Sheet Details
             //var date1 = facingSheet.DateOfIssue?.ToString("dd-MM-yyyy");

                //  fields.SetField("ChallanValidity", NBChallanDetails.Challan_Validity);
                //  fields.SetField("District", NBChallanDetails.dm_name_english.ToString().Trim().ToUpper());
                //  fields.SetField("Department", NBChallanDetails.dm_deptname_english);
                //   fields.SetField("DDOOffice", NBChallanDetails.dm_ddo_office);
                fields.SetField("Category", NBChallanDetails.Category);
                //fields.SetField("Date", NBChallanDetails.challan_date.ToString().Trim().ToUpper());
                fields.SetField("Date", NBChallanDetails.challan_date);
                fields.SetField("ChallanReferenceNo", NBChallanDetails.challan_ref_no);
                fields.SetField("ChallanStatus", NBChallanDetails.challan_status);
                fields.SetField("DDOCode", NBChallanDetails.dm_ddo_code);
                //fields.SetField("RemitterName", NBChallanDetails.employee_name.ToString().Trim().ToUpper());
                // fields.SetField("MobileNo", NBChallanDetails.mobile_number.ToString().Trim().ToUpper());
                //  fields.SetField("Address", NBChallanDetails.ead_address);

                fields.SetField("Purpose", NBChallanDetails.purpose_desc);
                fields.SetField("HOA", NBChallanDetails.hoa_desc);
                fields.SetField("SubPurposeName", NBChallanDetails.sub_purpose_desc);
                fields.SetField("PurposeSpecificID", NBChallanDetails.purpose_id);
                fields.SetField("Amount", NBChallanDetails.p_premium.ToString().Trim().ToUpper());
                // fields.SetField("RemittanceBank", NBChallanDetails.RemittanceBank);
                fields.SetField("GrandTotal", NBChallanDetails.GrandTotal);
                fields.SetField("TotalAmountinWords", amtwords);
                //fields.SetField("ChequeDDNo", NBChallanDetails.Cheque_DD_No);
                //fields.SetField("ChequeDDBank", NBChallanDetails.Cheque_DD_Bank);
                //fields.SetField("IFSCode", NBChallanDetails.IFSC_Code);
                //fields.SetField("MICRCode", NBChallanDetails.MICR_Code);
                //fields.SetField("ChequeDDDate", NBChallanDetails.Cheque_DD_Date.ToString().Trim().ToUpper());
                //fields.SetField("ChequeDDDate", NBChallanDetails.Cheque_DD_Date?.ToString("dd-MM-yyyy"));
            }
            pdfStamper.Close();
            return newFile;
        }


     
        [Route("gis-upload-emp-data")]
        public ActionResult GISUploadEmployeeData()
        {

            DbConnectionKGID _db = new DbConnectionKGID();
            VM_GIS_Upload_EmployeeForm objUEData = new VM_GIS_Upload_EmployeeForm();

            objUEData.App_Employee_Code = Convert.ToInt64(Session["UID"]);
            if (objUEData.App_Employee_Code != 0)
            {
                //objUEData = _Objemployee.GetUploadDocBll(objUEData.App_Employee_Code);
                objUEData = _IGroupInsuranceBLL.GetUploadDoc(objUEData.App_Employee_Code);
            }
            objUEData.App_Employee_Code = Convert.ToInt64(Session["UID"]);
            return this.PartialView(objUEData);
        }
       
        private string UploadDocument(HttpPostedFileBase document, long empId, string docType)
        {
            string path = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);




                string subPath = string.Empty;// = "~/EmployeeDocuments/" + empId.ToString() + "/" + docType;

                if (docType == "ApplicationForm")
                {

                    // subPath = @"C:/Users/CSG/Documents/Challans/";// + empId.ToString() + "/" + docType;
                    if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    {

                        if (!Directory.Exists(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\ApplicationForm\" + empId))
                            Directory.CreateDirectory(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\ApplicationForm\" + empId);
                        subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\ApplicationForm\" + empId + "\\";
                        // subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"ApplicationForm\";
                    }
                }
                else if (docType == "Form6")
                {
                    // subPath = @"C:/Users/CSG/Documents/Form6/";
                    //subPath = @"F:/Documents/EmployeeDocuments/MedicalForm/";
                    if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    {
                        if (!Directory.Exists(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\Form6\" + empId))
                            Directory.CreateDirectory(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\Form6\" + empId);
                        subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\Form6\" + empId + "\\";
                        // subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"Form6\";
                    }
                }
                else if (docType == "Form7")
                {
                    //subPath = @"C:/Users/CSG/Documents/Form7/";
                    //subPath = @"F:/Documents/EmployeeDocuments/MedicalForm/";
                    if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
                    {
                        if (!Directory.Exists(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\Form7\" + empId))
                            Directory.CreateDirectory(WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\Form7\" + empId);
                        subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\Form7\" + empId + "\\";
                        //subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"Form7\";
                    }
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
        [Route("gis-upload-form-data")]
        public ActionResult GISUploadEmployeeData(VM_GIS_Upload_EmployeeForm objUF)
        {
            int result = 0;
            tbl_GIS_upload_employeeform objEmpForm = new tbl_GIS_upload_employeeform();
            if (ModelState.IsValid)
            {
                //string path = Server.MapPath("~/Uploads/");
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                string AppPath = UploadDocument(objUF.ApplicationFormDoc, objUF.App_Employee_Code, "ApplicationForm");
                string Form6Path = UploadDocument(objUF.Form6Doc, objUF.App_Employee_Code, "Form6");
                string Form7Path = UploadDocument(objUF.Form7Doc, objUF.App_Employee_Code, "Form7");
                objEmpForm.App_Employee_Code = Convert.ToInt64(Session["UID"]);
                objEmpForm.App_Application_Form = AppPath;
                objEmpForm.App_Form6 = Form6Path;
                objEmpForm.App_Form7 = Form7Path;
                //string SubPathForApp = "/EmployeeDocuments/" + objEmpForm.App_Employee_Code.ToString() + "/ApplicationForm/";
                //string SubPathForMed = "/EmployeeDocuments/" + objEmpForm.App_Employee_Code.ToString() + "/MedicalForm/";

                //objEmpForm.App_Application_Form = SubPathForApp + objUF.ApplicationFormDoc.FileName;
                objEmpForm.App_Application_Form = AppPath;
                objEmpForm.App_Form6 = Form6Path;
                objEmpForm.App_Form7 = Form7Path;
                //objEmpForm.App_Medical_Form = SubPathForMed + objUF.MedicalFormDoc.FileName;



                result = _IGroupInsuranceBLL.GISSaveEmployeeForm(objEmpForm);
                if (result == 1)
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
                    return Json(new { RedirectUrl = "/gis-view-app/", Result = result }, JsonRequestBehavior.AllowGet);
                }
                else if (result == 2)
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
                    return Json(new { RedirectUrl = "/gis-view-app/", Result = result }, JsonRequestBehavior.AllowGet);
                }
            }
            //return View(objEmpForm);
            //return RedirectToAction("UploadEmployeeData", "VerifyData");
            //return Json(new { RedirectUrl = "/gis-upload-emp-data/", Result = result }, JsonRequestBehavior.AllowGet);
            return Json(new { RedirectUrl = "/gis-view-app/", Result = result }, JsonRequestBehavior.AllowGet);
            //return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }
  
        public ActionResult UploadEmployeeDownload(int id)
        {
            VM_GIS_Upload_EmployeeForm objUEData = new VM_GIS_Upload_EmployeeForm();
            objUEData.App_Employee_Code = Convert.ToInt64(Session["UID"]);
            if (objUEData.App_Employee_Code != 0)
            {
                objUEData = _IGroupInsuranceBLL.GetUploadDocDll(objUEData.App_Employee_Code);
            }
            string fullPath = "";
            if (id == 1)
            {
                fullPath = objUEData.ApplicationFormDocName;
            }
            else if (id == 2)
            {
                fullPath = objUEData.Form6DocName;
            }
            else if (id == 3)
            {
                fullPath = objUEData.Form7DocName;
            }
            //string fullPath = Path.Combine(Server.MapPath("~/Images/ArticleOfAssociations"), _filepathAppDoc);
            //string fullPath = Server.MapPath("~/EmployeeDocuments/900811781/ApplicationForm/UntitledDocument.pdf");

            return File(fullPath, "application/pdf");

            //return View(objUEData);
        }

        // GetPathforDownload


        string[] words0 = { "Zero ", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine ", "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen ", "Twenty " };
        string[] words2 = { "Zero ", "Ten ", "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety ", "Hundred " };
        string[] words3 = { "Hundred ", "Thousand ", "Lakh ", "Crore " };
        int[] numbers = new int[] { 0, 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 };
        string numstr;
        string words = "";
        int tempNum;
        int temp = 0;
        private string ConvertToRupee(string number)
        {
            numstr = number.ToString();
            words = "";
            tempNum = Convert.ToInt32(number);
            temp = 0;
            while (numstr != "0" && numstr.Length != 0)
            {
                switch (numstr.Length)
                {
                    case 1:
                        words += words0[tempNum];
                        numstr = "";
                        break;
                    case 2:
                        if (tempNum <= 20)
                        {
                            words += words0[tempNum];
                            numstr = "";
                        }
                        else
                        {
                            temp = tempNum / numbers[2];
                            words += words2[temp];
                            tempNum = tempNum % numbers[2];
                            numstr = tempNum.ToString();
                        }
                        break;
                    case 3:
                        Method1(3, "Hundred ");
                        break;
                    case 4:
                        Method1(4, "Thousand ");
                        break;
                    case 5:
                        Method2(4, "Thousand ");
                        break;
                    case 6:
                        Method1(6, "Lakh ");
                        break;
                    case 7:
                        Method2(6, "Lakh ");
                        break;
                    case 8:
                        Method1(8, "Crore ");
                        break;
                    case 9:
                        Method2(8, "Crore ");
                        break;
                    default:
                        break;
                }
            }
            words += "Rupees";
            return words;
        }

        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }
        private void Method1(int n, string wo)
        {
            temp = tempNum / numbers[n];
            words += words0[temp] + wo;
            tempNum = tempNum % numbers[n];
            numstr = tempNum.ToString();
        }
        private void Method2(int n, string wo)
        {
            temp = tempNum / numbers[n];
            if (temp == 10)
                words += words0[temp] + wo;
            else if (temp <= 20)
                words += words0[temp] + wo;
            else
            {
                int twoDig = temp / numbers[2];
                int digit = temp % numbers[2];
                words += words2[twoDig] + words0[digit] + wo;
            }
            tempNum = tempNum % numbers[n];
            numstr = tempNum.ToString();
        }


        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents    
                        endStr = "Paisa " + endStr;//Cents    
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertToRupee(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }


   

        #region KII
        string deptRefNum = string.Empty;
        long amount = 0;
        string transactiondate = string.Empty;
        string chlntransactiondate = string.Empty;
        [HttpPost]
        public void IndexPost(VM_GISPaymentDetails objPaymentDetails)//KIIRequest request
        {
            Session.Timeout = Session.Timeout + 15;
            Logger.LogMessage(TracingLevel.INFO, "IndexPost-Enter to method");
            _IGroupInsuranceBLL.SaveLogs("IndexPost-Enter to method", Convert.ToInt64(Session["UID"]));
            Session["KIIReturn"] = "GISPay";
            //Logger.LogMessage(TracingLevel.INFO, "Create Cookie Start:" );
            //HttpCookie k2return = new HttpCookie("k2return");
            //k2return.Value="NBPay";
            ////k2return.Expires = DateTime.Now.AddHours(1);
            //Response.SetCookie(k2return);
            //Response.Flush();
            //string Emp = Session["UID"].ToString();
            //HttpCookie empid = new HttpCookie("empid");
            //empid.Value = Emp;
            ////empid.Expires = DateTime.Now.AddHours(1);
            //Response.SetCookie(empid);
            //Response.Flush();
            //Logger.LogMessage(TracingLevel.INFO, "Create Cookie End:");
            // Get Payment Details
          //  VM_PaymentDetails objPD = _INBApplicationbll.NBPaymentDll(Convert.ToInt64(Session["UID"]));  -- dd cc

            VM_GISPaymentDetails objPD = _IGroupInsuranceBLL.GISPaymentDll(Convert.ToInt64(Session["UID"]));
            objPaymentDetails.gcd_amount = objPD.gcd_amount;         // objPaymentDetails.cd_amount = objPD.cd_amount; -- dd cc
                                                                    //
            long result = _IGroupInsuranceBLL.GISSavePaymentDll(objPaymentDetails);   // long result = _INBApplicationbll.SaveNBPaymentBll(objPaymentDetails);-- dd cc
            //long result = 0;
            if (result == 1)
            {
                string dd = DateTime.Now.ToString("dd");
                string MM = DateTime.Now.ToString("MM");
                string yy = DateTime.Now.ToString("yy");
                string ddHHmmss = DateTime.Now.ToString("ddHHmmss");
                transactiondate = DateTime.Now.ToString("ddMMyyyy");
                chlntransactiondate = DateTime.Now.ToString("dd/MM/yyyy").Replace('-', '/');

                //deptRefNum = "KD" + MM + yy + "8011" + ddHHmmss;
                //string testdataenc = "VhrkkSQ5YXM%2BZJ49L439AKCEJZzsN5PNVD1WMGXbOu0XTy60BJSDp8LJDWviKwGTPX0we7NfLgxa%0D%0AN7iabx7f837vvVNeNjp4dfUJTBLRN5wAxuqTLDbXRylnuM%2F2e8hZiGjY4LDeafJ53cab7dai6XIf%0D%0Axcp5gxMg1TmYN4DmpadwzNCOsKF8W8g8A7FUeW05%2F3w35rFXH1XmmWW45AVevd8Y3dDikSZlX1%2BG%0D%0AQpm9ZE%2BGf4gbQgmxP4CKObQ7W6epxpazPTxnTD30FKeMOiRKfAY9ByYLgG48QKRIrBVHZcGvZq58%0D%0A0y7MM5ZEW3rB5EMg";
                //var dec = HttpUtility.UrlDecode(testdataenc);
                //dec.Replace(" ", "");
                //var resdecData = SymmetricDecrypt(dec, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                //string SymmetricDecryptData11 = SymmetricDecrypt(testdataenc, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                //objPaymentDetails.cd_amount = 1;
                //objPaymentDetails.cd_challan_ref_no = deptRefNum;
                amount = Convert.ToInt64(objPaymentDetails.gcd_amount); //amount = Convert.ToInt64(objPaymentDetails.cd_amount); -- dd cc
                deptRefNum = objPaymentDetails.gcd_challan_ref_no;  // deptRefNum = objPaymentDetails.cd_challan_ref_no; -- dd cc
                Log.Debug(deptRefNum);
                Log.Debug(amount);
                ///////KII Integration Start//////
                ByteArrayOutputStream fileWriter = null;
                StringBuilder content = null;
                string currPath = string.Empty;
                string SignedresultContent = string.Empty;
                string KIIsignresponse = string.Empty;
                string resFile;
                if (objPaymentDetails.gcd_savings_amount == 0)
                {
                    resFile = TextFileCreate(Convert.ToInt64(amount), deptRefNum, objPaymentDetails.EmpName, objPaymentDetails.hoa, objPaymentDetails.ddo_code, objPaymentDetails.gcd_purpose_id, objPaymentDetails.sub_purpose_desc); // -- dd check sub purpose desc
    
                }
                else
                {
                    resFile = TextFileCreate1(Convert.ToInt64(amount), deptRefNum, objPaymentDetails.EmpName, objPaymentDetails.hoa, objPaymentDetails.ddo_code, objPaymentDetails.gcd_purpose_id, objPaymentDetails.sub_purpose_desc, objPaymentDetails.hoa1, objPaymentDetails.gcd_purpose_id1, objPaymentDetails.sub_purpose_desc1, objPaymentDetails.gcd_insurance_amount, objPaymentDetails.gcd_savings_amount); // -- dd check sub purpose desc
                }
              
                XMLInputFactory factory = XMLInputFactory.newInstance();
                //File fileLoc = new File(filePath);
                FileReader fileReader = new FileReader(resFile);
                //XMLStreamReader reader = factory.createXMLStreamReader(fileReader);
                //content = new StringBuilder();
                // Parsing XML using stream reader and writing to a ByteArrayOutputStream
                string AsBase64String = string.Empty;
                byte[] AsBytes = System.IO.File.ReadAllBytes(resFile);
                fileReader = new FileReader(resFile);
                XMLStreamReader reader = factory.createXMLStreamReader(fileReader);
                content = new StringBuilder();
                // Parsing XML using stream reader and writing to a ByteArrayOutputStream
                while (reader.hasNext())
                {
                    int eventType = reader.next();
                    switch (eventType)
                    {

                        case XMLEvent.START_ELEMENT:

                            currPath = currPath + "/" + reader.getLocalName();
                            //Instead
                            if (currPath.Contains("data"))
                            {
                                String startTag = "";
                                //Instead
                                if (reader.getLocalName().Equals("data"))
                                {
                                    fileWriter = new ByteArrayOutputStream();
                                    startTag = "<" + reader.getLocalName();
                                    for (int k = 0; k < reader.getNamespaceCount(); k++)
                                    {
                                        if (reader.getNamespacePrefix(k) != null)
                                            startTag = startTag + " xmlns:" + reader.getNamespacePrefix(k) + "=\"" + reader.getNamespaceURI(k) + "\"";
                                        else
                                            startTag = startTag + " xmlns=\"" + reader.getNamespaceURI(k) + "\"";
                                    }
                                    startTag = startTag + ">";
                                }
                                else
                                {
                                    startTag = "<" + reader.getLocalName() + ">";
                                }

                                if (fileWriter != null)
                                    fileWriter.write(Encoding.ASCII.GetBytes(startTag));
                            }
                            break;

                        case XMLStreamConstants.CHARACTERS:
                            if (fileWriter != null)
                            {
                                fileWriter.write(Encoding.ASCII.GetBytes(reader.getText()));
                            }
                            break;

                        case XMLStreamConstants.END_ELEMENT:
                            //Instead
                            if (currPath.Contains("data"))
                            {
                                string endTag = "</" + reader.getLocalName() + ">";

                                if (fileWriter != null)
                                {
                                    fileWriter.write(Encoding.ASCII.GetBytes(endTag));
                                }
                            }
                            content = new StringBuilder();
                            //RemoveLasttag(currPath);
                            currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
                            break;

                        case XMLEvent.CDATA:
                            break;
                        case XMLEvent.SPACE:
                            break;

                    }
                }
                com.sun.org.apache.xml.@internal.security.Init.init();
                Canonicalizer canon = Canonicalizer.getInstance(Canonicalizer.ALGO_ID_C14N_OMIT_COMMENTS);
                byte[] canonXmlBytes = canon.canonicalize(fileWriter.toByteArray());
                string beforesignedData = Convert.ToBase64String(canonXmlBytes);
                string beforecanonXmlData = Encoding.UTF8.GetString(AsBytes);
                string aftercanonXmlData = Encoding.UTF8.GetString(canonXmlBytes);

                //SIGN DATA WITH PFX FILE
                string xml_inBase64 = Convert.ToBase64String(AsBytes);
                string em = Encoding.UTF8.GetString(canonXmlBytes);

                Logger.LogMessage(TracingLevel.INFO, "10.10.31.231:8080/SignXmlData-Before convertion");
                _IGroupInsuranceBLL.SaveLogs("10.10.31.231:8080/SignXmlData-Before convertion", Convert.ToInt64(Session["UID"]));
                //WebAPI Service Call
                using (var client = new HttpClient())
                {
                    string vSignXmlDataURI = System.Configuration.ConfigurationManager.AppSettings["SignXmlDataURI"]; 

                    client.BaseAddress = new Uri(vSignXmlDataURI);

                    //client.BaseAddress = new Uri("http://10.10.31.231:8080/SignXmlData/");

                    object reqdata = new
                    {
                        data = beforesignedData
                    };
                    var myContent = JsonConvert.SerializeObject(reqdata);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    /////////
                    var resultsac = client.PostAsync("getSignforDataByte", byteContent).Result;
                    SignedresultContent = resultsac.Content.ReadAsStringAsync().Result;
                    client.CancelPendingRequests();
                    //Console.WriteLine("about to dispose the client");
                    client.Dispose();
                    //return resultContent;
                    Log.Debug(SignedresultContent);
                }
                Logger.LogMessage(TracingLevel.INFO, "10.10.31.231:8080/SignXmlData-After convertion");
                _IGroupInsuranceBLL.SaveLogs("10.10.31.231:8080/SignXmlData-After convertion", Convert.ToInt64(Session["UID"]));
                ////////
                try
                {
                    if (SignedresultContent != null)
                    {
                        string data = GetKIISignDetails(SignedresultContent, Encoding.UTF8.GetString(AsBytes));
                        Logger.LogMessage(TracingLevel.INFO, "GetKIISignDetails Response: " + data);
                        _IGroupInsuranceBLL.SaveLogs("GetKIISignDetails Response: " + data, Convert.ToInt64(Session["UID"]));
                        SignedDataResponseKII signedDataResponseK2 = new SignedDataResponseKII();
                        signedDataResponseK2 = JsonConvert.DeserializeObject<SignedDataResponseKII>(data);
                        Logger.LogMessage(TracingLevel.INFO, "GetKIISignDetails Response: " + "StatusCode:" + signedDataResponseK2.statusCode + "&" + "statusDescription:" + signedDataResponseK2.statusDescription);
                        _IGroupInsuranceBLL.SaveLogs("GetKIISignDetails Response: " + "StatusCode:" + signedDataResponseK2.statusCode + "&" + "statusDescription:" + signedDataResponseK2.statusDescription, Convert.ToInt64(Session["UID"]));
                        //Log.Debug(data);
                        if (signedDataResponseK2.statusCode == "KII-RCTER-00" && signedDataResponseK2.statusDescription == "Success")
                        {
                            //string transactiondate = DateTime.Now.ToString("ddMMyyyy");
                            string HashChechsumMD5 = "dept_ref_no=" + deptRefNum + "|txn_date=" + transactiondate + "|amount=" + amount + "|dept_pwd=1234";
                            string AfterHashChechsumMD5 = GetMD5Checksum(HashChechsumMD5);
                            string BeforeEncryptedStringData = "dept_ref_no=" + deptRefNum + "|txn_date=" + transactiondate + "|amount=" + amount + "|dept_pwd=1234|checkSum=" + AfterHashChechsumMD5 + "";
                            string EncryptedStringData = SymetricEncrypt(BeforeEncryptedStringData, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                            //Log.Debug("Encrypt Data After Signed Data Success");
                            //Log.Debug(EncryptedStringData);
                            Logger.LogMessage(TracingLevel.INFO, "Encrypt Data After Signed Data Success: " + EncryptedStringData);
                            string SymmetricDecryptData = SymmetricDecrypt(EncryptedStringData, "EdZUiBM0d8C46PEZ2Yn9Gg==");
                            //Log.Debug("Test Decrypt Data");
                            //Log.Debug(SymmetricDecryptData);
                            Logger.LogMessage(TracingLevel.INFO, "Test Decrypt Data: " + EncryptedStringData);
                            _IGroupInsuranceBLL.SaveLogs("Test Decrypt Data: " + EncryptedStringData, Convert.ToInt64(Session["UID"]));

                            string KIIurl = "https://preprodk2.karnataka.gov.in/wps/portal/Home/DepartmentPayment/?uri=receiptsample:com.tcs.departmentpage:departmentportlet";
                            string redirect_url = "" + KIIurl + "" + "&encdata=" + EncryptedStringData + "&dept_code=12C";
                            //Log.Debug("Redirect url here");
                            //Log.Debug(redirect_url);
                            Logger.LogMessage(TracingLevel.INFO, "Redirect url here: " + redirect_url);
                            _IGroupInsuranceBLL.SaveLogs("Redirect url here: " + redirect_url, Convert.ToInt64(Session["UID"]));
                            RemotePost myremotepost = new RemotePost();
                            myremotepost.Url = redirect_url;
                            // GIS -- 22 Dec
                            //myremotepost.Add("surl", "https://kgidonline.karnataka.gov.in/Home/Return");//Change the success url here depending upon the port number of your local system.  
                            //myremotepost.Add("furl", "https://kgidonline.karnataka.gov.in/Home/Return");//Change the failure url here depending upon the port number of your local system.  

                            myremotepost.Add("surl", "http://49.206.243.82:82/Home/Return");//Change the success url here depending upon the port number of your local system.  
                            myremotepost.Add("furl", "http://49.206.243.82:82/Home/Return");//Change the failure url here depending upon the port number of your local system.  

                            // GIS -- 22 Dec
                            myremotepost.Post();
                            //Log.Debug("Redirect Done");
                            Logger.LogMessage(TracingLevel.INFO, "Redirect Done");
                            _IGroupInsuranceBLL.SaveLogs("Redirect Done", Convert.ToInt64(Session["UID"]));
                            //string finalresponse = GrtUrl(KIIurl, EncryptedStringData, "12C");
                            //return redirect_url;
                        }
                        else
                        {
                            //Log.Error("redirect error");
                            Logger.LogMessage(TracingLevel.INFO, "redirect error");
                            _IGroupInsuranceBLL.SaveLogs("redirect error", Convert.ToInt64(Session["UID"]));
                            //return null;
                            //Unable to signed the data
                        }
                    }
                    else
                    {
                        Logger.LogMessage(TracingLevel.INFO, "signed content error");
                        _IGroupInsuranceBLL.SaveLogs("signed content error", Convert.ToInt64(Session["UID"]));
                        // Log.Error("signed content error");
                        //Signed Data Not Captured
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "Line catch" + ex.Message);
                    _IGroupInsuranceBLL.SaveLogs("Line catch" + ex.Message, Convert.ToInt64(Session["UID"]));
                    //Log.Error("Error Level", ex);
                }
                ///////KII Integration End//////
            }
            else
            {
                RemotePostFalse myremotepostfalse = new RemotePostFalse();
                myremotepostfalse.Url = "https://kgidonline.karnataka.gov.in/kgid-app?pay=false";
                myremotepostfalse.Post();
            }
        }
        public string TextFileCreate(long ChallanAmount, string Refno, string rmtrName, string prpsName, string ddoCode, int deptPrpsId, string subPrpsName)
        {
            Logger.LogMessage(TracingLevel.INFO, "TextFileCreate()");
            _IGroupInsuranceBLL.SaveLogs("TextFileCreate()", Convert.ToInt64(Session["UID"]));
            // KD0221801112345678
            //string dd = DateTime.Now.ToString("dd");
            //string MM = DateTime.Now.ToString("MM");
            //string yy = DateTime.Now.ToString("yy");
            //string ddHHmmss = DateTime.Now.ToString("ddHHmmss");
            //deptRefNum = "KD" + MM + yy + "8011" + ddHHmmss;
            //var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            //string newFile = Server.MapPath("~/Documents/KIIRequest/" + deptRefNum + ".txt");
            //string newFile = @"F:/Documents/KIIRequest/" + deptRefNum + ".txt";
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\KIIRequest\" + deptRefNum + ".txt";
            }
            //string fileName = @"D:\VenkatTXFITx.txt";
            System.IO.FileInfo fi = new System.IO.FileInfo(newFile);
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (fi.Exists)
                {
                    fi.Delete();
                }

                // Create a new file     
                using (System.IO.StreamWriter strmwrtr = fi.CreateText())
                {
                    //strmwrtr.WriteLine("New file created: {0}", DateTime.Now.ToString());
                    //strmwrtr.WriteLine("Author: Venkatesh);
                    strmwrtr.WriteLine("<data>");
                    strmwrtr.WriteLine("<RctReceiveValidateChlnRq>");
                    strmwrtr.WriteLine("      <chlnDate>" + chlntransactiondate + "</chlnDate>");
                    strmwrtr.WriteLine("      <deptCode>12C</deptCode>");
                    strmwrtr.WriteLine("      <ddoCode>" + ddoCode + "</ddoCode>");
                    strmwrtr.WriteLine("      <deptRefNum>" + deptRefNum + "</deptRefNum>");
                    strmwrtr.WriteLine("      <rctReceiveValidateChlnDtls>");
                    strmwrtr.WriteLine("        <amount>" + ChallanAmount + "</amount>");
                    strmwrtr.WriteLine("        <deptPrpsId>3</deptPrpsId>");
                    strmwrtr.WriteLine("        <prpsName>8011-00-107-0-01</prpsName>");//0230~00~104~0~00~000
                    strmwrtr.WriteLine("        <subPrpsName>19</subPrpsName>");//FACT REN 03   "+ subPrpsName + "
                    strmwrtr.WriteLine("        <subDeptRefNum>" + deptRefNum + "</subDeptRefNum>");
                    strmwrtr.WriteLine("      </rctReceiveValidateChlnDtls>");                   
                    strmwrtr.WriteLine("      <rmtrName>" + rmtrName + "</rmtrName>");
                    strmwrtr.WriteLine("      <totalAmount>" + ChallanAmount + "</totalAmount>");
                    strmwrtr.WriteLine("      <trsryCode>572A</trsryCode>");
                    strmwrtr.WriteLine("</RctReceiveValidateChlnRq>");
                    strmwrtr.WriteLine("</data>");
                }

                //Write file contents on console.
                //using (StreamReader sr = File.OpenText(fileName))
                //{
                //    string s = "";
                //    while ((s = sr.ReadLine()) != null)
                //    {
                //        //Console.WriteLine(s);
                //    }
                //}
                return newFile;
            }
            catch (Exception Ex)
            {
                _IGroupInsuranceBLL.SaveLogs("Line catch" + Ex.Message, Convert.ToInt64(Session["UID"]));
                return null;
               
            }
        }
        public string GetKIISignDetails(string signeddata, string xmldata)
        {
            try
            {
                Logger.LogMessage(TracingLevel.INFO, "GetKIISignDetails: SIGNED DATA-" + signeddata + ",xmldata-" + xmldata);
                _IGroupInsuranceBLL.SaveLogs("GetKIISignDetails: SIGNED DATA-" + signeddata + ",xmldata-" + xmldata, Convert.ToInt64(Session["UID"]));
                // string URL = "https://khajane2.karnataka.gov.in/KhajaneWs/rct/rrvcs/secbc/RctReceiveValidateChlnService?wsdl";
                //  string URL = "https://preprodkhajane2.karnataka.gov.in/KhajaneWs/rct/rrvcs/secbc/RctReceiveValidateChlnService?wsdl";

                string URL = System.Configuration.ConfigurationManager.AppSettings["preprodkhajane2test"];


                string xml2 = @"<?xml version='1.0' encoding='utf-8'?>"
                    +
                    "<soapenv:Envelope xmlns:soapenv=" + "'" + "http://schemas.xmlsoap.org/soap/envelope/" + "'" + " xmlns:ser=" + "'" + "http://service.receivevalidatechallan.dept.rct.integration.ifms.gov.in/" + "'" + " xmlns:head=" + "'" + "http://header.ei.integration.ifms.gov.in/" + "'" + ">"
                    + "\n"
                    + "   <soapenv:Header>"
                    + "\n"
                    + "      <ser:Header>"
                    + "\n"
                    + "         <head:agencyCode>EA_KID</head:agencyCode>"
                     + "\n"
                    + "         <head:integrationCode>RCT033</head:integrationCode>"
                     + "\n"
                    + "         <head:uirNo>EA_KID-RCT033-" + transactiondate + "-" + deptRefNum + "</head:uirNo>"
                     + "\n"
                    + "      </ser:Header>"
                     + "\n"
                    + "   </soapenv:Header>"
                     + "\n"
                    + "   <soapenv:Body>"
                     + "\n"
                    + "      <ser:envelopedDataReq>"
                     + "\n"
                    + "         <Signature>" + signeddata + "</Signature>"
                    + "\n"
                    + xmldata
                    + "      </ser:envelopedDataReq>"
                     + "\n"
                    + "   </soapenv:Body>"
                     + "\n"
                    + "</soapenv:Envelope>";

                /////////////

                //////////////////////



                string responseStr = string.Empty;
                string jsonText = string.Empty;
                //var _url = "https://preprodkhajane2.karnataka.gov.in/KhajaneWs/rct/rrvcs/secbc/";
                //var _action = "RctReceiveValidateChlnService?wsdl";

                //XmlDocument soapEnvelopeXml = CreateSoapEnvelope(signeddata, xml2);
                // WebRequest.DefaultCachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                //Log.Debug("Request for Signature Validation");

                Logger.LogMessage(TracingLevel.INFO, "Request for Signature Validation");
                _IGroupInsuranceBLL.SaveLogs("Request for Signature Validation", Convert.ToInt64(Session["UID"]));
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(URL);
                byte[] bytes;
                bytes = Encoding.UTF8.GetBytes(xml2);
                request.ContentType = "text/xml";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                //request.SendChunked = false;
                // request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0, no-cache, no-store");
                //request.KeepAlive = true;
                //request.AllowWriteStreamBuffering = false;
                //request.ServicePoint.ConnectionLimit = 10;    // The default value of 2 within a ConnectionGroup caused me always a "Timeout exception" because a user's 1-3 concurrent WebRequests within a second.
                //request.ServicePoint.MaxIdleTime = 5 * 1000;  // (5 sec) default was 100000 (100 sec).  Max idle time for a connection within a ConnectionGroup for reuse before closing

                //Log.Debug("FileStream Rquesting");
                Logger.LogMessage(TracingLevel.INFO, "FileStream Rquesting");
                _IGroupInsuranceBLL.SaveLogs("FileStream Rquesting", Convert.ToInt64(Session["UID"]));
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                //Log.Debug("FileStream Rquest Done");
                Logger.LogMessage(TracingLevel.INFO, "FileStream Rquest Done");
                _IGroupInsuranceBLL.SaveLogs("FileStream Rquesting", Convert.ToInt64(Session["UID"]));
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                //Log.Debug("Response from KII Signing:  " + response.StatusCode);
                Logger.LogMessage(TracingLevel.INFO, "Response from KII Signing: Status Code" + response.StatusCode);
                _IGroupInsuranceBLL.SaveLogs("Response from KII Signing: Status Code" + response.StatusCode, Convert.ToInt64(Session["UID"]));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    responseStr = new StreamReader(responseStream).ReadToEnd();
                    XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(responseStr));
                    Log.Debug("Response Data:  " + xmlDocumentWithoutNs);
                    Logger.LogMessage(TracingLevel.INFO, "Response from KII Signing Response Data: " + xmlDocumentWithoutNs);
                    _IGroupInsuranceBLL.SaveLogs("Response from KII Signing Response Data: " + xmlDocumentWithoutNs, Convert.ToInt64(Session["UID"]));
                    XDocument xDoc = XDocument.Load(new System.IO.StringReader(responseStr));

                    var unwrappedResponse = xDoc.Descendants((XNamespace)"http://schemas.xmlsoap.org/soap/envelope/" + "Body").First().FirstNode;
                    //var xmlwithoutheaders = unwrappedResponse.ToString();
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(unwrappedResponse.ToString());
                    jsonText = JsonConvert.SerializeXmlNode(doc1.ChildNodes[0].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                    ////
                    //var xmlWithoutNs = xmlDocumentWithoutNs.ToString();
                    //Log.Debug("Response Data:  " + xmlWithoutNs);
                    //return Content(responseStr);
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(xmlWithoutNs);
                    //XmlNode node = doc.SelectSingleNode("Basic_vehicle_detailsResult");
                    //string jsonText = JsonConvert.SerializeXmlNode(doc);

                    // To convert JSON text contained in string json into an XML node
                    //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
                    ViewBag.Response = responseStr;
                    // ViewBag.Response1 = doc.InnerText;
                    //jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                    //jsonText = JsonConvert.SerializeXmlNode(doc);
                    //Log.Debug("After Json Convervation Data: " + jsonText);

                    //DEVIKA
                    //Stream responseStream = response.GetResponseStream();
                    //responseStr = new StreamReader(responseStream).ReadToEnd();
                    //XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(responseStr));
                    //var xmlWithoutNs = xmlDocumentWithoutNs.ToString();
                    //Log.Debug("Response Data:  " + xmlWithoutNs);
                    //Logger.LogMessage(TracingLevel.INFO, "Response from KII Signing Response Data: " + xmlWithoutNs);
                    //_IGroupInsuranceBLL.SaveLogs("Response from KII Signing Response Data: " + xmlWithoutNs, Convert.ToInt64(Session["UID"]));
                    ////return Content(responseStr);
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(xmlWithoutNs);
                    ////XmlNode node = doc.SelectSingleNode("Basic_vehicle_detailsResult");
                    ////string jsonText = JsonConvert.SerializeXmlNode(doc);

                    //// To convert JSON text contained in string json into an XML node
                    ////XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
                    //ViewBag.Response = responseStr;
                    //ViewBag.Response1 = doc.InnerText;
                    //jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                    ////jsonText = JsonConvert.SerializeXmlNode(doc);
                    //Log.Debug("After Json Convervation Data: " + jsonText);
                    //_IGroupInsuranceBLL.SaveLogs("After Json Convervation Data: " + jsonText, Convert.ToInt64(Session["UID"]));

                    //DEVIKA
                }

                //VM_MotorInsuranceVehicleDetails obj = new VM_MotorInsuranceVehicleDetails();
                //dynamic result = JsonConvert.DeserializeObject(ViewBag.Response);

                //obj = _IMotorInsuranceVehicleDetailsBll.BindVahanResponseDetailstoModel(result);

                return jsonText;
            }
            catch (Exception ex)
            {
                //Log.Error("Signing Data Error Level", ex);
                Logger.LogMessage(TracingLevel.INFO, "Signing Data Error Level" + ex.Message);
                _IGroupInsuranceBLL.SaveLogs("Signing Data Error Level" + ex.Message, Convert.ToInt64(Session["UID"]));
                return null;
            }
        }
        public class SignedDataResponseKII
        {
            public string deptRefNum { get; set; }
            public string totalAmount { get; set; }
            public string statusCode { get; set; }
            public string statusDescription { get; set; }
        }
        //Implemented based on requirement--Added by Venkatesh--
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
        //Remote post--Added by Venkatesh
        public class RemotePost
        {
            //Remote post added by--Venkatesh
            private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();


            public string Url = "";
            public string Method = "post";
            public string FormName = "form1";

            public void Add(string name, string value)
            {
                Inputs.Add(name, value);
            }

            public void Post()
            {
                System.Web.HttpContext.Current.Response.Clear();

                System.Web.HttpContext.Current.Response.Write("<html><head>");

                System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
                System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                System.Web.HttpContext.Current.Response.Write("</form>");
                System.Web.HttpContext.Current.Response.Write("</body></html>");

                System.Web.HttpContext.Current.Response.End();
            }
        }
        public class RemotePostFalse
        {
            //Remote post added by--Venkatesh
            private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();


            public string Url = "";
            public string Method = "post";
            public string FormName = "form1";

            public void Add(string name, string value)
            {
                Inputs.Add(name, value);
            }

            public void Post()
            {
                System.Web.HttpContext.Current.Response.Clear();

                System.Web.HttpContext.Current.Response.Write("<html><head>");

                System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
                System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                System.Web.HttpContext.Current.Response.Write("</form>");
                System.Web.HttpContext.Current.Response.Write("</body></html>");

                System.Web.HttpContext.Current.Response.End();
            }
        }

        public static string GetMD5Checksum(string filename)
        {
            byte[] b = CreateChecksum(filename);
            string result = "";
            for (int i = 0; i < b.Length; i++)
            {
                result += java.lang.Integer.toString((b[i] & 0xff) + 0x100, 16).Substring(1);
            }
            return result;
        }
        public static byte[] CreateChecksum(string filename)
        {
            InputStream fis = new ByteArrayInputStream(System.Text.Encoding.UTF8.GetBytes(filename));
            byte[] buffer = new byte[1024];
            MessageDigest complete = MessageDigest.getInstance("MD5");
            int numRead;
            do
            {
                numRead = fis.read(buffer);
                if (numRead > 0)
                {
                    complete.update(buffer, 0, numRead);
                }
            }
            while (numRead != -1);
            fis.close();
            return complete.digest();
        }

        public static string SymetricEncrypt(string text, string secretkey)
        {
            Log.Debug("Request SymetricEncryptData: " + text);
            byte[] raw;
            string encryptedString;
            SecretKeySpec skeyspec;

            Cipher cipher;
            try
            {
                byte[] encryptText = Encoding.UTF8.GetBytes(text);
                //byte[] encryptText = text.getBytes("UTF-8");
                //FileInputStream fileinputstream = new FileInputStream("D://KII//KGID_KHAJANE.key");
                //byte[] abyte = new byte[fileinputstream.available()];
                byte[] abyte = Encoding.UTF8.GetBytes(secretkey);
                //fileinputstream.read(abyte);
                //fileinputstream.close();

                byte[] keyBytes = new byte[16];
                int len = abyte.Length;
                if (len > keyBytes.Length) len = keyBytes.Length;
                //System.arraycopy(abyte, 0, keyBytes, 0, len);
                Array.Copy(abyte, 0, keyBytes, 0, len);
                raw = Base64.decode(secretkey);
                skeyspec = new SecretKeySpec(keyBytes, "AES");
                IvParameterSpec ivSpec = new IvParameterSpec(keyBytes);
                cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
                cipher.init(Cipher.ENCRYPT_MODE, skeyspec, ivSpec);
                //byte[] results = cipher.doFinal(encryptText);
                //string beforesignedData = Convert.ToBase64String(results);
                encryptedString = Base64.encode(cipher.doFinal(encryptText));
                //encryptedString = Convert.ToBase64String(cipher.doFinal(encryptText));
                return encryptedString;
            }
            catch (Exception ex)
            {
                Log.Error("SymetricEncrypt Error Level", ex);
                return null;
            }
        }
        public static string SymmetricDecrypt(string text, string secretkey)
        {
            Cipher cipher;
            string encryptedString;
            byte[] encryptText = null;
            byte[] raw;
            SecretKeySpec skeySpec;
            try
            {
                //FileInputStream fileinputstream = new FileInputStream("D://KII//KGID_KHAJANE.key");
                //byte[] abyte = new byte[fileinputstream.available()];
                byte[] abyte = Encoding.UTF8.GetBytes(secretkey);
                //fileinputstream.read(abyte);
                //fileinputstream.close();
                byte[] keyBytes = new byte[16];
                int len = abyte.Length;
                if (len > keyBytes.Length) len = keyBytes.Length;
                //System.arraycopy(abyte, 0, keyBytes, 0, len);
                Array.Copy(abyte, 0, keyBytes, 0, len);
                //raw = Base64.decode(secretkey);
                skeySpec = new SecretKeySpec(keyBytes, "AES");
                //encryptText = System.Text.Encoding.UTF8.GetBytes(text);
                //encryptText = Base64.decode(text);
                encryptText = Convert.FromBase64String(text);

                IvParameterSpec ivSpec = new IvParameterSpec(keyBytes);
                cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
                cipher.init(Cipher.DECRYPT_MODE, skeySpec, ivSpec);
                //encryptedString = new String(cipher.doFinal(encryptText));
                //encryptedString = Convert.ToBase64String(cipher.doFinal(encryptText));
                encryptedString = Encoding.UTF8.GetString(cipher.doFinal(encryptText));
                return encryptedString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //[HttpGet]
        public ActionResult Return(string encdata, string dept_code, string urlId, string connect)
        {
            Log.Debug("After Payment Data: " + encdata);
            //ViewBag.Message = form.Keys;
            //ViewBag.Test = dept_code;
            //UrlDecode
            //string dec = HttpUtility.UrlDecode(encdata);
            string dec = encdata;
            Log.Debug("URL Decode Data: " + dec);
            dec.Replace(" ", "");
            string resdecData = SymmetricDecrypt(dec, "EdZUiBM0d8C46PEZ2Yn9Gg==");
            string[] resdecDataList = resdecData.Split(new Char[] { '|', '=' });
            string BankTransactionNo = resdecDataList[1];
            string ChallanAmount = resdecDataList[3];
            string ChallanRefNo = resdecDataList[5];
            string Status = resdecDataList[7];
            string BankName = resdecDataList[9];
            string PaymentMode = resdecDataList[11];
            string TransactionTimeStamp = resdecDataList[13];
            string CheckSum = resdecDataList[15];

            KIIPaymentResponse PaymentResponseData = new KIIPaymentResponse();
            PaymentResponseData.BankTransactionNo = BankTransactionNo;
            PaymentResponseData.ChallanAmount = ChallanAmount;
            PaymentResponseData.ChallanRefNo = ChallanRefNo;
            PaymentResponseData.Status = Status;
            PaymentResponseData.BankName = BankName;
            PaymentResponseData.PaymentMode = PaymentMode;
            PaymentResponseData.TransactionTimeStamp = TransactionTimeStamp;
            PaymentResponseData.CheckSum = CheckSum;

            Log.Debug("After Payment Redirected Decrypt Data: " + resdecData);
            ViewBag.Message = encdata;
            ViewBag.Test = resdecData;
            //ViewBag.Test = "test";
            return View(PaymentResponseData);
        }

        public string TextFileCreate1(long ChallanAmount, string Refno, string rmtrName, string prpsName, string ddoCode, int deptPrpsId, string subPrpsName, string prpsName1,  int deptPrpsId1, string subPrpsName1,int insuranceamt, int savingsamt)
        {
            Logger.LogMessage(TracingLevel.INFO, "TextFileCreate() -- multiple HOA");
            _IGroupInsuranceBLL.SaveLogs("TextFileCreate() -- multiple HOA", Convert.ToInt64(Session["UID"]));
            // KD0221801112345678
            //string dd = DateTime.Now.ToString("dd");
            //string MM = DateTime.Now.ToString("MM");
            //string yy = DateTime.Now.ToString("yy");
            //string ddHHmmss = DateTime.Now.ToString("ddHHmmss");
            //deptRefNum = "KD" + MM + yy + "8011" + ddHHmmss;
            //var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            //string newFile = Server.MapPath("~/Documents/KIIRequest/" + deptRefNum + ".txt");
            //string newFile = @"F:/Documents/KIIRequest/" + deptRefNum + ".txt";
            string newFile = string.Empty;
            if (WebConfigurationManager.AppSettings["RootDirectory"] != null)
            {
                newFile = WebConfigurationManager.AppSettings["RootDirectory"] + @"GISEmployeeDocuments\KIIRequest\" + deptRefNum + ".txt";
            }
            //string fileName = @"D:\VenkatTXFITx.txt";
            System.IO.FileInfo fi = new System.IO.FileInfo(newFile);
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (fi.Exists)
                {
                    fi.Delete();
                }

                // Create a new file     
                using (System.IO.StreamWriter strmwrtr = fi.CreateText())
                {
                    //strmwrtr.WriteLine("New file created: {0}", DateTime.Now.ToString());
                    //strmwrtr.WriteLine("Author: Venkatesh);
                    strmwrtr.WriteLine("<data>");
                    strmwrtr.WriteLine("<RctReceiveValidateChlnRq>");
                    strmwrtr.WriteLine("      <chlnDate>" + chlntransactiondate + "</chlnDate>");
                    strmwrtr.WriteLine("      <deptCode>12C</deptCode>");
                    strmwrtr.WriteLine("      <ddoCode>" + ddoCode + "</ddoCode>");
                    strmwrtr.WriteLine("      <deptRefNum>" + deptRefNum + "</deptRefNum>");
                    strmwrtr.WriteLine("      <rctReceiveValidateChlnDtls>");
                    strmwrtr.WriteLine("        <amount>" + insuranceamt + "</amount>");
                    strmwrtr.WriteLine("        <deptPrpsId>3</deptPrpsId>");
                    strmwrtr.WriteLine("        <prpsName>8011-00-107-0-01</prpsName>");//0230~00~104~0~00~000
                    strmwrtr.WriteLine("        <subPrpsName>19</subPrpsName>");//FACT REN 03   "+ subPrpsName + "
                    strmwrtr.WriteLine("        <subDeptRefNum>" + deptRefNum + "AA" + "</subDeptRefNum>");
                    strmwrtr.WriteLine("      </rctReceiveValidateChlnDtls>");
                    strmwrtr.WriteLine("      <rctReceiveValidateChlnDtls>");
                    strmwrtr.WriteLine("        <amount>" + savingsamt + "</amount>");
                    strmwrtr.WriteLine("        <deptPrpsId>" + deptPrpsId + "</deptPrpsId>");
                    strmwrtr.WriteLine("        <prpsName>8011-00-107-0-02</prpsName>");//0230~00~104~0~00~000
                    strmwrtr.WriteLine("        <subPrpsName>22</subPrpsName>");//FACT REN 03   "+ subPrpsName + "
                    strmwrtr.WriteLine("        <subDeptRefNum>" + deptRefNum +"BB"+ "</subDeptRefNum>");
                    strmwrtr.WriteLine("      </rctReceiveValidateChlnDtls>");
                    strmwrtr.WriteLine("      <rmtrName>" + rmtrName + "</rmtrName>");
                    strmwrtr.WriteLine("      <totalAmount>" + ChallanAmount + "</totalAmount>");
                    strmwrtr.WriteLine("      <trsryCode>572A</trsryCode>");
                    strmwrtr.WriteLine("</RctReceiveValidateChlnRq>");
                    strmwrtr.WriteLine("</data>");
                }

                //Write file contents on console.
                //using (StreamReader sr = File.OpenText(fileName))
                //{
                //    string s = "";
                //    while ((s = sr.ReadLine()) != null)
                //    {
                //        //Console.WriteLine(s);
                //    }
                //}
                return newFile;
            }
            catch (Exception Ex)
            {
                _IGroupInsuranceBLL.SaveLogs("Line catch" + Ex.Message, Convert.ToInt64(Session["UID"]));
                return null;
            }
        }

        //[HttpPost]
        //public void Return(string encdata,string dept_code,string urlId,string connect)
        //{
        //    ViewBag.Message = encdata;
        //    //ViewBag.Message = form.Keys;
        //    ViewBag.Test = dept_code;
        //}
        #endregion
    }
}