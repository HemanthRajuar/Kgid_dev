using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGID_Models.KGID_GroupInsurance;
using KGID_Models.KGID_Policy;
using KGID_Models.NBApplication;

namespace BLL.KGIDGISNomineeChangeReqBLL
{
   public interface IGISNomineeChangeReqBLL
    {
        Task<long> GIS_NR_SaveNominee(VM_NomineeDetail objNominee);
        Task<int> GISSaveDeclaration(long EmpId, long AppId);
        Task<int> GIS_NR_UpdateWorkFlow(VM_GISDeptVerificationDetails objEmpForm);
        Task<int> GIS_NR_UploadForms(tbl_GIS_NR_upload_form objEmpForm);
        Task<VM_GISDDOVerificationDetails> GetEmployeeNomineeDetailsForDDOVerification(long empId);
        IList<VM_GISWorkflowDetail> GetWorkFlowDetails(long applicationId);
        string GISSaveVerifiedDetails(VM_GISDeptVerificationDetails objVerification);
        Task<long> GetGIS_NCRApplicationStatus(long empid);

        VM_GIS_Upload_EmployeeForm GetUploadDoc(long _EmpId);

        VM_GISDeptVerificationDetails GetUploadedDocuments(long empId, long applicationId);

        Task<long> GISSaveNBNomineeChangeRequest(VM_NomineeDetail objNominee);
    }
}
