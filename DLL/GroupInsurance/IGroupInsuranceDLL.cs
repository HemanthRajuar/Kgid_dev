using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.KGID_GroupInsurance;
using KGID_Models.KGID_Master;
using KGID_Models.KGID_Policy;
using KGID_Models.KGID_User;
using KGID_Models.NBApplication;

namespace DLL.GroupInsurance
{
  public  interface IGroupInsuranceDLL
    {
        //Task<long> GenerateGISApplicationNumber(long empid);
        // long GenerateGISApplicationNumber(long empid);
        Task<VM_ApplicationDetail> GenerateGISApplicationNumber(long empid);

       //VM_ApplicationDetail GenerateGISApplicationNumber(long empid);
        // Task<VM_ApplicationDetail> GISGetEmployeeDetails(long empid);
        VM_BasicDetails GISGetEmployeeDetails(long empid);
        List<VM_DropDownList> GetNomineelist(long empId);
         Task<long> GISSaveEmpBasicDetails(VM_BasicDetails employeeBasicDetails);
        Task<long> GISSaveNBNominee (VM_NomineeDetail objNominee);
        Task<int> GISDeleteNominee(VM_NomineeDetail objNominee);
        List<VM_NomineeDetail> GISNomineeDetailsDll(long empid);
        float GisGetIntialPaymentDetails(long empId);
        VM_GISPaymentDetails GISPaymentDll(long EmpID);
        long GISSavePaymentDll(VM_GISPaymentDetails objPaymentDetails);
        string AddEmployeeBasicDetails(VM_BasicDetails employeeDetails);

        int GISSaveDeclaration(long EmpId, long AppId);

        VM_GISDDOVerificationDetails GetEmployeeDetailsForDDOVerification(long empId);

        VM_ChallanPrintDetails ChallanprintDetails(long EmpId, long AppId);

        long GISSaveChallanStatusDll(VM_GISPaymentDetails objPaymentDetails);

        string GISSaveVerifiedDetails(VM_GISDeptVerificationDetails objVerification);

        int GISSaveEmployeeForm(tbl_GIS_upload_employeeform objEmpForm);

        VM_GIS_Upload_EmployeeForm GetUploadDoc(long _EmpId);
        IList<VM_GISWorkflowDetail> GetWorkFlowDetails(long applicationId);

        VM_GISDeptVerificationDetails GetUploadedDocuments(long empId, long applicationId);
        Task<long> GetGISApplicationStatus(long empid);

        VM_GISDDOVerificationDetails GISGetEmployeeApplicationStatusDll(long empId);

        VM_GIS_Upload_EmployeeForm GetUploadDocDll(long _EmpId);

        IList<KGID_Models.KGID_GroupInsurance.UploadedDocuments> GetUploadedAdoptionFile(long empId, long applicationId);

        //KUNAL

        GIS_CliamDetails GISProposerDetailsBll(long employeeCode, string PageType, long RefNo, int Category);

        long SaveGISProposalAppnRefNo(GIS_CliamDetails objPD);

        GIS_Ledger GetEMPGISLedgerDll(string EmpId);

        //14 dec


        // IList<GIS_Ledger_det> GetEmployeeLedgerDeatils(long empIdKGID);
        IList<VM_ClaimLedger> GetEmployeeLedgerDeatils(long empIdKGID);

       IList<tbl_employee_group_master> GetGroup_Masters();
        IList<tbl_year_master> GetYear_Masters();
       IList<tbl_month_master> GetMonth_Masters();

        bool AddLedgerDetails(IList<VM_ClaimLedger> LedgerDeatils);
        long GetEmployeeDetailsByKgid(long kgidNum);

        bool UpdateLedgerDetails(IList<VM_ClaimLedger> LedgerDeatils);

         Task<long> GISSaveNomineeBankDetails(VM_NomineeDetail objNomineeBank);

        VM_GISDDOVerificationDetails GISGetSubscriptionDetails(long empid);

        string GISUpdateChallanStatusDll(string ChallanRefNo, long StatusCode, long EmpID);

        void SaveLogs(string exceptionDesc, long CreatedBy);
      
    }
}
