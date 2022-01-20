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

namespace DLL.GroupInsurance
{
    public class GroupInsuranceDLL : IGroupInsuranceDLL
    {
       
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();
        private readonly AllCommon _commnobj = new AllCommon();

        // not used--AddEmployeeBasicDetails -- ddo entring new employee data is done from NB 
        public string AddEmployeeBasicDetails(VM_BasicDetails employeeDetails)
        {
            string DuplicateDetails = "";
            try
            {

                var CheckDupMobile = (from emp in _db.tbl_employee_basic_details
                                      where ((emp.mobile_number == employeeDetails.mobile_number) && emp.employee_id != employeeDetails.employee_id)
                                      select emp).Count();
                var CheckDupEmail = (from emp in _db.tbl_employee_basic_details
                                     where ((emp.email_id == employeeDetails.email_id) && emp.employee_id != employeeDetails.employee_id)
                                     select emp).Count();
                var CheckDupPanNo = (from emp in _db.tbl_employee_basic_details
                                     where ((emp.pan_number == employeeDetails.pan_number) && emp.employee_id != employeeDetails.employee_id)
                                     select emp).Count();

                if (CheckDupMobile > 0)
                {
                    DuplicateDetails = "Mobile number,";
                }
                if (CheckDupEmail > 0)
                {
                    DuplicateDetails = "Email id" + "," + DuplicateDetails;
                }
                if (CheckDupPanNo > 0)
                {
                    DuplicateDetails = "Pan number" + "," + DuplicateDetails;
                }

                int CODE = Convert.ToInt32(employeeDetails.payscalecode);
                employeeDetails.payscale_minimum = (from ps in _db.tbl_payscales_master
                                                    where ps.payscale_status == 1 && ps.payscale_id == employeeDetails.ewd_payscale_id
                                                    select ps.payscale_minimum).FirstOrDefault();

                if (DuplicateDetails == "")
                {
                    using (var dbContextTransaction = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            tbl_employee_basic_details objBD = new tbl_employee_basic_details();
                            objBD.dept_employee_code = Convert.ToInt32(employeeDetails.dept_employee_code);
                            objBD.employee_name = employeeDetails.employee_name;
                            objBD.father_name = employeeDetails.father_name;
                            objBD.spouse_name = employeeDetails.spouse_name;

                            objBD.employee_name_kannada = employeeDetails.employee_name_kannada;
                            objBD.father_name_kannada = employeeDetails.father_name_kannada;
                            objBD.spouse_name_kannada = employeeDetails.spouse_name_kannada;

                            objBD.gender_id = employeeDetails.gender_id;
                            objBD.date_of_birth = employeeDetails.dateofbirth;
                            objBD.place_of_birth = employeeDetails.place_of_birth;
                            objBD.pan_number = employeeDetails.pan_number;
                            objBD.date_of_appointment = employeeDetails.dateofappointment;
                            objBD.mobile_number = employeeDetails.mobile_number;
                            objBD.email_id = employeeDetails.email_id;
                            objBD.user_category_id = "1";
                            objBD.active_status = true;
                            objBD.creation_datetime = DateTime.Now;
                            objBD.updation_datetime = DateTime.Now;
                            objBD.created_by = employeeDetails.created_by;
                            objBD.updated_by = employeeDetails.created_by;
                            objBD.ddo_upload_status = false;
                            _db.tbl_employee_basic_details.Add(objBD);
                            _db.SaveChanges();

                            long EMPID = (from emp in _db.tbl_employee_basic_details orderby emp.employee_id descending select emp.employee_id).First();

                            tbl_employee_work_details objEWDetails = new tbl_employee_work_details();
                            objEWDetails.ewd_emp_id = EMPID;
                            objEWDetails.ewd_place_of_posting = employeeDetails.ewd_place_of_posting;
                            objEWDetails.ewd_date_of_joining = employeeDetails.ewddateofjoining;
                            objEWDetails.ewd_payscale_id = employeeDetails.ewd_payscale_id;
                            objEWDetails.ewd_employment_type = employeeDetails.et_employee_type_id;
                            objEWDetails.ewd_group_id = employeeDetails.ewd_group_id;
                            objEWDetails.ewd_designation_id = employeeDetails.d_designation_id;
                            objEWDetails.ewd_ddo_id = employeeDetails.ewd_ddo_id;
                            objEWDetails.ewd_active_status = true;
                            objEWDetails.ewd_created_by = employeeDetails.created_by;
                            objEWDetails.ewd_creation_datetime = DateTime.Now;
                            objEWDetails.ewd_updated_by = employeeDetails.created_by;
                            objEWDetails.ewd_updation_datetime = DateTime.Now;
                            _db.tbl_employee_work_details.Add(objEWDetails);
                            _db.SaveChanges();

                            KGID_Models.KGIDLoan.tbl_hrms_pay_details_master objHRMSPay = new KGID_Models.KGIDLoan.tbl_hrms_pay_details_master();

                            objHRMSPay.hrms_emp_id = EMPID;
                            objHRMSPay.hrms_month_id = Convert.ToInt32(DateTime.Now.Month - 1);
                            objHRMSPay.hrms_year_id = Convert.ToInt32(DateTime.Now.Year - 1);
                            objHRMSPay.hrms_gross_pay = Convert.ToInt32(employeeDetails.payscale_minimum);
                            objHRMSPay.hrms_active = true;
                            objHRMSPay.hrms_creation_datetime = DateTime.Now;
                            objHRMSPay.hrms_created_by = employeeDetails.created_by;
                            objHRMSPay.hrms_updation_datetime = DateTime.Now;
                            objHRMSPay.hrms_updated_by = employeeDetails.created_by;
                            _db.tbl_hrms_pay_details_master.Add(objHRMSPay);
                            _db.SaveChanges();
                            dbContextTransaction.Commit();
                            DuplicateDetails = "1";


                            tbl_GIS_Subcription_details objgisPolicy = new tbl_GIS_Subcription_details();
                            objgisPolicy.gpd_empId = EMPID;
                            // objEWgisPolicy.gpd_firstKgidPolicyNum = objEWgisPolicy.ewd_place_of_posting;
                            objgisPolicy.gpd_appliedforGis = 1;
                            objgisPolicy.gdp_activeStatus = 1;                           
                            objgisPolicy.gdp_updated_by = (long)employeeDetails.created_by;
                            objgisPolicy.gdp_creation_datetime = DateTime.Now;
                            objgisPolicy.gdp_created_by = (long)employeeDetails.created_by;
                            objgisPolicy.gdp_updation_datetime = DateTime.Now;
                            _db.tbl_GIS_Subcription_details.Add(objgisPolicy);
                            _db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return DuplicateDetails;
        }

     //Genereate Application Number
        public async  Task<VM_ApplicationDetail> GenerateGISApplicationNumber(long empid)
        {
            VM_ApplicationDetail vM_Application = new VM_ApplicationDetail();
            long ApplicationReferenceNumber =0;
            long ApplicationID = 0;
            int ApplicationStatus1;
            int sentbackApplication1;
            string ApplicationRemark1;
            int age;
            tbl_GIS_application_details appDetails;
            tbl_GIS_application_workflow_details wfDetails;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
              
                tbl_employee_basic_details empDetails = db.tbl_employee_basic_details.AsNoTracking().Where(g => g.employee_id == empid && g.active_status == true).FirstOrDefault();
                DateTime dob = (DateTime)empDetails.date_of_birth;
                 age = DateTime.Now.Year - dob.Year;
                 appDetails = db.tbl_GIS_application_details.AsNoTracking().Where(g => g.gad_employee_id == empid && g.gad_active == 1).FirstOrDefault();
                
                if (appDetails == null)
                {
                    string date1 = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "");
                    date1 = date1.Replace("/", "");
                    date1 = date1.Replace(" ", "");
                    date1 = date1.Replace(":", "");
                    ApplicationReferenceNumber = long.Parse(date1);
                    try
                    {
                        tbl_GIS_application_details objBD = new tbl_GIS_application_details();
                        objBD.gad_application_no = ApplicationReferenceNumber;
                        objBD.gad_employee_id = empid;
                        objBD.gad_active = 1;
                        objBD.gad_created_by = empid;
                        objBD.gad_creation_date = DateTime.Now;
                        objBD.gad_updated_by = empid;
                        objBD.gad_updated_date = DateTime.Now;
                        _db.tbl_GIS_application_details.Add(objBD);
                         await _db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "GenerateGISApplicationNumber:Generrate application number" + ex.Message);
                        tbl_ErrorLog obj = new tbl_ErrorLog();
                        obj.ErrorDesc = ex.Message;
                        obj.CreatedBy = empid;
                        obj.CreatedDate = DateTime.UtcNow;
                        _db.tbl_ErrorLog.Add(obj);
                        _db.SaveChanges();
                    }
                }
                else
                { 
                    ApplicationReferenceNumber = appDetails.gad_application_no; 
                }
            }
            appDetails = await _db.tbl_GIS_application_details.AsNoTracking().Where(g => g.gad_employee_id == empid && g.gad_active == 1).FirstOrDefaultAsync();

            long applid = appDetails.gad_application_id;

            // INSERT APPLICATION ID TO tbl_GIS_Subcription_details

            var gisSubDetails = await _db.tbl_GIS_Subcription_details.AsNoTracking().Where(e => e.gpd_empId == empid && e.gdp_activeStatus == 1).FirstOrDefaultAsync();
            if (gisSubDetails != null)
            {
                try
                {
                    gisSubDetails.gpd_ApplicationId = applid;
                    _db.Entry(gisSubDetails).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "GenerateGISApplicationNumber:Update into GIS Subcription table" + ex.Message.ToString());
                    tbl_ErrorLog obj = new tbl_ErrorLog();
                    obj.ErrorDesc = ex.Message.ToString();
                    obj.CreatedBy = empid;
                    obj.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj);
                    _db.SaveChanges();
                }
            }
            else
            {
                try
                {
                    tbl_GIS_Subcription_details objSubDetails = new tbl_GIS_Subcription_details();

                  
                      objSubDetails.gpd_empId = empid;
                   // objSubDetails.gpd_GISPolicyNum = applid;
                    objSubDetails.gpd_appliedforGis =1;
                    objSubDetails.gdp_activeStatus =1;
                    objSubDetails.gdp_updated_by =empid;
                    objSubDetails.gdp_created_by = empid;
                    objSubDetails.gdp_creation_datetime =DateTime.Now;
                    objSubDetails.gdp_updation_datetime = DateTime.Now;
                    objSubDetails.gpd_ApplicationId = applid;
                    _db.tbl_GIS_Subcription_details.Add(objSubDetails);
                    await _db.SaveChangesAsync();

                    //_db.Entry(gisSubDetails).State = EntityState.Modified;
                    //await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "Update into GIS Subcription table" + ex.Message);
                    tbl_ErrorLog obj = new tbl_ErrorLog();
                    obj.ErrorDesc = ex.Message;
                    obj.CreatedBy = empid;
                    obj.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj);
                    _db.SaveChanges();
                }
            }

            wfDetails = _db.tbl_GIS_application_workflow_details.AsNoTracking().Where(g => g.gawt_application_id == appDetails.gad_application_id && g.gawt_active_status == 1 && (g.gawt_remarks != null || g.gawt_remarks != "")).FirstOrDefault();
            if (wfDetails != null)
            { ApplicationRemark1 = wfDetails.gawt_remarks; }
            else { ApplicationRemark1 = null; }
            var ApplicationStatus = (from workflorDetails in _db.tbl_GIS_application_workflow_details
                                     where ((workflorDetails.gawt_application_id == applid) && (workflorDetails.gawt_application_status != 2 || workflorDetails.gawt_application_status != 15 || workflorDetails.gawt_application_status != 23))
                                     select workflorDetails).Count();
            ApplicationStatus1 = ApplicationStatus;


            int sentbackApplication = (from workflorDetails in _db.tbl_GIS_application_workflow_details
                                       where ((workflorDetails.gawt_application_id == applid) && (workflorDetails.gawt_application_status == 2))
                                       select workflorDetails).Count();
            sentbackApplication1 = sentbackApplication;

            var cd = _db.tbl_GIS_challan_details.AsNoTracking().Where(g => g.gcd_emp_id == empid && g.gcd_application_id == applid && g.gcd_active_status ==1).FirstOrDefault();
            if (cd == null)
            { vM_Application.PaymentStatus = 1; }
            else 
            {
                var cs = _db.tbl_GIS_challan_status.AsNoTracking().Where(g => g.gcs_challan_id == cd.gcd_challan_id).FirstOrDefault();
                if (cs== null)
                { vM_Application.PaymentStatus = 0; }    
              
                else
                {
                   

                    var cs1 = _db.tbl_GIS_challan_status.AsNoTracking().Where(g => g.gcs_challan_id == cd.gcd_challan_id && (g.gcs_status == 2 || g.gcs_status == 5)).FirstOrDefault();
                    if (cs1 != null)
                    {
                        vM_Application.PaymentStatus = 3;
                    }
                    else
                    { 
                    var cs2 = _db.tbl_GIS_challan_status.AsNoTracking().Where(g => g.gcs_challan_id == cd.gcd_challan_id && (g.gcs_status == 1 || g.gcs_status == 6 || g.gcs_status == 15)).FirstOrDefault();
                    if (cs2 != null) { vM_Application.PaymentStatus = 0; }
                    else vM_Application.PaymentStatus = 2;
                    }
                   
                }
            }
            vM_Application.ApplicationId = applid;
            vM_Application.ApplicationNumber = ApplicationReferenceNumber.ToString();
            vM_Application.SentBackAppliaction = sentbackApplication1;
            vM_Application.Remarks = ApplicationRemark1 != null ?int.Parse(ApplicationRemark1): 0;
            vM_Application.ApplicationCount = ApplicationStatus1;//aplication status
            vM_Application.RestrictApplyingPolicy = age;
            // RESTRICTING POLICY PENDING 
            return vM_Application;
        }


        //TO CHECK IF APPLICATION IS APPROVED OR SENT BACK TO EMPLOYEE -- to restric access in gis application
        public async Task<long> GetGISApplicationStatus(long empid)
        { long applicationstatus = 0; ;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
               
                tbl_GIS_application_details appDetails = db.tbl_GIS_application_details.AsNoTracking().Where(g => g.gad_employee_id == empid && g.gad_active == 1).FirstOrDefault();
                if (appDetails != null)
                { 
                tbl_GIS_application_workflow_details workflow = db.tbl_GIS_application_workflow_details.AsNoTracking().Where(g => g.gawt_application_id == appDetails.gad_application_id && g.gawt_active_status == 1).FirstOrDefault();
                    if (workflow != null)
                    {
                        applicationstatus = workflow.gawt_application_status;
                    }
                }
            }
            return applicationstatus;
        }

        public VM_BasicDetails GISGetEmployeeDetails(long empid)
        {

            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                return (from empBasic in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true && e.employee_id == empid)
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
                        join appl1 in db.tbl_GIS_application_details.AsNoTracking().Where(a => a.gad_active == 1) on empBasic.employee_id equals appl1.gad_employee_id into set3
                        from appl in set3.DefaultIfEmpty()
                        join cd1 in db.tbl_GIS_challan_details.AsNoTracking().Where(a => a.gcd_active_status == 1) on appl.gad_application_id equals cd1.gcd_application_id into set4
                        from cd in set4.DefaultIfEmpty()
                        join cs1 in db.tbl_GIS_challan_status.AsNoTracking().Where(a => a.gcs_active_status == 1) on cd.gcd_challan_id equals cs1.gcs_challan_id into set5
                        from cs in set5.DefaultIfEmpty()

                        select new VM_BasicDetails
                        {

                            employee_name = empBasic.employee_name,
                            father_name = empBasic.father_name,
                            spouse_name = empBasic.spouse_name == null ? empBasic.spouse_name : "",
                            employee_name_kannada = empBasic.employee_name_kannada != null ? empBasic.employee_name_kannada : "",
                            father_name_kannada = empBasic.father_name_kannada != null ? empBasic.father_name_kannada : "",
                            spouse_name_kannada = empBasic.spouse_name_kannada != null ? empBasic.spouse_name_kannada : "",
                            gender_id = empBasic.gender_id,
                            gender_desc = genderMster.gender_desc,
                            mobile_number = empBasic.mobile_number,
                            email_id = empBasic.email_id,
                            ewddateofjoining = empWork.ewd_date_of_joining,
                            dateofbirth = empBasic.date_of_birth,
                            date_of_birth = empBasic.date_of_birth.ToString(),
                            place_of_birth = empBasic.place_of_birth,
                            // dept_employee_code=empWork.ed
                            ewd_ddo_id = empWork.ewd_ddo_id,
                            ddocode = ddoMaster.dm_ddo_code,
                            dateofappointment = empBasic.date_of_appointment,
                            pan_number = empBasic.pan_number,
                            ewd_date_of_joining = empWork.ewd_date_of_joining.ToString(),
                            eg_group_desc = groupMaster.eg_group_desc,
                            d_designation_desc = designationmaster.d_designation_desc,
                            payscle_code = payscaleMaster.payscale_minimum + "-" + payscaleMaster.payscale_maximum,                          
                            ewd_place_of_posting = empWork.ewd_place_of_posting,
                            ewd_employment_type = (int)empWork.ewd_employment_type,
                            et_employee_type_desc = empType.et_employee_type_desc,
                            ead_address = empadd.ead_address != null ? empadd.ead_address : "",
                            ead_pincode = empadd.ead_pincode != null ? empadd.ead_pincode : null,
                            //divorced_upload_doc_path = empBasic.divorced_upload_doc != null ? empBasic.divorced_upload_doc : null,
                            eod_emp_orphan = empOther.eod_emp_orphan != null ? (empOther.eod_emp_orphan.ToString() == "true" ? true : false) : false,// css.cs_challan_status_id != null ? css.cs_challan_status_id : 0,
                            eod_emp_married = empOther.eod_emp_married != null ? (empOther.eod_emp_married.ToString() == "true" ? true : false) : false,
                            eod_spouse_govt_emp = empOther.eod_spouse_govt_emp != null ? (empOther.eod_spouse_govt_emp.ToString() == "true" ? true : false) : false,
                            eod_spouse_kgid_number = empOther.eod_spouse_kgid_number != null ? empOther.eod_spouse_kgid_number : "",
                            eod_spouse_pan_number = empOther.eod_spouse_pan_number != null ? empOther.eod_spouse_pan_number : "",
                            Current_spouse_name = empBasic.Current_spouse_name != null ? empBasic.Current_spouse_name : "",
                            dr_status = empBasic.dr_status,                        
                            kad_application_id = appl.gad_application_id != null ? appl.gad_application_id : 0,
                            kad_kgid_application_number = appl.gad_application_no != null ? appl.gad_application_no.ToString() : "",
                            ProposalSubmissionDate = appl.gad_creation_date.ToString() != null ? appl.gad_creation_date.ToString() : "",//!= null :appl.gad_creation_date.ToString() :"",
                            ChallanReferenceNumber = cs.gcs_transaction_ref_no != null ? cs.gcs_transaction_ref_no : "",
                            ChallanAmount = cs.gcs_amount.ToString() != null ? cs.gcs_amount.ToString() : "",
                            ChallanPaymentDate = cs.gcs_date_of_transaction.ToString() != null ? cs.gcs_date_of_transaction.ToString() : "",
                        }).FirstOrDefault();
            }
        }

        public async Task<long> GISSaveEmpBasicDetails(VM_BasicDetails employeeBasicDetails)
        {
            long result = 0;
            try
            {
               
                using (DbConnectionKGID db = new DbConnectionKGID())
                {
                    var objAppD = await db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_employee_id == employeeBasicDetails.employee_id && e.gad_active == 1).FirstOrDefaultAsync();
                    var empother = await db.tbl_employee_other_details.AsNoTracking().Where(e => e.eod_emp_id == employeeBasicDetails.employee_id && e.eod_active_status == true).FirstOrDefaultAsync();
                    var objBD1 = db.tbl_employee_basic_details.AsNoTracking().Where(e => e.employee_id == employeeBasicDetails.employee_id && e.active_status == true).FirstOrDefault();

                    var empaddress = await db.tbl_employee_address_details.AsNoTracking().Where(e => e.ead_emp_id == employeeBasicDetails.employee_id && e.ead_active_status == true).FirstOrDefaultAsync();
                    string address = employeeBasicDetails.ead_address;
                    int pincode = (int)employeeBasicDetails.ead_pincode;
                    try
                    {
                        objBD1.father_name_kannada = employeeBasicDetails.father_name_kannada;
                        objBD1.employee_name_kannada = employeeBasicDetails.employee_name_kannada;
                        objBD1.active_status = true;
                        db.Entry(objBD1).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        result = 1;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "GISSaveEmpBasicDetails:Save Basic Details" + ex.Message);
                        result = 0;
                        tbl_ErrorLog obj = new tbl_ErrorLog();
                        obj.ErrorDesc = ex.Message;
                        obj.CreatedBy = (int)employeeBasicDetails.employee_id;
                        obj.CreatedDate = DateTime.UtcNow;
                        _db.tbl_ErrorLog.Add(obj);
                        _db.SaveChanges();
                    }
                    try
                    {
                        if (empaddress == null)
                        {
                            tbl_employee_address_details objAD = new tbl_employee_address_details();
                            objAD.ead_address = employeeBasicDetails.ead_address;
                            objAD.ead_pincode = employeeBasicDetails.ead_pincode;
                            objAD.ead_emp_id = employeeBasicDetails.employee_id;
                            objAD.ead_active_status = true;
                            objAD.ead_updated_by = (int)employeeBasicDetails.employee_id;
                            objAD.ead_created_by = (int)employeeBasicDetails.employee_id;
                            objAD.ead_creation_datetime = DateTime.Now;
                            objAD.ead_updation_datetime = DateTime.Now;
                            objAD.ead_application_id = objAppD.gad_application_id;
                            _db.tbl_employee_address_details.Add(objAD);
                            await _db.SaveChangesAsync();
                            result = 1;
                        }
                        else
                        {
                           
                            empaddress.ead_address = address;
                            empaddress.ead_pincode = pincode;
                            empaddress.ead_updated_by = (int)employeeBasicDetails.employee_id;
                            empaddress.ead_updation_datetime = DateTime.Now;
                            db.Entry(empaddress).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            result = 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "GISSaveEmpBasicDetails:address details" + ex.Message);
                        result = 2;
                        tbl_ErrorLog obj = new tbl_ErrorLog();
                        obj.ErrorDesc = ex.Message;
                        obj.CreatedBy = (int)employeeBasicDetails.employee_id;
                        obj.CreatedDate = DateTime.UtcNow;
                        _db.tbl_ErrorLog.Add(obj);
                        _db.SaveChanges();
                    }                 
                }
                
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "GISSaveEmpBasicDetails : Error in saving" + ex.Message);
                tbl_ErrorLog obj = new tbl_ErrorLog();
                obj.ErrorDesc = ex.Message;
                obj.CreatedBy = (int)employeeBasicDetails.employee_id;
                obj.CreatedDate = DateTime.UtcNow;
                _db.tbl_ErrorLog.Add(obj);
                _db.SaveChanges();
            }
            return result;
        }
        public List<VM_DropDownList> GetNomineelist(long empId)
        {

            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                var mw = db.tbl_employee_basic_details.AsNoTracking().Where(e => e.employee_id == empId && e.active_status == true).FirstOrDefault();
                var m = db.tbl_employee_other_details.AsNoTracking().Where(e => e.eod_emp_id == mw.employee_id).FirstOrDefault();

                if (mw != null)
                {
                    bool ismarried = mw.spouse_name != null ? true : false;
                    if (ismarried == true)
                    {
                        return (from relationList in db.tbl_GIS_family_relation_master.AsNoTracking().Where(e => e.frm_active_status == true && e.frm_family_type_id == 2)

                                select new VM_DropDownList
                                {
                                    Id = relationList.frm_relation_id,
                                    Value = relationList.frm_relation_desc,
                                }).ToList();
                    }
                    else
                    {
                        return (from relationList in db.tbl_GIS_family_relation_master.AsNoTracking().Where(e => e.frm_active_status == true && e.frm_family_type_id == 1)

                                select new VM_DropDownList
                                {
                                    Id = relationList.frm_relation_id,
                                    Value = relationList.frm_relation_desc,
                                }).ToList();
                    }
                }
                return null;
            }       
        }

        public async Task<long> GISSaveNBNominee(VM_NomineeDetail objNominee)
        {
            long gndapplicationid = 0;
            long result = 1;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                var objAppD =  await db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_employee_id == (long)objNominee.EmpId && e.gad_active == 1).FirstOrDefaultAsync();
                if (objAppD != null)
                    gndapplicationid = objAppD.gad_application_id;

                if ( objNominee.Id != 0 )
                        {
                    try
                    {
                        tbl_GIS_nominee_details editNom = await _db.tbl_GIS_nominee_details.AsNoTracking().Where(e => (long)e.gnd_nominee_id == (long)objNominee.Id ).FirstOrDefaultAsync();
                         editNom.gnd_relation_id = int.Parse(objNominee.Relation);
                        editNom.gnd_name_of_nominee = objNominee.NameOfNominee;
                        editNom.gnd_percentage_of_share =(int)objNominee.PercentageShare;                       
                        editNom.gnd_updation_datetime = DateTime.Now;
                        editNom.gnd_updated_by = (long)objNominee.EmpId;
                        editNom.gnd_Nominee_age = objNominee.Age !=null ? objNominee.Age:0;
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
                        Logger.LogMessage(TracingLevel.INFO, "GISSaveNBNominee: error in saving nominee details" + ex.Message);
                        result  = 0;
                        tbl_ErrorLog obj = new tbl_ErrorLog();
                        obj.ErrorDesc = ex.Message;
                        obj.CreatedBy = (long)objNominee.EmpId;
                        obj.CreatedDate = DateTime.UtcNow;
                        _db.tbl_ErrorLog.Add(obj);
                        _db.SaveChanges();
                    }
                }
                else { 
                try
                {
                    tbl_GIS_nominee_details objBD = new tbl_GIS_nominee_details();
                    objBD.gnd_emp_id = (long)objNominee.EmpId;
                    objBD.gnd_application_id = gndapplicationid;
                    objBD.gnd_relation_id = int.Parse(objNominee.Relation);
                    objBD.gnd_name_of_nominee = objNominee.NameOfNominee;                  
                    objBD.gnd_percentage_of_share = objNominee.PercentageShare != null ?(int)objNominee.PercentageShare:0;                  
                    objBD.gnd_active = 1;
                    objBD.gnd_creation_datetime = DateTime.Now;
                    objBD.gnd_created_by = (long)objNominee.EmpId;
                    objBD.gnd_updation_datetime = DateTime.Now;
                    objBD.gnd_updated_by = (long)objNominee.EmpId;                    
                    objBD.gnd_Nominee_age = objNominee.Age;
                        if (objNominee.Relation != "14") { objBD.gnd_nomineeDob = objNominee.nomineeDOB; }
                  
                        objBD.gnd_contingencies = objNominee.gnd_contingencies !=null ? objNominee.gnd_contingencies :null;
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
                        tbl_ErrorLog obj = new tbl_ErrorLog();
                        obj.ErrorDesc = ex.Message;
                        obj.CreatedBy = (long)objNominee.EmpId;
                        obj.CreatedDate = DateTime.UtcNow;
                        _db.tbl_ErrorLog.Add(obj);
                        _db.SaveChanges();
                    }
                }
            }

            return result ;
        }

        public async Task<int> GISDeleteNominee(VM_NomineeDetail objNominee)
        {
            int result = 0;
            using (DbConnectionKGID context = new DbConnectionKGID())
            {
                try
                {
                  
                    if (objNominee.Id != 0)
                    {
                       
                            tbl_GIS_nominee_details editNom = await _db.tbl_GIS_nominee_details.AsNoTracking().Where(e => (long)e.gnd_nominee_id == (long)objNominee.Id).FirstOrDefaultAsync();
                            editNom.gnd_active = 0;
                            _db.Entry(editNom).State = EntityState.Modified;
                            await _db.SaveChangesAsync();
                            result = 1;
                    }
                   

                    //


                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "GISDeleteNominee:Delete Nominee Details - GISDeleteNominee-- Group Insurance" + ex.Message);
                    result = 0;
                    tbl_ErrorLog obj = new tbl_ErrorLog();
                    obj.ErrorDesc = ex.Message;
                    obj.CreatedBy = (long)objNominee.EmpId;
                    obj.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj);
                    _db.SaveChanges();
                }          
            }
            return  result;
        }
        public List<VM_NomineeDetail> GISNomineeDetailsDll(long empid)
        {

            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                try
                {

                    return (from empBasic in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true && e.employee_id == empid)
                            join empNominee1 in db.tbl_GIS_nominee_details.AsNoTracking().Where(a => a.gnd_active == 1) on empBasic.employee_id equals empNominee1.gnd_emp_id into set2
                            from empNominee in set2.DefaultIfEmpty()
                            join familyMaster in db.tbl_GIS_family_relation_master.AsNoTracking().Where(a => a.frm_active_status == true) on empNominee.gnd_relation_id equals familyMaster.frm_relation_id
                            select new VM_NomineeDetail
                            {
                                
                                EmpId = empBasic.employee_id,                                
                                RelationId = (int)empNominee.gnd_relation_id,
                                NameOfNominee = empNominee.gnd_name_of_nominee,                                
                                PercentageShare = (int)empNominee.gnd_percentage_of_share,                              
                                Age = empNominee.gnd_Nominee_age,
                                Relation = familyMaster.frm_relation_desc,
                                Id= empNominee.gnd_nominee_id,
                                nomineeDOB = empNominee.gnd_relation_id != 14 ? empNominee.gnd_nomineeDob:default(DateTime),
                                // nomineeDOB= empNominee.gnd_nomineeDob ,
                                SonDaughterAdoption_doc_path = empNominee.gnd_AdoptionFile_path,
                                gnd_contingencies = empNominee.gnd_contingencies,
                                gnd_predeceasing = empNominee.gnd_predeceasing,
                            }).ToList();
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "GISNomineeDetailsDll: error in retriving nominee details" + ex.Message);
                    tbl_ErrorLog obj = new tbl_ErrorLog();
                    obj.ErrorDesc = ex.Message;
                    obj.CreatedBy = 0;
                    obj.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj);
                    _db.SaveChanges();
                    return null;
                }
            }
         //   
        }

        public float GisGetIntialPaymentDetails(long empId)
        {
            float initialPayment = 0;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                var mw = db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_emp_id == empId && e.ewd_active_status == true).FirstOrDefault();
                int groupId = mw.ewd_group_id;
                string JoiningDate = mw.ewd_date_of_joining.ToString();
                string month = JoiningDate.Substring(3,2);
            }
            return initialPayment;
        }//Not used

        public VM_GISPaymentDetails GISPaymentDll(long EmpID)
        {
            VM_GISPaymentDetails obj = new VM_GISPaymentDetails();
            int GISInsuranceamt = 0;
            int GISIamt = 0;
            int GISSamt = 0;
            string receiptType = "";
            int receiptTypeId = 0;
            int gcd_purpose_id = 0;
            int gcd_sub_purpose_id = 0;
            string sub_purpose_desc = "";
            string purpose_desc = "";
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
              


                var empWork = db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_emp_id == EmpID && e.ewd_active_status == true).FirstOrDefault();
                var gisApplDetails = db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_employee_id == EmpID && e.gad_active== 1).FirstOrDefault();
                long appid = gisApplDetails.gad_application_id;
                var gisChallanDetails = db.tbl_GIS_challan_details.AsNoTracking().Where(e => e.gcd_emp_id == EmpID && e.gcd_application_id == appid && e.gcd_active_status == 1).FirstOrDefault();

                string dateofJoining = empWork.ewd_date_of_joining.ToString().Substring(3, 2);
                var GisSubscriptionMaster = db.tbl_GIS_Subcription_Master.AsNoTracking().Where(e => e.GSM_Active_Status == 1 && e.GSM_Group_ID == empWork.ewd_group_id).FirstOrDefault();
                var ddocode = db.tbl_ddo_master.AsNoTracking().Where(e => e.dm_ddo_id == empWork.ewd_ddo_id && e.dm_active == true ).FirstOrDefault();
                try
                {
                    if (dateofJoining == "01")
                    {
                        GISInsuranceamt = GisSubscriptionMaster.GSM_Total;
                        GISIamt = GisSubscriptionMaster.GSM_InsuranceFund;
                        GISSamt = GisSubscriptionMaster.GSM_SavingFund;
                        //receiptType = "GIS_Savings Fund";
                        receiptType = "GIS Savings Fund";
                        receiptTypeId = 4;
                        gcd_purpose_id = 4;
                        gcd_sub_purpose_id = 24;
                        //sub_purpose_desc = "Savings Fund  ";
                        //purpose_desc = "GIS_Savings Fund  ";
                        sub_purpose_desc = "Savings Fund";
                        purpose_desc = "GIS Savings Fund";
                    }
                    else
                    {
                        GISInsuranceamt = GisSubscriptionMaster.GSM_InsuranceFund;
                        GISIamt = GisSubscriptionMaster.GSM_InsuranceFund;
                        GISSamt = 0;
                        //receiptType = "GIS_ Insurance Fund";
                        receiptType = "GIS Insurance Fund";
                        receiptTypeId = 3;
                        gcd_purpose_id = 3;
                        gcd_sub_purpose_id = 21;
                        //sub_purpose_desc = "Insurance Fund  ";
                        //purpose_desc = "GIS_ Insurance Fund  ";
                        sub_purpose_desc = "Insurance Fund";
                        purpose_desc = "GIS Insurance Fund";
                    }
                }
                catch (Exception ex)
                { Logger.LogMessage(TracingLevel.INFO, "GISPayment: error in inializing payment details" + ex.Message);


                    tbl_ErrorLog obj1 = new tbl_ErrorLog();
                    obj1.ErrorDesc = ex.Message;
                    obj1.CreatedBy = EmpID;
                    obj1.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj1);
                    _db.SaveChanges();
                }



            }
            try
            {

                DataSet dsBD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpID",EmpID)
                };
                dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_GIS2_selectchallandetails");
                if (dsBD.Tables[0].Rows.Count > 0)
                {
                    obj.EmpID = Convert.ToInt32(dsBD.Tables[0].Rows[0]["EmpID"]);
                   obj.gcd_application_id = Convert.ToInt32(dsBD.Tables[0].Rows[0]["cd_application_id"]);
                    obj.ddo_code = dsBD.Tables[0].Rows[0]["ddo_code"].ToString();
                    obj.gcd_challan_ref_no = dsBD.Tables[0].Rows[0]["cd_challan_ref_no"].ToString();
                    obj.gcd_date_of_generation = dsBD.Tables[0].Rows[0]["cd_date_of_generation"].ToString();
                    obj.hoa = dsBD.Tables[0].Rows[0]["hoa"].ToString();
                    obj.hoa1 = dsBD.Tables[0].Rows[0]["hoa1"].ToString();
                    obj.gcd_amount = GISInsuranceamt; //Convert.ToInt32(dsBD.Tables[0].Rows[0]["gcd_amount"]);
                    obj.PayStatus = dsBD.Tables[0].Rows[0]["PayStatus"].ToString();
                    //Newly Added 22-02-2021 Ujwal
                    obj.EmpName = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                    obj.gcd_purpose_id = gcd_purpose_id;//Convert.ToInt32(dsBD.Tables[0].Rows[0]["purpose_code"]);
                    obj.purpose_desc = purpose_desc;//dsBD.Tables[0].Rows[0]["purpose_desc"].ToString();
                    obj.gcd_sub_purpose_id = gcd_sub_purpose_id;//Convert.ToInt32(dsBD.Tables[0].Rows[0]["sub_purpose_code"]);
                    obj.sub_purpose_desc = sub_purpose_desc;//dsBD.Tables[0].Rows[0]["sub_purpose_desc"].ToString();
                    obj.receipttypeid = receiptTypeId;
                    obj.gcd_insurance_amount = GISIamt;
                    obj.gcd_savings_amount = GISSamt;
                }

            }
            catch (Exception ex)
            {

                tbl_ErrorLog obj1 = new tbl_ErrorLog();
                obj1.ErrorDesc = ex.Message;
                obj1.CreatedBy = EmpID;
                obj1.CreatedDate = DateTime.UtcNow;
                _db.tbl_ErrorLog.Add(obj1);
                _db.SaveChanges();
            }
            return obj;
        }

        public long GISSavePaymentDll(VM_GISPaymentDetails objPaymentDetails)
        {
            long result = 0;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                var empWork = db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_emp_id == objPaymentDetails.EmpID && e.ewd_active_status == true).FirstOrDefault();
                var DM = db.tbl_ddo_master.AsNoTracking().Where(e => e.dm_ddo_id == empWork.ewd_ddo_id && e.dm_active == true).FirstOrDefault();
                var DD = db.tbl_ddo_dio_master.AsNoTracking().Where(e => e.dd_district_id == DM.dm_district_id && e.dd_status == true).FirstOrDefault();
                var gcd =db.tbl_GIS_challan_details.AsNoTracking().Where(e => e.gcd_emp_id == objPaymentDetails.EmpID && e.gcd_application_id == objPaymentDetails.gcd_application_id && e.gcd_challan_ref_no == objPaymentDetails.gcd_challan_ref_no).FirstOrDefault();
                if (gcd == null)
                {
                    var gcd1 = db.tbl_GIS_challan_details.AsNoTracking().Where(e => e.gcd_emp_id == objPaymentDetails.EmpID && e.gcd_application_id == objPaymentDetails.gcd_application_id ).FirstOrDefault();
                    if (gcd1 != null)
                    {
                        gcd1.gcd_active_status = 0;
                        db.Entry(gcd1).State = EntityState.Modified;
                        db.SaveChanges();

                        //objWFD.gawt_updated_by =
                    }
                    try
                    {
                        tbl_GIS_challan_details objBD = new tbl_GIS_challan_details();

                        objBD.gcd_challan_ref_no = objPaymentDetails.gcd_challan_ref_no;
                        objBD.gcd_purpose_id = objPaymentDetails.gcd_purpose_id;
                        objBD.gcd_sub_purpose_id = objPaymentDetails.gcd_sub_purpose_id;
                        objBD.gcd_amount = objPaymentDetails.gcd_amount;
                        objBD.gcd_application_id = objPaymentDetails.gcd_application_id;
                        objBD.gcd_emp_id = objPaymentDetails.EmpID;
                        objBD.gcd_date_of_generation = DateTime.UtcNow;
                        //objBD.gcd_dio_id = objPaymentDetails.ddo_code_id;
                        objBD.gcd_dio_id = DD.dd_dio_id;
                        objBD.gcd_date_of_generation = DateTime.UtcNow;
                        objBD.gcd_agency_ddo_id = DM.dm_ddo_id;
                        objBD.gcd_active_status = 1;
                        objBD.gcd_creation_datetime = DateTime.UtcNow;
                        objBD.gcd_updation_datetime = DateTime.UtcNow;
                        objBD.gcd_created_by = objPaymentDetails.EmpID;
                        objBD.gcd_updated_by = objPaymentDetails.EmpID;
                        objBD.gcd_insurance_amount = objPaymentDetails.gcd_insurance_amount;
                        objBD.gcd_savings_amount = objPaymentDetails.gcd_savings_amount;

                        _db.tbl_GIS_challan_details.Add(objBD);
                        _db.SaveChanges();
                        result = 1;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "GISSavePaymentDll: error in challan details" + ex.Message);

                        tbl_ErrorLog obj1 = new tbl_ErrorLog();
                        obj1.ErrorDesc = ex.Message;
                        obj1.CreatedBy = objPaymentDetails.EmpID;
                        obj1.CreatedDate = DateTime.UtcNow;
                        _db.tbl_ErrorLog.Add(obj1);
                        _db.SaveChanges();
                    }
                }
                else 
                {
                    try
                    {
                       

                        gcd.gcd_challan_ref_no = objPaymentDetails.gcd_challan_ref_no;
                        gcd.gcd_purpose_id = objPaymentDetails.gcd_purpose_id;
                        gcd.gcd_sub_purpose_id = objPaymentDetails.gcd_sub_purpose_id;
                        gcd.gcd_amount = objPaymentDetails.gcd_amount;
                        gcd.gcd_application_id = objPaymentDetails.gcd_application_id;
                        gcd.gcd_emp_id = objPaymentDetails.EmpID;
                       // gcd.gcd_date_of_generation = DateTime.UtcNow;
                        //objBD.gcd_dio_id = objPaymentDetails.ddo_code_id;
                        gcd.gcd_dio_id = DD.dd_dio_id;
                        //gcd.gcd_date_of_generation = DateTime.UtcNow;
                        if (objPaymentDetails.gcd_date_of_generation != null) { gcd.gcd_date_of_generation = DateTime.Parse(objPaymentDetails.gcd_date_of_generation); }
                        else { gcd.gcd_date_of_generation = DateTime.UtcNow; }
                        gcd.gcd_agency_ddo_id = DM.dm_ddo_id;
                        gcd.gcd_active_status = 1;
                      //  gcd.gcd_creation_datetime = DateTime.UtcNow;
                        gcd.gcd_updation_datetime = DateTime.UtcNow;
                       // gcd.gcd_created_by = objPaymentDetails.EmpID;
                        gcd.gcd_updated_by = objPaymentDetails.EmpID;
                        gcd.gcd_insurance_amount = objPaymentDetails.gcd_insurance_amount;
                        gcd.gcd_savings_amount = objPaymentDetails.gcd_savings_amount;

                        _db.tbl_GIS_challan_details.Add(gcd);
                        _db.SaveChanges();
                        result = 1;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "GISSavePaymentDll: error in challan details" + ex.Message);

                        tbl_ErrorLog obj1 = new tbl_ErrorLog();
                        obj1.ErrorDesc = ex.Message;
                        obj1.CreatedBy = objPaymentDetails.EmpID;
                        obj1.CreatedDate = DateTime.UtcNow;
                        _db.tbl_ErrorLog.Add(obj1);
                        _db.SaveChanges();
                    }
                }
               
            }
            return result;
        }

        public long GISSaveChallanStatusDll(VM_GISPaymentDetails objPaymentDetails)
        {
            long result = 1;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                var challanDetails = _db.tbl_GIS_challan_details.AsNoTracking().Where(e => e.gcd_application_id == objPaymentDetails.gcd_application_id && e.gcd_active_status == 1).FirstOrDefault();

                try
                {
                    tbl_GIS_challan_status obj = new tbl_GIS_challan_status();                 
                    obj.gcs_challan_id = challanDetails.gcd_challan_id;
                    obj.gcs_transaction_ref_no = objPaymentDetails.gcs_transaction_ref_no;
                    obj.gcs_amount = objPaymentDetails.gcs_amount;
                    obj.gcs_date_of_transaction = DateTime.UtcNow;
                    obj.gcs_status = objPaymentDetails.gcs_status;
                    obj.gcs_active_status =1;
                    obj.gcs_creation_datetime = DateTime.UtcNow;
                    obj.gcs_updation_datetime = DateTime.UtcNow;
                    obj.gcs_created_by = objPaymentDetails.EmpID;
                    obj.gcs_updated_by = objPaymentDetails.EmpID;                   
                    _db.tbl_GIS_challan_status.Add(obj);
                    _db.SaveChanges();                    //result = 0;
                }
                catch (Exception ex)
                {
                    result = 0;
                    Logger.LogMessage(TracingLevel.INFO, "GISSaveChallanStatusDll: error in challan status" + ex.Message);
                    tbl_ErrorLog obj1 = new tbl_ErrorLog();
                    obj1.ErrorDesc = ex.Message;
                    obj1.CreatedBy = objPaymentDetails.EmpID;
                    obj1.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj1);
                    _db.SaveChanges();
                }
            }
            return result;
        
            
        }

        public string GISUpdateChallanStatusDll(string ChallanRefNo, long StatusCode, long EmpID) // -ref nb
        {
            string result = "0";
            using (DbConnectionKGID db = new DbConnectionKGID())
            { 
                try
                {
                var challanDetails = _db.tbl_GIS_challan_details.AsNoTracking().Where(e => e.gcd_challan_ref_no == ChallanRefNo  && e.gcd_active_status == 1).FirstOrDefault();

                var challanStatus = _db.tbl_GIS_challan_status.AsNoTracking().Where(e => e.gcs_challan_id == challanDetails.gcd_challan_id ).FirstOrDefault();
                var challanStatusMastr = _db.tbl_challan_status_master.AsNoTracking().Where(e => e.csm_status_code == StatusCode).FirstOrDefault();

                    if (challanStatus != null)
                    {
                        challanStatus.gcs_status = challanStatusMastr.csm_status_id;
                        db.Entry(challanStatus).State = EntityState.Modified;
                        db.SaveChanges();
                        result = "1";
                    }
                    else
                    {
                        tbl_GIS_challan_status obj = new tbl_GIS_challan_status();
                        obj.gcs_challan_id = challanDetails.gcd_challan_id;
                        obj.gcs_transaction_ref_no = ChallanRefNo;
                        obj.gcs_amount = challanDetails.gcd_amount;
                        obj.gcs_date_of_transaction = DateTime.UtcNow;
                        obj.gcs_status = challanStatusMastr.csm_status_id;
                        obj.gcs_active_status = 1;
                        obj.gcs_creation_datetime = DateTime.UtcNow;
                        obj.gcs_updation_datetime = DateTime.UtcNow;
                        obj.gcs_created_by = EmpID;
                        obj.gcs_updated_by = EmpID;
                        _db.tbl_GIS_challan_status.Add(obj);
                        _db.SaveChanges();
                        result = "1";
                    }
                }
                catch (Exception ex)
                {
                    result = "0";
                    Logger.LogMessage(TracingLevel.INFO, "GISSaveChallanStatusDll: error in challan status" + ex.Message);
                    tbl_ErrorLog obj1 = new tbl_ErrorLog();
                    obj1.ErrorDesc = ex.Message;
                    obj1.CreatedBy = EmpID;
                    obj1.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj1);
                    _db.SaveChanges();
                }
            }
            return result;


        }



        
       public int GISSaveDeclaration(long EmpId, long AppId)
        {
            long gndapplicationid = 0;
            int result = 1;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                var objAppD = db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_employee_id == (long)EmpId && e.gad_active == 1).FirstOrDefault();
                if (objAppD != null)
                    gndapplicationid = objAppD.gad_application_id;
                var objChallanD = db.tbl_GIS_challan_details.AsNoTracking().Where(e => e.gcd_application_id == (long)gndapplicationid && e.gcd_active_status == 1).FirstOrDefault();
                var objBD = db.tbl_employee_basic_details.AsNoTracking().Where(e => e.employee_id == (long)EmpId && e.active_status == true).FirstOrDefault();
                var objWFD = db.tbl_GIS_application_workflow_details.AsNoTracking().Where(e => e.gawt_application_id == AppId && e.gawt_active_status == 1).FirstOrDefault();


                ////CODE TO FIND ASSIGN APPLICATION  --.gawt_assigned_to  REFERENCE--sp_kgid_AssignApplicationRole sp_kgid_insertDepartmentWorkflowVerification]
                //var objwD = db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_emp_id == (long)EmpId && e.ewd_active_status == true).FirstOrDefault();
                //var objddomaster = db.tbl_ddo_master.AsNoTracking().Where(e => e.dm_ddo_id == objwD.ewd_ddo_id && e.dm_active == true).FirstOrDefault();
                //var objddodio = db.tbl_ddo_dio_master.AsNoTracking().Where(e => e.dd_district_id == (int)objddomaster.dm_district_id && e.dd_status == true).FirstOrDefault();  // 
                //var objassignappl = db.tbl_assign_application.AsNoTracking().Where(e => e.ap_dist_id == (int)objddodio.dd_district_id && e.ap_category_id == 2 && e.ap_active_status == true).FirstOrDefault();//&& 

                //var assignto = objassignappl.ap_emp_id;
                //////

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
                    if (objWFD != null)
                    {
                        objWFD.gawt_active_status = 0;
                        db.Entry(objWFD).State = EntityState.Modified;
                        db.SaveChanges();

                        //objWFD.gawt_updated_by =;
                        //objWFD.gawt_updation_datetime=
                    }

                    try
                    {
                        tbl_GIS_application_workflow_details objGad = new tbl_GIS_application_workflow_details();
                        objGad.gawt_application_id = gndapplicationid;
                        objGad.gawt_InsuranceAmt = objChallanD.gcd_insurance_amount;
                        objGad.gawt_savingAmt = objChallanD.gcd_savings_amount;
                        objGad.gawt_InsuranceDate = objChallanD.gcd_date_of_generation;
                        objGad.gawt_savingDate = objChallanD.gcd_date_of_generation;
                        objGad.gawt_verified_by = EmpId;
                        if (ddocode == null) { objGad.gawt_assigned_to = (long)objBD.created_by; }
                        else { objGad.gawt_assigned_to = (long)ddocode.ewd_emp_id; }
                        //  objGad.gawt_assigned_to = (long)objBD.created_by;
                        //objGad.gawt_remarks ="";
                        //objGad.gawt_comments ="";
                        objGad.gawt_application_status = 3;
                        objGad.gawt_active_status = 1;

                        objGad.gawt_created_by = EmpId;
                        objGad.gawt_creation_datetime = DateTime.UtcNow;
                        objGad.gawt_updated_by = EmpId;
                        objGad.gawt_updation_datetime = DateTime.UtcNow;
                        _db.tbl_GIS_application_workflow_details.Add(objGad);
                        _db.SaveChanges();
                        result = 2; //sucessfully saved
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "GISSaveDeclaration: workflow error --Submit to DDO" + ex.Message);
                        result = 1;  //error in saving workflow from applicant to ddo
                        tbl_ErrorLog obj1 = new tbl_ErrorLog();
                        obj1.ErrorDesc = ex.Message;
                        obj1.CreatedBy = EmpId;
                        obj1.CreatedDate = DateTime.UtcNow;
                        _db.tbl_ErrorLog.Add(obj1);
                        _db.SaveChanges();
                    }
                }
                else { result = 3;}   // ddo not mappped
            }

            return result;
        }


        public VM_GISDDOVerificationDetails GetEmployeeDetailsForDDOVerification(long empId)
        {
            VM_GISDDOVerificationDetails verificationDetails = new VM_GISDDOVerificationDetails();
            try
            {
                using (DbConnectionKGID db = new DbConnectionKGID())
                {
                    var empworkdetails = db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_emp_id == empId && e.ewd_active_status == true).FirstOrDefault();
                    int ddoid = empworkdetails.ewd_ddo_id;

                    var EmployeeVerification = 
                        (from workFlow in db.tbl_GIS_application_workflow_details.AsNoTracking().Where(e => e.gawt_active_status == 1 && (e.gawt_application_status != 15)) 
                          join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on workFlow.gawt_application_id  equals application.gad_application_id                       
                          join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on application.gad_employee_id equals ebd.employee_id
                          join  ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid  && e.ewd_active_status== true )on ebd.employee_id equals ewd.ewd_emp_id
                         join deptmaster in db.tbl_department_master.AsNoTracking().Where(e => e.dm_active == true ) on ebd.dept_employee_code equals deptmaster.dm_dept_id
                         join ddomaster in db.tbl_ddo_master.AsNoTracking().Where(e => e.dm_active == true) on ewd.ewd_ddo_id equals ddomaster.dm_ddo_id
                         join ddodiomaster in db.tbl_ddo_dio_master.AsNoTracking().Where(e => e.dd_status == true) on ddomaster.dm_district_id equals ddodiomaster.dd_district_id
                         join distmaster in db.tbl_district_master.AsNoTracking().Where(e => e.dm_status == true) on ddodiomaster.dd_district_id equals distmaster.dm_id

                          select new GISEmployeeDDOVerificationDetail
                                {

                             EmployeeCode =ebd.employee_id,
                             Name = ebd.employee_name,
                             ApplicationNumber =application.gad_application_no.ToString(),
                             ApplicationId = application.gad_application_id,
                             Status = workFlow.gawt_application_status == 3 ? "Pending" : "Revert",//AppStatus
                             Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                             RowNum = (int)ebd.employee_id,
                             Department = deptmaster.dm_deptname_english,
                             District      = distmaster.dm_name_english,

                         }).ToList();

                    var LastUpdatedStatus  = (from ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid && e.ewd_active_status == true )
                         join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on ewd.ewd_emp_id equals ebd.employee_id
                         join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on ebd.employee_id equals application.gad_employee_id
                        join workFlow in db.tbl_GIS_application_workflow_details.AsNoTracking().Where(e => e.gawt_active_status == 1 &&  e.gawt_application_status != 15) on application.gad_application_id equals workFlow.gawt_application_id
                          select new GISEmployeeDDOVerificationDetail
                          {

                              EmployeeCode = ebd.employee_id,
                              Name = ebd.employee_name,
                              ApplicationNumber = application.gad_application_no.ToString(),
                              ApplicationId = application.gad_application_id,
                              Status = workFlow.gawt_application_status == 3 ? "Pending" : "Revert",//AppStatus
                              Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                              District = "",
                              Department = ""
                          }).ToList();
                    var IEmployeeVerification=
                        (from ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid && e.ewd_active_status == true )
                          join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true & e.first_kgid_policy_no !=null) on ewd.ewd_emp_id equals ebd.employee_id
                          join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on ebd.employee_id equals application.gad_employee_id
                          join workFlow in db.tbl_GIS_application_workflow_details.AsNoTracking().Where(e => e.gawt_active_status == 1 && (e.gawt_application_status == 3 || e.gawt_application_status == 4)) on application.gad_application_id equals workFlow.gawt_application_id
                        
                         select new GISEmployeeDDOVerificationDetail
                          {

                              EmployeeCode = ebd.employee_id,
                              Name = ebd.employee_name,
                              ApplicationNumber = application.gad_application_no.ToString(),
                              ApplicationId = application.gad_application_id,
                              Status = workFlow.gawt_application_status == 3 ? "Pending" : "Reverted",//AppStatus
                              Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                          }).ToList();
                    //NOT yet done 
                    var ApprovedStatus = (from ewd in db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_ddo_id == ddoid && e.ewd_active_status == true)
                                             join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on ewd.ewd_emp_id equals ebd.employee_id
                                             join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on ebd.employee_id equals application.gad_employee_id
                                             join workFlow in db.tbl_GIS_application_workflow_details.AsNoTracking().Where(e => e.gawt_active_status == 1 && (e.gawt_application_status == 15)) on application.gad_application_id equals workFlow.gawt_application_id
                                          join challanDeatils in db.tbl_GIS_challan_details.AsNoTracking().Where(e => e.gcd_active_status == 1 ) on workFlow.gawt_application_id equals challanDeatils.gcd_application_id

                                            select new GISEmployeeDDOVerificationDetail
                                             {

                                                 EmployeeCode = ebd.employee_id,
                                                 Name = ebd.employee_name,
                                                 ApplicationNumber = application.gad_application_no.ToString(),
                                                 ApplicationId = application.gad_application_id,
                                                 Status ="Approved",//AppStatus
                                                // LastUpdatedDate = dataRow.Field<string>("kawt_creation_datetime"),
                                                
                                                 Premium = challanDeatils.gcd_amount.ToString(),
                                          }).ToList();

                    verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                    verificationDetails.IEmployeeVerificationDetails = IEmployeeVerification;
                    verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                    verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;


                    //pending application
                    var pendingapplication =
                   (from workFlow in db.tbl_GIS_application_workflow_details.AsNoTracking().Where(e => e.gawt_active_status == 1 && (e.gawt_application_status == 3))
                    join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on workFlow.gawt_application_id equals application.gad_application_id
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
                        Status = workFlow.gawt_application_status == 3 ? "Pending" : "Revert",//AppStatus
                             Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                        RowNum = (int)ebd.employee_id,
                        Department = deptmaster.dm_deptname_english,
                        District = distmaster.dm_name_english,

                    }).ToList().Count;


                    //sent back application
                    var sentbackapplication =
                   (from workFlow in db.tbl_GIS_application_workflow_details.AsNoTracking().Where(e => e.gawt_active_status == 1 && (e.gawt_application_status == 2))
                    join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on workFlow.gawt_application_id equals application.gad_application_id
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
                        Status = workFlow.gawt_application_status != 3 ? "Revert" : "Pending",//AppStatus
                        Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                        RowNum = (int)ebd.employee_id,
                        Department = deptmaster.dm_deptname_english,
                        District = distmaster.dm_name_english,

                    }).ToList().Count;


                    //total application
                    var totalApplication =
                   (from workFlow in db.tbl_GIS_application_workflow_details.AsNoTracking().Where(e => e.gawt_active_status == 1 )
                    join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on workFlow.gawt_application_id equals application.gad_application_id
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
                        Status = workFlow.gawt_application_status != 3 ? "Revert" : "Pending",//AppStatus
                        Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                        RowNum = (int)ebd.employee_id,
                        Department = deptmaster.dm_deptname_english,
                        District = distmaster.dm_name_english,

                    }).ToList().Count;
                    verificationDetails.TotalReceived = totalApplication;
                    verificationDetails.SentBackApplication = sentbackapplication; //EmployeeVerificationss1;
                    // verificationDetails.ForwardedApplications =0;
                    verificationDetails.PendingApplications = pendingapplication;//EmployeeVerification11;

                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "GetEmployeeDetailsForDDOVerification" + ex.Message);
                tbl_ErrorLog obj1 = new tbl_ErrorLog();
                obj1.ErrorDesc = ex.Message;
                obj1.CreatedBy = 0;
                obj1.CreatedDate = DateTime.UtcNow;
                _db.tbl_ErrorLog.Add(obj1);
                _db.SaveChanges();
            }
            return verificationDetails;
        }




        //devika
        public IList<VM_GISWorkflowDetail> GetWorkFlowDetails(long applicationId)
        {
          
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                try
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
                            join workflow1 in db.tbl_GIS_application_workflow_details.AsNoTracking().Where(a => a.gawt_active_status == 1 || a.gawt_active_status == 0) on appl.gad_application_id equals workflow1.gawt_application_id into set3
                            from workflow in set3.DefaultIfEmpty()
                            join applnstatus1 in db.tbl_application_status_master.AsNoTracking().Where(a => a.asm_active == 1) on workflow.gawt_application_status equals applnstatus1.asm_status_id into set4
                            from applnstatus in set4.DefaultIfEmpty()
                            join remarks1 in db.tbl_remarks_master.AsNoTracking().Where(a => a.RM_Active_Status == true) on workflow.gawt_remarks equals remarks1.RM_Remarks_id.ToString() into set5
                            from remarks in set5.DefaultIfEmpty()
                            select new VM_GISWorkflowDetail
                            {

                                ApplicationRefNo = appl.gad_application_no.ToString(),//dr["ApplicationRefNo"].ToString();
                                From = workflow.gawt_application_status == 1 || workflow.gawt_application_status == 2 ? "DDO" : "Applicant",//dr["From"].ToString();
                                To = workflow.gawt_application_status == 3 || workflow.gawt_application_status == 4 ? "DDO" : "Applicant",//dr["To"].ToString();


                                Remarks = remarks.RM_Remarks_Desc,//dr["Remarks"].ToString();
                                Comments = workflow.gawt_comments,//dr["Comments"].ToString();                           
                                CreationDateTime = workflow.gawt_creation_datetime,//dr["CreationDateTime"].ToString();
                                ApplicationStatus = applnstatus.asm_status_desc,

                            }).ToList();
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "GetWorkFlowDetails" + ex.Message);
                    tbl_ErrorLog obj1 = new tbl_ErrorLog();
                    obj1.ErrorDesc = ex.Message;
                    obj1.CreatedBy = 0;
                    obj1.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj1);
                    _db.SaveChanges();
                    return null;
                }
            }
        }


        #region


        public tbl_nb_declaration_master DeclarationDetailsDll(long EmployeeCode)
        {
            var DeclartaionDtls = (from n in _db.tbl_nb_declaration_master

                                   where n.ndm_declaration_code == EmployeeCode
                                   select n).FirstOrDefault();
            var RefDtls = (from RNo in _db.tbl_application_referenceno_details

                           where RNo.ard_system_emp_code == EmployeeCode
                           select RNo).FirstOrDefault();
            if (DeclartaionDtls != null && RefDtls != null)
            {
                DeclartaionDtls.ndm_referance_no = RefDtls.ard_application_reference_no;
            }

            return DeclartaionDtls;
        }

        public VM_PolicyDetails KGIDDetailsDll(long EmployeeCode)
        {
            VM_PolicyDetails objPD = new VM_PolicyDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",EmployeeCode)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectKGIDDetails");
                //if (dsPD.Tables.Count == 1)
                //{
                if (dsPD.Tables[0].Rows.Count > 0)
                {
                    objPD.employee_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["employee_id"].ToString());
                    objPD.premium_Amount = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["PremiumAmount"].ToString());
                    objPD.premium_Amount_to_Pay = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["PremiumAmount"].ToString());
                    objPD.application_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["kad_application_id"].ToString());
                    objPD.gross_pay = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["gross_pay"]);
                }
               
                if (dsPD.Tables.Count > 1)
                {
                    if (dsPD.Tables[1].Rows.Count > 0)
                    {
                        var list = dsPD.Tables[1].AsEnumerable().Select(dataRow => new VM_PolicyDetails
                        {
                            employee_id = dataRow.Field<long>("employee_id"),
                            p_kgid_policy_number = dataRow.Field<long>("p_kgid_policy_number"),
                            p_sanction_date = dataRow.Field<string>("p_sanction_date"),
                            p_premium = dataRow.Field<double?>("p_premium"),
                            application_id = dataRow.Field<long>("p_application_id")
                        }).ToList();
                        objPD.KGIDPolicyList = list;
                    }
                }
               

            }
            catch (Exception ex)
            {
                tbl_ErrorLog obj1 = new tbl_ErrorLog();
                obj1.ErrorDesc = ex.Message;
                obj1.CreatedBy = 0;
                obj1.CreatedDate = DateTime.UtcNow;
                _db.tbl_ErrorLog.Add(obj1);
                _db.SaveChanges();
            }
            return objPD;
        }
        #endregion

        public VM_ChallanPrintDetails ChallanprintDetails(long EmpId, long AppId)
        {
            // details = _Conn.ExeccuteDataset(parms, "sp_kgid_NB_Print_PaymentDetails")  sp_kgid_NB_Print_PaymentDetails;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                try
                {

                    return (from empBasic in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true && e.employee_id == (long)EmpId)
                            join appl1 in db.tbl_GIS_application_details.AsNoTracking().Where(a => a.gad_active == 1 && a.gad_application_id == AppId) on empBasic.employee_id equals appl1.gad_employee_id into set2
                            from appl in set2.DefaultIfEmpty()
                            join ChallanDetails1 in db.tbl_GIS_challan_details.AsNoTracking().Where(a => a.gcd_active_status == 1) on appl.gad_application_id equals ChallanDetails1.gcd_application_id into set3
                            from ChallanDetails in set3.DefaultIfEmpty()
                            join ChallanStatus1 in db.tbl_GIS_challan_status.AsNoTracking().Where(a => a.gcs_active_status == 1) on ChallanDetails.gcd_challan_id equals ChallanStatus1.gcs_challan_id into set4
                            from ChallanStatus in set4.DefaultIfEmpty()
                            join empwork1 in db.tbl_employee_work_details.AsNoTracking().Where(a => a.ewd_active_status == true) on empBasic.employee_id equals empwork1.ewd_emp_id into set5
                            from empwork in set5.DefaultIfEmpty()
                            join ddomaster1 in db.tbl_ddo_master.AsNoTracking().Where(a => a.dm_active == true) on empwork.ewd_ddo_id equals ddomaster1.dm_ddo_id into set6
                            from ddomaster in set6.DefaultIfEmpty()
                            //join challanStatusMaster1 in db.tbl_challan_status_master.AsNoTracking().Where(a => a.dm_active == true) on empwork.ewd_ddo_id equals ddomaster1.dm_ddo_id into set6
                            //from ddomaster in set6.DefaultIfEmpty()
                            select new VM_ChallanPrintDetails
                            {
                                dm_ddo_code = ddomaster.dm_ddo_code,
                                hoa_desc = "8011~00~105~1~01~000",
                                purpose_id = ChallanDetails.gcd_purpose_id.ToString() ,
                                purpose_desc = ChallanDetails.gcd_purpose_id == 3 ? "GIS_ Insurance Fund  " : "GIS_Savings Fund  ",
                                sub_purpose_desc = ChallanDetails.gcd_sub_purpose_id == 21 ? "Insurance Fund  " : "Savings Fund  ",
                                p_premium = ChallanDetails.gcd_amount,
                                challan_ref_no = ChallanStatus.gcs_transaction_ref_no,
                                LastUpdatedDateTime = ChallanStatus.gcs_date_of_transaction,                              
                               challan_date = ChallanStatus.gcs_date_of_transaction.ToString(),
                                //challan_status = ChallanStatus.gcs_challan_status_id,
                               ApplicationReferenceNo = appl.gad_application_no.ToString(),

                }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "ChallanprintDetails" + ex.Message);
                    tbl_ErrorLog obj1 = new tbl_ErrorLog();
                    obj1.ErrorDesc = ex.Message;
                    obj1.CreatedBy = EmpId;
                    obj1.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj1);
                    _db.SaveChanges();
                    return null;
                }
            }
        }

    
        public string GISSaveVerifiedDetails(VM_GISDeptVerificationDetails objVerification)
        {
           // returnString = _Conn.ExecuteCmd(sqlparam, "sp_kgid_insertDepartmentWorkflowVerification");
            string returnString = string.Empty;


            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                var objWFD = db.tbl_GIS_application_workflow_details.AsNoTracking().Where(e => e.gawt_application_id == (long)objVerification.ApplicationId && e.gawt_active_status == 1).FirstOrDefault();
                var objappln = db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_application_id == (long)objVerification.ApplicationId && e.gad_active == 1).FirstOrDefault();

                try
                {
                    if (objWFD != null)
                    {
                        objWFD.gawt_active_status = 0;
                        db.Entry(objWFD).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    tbl_GIS_application_workflow_details wfd = new tbl_GIS_application_workflow_details();

                    wfd.gawt_application_id = objVerification.ApplicationId;
                    wfd.gawt_InsuranceAmt = objWFD.gawt_InsuranceAmt;
                    wfd.gawt_savingAmt = objWFD.gawt_savingAmt;
                    wfd.gawt_InsuranceDate = objWFD.gawt_InsuranceDate;
                    wfd.gawt_savingDate = objWFD.gawt_savingDate;
                    wfd.gawt_verified_by = (long)objVerification.CreatedBy;
                    wfd.gawt_assigned_to = objVerification.ApplicationStatus == 2 ? objappln.gad_employee_id : 0;
                    wfd.gawt_remarks = objVerification.Remarks;
                    wfd.gawt_comments = objVerification.Comments;
                    wfd.gawt_application_status = objVerification.ApplicationStatus;
                    wfd.gawt_active_status = 1;
                    wfd.gawt_created_by = objVerification.EmpCode;
                    wfd.gawt_creation_datetime = DateTime.UtcNow;
                    wfd.gawt_updated_by = objVerification.EmpCode;
                    wfd.gawt_updation_datetime = DateTime.UtcNow;
                    _db.tbl_GIS_application_workflow_details.Add(wfd);
                    _db.SaveChanges();


                    if (objVerification.ApplicationStatus == 15)
                    {
                        // var objGisSubcription = _db.tbl_GIS_Subcription_details.AsNoTracking().Where(e => e.gpd_ApplicationId  ==objVerification.ApplicationId && e.gpd_empId == objappln.gad_employee_id).FirstOrDefault();

                        int maxpolicyNum = (int)_db.tbl_GIS_Subcription_details.Select(p => p.gpd_GISPolicyNum).DefaultIfEmpty(0).Max();
                        if (maxpolicyNum == 0)
                        { maxpolicyNum = 100001; }
                        else { maxpolicyNum = maxpolicyNum + 1; }



                        var gisSubDetails =  _db.tbl_GIS_Subcription_details.AsNoTracking().Where(e => e.gpd_empId == objappln.gad_employee_id && e.gdp_activeStatus == 1).FirstOrDefault();
                        if (gisSubDetails != null)
                        {
                            try
                            {
                                gisSubDetails.gpd_ApplicationId = objVerification.ApplicationId;
                                gisSubDetails.gpd_GISPolicyNum = maxpolicyNum;
                                gisSubDetails.gdp_updated_by = (long)objVerification.EmpCode;
                                gisSubDetails.gdp_updation_datetime = DateTime.Now;
                                _db.Entry(gisSubDetails).State = EntityState.Modified;
                                 _db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Logger.LogMessage(TracingLevel.INFO, "GISSaveVerifiedDetails:Error in updating Subcription details" + ex.Message);
                                tbl_ErrorLog obj1 = new tbl_ErrorLog();
                                obj1.ErrorDesc = ex.Message;
                                obj1.CreatedBy = objVerification.EmpCode;
                                obj1.CreatedDate = DateTime.UtcNow;
                                _db.tbl_ErrorLog.Add(obj1);
                                _db.SaveChanges();
                            }
                        }
                    }
                    returnString = "1";
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "GISSaveVerifiedDetails " + ex.Message);
                    tbl_ErrorLog obj1 = new tbl_ErrorLog();
                    obj1.ErrorDesc = ex.Message;
                    obj1.CreatedBy = objVerification.EmpCode;
                    obj1.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj1);
                    _db.SaveChanges();
                }
            }

            

            return returnString;


        }

     
        public VM_GIS_Upload_EmployeeForm GetUploadDoc(long _EmpId) // To Be Changed
        {
            var EmpUpload = (from n in _db.tbl_GIS_upload_employeeform
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
                objUD.ApplicationFormDocName = EmpUpload.App_Application_Form;
                objUD.Form6DocName = EmpUpload.App_Form6;
                objUD.Form7DocName = EmpUpload.App_Form7;
            }
            else {
                objUD.App_Employee_Code = _EmpId;
                objUD.App_ApplicationID = EmpApplId.gad_application_id != null ? EmpApplId.gad_application_id : 0;
            }
            return objUD;
        }

        // fomm verifydatacontroller n dll--aveEmployeeFormDll
        public int GISSaveEmployeeForm(tbl_GIS_upload_employeeform objEmpForm)//To Be Changed 
        {
          
            
            int Result = 0;
            var EmpRefNo = (from n in _db.tbl_GIS_application_details
                            where n.gad_employee_id == objEmpForm.App_Employee_Code
                            && n.gad_active == 1
                            select n).FirstOrDefault();
            var EmpUpload = (from n in _db.tbl_GIS_upload_employeeform
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
                        if (EmpUpload.App_Application_Form != string.Empty && objEmpForm.App_Application_Form == string.Empty)
                        {
                            objEmpForm.App_Application_Form = EmpUpload.App_Application_Form;
                        }
                        else if (EmpUpload.App_Form6 != string.Empty && objEmpForm.App_Form6 == string.Empty)
                        {
                            objEmpForm.App_Form6 = EmpUpload.App_Form6;
                        }
                        else if (EmpUpload.App_Form7 != string.Empty && objEmpForm.App_Form7 == string.Empty)
                        {
                            objEmpForm.App_Form7 = EmpUpload.App_Form7;
                        }
                        //EmpUpload.App_Application_Form = objEmpForm.App_Application_Form;
                        //EmpUpload.App_Form6 = objEmpForm.App_Form6;
                        //EmpUpload.App_Form7 = objEmpForm.App_Form7;
                        //EmpUpload.App_Creation_Date = DateTime.Now;



                        // -- 23 ADDED
                        objEmpForm.App_Active = 1;
                        objEmpForm.App_Created_By = Convert.ToInt64(objEmpForm.App_Employee_Code);
                        objEmpForm.App_Creation_Date = DateTime.Now;
                        objEmpForm.App_ApplicationID = EmpRefNo.gad_application_id;
                        _db.tbl_GIS_upload_employeeform.Add(objEmpForm);
                        _db.SaveChanges();

                        // -- 23 ADDED
                        Result = 2; //New record saved 
                    }
                    else
                    {
                        var EmpInsuredUpload = (from n in _db.tbl_GIS_upload_employeeform
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
                        _db.tbl_GIS_upload_employeeform.Add(objEmpForm);
                        _db.SaveChanges();

                        Result = 1;
                    }
                }



                else Result = 0;
            }
            catch (Exception ex)
            { Logger.LogMessage(TracingLevel.INFO, "GISSaveEmployeeForm" + ex.Message);
                tbl_ErrorLog obj1 = new tbl_ErrorLog();
                obj1.ErrorDesc = ex.Message;
                obj1.CreatedBy = Convert.ToInt64(objEmpForm.App_Employee_Code);
                obj1.CreatedDate = DateTime.UtcNow;
                _db.tbl_ErrorLog.Add(obj1);
                _db.SaveChanges();
            }
          
            return Result;
        }

        #region VIEW ALL uploaded Documents for employees
        public VM_GISDeptVerificationDetails GetUploadedDocuments(long empId, long applicationId)
        {
            // VM_DeptVerificationDetails verificationDetails = new VM_DeptVerificationDetails();
           // dsUD = _Conn.ExeccuteDataset(sqlparameters, "sp_kgid_selectEmployeeDocuments");
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                try
                {
                    return (from empBasic in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true && e.employee_id == empId)
                            join forms1 in db.tbl_GIS_upload_employeeform.AsNoTracking().Where(a => a.App_Active == 1) on empBasic.employee_id equals forms1.App_Employee_Code into set5
                            from forms in set5.DefaultIfEmpty()
                            select new VM_GISDeptVerificationDetails
                            {

                                ApplnUploaddocPath = forms.App_Application_Form,
                                ApplnUploaddocType = "Application Form",
                                Form6UploaddocPath = forms.App_Form6,
                                Form6UploaddocType = "Form 6",
                                Form7UploaddocPath = forms.App_Form7,
                                Form7UploaddocType = "Form 7",

                            }).FirstOrDefault();
                }
                catch ( Exception ex)                
                { 
                    Logger.LogMessage(TracingLevel.INFO, "GetUploadedDocuments" + ex.Message);
                    tbl_ErrorLog obj1 = new tbl_ErrorLog();
                    obj1.ErrorDesc = ex.Message;
                    obj1.CreatedBy = 0;
                    obj1.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj1);
                    _db.SaveChanges();
                    return null;
                } 
                
            }

        
        }

        public IList<KGID_Models.KGID_GroupInsurance.UploadedDocuments> GetUploadedAdoptionFile(long empId, long applicationId)
        {
            // VM_DeptVerificationDetails verificationDetails = new VM_DeptVerificationDetails();
            // dsUD = _Conn.ExeccuteDataset(sqlparameters, "sp_kgid_selectEmployeeDocuments");
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                try
                {
                    return (//from empBasic in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true && e.employee_id == empId)
                              from forms in db.tbl_GIS_nominee_details.AsNoTracking().Where(a => a.gnd_active == 1 && a.gnd_application_id == applicationId && a.gnd_emp_id == empId && (a.gnd_AdoptionFile_path != null && a.gnd_AdoptionFile_path != "")) 
                             // from forms in set5.DefaultIfEmpty()
                              select new KGID_Models.KGID_GroupInsurance.UploadedDocuments
                              {
                                  UploaddocPath = forms.gnd_AdoptionFile_path,
                                  UploaddocType = forms.gnd_name_of_nominee,

                              }).ToList();
                    
                }
                catch (Exception ex)
                {
                    Logger.LogMessage(TracingLevel.INFO, "GetUploadedDocuments" + ex.Message);
                    tbl_ErrorLog obj1 = new tbl_ErrorLog();
                    obj1.ErrorDesc = ex.Message;
                    obj1.CreatedBy = 0;
                    obj1.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj1);
                    _db.SaveChanges();
                    return null;
                }

            }


        }
        #endregion





        public VM_GISDDOVerificationDetails GISGetEmployeeApplicationStatusDll(long empId)
        {

            // "sp_kgid_getEmployeeFormDetails"
            VM_GISDDOVerificationDetails verificationDetails = new VM_GISDDOVerificationDetails();
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                try
                {
                    var empworkdetails = db.tbl_employee_work_details.AsNoTracking().Where(e => e.ewd_emp_id == empId && e.ewd_active_status == true).FirstOrDefault();
                    int ddoid = empworkdetails.ewd_ddo_id;

                    var EmployeeVerification =
                            (from gisSub in db.tbl_GIS_Subcription_details.AsNoTracking().Where(e => e.gdp_activeStatus == 1 && e.gpd_empId == empId)
                             join application in db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_active == 1) on gisSub.gpd_ApplicationId equals application.gad_application_id
                             join workFlow in db.tbl_GIS_application_workflow_details.AsNoTracking().Where(e => e.gawt_active_status == 1 && (e.gawt_application_status == 15)) on application.gad_application_id equals workFlow.gawt_application_id
                             join ebd in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true) on application.gad_employee_id equals ebd.employee_id
                             join applstatusMaster in db.tbl_application_status_master.AsNoTracking().Where(e => e.asm_active == 1) on workFlow.gawt_application_status equals applstatusMaster.asm_status_id
                             select new GISEmployeeDDOVerificationDetail
                             {

                                 EmployeeCode = ebd.employee_id,
                                 Name = ebd.employee_name,
                                 ApplicationNumber = application.gad_application_no.ToString(),
                                 ApplicationId = application.gad_application_id,
                                 Status = applstatusMaster.asm_status_desc,
                                 PolicyNumber = gisSub.gpd_GISPolicyNum.ToString(),
                                 Remarks = workFlow.gawt_comments,
                                 // Priority = ebd.first_kgid_policy_no == null ? 2 : 1,
                                 //RowNum = (int)ebd.employee_id,

                             }).ToList();
                    var IEmployeeVerificationDetails = (from empBasic in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true && e.employee_id == empId)
                                                        join empWork in db.tbl_employee_work_details.AsNoTracking().Where(v => v.ewd_active_status == true) on empBasic.employee_id equals empWork.ewd_emp_id
                                                        join groupMaster in db.tbl_employee_group_master.AsNoTracking().Where(s => s.eg_active == true) on empWork.ewd_group_id equals groupMaster.eg_group_id
                                                        join dept in db.tbl_department_master.AsNoTracking().Where(s => s.dm_active == true) on empBasic.dept_employee_code equals dept.dm_dept_id                     
                                                        join GisSubscription in db.tbl_GIS_Subcription_details.AsNoTracking().Where(v => v.gdp_activeStatus == 1) on empBasic.employee_id equals GisSubscription.gpd_empId                                                                                                                                                          //from Pab in su.DefaultIfEmpty()
                                                        join designationmaster in db.tbl_designation_master.AsNoTracking().Where(a => a.d_status == 1) on empWork.ewd_designation_id equals designationmaster.d_designation_id
                                                        join cd in db.tbl_GIS_challan_details.AsNoTracking().Where(a => a.gcd_active_status == 1) on empBasic.employee_id equals cd.gcd_emp_id

                                                        select new GISEmployeeDDOVerificationDetail
                                                        {
                                                            employeename = empBasic.employee_name,
                                                           deptName=dept.dm_deptname_english,
                                                            SubscriptionDate = GisSubscription.gdp_updation_datetime,
                                                            SavingInsuranceAmt = cd.gcd_amount,
                                                            groupDesc = groupMaster.eg_group_desc,
                                                            designation = designationmaster.d_designation_desc,
                                                            currentdate = DateTime.UtcNow,
                                                            subcriptionNumber= GisSubscription.gpd_GISPolicyNum,


                                                        }).ToList();
                    verificationDetails.IEmployeeVerificationDetails = IEmployeeVerificationDetails;
                    verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                }
                catch (Exception ex)
                { Logger.LogMessage(TracingLevel.INFO, " GIS Get Employee ApplicationStatus " + ex.Message);
                    tbl_ErrorLog obj1 = new tbl_ErrorLog();
                    obj1.ErrorDesc = ex.Message;
                    obj1.CreatedBy = 0;
                    obj1.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj1);
                    _db.SaveChanges();
                }
                return verificationDetails;
            }
            
        }



        public VM_GIS_Upload_EmployeeForm GetUploadDocDll(long _EmpId) // To Be Changed  -- changed mmm
        {
            VM_GIS_Upload_EmployeeForm objUD=null;
            try
            {
                var EmpUpload = (from n in _db.tbl_GIS_upload_employeeform
                                 where n.App_Employee_Code == _EmpId && n.App_Active == 1
                                 select n).FirstOrDefault();
                 objUD = new VM_GIS_Upload_EmployeeForm();
                if (EmpUpload != null)
                {
                    objUD.App_Employee_Code = EmpUpload.App_Employee_Code;
                    objUD.ApplicationFormDocName = EmpUpload.App_Application_Form;
                    objUD.Form6DocName = EmpUpload.App_Form6;
                    objUD.Form7DocName = EmpUpload.App_Form7;
                }
            }
            catch(Exception ex) {
                tbl_ErrorLog obj1 = new tbl_ErrorLog();
                obj1.ErrorDesc = ex.Message;
                obj1.CreatedBy = 0;
                obj1.CreatedDate = DateTime.UtcNow;
                _db.tbl_ErrorLog.Add(obj1);
                _db.SaveChanges();
            }
            return objUD;
        }


        //Logger
        public void SaveLogs(string exceptionDesc,long CreatedBy)
        {
            int result = 1;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                try
                {
                    tbl_ErrorLog obj1 = new tbl_ErrorLog();
                    obj1.ErrorDesc = exceptionDesc;
                    obj1.CreatedBy = CreatedBy;
                    obj1.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ErrorLog.Add(obj1);
                    _db.SaveChanges();
                    result = 1;
                }
                catch (Exception ex)
                {
                  
                    Logger.LogMessage(TracingLevel.INFO, "Error in saving Log" + ex.Message);
                    result = 0;
                }
            }
          //  return result;
        }

        //  KUNAL

        public GIS_CliamDetails GISProposerDetailsBll(long employeeCode, string PageType, long RefNo, int Category)
        {

            GIS_CliamDetails objBD = new GIS_CliamDetails();
            objBD.gis_pagetype = PageType;
            try
            {
                DataSet dsBD = new DataSet();
                if (PageType == "Emp" || PageType == "EmpRenewal")
                {
                    SqlParameter[] sqlparam =
                 {
                    new SqlParameter("@employee_id",employeeCode),
                    new SqlParameter("@PageType",PageType=="EmpRenewal"?"Renewal":""),
                     new SqlParameter("@RefNo",RefNo)
                 };
                    dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectGISProposerDetailsEMP");
                }
                else
                {
                    SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",employeeCode),
                    new SqlParameter("@PageType",(PageType=="Renewal"?"Renewal":PageType)),
                     new SqlParameter("@RefNo",RefNo),
                      new SqlParameter("@Category",Category)
                };
                    dsBD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_selectGISProposerDetails");
                }

                if (dsBD.Tables[0].Rows.Count > 0)
                {
                    objBD.employee_id = Convert.ToInt32(dsBD.Tables[0].Rows[0]["employee_id"]);
                    objBD.employee_name = dsBD.Tables[0].Rows[0]["employee_name"].ToString();
                    objBD.department = dsBD.Tables[0].Rows[0]["Department"].ToString();
                    objBD.gis_address = dsBD.Tables[0].Rows[0]["ead_address"].ToString();
                    objBD.gis_pincode = Convert.ToInt32(dsBD.Tables[0].Rows[0]["ead_pincode"]);
                    objBD.mobile_number = Convert.ToInt64(dsBD.Tables[0].Rows[0]["mobile_number"]);
                    objBD.gis_occupation = dsBD.Tables[0].Rows[0]["d_designation_desc"].ToString();
                    objBD.gis_email = dsBD.Tables[0].Rows[0]["email_id"].ToString();

                    objBD.PolicyMonths = dsBD.Tables[0].Rows[0]["PolicyMonths"].ToString();
                    var TypeofCoverList = (from n in _db.tbl_motor_insurance_type_of_cover
                                           select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
                                                              ).ToList();
                    objBD.gis_type_of_cover = dsBD.Tables[0].Rows[0]["type_of_cover"].ToString();

                    objBD.gis_fax_no = Convert.ToInt64((dsBD.Tables[0].Rows[0]["dm_fax_no"] == DBNull.Value) ? 0 : dsBD.Tables[0].Rows[0]["dm_fax_no"]);

                    if (PageType == "EditRenewal" || PageType == "ViewRenewal" || PageType == "Renewal")
                    {
                        objBD.gis_kgid_application_number = dsBD.Tables[0].Rows[0]["mira_renewal_application_ref_no"].ToString();
                        objBD.gis_kgid_renewal_application_number = dsBD.Tables[0].Rows[0]["mira_renewal_application_ref_no"].ToString();
                    }
                    else
                    {
                        objBD.gis_kgid_application_number = dsBD.Tables[0].Rows[0]["mia_application_ref_no"].ToString();
                    }

                    objBD.gis_old_application_Ref_number = RefNo;
                    objBD.gis_pagetype = (PageType == "EmpRenewal" ? "Renewal" : PageType);

                }

            }
            catch (Exception ex)
            {

            }
            return objBD;
        }

        public long SaveGISProposalAppnRefNo(GIS_CliamDetails objPD)
        {
            long result = 0;
            string RefNo = "";
            try
            {
                if (!String.IsNullOrWhiteSpace(objPD.gis_kgid_application_number))
                {
                    RefNo = Convert.ToString(objPD.gis_kgid_application_number);
                }
                else
                {
                    RefNo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                }

                SqlParameter[] sqlparam =
                {
                        new SqlParameter("@reference_no",RefNo),
                        new SqlParameter("@employee_id",objPD.gis_employee_id),
                        new SqlParameter("@PageType",(objPD.gis_pagetype==null?"":objPD.gis_pagetype)),
                        new SqlParameter("@Category",objPD.gis_category),
                        new SqlParameter("@adr",objPD.gis_address),
                        new SqlParameter("@email",objPD.gis_email),
                        new SqlParameter("@faxno",objPD.gis_fax_no),
                        new SqlParameter("@pincode",objPD.gis_pincode),

                };
                result = Convert.ToInt64(_Conn.ExecuteCmd(sqlparam, "sp_kgid_saveGISReferenceNo"));

            }
            catch (Exception ex)
            {

            }
            return result;
        }
        // 20-11-2021
        public GIS_Ledger GetEMPGISLedgerDll(string EmpId)
        {
            GIS_Ledger obj = new GIS_Ledger();
            DataSet dsPS = new DataSet();
            if (EmpId is null)
            {
                EmpId = "00000";
            }
            SqlParameter[] parms = {
            new SqlParameter("@P_EMP_ID",EmpId),
            };

            dsPS = _Conn.ExeccuteDataset(parms, "sp_get_gis_ledger");
            if (dsPS.Tables[0].Rows.Count > 0)
            {
                obj.employee_id = Convert.ToInt32(dsPS.Tables[0].Rows[0]["employee_id"].ToString());
                obj.employee_name = dsPS.Tables[0].Rows[0]["employee_name"].ToString();
            }

            if (dsPS.Tables[1].Rows.Count > 0)
            {
                var list = dsPS.Tables[1].AsEnumerable().Select(dataRow => new GIS_Ledger_det
                {
                    SR = dataRow.Field<decimal>("SR"),
                    MNYEAR = dataRow.Field<string>("MNYEAR"),
                    EMPCODE = dataRow.Field<string>("EMP_CODE"),
                    CURR_OPN = dataRow.Field<decimal>("CURR_OPN"),
                    CONT_GIS = dataRow.Field<decimal>("CONT_GIS"),
                    CURR_CLS = dataRow.Field<decimal>("CURR_CLS"),
                    INT_RATE = dataRow.Field<decimal>("INT_RATE"),
                    MON = dataRow.Field<decimal>("MON"),
                    YR = dataRow.Field<decimal>("YR"),
                    MON_INT_AMT = dataRow.Field<decimal>("MON_INT_AMT"),
                    INT_AMT = dataRow.Field<decimal>("INT_AMT"),

                }).ToList();
                obj.listgisledgerdet = list;

            }



            return obj;
        }


        //  DD
        //Get employee Ledger Deatils
        public IList<VM_ClaimLedger> GetEmployeeLedgerDeatils(long empIdKGID)
        {

            // "sp_kgid_getEmployeeFormDetails"
        //    VM_ClaimLedger LedgerDeatils = new VM_ClaimLedger();
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                try
                {
                    var empworkdetails = db.tbl_employee_basic_details.AsNoTracking().Where(e => e.first_kgid_policy_no == empIdKGID && e.active_status == true).FirstOrDefault();
                  //  int ddoid = empworkdetails.ewd_ddo_id;

                  //  var EmployeeVerification =
                     return       (from emp in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.first_kgid_policy_no == empIdKGID && e.active_status == true)
                             join gisfund in db.TBL_GIS_FUND_MONTHWISE.AsNoTracking().Where(e => e.gfm_active_Status == 1) on emp.employee_id equals gisfund.employee_id
                             select new VM_ClaimLedger
                             {

                                 LedgerId=gisfund.LedgerId,
                               employee_id =gisfund.employee_id,
                               YR=gisfund.YR,
                               MON=gisfund.MON,
                               SAVING_FUND=gisfund.SAVING_FUND,
                               INSURANCE_FUND=gisfund.INSURANCE_FUND,
                              Total= (int)gisfund.Total,
                                grp = gisfund.grp,
                                 remark=gisfund.remark,

                             }).ToList();
                  //  VM_ClaimLedger.GIS_Ledger_det = EmployeeVerification;
                }
                catch (Exception ex)
                { Logger.LogMessage(TracingLevel.INFO, " GIS Get Employee ApplicationStatus " + ex.Message);return null; }
                
                //return (IList<VM_ClaimLedger>)LedgerDeatils;
            }

        }

        public  IList<tbl_employee_group_master> GetGroup_Masters()
        {
            return (from relations in _db.tbl_employee_group_master
                    select relations).ToList();
        }

        public  IList<tbl_year_master> GetYear_Masters()
        {
            return (from relations in _db.tbl_year_master
                    select relations).ToList();
        }

        public  IList<tbl_month_master> GetMonth_Masters()
        {
            return (from relations in _db.tbl_month_master
                    select relations).ToList();

        }

        //


        public  bool AddLedgerDetails(IList<VM_ClaimLedger> LedgerDeatils)
        {
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                
                    if (LedgerDeatils != null && LedgerDeatils.Count > 0)
                    {
                    List<TBL_GIS_FUND_MONTHWISE> LedgerDeatilsss = (List<TBL_GIS_FUND_MONTHWISE>)LedgerDeatils.Select(s => new TBL_GIS_FUND_MONTHWISE
                    {
                        YR = (int)s.YR,
                        MON = (int)s.MON,
                        SAVING_FUND = (decimal)s.SAVING_FUND,
                        INSURANCE_FUND = (decimal)s.INSURANCE_FUND,
                        Total = (decimal)s.Total,
                        grp = s.grp.ToString(),
                        remark = s.remark,
                        gfm_created_by = 1,
                        gfm_creation_datetime =DateTime.UtcNow,
                        gfm_updated_by = 1,
                        gfm_updation_datetime = DateTime.UtcNow,
                        gfm_active_Status =1,
                        employee_id=s.employee_id,

                    }).ToList();
                    db.TBL_GIS_FUND_MONTHWISE.AddRange(LedgerDeatilsss);

                    db.SaveChanges();
                    }
                   
              
                return true;

            }
        }

        public bool UpdateLedgerDetails(IList<VM_ClaimLedger> LedgerDeatils)
        {
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
               
                if (LedgerDeatils != null && LedgerDeatils.Count > 0)
                {
                    foreach (var s in LedgerDeatils)
                    {
                        if (s.LedgerId != 0)
                        {
                            TBL_GIS_FUND_MONTHWISE obj = db.TBL_GIS_FUND_MONTHWISE.Where(d => d.LedgerId == s.LedgerId).FirstOrDefault();
                            if (obj != null)
                            {

                                obj.MON = (int)s.MON;
                                obj.YR = (int)s.YR;
                                obj.grp = s.grp;
                                obj.SAVING_FUND = (decimal)s.SAVING_FUND;
                                obj.INSURANCE_FUND = (decimal)s.INSURANCE_FUND;
                                obj.Total = (decimal)s.Total;
                                obj.remark = s.remark;
                                obj.gfm_active_Status = 1;
                                obj.gfm_created_by = 1;
                                obj.gfm_updated_by = 1;
                                obj.gfm_updation_datetime = DateTime.UtcNow;
                                obj.gfm_creation_datetime = DateTime.UtcNow;

                            }
                            db.SaveChanges();

                        }
                        else
                        {
                            TBL_GIS_FUND_MONTHWISE obj1 = new TBL_GIS_FUND_MONTHWISE

                            {
                                MON = (int)s.MON,
                                YR = (int)s.YR,
                                grp = s.grp,
                                SAVING_FUND = (decimal)s.SAVING_FUND,
                                INSURANCE_FUND = (decimal)s.INSURANCE_FUND,
                                Total = (decimal)s.Total,
                                remark = s.remark,
                                gfm_active_Status = 1,
                                gfm_created_by = 1,
                                gfm_updated_by = 1,
                                gfm_updation_datetime = DateTime.UtcNow,
                                gfm_creation_datetime = DateTime.UtcNow,
                                employee_id=s.employee_id,
                            };
                            db.TBL_GIS_FUND_MONTHWISE.Add(obj1);
                            db.SaveChanges();

                        }
                    }
                }

                return true;

            }
        }

        public  long GetEmployeeDetailsByKgid(long kgidNum)
        {
            long empid = 0;
            empid = (from emp in _db.tbl_employee_basic_details
                                     where emp.first_kgid_policy_no == kgidNum
                                     select emp.employee_id).FirstOrDefault();
            return empid;
        }

        public async Task<long> GISSaveNomineeBankDetails(VM_NomineeDetail objNomineeBank)
        {
            long gndapplicationid = 0;
            long result = 1;
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                //var objAppD = await db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_employee_id == (long)objNomineeBank.EmpId && e.gad_active == 1).FirstOrDefaultAsync();
                //if (objAppD != null)
                //    gndapplicationid = objAppD.gad_application_id;

                if (objNomineeBank.Id != 0)
                {
                    try
                    {
                        tbl_GIS_NomineeBankDetails editNom = await _db.tbl_GIS_NomineeBankDetails.AsNoTracking().Where(e => (long)e.gnd_nominee_id == (long)objNomineeBank.Id).FirstOrDefaultAsync();
                        editNom.gnd_relation_id = int.Parse(objNomineeBank.Relation);
                        editNom.gnd_name_of_nominee = objNomineeBank.NameOfNominee;
                        editNom.gnd_percentage_of_share = (int)objNomineeBank.PercentageShare;
                        editNom.gnd_updation_datetime = DateTime.Now;
                        editNom.gnd_updated_by = (long)objNomineeBank.EmpId;
                        editNom.gnd_Nominee_age = objNomineeBank.Age != null ? objNomineeBank.Age : 0;
                        if (objNomineeBank.Relation != "14") { editNom.gnd_nomineeDob = objNomineeBank.nomineeDOB; }
                        editNom.gnd_contingencies = objNomineeBank.gnd_contingencies;
                        editNom.gnd_predeceasing = objNomineeBank.gnd_predeceasing;
                        if (objNomineeBank.SonDaughterAdoption_doc_path != null || objNomineeBank.SonDaughterAdoption_doc_path != "")
                        { editNom.gnd_AdoptionFile_path = objNomineeBank.SonDaughterAdoption_doc_path; }
                        editNom.gnd_bank_account_number = objNomineeBank.gnd_bank_account_number;
                        editNom.gnd_ifsc = objNomineeBank.gnd_ifsc;
                        editNom.gnd_micr = objNomineeBank.gnd_micr;
                        _db.Entry(editNom).State = EntityState.Modified;
                        await _db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "GISSaveNomineeBankDetails: error in saving nominee -- add bank details" + ex.Message);
                        result = 0;
                    }
                }
                else
                {
                    try
                    {
                        tbl_GIS_NomineeBankDetails objBD = new tbl_GIS_NomineeBankDetails();
                        objBD.gnd_emp_id = (long)objNomineeBank.EmpId;
                      //  objBD.gnd_application_id = gndapplicationid;
                        objBD.gnd_relation_id = int.Parse(objNomineeBank.Relation);
                        objBD.gnd_name_of_nominee = objNomineeBank.NameOfNominee;
                        objBD.gnd_percentage_of_share = objNomineeBank.PercentageShare != null ? (int)objNomineeBank.PercentageShare : 0;
                        objBD.gnd_active = 1;
                        objBD.gnd_creation_datetime = DateTime.Now;
                        objBD.gnd_created_by = (long)objNomineeBank.EmpId;
                        objBD.gnd_updation_datetime = DateTime.Now;
                        objBD.gnd_updated_by = (long)objNomineeBank.EmpId;
                        objBD.gnd_Nominee_age = objNomineeBank.Age;
                        if (objNomineeBank.Relation != "14") { objBD.gnd_nomineeDob = objNomineeBank.nomineeDOB; }

                        objBD.gnd_contingencies = objNomineeBank.gnd_contingencies != null ? objNomineeBank.gnd_contingencies : null;
                        objBD.gnd_predeceasing = objNomineeBank.gnd_predeceasing != null ? objNomineeBank.gnd_predeceasing : null;

                        if (objNomineeBank.SonDaughterAdoption_doc_path != null || objNomineeBank.SonDaughterAdoption_doc_path != "")
                        { objBD.gnd_AdoptionFile_path = objNomineeBank.SonDaughterAdoption_doc_path; }

                        objBD.gnd_bank_account_number = objNomineeBank.gnd_bank_account_number != null ? objNomineeBank.gnd_bank_account_number : null;
                        objBD.gnd_ifsc= objNomineeBank.gnd_ifsc != null ? objNomineeBank.gnd_ifsc : null;
                        objBD.gnd_micr = objNomineeBank.gnd_micr != null ? objNomineeBank.gnd_micr : null;
                        _db.tbl_GIS_NomineeBankDetails.Add(objBD);
                        await _db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMessage(TracingLevel.INFO, "GISSaveNomineeBankDetails: error in saving nominee--  update bank details" + ex.Message);
                        result = 0;
                    }
                }
            }

            return result;
        }

        public VM_GISDDOVerificationDetails GISGetSubscriptionDetails(long empid)
        {
            VM_GISDDOVerificationDetails verificationDetails = new VM_GISDDOVerificationDetails();
            using (DbConnectionKGID db = new DbConnectionKGID())
            {
                var IEmployeeVerificationDetails = (from empBasic in db.tbl_employee_basic_details.AsNoTracking().Where(e => e.active_status == true && e.employee_id == empid)
                        join empWork in db.tbl_employee_work_details.AsNoTracking().Where(v => v.ewd_active_status == true) on empBasic.employee_id equals empWork.ewd_emp_id
                        join groupMaster in db.tbl_employee_group_master.AsNoTracking().Where(s => s.eg_active == true) on empWork.ewd_group_id equals groupMaster.eg_group_id
                        //join dept in db.tbl_department_master.AsNoTracking().Where(s => s.dm_active == true) on empBasic.dept_employee_code.ToString() equals dept.dm_deptcode                        
                        join GisSubscription in db.tbl_GIS_Subcription_details.AsNoTracking().Where(v => v.gdp_activeStatus == 1) on empBasic.employee_id equals GisSubscription.gpd_empId                                                                                                                                                          //from Pab in su.DefaultIfEmpty()
                        join designationmaster in db.tbl_designation_master.AsNoTracking().Where(a => a.d_status == 1) on empWork.ewd_designation_id equals designationmaster.d_designation_id                        
                        join cd in db.tbl_GIS_challan_details.AsNoTracking().Where(a => a.gcd_active_status == 1) on empBasic.employee_id equals cd.gcd_emp_id

                        select new GISEmployeeDDOVerificationDetail
                        {
                            employeename = empBasic.employee_name,
                           // deptName=dept.dm_deptname_english,
                            SubscriptionDate = GisSubscription.gdp_updation_datetime,
                            SavingInsuranceAmt= cd.gcd_amount,
                            groupDesc = groupMaster.eg_group_desc,


                        }).ToList();
                verificationDetails.IEmployeeVerificationDetails = IEmployeeVerificationDetails;
                return verificationDetails;
            }
        }


        //public async Task<long> GISSaveNomineeBankDetails(VM_NomineeDetail objNomineeBank)
        //{
        //    long gndapplicationid = 0;
        //    long result = 1;
        //    using (DbConnectionKGID db = new DbConnectionKGID())
        //    {
        //        //var objAppD = await db.tbl_GIS_application_details.AsNoTracking().Where(e => e.gad_employee_id == (long)objNomineeBank.EmpId && e.gad_active == 1).FirstOrDefaultAsync();
        //        //if (objAppD != null)
        //        //    gndapplicationid = objAppD.gad_application_id;

        //        if (objNomineeBank.gnbd_NomineeId != 0)
        //        {
        //            try
        //            {
        //                tbl_GIS_NomineeBankDetails editNom = await _db.tbl_GIS_NomineeBankDetails.AsNoTracking().Where(e => (long)e.gnbd_NomineeId == (long)objNomineeBank.gnbd_NomineeId).FirstOrDefaultAsync();
        //                editNom.gnbd_employee_id = objNomineeBank.gnbd_employee_id;
        //                editNom.gnbd_NameofNomine = objNomineeBank.gnbd_NameofNomine;
        //                editNom.gnbd_bank_account_number = objNomineeBank.gnbd_ifsc;
        //                editNom.gnbd_ifsc = objNomineeBank.gnbd_ifsc;
        //                editNom.gnbd_micr = objNomineeBank.gnbd_micr;
        //                editNom.gnbd_status = objNomineeBank.gnbd_status;
        //                editNom.gnbd_creation_datetime = objNomineeBank.gnbd_creation_datetime;
        //                editNom.gnbd_updation_datetime = objNomineeBank.gnbd_updation_datetime;
        //                editNom.gnbd_created_by = objNomineeBank.gnbd_created_by;
        //                editNom.gnbd_updated_by = objNomineeBank.gnbd_updated_by;
        //                editNom.gnbd_application_id = objNomineeBank.gnbd_application_id;
        //                _db.Entry(editNom).State = EntityState.Modified;
        //                await _db.SaveChangesAsync();
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.LogMessage(TracingLevel.INFO, "GISSaveNomineeBankDetails: error in saving nominee -- add bank details" + ex.Message);
        //                result = 0;
        //            }
        //        }
        //        else
        //        {
        //            try
        //            {
        //                tbl_GIS_NomineeBankDetails objNBD = new tbl_GIS_NomineeBankDetails();
        //                objNBD.gnbd_employee_id = objNomineeBank.gnbd_employee_id;
        //                objNBD.gnbd_NameofNomine = objNomineeBank.gnbd_NameofNomine;
        //                objNBD.gnbd_bank_account_number = objNomineeBank.gnbd_ifsc;
        //                objNBD.gnbd_ifsc = objNomineeBank.gnbd_ifsc;
        //                objNBD.gnbd_micr = objNomineeBank.gnbd_micr;
        //                objNBD.gnbd_status = objNomineeBank.gnbd_status;
        //                objNBD.gnbd_creation_datetime = objNomineeBank.gnbd_creation_datetime;
        //                objNBD.gnbd_updation_datetime = objNomineeBank.gnbd_updation_datetime;
        //                objNBD.gnbd_created_by = objNomineeBank.gnbd_created_by;
        //                objNBD.gnbd_updated_by = objNomineeBank.gnbd_updated_by;
        //                objNBD.gnbd_application_id = objNomineeBank.gnbd_application_id;

        //                _db.tbl_GIS_NomineeBankDetails.Add(objNBD);
        //                await _db.SaveChangesAsync();
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.LogMessage(TracingLevel.INFO, "GISSaveNomineeBankDetails: error in saving nominee--  update bank details" + ex.Message);
        //                result = 0;
        //            }
        //        }
        //    }

        //    return result;
        //}

    }
}
