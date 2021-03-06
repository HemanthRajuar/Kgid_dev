using Common;
using DLL.DBConnection;
using KGID_Models.KGID_Verification;
using KGID_Models.KGIDMotorInsurance;
using KGID_Models.Ticketing_Tool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using _ExcelSheet = Microsoft.Office.Interop.Excel;

namespace DLL
{
    public class TicketingToolDLL : ITicketingToolDLL
    {
        private readonly Common_Connection _Conn = new Common_Connection();
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly AllCommon _commnobj = new AllCommon();

        public IEnumerable<tbl_module_type_master> GetModuleListDLL()
        {
            List<tbl_module_type_master> types = new List<tbl_module_type_master>();

            types = (from t in _db.tbl_module_type_master
                     where t.mt_status == true
                     select (new tbl_module_type_master { mt_desc = t.mt_desc, mt_module_type = t.mt_module_type })).ToList<tbl_module_type_master>();

            return types;
        }
        public IEnumerable<tbl_problem_type_master> GetProblemTypeListDLL()
        {
            List<tbl_problem_type_master> types = new List<tbl_problem_type_master>();

            types = (from t in _db.tbl_problem_type_master

                     select (new tbl_problem_type_master { pr_description = t.pr_description, pt_id = t.pt_id })).ToList<tbl_problem_type_master>();

            return types;
        }
        public class DetailView
        {
            public string SiteName { get; set; }
            public string ItemType { get; set; }
            public string AssetStorage { get; set; }


        }
        public DataSet CreateDataTable1()
        {
            long EmpID = 0;
            TTReportProblem ApplicationDetails = new TTReportProblem();
            DataSet dsMBList = new DataSet();
            try
            {
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@rp_empid",EmpID),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetExcelDownloadData");
            }
            catch (Exception ex)
            {

            }
            return dsMBList;
        }
        public DataTable CreateDataTable(string className)
        {
            DataTable dt = new System.Data.DataTable();
            Type classtype = GetType();
            if (className == "TicketingToolReportProblem")
            {
                classtype = typeof(TicketingToolReportProblem);
            }

            PropertyInfo[] properties = classtype.GetProperties();
            // TTReportProblem tTReport1 = new TTReportProblem();

            // var properties1 = tTReport1.TicketingToolReportProblemlist.GetType();


            foreach (System.Reflection.PropertyInfo pi in properties)
            {
                //if (className == "VM_FamilyDetail" && pi.Name != "EditDeleteStatus" && pi.Name != "AppliactionSentBack" && pi.Name != "ApplicationInsured")
                //    dt.Columns.Add(pi.Name);
                //else if (className != "VM_FamilyDetail")
                dt.Columns.Add(pi.Name);
            }
            //DataRow dr=null ;
            //dt.Rows.Add(dr);
            //DetailView fieldsInst = new DetailView();
            //// Get the type of DetailView.

            //Type fieldsType = typeof(tTReport1.TicketingToolReportProblemlist);

            //PropertyInfo[] props = fieldsType.GetProperties(BindingFlags.Public
            //    | BindingFlags.Instance);


            //for (int i = 0; i < props.Length; i++)
            //{
            //    Console.WriteLine("   {0}",
            //        props[i].Name);
            //}

            return dt;
        }
        public TTReportProblem GetAllReportedProblemsDLL()
        {
            long EmpID = 0;
            TTReportProblem ApplicationDetails = new TTReportProblem();
            try
            {
                DataSet dsMBList = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@rp_empid",EmpID),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetAllReportedProblems");
                if (dsMBList.Tables[0].Rows.Count > 0)
                {
                    var MBList = dsMBList.Tables[0].AsEnumerable().Select(dataRow => new TicketingToolReportProblem
                    {
                        // RowNumber = dataRow.Field<long>("RowNumber"),
                        rp_id = dataRow.Field<long>("rp_id"),
                        rp_ticket_no = dataRow.Field<string>("rp_ticket_no"),
                        //rp_empid = dataRow.Field<long>("rp_empid"),
                        mt_desc = dataRow.Field<string>("mt_desc"),
                        pr_description = dataRow.Field<string>("pr_description"),
                        //rp_complaint_description = dataRow.Field<string>("rp_complaint_description"),
                        //rp_upload_document = dataRow.Field<string>("rp_upload_document"),
                        rp_report_problem_status = dataRow.Field<string>("rp_report_problem_status"),
                        //rp_date_of_submission = dataRow.Field<DateTime>("rp_date_of_submission"),
                        //rp_date_of_resolve = dataRow["rp_date_of_resolve"].ToString()!=""? DateTime.Parse(dataRow["rp_date_of_resolve"].ToString()).ToString("dd/MM/yyyy"):"",
                        rp_remarks = dataRow.Field<string>("rp_remarks"),
                        rp_status = dataRow.Field<bool>("rp_status"),
                        SubmissionDate = dataRow.Field<string>("SubmissionDate"),
                        ResolveDate = dataRow.Field<string>("ResolveDate"),
                        //rp_creation_datetime = dataRow.Field<DateTime>("rp_creation_datetime"),
                        //rp_updation_datetime = dataRow.Field<DateTime>("rp_updation_datetime"),
                        //rp_created_by = dataRow.Field<long>("rp_created_by"),
                        //rp_updated_by = dataRow.Field<long>("rp_updated_by")
                        AssignedTo = dataRow.Field<string>("al_agency_user_id")
                    }).ToList();
                    ApplicationDetails.TicketingToolReportProblemlist = MBList;
                    DataTable st = new DataTable();
                    CreateDataTable("TicketingToolReportProblem");
                }


            }
            catch (Exception ex)
            {

            }
            return ApplicationDetails;
        }

        public TTReportProblem GetDetailsByEmpIdDll(long EmpID, string emptype)
        {
            TTReportProblem ApplicationDetails = new TTReportProblem();
            try
            {
                DataSet dsMBList = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",EmpID),
                    new SqlParameter("@type",emptype),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetDetailsByEmpId");
                if (dsMBList.Tables[0].Rows.Count > 0)
                {
                    var MBList = dsMBList.Tables[0].AsEnumerable().Select(dataRow => new TicketingToolReportProblem
                    {
                        //RowNumber = dataRow.Field<long>("RowNumber"),
                        rp_id = dataRow.Field<long>("rp_id"),
                        rp_ticket_no = dataRow.Field<string>("rp_ticket_no"),
                        //rp_empid = dataRow.Field<long>("rp_empid"),
                        mt_desc = dataRow.Field<string>("mt_desc"),
                        pr_description = dataRow.Field<string>("pr_description"),
                        //rp_complaint_description = dataRow.Field<string>("rp_complaint_description"),
                        //rp_upload_document = dataRow.Field<string>("rp_upload_document"),
                        rp_report_problem_status = dataRow.Field<string>("rp_report_problem_status"),
                        //rp_date_of_submission = dataRow.Field<DateTime>("rp_date_of_submission"),
                        //rp_date_of_resolve = dataRow["rp_date_of_resolve"].ToString() != "" ? DateTime.Parse(dataRow["rp_date_of_resolve"].ToString()).ToString("dd/MM/yyyy") : "",
                        rp_remarks = dataRow.Field<string>("rp_remarks"),
                        rp_status = dataRow.Field<bool>("rp_status"),
                        UTYPE = dataRow.Field<int>("UTYPE"),
                        SubmissionDate = dataRow.Field<string>("SubmissionDate"),
                        ResolveDate = dataRow.Field<string>("ResolveDate"),
                        //rp_creation_datetime = dataRow.Field<DateTime>("rp_creation_datetime"),
                        //rp_updation_datetime = dataRow.Field<DateTime>("rp_updation_datetime"),
                        //rp_created_by = dataRow.Field<long>("rp_created_by"),
                        //rp_updated_by = dataRow.Field<long>("rp_updated_by")
                    }).ToList();
                    ApplicationDetails.TicketingToolReportProblemlist = MBList;
                }


            }
            catch (Exception ex)
            {

            }
            return ApplicationDetails;
        }
        public TTReportProblem GetDetailsByIdDll1(int ID)
        {
            TTReportProblem ApplicationDetails = new TTReportProblem();
            try
            {
                DataSet dsMBList = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@Id",ID),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetDetailsById");
                if (dsMBList.Tables[0].Rows.Count > 0)
                {
                    var MBList = dsMBList.Tables[0].AsEnumerable().Select(dataRow => new TicketingToolReportProblem
                    {
                        //RowNumber = dataRow.Field<long>("RowNumber"),

                        rp_id = dataRow.Field<long>("rp_id"),
                        rp_ticket_no = dataRow.Field<string>("rp_ticket_no"),
                        //rp_empid = dataRow.Field<long>("rp_empid"),
                        rp_module_id = dataRow.Field<int>("rp_module_id"),
                        rp_problem_type_id = dataRow.Field<int>("rp_problem_type_id"),
                        rp_complaint_description = dataRow.Field<string>("rp_complaint_description"),
                        rp_upload_document = dataRow.Field<string>("rp_upload_document"),
                        rp_report_problem_status = dataRow.Field<string>("rp_report_problem_status"),
                        rp_date_of_submission = dataRow.Field<DateTime>("rp_date_of_submission"),
                        //rp_date_of_resolve = dataRow.Field<DateTime>("rp_date_of_resolve"),
                        rp_remarks = dataRow.Field<string>("rp_remarks"),
                        //rp_status = dataRow.Field<bool>("rp_status"),
                        //rp_creation_datetime = dataRow.Field<DateTime>("rp_creation_datetime"),
                        //rp_updation_datetime = dataRow.Field<DateTime>("rp_updation_datetime"),
                        //rp_created_by = dataRow.Field<long>("rp_created_by"),
                        //rp_updated_by = dataRow.Field<long>("rp_updated_by")
                    }).ToList();
                    ApplicationDetails.TicketingToolReportProblemlist = MBList;
                }


            }
            catch (Exception ex)
            {

            }
            return ApplicationDetails;
        }
        public tbl_report_problem GetDetailsByIdDll(int? ID)
        {
            tbl_report_problem ApplicationDetails = new tbl_report_problem();
            try
            {
                DataSet dsMBList = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@Id",ID),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetDetailsById");
                if (dsMBList.Tables[0].Rows.Count > 0)
                {
                    var MBList = dsMBList.Tables[0].AsEnumerable().Select(dataRow => new tbl_report_problem
                    {
                        //RowNumber = dataRow.Field<long>("RowNumber"),
                        rp_id = dataRow.Field<long>("rp_id"),
                        rp_ticket_no = dataRow.Field<string>("rp_ticket_no"),
                        //rp_empid = dataRow.Field<long>("rp_empid"),
                        rp_module_id = dataRow.Field<int>("rp_module_id"),
                        rp_problem_type_id = dataRow.Field<int>("rp_problem_type_id"),
                        rp_complaint_description = dataRow.Field<string>("rp_complaint_description"),
                        rp_upload_document = dataRow.Field<string>("rp_upload_document"),
                        rp_report_problem_status = dataRow.Field<string>("rp_report_problem_status"),
                        rp_date_of_submission = dataRow.Field<DateTime>("rp_date_of_submission"),
                        //rp_date_of_resolve = dataRow.Field<DateTime>("rp_date_of_resolve"),
                        rp_remarks = dataRow.Field<string>("rp_remarks"),
                        //rp_status = dataRow.Field<bool>("rp_status"),
                        //rp_creation_datetime = dataRow.Field<DateTime>("rp_creation_datetime"),
                        //rp_updation_datetime = dataRow.Field<DateTime>("rp_updation_datetime"),
                        //rp_created_by = dataRow.Field<long>("rp_created_by"),
                        //rp_updated_by = dataRow.Field<long>("rp_updated_by")
                    }).ToList<tbl_report_problem>();
                    ApplicationDetails = MBList.First<tbl_report_problem>();
                }


            }
            catch (Exception ex)
            {

            }
            return ApplicationDetails;
        }

        public bool SaveReportProblemDll(TicketingToolReportProblem rp)
        {
            int res = 0;
            bool result = false;
            try
            {

                string s = TTUploadDocument(rp.UploadedDoc, rp.rp_empid, rp.extensionofDoc);
                rp.rp_upload_document = s;
                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@rp_ticket_no",rp.rp_ticket_no),
                    new SqlParameter("@rp_empid",rp.rp_empid),
                    new SqlParameter("@rp_module_id",rp.rp_module_id),
                    new SqlParameter("@rp_problem_type_id",rp.rp_problem_type_id),
                    new SqlParameter("@rp_complaint_description",rp.rp_complaint_description),
                    new SqlParameter("@rp_upload_document",rp.rp_upload_document),
                    //new SqlParameter("@rp_report_problem_status","Pending"),// rp.rp_report_problem_status),
                    //new SqlParameter("@rp_date_of_resolve",rp.rp_date_of_resolve),
                    //new SqlParameter("@rp_remarks",rp.rp_remarks),
                    //new SqlParameter("@rp_status",rp.rp_status),
                    new SqlParameter("@rp_created_by",rp.rp_created_by)

                };
                //result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_InsertReportProblem"));
                res = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_InsertReportProblem"));
                if (res > 0) result = true; else result = false;
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        //Print MI Challan Print Details 
        //public VM_ChallanPrintDetails PrintMIChallanDetailsDll(long EmpID, int Category, string RefNos, string Type)
        //{
        //    VM_ChallanPrintDetails NBChallanDetails = new VM_ChallanPrintDetails();
        //    DataSet details = new DataSet();
        //    SqlParameter[] parms = {
        //      new SqlParameter("@empId",EmpID),
        //      new SqlParameter("@applicationId",RefNos),
        //      new SqlParameter("@category",Convert.ToString(Category)),
        //      new SqlParameter("@type",Type)
        //    };
        //    details = _Conn.ExeccuteDataset(parms, "sp_kgid_MB_Print_PaymentDetails");
        //    if (details.Tables != null && details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
        //    {
        //        NBChallanDetails.dm_ddo_code = details.Tables[0].Rows[0]["dm_ddo_code"].ToString();
        //        NBChallanDetails.dm_ddo_office = details.Tables[0].Rows[0]["dm_ddo_office"].ToString();
        //        NBChallanDetails.dm_deptname_english = details.Tables[0].Rows[0]["dm_deptname_english"].ToString();
        //        NBChallanDetails.dm_name_english = details.Tables[0].Rows[0]["dm_name_english"].ToString();
        //        NBChallanDetails.employee_name = details.Tables[0].Rows[0]["employee_name"].ToString();
        //        NBChallanDetails.ead_address = details.Tables[0].Rows[0]["ead_address"].ToString();
        //        NBChallanDetails.mobile_number = details.Tables[0].Rows[0]["mobile_number"].ToString();
        //        NBChallanDetails.hoa_desc = details.Tables[0].Rows[0]["hoa_desc"].ToString();
        //        NBChallanDetails.purpose_id = details.Tables[0].Rows[0]["purpose_id"].ToString();
        //        NBChallanDetails.purpose_desc = details.Tables[0].Rows[0]["purpose_desc"].ToString();
        //        NBChallanDetails.sub_purpose_desc = details.Tables[0].Rows[0]["sub_purpose_desc"].ToString();
        //        NBChallanDetails.p_premium = (details.Tables[0].Rows[0]["p_premium"] == DBNull.Value) ? (double?)0 : Convert.ToDouble((details.Tables[0].Rows[0]["p_premium"]));
        //        //NBChallanDetails.LastUpdatedDateTime = Convert.ToDateTime(details.Tables[0].Rows[0]["p_updation_datetime"].ToString());
        //    }
        //    return NBChallanDetails;
        //}
        //
        private string TTUploadDocument(HttpPostedFileBase document, long? ApplicationID, string docType)
        {
            string subPath = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);
                subPath = "/TTDocuments/" + ApplicationID.ToString() + "/" + docType;
                bool exists = Directory.Exists(HttpContext.Current.Server.MapPath(subPath));
                if (!exists)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subPath));
                }

                string path = Path.Combine(HttpContext.Current.Server.MapPath(subPath), fileName);
                document.SaveAs(path);
                subPath = subPath + "/" + fileName;
            }

            return subPath;
        }

        public TTReportProblem GetAllReportedProblemsBasedonFiltersDLL(int moduleid, string fdate, string tdate, string fstatus)
        {
            TTReportProblem ApplicationDetails = new TTReportProblem();
            try
            {
                string FROMDATE = string.Empty;
                string TODATE = string.Empty;
                if (fdate != null && fdate != string.Empty)
                {
                    FROMDATE = _commnobj.DateConversion(Convert.ToDateTime(fdate).ToShortDateString());
                }
                if (tdate != null && tdate != string.Empty)
                {
                    TODATE = _commnobj.DateConversion(Convert.ToDateTime(tdate).ToShortDateString());
                }
                if (fstatus == "--Select--")
                { fstatus = string.Empty; }
                //DateTime Fdat = Convert.ToDateTime(fdate);
                //var FROMDATE = Fdat.ToString("yyyy-MM-dd");
                //DateTime Tdat = Convert.ToDateTime(tdate);
                //var TODATE = Tdat.ToString("yyyy-MM-dd");
                DataSet dsMBList = new DataSet();
                // if(fstatus=="")
                // { fstatus = ""; }
                // else { fstatus = ""; }
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@module",moduleid),
                    new SqlParameter("@fdate", FROMDATE),
                    new SqlParameter("@tdate",TODATE),
                    new SqlParameter("@fstatus",fstatus),
                };
                dsMBList = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_TicketingTool_GetAllReportedProblemsBasedonFilters");
                if (dsMBList.Tables[0].Rows.Count > 0)
                {
                    var MBList = dsMBList.Tables[0].AsEnumerable().Select(dataRow => new TicketingToolReportProblem
                    {
                        // RowNumber = dataRow.Field<long>("RowNumber"),
                        rp_id = dataRow.Field<long>("rp_id"),
                        rp_ticket_no = dataRow.Field<string>("rp_ticket_no"),
                        //rp_empid = dataRow.Field<long>("rp_empid"),
                        mt_desc = dataRow.Field<string>("mt_desc"),
                        pr_description = dataRow.Field<string>("pr_description"),
                        //rp_complaint_description = dataRow.Field<string>("rp_complaint_description"),
                        //rp_upload_document = dataRow.Field<string>("rp_upload_document"),
                        rp_report_problem_status = dataRow.Field<string>("rp_report_problem_status"),
                        //rp_date_of_submission = dataRow.Field<DateTime>("rp_date_of_submission"),
                        SubmissionDate = dataRow.Field<string>("SubmissionDate"),
                        //rp_date_of_resolve = dataRow.Field<DateTime>("rp_date_of_resolve"),
                        rp_remarks = dataRow.Field<string>("rp_remarks"),
                        rp_status = dataRow.Field<bool>("rp_status"),
                        //rp_creation_datetime = dataRow.Field<DateTime>("rp_creation_datetime"),
                        //rp_updation_datetime = dataRow.Field<DateTime>("rp_updation_datetime"),
                        //rp_created_by = dataRow.Field<long>("rp_created_by"),
                        //rp_updated_by = dataRow.Field<long>("rp_updated_by")
                    }).ToList();
                    ApplicationDetails.TicketingToolReportProblemlist = MBList;
                }


            }
            catch (Exception ex)
            {

            }
            return ApplicationDetails;
        }

        public bool UpdateReportProblemDll(TicketingToolReportProblem rp)
        {
            int res = 0;
            bool result = false;
            try
            {
                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@id",rp.rp_id),
                    //new SqlParameter("@rp_empid",rp.rp_empid),
                    //new SqlParameter("@rp_module_id",rp.rp_module_id),
                    //new SqlParameter("@rp_problem_type_id",rp.rp_problem_type_id),
                    //new SqlParameter("@rp_complaint_description",rp.rp_complaint_description),
                    //new SqlParameter("@rp_upload_document",rp.rp_upload_document),
                    new SqlParameter("@status",rp.rp_report_problem_status),// rp.rp_report_problem_status),
                    //new SqlParameter("@rp_date_of_resolve",rp.rp_date_of_resolve),
                    new SqlParameter("@comment",rp.rp_remarks),
                    new SqlParameter("@assignto",rp.rp_assignedto),
                    new SqlParameter("@updatedBy",rp.rp_created_by)
                };
                //result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_InsertReportProblem"));
                res = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_UpdateReportedProblem"));
                if (res > 0) result = true; else result = false;
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public bool UpdateAssignTicketDll(TicketingToolReportProblem rp)
        {
            int res = 0;
            bool result = false;
            try
            {
                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@id",rp.rp_id),
                    new SqlParameter("@assignto",rp.rp_assignedto),
                    new SqlParameter("@updatedBy",rp.rp_updated_by)
                };
                res = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_assignissue"));
                if (res > 0) result = true; else result = false;
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public bool UpdateIssueDetailsDll(TicketingToolReportProblem rp)
        {
            int res = 0;
            bool result = false;
            try
            {
                SqlParameter[] sqlparam =
                  {
                    new SqlParameter("@id",rp.rp_id),
                    new SqlParameter("@status",rp.rp_report_problem_status),
                    new SqlParameter("@comment",rp.rp_remarks),
                    new SqlParameter("@updatedBy",rp.rp_updated_by)
                };
                //result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_InsertReportProblem"));
                res = Convert.ToInt32(_Conn.ExecuteCmd(sqlparam, "sp_kgid_TicketingTool_UpdateIssueDetails"));
                if (res > 0) result = true; else result = false;
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        #region FileUpload
        public string FileUploaddll(HttpPostedFileBase fileBase, int uploadType, int masterTable)
        {
            string result = "0";
            string filePath = "";
            //List<tbl_leave> _tl = new List<tbl_leave>();
            //Commonfunction _cf = new Commonfunction();
            //int roleId = Convert.ToInt32(HttpContext.Current.Session["role_id"].ToString());
            _ExcelSheet.Application application = new _ExcelSheet.Application();
            _ExcelSheet.Workbook workbook = null;
            string filename = string.Empty;
            string nameofTable = string.Empty;
            if (fileBase != null && (fileBase.FileName.EndsWith("xls") || fileBase.FileName.EndsWith("xlsx") || fileBase.FileName.EndsWith("csv")))
            {
                filename = fileBase.FileName;
                string[] filenameList = filename.Split('.');
                nameofTable = filenameList[0];

                string registrationno = "";
                string Chassisno = "";
                string engineno = "";

                //string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //string tableQuery = @"select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME='{0}'";
                try
                {


                    result = "Table Exist!!!";
                    filePath = UploadExcelSheetFiles("TableName", fileBase);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        workbook = application.Workbooks.Open(filePath);
                        _ExcelSheet.Worksheet worksheet = workbook.ActiveSheet;
                        _ExcelSheet.Range range = worksheet.UsedRange;
                        bool columnMatchFlag = true;

                        if (columnMatchFlag == true)
                        {


                            for (int row = 2; row <= range.Rows.Count; row++)
                            {
                                registrationno = ((_ExcelSheet.Range)range.Cells[row, 9]).Text;
                                Chassisno = ((_ExcelSheet.Range)range.Cells[row, 14]).Text;
                                engineno = ((_ExcelSheet.Range)range.Cells[row, 13]).Text;
                                var temp = "";
                                if (registrationno != null | registrationno != "")
                                {
                                    temp = (from n in _db.tbl_motor_insurance_vehicle_details
                                            where n.mivd_registration_no == registrationno || n.mivd_chasis_no == Chassisno || n.mivd_engine_no == engineno
                                            select n.mivd_registration_no).FirstOrDefault();
                                }
                                else
                                {
                                    temp = (from n in _db.tbl_motor_insurance_vehicle_details
                                            where n.mivd_chasis_no == Chassisno || n.mivd_engine_no == engineno
                                            select n.mivd_registration_no).FirstOrDefault();
                                }
                                try
                                {
                                    if (temp == null || temp == "")
                                    {
                                        VM_MotorInsuranceProposerDetails objBD = new VM_MotorInsuranceProposerDetails();
                                        string RefNo = DateTime.Now.ToString("yyyy-MM-dd HH:ffff").Replace("-", "").Replace(" ", "").Replace(":", "").Replace(".", "");
                                        string ownername = ((_ExcelSheet.Range)range.Cells[row, 41]).Text;
                                        string owneraddress = ((_ExcelSheet.Range)range.Cells[row, 42]).Text;

                                        DLL.KGIDMotorInsurance.MotorInsuranceProposerDetailsDll cls = new DLL.KGIDMotorInsurance.MotorInsuranceProposerDetailsDll();
                                        objBD.mipd_employee_id = Convert.ToInt64(HttpContext.Current.Session["UID"]);
                                        objBD.mipd_pagetype = "New";
                                        objBD.mipd_category = Convert.ToString(HttpContext.Current.Session["SelectedCategory"]); //Convert.ToString(HttpContext.Current.Session["Categories"]);
                                        objBD.mipd_email = ((_ExcelSheet.Range)range.Cells[row, 6]).Text;
                                        if (!string.IsNullOrEmpty(ownername) || !string.IsNullOrEmpty(owneraddress))
                                        {
                                            objBD.OwnerofTheVehicle = (ownername + " " + owneraddress);
                                        }
                                        else
                                        {
                                            objBD.OwnerofTheVehicle = null;
                                        }
                                        //objBD.OwnerofTheVehicle = (!string.IsNullOrEmpty(ownername) && !string.IsNullOrEmpty(owneraddress) ? null : (ownername + " " + owneraddress));
                                        objBD.mipd_address = ((_ExcelSheet.Range)range.Cells[row, 42]).Text;
                                        objBD.mipd_fax_no = null;
                                        //objBD.mipd_pincode = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 4]).Text);
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 4]).Text))
                                        {
                                            objBD.mipd_pincode = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 4]).Text);
                                        }
                                        else
                                        {
                                            objBD.mipd_pincode = null;
                                        }


                                        cls.SaveMIProposalAppnRefNo1(objBD, RefNo);

                                        #region[VM_MotorInsuranceVehicleDetails]
                                        VM_MotorInsuranceVehicleDetails vmVehicleDetails = new VM_MotorInsuranceVehicleDetails();

                                        vmVehicleDetails.mivd_pagetype = "New";
                                        vmVehicleDetails.mi_referenceno = Convert.ToInt64(RefNo);
                                        vmVehicleDetails.mivd_employee_id = Convert.ToInt64(HttpContext.Current.Session["UID"]);
                                        vmVehicleDetails.mivd_registration_no = ((_ExcelSheet.Range)range.Cells[row, 9]).Text;
                                        vmVehicleDetails.mivd_registration_authority_and_location = string.Empty;
                                        vmVehicleDetails.mivd_date_of_registration = ((_ExcelSheet.Range)range.Cells[row, 10]).Text;
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 11]).Text))
                                        {
                                            vmVehicleDetails.mivd_vehicle_rto_id = Convert.ToInt64(((_ExcelSheet.Range)range.Cells[row, 11]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_vehicle_rto_id = null;
                                        }
                                        vmVehicleDetails.mivd_chasis_no = ((_ExcelSheet.Range)range.Cells[row, 14]).Text;
                                        vmVehicleDetails.mivd_engine_no = ((_ExcelSheet.Range)range.Cells[row, 13]).Text;
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 17]).Text))
                                        {
                                            vmVehicleDetails.mivd_cubic_capacity = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 17]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_cubic_capacity = null;
                                        }

                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 18]).Text))
                                        {
                                            vmVehicleDetails.mivd_seating_capacity_including_driver = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 18]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_seating_capacity_including_driver = null;
                                        }

                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 21]).Text))
                                        {
                                            vmVehicleDetails.mivd_vehicle_weight = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 21]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_vehicle_weight = 0;
                                        }
                                        //
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 24]).Text))
                                        {
                                            vmVehicleDetails.mivd_vehicle_category_type_id = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 24]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_vehicle_category_type_id = null;
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 15]).Text))
                                        {
                                            vmVehicleDetails.mivd_make_of_vehicle1 = ((_ExcelSheet.Range)range.Cells[row, 15]).Text;
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_make_of_vehicle1 = "";
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 16]).Text))
                                        {
                                            vmVehicleDetails.mivd_type_of_model = ((_ExcelSheet.Range)range.Cells[row, 16]).Text;
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_type_of_model = string.Empty;
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 12]).Text))
                                        {
                                            vmVehicleDetails.mivd_year_of_manufacturer = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 12]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_year_of_manufacturer = null;
                                        }

                                        vmVehicleDetails.mivd_manufacturer_month = "02";

                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 19]).Text))
                                        {
                                            vmVehicleDetails.mivd_vehicle_fuel_type = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 19]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_vehicle_fuel_type = null;
                                        }

                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 22]).Text))
                                        {
                                            vmVehicleDetails.mivd_vehicle_type_id = ((_ExcelSheet.Range)range.Cells[row, 22]).Text;
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_vehicle_type_id = string.Empty;
                                        }

                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 23]).Text))
                                        {
                                            vmVehicleDetails.mivd_vehicle_subtype_id = ((_ExcelSheet.Range)range.Cells[row, 23]).Text;
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_vehicle_subtype_id = string.Empty;
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 25]).Text))
                                        {
                                            vmVehicleDetails.mivd_vehicle_category_id = ((_ExcelSheet.Range)range.Cells[row, 25]).Text;
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_vehicle_category_id = string.Empty;
                                        }
                                        vmVehicleDetails.mivd_vehicle_class_id = null;
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 37]).Text))
                                        {
                                            vmVehicleDetails.VehicleIDVAmount = ((_ExcelSheet.Range)range.Cells[row, 37]).Text;
                                        }
                                        else
                                        {
                                            vmVehicleDetails.VehicleIDVAmount = string.Empty;
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 8]).Text))
                                        {
                                            vmVehicleDetails.mipd_type_of_cover_id = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 8]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mipd_type_of_cover_id = null;
                                        }

                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 29]).Text))
                                        {
                                            vmVehicleDetails.p_mi_policy_number = ((_ExcelSheet.Range)range.Cells[row, 29]).Text;
                                        }
                                        else
                                        {
                                            vmVehicleDetails.p_mi_policy_number = string.Empty;
                                        }

                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 31]).Text))
                                        {
                                            vmVehicleDetails.p_mi_from_date = Convert.ToDateTime(((_ExcelSheet.Range)range.Cells[row, 31]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.p_mi_from_date = null;
                                        }

                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 32]).Text))
                                        {
                                            vmVehicleDetails.p_mi_to_date = Convert.ToDateTime(((_ExcelSheet.Range)range.Cells[row, 32]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.p_mi_to_date = null;
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 33]).Text))
                                        {
                                            vmVehicleDetails.p_mi_tpfrom_date = Convert.ToDateTime(((_ExcelSheet.Range)range.Cells[row, 33]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.p_mi_tpfrom_date = null;
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 34]).Text))
                                        {
                                            vmVehicleDetails.p_mi_tpto_date = Convert.ToDateTime(((_ExcelSheet.Range)range.Cells[row, 34]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.p_mi_tpto_date = null;
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 46]).Text))
                                        {
                                            vmVehicleDetails.mivd_vehicle_fit_upto = Convert.ToDateTime(((_ExcelSheet.Range)range.Cells[row, 46]).Text);
                                        }
                                        else
                                        {
                                            vmVehicleDetails.mivd_vehicle_fit_upto = null;
                                        }

                                        DLL.KGIDMotorInsurance.MotorInsuranceVehicleDetailsDll cls1 = new DLL.KGIDMotorInsurance.MotorInsuranceVehicleDetailsDll();
                                        //cls1.SaveMIVehicleDetailsData(vmVehicleDetails);
                                        cls1.SaveMIVehicleDetailsDataExcel(vmVehicleDetails);
                                        #endregion

                                        #region[VM_MotorInsuranceOtherDetails]
                                        VM_MotorInsuranceOtherDetails objPersonal = new VM_MotorInsuranceOtherDetails();


                                        objPersonal.miod_emp_id = Convert.ToInt64(HttpContext.Current.Session["UID"]);
                                        objPersonal.miod_application_id = Convert.ToInt64(RefNo);
                                        //1
                                        objPersonal.miod_is_non_conventioanal_source = false;
                                        objPersonal.miod_is_non_conventioanal_source_details = null;
                                        //2
                                        objPersonal.miod_is_driving_tuitions = false;
                                        //3
                                        objPersonal.miod_is_geographical = false;
                                        objPersonal.miod_geographical_ext1 = null;
                                        //4
                                        objPersonal.miod_is_own_premises = true;
                                        //5
                                        objPersonal.miod_is_commercial_purpose = false;
                                        //6
                                        objPersonal.miod_is_foreign_embasy = false;
                                        //7
                                        objPersonal.miod_is_vintage_car = false;
                                        //8
                                        objPersonal.miod_is_for_blind_or_ph = false;
                                        //9
                                        objPersonal.miod_is_fibre_glass_tank = false;
                                        //10
                                        objPersonal.miod_is_bi_fuel_system = false;
                                        objPersonal.miod_bi_fuel_amount = null;
                                        //11
                                        objPersonal.miod_is_higher_deductible = false;
                                        objPersonal.miod_higher_deductible_amount = null;
                                        //12
                                        objPersonal.miod_is_automobile_association_of_india = false;
                                        objPersonal.miod_name_of_association = null;
                                        objPersonal.miod_membership_no = null;
                                        objPersonal.miod_date_of_expiry = null;
                                        //13
                                        objPersonal.miod_is_cover_legal_liability = false;
                                        objPersonal.miod_cll_driver_conductor_count = null;
                                        objPersonal.miod_cll_other_emp_count = null;
                                        objPersonal.miod_cll_non_fare_passengers_count = null;
                                        //14
                                        objPersonal.miod_is_no_claim_bonus = false;
                                        objPersonal.miod_is_no_claim_bonus_doc = null;
                                        objPersonal.Miod_is_no_claim_bonus_doc_filename = null;
                                        //15
                                        objPersonal.miod_is_liability_third_parties = false;
                                        //16
                                        objPersonal.miod_is_higher_towing_charges = false;
                                        objPersonal.miod_is_higher_towing_charges_amount = null;
                                        //17
                                        objPersonal.miod_is_include_personal_accident = false;
                                        objPersonal.miod_pa_driver_conductor_count = null;
                                        objPersonal.miod_pa_other_emp_count = null;
                                        objPersonal.miod_pa_unnamed_passengers_count = null;
                                        //18
                                        objPersonal.miod_is_include_personal_accident_for_persons = false;
                                        objPersonal.miod_ipap_name1 = null;
                                        objPersonal.miod_ipap_name1_amount = null;
                                        objPersonal.miod_ipap_name2 = null;
                                        objPersonal.miod_ipap_name2_amount = null;
                                        objPersonal.miod_ipap_name3 = null;
                                        objPersonal.miod_ipap_name3_amount = null;
                                        //19
                                        objPersonal.miod_is_include_pa_cover_for_unnamed_persons = false;
                                        objPersonal.miod_ipaun_name1 = null;
                                        objPersonal.miod_ipaun_name1_amount = null;
                                        objPersonal.miod_ipaun_name2 = null;
                                        objPersonal.miod_ipaun_name2_amount = null;
                                        objPersonal.miod_ipaun_name3 = null;
                                        objPersonal.miod_ipaun_name3_amount = null;
                                        //20
                                        objPersonal.miod_is_anti_theft = false;
                                        objPersonal.miod_is_anti_theft_doc = null;
                                        objPersonal.Miod_is_anti_theft_doc_filename = null;

                                        cls1.SaveMIOtherDetailsDll(objPersonal);
                                        #endregion

                                        #region[VM_MotorInsurancePreviousHistoryDetails]
                                        VM_MotorInsurancePreviousHistoryDetails vmPreviousHistoryDetails = new VM_MotorInsurancePreviousHistoryDetails();

                                        vmPreviousHistoryDetails.ph_DateOfPurchaseOfVehicle = null;
                                        vmPreviousHistoryDetails.ph_DateOfPurchaseOfVehicle = null;
                                        vmPreviousHistoryDetails.ph_PurchaseType = false;
                                        vmPreviousHistoryDetails.ph_VehicleUsedPurposeA = false;
                                        vmPreviousHistoryDetails.ph_VehicleUsedPurposeB = false;
                                        vmPreviousHistoryDetails.ph_vehicleCondition = false;
                                        vmPreviousHistoryDetails.ph_VehicleConditionReason = null;
                                        vmPreviousHistoryDetails.ph_previousinsurerDetails = null;
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 29]).Text))
                                        {
                                            vmPreviousHistoryDetails.ph_previousinsurerNo = ((_ExcelSheet.Range)range.Cells[row, 29]).Text;
                                        }
                                        else
                                        {
                                            vmPreviousHistoryDetails.ph_previousinsurerNo = string.Empty;
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 31]).Text))
                                        {
                                            vmPreviousHistoryDetails.ph_insuranceFromDt = Convert.ToDateTime(((_ExcelSheet.Range)range.Cells[row, 31]).Text);
                                        }
                                        else
                                        {
                                            vmPreviousHistoryDetails.ph_insuranceFromDt = null;
                                        }

                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 32]).Text))
                                        {
                                            vmPreviousHistoryDetails.ph_insuranceToDt = Convert.ToDateTime(((_ExcelSheet.Range)range.Cells[row, 32]).Text);
                                        }
                                        else
                                        {
                                            vmPreviousHistoryDetails.ph_insuranceToDt = null;
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 8]).Text))
                                        {
                                            vmPreviousHistoryDetails.ph_TypeOfCover = ((_ExcelSheet.Range)range.Cells[row, 8]).Text;
                                        }
                                        else
                                        {
                                            vmPreviousHistoryDetails.ph_TypeOfCover = string.Empty;
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 35]).Text))
                                        {
                                            vmPreviousHistoryDetails.previous_vehicle_malus = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 35]).Text);
                                        }
                                        else
                                        {
                                            vmPreviousHistoryDetails.previous_vehicle_malus = null;
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 36]).Text))
                                        {
                                            vmPreviousHistoryDetails.previous_vehicle_ncb = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 36]).Text);
                                        }
                                        else
                                        {
                                            vmPreviousHistoryDetails.previous_vehicle_ncb = null;
                                        }
                                        vmPreviousHistoryDetails.ph_InsuranceDeclined = false;
                                        vmPreviousHistoryDetails.ph_InsuranceCancelled = false;
                                        vmPreviousHistoryDetails.ph_CancelledReason = null;
                                        vmPreviousHistoryDetails.ph_InsuranceImposed = false;
                                        vmPreviousHistoryDetails.ph_Hire = false;
                                        vmPreviousHistoryDetails.ph_Lease = false;
                                        vmPreviousHistoryDetails.ph_Hypothecation = false;
                                        vmPreviousHistoryDetails.ph_HReason = null;
                                        vmPreviousHistoryDetails.ph_OtherInfo = null;
                                        vmPreviousHistoryDetails.ph_EmployeeCode = Convert.ToInt64(HttpContext.Current.Session["UID"]);
                                        vmPreviousHistoryDetails.ph_reference = Convert.ToInt64(RefNo);
                                        vmPreviousHistoryDetails.mivd_pagetype = null;

                                        cls1.SaveMIPreviousHistoryDetails(vmPreviousHistoryDetails);
                                        #endregion

                                        #region[Calculation]
                                        // depreciationvalue
                                        //int year_id = (string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 12]).Text) ? 0 : Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 12]).Text));
                                        //string excelmonth = ((_ExcelSheet.Range)range.Cells[row, 43]).Text;
                                        //string year = null;

                                        //if (year_id > 0) { year = Get_Year(year_id); }

                                        //DateTime DateofManufacture = Convert.ToDateTime(year + "-" + excelmonth + "-01");

                                        DateTime DateofManufacture = Convert.ToDateTime(((_ExcelSheet.Range)range.Cells[row, 20]).Text);
                                        DateTime todayDate = Convert.ToDateTime(DateTime.Today);
                                        int month = ((todayDate.Year - DateofManufacture.Year) * 12) + todayDate.Month - DateofManufacture.Month;
                                        int vd_dep_value = Convert.ToInt32(getdepreciationvalue(month));
                                        //int pvv = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 37]).Text);
                                        decimal pvv = default(decimal);
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 37]).Text))
                                        {
                                            pvv = Convert.ToDecimal(((_ExcelSheet.Range)range.Cells[row, 37]).Text);
                                        }
                                        else
                                        {
                                            pvv = default(decimal);
                                        }

                                        decimal resultA = Convert.ToDecimal(pvv);
                                        decimal DepreB = ((Convert.ToDecimal(resultA)) / 100) * Convert.ToDecimal(vd_dep_value);
                                        decimal ValueC = Convert.ToDecimal(resultA) - Convert.ToDecimal(DepreB);
                                        decimal TotalPVVNumeric = Math.Round(ValueC);
                                        #endregion

                                        #region[VM_MotorInsuranceIDVDetails]
                                        VM_MotorInsuranceIDVDetails objIDV = new VM_MotorInsuranceIDVDetails();
                                        objIDV.miidv_emp_id = Convert.ToInt64(HttpContext.Current.Session["UID"]);
                                        objIDV.miidv_application_id = Convert.ToInt64(RefNo);
                                        objIDV.miidv_vaahanidvamount = "0";
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 37]).Text))
                                        {
                                            objIDV.miidv_insured_declared_value_amount = ((_ExcelSheet.Range)range.Cells[row, 37]).Text;
                                        }
                                        else
                                        {
                                            objIDV.miidv_insured_declared_value_amount = "0";
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 47]).Text))
                                        {
                                            objIDV.vid_premium_short = ((_ExcelSheet.Range)range.Cells[row, 47]).Text;
                                        }
                                        else
                                        {
                                            objIDV.vid_premium_short = "0";
                                        }
                                        if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 48]).Text))
                                        {
                                            objIDV.vid_premium_excess = ((_ExcelSheet.Range)range.Cells[row, 48]).Text;
                                        }
                                        else
                                        {
                                            objIDV.vid_premium_excess = "0";
                                        }
                                        //if (!string.IsNullOrEmpty(((_ExcelSheet.Range)range.Cells[row, 30]).Text))
                                        //{
                                        //    //var temp_amount = (_ExcelSheet.Range)range.Cells[row, 30]).Text;
                                        //    objIDV.premium_amount = Convert.ToDecimal(((_ExcelSheet.Range)range.Cells[row, 30]).Text);
                                        //}
                                        //else
                                        //{ 
                                        //    objIDV.premium_amount = default(decimal);
                                        //}
                                        objIDV.miidv_non_electrical_accessories_amount = "0";
                                        objIDV.miidv_electrical_accessories_amount = "0";
                                        objIDV.miidv_side_car_trailer_amount = "0";
                                        objIDV.miidv_value_of_cng_lpg_amount = "0";
                                        objIDV.miidv_total_amount = Convert.ToString(TotalPVVNumeric);// tbl_vehicle_depreciation_master/mFD
                                        objIDV.premium_amount = 0;// Calculation
                                        objIDV.miidv_pagetype = null;
                                        cls1.SaveMIIDVDetailsDll(objIDV);

                                        #endregion
                                        // Total Amount Payable Rs
                                        //int ddlTypeofCover = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 8]).Text);
                                        //int ddlCategory = Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 24]).Text);

                                        //int vehicalAge = Convert.ToInt32(getvehicalAge(ddlCategory));
                                        //int zone_id= Convert.ToInt32(((_ExcelSheet.Range)range.Cells[row, 28]).Text);
                                        //int ODValue = Convert.ToInt32(getODValue(ddlCategory, vehicalAge, zone_id,));


                                    }
                                    else
                                    {
                                        //KGID_Models.KGID_Verification.tbl_ExcelUpload awd = null;
                                        tbl_ExcelUpload awd = new tbl_ExcelUpload();

                                        //using (DbConnectionKGID context = new DbConnectionKGID())
                                        //{
                                        awd.Registrationno = registrationno;
                                        awd.Remarks = "Registration No Alredy Exist";
                                        awd.UpdatedOn = DateTime.UtcNow;
                                        awd.Chassisno = Chassisno;
                                        awd.engineno = engineno;
                                        _db.tbl_ExcelUpload.Add(awd);
                                        _db.SaveChanges();
                                        //context.SaveChangesAsync();
                                        //}


                                    }
                                }
                                catch (Exception ex)
                                {
                                    tbl_ExcelUpload awd = new tbl_ExcelUpload();

                                    //using (DbConnectionKGID context = new DbConnectionKGID())
                                    //{
                                    awd.Registrationno = registrationno;
                                    awd.Remarks = ex.Message.ToString();
                                    awd.UpdatedOn = DateTime.UtcNow;
                                    awd.Chassisno = Chassisno;
                                    awd.engineno = engineno;
                                    _db.tbl_ExcelUpload.Add(awd);
                                    _db.SaveChanges();
                                    continue;
                                }


                            }


                            result = "Upload Excel Sheet Successfully!!!";
                        }

                    }
                    workbook.Close(0);
                    application.Quit();

                }
                catch (Exception ex)
                {
                    //filePath
                    result = "Error!!!";
                    Console.WriteLine(ex);
                    tbl_ExcelUpload awd = new tbl_ExcelUpload();

                    //using (DbConnectionKGID context = new DbConnectionKGID())
                    //{
                    awd.Registrationno = registrationno;
                    awd.Remarks = "Error!!!";
                    awd.UpdatedOn = DateTime.UtcNow;
                    awd.Chassisno = Chassisno;
                    awd.engineno = engineno;
                    _db.tbl_ExcelUpload.Add(awd);
                    _db.SaveChanges();
                    //workbook.Close(0);
                    //application.Quit();
                }

            LoopEnd:
                result = result;
            }
            else
            {
                result = "Upload File Proper!!!";
            }
            return result;
        }


        public string UploadExcelSheetFiles(string folderName, HttpPostedFileBase uploadedFiles)
        {
            string result = "";
            string appPath = "";
            string savePath = "";
            try
            {
                // appPath = HttpContext.Current.Request.PhysicalApplicationPath + "ExcelTemplate\\FileUpload\\" + folderName + "\\";
                //appPath = HttpContext.Current.Request.PhysicalApplicationPath + "ExcelTemplate\\FileUpload\\" + DateTime.Now.ToString("dd/MM/yyyy") + "\\" + DateTime.Now.ToString("h/mm/ss") + "\\";
                appPath = HttpContext.Current.Request.PhysicalApplicationPath + "ExcelTemplate\\FileUpload\\";
                if (Directory.Exists(appPath))
                {
                    // Directory.CreateDirectory(appPath);
                    savePath = appPath + HttpContext.Current.Server.HtmlEncode(uploadedFiles.FileName);
                    uploadedFiles.SaveAs(savePath);
                    result = savePath;
                }
                //else
                //{
                //    System.IO.DirectoryInfo di = new DirectoryInfo(appPath);
                //    foreach (FileInfo file in di.GetFiles())
                //    {
                //        file.Delete();
                //    }
                //}
                //savePath = appPath + HttpContext.Current.Server.HtmlEncode(uploadedFiles.FileName);
                //uploadedFiles.SaveAs(savePath);
                //result = savePath;
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(savePath))
                {
                    System.IO.File.Delete(savePath);
                }
                throw;
            }
            finally
            {

            }
            return result;
        }

        public string getdepreciationvalue(int month)
        {
            string result = "";
            try
            {
                SqlParameter[] sqlparam =
                {
                        new SqlParameter("@month",month),

                };
                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_getdepreciationvalue");

            }
            catch (Exception ex)
            {

            }

            return result;
        }
        #endregion

        #region Vehical Mapping To DDO
        public List<VehicalMappingToDDO> GetVehicalDetailsUsingVehicalNumber_dll(string VehicalNumber)
        {
            List<VehicalMappingToDDO> VehicalMappingToDDO = new List<VehicalMappingToDDO>();
            try
            {
                SqlParameter[] sqlparam =
                {
                        new SqlParameter("@VehicalNumber",VehicalNumber==""?DBNull.Value:(object)VehicalNumber),

                };
                DataTable dt = _Conn.ExeccuteDatatablet(sqlparam, "sp_kgid_GetVehicalDetailsUsingVehicalNumber");
                if (dt.Rows.Count > 0)
                {
                    VehicalMappingToDDO = dt.AsEnumerable().Select(t =>
                    {
                        var obj = new VehicalMappingToDDO();
                        obj.mivd_vehicle_details_id = Convert.ToInt32(t["mivd_vehicle_details_id"]);
                        obj.mivd_registration_no = Convert.ToString(t["mivd_registration_no"]);
                        obj.rto_desc = "";
                        obj.mivd_type_of_model = Convert.ToString(t["mivd_type_of_model"]);
                        obj.year_desc = Convert.ToString(t["ym_year_desc"]);
                        obj.mivd_chasis_no = Convert.ToString(t["mivd_chasis_no"]);
                        obj.mia_owner_of_the_vehicle = Convert.ToString(t["mia_owner_of_the_vehicle"]);
                        obj.p_mi_policy_number = Convert.ToString(t["p_mi_policy_number"]);
                        return obj;
                    }).ToList();
                }

                return VehicalMappingToDDO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetDdoCode_dll(string ddocode)
        {
            try
            {
                SqlParameter[] sqlparam =
                {
                        new SqlParameter("@ddocode",ddocode),

                };
                DataSet ds = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_GetDdoCode");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string SaveDDoMapping_dll(List<VehicalMappingToDDO> VehicalMappingToDDO)
        {
            string result = string.Empty;
            try
            {
                foreach (VehicalMappingToDDO _VehicalMappingToDDO in VehicalMappingToDDO)
                {
                    SqlParameter[] sqlparam =
                    {
                        new SqlParameter("@mivd_vehicle_details_id",_VehicalMappingToDDO.mivd_vehicle_details_id),
                        new SqlParameter("@employee_id",_VehicalMappingToDDO.employee_id),
                };
                    result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_SaveDDoMapping");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return result;
        }


        public DataSet GetDdoDetailsUsingId_dll(int DDOId)
        {
            try
            {
                SqlParameter[] sqlparam =
                {
                        new SqlParameter("@DDOId",DDOId),

                };
                DataSet ds = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_GetDdoDetailsUsingId");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion


        public string Get_Year(int year_id)
        {
            string result = null;
            SqlParameter[] sqlparam =
            {
             new SqlParameter("@year_id",year_id),
            };
            result = Convert.ToString(_Conn.ExecuteCmd(sqlparam, "sp_kgid_get_year"));
            return result;
        }

    }
}
