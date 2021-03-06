using BLL.DDOMasterBLL;
using BLL.DeptMasterBLL;
using BLL.InsuredEmployeeBll;
using BLL.KGIDMotorInsurance;
using BLL.NewEmployeeBLL;
using Common;
using KGID.Models;
using KGID_Models.KGID_Policy;
using KGID_Models.KGIDEmployee;
using KGID_Models.KGIDMotorInsurance;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static KGID.FilterConfig;
using System.Threading;

namespace KGID.Controllers
{
    [RoutePrefix("ViewDataToVerify")]
    [NoCache]
    [SessionAuthorize]
    public class ViewDataToVerifyController : Controller
    {
        private readonly INewEmployeeDetailsBLL _newemp;
        private readonly IDDOMasterBLL _ddomaster;
        private readonly IDeptMasterBLL _deptmaster;

        private readonly IInsuredEmployeeBll _InsuredEmployeebll;
        private readonly INBApplicationBll _INBApplicationbll;

        private readonly IMotorInsuranceVehicleDetailsBll _IMotorInsuranceVehicleDetailsBll;
        private readonly IMotorInsuranceProposerDetailsBll _IMotorInsuranceProposerDetailsBll;
        private readonly IMotorInsuranceRenewalDetailsBll _IMotorInsuranceRenewalDetailsBll;

