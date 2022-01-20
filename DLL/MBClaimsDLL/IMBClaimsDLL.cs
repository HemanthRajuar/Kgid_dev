using KGID_Models.KGID_MB_Claim;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.MBClaimsDLL
{
    public interface IMBClaimsDLL
    {
        VM_MIOwnDamageClaimDetails GetMIOwnDamageClaimDetailsDLL(long empId, int category);
        VM_ODClaimVerificationDetails GetODClaimApplicationStatusListDLL(long empId, int category);
        List<tbl_district_master> GetDistListDLL();
        List<tbl_taluka_master> GetTalukaListDLL(int DistId);
        List<tbl_od_cost_component_master> GetComponentListDLL();

        long SaveODClaimApplicationDetailsDLL(VM_ODClaimApplicationDetails objCAD);
        VM_ODClaimApplicationDetails GetODClaimApplicationDetailsDLL(long Empid, string PolicyNumber);

        //OD Claim Workflow
        VM_ODClaimVerificationDetails GetEmployeeDetailsForCWVerificationDLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForSuperintendentVerificationDLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForADVerificationDLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForDDVerificationDLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForDVerificationDLL(long EmpID, string Category);

        VM_MIODClaimDeptVerficationDetails GetWorkFlowDetailsDLL(long applicationId, int category);
        string SaveVerifiedDetailsDLL(VM_MIODClaimDeptVerficationDetails objVerifyDetails);

        // Surveyor Workflow
        VM_ODClaimSurveyorVerificationDetails GetEmployeeDetailsForSurveyorVerificationDLL(long EmpID);

        // View Application
        VM_ODClaimApprovedApplicationDetails GetApprovedApplicationListDLL(long EmpID, string Category);
        VM_ODClaimWorkOrderDetails GetODClaimAprvdAppDetailsDLL(long Empid, string PolicyNumber, string Category);
    }
}
