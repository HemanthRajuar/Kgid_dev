using Common;
using DLL.DBConnection;
using KGID_Models.Dashboard;
using KGID_Models.KGID_Master;
using KGID_Models.KGID_User;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDEmployee;
//using KGID_Models.KGIDLoan;
using KGID_Models.KGIDNBApplication;
using KGID_Models.NBApplication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using KGID_Models.KGID_GroupInsurance;
using KGID_Models.KGID_Policy;

namespace DLL.GISNomineeChangeReqDLL
{
    public class GISNomineeChangeReqDLL : IGISNomineeChangeReqDLL
    {

        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();
        private readonly AllCommon _commnobj = new AllCommon();


        //TO CHECK IF Nominee Change request APPLICATION IS APPROVED OR SENT BACK TO EMPLOYEE
        public async Task<long> GetGIS_NCRApplicationStatus(long empid)
        {
            long applicationstatus = 0; ;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {

                tbl_GIS_application_details appDetails = db.tbl_GIS_application_details.AsNoTracking().Where(g => g.gad_employee_id == empid && g.gad_active == 1).FirstOrDefault();

                if (appDetails != null)
                {
                    try
                    {
                        tbl_GIS_NomineeReq_workflow_details workflow = db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(g => g.gnwt_application_id == appDetails.gad_application_id && g.gnwt_active_status == 1).FirstOrDefault();
                        if (workflow != null)
                        {
                            applicationstatus = workflow.gnwt_nr_application_status;
                        }
                    }
                    catch (Exception ex)
                    {
                        //dbContextTransaction.Rollback();
                        return 0;
                    }
                }
            }
            return applicationstatus;
        }



        public async Task<long> GIS_NR_SaveNominee(VM_NomineeDetail objNominee)
        {
            long gndapplicationid = 0;
            long result = 1;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                var objAppD = await db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_employee_id == (long)objNominee.EmpId && e.gad_active == 1).FirstOrDefaultAsync();
                if (objAppD != null)
                    gndapplicationid = objAppD.gad_application_id;

                if (objNominee.Id != 0)
                {
                    try
                    {
                        tbl_GIS_nominee_details editNom = await _db.tbl_GIS_nominee_details.AsNoTracking().Where(e => (long)e.gnd_nominee_id == (long)objNominee.Id).FirstOrDefaultAsync();


                        //editNom.gnd_relation_id = int.Parse(objNominee.Relation);
                        //editNom.gnd_name_of_nominee = objNominee.NameOfNominee;
                        //editNom.gnd_percentage_of_share = (int)objNominee.PercentageShare;
                        //editNom.gnd_updation_datetime = DateTime.Now;
                        //editNom.gnd_updated_by = (long)objNominee.EmpId;
                        //editNom.gnd_Nominee_age = objNominee.Age;
                        //editNom.gnd_nomineeDob = objNominee.nomineeDOB;
                        //editNom.gnd_contingencies = objNominee.gnd_contingencies;
                        //editNom.gnd_predeceasing = objNominee.gnd_predeceasing;
                        ////editNom.gnd_AdoptionFile_path = objNominee.SonDaughterAdoption_doc_path != null ? objNominee.SonDaughterAdoption_doc_path : null;
                        //if (objNominee.SonDaughterAdoption_doc_path != null)
                        //{ editNom.gnd_AdoptionFile_path = objNominee.SonDaughterAdoption_doc_path; }
                        ////editNom.State = EntityState.Modified;
                        //_db.Entry(editNom).State = EntityState.Modified;
                        //await _db.SaveChangesAsync();

                        //tbl_GIS_nominee_details editNom = await _db.tbl_GIS_nominee_details.AsNoTracking().Where(e => (long)e.gnd_nominee_id == (long)objNominee.Id).FirstOrDefaultAsync();
                        editNom.gnd_relation_id = int.Parse(objNominee.Relation);
                        editNom.gnd_name_of_nominee = objNominee.NameOfNominee;
                        editNom.gnd_percentage_of_share = (int)objNominee.PercentageShare;
                        editNom.gnd_updation_datetime = DateTime.Now;
                        editNom.gnd_updated_by = (long)objNominee.EmpId;
                        editNom.gnd_Nominee_age = objNominee.Age != null ? objNominee.Age : 0;
                        if (objNominee.Relation != "14") { editNom.gnd_nomineeDob = objNominee.nomineeDOB; }

                        editNom.gnd_contingencies = objNominee.gnd_contingencies;
                        editNom.gnd_predeceasing = objNominee.gnd_predeceasing;
                        if (objNominee.SonDaughterAdoption_doc_path != null || objNominee.SonDaughterAdoption_doc_path != "")
                        { editNom.gnd_AdoptionFile_path = objNominee.SonDaughterAdoption_doc_path; }
                        _db.Entry(editNom).State = EntityState.Modified;
                        await _db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        //dbContextTransaction.Rollback();
                        result = 0;
                    }
                }
                else
                {
                    try
                    {
                        //tbl_GIS_nominee_details objBD = new tbl_GIS_nominee_details();
                        //objBD.gnd_emp_id = (long)objNominee.EmpId;
                        //objBD.gnd_application_id = gndapplicationid;
                        //objBD.gnd_relation_id = int.Parse(objNominee.Relation);
                        //objBD.gnd_name_of_nominee = objNominee.NameOfNominee;
                        //objBD.gnd_percentage_of_share = (int)objNominee.PercentageShare;
                        //objBD.gnd_active = 1;
                        //objBD.gnd_creation_datetime = DateTime.Now;
                        //objBD.gnd_created_by = (long)objNominee.EmpId;
                        //objBD.gnd_updation_datetime = DateTime.Now;
                        //objBD.gnd_updated_by = (long)objNominee.EmpId;
                        //objBD.gnd_Nominee_age = objNominee.Age;
                        //objBD.gnd_nomineeDob = objNominee.nomineeDOB;
                        //objBD.gnd_contingencies = objNominee.gnd_contingencies;
                        //objBD.gnd_predeceasing = objNominee.gnd_predeceasing;
                        ////editNom.gnd_AdoptionFile_path = objNominee.SonDaughterAdoption_doc_path != null ? objNominee.SonDaughterAdoption_doc_path : null;
                        //if (objNominee.SonDaughterAdoption_doc_path != null)
                        //{ objBD.gnd_AdoptionFile_path = objNominee.SonDaughterAdoption_doc_path; }
                        //_db.tbl_GIS_nominee_details.Add(objBD);
                        //await _db.SaveChangesAsync();

                        tbl_GIS_nominee_details objBD = new tbl_GIS_nominee_details();
                        objBD.gnd_emp_id = (long)objNominee.EmpId;
                        objBD.gnd_application_id = gndapplicationid;
                        objBD.gnd_relation_id = int.Parse(objNominee.Relation);
                        objBD.gnd_name_of_nominee = objNominee.NameOfNominee;
                        objBD.gnd_percentage_of_share = objNominee.PercentageShare != null ? (int)objNominee.PercentageShare : 0;
                        objBD.gnd_active = 1;
                        objBD.gnd_creation_datetime = DateTime.Now;
                        objBD.gnd_created_by = (long)objNominee.EmpId;
                        objBD.gnd_updation_datetime = DateTime.Now;
                        objBD.gnd_updated_by = (long)objNominee.EmpId;
                        objBD.gnd_Nominee_age = objNominee.Age;
                        if (objNominee.Relation != "14") { objBD.gnd_nomineeDob = objNominee.nomineeDOB; }

                        objBD.gnd_contingencies = objNominee.gnd_contingencies != null ? objNominee.gnd_contingencies : null;
                        objBD.gnd_predeceasing = objNominee.gnd_predeceasing != null ? objNominee.gnd_predeceasing : null;

                        if (objNominee.SonDaughterAdoption_doc_path != null || objNominee.SonDaughterAdoption_doc_path != "")
                        { objBD.gnd_AdoptionFile_path = objNominee.SonDaughterAdoption_doc_path; }

                        _db.tbl_GIS_nominee_details.Add(objBD);
                        await _db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        //dbContextTransaction.Rollback();
                        result = 0;
                    }
                }
            }

