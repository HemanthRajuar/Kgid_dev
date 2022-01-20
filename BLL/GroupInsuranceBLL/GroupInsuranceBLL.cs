using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.GroupInsurance;
using KGID_Models.KGID_GroupInsurance;
using KGID_Models.KGID_Master;
using KGID_Models.KGID_Policy;
using KGID_Models.KGID_User;
using KGID_Models.NBApplication;

namespace BLL.KGIDGroupInsuranceBLL
{
    public class GroupInsuranceBLL : IGroupInsuranceBLL
    {
        private readonly IGroupInsuranceDLL _IGroupInsurancedll;
        public GroupInsuranceBLL()
        {
            this._IGroupInsurancedll = new GroupInsuranceDLL();
        }
        public async Task<VM_ApplicationDetail> GenerateGISApplicationNumber(long empid)
        {
            return await _IGroupInsurancedll.GenerateGISApplicationNumber(empid);
        }

        public VM_BasicDetails GISGetEmployeeDetails(long empid)
        {
            return _IGroupInsurancedll.GISGetEmployeeDetails(empid);
        }
        public List<VM_DropDownList> GetNomineelist(long empId)
        {
            return _IGroupInsurancedll.GetNomineelist(empId);
        }
        public async Task<long> GISSaveEmpBasicDetails(VM_BasicDetails employeeBasicDetails)
        {
            return await _IGroupInsurancedll.GISSaveEmpBasicDetails(employeeBasicDetails);
        }
        public async Task<long> GISSaveNBNominee(VM_NomineeDetail objNominee)
        {
            return await _IGroupInsurancedll.GISSaveNBNominee(objNominee);
        }
        public async Task<int> GISDeleteNominee(VM_NomineeDetail objNominee)
        {
            return await _IGroupInsurancedll.GISDeleteNominee(objNominee);
        }       
        public List<VM_NomineeDetail> GISNomineeDetailsDll(long empid)
        {
            return _IGroupInsurancedll.GISNomineeDetailsDll(empid);
        }

        public float GisGetIntialPaymentDetails(long empId)
        {
            return _IGroupInsurancedll.GisGetIntialPaymentDetails(empId);
        }
        
        public VM_GISPaymentDetails GISPaymentDll(long EmpID)
        {
            return _IGroupInsurancedll.GISPaymentDll(EmpID);
        }

        public long GISSavePaymentDll(VM_GISPaymentDetails objPaymentDetails)
        {
            return _IGroupInsurancedll.GISSavePaymentDll(objPaymentDetails);
        }

        public string AddEmployeeBasicDetails(VM_BasicDetails employeeDetails)
        {
            return _IGroupInsurancedll.AddEmployeeBasicDetails(employeeDetails);
        }
        public int GISSaveDeclaration(long EmpId, long AppId)
        {
            return _IGroupInsurancedll.GISSaveDeclaration(EmpId, AppId);
        }
        public  VM_GISDDOVerificationDetails GetEmployeeDetailsForDDOVerification(long empId)
        {
            return _IGroupInsurancedll.GetEmployeeDetailsForDDOVerification(empId);
        }
        public VM_ChallanPrintDetails ChallanprintDetails(long EmpId, long AppId)
        {
            return _IGroupInsurancedll.ChallanprintDetails(EmpId, AppId);
        }

        public long GISSaveChallanStatusDll(VM_GISPaymentDetails objPaymentDetails)
        {
            return _IGroupInsurancedll.GISSaveChallanStatusDll(objPaymentDetails);
        }
        public string GISSaveVerifiedDetails(VM_GISDeptVerificationDetails objVerification)
        {
            return _IGroupInsurancedll.GISSaveVerifiedDetails(objVerification);
        }


        public int GISSaveEmployeeForm(tbl_GIS_upload_employeeform objEmpForm)
        {
            return _IGroupInsurancedll.GISSaveEmployeeForm(objEmpForm);
        }

        public VM_GIS_Upload_EmployeeForm GetUploadDoc(long _EmpId)
        {
            return _IGroupInsurancedll.GetUploadDoc(_EmpId);
        }


