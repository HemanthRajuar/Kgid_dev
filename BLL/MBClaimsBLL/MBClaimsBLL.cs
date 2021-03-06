using DLL.MBClaimsDLL;
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
    public class MBClaimsBLL : IMBClaimsBLL
    {
        private readonly IMBClaimsDLL _IMBClaimsDLL;

        public MBClaimsBLL()
        {
            this._IMBClaimsDLL = new MBClaimsDLL();
            //claims = new MBClaimsDLL();
        }
        public VM_MIOwnDamageClaimDetails GetMIOwnDamageClaimDetailsBLL(long empId, int category)
        {
            return _IMBClaimsDLL.GetMIOwnDamageClaimDetailsDLL(empId, category);
        }
        public VM_ODClaimVerificationDetails GetODClaimApplicationStatusListBLL(long empId, int category)
        {
            return _IMBClaimsDLL.GetODClaimApplicationStatusListDLL(empId, category);
        }
        //Dist List
        public List<tbl_district_master> GetDistListBLL()
        {
            return _IMBClaimsDLL.GetDistListDLL();
        }
        //Taluka List
        public List<tbl_taluka_master> GetTalukaListBLL(int DistId)
        {
            return _IMBClaimsDLL.GetTalukaListDLL(DistId);
        }
        //Component List
        public List<tbl_od_cost_component_master> GetComponentListBLL()
        {
            return _IMBClaimsDLL.GetComponentListDLL();
        }
        //Save OD Claim Application Details
        public long SaveODClaimApplicationDetailsBLL(VM_ODClaimApplicationDetails objCAD)
        {
            return _IMBClaimsDLL.SaveODClaimApplicationDetailsDLL(objCAD);
        }
        //Get OD Claim Application Details
        public VM_ODClaimApplicationDetails GetODClaimApplicationDetailsBLL(long Empid, string PolicyNumber)
        {
            return _IMBClaimsDLL.GetODClaimApplicationDetailsDLL(Empid, PolicyNumber);
        }


        #region OD Claim Workfow
        // OD Claim Workfow
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForCWVerificationBLL(long EmpId, string Category)
        {
            return _IMBClaimsDLL.GetEmployeeDetailsForCWVerificationDLL(EmpId, Category);
        }
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForSuperintendentVerificationBLL(long EmpId, string Category)
        {
            return _IMBClaimsDLL.GetEmployeeDetailsForSuperintendentVerificationDLL(EmpId, Category);
        }
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForADVerificationBLL(long EmpId, string Category)
        {
            return _IMBClaimsDLL.GetEmployeeDetailsForADVerificationDLL(EmpId, Category);
        }
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForDDVerificationBLL(long EmpId, string Category)
        {
            return _IMBClaimsDLL.GetEmployeeDetailsForDDVerificationDLL(EmpId, Category);
        }
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForDVerificationBLL(long EmpId, string Category)
        {
            return _IMBClaimsDLL.GetEmployeeDetailsForDVerificationDLL(EmpId, Category);
        }

        public VM_MIODClaimDeptVerficationDetails GetWorkFlowDetailsBLL(long applicationId, int category)
        {
            return _IMBClaimsDLL.GetWorkFlowDetailsDLL(applicationId, category);
        }
        public string SaveVerifiedDetailsBLL(VM_MIODClaimDeptVerficationDetails objVerifyDetails)
        {
            return _IMBClaimsDLL.SaveVerifiedDetailsDLL(objVerifyDetails);
        }
        #endregion

        // Surveyor Workflow
        public VM_ODClaimSurveyorVerificationDetails GetEmployeeDetailsForSurveyorVerificationBLL(long EmpId)
        {
            return _IMBClaimsDLL.GetEmployeeDetailsForSurveyorVerificationDLL(EmpId);
        }

        //Aproved Application Work Order View
        public VM_ODClaimApprovedApplicationDetails GetApprovedApplicationListBLL(long EmpID, string Category)
        {
            return _IMBClaimsDLL.GetApprovedApplicationListDLL(EmpID,Category);
        }
        public VM_ODClaimWorkOrderDetails GetODClaimAprvdAppDetailsBLL(long Empid, string PolicyNumber, string Category)
        {
            return _IMBClaimsDLL.GetODClaimAprvdAppDetailsDLL(Empid, PolicyNumber,Category);
        }
    }
}