            return result;
        }

        // insert into nominee change request (workflow table) -- insert  submitted by Applicant
        //public async Task<bool> GISSaveDeclaration(long EmpId, long AppId)
        //{
        //    long gndapplicationid = 0;
        //    bool result = false;
        //    using (DbConnectionKGID db = new DbConnectionKGID())
        //    {
        //        //var objAppD = db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_employee_id == (long)EmpId && e.gad_active == 1).FirstOrDefault();
        //        //if (objAppD != null)
        //        //    gndapplicationid = objAppD.gad_application_id;
        //        //var objChallanD = db.tbl_GIS_challan_details.AsNoTracking().Where(e => e.gcd_application_id == (long)gndapplicationid && e.gcd_active_status == 1).FirstOrDefault();
        //        var objBD = db.tbl_employee_basic_details.AsNoTracking().Where(e => e.employee_id == (long)EmpId && e.active_status == true).FirstOrDefault();

        //        try
        //        {
        //            tbl_GIS_NomineeReq_workflow_details objGnrwd = new tbl_GIS_NomineeReq_workflow_details();
        //            objGnrwd.gnwt_application_id = AppId;//gndapplicationid;
        //            objGnrwd.gnwt_verified_by = EmpId;
        //             objGnrwd.gnwt_assigned_to = (long)objBD.created_by;
        //            //objGnrwd.gnwt_remarks = "";
        //            //objGnrwd.gnwt_comments ="";
        //            objGnrwd.gnwt_nr_application_status =3;
        //            objGnrwd.gnwt_active_status = 1;
        //            objGnrwd.gnwt_created_by = EmpId;
        //            objGnrwd.gnwt_creation_datetime = DateTime.UtcNow;
        //            objGnrwd.gnwt_updated_by = EmpId;
        //            objGnrwd.gnwt_updation_datetime = DateTime.UtcNow;
        //            _db.tbl_GIS_NomineeReq_workflow_details.Add(objGnrwd);
        //            await _db.SaveChangesAsync();
        //            result = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            //dbContextTransaction.Rollback();
        //            return false;
        //        }
        //    }

        //    //return result;
        //    return result;
        //}
        public async Task<int> GISSaveDeclaration(long EmpId, long AppId)
        {
            long gndapplicationid = 0;
            int result = 0;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                var objAppD = db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_employee_id == (long)EmpId && e.gad_active == 1).FirstOrDefault();
                if (objAppD != null)
                    gndapplicationid = objAppD.gad_application_id;
                var objBD = db.tbl_employee_basic_details.AsNoTracking().Where(e => e.employee_id == (long)EmpId && e.active_status == true).FirstOrDefault();
                //code to check if ddo is available or not
                var ddoid = (from n in _db.tbl_employee_work_details
                             join _KGIDD in _db.tbl_employee_basic_details on n.ewd_emp_id equals _KGIDD.employee_id
                             where (n.ewd_emp_id == EmpId)
                             select n).FirstOrDefault();

                var ddocode = (from n in _db.tbl_employee_work_details
                               join _KGIDD in _db.tbl_employee_basic_details on n.ewd_emp_id equals _KGIDD.employee_id
                               where (n.ewd_ddo_id == ddoid.ewd_ddo_id && _KGIDD.user_category_id.Contains("2"))
                               select n).FirstOrDefault();

                if (ddocode != null)
                {
                    try
                    {
                        tbl_GIS_NomineeReq_workflow_details previousworkflowId = await _db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_application_id == AppId && e.gnwt_active_status == 1).FirstOrDefaultAsync();
                        if (previousworkflowId != null)
                        {
                            previousworkflowId.gnwt_active_status = 0;
                            previousworkflowId.gnwt_updated_by = EmpId;
                            previousworkflowId.gnwt_updation_datetime = DateTime.UtcNow;
                            db.Entry(previousworkflowId).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            result = 2;
                        }
                    }
                    catch (Exception ex)
                    {
                        //dbContextTransaction.Rollback();
                        result = 1;
                    }


                    try
                    {
                        //    tbl_GIS_NomineeReq_workflow_details objGnrwd = new tbl_GIS_NomineeReq_workflow_details();
                        //    objGnrwd.gnwt_application_id = AppId;//gndapplicationid;
                        //    objGnrwd.gnwt_verified_by = EmpId;
                        //    objGnrwd.gnwt_assigned_to = (long)objBD.created_by;
                        //    //objGnrwd.gnwt_remarks = "";
                        //    //objGnrwd.gnwt_comments ="";
                        //    objGnrwd.gnwt_nr_application_status = 3;
                        //    objGnrwd.gnwt_active_status = 1;
                        //    objGnrwd.gnwt_created_by = EmpId;
                        //    objGnrwd.gnwt_creation_datetime = DateTime.UtcNow;
                        //    objGnrwd.gnwt_updated_by = EmpId;
                        //    objGnrwd.gnwt_updation_datetime = DateTime.UtcNow;
                        //    _db.tbl_GIS_NomineeReq_workflow_details.Add(objGnrwd);
                        //    await _db.SaveChangesAsync();
                        //    result = true;
                        // long applstatus = 3;
                        tbl_GIS_NomineeReq_workflow_details objGad = new tbl_GIS_NomineeReq_workflow_details();
                        objGad.gnwt_application_id = gndapplicationid;
                        //objGad.gawt_verified_by =;
                        objGad.gnwt_assigned_to = (long)objBD.created_by;
                        //objGad.gawt_remarks ="";
                        //objGad.gawt_comments ="";
                        objGad.gnwt_nr_application_status = 3;
                        objGad.gnwt_active_status = 1;

                        objGad.gnwt_created_by = EmpId;
                        objGad.gnwt_creation_datetime = DateTime.UtcNow;
                        objGad.gnwt_updated_by = EmpId;
                        objGad.gnwt_updation_datetime = DateTime.UtcNow;
                        _db.tbl_GIS_NomineeReq_workflow_details.Add(objGad);
                        _db.SaveChanges();
                        result = 2;
                    }
                    catch (Exception ex)
                    {
                        //dbContextTransaction.Rollback();
                        result = 1;
                    }
                }
                else { result = 3; }
                // ddo not mapped
            }
            //return result;
            return result;  //result 2 is saved successfully result 1 is error
        }

        //insert into nominee change request (workflow table) -- after verification by ddo
        public async Task<int> GIS_NR_UpdateWorkFlow(VM_GISDeptVerificationDetails objEmpForm)
        {

            long gndapplicationid = 0;
            int result = 0;
            long empid = objEmpForm.EmpCode;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                tbl_GIS_NomineeReq_workflow_details previousworkflowId = await _db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_application_id == objEmpForm.EmpCode && e.gnwt_active_status == 1).FirstOrDefaultAsync();
                if (previousworkflowId != null)
                {
                    previousworkflowId.gnwt_active_status = 0;
                    previousworkflowId.gnwt_updated_by = objEmpForm.EmpCode;
                    previousworkflowId.gnwt_updation_datetime = DateTime.UtcNow;
                    db.Entry(previousworkflowId).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                if (previousworkflowId == null)
                {
                    try
                    {
                        var objBD = db.tbl_employee_basic_details.AsNoTracking().Where(e => e.employee_id == (long)empid && e.active_status == true).FirstOrDefault();


                        tbl_GIS_NomineeReq_workflow_details objGnrwd = new tbl_GIS_NomineeReq_workflow_details();
                        objGnrwd.gnwt_application_id = gndapplicationid;
                        objGnrwd.gnwt_verified_by = (long)objBD.created_by;
                        objGnrwd.gnwt_assigned_to = 0;
                        objGnrwd.gnwt_remarks = objEmpForm.Remarks;
                        objGnrwd.gnwt_comments = objEmpForm.Comments;
                        objGnrwd.gnwt_nr_application_status = objEmpForm.ApplicationStatus;
                        objGnrwd.gnwt_active_status = 1;
                        objGnrwd.gnwt_created_by = objEmpForm.EmpCode;
                        objGnrwd.gnwt_creation_datetime = DateTime.UtcNow;
                        objGnrwd.gnwt_updated_by = objEmpForm.EmpCode;
                        objGnrwd.gnwt_updation_datetime = DateTime.UtcNow;
                        _db.tbl_GIS_NomineeReq_workflow_details.Add(objGnrwd);
                        _db.SaveChanges();
                        result = 1;
                    }
                    catch (Exception ex)
                    {
                        //dbContextTransaction.Rollback();
                        result = 0;
                    }
                }

                //else
                //{
                //    try
                //    {


                //        //previousworkflowId.gnwt_application_id = gndapplicationid;
                //        //  objGnrwd.gnwt_verified_by = objChallanD.gcd_insurance_amount;
                //        previousworkflowId.gnwt_assigned_to = 0;
                //        previousworkflowId.gnwt_remarks = objEmpForm.re;
                //        previousworkflowId.gnwt_comments = "";
                //        previousworkflowId.gnwt_nr_application_status = 1;
                //        previousworkflowId.gnwt_active_status = 1;
                //        previousworkflowId.gnwt_created_by = 0;
                //        previousworkflowId.gnwt_creation_datetime = DateTime.UtcNow;
                //        previousworkflowId.gnwt_updated_by = 0;
                //        previousworkflowId.gnwt_updation_datetime = DateTime.UtcNow;
                //        db.Entry(previousworkflowId).State = EntityState.Modified;
                //        db.SaveChanges();
                //        result =1;
                //    }
                //    catch (Exception ex)
                //    {
                //        //dbContextTransaction.Rollback();
                //        result = 0;
                //    }
                //}



            }
            return result;
        }

        //upload document form6/form7  path
        public async Task<int> GIS_NR_UploadForms(tbl_GIS_NR_upload_form objEmpForm)//To Be Changed
        {

            int Result = 0;
            var EmpRefNo = (from n in _db.tbl_GIS_application_details
                            where n.gad_employee_id == objEmpForm.App_Employee_Code
                            && n.gad_active == 1
                            select n).FirstOrDefault();
            var EmpUpload = (from n in _db.tbl_GIS_NR_upload_form
                             where n.App_Employee_Code == objEmpForm.App_Employee_Code
                             && n.App_ApplicationID == EmpRefNo.gad_application_id
                             && n.App_Active == 1
                             select n).FirstOrDefault();

            try
            {



                if (EmpRefNo != null)
                {


                    if (EmpUpload != null)
                    {
                        EmpUpload.App_Active = 0; // -- 23 ADDED
                        EmpUpload.App_Creation_Date = DateTime.Now;
                        _db.Entry(EmpUpload).State = EntityState.Modified;
                        _db.SaveChanges();

                        EmpUpload.App_Employee_Code = objEmpForm.App_Employee_Code;
                        //if (EmpUpload.App_Application_Form != string.Empty && objEmpForm.App_Application_Form == string.Empty)
                        //{
                        //    objEmpForm.App_Application_Form = EmpUpload.App_Application_Form;
                        //}
                        if (EmpUpload.App_Form6 != string.Empty && objEmpForm.App_Form6 == string.Empty)
                        {
                            objEmpForm.App_Form6 = EmpUpload.App_Form6;
                        }
                        else if (EmpUpload.App_Form7 != string.Empty && objEmpForm.App_Form7 == string.Empty)
                        {
                            objEmpForm.App_Form7 = EmpUpload.App_Form7;
                        }
                        // EmpUpload.App_Application_Form = objEmpForm.App_Application_Form;
                        //EmpUpload.App_Form6 = objEmpForm.App_Form6;
                        //EmpUpload.App_Form7 = objEmpForm.App_Form7;
                        //EmpUpload.App_Creation_Date = DateTime.Now;
                        //_db.Entry(EmpUpload).State = EntityState.Modified;
                        //_db.SaveChanges();


                        objEmpForm.App_Active = 1;
                        objEmpForm.App_Created_By = Convert.ToInt64(objEmpForm.App_Employee_Code);
                        objEmpForm.App_Creation_Date = DateTime.Now;
                        objEmpForm.App_ApplicationID = EmpRefNo.gad_application_id;
                        _db.tbl_GIS_NR_upload_form.Add(objEmpForm);
                        _db.SaveChanges();

                        Result = 2; //New record saved 
                    }
                    else
                    {
                        var EmpInsuredUpload = (from n in _db.tbl_GIS_NR_upload_form
                                                where n.App_Employee_Code == objEmpForm.App_Employee_Code
                                                && n.App_ApplicationID != EmpRefNo.gad_application_id
                                                && n.App_Active == 1
                                                select n).FirstOrDefault();
                        if (EmpInsuredUpload != null)
                        {
                            EmpInsuredUpload.App_Active = 0;
                            _db.SaveChanges();
                        }

                        objEmpForm.App_Active = 1;
                        objEmpForm.App_Created_By = Convert.ToInt64(objEmpForm.App_Employee_Code);
                        objEmpForm.App_Creation_Date = DateTime.Now;
                        objEmpForm.App_ApplicationID = EmpRefNo.gad_application_id;
                        _db.tbl_GIS_NR_upload_form.Add(objEmpForm);
                        _db.SaveChanges();

                        Result = 1;
                    }
                }



                else Result = 0;
            }
            catch (Exception ex)
            { Logger.LogMessage(TracingLevel.INFO, "GISNominee changeSaveEmployeeForm" + ex.Message); }

            return Result;
        }

        public VM_GIS_Upload_EmployeeForm GetUploadDoc(long _EmpId) // To Be Changed
        {
            var EmpUpload = (from n in _db.tbl_GIS_NR_upload_form
                             where n.App_Employee_Code == _EmpId && n.App_Active == 1
                             select n).FirstOrDefault();
            var EmpApplId = (from n in _db.tbl_GIS_application_details
                             where n.gad_employee_id == _EmpId
                             select n).FirstOrDefault();
            VM_GIS_Upload_EmployeeForm objUD = new VM_GIS_Upload_EmployeeForm();
            if (EmpUpload != null)
            {
                objUD.App_Employee_Code = EmpUpload.App_Employee_Code;
                objUD.App_ApplicationID = EmpApplId.gad_application_id != null ? EmpApplId.gad_application_id : 0;
                //objUD.ApplicationFormDocName = EmpUpload.App_Application_Form;
                objUD.Form6DocName = EmpUpload.App_Form6;
                objUD.Form7DocName = EmpUpload.App_Form7;
            }
            else
            {
                objUD.App_Employee_Code = _EmpId;
                objUD.App_ApplicationID = EmpApplId.gad_application_id != null ? EmpApplId.gad_application_id : 0;
            }
            return objUD;
        }

        public async Task<VM_GISDDOVerificationDetails> GetEmployeeNomineeDetailsForDDOVerification(long empId)
        {
            VM_GISDDOVerificationDetails verificationDetails = new VM_GISDDOVerificationDetails();
            try
            {
                using (DbConnectionKGID db = new DbConnectionKGID())
                {
                    var empworkdetails = await db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_emp_id == empId && e.ewd_active_status == true).FirstOrDefaultAsync();
                    int ddoid = empworkdetails.ewd_ddo_id;
                    //NEW EMPLOPYEE REQUEST FOR NOMINEE CHANGE
                    var EmployeeVerification =
                  await (from workFlow in db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_active_status == 1 && (e.gnwt_nr_application_status != 15))
                         join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on workFlow.gnwt_application_id equals application.gad_application_id
                         join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on application.gad_employee_id equals ebd.employee_id
                         join ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid && e.ewd_active_status == true) on ebd.employee_id equals ewd.ewd_emp_id
                         join deptmaster in db.tbl_department_master.AsNoTracking().Where(e => e.dm_active == true) on ebd.dept_employee_code equals deptmaster.dm_dept_id
                         join ddomaster in db.tbl_ddo_master.AsNoTracking().Where(e => e.dm_active == true) on ewd.ewd_ddo_id equals ddomaster.dm_ddo_id
                         join ddodiomaster in db.tbl_ddo_dio_master.AsNoTracking().Where(e => e.dd_status == true) on ddomaster.dm_district_id equals ddodiomaster.dd_district_id
                         join distmaster in db.tbl_district_master.AsNoTracking().Where(e => e.dm_status == true) on ddodiomaster.dd_district_id equals distmaster.dm_id


                         //await   (from ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid && e.ewd_active_status == true)
                         //   join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on ewd.ewd_emp_id equals ebd.employee_id
                         //   join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on ebd.employee_id equals application.gad_employee_id
                         //   join workFlow in db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_active_status== 1 && (e.gnwt_nr_application_status != 15)) on application.gad_application_id equals workFlow.gnwt_application_id
                         //   // join workFlow in db.tbl_GIS_application_workflow_details.AsNoTracking().Where(e => e.gawt_active_status == 1 && (e.gawt_application_status == 3 || e.gawt_application_status == 4)) on application.gad_application_id equals workFlow.gawt_application_id

                         //          join empType in db.tbl_employment_type_master.AsNoTracking().Where(a => a.et_active == true) on empWork.ewd_employment_type equals empType.et_employee_type_id
                         //            join empOther1 in db.tbl_employee_other_details.AsNoTracking().Where(a => a.eod_active_status == true) on empBasic.employee_id equals empOther1.eod_emp_id into set1
                         //            from empOther in set1.DefaultIfEmpty()
                         //            join empadd1 in db.tbl_employee_address_details.AsNoTracking().Where(a => a.ead_active_status == true) on empBasic.employee_id equals empadd1.ead_emp_id into set2
                         //            from empadd in set2.DefaultIfEmpty()
                         select new GISEmployeeDDOVerificationDetail
                         {

                             EmployeeCode = ebd.employee_id,
                             Name = ebd.employee_name,
                             ApplicationNumber = application.gad_application_no.ToString(),
                             ApplicationId = application.gad_application_id,
                             Status = workFlow.gnwt_nr_application_status == 3 ? "Pending" : "Revert",//AppStatus
                             Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                             RowNum = (int)ebd.employee_id,
                             District = distmaster.dm_name_english,
                             Department = deptmaster.dm_deptname_english
                         }).ToListAsync();

                    var LastUpdatedStatus = await (from ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid && e.ewd_active_status == true)
                                                   join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on ewd.ewd_emp_id equals ebd.employee_id
                                                   join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on ebd.employee_id equals application.gad_employee_id
                                                   join workFlow in db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_active_status == 1 && e.gnwt_nr_application_status != 15) on application.gad_application_id equals workFlow.gnwt_application_id
                                                   select new GISEmployeeDDOVerificationDetail
                                                   {

                                                       EmployeeCode = ebd.employee_id,
                                                       Name = ebd.employee_name,
                                                       ApplicationNumber = application.gad_application_no.ToString(),
                                                       ApplicationId = application.gad_application_id,
                                                       Status = workFlow.gnwt_application_id == 3 ? "Pending" : "Revert",//AppStatus
                                                       Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                                                       District = "",
                                                       Department = ""
                                                   }).ToListAsync();
                    var IEmployeeVerification =
                       await (from ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid && e.ewd_active_status == true)
                              join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true & e.first_kgid_policy_no != null) on ewd.ewd_emp_id equals ebd.employee_id
                              join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on ebd.employee_id equals application.gad_employee_id
                              join workFlow in db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_active_status == 1 && (e.gnwt_nr_application_status == 3 || e.gnwt_nr_application_status == 4)) on application.gad_application_id equals workFlow.gnwt_application_id

                              select new GISEmployeeDDOVerificationDetail
                              {

                                  EmployeeCode = ebd.employee_id,
                                  Name = ebd.employee_name,
                                  ApplicationNumber = application.gad_application_no.ToString(),
                                  ApplicationId = application.gad_application_id,
                                  Status = workFlow.gnwt_application_id == 3 ? "Pending" : "Reverted",//AppStatus
                                  Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                              }).ToListAsync();
                    //NOT yet done 
                    var ApprovedStatus = await (from ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid && e.ewd_active_status == true)
                                                join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on ewd.ewd_emp_id equals ebd.employee_id
                                                join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on ebd.employee_id equals application.gad_employee_id
                                                join workFlow in db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => (e.gnwt_nr_application_status == 15)) on application.gad_application_id equals workFlow.gnwt_application_id

                                                // join workFlow in db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_active_status == 1 && (e.gnwt_nr_application_status == 15)) on application.gad_application_id equals workFlow.gnwt_application_id
                                                // join challanDeatils in db.tbl_GIS_challan_details.AsNoTracking().Where(e => e.gcd_active_status == 1) on workFlow.gawt_application_id equals challanDeatils.gcd_application_id

                                                //          join empType in db.tbl_employment_type_master.AsNoTracking().Where(a => a.et_active == true) on empWork.ewd_employment_type equals empType.et_employee_type_id
                                                //            join empOther1 in db.tbl_employee_other_details.AsNoTracking().Where(a => a.eod_active_status == true) on empBasic.employee_id equals empOther1.eod_emp_id into set1
                                                //            from empOther in set1.DefaultIfEmpty()
                                                //            join empadd1 in db.tbl_employee_address_details.AsNoTracking().Where(a => a.ead_active_status == true) on empBasic.employee_id equals empadd1.ead_emp_id into set2
                                                //            from empadd in set2.DefaultIfEmpty()
                                                select new GISEmployeeDDOVerificationDetail
                                                {

                                                    EmployeeCode = ebd.employee_id,
                                                    Name = ebd.employee_name,
                                                    ApplicationNumber = application.gad_application_no.ToString(),
                                                    ApplicationId = application.gad_application_id,
                                                    Status = "Approved",//AppStatus
                                                                        // LastUpdatedDate = dataRow.Field<string>("kawt_creation_datetime"),

                                                    //Premium = challanDeatils.gcd_amount.ToString(),
                                                    Premium = "",
                                                }).ToListAsync();

                    verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                    verificationDetails.IEmployeeVerificationDetails = IEmployeeVerification;
                    verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                    verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;


                    var pendingapplication =
                 (from workFlow in db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_active_status == 1 && (e.gnwt_nr_application_status == 3))
                       join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on workFlow.gnwt_application_id equals application.gad_application_id
                       join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on application.gad_employee_id equals ebd.employee_id
                       join ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid && e.ewd_active_status == true) on ebd.employee_id equals ewd.ewd_emp_id
                       join deptmaster in db.tbl_department_master.AsNoTracking().Where(e => e.dm_active == true) on ebd.dept_employee_code equals deptmaster.dm_dept_id
                       join ddomaster in db.tbl_ddo_master.AsNoTracking().Where(e => e.dm_active == true) on ewd.ewd_ddo_id equals ddomaster.dm_ddo_id
                       join ddodiomaster in db.tbl_ddo_dio_master.AsNoTracking().Where(e => e.dd_status == true) on ddomaster.dm_district_id equals ddodiomaster.dd_district_id
                       join distmaster in db.tbl_district_master.AsNoTracking().Where(e => e.dm_status == true) on ddodiomaster.dd_district_id equals distmaster.dm_id
                       select new GISEmployeeDDOVerificationDetail
                       {

                           EmployeeCode = ebd.employee_id,
                           Name = ebd.employee_name,
                           ApplicationNumber = application.gad_application_no.ToString(),
                           ApplicationId = application.gad_application_id,
                           Status = workFlow.gnwt_nr_application_status == 3 ? "Pending" : "Revert",//AppStatus
                             Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                           RowNum = (int)ebd.employee_id,
                           District = distmaster.dm_name_english,
                           Department = deptmaster.dm_deptname_english
                       }).ToList().Count;

                    var TotalApplication =
                (from workFlow in db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_active_status == 1 )
                 join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on workFlow.gnwt_application_id equals application.gad_application_id
                 join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on application.gad_employee_id equals ebd.employee_id
                 join ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid && e.ewd_active_status == true) on ebd.employee_id equals ewd.ewd_emp_id
                 join deptmaster in db.tbl_department_master.AsNoTracking().Where(e => e.dm_active == true) on ebd.dept_employee_code equals deptmaster.dm_dept_id
                 join ddomaster in db.tbl_ddo_master.AsNoTracking().Where(e => e.dm_active == true) on ewd.ewd_ddo_id equals ddomaster.dm_ddo_id
                 join ddodiomaster in db.tbl_ddo_dio_master.AsNoTracking().Where(e => e.dd_status == true) on ddomaster.dm_district_id equals ddodiomaster.dd_district_id
                 join distmaster in db.tbl_district_master.AsNoTracking().Where(e => e.dm_status == true) on ddodiomaster.dd_district_id equals distmaster.dm_id
                 select new GISEmployeeDDOVerificationDetail
                 {

                     EmployeeCode = ebd.employee_id,
                     Name = ebd.employee_name,
                     ApplicationNumber = application.gad_application_no.ToString(),
                     ApplicationId = application.gad_application_id,
                     Status = workFlow.gnwt_nr_application_status == 3 ? "Pending" : "Revert",//AppStatus
                      Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                     RowNum = (int)ebd.employee_id,
                     District = distmaster.dm_name_english,
                     Department = deptmaster.dm_deptname_english
                 }).ToList().Count;

                    var SentBackApplication =
               (from workFlow in db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_active_status == 1 && (e.gnwt_nr_application_status == 2))
                join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on workFlow.gnwt_application_id equals application.gad_application_id
                join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on application.gad_employee_id equals ebd.employee_id
                join ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid && e.ewd_active_status == true) on ebd.employee_id equals ewd.ewd_emp_id
                join deptmaster in db.tbl_department_master.AsNoTracking().Where(e => e.dm_active == true) on ebd.dept_employee_code equals deptmaster.dm_dept_id
                join ddomaster in db.tbl_ddo_master.AsNoTracking().Where(e => e.dm_active == true) on ewd.ewd_ddo_id equals ddomaster.dm_ddo_id
                join ddodiomaster in db.tbl_ddo_dio_master.AsNoTracking().Where(e => e.dd_status == true) on ddomaster.dm_district_id equals ddodiomaster.dd_district_id
                join distmaster in db.tbl_district_master.AsNoTracking().Where(e => e.dm_status == true) on ddodiomaster.dd_district_id equals distmaster.dm_id
                select new GISEmployeeDDOVerificationDetail
                {

                    EmployeeCode = ebd.employee_id,
                    Name = ebd.employee_name,
                    ApplicationNumber = application.gad_application_no.ToString(),
                    ApplicationId = application.gad_application_id,
                    Status = workFlow.gnwt_nr_application_status == 3 ? "Pending" : "Revert",//AppStatus
                     Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                    RowNum = (int)ebd.employee_id,
                    District = distmaster.dm_name_english,
                    Department = deptmaster.dm_deptname_english
                }).ToList().Count;


                    verificationDetails.TotalReceived = TotalApplication;
                    verificationDetails.SentBackApplication = SentBackApplication;
                   // verificationDetails.ForwardedApplications = 0;
                    verificationDetails.PendingApplications = pendingapplication;

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public IList<VM_GISWorkflowDetail> GetWorkFlowDetails(long applicationId)
        {
            IList<VM_GISWorkflowDetail> workflowDetails = null;


            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                return (from appl in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1 && e.gad_application_id == applicationId)
                        join empBasic in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on appl.gad_employee_id equals empBasic.employee_id
                        join empWork in db.tbl_employee_work_details.AsNoTracking().Where(v => v.ewd_active_status == true) on empBasic.employee_id equals empWork.ewd_emp_id
                        join groupMaster in db.tbl_employee_group_master.AsNoTracking().Where(s => s.eg_active == true) on empWork.ewd_group_id equals groupMaster.eg_group_id
                        join payscaleMaster in db.tbl_payscales_master.AsNoTracking().Where(v => v.payscale_status == 1) on empWork.ewd_payscale_id equals payscaleMaster.payscale_id// into su
                                                                                                                                                                                     //from Pab in su.DefaultIfEmpty()
                        join designationmaster in db.tbl_designation_master.AsNoTracking().Where(a => a.d_status == 1) on empWork.ewd_designation_id equals designationmaster.d_designation_id
                        join ddoMaster in db.tbl_ddo_master.AsNoTracking().Where(a => a.dm_active == true) on empWork.ewd_ddo_id equals ddoMaster.dm_ddo_id
                        join genderMster in db.tbl_gender_master.AsNoTracking().Where(a => a.active_status == true) on empBasic.gender_id equals genderMster.gender_id
                        join empType in db.tbl_employment_type_master.AsNoTracking().Where(a => a.et_active == true) on empWork.ewd_employment_type equals empType.et_employee_type_id
                        join empOther1 in db.tbl_employee_other_details.AsNoTracking().Where(a => a.eod_active_status == true) on empBasic.employee_id equals empOther1.eod_emp_id into set1
                        from empOther in set1.DefaultIfEmpty()
                        join empadd1 in db.tbl_employee_address_details.AsNoTracking().Where(a => a.ead_active_status == true) on empBasic.employee_id equals empadd1.ead_emp_id into set2
                        from empadd in set2.DefaultIfEmpty()
                        join workflow1 in db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(a => a.gnwt_active_status == 1 || a.gnwt_active_status == 0) on appl.gad_application_id equals workflow1.gnwt_application_id into set3
                        from workflow in set3.DefaultIfEmpty()
                        join applStatusMaster1 in db.tbl_application_status_master.AsNoTracking().Where(a => a.asm_active == 1) on workflow.gnwt_nr_application_status equals applStatusMaster1.asm_status_id into set4
                        from applStatusMaster in set4.DefaultIfEmpty()
                        join remarks1 in db.tbl_remarks_master.AsNoTracking().Where(a => a.RM_Active_Status == true) on workflow.gnwt_remarks equals remarks1.RM_Remarks_id.ToString() into set5
                        from remarks in set5.DefaultIfEmpty()
                            //join workflow1 in db.tbl_GIS_application_workflow_details.AsNoTracking().Where(a => a.gawt_active_status == 1 || a.gawt_active_status == 0) on appl.gad_application_id equals workflow1.gawt_application_id into set3
                            //from workflow in set3.DefaultIfEmpty()
                        select new VM_GISWorkflowDetail
                        {

                            ApplicationRefNo = appl.gad_application_no.ToString(),//dr["ApplicationRefNo"].ToString();
                            //From = workflow.gawt_application_status == 4 ? "DDO" : workflow.gawt_application_status == 2 ? "Applicant" : "Approved",//dr["From"].ToString();
                            //To = workflow.gawt_application_status == 3 ? "DDO" : workflow.gawt_application_status == 2 ? "Applicant" : "Approved",//dr["To"].ToString();
                            From = workflow.gnwt_nr_application_status == 15  || workflow.gnwt_nr_application_status == 2 ? "DDO" : "Applicant",//dr["From"].ToString();
                            To = workflow.gnwt_nr_application_status == 3 || workflow.gnwt_nr_application_status == 4 ? "DDO" : "Applicant",//dr["To"].ToString();


                            Remarks = remarks.RM_Remarks_Desc,//dr["Remarks"].ToString();
                            Comments = workflow.gnwt_comments,//dr["Comments"].ToString();
                            CreationDateTime = workflow.gnwt_updation_datetime,//dr["CreationDateTime"].ToString();
                                                                               // ApplicationStatus = workflow.gnwt_nr_application_status.ToString(),//dr["ApplicationStatus"].ToString();
                            ApplicationStatus = applStatusMaster.asm_status_desc,//dr["ApplicationStatus"].ToString();

                            orderid = workflow.gnwt_workflow_id,


                        }).OrderByDescending(s => s.orderid).ToList();    //ToList().OrderByDescending(workflow.gnwt_workflow_id);
              


                //   return null;
            }

            //  return workflowDetails.OrderByDescending(t => t.CreationDateTime).ToList();

            return null;
        }

        public string GISSaveVerifiedDetails(VM_GISDeptVerificationDetails objVerification)
        {

            string returnString = string.Empty;
            long applid = objVerification.ApplicationId;

            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                //var objWFD = db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_application_id == applid && e.gnwt_active_status == 1).FirstOrDefault();
                var objappln = db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_application_id == (long)objVerification.ApplicationId && e.gad_active == 1).FirstOrDefault();

                var obj_NCR_WFD = db.tbl_GIS_NomineeReq_workflow_details.AsNoTracking().Where(e => e.gnwt_application_id == (long)objVerification.ApplicationId && e.gnwt_active_status == 1).FirstOrDefault();

                if (obj_NCR_WFD != null)
                {
                    obj_NCR_WFD.gnwt_active_status = 0;
                    db.Entry(obj_NCR_WFD).State = EntityState.Modified;
                    db.SaveChanges();
                }

                tbl_GIS_NomineeReq_workflow_details wfd = new tbl_GIS_NomineeReq_workflow_details();

                wfd.gnwt_application_id = objVerification.ApplicationId;
                //wfd.gawt_InsuranceAmt = objWFD.gawt_InsuranceAmt;
                // wfd.gawt_savingAmt = objWFD.gawt_savingAmt;
                // wfd.gawt_InsuranceDate = objWFD.gawt_InsuranceDate;
                //  wfd.gawt_savingDate = objWFD.gawt_savingDate;
                wfd.gnwt_verified_by = (long)objVerification.CreatedBy;
                wfd.gnwt_assigned_to = objVerification.ApplicationStatus == 2 ? objappln.gad_employee_id : 0;
                wfd.gnwt_remarks = objVerification.Remarks;
                wfd.gnwt_comments = objVerification.Comments;
                wfd.gnwt_nr_application_status = objVerification.ApplicationStatus;
                wfd.gnwt_active_status = 1;
                wfd.gnwt_created_by = objappln.gad_employee_id;
                wfd.gnwt_creation_datetime = DateTime.UtcNow;
                wfd.gnwt_updated_by = objappln.gad_employee_id;
                wfd.gnwt_updation_datetime = DateTime.UtcNow;
                _db.tbl_GIS_NomineeReq_workflow_details.Add(wfd);
                _db.SaveChanges();

                // check created and updated by once again -- IMPORTANT

            }
            returnString = "1";
            return returnString;


        }

        public VM_GISDeptVerificationDetails GetUploadedDocuments(long empId, long applicationId)
        {
            // VM_DeptVerificationDetails verificationDetails = new VM_DeptVerificationDetails();
            // dsUD = _Conn.ExeccuteDataset(sqlparameters, "sp_kgid_selectEmployeeDocuments");
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                try
                {
                    return (from empBasic in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true && e.employee_id == empId)
                            join forms1 in db.tbl_GIS_NR_upload_form.AsNoTracking().Where(a => a.App_Active == 1) on empBasic.employee_id equals forms1.App_Employee_Code into set5
                            from forms in set5.DefaultIfEmpty()
                            select new VM_GISDeptVerificationDetails
                            {

                                // ApplnUploaddocPath = forms.App_Application_Form,
                                // ApplnUploaddocType = "Application Form",
                                Form6UploaddocPath = forms.App_Form6,
                                Form6UploaddocType = "Form 6",
                                Form7UploaddocPath = forms.App_Form7,
                                Form7UploaddocType = "Form 7",

                            }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "GetUploadedDocuments Nominee Change Request" + ex.Message);
                    return null;
                }

            }


        }


        public async Task<long> GISSaveNBNomineeChangeRequest(VM_NomineeDetail objNominee)
        {
            long gndapplicationid = 0;
            long result = 1;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                var objAppD = await db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_employee_id == (long)objNominee.EmpId && e.gad_active == 1).FirstOrDefaultAsync();
                if (objAppD != null)
                    gndapplicationid = objAppD.gad_application_id;

                if (objNominee.Id != 0)
                {
                    try
                    {
                        tbl_GIS_nominee_details editNom = await _db.tbl_GIS_nominee_details.AsNoTracking().Where(e => (long)e.gnd_nominee_id == (long)objNominee.Id).FirstOrDefaultAsync();
                        editNom.gnd_active = 0;
                        _db.Entry(editNom).State = EntityState.Modified;
                        await _db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "GISSaveNBNominee: error in saving nominee details" + ex.Message);
                        result = 0;
                    }
                }
                //else
                //{
                    try
                    {
                        tbl_GIS_nominee_details objBD = new tbl_GIS_nominee_details();
                        objBD.gnd_emp_id = (long)objNominee.EmpId;
                        objBD.gnd_application_id = gndapplicationid;
                        objBD.gnd_relation_id = int.Parse(objNominee.Relation);
                        objBD.gnd_name_of_nominee = objNominee.NameOfNominee;
                        objBD.gnd_percentage_of_share = objNominee.PercentageShare != null ? (int)objNominee.PercentageShare : 0;
                        objBD.gnd_active = 1;
                        objBD.gnd_creation_datetime = DateTime.Now;
                        objBD.gnd_created_by = (long)objNominee.EmpId;
                        objBD.gnd_updation_datetime = DateTime.Now;
                        objBD.gnd_updated_by = (long)objNominee.EmpId;
                        objBD.gnd_Nominee_age = objNominee.Age;
                        if (objNominee.Relation != "14") { objBD.gnd_nomineeDob = objNominee.nomineeDOB; }

                        objBD.gnd_contingencies = objNominee.gnd_contingencies != null ? objNominee.gnd_contingencies : null;
                        objBD.gnd_predeceasing = objNominee.gnd_predeceasing != null ? objNominee.gnd_predeceasing : null;

                        if (objNominee.SonDaughterAdoption_doc_path != null || objNominee.SonDaughterAdoption_doc_path != "")
                        { objBD.gnd_AdoptionFile_path = objNominee.SonDaughterAdoption_doc_path; }

                        _db.tbl_GIS_nominee_details.Add(objBD);
                        await _db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "GISSaveNBNominee: error in saving new nominee details" + ex.Message);
                        result = 0;
                    }
               //}
            }

            return result;
        }
    }
}