        public IList<VM_GISWorkflowDetail> GetWorkFlowDetails(long applicationId)
        {
            return _IGroupInsurancedll.GetWorkFlowDetails(applicationId);
        }
        public VM_GISDeptVerificationDetails GetUploadedDocuments(long empId, long applicationId)
        {
            return _IGroupInsurancedll.GetUploadedDocuments(empId,applicationId);
        }
        public Task<long> GetGISApplicationStatus(long empid)
        {
            return _IGroupInsurancedll.GetGISApplicationStatus(empid);
        }

        public VM_GISDDOVerificationDetails GISGetEmployeeApplicationStatusDll(long empId)
        {
            return _IGroupInsurancedll.GISGetEmployeeApplicationStatusDll(empId);
        }

        public VM_GIS_Upload_EmployeeForm GetUploadDocDll(long _EmpId)
        {
            return _IGroupInsurancedll.GetUploadDocDll(_EmpId);
        }

        public IList<KGID_Models.KGID_GroupInsurance.UploadedDocuments> GetUploadedAdoptionFile(long empId, long applicationId)
        {
            return _IGroupInsurancedll.GetUploadedAdoptionFile(empId, applicationId);
        }

        // KUNAL 

        public GIS_CliamDetails GISProposerDetailsBll(long EmployeeCode, string PageType, long RefNo, int Category)
        {
            return _IGroupInsurancedll.GISProposerDetailsBll(EmployeeCode, PageType, RefNo, Category);
        }
        public long SaveGISProposalAppnRefNo(GIS_CliamDetails objPD)
        {
            return _IGroupInsurancedll.SaveGISProposalAppnRefNo(objPD);
        }

        public GIS_Ledger GetEMPGISLedgerBll(string EmpId)
        {
            return _IGroupInsurancedll.GetEMPGISLedgerDll(EmpId);
        }

        //14 dec

        //public IList<GIS_Ledger_det> GetEmployeeLedgerDeatils(long empIdKGID)
        //{
        //    return _IGroupInsurancedll.GetEmployeeLedgerDeatils(empIdKGID);
        //}


        public IList<VM_ClaimLedger> GetEmployeeLedgerDeatils(long empIdKGID)
        {
            return _IGroupInsurancedll.GetEmployeeLedgerDeatils(empIdKGID);
        }

        public IList<tbl_employee_group_master> GetGroup_Masters()
        {
            return _IGroupInsurancedll.GetGroup_Masters();
        }

        public IList<tbl_year_master> GetYear_Masters()
        {
            return _IGroupInsurancedll.GetYear_Masters();
        }

        public IList<tbl_month_master> GetMonth_Masters()

        {
            return _IGroupInsurancedll.GetMonth_Masters();
        }



        public bool AddLedgerDetails(IList<VM_ClaimLedger> LedgerDeatils)

        {
            return _IGroupInsurancedll.AddLedgerDetails(LedgerDeatils);
        }

        public long GetEmployeeDetailsByKgid(long kgidNum)
        {
            return _IGroupInsurancedll.GetEmployeeDetailsByKgid(kgidNum);
        }


        public bool UpdateLedgerDetails(IList<VM_ClaimLedger> LedgerDeatils)
        {
            return _IGroupInsurancedll.UpdateLedgerDetails(LedgerDeatils);
        }
        public Task<long> GISSaveNomineeBankDetails(VM_NomineeDetail objNomineeBank)
        {
            return _IGroupInsurancedll.GISSaveNomineeBankDetails(objNomineeBank);
        }

        public VM_GISDDOVerificationDetails GISGetSubscriptionDetails(long empid)
        {
            return _IGroupInsurancedll.GISGetSubscriptionDetails(empid);
        }

        public string GISUpdateChallanStatusDll(string ChallanRefNo, long StatusCode, long EmpID)
        {
            return _IGroupInsurancedll.GISUpdateChallanStatusDll(ChallanRefNo, StatusCode, EmpID);
        }

        public void SaveLogs(string exceptionDesc, long CreatedBy)
        {
             _IGroupInsurancedll.SaveLogs(exceptionDesc, CreatedBy);
        }
       


    }
}