        public ViewDataToVerifyController()
        {
            this._newemp = new NewEmployeeDetailsBLL();
            this._ddomaster = new DDOMasterBLL();
            this._deptmaster = new DeptMasterBLL();

            this._InsuredEmployeebll = new InsuredEmployeeBll();
            this._INBApplicationbll = new NBApplicationBll();
            this._IMotorInsuranceVehicleDetailsBll = new MotorInsuranceVehicleDetailsBll();
            this._IMotorInsuranceProposerDetailsBll = new MotorInsuranceProposerDetailsBll();
            this._IMotorInsuranceRenewalDetailsBll = new MotorInsuranceRenewalDetailsBll();
        }
        // GET: ViewDataToVerify
        [Route("VerifyEmployeeDetails")]
        public ActionResult Index(long EmpID)
        {
            if (EmpID != 0)
            {
                Session["RUID"] = EmpID;
            }
            return View();
        }
        [NoCache]
        [SessionAuthorize]
        public ActionResult BasicDetailsToView()
        {
            VM_BasicDetails _BasicData = _INBApplicationbll.BasicDetailsBll(Convert.ToInt64(Session["RUID"]));
            if (_BasicData == null)
            {
                _BasicData = new VM_BasicDetails();
            }
            else
            {
                if (_BasicData.kad_kgid_application_number != null && _BasicData.kad_kgid_application_number != "")
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
            return this.PartialView("_BasicDetails", _BasicData);
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
        public ActionResult NomineeDetailsToView()
        {
            VM_NomineeDetails _NomineeData = _INBApplicationbll.NomineeDetailsBll(Convert.ToInt64(Session["RUID"]));
            if (_NomineeData == null)
            {
                _NomineeData = new VM_NomineeDetails();
            }

            return this.PartialView("_NomineeDetails", _NomineeData);
        }
        public ActionResult FamilyDetailsToView()
        {
            VM_FamilyDetails _FamilyData = _INBApplicationbll.FamilyDetailsBll(Convert.ToInt64(Session["RUID"]));
            if (_FamilyData == null)
            {
                _FamilyData = new VM_FamilyDetails();
            }

            return this.PartialView("_FamilyDetails", _FamilyData);
        }
        public ActionResult PersonalDetailsToView()
        {
            long EmpID = 0;
            if (Session["RUID"] == null)
            {
                EmpID = Convert.ToInt64(Session["UID"]);
            }
            else
            {
                EmpID = Convert.ToInt64(Session["RUID"]);
            }
            VM_PersonalHealthDetails _PersonalData = _INBApplicationbll.PersonalDetailsBll(EmpID);
            if (_PersonalData == null)
            {
                _PersonalData = new VM_PersonalHealthDetails();
            }

            return this.PartialView("_PersonalDetails", _PersonalData);
        }
        public ActionResult MedicalLeaveDetailsToView()
        {
            VM_MedicalLeaveDetails _MedicalLeaveData = _INBApplicationbll.MedicalLeaveDetailsBll(Convert.ToInt64(Session["RUID"]),"");
            if (_MedicalLeaveData == null)
            {
                _MedicalLeaveData = new VM_MedicalLeaveDetails();
            }

            return PartialView("_MedicalLeaveDetails", _MedicalLeaveData);
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
        public ActionResult HBasicDetailsToView()
        {
            VM_BasicDetails _BasicData = _INBApplicationbll.BasicDetailsBll(Convert.ToInt64(Session["RUID"]));
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
            return this.PartialView("_HBasicDetails", _BasicData);
        }
        public ActionResult HPhysicalDetailsToView()
        {
            VM_MPhysicalDetails _HPhysicalDetailsData = _INBApplicationbll.HPhysicalDetailsBll(Convert.ToInt64(Session["RUID"]));
            if (_HPhysicalDetailsData == null)
            {
                _HPhysicalDetailsData = new VM_MPhysicalDetails();
            }

            return this.PartialView("_HPhysicalDetails", _HPhysicalDetailsData);
        }
        public ActionResult HOtherDetailsToView()
        {
            VM_MOtherDetails _HOtherDetailsData = _INBApplicationbll.HOtherDetailsBll(Convert.ToInt64(Session["RUID"]));
            _HOtherDetailsData.employee_id = Convert.ToInt64(Session["RUID"]);
            if (_HOtherDetailsData == null)
            {
                _HOtherDetailsData = new VM_MOtherDetails();
            }

            return this.PartialView("_HOtherDetails", _HOtherDetailsData);
        }
        public ActionResult HHealthDetailsToView()
        {
            VM_MOtherDetails _HHealthDetailsData = _INBApplicationbll.HHealthDetailsBll(Convert.ToInt64(Session["RUID"]));
            _HHealthDetailsData.employee_id = Convert.ToInt64(Session["RUID"]);
            if (_HHealthDetailsData == null)
            {
                _HHealthDetailsData = new VM_MOtherDetails();
            }

            return this.PartialView("_HHealthDetails", _HHealthDetailsData);
        }
        public ActionResult HDoctorDetailsToView()
        {
            VM_DoctorDetails _HDoctorDetailsData = new VM_DoctorDetails();
            _HDoctorDetailsData = _INBApplicationbll.GetDoctorDetails(Convert.ToInt64(Session["RUID"]));

            return this.PartialView("_HDoctorDetails", _HDoctorDetailsData);
        }
        public ActionResult HDeclarationDetailsToView()
        {
            tbl_medical_declaration _HDeclarationData = _INBApplicationbll.HDeclarationDetailsBll(Convert.ToInt64(Session["RUID"]));
            if (_HDeclarationData == null)
            {
                _HDeclarationData = new tbl_medical_declaration();
            }

            return this.PartialView("_HDeclarationDetails", _HDeclarationData);
        }
        public ActionResult PaymentDetailsToView(int EmpId,int AppId)
        {
            VM_ChallanPrintDetails NBChallanDetails = new VM_ChallanPrintDetails();
            NBChallanDetails = _newemp.ChallanprintDetailsBLL(EmpId, AppId);
            Thread.Sleep(2000);
            //NBChallanDetails.challan_ref_no = "CHN" + DateTime.Now.ToShortDateString() + "_" + EmpId + "_" + AppId;
            return this.PartialView("_PaymentDetails", NBChallanDetails);
        }
        #region Motor Insurance view Data to verify

        public ActionResult ProposerDetailsToView(string PageType, string Category)
        {
            VM_MotorInsuranceProposerDetails _ProposerDetail;
            if(Category=="1")
            {
                _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["RUID"]), "Emp", (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Category));
            }
            else
            {
                _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["RUID"]), (PageType == null) ? "" : PageType, (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Category));
            }
            
