using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KGID_Models.KGID_GroupInsurance;
using BLL.UploadEmployeeBLL;
using BLL.KGIDGroupInsuranceBLL;
using Newtonsoft.Json;
using System.Xml;
using System.Web.Script.Serialization;
using System.IO;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using Common;
using KGID_Models.NBApplication;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace KGID.Controllers
{
    public class GroupInsuranceClaimController : Controller
    {


        public GroupInsuranceClaimController()
        {
            this._uploadbll = new UploadEmployeeBLL();
            this._IGroupInsuranceBLL = new GroupInsuranceBLL();
           
        }
        private readonly IGroupInsuranceBLL _IGroupInsuranceBLL;
        private readonly IUploadEmployeeBLL _uploadbll;
        // GET: GroupInsuranceClaim
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult  ClaimsSearchEmployee()
        {
            return View();

        }
        public ActionResult SearchEmployeeDetailsForClaims()
        {
            return View();

        }
        public ActionResult SearchEmployeeDetailsForClaimsss(long kgid = 0)
        {
            bool isSuccess = false;
            string message = string.Empty;
          //  long kgid = Convert.ToInt64(Session["RID"]);
            long employeeid = _IGroupInsuranceBLL.GetEmployeeDetailsByKgid(kgid);
            Session["CUID"] = (employeeid != 0) ? employeeid : 0;
            return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ClaimsSearchEmployeeByKGID(long empId = 0)
        {
            bool isSuccess = false;
            string message = string.Empty;
            Session["RID"] = (empId != 0) ? empId : 0;
            VM_ClaimLedger model = new VM_ClaimLedger();
            long kgid = Convert.ToInt64(Session["RID"]);
            model.ledgerList = _IGroupInsuranceBLL.GetEmployeeLedgerDeatils(kgid);
            if (model.ledgerList != null) { message = (model.ledgerList.Count).ToString(); }
            else { message = "0"; }

            //  return Viewmodel.ledgerList.Count("ClaimsEditLedgerDetails");
            //if (model.ledgerList.Count > 0)
            //{ return View("ClaimsEditLedgerDetails", model); }
            //else
            //{ return View("ClaimsAddLedgerDetails", model); }

              return Json(new { IsSuccess = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);

        }

        [Route("gis-Claim-app")]
        public ActionResult GroupInsuranceClaimApplication()
        {
            //Session["RID"] = (refNo != 0) ? refNo : 0;
            //   GIS_CliamDetails obj = new GIS_CliamDetails();
            //obj.Type = Convert.ToString((PageType == "empty") ? "" : PageType);
            //obj.Type = "New";
            //obj.SentBackAppliaction = 0;

            //if (status == 2)
            //{
            //    obj.SentBackAppliaction = 1;
            //}
            //else if (status == 94)
            //{
            //    obj.SentBackAppliaction = 94;
            //}
            //else
            //{
            //    obj.SentBackAppliaction = 0;
            //}
            //Session["RID"] = (empId != 0) ? empId : 0;
            return View();
        }


         [Route("gis-Claimview-app")]
        public async Task<ActionResult> ClaimsAddLedgerDetails()
        {
            VM_ClaimLedger model = new VM_ClaimLedger();
            long kgid = Convert.ToInt64(Session["RID"]);
            model.monthlist =  _IGroupInsuranceBLL.GetMonth_Masters();
            model.yearlist =  _IGroupInsuranceBLL.GetYear_Masters();
            model.grouplist =  _IGroupInsuranceBLL.GetGroup_Masters();
            model.employee_id = _IGroupInsuranceBLL.GetEmployeeDetailsByKgid(kgid);
            model.newledger = new VM_ClaimLedger();
            int ledgerId = 0;
            model.newledger.LedgerId = ledgerId == 0 ? default(int) : (int)ledgerId;
            return View(model);
        }


        [Route("gis-ClaimEditview-app")]
         public async  Task<ActionResult> ClaimsEditLedgerDetails()
        {
            VM_ClaimLedger model = new VM_ClaimLedger();         
            long kgid = Convert.ToInt64(Session["RID"]);
            model.employee_id = _IGroupInsuranceBLL.GetEmployeeDetailsByKgid(kgid);
            model.monthlist = _IGroupInsuranceBLL.GetMonth_Masters();
            model.yearlist =  _IGroupInsuranceBLL.GetYear_Masters();
            model.grouplist =  _IGroupInsuranceBLL.GetGroup_Masters();
            model.ledgerList = _IGroupInsuranceBLL.GetEmployeeLedgerDeatils(kgid);
            return View(model);
        }


        public ActionResult EmployeeDetailsToView()
        {
            GIS_CliamDetails employeeDetails = null;
            return this.PartialView("_EmployeeDetailsView", employeeDetails);
        }

        


        //add Ledger Details
        [HttpPost]
         public ActionResult AddLedgerDetails(IList<VM_ClaimLedger> LedgerDeatils)
        {
            //foreach (var i  in LedgerDeatils)
            //{
            //    LedgerDeatils.em
            //}
            return Json(_IGroupInsuranceBLL.AddLedgerDetails(LedgerDeatils) ? "Success" : "Failure");
            // GIS_CliamDetails employeeDetails = _uploadbll.GetEMPDetailsById(0);
            // return this.PartialView("_EmployeeDetailsView", employeeDetails);
        }

        [HttpPost]
        public ActionResult UpdateLedgerDetails(IList<VM_ClaimLedger> LedgerDeatils)
        {

            return Json(_IGroupInsuranceBLL.UpdateLedgerDetails(LedgerDeatils) ? "Success" : "Failure");
        }

     
        public async Task<ActionResult> ClaimEmployeeDetailsToView()
        
        {
            //GIS_CliamDetails employeeDetails = _uploadbll.GetEMPDetailsById(0);
            //return this.PartialView("ClaimEmployeeDetailsToView", employeeDetails);


            VM_BasicDetails model = new VM_BasicDetails();
            long empId = Convert.ToInt64(Session["CUID"]);
            model = _IGroupInsuranceBLL.GISGetEmployeeDetails(empId);
            if (model != null)
            { 
            DateTime f = (DateTime)model.dateofbirth;
            DateTime f1 = (DateTime)model.dateofappointment;
            DateTime f2 = (DateTime)model.ewddateofjoining;
            // model.ead_pincode = int.Parse(model.ead_pincode1);
            model.date_of_birth = f.ToShortDateString();
            model.ewd_date_of_joining = f2.ToShortDateString();
            model.date_of_appointment = f1.ToShortDateString();
            }
            return this.PartialView("ClaimEmployeeDetailsToView", model);


        }

        public ActionResult ClaimNomineeDetails()
        {
            //long empId = Convert.ToInt64(Session["CUID"]);
            //VM_NomineeDetails _NomineeData = new VM_NomineeDetails();
            //if (empId != 0) 
            //{ 
            //_NomineeData.NomineeDetails = _IGroupInsuranceBLL.GISNomineeDetailsDll(empId);
            //_NomineeData.EmployeeId = Convert.ToInt64(Session["CUID"]); ;
            //_NomineeData.ddlNomineeList = _IGroupInsuranceBLL.GetNomineelist(empId);
            //}
            //return this.PartialView("ClaimNomineeDetails", _NomineeData);


            VM_BasicDetails model = new VM_BasicDetails();
            long empId = Convert.ToInt64(Session["CUID"]);
            model = _IGroupInsuranceBLL.GISGetEmployeeDetails(empId);  
            VM_NomineeDetails _NomineeData = new VM_NomineeDetails();
            _NomineeData.NomineeDetails = _IGroupInsuranceBLL.GISNomineeDetailsDll(empId);
            _NomineeData.EmployeeId = Convert.ToInt64(Session["UID"]); ;
            _NomineeData.ddlNomineeList = _IGroupInsuranceBLL.GetNomineelist(empId);

            return this.PartialView("ClaimNomineeDetails", _NomineeData);

        }
        public ActionResult EmployeeDetailsById(long ID)
        {
            try
            {
                GIS_CliamDetails obj = null; //_uploadbll.GetEMPDetailsById(ID);

                return Json(new
                {
                    IsSuccess = true,
                    VahanDetails = obj,

                }, JsonRequestBehavior.AllowGet);

                //var obj1 = new { name = obj.employee_name };

                //var genericResult = new { IsSuccess = true, EmpDetails = obj1 };
                //return Json(genericResult);


            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, responseText = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult NomineeDetailsToView(GIS_CliamDetails employeeDetails)
        {
            return this.PartialView("_NomineeDetailsView", employeeDetails);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [Route("InsertGISAppnReferenceno")]
        public JsonResult InsertGISAppnReferenceno(GIS_CliamDetails objGISPDetails)
        {
            bool isSuccess = false;
            string message = string.Empty;
            long RefNo = 0;
            string QRCode = string.Empty;
            long result = 0;
            objGISPDetails.gis_employee_id = Convert.ToInt64(Session["UID"]);
            objGISPDetails.gis_kgid_application_number = (Convert.ToString(Session["RID"]) == "0" ? "" : Convert.ToString(Session["RID"]));
            objGISPDetails.gis_category = Convert.ToString(Session["Categories"]);
            objGISPDetails.gis_pagetype = "New";

            if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
            {
                objGISPDetails.gis_category = Convert.ToString(Session["SelectedCategory"]);
                result = _IGroupInsuranceBLL.SaveGISProposalAppnRefNo(objGISPDetails);
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                result = _IGroupInsuranceBLL.SaveGISProposalAppnRefNo(objGISPDetails);
            }
            else
            {
                objGISPDetails.gis_category = Convert.ToString(Session["SelectedCategory"]);
                result = _IGroupInsuranceBLL.SaveGISProposalAppnRefNo(objGISPDetails);
            }

            if (result > 2)
            {
                RefNo = result;
                Session["RID"] = result;
                isSuccess = true;
                message = "Proposer details saved successfully";
                //QR Code 
                using (MemoryStream ms = new MemoryStream())
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(result.ToString(), QRCodeGenerator.ECCLevel.Q);
                    using (Bitmap bitMap = qrCode.GetGraphic(20))
                    {
                        bitMap.Save(ms, ImageFormat.Png);
                        QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            else if (result != 0)
            {
                RefNo = Convert.ToInt64(Session["RID"]);
                isSuccess = true;
                message = "Proposer details saved successfully";
            }

            return Json(new { IsSuccess = isSuccess, Message = message, ReferenceNo = RefNo, QRCodeImage = QRCode }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProposerDetailsToView(string PageType, string refNo = "")
        {
            GIS_CliamDetails _ProposerDetail = new GIS_CliamDetails();
            try
            {
                if (Convert.ToInt32(Session["SelectedCategory"]) == 2)
                {
                    _ProposerDetail = _IGroupInsuranceBLL.GISProposerDetailsBll(Convert.ToInt64(Session["UID"]), (PageType == null) ? "" : PageType, (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["SelectedCategory"]));
                }

                else if (Convert.ToInt32(Session["SelectedCategory"]) == 1)
                {
                    _ProposerDetail = _IGroupInsuranceBLL.GISProposerDetailsBll(Convert.ToInt64(Session["UID"]), "Emp", (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["SelectedCategory"]));
                }

            }
            catch (Exception ex)
            {

            }

            if (!String.IsNullOrEmpty(_ProposerDetail.gis_kgid_application_number))
            {
                Session["RID"] = _ProposerDetail.gis_kgid_application_number;
                TempData["Deptmartment"] = _ProposerDetail.gis_Department;
                using (MemoryStream ms = new MemoryStream())
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(_ProposerDetail.gis_kgid_application_number.ToString(), QRCodeGenerator.ECCLevel.Q);
                    using (Bitmap bitMap = qrCode.GetGraphic(20))
                    {
                        bitMap.Save(ms, ImageFormat.Png);
                        _ProposerDetail.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return this.PartialView("_GISProposerDtails", _ProposerDetail);
        }

        public ActionResult EmployeeGISLedger(string empsearch)
        {
            try
            {
                long empId = Convert.ToInt64(Session["CUID"]);            
                 GIS_Ledger obj = _IGroupInsuranceBLL.GetEMPGISLedgerBll(empId.ToString());               
                return this.PartialView(obj);

            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, responseText = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        //SAVE NOMINEE details , NOMINEE BANK details

        public Task<long> GISInsertNomineeDetails(VM_NomineeDetail objNomineeDetails)
        {
            string path = UploadDocumentNominee(objNomineeDetails.SonDaughterAdoption_doc, (long)objNomineeDetails.EmpId, "AdoptionDocument");
            objNomineeDetails.SonDaughterAdoption_doc_path = path;
            var result = _IGroupInsuranceBLL.GISSaveNomineeBankDetails(objNomineeDetails);
            //var result1 = _IGroupInsuranceBLL.GISSaveNomineeBankDetails
            return result;
        }

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
                        subPath = WebConfigurationManager.AppSettings["RootDirectory"] + @"Adoption\";
                    }
                }
                string FileNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                path = subPath + empId.ToString() + FileNo + fileName;
                document.SaveAs(path);
            }
            return path;
        }
    }
}