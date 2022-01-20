using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.GroupInsurance;
using KGID_Models.KGID_GroupInsurance;
using KGID_Models.KGID_Policy;
using KGID_Models.NBApplication;

using DLL.GISNomineeChangeReqDLL;
using System.Collections.Generic;

namespace BLL.KGIDGISNomineeChangeReqBLL
{
    public class GISNomineeChangeReqBLL : IGISNomineeChangeReqBLL
    {

        private readonly IGISNomineeChangeReqDLL _IGISNomineeChangeReqDLL;

        public GISNomineeChangeReqBLL()
        {
            this._IGISNomineeChangeReqDLL = new GISNomineeChangeReqDLL();
        }

        public async Task<int> GIS_NR_UpdateWorkFlow(VM_GISDeptVerificationDetails objEmpForm)
        {
            return await _IGISNomineeChangeReqDLL.GIS_NR_UpdateWorkFlow(objEmpForm);
        }


        public async Task<int> GIS_NR_UploadForms(tbl_GIS_NR_upload_form objEmpForm)
        {
            return await _IGISNomineeChangeReqDLL.GIS_NR_UploadForms(objEmpForm);
        }

        public async Task<long> GIS_NR_SaveNominee(VM_NomineeDetail objNominee)
        {
            return await _IGISNomineeChangeReqDLL.GIS_NR_SaveNominee(objNominee);
        }

        public async Task<int> GISSaveDeclaration(long EmpId, long AppId)
        {
            return await _IGISNomineeChangeReqDLL.GISSaveDeclaration(EmpId, AppId);
        }

        public async Task<VM_GISDDOVerificationDetails> GetEmployeeNomineeDetailsForDDOVerification(long empId)
        {
            return await _IGISNomineeChangeReqDLL.GetEmployeeNomineeDetailsForDDOVerification(empId);
        }

        public IList<VM_GISWorkflowDetail> GetWorkFlowDetails(long applicationId)
        {
            return  _IGISNomineeChangeReqDLL.GetWorkFlowDetails(applicationId);
        }

        public string GISSaveVerifiedDetails(VM_GISDeptVerificationDetails objVerification)
        {
            return _IGISNomineeChangeReqDLL.GISSaveVerifiedDetails(objVerification);
        }
        public  Task<long> GetGIS_NCRApplicationStatus(long empid)
        {
            return _IGISNomineeChangeReqDLL.GetGIS_NCRApplicationStatus(empid);
        }

        public VM_GIS_Upload_EmployeeForm GetUploadDoc(long _EmpId)
        {
            return _IGISNomineeChangeReqDLL.GetUploadDoc(_EmpId);
        }

        public VM_GISDeptVerificationDetails GetUploadedDocuments(long empId, long applicationId)
        {
            return _IGISNomineeChangeReqDLL.GetUploadedDocuments(empId, applicationId);
        }
        public Task<long> GISSaveNBNomineeChangeRequest(VM_NomineeDetail objNominee)
        {
            return _IGISNomineeChangeReqDLL.GISSaveNBNomineeChangeRequest(objNominee);
        }
     
    }
}