            //if (Session["SelectedCategory"] != null && (Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO))|| Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.CASEWORKER))))
            //{
            //    _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["RUID"]), (PageType == null) ? "" : PageType, (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["SelectedCategory"]));
            //}
            //else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            //{
            //    _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["RUID"]), (PageType == null) ? "" : PageType, (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["Categories"]));
            //}
            //else
            //{
            //    _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["RUID"]), (PageType == null) ? "" : PageType, (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["Categories"]));
            //}
            if (!String.IsNullOrEmpty(_ProposerDetail.mipd_kgid_application_number))
            {
                Session["RID"] = _ProposerDetail.mipd_kgid_application_number;

            }

            return this.PartialView("_ProposerDetails", _ProposerDetail);
        }
        public ActionResult VehicleDetailsToView()
        {
            VM_MotorInsuranceVehicleDetails _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetMIVehicleDetails(Convert.ToInt64(Session["RUID"]), Convert.ToInt64(Session["RID"]));

            return this.PartialView("_VehicleDetails", _VehicleDetails);
        }
        public ActionResult OtherDetailsToView()
        {
            VM_MotorInsuranceOtherDetails _OtherData = _IMotorInsuranceVehicleDetailsBll.OtherDetailsBll(Convert.ToInt64(Session["RUID"]), Convert.ToInt64(Session["RID"]));
            if (_OtherData == null)
            {
                _OtherData = new VM_MotorInsuranceOtherDetails();
            }
            //VM_MotorInsuranceVehicleDetails _OtherDetails = _IMotorInsuranceVehicleDetailsBll.GetMultipleVehicleDetailsList();
            return this.PartialView("_OtherDetailsMI", _OtherData);
        }
        public ActionResult IDVDetailsToView()
        {
            VM_MotorInsuranceIDVDetails _IDVData = _IMotorInsuranceVehicleDetailsBll.IDVDetailsBll(Convert.ToInt64(Session["RUID"]), Convert.ToInt64(Session["RID"]));
            if (_IDVData == null)
            {
                _IDVData = new VM_MotorInsuranceIDVDetails();
            }
            return this.PartialView("_IDVDetailsMI", _IDVData);
        }
        public ActionResult PreviousHistoryToView()
        {
            ViewBag.TypeofCover = "Package Policy";
            VM_MotorInsurancePreviousHistoryDetails onjPreviousHistory = _IMotorInsuranceVehicleDetailsBll.PreviousHistoryDetails(Convert.ToInt64(Session["RUID"]), Convert.ToInt64(Session["RID"]));

            return this.PartialView("_PreviousHistoryMI", onjPreviousHistory);
        }
        public ActionResult UploadedDocumentsToView()
        {
            VM_MI_Upload_Documents _MIDocumentData = _IMotorInsuranceVehicleDetailsBll.MIDocumentDetailsBll(Convert.ToInt64(Session["RUID"]), Convert.ToInt64(Session["RID"]));

            return this.PartialView("_MIDocumentDetails", _MIDocumentData);
        }

        public ActionResult RenewalUploadedDocumentsToView(string PageType, string refNo)
        {
            VM_MI_Upload_Documents _MIDocumentData = _IMotorInsuranceVehicleDetailsBll.MIDocumentDetailsBll(Convert.ToInt64(Session["RUID"]), Convert.ToInt64(refNo));
            _MIDocumentData.MIPAgetype = PageType;
            return this.PartialView("_MIDocumentDetails", _MIDocumentData);
        }


