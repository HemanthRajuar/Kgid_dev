using KGID_Models.KGID_Policy;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using KGID_Models.NBApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.KGIDMotorInsurance
{
  public  interface IMotorInsuranceVehicleDetailsBll
    {
     
        string SaveMIVehicleDetailsData(VM_MotorInsuranceVehicleDetails vmVehicleDetails);
        VM_MotorInsuranceVehicleDetails GetRTODetailsBll(string chasisNo, string EngineNo);

        VM_MotorInsuranceOtherDetails OtherDetailsBll(long EmployeeCode, long ReferenceId);
        int SaveOtherDetailsBll(VM_MotorInsuranceOtherDetails SaveOtherDetailsBll);

        VM_MotorInsuranceIDVDetails IDVDetailsBll(long EmployeeCode, long ReferenceId);
        int SaveIDVDetailsBll(VM_MotorInsuranceIDVDetails SaveIDVDetailsBll);
    
	    VM_MotorInsurancePreviousHistoryDetails PreviousHistoryDetails(long EmployeeCode, long ReferenceId);
        string SaveMIPreviousHistoryDetails(VM_MotorInsurancePreviousHistoryDetails vmPreviousHistoryDetails);

        VM_MI_Upload_Documents MIDocumentDetailsBll(long EmployeeCode, long ReferenceId);
        int SaveMIDocumentDetailsBll(VM_MI_Upload_Documents SaveMIDocumentDetailsBll);
        string SaveMIChallanDetailsBll(long EmpID, int Category, string RefNos, int ChallanAmount, string Type);

        
        VM_MotorInsuranceIDVDetails IDVRenewalDetailsBll(long EmployeeCode, long refNo, long RenewalRefNo);
        
        string InsertChallanDetailsBll(long EmpID, int Category, string RefNos, int ChallanAmount, string sanno, DateTime sandate, string voucherno, string hoa, string ddocode, string Type);


        VM_MotorInsuranceVehicleDetails GetMIVehicleDetails(long Empid, long RefID);
        string CheckVehicleExists(string chassisNo, string engineNo);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForDDOVerification(long EmpID);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForCWVerification(long EmpID);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForSuperintendentVerification(long EmpID);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForDIOVerification(long EmpID);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForDDVerification(long EmpID);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForADVerification(long EmpID);
        VM_DDOVerificationDetailsMI GetEmployeeDetailsForDVerification(long EmpID);
        IList<VM_MIWorkFlowDetails> GetWorkFlowDetails(long applicationId, int category);
       string SaveVerifiedDetailsBll(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails);
        VM_MotorInsuranceVehicleDetails GetModelListBasedonMake(int makeid);
        VM_DDOVerificationDetailsMI getMIApplicationEmployeeList(long empId,int Category);
        VM_DDOVerificationDetailsMI getMIApplicationEmployeeStatusList(long empId, int Category);

        PolicyPremiumDetailMI selectPaymentDetailsMI(string pagetype,long empId, int applicationId);
        IEnumerable<tbl_vehicle_category_master> GetVehCatergoryList(string TypeId, long SubTypeID);
        VM_MotorInsuranceVehicleDetails GetManufactureListBasedonMake(int makeid);
        VM_MotorInsuranceVehicleDetails GetTypeOfVehiclebasedonCategory(int categoryID);

        VM_MotorInsuranceVehicleDetails GetMIRenewalVehicleDetails(long Empid, long RefID,long RenewalRefNo);
		VM_MotorInsurancePolicyPrintDetails MIPolicyPrintDetailsBll(string Type,long EmployeeCode, long ReferenceId);

        string GetDateOfRisk_bll(string ReferanceNo);

        //Save MB Bond
        string MBBondDocUploadBLL(long AppId, long EmpId, string DocPath, string DocType);
        //Sign MB Bond
        string GetMBBondDocFileBLL(long AppId, long EmpId);
        string MBSignBondUploadBLL(long AppId, long EmpId, string DocPath);

        VM_DDOVerificationDetailsMI getMIRenewalApplicationList(long empId, int Category);

        VM_MBApplicationDetails GetMBApplicationListBll(long EmpID, int Category);
        string GetEmployeeLoanDetails(long UserID);
        //GetMBBondDetails
        VM_DDOVerificationDetailsMI GetMBBondDetailsBll(long empId);
        #region Renewal Workflow
        VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForCWVerification(long EmpID);
        VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForSuperintendentVerification(long EmpID);
        VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForADVerification(long EmpID);
        VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForDDVerification(long EmpID);
        VM_DDOVerificationDetailsMI GetEmployeeRenewalDetailsForDVerification(long EmpID);

        IList<VM_MIWorkFlowDetails> GetRenewalWorkFlowDetails(long applicationId, int category);
        string SaveRenewalVerifiedDetailsBll(VM_MotorInsuranceDeptVerficationDetails objVerifyDetails);
        VM_MotorInsuranceVehicleDetails BindVahanResponseDetailstoModel(dynamic responseStr);
        #endregion

        VM_MotorInsuranceCancellation GetCancelReasonList();
        int MIAppCancelRequestAction(VM_MotorInsuranceCancellation objMIcancellation);
        string CheckVehicleNo(string vehicleNo, int Category);

        VM_MotorInsuranceCancellation VehicleDetailsForCancellation(string vehicleNo);
        // Transfer Vehicle
        string CheckTransferVehicleNo(string vehicleNo);
        VM_MotorInsuranceVehicleTransfer VehicleDetailsForTransfer(string vehicleNo);

        // ICT  18-09-2021
        string UpdateBPS025AckBll(string AckFilePath, string ChallanRefno);
        VM_MotorInsurancePaymentStatus MotorInsurancePaymentStatusBll(string EmpId);

        string UpdateBPS025RequestBll(string Upload, string ChallanRefno);

        VM_ChallanPrintDetails PrintMIChallanDetailsBll(long EmpID, int Category, string RefNos, string Type);

        VM_DDOVerificationDetailsMI GetChallanDetailsBll(string ChallanNo, long EmpID, int Category);

        //ict 30-10-2021
        //string InsertotcChallanDetailsBll(long EmpID, int Category, string RefNos, int ChallanAmount, string sanno, DateTime sandate, string voucherno, string hoa, string ddocode, string Type);
        //VM_DDOVerificationDetailsMI GetotcChallanDetailsBll(string ChallanNo, long EmpID);

        //string UpdateRCT0335AckBll(string ChallanRefno, string statusflag, string statusMessage);

        //VM_MotorInsurancePaymentStatus MotorInsuranceotcPaymentStatusBll(string EmpId);

        //long SaveMBChallanStatusDll(VM_PaymentDetails objPaymentDetails);
        string InsertotcChallanDetailsBll(long EmpID, int Category, string RefNos, int ChallanAmount, string sanno, DateTime sandate, string voucherno, string hoa, string ddocode, string Type);
        VM_DDOVerificationDetailsMI GetotcChallanDetailsBll(string ChallanNo, long EmpID, int Categories);

        string UpdateRCT0335AckBll(string ChallanRefno, string statusflag, string statusMessage);

        VM_MotorInsurancePaymentStatus MotorInsuranceotcPaymentStatusBll(string EmpId);

        long SaveMBChallanStatusDll(VM_PaymentDetails objPaymentDetails);

        // 18-11-2021
        string InsertRenewalChallanDetailsBll(long EmpID, int Category, string RefNos, int ChallanAmount, string sanno, DateTime sandate, string voucherno, string hoa, string ddocode, string Type);

        VM_DDOVerificationDetailsMI GetRenewalChallanDetailsBll(string ChallanNo, long EmpID);

        string InsertRenewalotcChallanDetailsBll(long EmpID, int Category, string RefNos, int ChallanAmount, string sanno, DateTime sandate, string voucherno, string hoa, string ddocode, string Type);

        VM_DDOVerificationDetailsMI GetotcRenewalChallanDetailsBll(string ChallanNo, long EmpID);
    }
}
