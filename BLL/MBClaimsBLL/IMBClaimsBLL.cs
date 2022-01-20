using KGID_Models.KGID_MB_Claim;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.MBClaimsBLL
{
    public interface IMBClaimsBLL
    {
        VM_MIOwnDamageClaimDetails GetMIOwnDamageClaimDetailsBLL(long empId, int category);
        VM_ODClaimVerificationDetails GetODClaimApplicationStatusListBLL(long empId, int category);
        List<tbl_district_master> GetDistListBLL();
        List<tbl_taluka_master> GetTalukaListBLL(int DistId);
        List<tbl_od_cost_component_master> GetComponentListBLL();

        long SaveODClaimApplicationDetailsBLL(VM_ODClaimApplicationDetails objCAD);
        VM_ODClaimApplicationDetails GetODClaimApplicationDetailsBLL(long Empid, string PolicyNumber);

        #region OD Claim Workflow
        //OD Claim Workflow
        VM_ODClaimVerificationDetails GetEmployeeDetailsForCWVerificationBLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForSuperintendentVerificationBLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForADVerificationBLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForDDVerificationBLL(long EmpID, string Category);
        VM_ODClaimVerificationDetails GetEmployeeDetailsForDVerificationBLL(long EmpID, string Category);

        VM_MIODClaimDeptVerficationDetails GetWorkFlowDetailsBLL(long applicationId, int category);
        string SaveVerifiedDetailsBLL(VM_MIODClaimDeptVerficationDetails objVerifyDetails);
        #endregion

        // Surveyor Workflow
        VM_ODClaimSurveyorVerificationDetails GetEmployeeDetailsForSurveyorVerificationBLL(long EmpID);
        // View Application
        VM_ODClaimApprovedApplicationDetails GetApprovedApplicationListBLL(long EmpID, string Category);
        VM_ODClaimWorkOrderDetails GetODClaimAprvdAppDetailsBLL(long Empid, string PolicyNumber, string Category);
    }
}