        public ActionResult MIPaymentDetailsToView(string PageType,string empId,string applicationId)
        {
            PolicyPremiumDetailMI obj = _IMotorInsuranceVehicleDetailsBll.selectPaymentDetailsMI(PageType, Convert.ToInt64(empId), Convert.ToInt32(applicationId));
            if (Session["MBVALUE"] != null && Convert.ToBoolean(Session["MBVALUE"]) == true)
            {
                obj.DDOCode = "12026D";                
            }
            return this.PartialView("_MIPaymentDetails",obj);
        }
        public ActionResult MIRenewalDetailsToView(long empId, int applicationId)
        {
            VM_MotorInsuranceRenewalDetails obj = _IMotorInsuranceRenewalDetailsBll.MIRenwalDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt32(Session["SelectedCategory"]));
            VM_MotorInsuranceRenewalDetails .MotorInsuranceRenewalDetailsMI objMI= obj.MotorInsuranceRenewalDetails.Where(a => a.MIApplicationId == applicationId).Select(a => a).FirstOrDefault();
            return this.PartialView("MIRenewalDetails", objMI);
        }
        #endregion

        public ActionResult RenewalProposerDetailsToView(string PageType, string refNo, string Category)
        {
            VM_MotorInsuranceProposerDetails _ProposerDetail = new VM_MotorInsuranceProposerDetails();
            _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["RUID"]), (PageType == null) ? "" : PageType, Convert.ToInt64(refNo), Convert.ToInt32(Category));
            //if (Session["SelectedCategory"] != null && Session["SelectedCategory"].ToString().Contains(Convert.ToString((int)UserCategories.DDO)))
            //{
            //    _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["RUID"]), (PageType == null) ? "" : PageType, Convert.ToInt64(refNo), Convert.ToInt32(Session["SelectedCategory"]));
            //}
            //else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            //{
            //    _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["RUID"]), (PageType == null) ? "" : PageType, Convert.ToInt64(refNo), Convert.ToInt32(Session["Categories"]));
            //}
            //else
            //{
            //    _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["RUID"]), "Emp", 0, Convert.ToInt32(Session["Categories"]));
            //}

            if (!String.IsNullOrEmpty(_ProposerDetail.mipd_kgid_application_number))
            {
                Session["RID"] = _ProposerDetail.mipd_kgid_application_number;
                TempData["Deptmartment"] = _ProposerDetail.mipd_Department;
            }
            return this.PartialView("_ProposerDetails", _ProposerDetail);
        }
        public ActionResult RenewalVehicleDetailsToView(string PageType, long refNo,long RenewalRefNo)
        {
            VM_MotorInsuranceVehicleDetails _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetMIRenewalVehicleDetails(Convert.ToInt64(Session["RUID"]), refNo,RenewalRefNo);
            _VehicleDetails.mivd_pagetype = PageType;

            return this.PartialView("_VehicleDetails", _VehicleDetails);
        }
        public ActionResult RenewalIDVDetailsToView(string PageType, string refNo = "")
        {
            VM_MotorInsuranceIDVDetails _IDVData = _IMotorInsuranceVehicleDetailsBll.IDVDetailsBll(Convert.ToInt64(Session["RUID"]), Convert.ToInt64(refNo));
            if (_IDVData == null)
            {
                _IDVData = new VM_MotorInsuranceIDVDetails();
            }
            _IDVData.miidv_pagetype = PageType;
            return this.PartialView("_IDVDetailsMI", _IDVData);
        }
        public ActionResult RenewalPreviousHistoryToView(string PageType, string refNo = "")
        {
            ViewBag.TypeofCover = "Package Policy";
            VM_MotorInsurancePreviousHistoryDetails onjPreviousHistory = _IMotorInsuranceVehicleDetailsBll.PreviousHistoryDetails(Convert.ToInt64(Session["RUID"]), Convert.ToInt64(refNo));
            onjPreviousHistory.mivd_pagetype = PageType;
            return this.PartialView("_PreviousHistoryMI", onjPreviousHistory);
        }
        public ActionResult RenewalOtherDetailsToView(string PageType, string refNo = "")
        {
            VM_MotorInsuranceOtherDetails _OtherData = _IMotorInsuranceVehicleDetailsBll.OtherDetailsBll(Convert.ToInt64(Session["RUID"]), Convert.ToInt64(refNo));
            if (_OtherData == null)
            {
                _OtherData = new VM_MotorInsuranceOtherDetails();
            }
            _OtherData.miod_PageType = PageType;
            return this.PartialView("_OtherDetailsMI", _OtherData);
        }
        #region Print Application
        public ActionResult PBasicDetailsToView()
        {
            VM_BasicDetails _BasicData = _INBApplicationbll.BasicDetailsBll(Convert.ToInt64(Session["UID"]));
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
            return this.PartialView("_PBasicDetails", _BasicData);
        }
        public ActionResult PKGIDDetailsToView()
        {
            VM_PolicyDetails _KGIDData = _INBApplicationbll.KGIDDetailsBll(Convert.ToInt64(Session["UID"]));
            if (_KGIDData == null)
            {
                _KGIDData = new VM_PolicyDetails();
            }

            return this.PartialView("_PKGIDDetails", _KGIDData);
        }
        public ActionResult PFamilyDetailsToView()
        {
            VM_FamilyDetails _FamilyData = _INBApplicationbll.FamilyDetailsBll(Convert.ToInt64(Session["UID"]));
            if (_FamilyData == null)
            {
                _FamilyData = new VM_FamilyDetails();
            }

            return this.PartialView("_PFamilyDetails", _FamilyData);
        }
        public ActionResult PNomineeDetailsToView()
        {
            VM_NomineeDetails _NomineeData = _INBApplicationbll.NomineeDetailsBll(Convert.ToInt64(Session["UID"]));
            if (_NomineeData == null)
            {
                _NomineeData = new VM_NomineeDetails();
            }

            return this.PartialView("_PNomineeDetails", _NomineeData);
        }
        public ActionResult PPersonalDetailsToView()
        {
            long EmpID = 0;
            if (Session["UID"] == null)
            {
                EmpID = Convert.ToInt64(Session["UID"]);
            }
            else
            {
                EmpID = Convert.ToInt64(Session["UID"]);
            }
            VM_PersonalHealthDetails _PersonalData = _INBApplicationbll.PersonalDetailsBll(EmpID);
            if (_PersonalData == null)
            {
                _PersonalData = new VM_PersonalHealthDetails();
            }

            return this.PartialView("_PPersonalDetails", _PersonalData);
        }
        #endregion
		
		
		public ActionResult PProposerDetailsToView(string PageType, string Category)
        {
            VM_MotorInsuranceProposerDetails _ProposerDetail = new VM_MotorInsuranceProposerDetails();
            if (Convert.ToInt32(Session["SelectedCategory"]) == 2)
            {
                _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), (PageType == null) ? "" : PageType, (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }
            else if (Session["Categories"] != null && Session["Categories"].ToString().Contains(Convert.ToString((int)UserCategories.AGENCY)))
            {
                _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), (PageType == null) ? "" : PageType, (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["Categories"]));
            }
            else if (Session["MBVALUE"] != null && Convert.ToBoolean(Session["MBVALUE"]) == true)
            {
                _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), (PageType == null) ? "" : PageType, (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["Categories"]));
            }
            else if(Convert.ToInt32(Session["SelectedCategory"]) == 1)
            {
                _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), "Emp", (Convert.ToInt64(Session["RID"]) == 0) ? 0 : Convert.ToInt64(Session["RID"]), Convert.ToInt32(Session["SelectedCategory"]));
            }

            return this.PartialView("_PProposerDetails", _ProposerDetail);
        }
        public ActionResult PVehicleDetailsToView()
        {
            VM_MotorInsuranceVehicleDetails _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetMIVehicleDetails(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));

            return this.PartialView("_PVehicleDetails", _VehicleDetails);
        }
        public ActionResult POtherDetailsToView()
        {
            VM_MotorInsuranceOtherDetails _OtherData = _IMotorInsuranceVehicleDetailsBll.OtherDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));
            if (_OtherData == null)
            {
                _OtherData = new VM_MotorInsuranceOtherDetails();
            }
            //VM_MotorInsuranceVehicleDetails _OtherDetails = _IMotorInsuranceVehicleDetailsBll.GetMultipleVehicleDetailsList();
            return this.PartialView("_POtherDetailsMI", _OtherData);
        }
        public ActionResult PIDVDetailsToView()
        {
            VM_MotorInsuranceIDVDetails _IDVData = _IMotorInsuranceVehicleDetailsBll.IDVDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));
            if (_IDVData == null)
            {
                _IDVData = new VM_MotorInsuranceIDVDetails();
            }
            return this.PartialView("_PIDVDetailsMI", _IDVData);
        }
        public ActionResult PPreviousHistoryToView()
        {
            ViewBag.TypeofCover = "Package Policy";
            VM_MotorInsurancePreviousHistoryDetails onjPreviousHistory = _IMotorInsuranceVehicleDetailsBll.PreviousHistoryDetails(Convert.ToInt64(Session["UID"]), Convert.ToInt64(Session["RID"]));

            return this.PartialView("_PPreviousHistoryMI", onjPreviousHistory);
        }

        #region Renewal Print Application

        public ActionResult PRenewalProposerDetailsToView(string PageType, string refNo, string Category)
        {
            VM_MotorInsuranceProposerDetails _ProposerDetail = new VM_MotorInsuranceProposerDetails();
            Category = Convert.ToString(Session["SelectedCategory"]);
            _ProposerDetail = _IMotorInsuranceProposerDetailsBll.MIProposerDetailsBll(Convert.ToInt64(Session["UID"]), (PageType == null) ? "" : PageType, Convert.ToInt64(refNo), Convert.ToInt32(Category));

            if (!String.IsNullOrEmpty(_ProposerDetail.mipd_kgid_application_number))
            {
                Session["RID"] = _ProposerDetail.mipd_kgid_application_number;
                TempData["Deptmartment"] = _ProposerDetail.mipd_Department;
            }
            return this.PartialView("_PProposerDetails", _ProposerDetail);
        }
        public ActionResult PRenewalVehicleDetailsToView(string PageType, long refNo = 0, long RenewalRefNo = 0)
        {
            RenewalRefNo = Convert.ToInt64(Session["RID"]);
            VM_MotorInsuranceVehicleDetails _VehicleDetails = _IMotorInsuranceVehicleDetailsBll.GetMIRenewalVehicleDetails(Convert.ToInt64(Session["UID"]), refNo, RenewalRefNo);
            _VehicleDetails.mivd_pagetype = PageType;
            return this.PartialView("_PVehicleDetails", _VehicleDetails);
        }
        public ActionResult PRenewalIDVDetailsToView(string PageType, long refNo = 0, long RenewalRefNo = 0)
        {
            RenewalRefNo = Convert.ToInt64(Session["RID"]);
            VM_MotorInsuranceIDVDetails _IDVData = _IMotorInsuranceVehicleDetailsBll.IDVRenewalDetailsBll(Convert.ToInt64(Session["UID"]), refNo, RenewalRefNo);
            if (_IDVData == null)
            {
                _IDVData = new VM_MotorInsuranceIDVDetails();
            }
            _IDVData.miidv_pagetype = PageType;
            return this.PartialView("_PIDVDetailsMI", _IDVData);
        }
        public ActionResult PRenewalPreviousHistoryToView(string PageType, string refNo = "")
        {
            ViewBag.TypeofCover = "Package Policy";
            VM_MotorInsurancePreviousHistoryDetails onjPreviousHistory = _IMotorInsuranceVehicleDetailsBll.PreviousHistoryDetails(Convert.ToInt64(Session["UID"]), Convert.ToInt64(refNo));
            onjPreviousHistory.mivd_pagetype = PageType;
            return this.PartialView("_PPreviousHistoryMI", onjPreviousHistory);
        }
        public ActionResult PRenewalOtherDetailsToView(string PageType, string refNo = "")
        {
            VM_MotorInsuranceOtherDetails _OtherData = _IMotorInsuranceVehicleDetailsBll.OtherDetailsBll(Convert.ToInt64(Session["UID"]), Convert.ToInt64(refNo));
            if (_OtherData == null)
            {
                _OtherData = new VM_MotorInsuranceOtherDetails();
            }
            _OtherData.miod_PageType = PageType;
            return this.PartialView("_POtherDetailsMI", _OtherData);
        }

        #endregion

    }
}