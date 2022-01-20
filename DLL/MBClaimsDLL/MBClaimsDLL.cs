using DLL.DBConnection;
using KGID_Models.KGID_MB_Claim;
using KGID_Models.KGID_VerifyData;
using KGID_Models.KGIDMotorInsurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static KGID_Models.KGID_MB_Claim.VM_MIOwnDamageClaimDetails;
using static KGID_Models.KGID_MB_Claim.VM_ODClaimApprovedApplicationDetails;

namespace DLL.MBClaimsDLL
{
    public class MBClaimsDLL : IMBClaimsDLL
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private readonly Common_Connection _Conn = new Common_Connection();

        DataTable dtODClaimImagesDocData = new DataTable();
        DataTable dtODClaimDocData = new DataTable();
        DataTable dtODComponentsData = new DataTable();
        DataTable dtODComponentsDataSurveyor = new DataTable();
        DataTable dtODComponentsDataDepartment = new DataTable();
        public VM_MIOwnDamageClaimDetails GetMIOwnDamageClaimDetailsDLL(long EmployeeCode, int Category)
        {
            VM_MIOwnDamageClaimDetails MIODclaimDetails = new VM_MIOwnDamageClaimDetails();
            try
            {
                DataSet dsRD = new DataSet();
                SqlParameter[] sqlparam =
                 {
                    new SqlParameter("@employeeId",EmployeeCode),
                    new SqlParameter("@category",Category)
                 };
                dsRD = _Conn.ExeccuteDataset(sqlparam, "sp_mbclaims_getMBClaimDetails");
                dsRD.Tables["Table"].Merge(dsRD.Tables["Table1"]);
                if (dsRD.Tables[0].Rows.Count > 0)
                {
                    var MIOwnDamelList = dsRD.Tables[0].AsEnumerable().Select(dataRow => new MotorInsuranceODDetailsMI
                    {
                        MIEmployeeId = dataRow.Field<long>("p_mi_emp_id"),
                        MIPolicyNumber = dataRow.Field<string>("p_mi_policy_number"),
                        MIPolicyId = dataRow.Field<long>("p_mi_policy_id"),
                        MIPremium = dataRow.Field<double>("p_mi_premium"),
                        MIApplicationId = dataRow.Field<long>("mia_motor_insurance_app_id"),
                        MIApplicationNumber = dataRow.Field<long>("mia_application_ref_no"),
                        MIPolicyActiveStatus = dataRow.Field<bool>("p_mi_active_status"),
                        MIApplicationActiveStatus = dataRow.Field<bool>("mia_active"),
                        MIUserCategoryId = Convert.ToInt32(dataRow.Field<string>("mia_user_category")),
                        MIPolicyFromDate = dataRow.Field<DateTime?>("p_mi_from_date"),
                        MIPolicyToDate = dataRow.Field<DateTime?>("p_mi_to_date"),

                        MIVehicleMakeName = dataRow.Field<string>("vm_vehicle_make_desc"),
                        //MIVehicleManufactureName = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
                        MIVehicleModelName = dataRow.Field<string>("vm_vehicle_model_desc"),
                        MIVehicleManufactureDate = dataRow.Field<DateTime?>("mivd_date_of_manufacturer"),
                        MIVehicleRegistrationNumber = dataRow.Field<string>("mivd_registration_no"),
                        MIRegistrationName = dataRow.Field<string>("mivd_registration_authority_and_location"),
                        MICubicCapacity = Convert.ToString(dataRow.Field<int>("mivd_cubic_capacity")),
                        MINoOfPassengers = Convert.ToString(dataRow.Field<int>("mivd_seating_capacity_including_driver")),
                        //MITypeOfCover = Convert.ToString(dataRow.Field<string>("mitoc_type_cover_name")),
                        //MIPolicyType = Convert.ToString(dataRow.Field<string>("MIPolicyType"))
                        //MIRenewalApplicationId = ((dataRow.Field<long?>("mira_motor_insurance_app_id")).ToString() == "") ? (long?)0 : dataRow.Field<long?>("mira_motor_insurance_app_id"),
                        //MIRenewalApplicationNumber = ((dataRow.Field<long?>("mira_application_ref_no")).ToString() == "") ? (long?)0 : dataRow.Field<long?>("mira_application_ref_no"),

                    }).ToList();
                    MIODclaimDetails.MIOwnDamageClaimDetails = MIOwnDamelList;
                }

            }
            catch (Exception ex)
            {

            }
            return MIODclaimDetails;
        }

        //Save OD Claim Application Details
        public long SaveODClaimApplicationDetailsDLL(VM_ODClaimApplicationDetails objCAD)
        {
            long result = 0;
            string RefNo = "";
            try
            {
                if (objCAD.Odca_claim_app_no != "" && objCAD.Odca_claim_app_no != null && objCAD.Odca_claim_app_no != "0")
                {
                    RefNo = Convert.ToString(objCAD.Odca_claim_app_no);
                }
                else
                {
                    RefNo = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                }
                if (RefNo != "0" && RefNo != "")
                {
                    dtODClaimImagesDocData.Columns.Add("odi_claim_app_id");
                    dtODClaimImagesDocData.Columns.Add("odi_image_desc");
                    dtODClaimImagesDocData.Columns.Add("odi_image_path");
                    dtODClaimImagesDocData.Columns.Add("odi_active_status");
                    if (objCAD.ClaimVehicleImages != null)
                    {
                        SaveMIODClaimImagesFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), "Claim Vehicle Images", true, objCAD.ClaimVehicleImages, objCAD.ClaimVehicleImagesFileName);
                    }
                    else
                    {
                        SaveMIODClaimImagesFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), "Claim Vehicle Images", true, objCAD.ClaimVehicleImages, objCAD.ClaimVehicleImagesFileName);
                    }
                    ///
                    dtODClaimDocData.Columns.Add("odcdd_claim_app_id");
                    dtODClaimDocData.Columns.Add("odcdd_claim_due_id");
                    dtODClaimDocData.Columns.Add("odcdd_doc_upload_path");
                    dtODClaimDocData.Columns.Add("odcdd_active_status");
                    if (objCAD.Odca_claim_id == 1)
                    {
                        if (objCAD.ClaimFormDoc1 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 1, true, objCAD.ClaimFormDoc1, objCAD.ClaimFormDoc1FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 1, true, objCAD.ClaimFormDoc1, objCAD.ClaimFormDoc1FileName);
                        }
                        if (objCAD.RegistrationCopyDoc1 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 2, true, objCAD.RegistrationCopyDoc1, objCAD.RegistrationCopyDoc1FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 2, true, objCAD.RegistrationCopyDoc1, objCAD.RegistrationCopyDoc1FileName);
                        }
                        if (objCAD.DLDoc1 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 3, true, objCAD.DLDoc1, objCAD.DLDoc1FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 3, true, objCAD.DLDoc1, objCAD.DLDoc1FileName);
                        }
                        if (objCAD.FIRDoc1 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 4, true, objCAD.FIRDoc1, objCAD.FIRDoc1FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 4, true, objCAD.FIRDoc1, objCAD.FIRDoc1FileName);
                        }
                        if (objCAD.EstimationReportDoc1 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 5, true, objCAD.EstimationReportDoc1, objCAD.EstimationReportDoc1FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 5, true, objCAD.EstimationReportDoc1, objCAD.EstimationReportDoc1FileName);
                        }
                    }
                    else if (objCAD.Odca_claim_id == 2)
                    {
                        if (objCAD.ClaimFormDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 6, true, objCAD.ClaimFormDoc2, objCAD.ClaimFormDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 6, true, objCAD.ClaimFormDoc2, objCAD.ClaimFormDoc2FileName);
                        }
                        if (objCAD.RegistrationCopyDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 7, true, objCAD.RegistrationCopyDoc2, objCAD.RegistrationCopyDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 7, true, objCAD.RegistrationCopyDoc2, objCAD.RegistrationCopyDoc2FileName);
                        }
                        if (objCAD.DLDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 8, true, objCAD.DLDoc2, objCAD.DLDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 8, true, objCAD.DLDoc2, objCAD.DLDoc2FileName);
                        }
                        if (objCAD.FIRDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 9, true, objCAD.FIRDoc2, objCAD.FIRDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 9, true, objCAD.FIRDoc2, objCAD.FIRDoc2FileName);
                        }
                        if (objCAD.CReportDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 10, true, objCAD.CReportDoc2, objCAD.CReportDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 10, true, objCAD.CReportDoc2, objCAD.CReportDoc2FileName);
                        }
                        if (objCAD.AffidavitDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 11, true, objCAD.AffidavitDoc2, objCAD.AffidavitDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 11, true, objCAD.AffidavitDoc2, objCAD.AffidavitDoc2FileName);
                        }
                        if (objCAD.ClaimDischargeFormDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 12, true, objCAD.ClaimDischargeFormDoc2, objCAD.ClaimDischargeFormDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 12, true, objCAD.ClaimDischargeFormDoc2, objCAD.ClaimDischargeFormDoc2FileName);
                        }
                        if (objCAD.AdvPayeeRecepit2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 13, true, objCAD.AdvPayeeRecepit2, objCAD.AdvPayeeRecepit2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 13, true, objCAD.AdvPayeeRecepit2, objCAD.AdvPayeeRecepit2FileName);
                        }
                        if (objCAD.RecipientIDDoc2 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 14, true, objCAD.RecipientIDDoc2, objCAD.RecipientIDDoc2FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 14, true, objCAD.RecipientIDDoc2, objCAD.RecipientIDDoc2FileName);
                        }
                    }
                    else if (objCAD.Odca_claim_id == 3)
                    {
                        if (objCAD.ClaimFormDoc3 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 15, true, objCAD.ClaimFormDoc3, objCAD.ClaimFormDoc3FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 15, true, objCAD.ClaimFormDoc3, objCAD.ClaimFormDoc3FileName);
                        }
                        if (objCAD.RegistrationCopyDoc3 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 16, true, objCAD.RegistrationCopyDoc3, objCAD.RegistrationCopyDoc3FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 16, true, objCAD.RegistrationCopyDoc3, objCAD.RegistrationCopyDoc3FileName);
                        }
                        if (objCAD.DLDoc3 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 17, true, objCAD.DLDoc3, objCAD.DLDoc3FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 17, true, objCAD.DLDoc3, objCAD.DLDoc3FileName);
                        }
                        if (objCAD.FIRDoc3 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 18, true, objCAD.FIRDoc3, objCAD.FIRDoc3FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 18, true, objCAD.FIRDoc3, objCAD.FIRDoc3FileName);
                        }
                        if (objCAD.EstimationReportDoc3 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 19, true, objCAD.EstimationReportDoc3, objCAD.EstimationReportDoc3FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 19, true, objCAD.EstimationReportDoc3, objCAD.EstimationReportDoc3FileName);
                        }
                    }
                    else if (objCAD.Odca_claim_id == 4)
                    {
                        if (objCAD.ClaimFormDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 20, true, objCAD.ClaimFormDoc4, objCAD.ClaimFormDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 20, true, objCAD.ClaimFormDoc4, objCAD.ClaimFormDoc4FileName);
                        }
                        if (objCAD.RegistrationCopyDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 21, true, objCAD.RegistrationCopyDoc4, objCAD.RegistrationCopyDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 21, true, objCAD.RegistrationCopyDoc4, objCAD.RegistrationCopyDoc4FileName);
                        }
                        if (objCAD.DLDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 22, true, objCAD.DLDoc4, objCAD.DLDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 22, true, objCAD.DLDoc4, objCAD.DLDoc4FileName);
                        }
                        if (objCAD.FIRDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 23, true, objCAD.FIRDoc4, objCAD.FIRDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 23, true, objCAD.FIRDoc4, objCAD.FIRDoc4FileName);
                        }
                        if (objCAD.EstimationReportDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 24, true, objCAD.EstimationReportDoc4, objCAD.EstimationReportDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 24, true, objCAD.EstimationReportDoc4, objCAD.EstimationReportDoc4FileName);
                        }
                        if (objCAD.RTOReportDoc4 != null)
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 25, true, objCAD.RTOReportDoc4, objCAD.RTOReportDoc4FileName);
                        }
                        else
                        {
                            SaveMIODClaimFileData(objCAD.Odca_proposer_id, Convert.ToInt64(RefNo), objCAD.Odca_claim_id, 25, true, objCAD.RTOReportDoc4, objCAD.RTOReportDoc4FileName);
                        }
                    }

                    ///
                    dtODComponentsData.Columns.Add("odccd_component_id");
                    dtODComponentsData.Columns.Add("odccd_component_name");
                    dtODComponentsData.Columns.Add("odccd_component_price");
                    dtODComponentsData.Columns.Add("odccd_od_claim_app_no");
                    //DataRow Ddr = dtODClaimDocData.NewRow();
                    //Ddr["odcdd_claim_app_id"] = ApplicationID ?? 0;
                    //Ddr["odcdd_claim_due_id"] = DocTypeID ?? 0;
                    //Ddr["odcdd_doc_upload_path"] = odcd_upload_document_path;
                    //Ddr["odcdd_active_status"] = true;
                    //dtODClaimDocData.Rows.Add(Ddr);

                    //var dt = new DataTable();
                    //dt.Columns.Add("ID", typeof(Int32));
                    //DataRow Ddr = dtODClaimDocData.NewRow();

                    //string newstr = objCAD.ClaimComponentListDetails.FirstOrDefault(en => en equals "undefined");
                    objCAD.ClaimComponentListDetails = objCAD.ClaimComponentListDetails.Where(p => !objCAD.ClaimComponentListDetails.Any(x => x.ID == p.ID && x.ID == "undefined")).ToList();
                    //objCAD.ClaimComponentListDetails = objCAD.ClaimComponentListDetails.Except("undefined").ToList();
                    if (objCAD.ClaimComponentListDetails != null)
                    {
                        for (int i = 0; i < objCAD.ClaimComponentListDetails.Count; i++)
                        {
                            DataRow Ddr = dtODComponentsData.NewRow();
                            //Ddr["odccd_component_id"] = objCAD.ClaimComponentListDetails[i].ID;
                            Ddr["odccd_component_name"] = objCAD.ClaimComponentListDetails[i].Type;
                            Ddr["odccd_component_price"] = objCAD.ClaimComponentListDetails[i].Value;
                            Ddr["odccd_od_claim_app_no"] = Convert.ToInt64(RefNo);
                            dtODComponentsData.Rows.Add(Ddr);
                        }
                    }

                    SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@reference_no",RefNo),
                    new SqlParameter("@employee_id",objCAD.Odca_proposer_id),
                    new SqlParameter("@category",objCAD.Odca_category_id),
                    //
                    new SqlParameter("@claim_id",objCAD.Odca_claim_id),
                    new SqlParameter("@damage_cost",objCAD.Odca_damage_cost),
                    new SqlParameter("@vehicle_number",objCAD.Odca_vehicle_number),
                    new SqlParameter("@policy_number",objCAD.Odca_policy_number),
                    new SqlParameter("@dateofaccident",objCAD.Odca_date_time_of_accident),
                    new SqlParameter("@accident_case_id",objCAD.Odca_accident_cause_id),
                    new SqlParameter("@place_of_accident",objCAD.Odca_place_of_accident),
                    new SqlParameter("@dist_id",objCAD.Odca_district_id),
                    new SqlParameter("@taluka_id",objCAD.Odca_taluka_id),
                    new SqlParameter("@MIODClaimVehicleImagesData",dtODClaimImagesDocData),
                    new SqlParameter("@MIODClaimDocumentsData",dtODClaimDocData),
                    new SqlParameter("@MIODComponentsData",dtODComponentsData)
                };
                    result = Convert.ToInt64(_Conn.ExecuteCmd(sqlparam, "sp_kgid_Save_MI_ODClaimApplicationDetails"));
                    //result = 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return result;
        }
        public void SaveMIODClaimImagesFileData(long? EmpCode, long? ApplicationID, string ClaimImages, bool status, HttpPostedFileBase MIODDoc, string odcd_upload_document_path)
        {
            try
            {
                if (string.IsNullOrEmpty(odcd_upload_document_path))
                {
                    odcd_upload_document_path = UploadODClaimImagesDocument(MIODDoc, ApplicationID, ClaimImages);
                }
                DataRow Ddr = dtODClaimImagesDocData.NewRow();
                Ddr["odi_claim_app_id"] = ApplicationID ?? 0;
                Ddr["odi_image_desc"] = ClaimImages;
                Ddr["odi_image_path"] = odcd_upload_document_path;
                Ddr["odi_active_status"] = true;
                dtODClaimImagesDocData.Rows.Add(Ddr);
            }
            catch (Exception ex)
            {

            }
        }
        //OD Claim Accident Vehicle Document Upload
        private string UploadODClaimImagesDocument(HttpPostedFileBase document, long? AppId, string ClaimImages)
        {
            string subPath = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);
                subPath = "/OD_Claim_Docs/" + AppId.ToString() + "/" + ClaimImages;
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
        public void SaveMIODClaimFileData(long? EmpCode, long? ApplicationID, long? ClaimID, long? DocTypeID, bool status, HttpPostedFileBase MIODDoc, string odcd_upload_document_path)
        {
            try
            {
                if (string.IsNullOrEmpty(odcd_upload_document_path))
                {
                    odcd_upload_document_path = UploadODClaimDocument(MIODDoc, ApplicationID, ClaimID);
                }
                DataRow Ddr = dtODClaimDocData.NewRow();
                Ddr["odcdd_claim_app_id"] = ApplicationID ?? 0;
                Ddr["odcdd_claim_due_id"] = DocTypeID ?? 0;
                Ddr["odcdd_doc_upload_path"] = odcd_upload_document_path;
                Ddr["odcdd_active_status"] = true;
                dtODClaimDocData.Rows.Add(Ddr);
            }
            catch (Exception ex)
            {

            }
        }
        //OD Claim Document Upload
        private string UploadODClaimDocument(HttpPostedFileBase document, long? AppId, long? ClaimId)
        {
            string subPath = string.Empty;
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);
                subPath = "/OD_Claim_Docs/" + AppId.ToString() + "/" + ClaimId;
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
        //Get OD Claim Application Details
        public VM_ODClaimApplicationDetails GetODClaimApplicationDetailsDLL(long EmployeeCode, string PolicyNumber)
        {
            VM_ODClaimApplicationDetails objPD = new VM_ODClaimApplicationDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",EmployeeCode),
                 new SqlParameter("@referenceid",PolicyNumber)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_ODClaimApplicationDetails");
                //bool OtherData = false;
                if (dsPD.Tables.Count > 0)
                {
                    if (dsPD.Tables[0].Rows.Count > 0)
                    {
                        objPD.Odca_claim_app_no = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_claim_app_no"]);
                        objPD.Odca_claim_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_claim_id"]);
                        objPD.Odca_proposer_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_proposer_id"]);
                        objPD.Odca_category_id = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_category_id"]);
                        objPD.Odca_vehicle_number = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_vehicle_number"]);
                        objPD.Odca_policy_number = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_policy_number"]);
                        objPD.Odca_date_time_of_accident = Convert.ToDateTime(dsPD.Tables[0].Rows[0]["odca_date_time_of_accident"]);
                        objPD.Odca_accident_cause_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_accident_cause_id"]);
                        objPD.Odca_place_of_accident = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_place_of_accident"]);
                        objPD.Odca_district_id = Convert.ToInt32(dsPD.Tables[0].Rows[0]["odca_district_id"]);
                        objPD.Odca_taluka_id = Convert.ToInt32(dsPD.Tables[0].Rows[0]["odca_taluka_id"]);
                        objPD.Odca_damage_cost = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["odca_damage_cost"]);
                    }
                    if (dsPD.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPD.Tables[1].Rows)
                        {
                            if (Convert.ToInt64(dr["odcd_claim_id"]) == 1)
                            {
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 1)
                                {
                                    objPD.ClaimFormDoc1FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 2)
                                {
                                    objPD.RegistrationCopyDoc1FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 3)
                                {
                                    objPD.DLDoc1FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 4)
                                {
                                    objPD.FIRDoc1FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 5)
                                {
                                    objPD.EstimationReportDoc1FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                            }
                            else if (Convert.ToInt64(dr["odcd_claim_id"]) == 2)
                            {
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 6)
                                {
                                    objPD.ClaimFormDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 7)
                                {
                                    objPD.RegistrationCopyDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 8)
                                {
                                    objPD.DLDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 9)
                                {
                                    objPD.FIRDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 10)
                                {
                                    objPD.CReportDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 11)
                                {
                                    objPD.AffidavitDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 12)
                                {
                                    objPD.ClaimDischargeFormDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 13)
                                {
                                    objPD.AdvPayeeRecepit2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 14)
                                {
                                    objPD.RecipientIDDoc2FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                            }
                            else if (Convert.ToInt64(dr["odcd_claim_id"]) == 3)
                            {
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 15)
                                {
                                    objPD.ClaimFormDoc3FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 16)
                                {
                                    objPD.RegistrationCopyDoc3FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 17)
                                {
                                    objPD.DLDoc3FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 18)
                                {
                                    objPD.FIRDoc3FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 19)
                                {
                                    objPD.EstimationReportDoc3FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                            }
                            else if (Convert.ToInt64(dr["odcd_claim_id"]) == 4)
                            {
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 20)
                                {
                                    objPD.ClaimFormDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 21)
                                {
                                    objPD.RegistrationCopyDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 22)
                                {
                                    objPD.DLDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 23)
                                {
                                    objPD.FIRDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 24)
                                {
                                    objPD.EstimationReportDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                                if (Convert.ToInt64(dr["odcdd_claim_due_id"]) == 25)
                                {
                                    objPD.RTOReportDoc4FileName = dr["odcdd_doc_upload_path"].ToString();
                                }
                            }
                        }
                    }
                    if (dsPD.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPD.Tables[2].Rows)
                        {
                            objPD.ClaimVehicleImagesFileName = dr["odi_image_path"].ToString();
                        }
                    }
                    if (dsPD.Tables[3].Rows.Count > 0)
                    {
                        var myData = dsPD.Tables[3].AsEnumerable().Select(r => new ClaimComponentList1
                        {
                            //ID = Convert.ToString(r.Field<long>("odccd_component_id")),
                            Type = r.Field<string>("odccd_component_name"),
                            Value = r.Field<string>("odccd_component_price")
                        });
                        var list = myData.ToList();
                        objPD.ClaimComponentListDetails = list;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objPD;
        }
        //Get OD Claim Application Status 
        public VM_ODClaimVerificationDetails GetODClaimApplicationStatusListDLL(long empId, int category)
        {
            VM_ODClaimVerificationDetails verificationDetails = new VM_ODClaimVerificationDetails();
            try
            {
                string description = GetCategoryDescription(category);


                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EmpId",empId),
                    new SqlParameter("@Category",category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getMI_ODClaim_ApplicationStatus");
                if (dsDDO.Tables[0].Rows.Count > 0)
                {
                    var CurrentStatusList = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                    {
                        EmployeeCode = dataRow.Field<long?>("employee_id"),
                        Name = dataRow.Field<string>("employee_name"),
                        ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                        //VehicleModelName = dataRow.Field<string>("mivd_type_of_model"),
                        //VehicleManufactureName = dataRow.Field<string>("vm_vehicle_manufacture_desc"),
                        //TypeofCover = dataRow.Field<string>("type_of_cover"),
                        //VehicleYear = dataRow.Field<string>("year"),

                        Status = (dataRow.Field<string>("asm_status_desc") == "Send Back to Employee") ? (dataRow.Field<string>("asm_status_desc").ToString().Replace("Employee", description)) : (dataRow.Field<string>("asm_status_desc").ToString().Replace("Applicant", description)),
                        //Status = (dataRow.Field<string>("asm_status_desc").Replace("Applicant",description)),
                        VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                        //ChasisNo = dataRow.Field<string>("mivd_chasis_no"),
                        //EngineNo = dataRow.Field<string>("mivd_engine_no"),
                        AppStatusID = dataRow.Field<int>("status"),
                        //LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                        CategoryId = dataRow.Field<string>("odca_category_id"),
                        ApplicationId = dataRow.Field<long>("odca_id"),
                       // PolicyPremium = dataRow.Field<double?>("p_mi_premium"),
                        PolicyNumber = dataRow.Field<string>("odca_policy_number"),
                        //UnsignBondDocPath = dataRow.Field<string>("unsignbondpath"),
                        //SignedBondDocPath = dataRow.Field<string>("signedbondpath")
                    }).ToList();
                    verificationDetails.ViewStatusForEmployees = CurrentStatusList.Where(a => a.AppStatusID != 2 && a.AppStatusID != 1).ToList();

                    verificationDetails.LastUpdatedStatusForEmployees = CurrentStatusList.Where(a => a.AppStatusID == 2 || a.AppStatusID == 1).Select(a => a).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        private string GetCategoryDescription(int Category)
        {
            string description = "";
            if (Category == 2)
            {
                description = "Department";
            }
            else if (Category == 11)
            {
                description = "Agency";
            }
            else if (Category == 1)
            {
                description = "Employee";
            }
            return description;
        }
        #region OD Claim Workflow
        // OD Claim Workflow
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForCWVerificationDLL(long empId, string Category)
        {
            VM_ODClaimVerificationDetails verificationDetails = new VM_ODClaimVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",2),
                    new SqlParameter("@EmpId",empId),
                    new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_Claim_Application_Details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                    ApplicationId = dataRow.Field<long>("odca_id"),
                    CategoryId = dataRow.Field<string>("odca_category_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("odca_id")
                }).ToList();
                //var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                //{
                //    EmployeeCode = dataRow.Field<long>("employee_id"),
                //    Name = dataRow.Field<string>("employee_name"),
                //    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                //    Status = dataRow.Field<string>("asm_status_desc"),
                //    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                //    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                //    Premium = dataRow.Field<string>("p_premium")
                //}).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                //verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForSuperintendentVerificationDLL(long empId, string Category)
        {
            VM_ODClaimVerificationDetails verificationDetails = new VM_ODClaimVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",3),
                    new SqlParameter("@EmpId",empId),
                     new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_Claim_Application_Details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                    ApplicationId = dataRow.Field<long>("odca_id"),
                    CategoryId = dataRow.Field<string>("odca_category_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("odca_id")
                }).ToList();
                //var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                //{
                //    EmployeeCode = dataRow.Field<long>("employee_id"),
                //    Name = dataRow.Field<string>("employee_name"),
                //    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                //    Status = dataRow.Field<string>("asm_status_desc"),
                //    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                //    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                //    Premium = dataRow.Field<string>("p_premium")
                //}).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                //verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }
        public VM_ODClaimVerificationDetails GetEmployeeDetailsForADVerificationDLL(long empId, string Category)
        {
            VM_ODClaimVerificationDetails verificationDetails = new VM_ODClaimVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",15),
                    new SqlParameter("@EmpId",empId),
                     new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_Claim_Application_Details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                    ApplicationId = dataRow.Field<long>("odca_id"),
                    CategoryId = dataRow.Field<string>("odca_category_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("odca_id")
                }).ToList();
                //var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                //{
                //    EmployeeCode = dataRow.Field<long>("employee_id"),
                //    Name = dataRow.Field<string>("employee_name"),
                //    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                //    Status = dataRow.Field<string>("asm_status_desc"),
                //    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                //    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                //    Premium = dataRow.Field<string>("p_premium")
                //}).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                //verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_ODClaimVerificationDetails GetEmployeeDetailsForDDVerificationDLL(long empId, string Category)
        {
            VM_ODClaimVerificationDetails verificationDetails = new VM_ODClaimVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",5),
                    new SqlParameter("@EmpId",empId),
                     new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_Claim_Application_Details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                    ApplicationId = dataRow.Field<long>("odca_id"),
                    CategoryId = dataRow.Field<string>("odca_category_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("odca_id")
                }).ToList();
                //var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                //{
                //    EmployeeCode = dataRow.Field<long>("employee_id"),
                //    Name = dataRow.Field<string>("employee_name"),
                //    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                //    Status = dataRow.Field<string>("asm_status_desc"),
                //    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                //    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                //    Premium = dataRow.Field<string>("p_premium")
                //}).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                //verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_ODClaimVerificationDetails GetEmployeeDetailsForDVerificationDLL(long empId, string Category)
        {
            VM_ODClaimVerificationDetails verificationDetails = new VM_ODClaimVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@EType",6),
                    new SqlParameter("@EmpId",empId),
                     new SqlParameter("@Category",Category)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_Claim_Application_Details");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long?>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                    ApplicationId = dataRow.Field<long>("odca_id"),
                    CategoryId = dataRow.Field<string>("odca_category_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();
                var LastUpdatedStatus = dsDDO.Tables[1].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long>("employee_id"),
                    Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    Status = dataRow.Field<string>("asm_status_desc"),
                    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                    ApplicationId = dataRow.Field<long>("odca_id")
                }).ToList();
                //var ApprovedStatus = dsDDO.Tables[3].AsEnumerable().Select(dataRow => new EmployeeVerificationDetailMIODClaim
                //{
                //    EmployeeCode = dataRow.Field<long>("employee_id"),
                //    Name = dataRow.Field<string>("employee_name"),
                //    ApplicationNumber = dataRow.Field<string>("p_mi_policy_number"),
                //    Status = dataRow.Field<string>("asm_status_desc"),
                //    LastUpdatedDate = dataRow.Field<string>("miwd_creation_datetime"),
                //    ApplicationId = dataRow.Field<long>("mia_application_ref_no"),
                //    Premium = dataRow.Field<string>("p_premium")
                //}).ToList();
                verificationDetails.EmployeeVerificationDetails = EmployeeVerification;
                verificationDetails.LastUpdatedStatusForEmployees = LastUpdatedStatus;
                //verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;

                if (dsDDO.Tables[2].Rows.Count > 0)
                {
                    if (dsDDO.Tables[2].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.PendingApplications = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["PENDING"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        public VM_MIODClaimDeptVerficationDetails GetWorkFlowDetailsDLL(long applicationId, int category)
        {
            VM_MIODClaimDeptVerficationDetails ResultClaimWFDetails = new VM_MIODClaimDeptVerficationDetails();
            //IList<VM_MIODClaimWorkFlowDetails> workflowDetails = null;
            //IList<VM_ODClaimApplicationDetails> applicationdetails = null;
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@applicationId",applicationId),
                    new SqlParameter("@category",category)
                };

                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_getMICliamWorkflowDetails");

                if (dsDDO.Tables.Count > 0 && dsDDO.Tables[0].Rows.Count > 0)
                {
                    //workflowDetails = new List<VM_MIODClaimWorkFlowDetails>();
                    foreach (DataRow dr in dsDDO.Tables[0].Rows)
                    {
                        VM_MIODClaimWorkFlowDetails workflowDetail = new VM_MIODClaimWorkFlowDetails();
                        workflowDetail.ApplicationRefNo = dr["ApplicationRefNo"].ToString();
                        workflowDetail.From = dr["From"].ToString();
                        workflowDetail.To = dr["To"].ToString();
                        workflowDetail.Remarks = dr["Remarks"].ToString();
                        workflowDetail.Comments = dr["Comments"].ToString();
                        workflowDetail.CreationDateTime = dr["CreationDateTime"].ToString();
                        workflowDetail.ApplicationStatus = dr["ApplicationStatus"].ToString();
                        workflowDetail.NameOfApplicant = dr["name"].ToString();
                        workflowDetail.Category = Convert.ToInt32(dr["odca_category_id"]);
                        ResultClaimWFDetails.WorkFlowDetails.Add(workflowDetail);
                    }
                    if (dsDDO.Tables[1].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[1].Rows)
                        {
                            ODClaimApplicationDetails ClaimApplicationDetails = new ODClaimApplicationDetails();
                            ClaimApplicationDetails.OD_Claim_Application_No = Convert.ToInt64(dr["odca_claim_app_no"]);
                            ClaimApplicationDetails.OD_Claim_Proposer_ID = Convert.ToInt64(dr["odca_proposer_id"]);
                            ClaimApplicationDetails.OD_Claim_Vehicle_Number = dr["odca_vehicle_number"].ToString();
                            ClaimApplicationDetails.OD_Claim_Policy_Number = dr["odca_policy_number"].ToString();
                            ClaimApplicationDetails.OD_Claim_ID = Convert.ToInt64(dr["odca_district_id"]);
                            ClaimApplicationDetails.OD_Claim_Damage_Cost = dr["odca_damage_cost"].ToString();
                            ClaimApplicationDetails.OD_Claim_Datetime_of_Accident = dr["odca_date_time_of_accident"].ToString();
                            ClaimApplicationDetails.OD_Claim_Accident_Cause_ID = Convert.ToInt64(dr["odca_accident_cause_id"]);
                            ClaimApplicationDetails.OD_Claim_Place_of_Accident = dr["odca_place_of_accident"].ToString();
                            ClaimApplicationDetails.OD_Claim_District_ID = Convert.ToInt64(dr["odca_district_id"]);
                            ClaimApplicationDetails.OD_Claim_Taluka_ID = Convert.ToInt64(dr["odca_taluka_id"]);
                            ClaimApplicationDetails.OD_Claim_District_Name = dr["dm_name_english"].ToString();
                            ClaimApplicationDetails.OD_Claim_Taluka_Name = dr["tm_englishname"].ToString();
                            ResultClaimWFDetails.ODClaimApplicationDetails.Add(ClaimApplicationDetails);
                        }
                    }
                    if (dsDDO.Tables[2].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[2].Rows)
                        {
                            ODClaimsDocumetsDetails ClaimDocDetails = new ODClaimsDocumetsDetails();
                            ClaimDocDetails.OD_Claim_ID = Convert.ToInt64(dr["odcd_claim_id"]);
                            ClaimDocDetails.OD_Claim_Description = dr["odc_description"].ToString();
                            ClaimDocDetails.OD_Claim_Document_Description = dr["odcd_document_desc"].ToString();
                            ClaimDocDetails.OD_Claim_Doc_id = Convert.ToInt64(dr["odcdd_id"]);
                            ClaimDocDetails.OD_Claim_Application_id = Convert.ToInt64(dr["odcdd_claim_app_id"]);
                            ClaimDocDetails.OD_Claim_Due_id = Convert.ToInt64(dr["odcdd_claim_due_id"]);
                            ClaimDocDetails.OD_Claim_Doc_Upload_Path = dr["odcdd_doc_upload_path"].ToString();
                            ResultClaimWFDetails.ClaimUploadDocumentDetails.Add(ClaimDocDetails);
                        }
                    }
                    if (dsDDO.Tables[3].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[3].Rows)
                        {
                            ODClaimsImageDetails ClaimImageDocDetails = new ODClaimsImageDetails();
                            ClaimImageDocDetails.OD_Claim_App_id = Convert.ToInt64(dr["odi_claim_app_id"]);
                            ClaimImageDocDetails.OD_Claim_Image_Description = dr["odi_image_desc"].ToString();
                            ClaimImageDocDetails.OD_Claim_Doc_Upload_Path = dr["odi_image_path"].ToString();
                            ResultClaimWFDetails.ClaimUploadImageDetails.Add(ClaimImageDocDetails);
                        }
                    }
                    if (dsDDO.Tables[4].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[4].Rows)
                        {
                            ODClaimsComponentDetailsApplicant ClaimComponentDetailsApplicant = new ODClaimsComponentDetailsApplicant();
                            ClaimComponentDetailsApplicant.ID = dr["odccd_component_id"].ToString();
                            ClaimComponentDetailsApplicant.Type = dr["odccd_component_name"].ToString();
                            ClaimComponentDetailsApplicant.Value = dr["odccd_component_price"].ToString();
                            ResultClaimWFDetails.ClaimsComponentDetailsApplicant.Add(ClaimComponentDetailsApplicant);
                        }
                    }
                    if (dsDDO.Tables[5].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[5].Rows)
                        {
                            ODClaimsComponentDetailsSurveyor ClaimComponentDetailsSurveyor = new ODClaimsComponentDetailsSurveyor();
                            ClaimComponentDetailsSurveyor.ID = dr["ssc_od_cost_component_id"].ToString();
                            ClaimComponentDetailsSurveyor.Type = dr["ssc_od_cost_component_name"].ToString();
                            ClaimComponentDetailsSurveyor.Value = dr["ssc_assesed_value"].ToString();
                            ResultClaimWFDetails.ClaimsComponentDetailsSurveyor.Add(ClaimComponentDetailsSurveyor);
                        }
                    }
                    if (dsDDO.Tables[6].Rows.Count > 0)
                    {
                        //applicationdetails = new List<VM_ODClaimApplicationDetails>();
                        foreach (DataRow dr in dsDDO.Tables[6].Rows)
                        {
                            ODClaimsComponentDetailsDepartment ClaimComponentDetailsDepartment = new ODClaimsComponentDetailsDepartment();
                            ClaimComponentDetailsDepartment.ID = dr["odcap_cost_component_id"].ToString();
                            ClaimComponentDetailsDepartment.Type = dr["odcap_cost_component_name"].ToString();
                            ClaimComponentDetailsDepartment.Value = dr["odcap_component_cost_approved"].ToString();
                            ResultClaimWFDetails.ClaimsComponentDetailsDepartment.Add(ClaimComponentDetailsDepartment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return ResultClaimWFDetails;
        }
        public string SaveVerifiedDetailsDLL(VM_MIODClaimDeptVerficationDetails objVerification)
        {
            var result = "";
            ///OD Components Data Surveyor
            dtODComponentsDataSurveyor.Columns.Add("odccd_component_id");
            dtODComponentsDataSurveyor.Columns.Add("odccd_component_name");
            dtODComponentsDataSurveyor.Columns.Add("odccd_component_price");
            dtODComponentsDataSurveyor.Columns.Add("odccd_od_claim_app_no");
            ///OD Components Data Department
            dtODComponentsDataDepartment.Columns.Add("odccd_component_id");
            dtODComponentsDataDepartment.Columns.Add("odccd_component_name");
            dtODComponentsDataDepartment.Columns.Add("odccd_component_price");
            dtODComponentsDataDepartment.Columns.Add("odccd_od_claim_app_no");
            if (objVerification.ApplicationStatus == 6)
            {

                ///
                if (objVerification.ClaimsComponentDetailsSurveyor != null)
                {
                    for (int i = 0; i < objVerification.ClaimsComponentDetailsSurveyor.Count; i++)
                    {
                        DataRow Ddr = dtODComponentsDataSurveyor.NewRow();
                        //Ddr["odccd_component_id"] = objVerification.ClaimsComponentDetailsSurveyor[i].ID;
                        Ddr["odccd_component_name"] = objVerification.ClaimsComponentDetailsSurveyor[i].Type;
                        Ddr["odccd_component_price"] = objVerification.ClaimsComponentDetailsSurveyor[i].Value;
                        Ddr["odccd_od_claim_app_no"] = objVerification.ApplicationRefNo;
                        dtODComponentsDataSurveyor.Rows.Add(Ddr);
                    }
                }
                

            }
            else if (objVerification.ApplicationStatus == 15)
            {
                if (objVerification.ClaimsComponentDetailsDepartment != null)
                {
                    for (int i = 0; i < objVerification.ClaimsComponentDetailsDepartment.Count; i++)
                    {
                        DataRow Ddr = dtODComponentsDataDepartment.NewRow();
                        //Ddr["odccd_component_id"] = objVerification.ClaimsComponentDetailsDepartment[i].ID;
                        Ddr["odccd_component_name"] = objVerification.ClaimsComponentDetailsDepartment[i].Type;
                        Ddr["odccd_component_price"] = objVerification.ClaimsComponentDetailsDepartment[i].Value;
                        Ddr["odccd_od_claim_app_no"] = objVerification.ApplicationRefNo;
                        dtODComponentsDataDepartment.Rows.Add(Ddr);
                    }
                }
            }
                try
            {
                SqlParameter[] sqlparam =
                    {
                    new SqlParameter("@employee_id",objVerification.EmpCode),
                    new SqlParameter("@micw_application_id",objVerification.ApplicationId),
                    new SqlParameter("@micw_application_refno",objVerification.ApplicationRefNo),
                    new SqlParameter("@micw_verified_by",objVerification.CreatedBy),
                    new SqlParameter("@micw_checklist_status",objVerification.VerifyApplicationDetails),
                    new SqlParameter("@micw_remarks",objVerification.Remarks),
                    new SqlParameter("@micw_comments",objVerification.Comments),
                    new SqlParameter("@micw_application_status",objVerification.ApplicationStatus),
                    new SqlParameter("@micw_active_status",true),
                    new SqlParameter("@micw_created_by",objVerification.CreatedBy),
                    new SqlParameter("@micw_creation_datetime",DateTime.Now),
                    new SqlParameter("@surveyor_id",objVerification.SurveyorId),
                    new SqlParameter("@damage_cost",objVerification.DamageCost),
                    new SqlParameter("@date_of_inspection",objVerification.DateOfInspection),
                    new SqlParameter("@MIODComponentsDataSurveyor",dtODComponentsDataSurveyor),
                    new SqlParameter("@MIODComponentsDataDepartment",dtODComponentsDataDepartment)

                };

                result = _Conn.ExecuteCmd(sqlparam, "sp_kgid_Insert_CW_ODClaimsWFVerification");
                if (objVerification.ApplicationStatus == 15)
                {
                    DataSet details = new DataSet();

                    SqlParameter[] sqlparamNotifDetails =
                    {
                        new SqlParameter("@employeeId", objVerification.EmpCode),
                        new SqlParameter("@applicationId",objVerification.ApplicationRefNo)

                    };

                    details = _Conn.ExeccuteDataset(sqlparamNotifDetails, "sp_kgid_getNotificationDetails");
                    //VM_NotificationDetailsMI notificationDetails = new VM_NotificationDetailsMI();

                    //if (details.Tables != null && details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
                    //{
                    //    notificationDetails.DDOEmailId = details.Tables[0].Rows[0]["DDOEmailId"].ToString();
                    //    notificationDetails.EmpEmailId = details.Tables[0].Rows[0]["EmpEmailId"].ToString();
                    //    notificationDetails.EmpMobileNumber = Convert.ToInt64(details.Tables[0].Rows[0]["EmpMobileNumber"].ToString());
                    //    notificationDetails.EmpName = details.Tables[0].Rows[0]["EmpName"].ToString();

                    //}

                    //SendInsurancePolicyNotification(notificationDetails);
                    //returnString = notificationDetails.PolicyNumber;
                }

            }
            catch (Exception ex)
            {

            }
            return result;


        }

        #endregion

        //public VM_MIOwnDamageClaimDetails GetMIOwnDamageClaimDetailsDLL(long empId, int category)
        //{
        //    VM_MIOwnDamageClaimDetails MIODclaimDetails = null;

        //    try
        //    {
        //        DataSet dsMIODClaims = new DataSet();
        //        SqlParameter[] sqlparam =
        //        {
        //            new SqlParameter("@employeeId", empId),
        //             new SqlParameter("@category", category)
        //        };

        //        dsMIODClaims = _Conn.ExeccuteDataset(sqlparam, "sp_mbclaims_getMBClaimDetails");
        //        if (dsMIODClaims.Tables[0].Rows.Count > 0)
        //        {
        //            MIODclaimDetails = new VM_MIOwnDamageClaimDetails();
        //            foreach (DataRow row in dsMIODClaims.Tables[0].Rows)
        //            {
        //                MotorInsuranceODDetailsMI claimDetail = new MotorInsuranceODDetailsMI();
        //                claimDetail.MIPolicyNumber = row["p_mi_policy_number"].ToString();
        //                claimDetail.MIPremium = Convert.ToDouble(row["p_mi_premium"].ToString());
        //                //claimDetail.IsBondReceived = Convert.ToBoolean(row["IsBondReceived"].ToString());
        //                //claimDetail.MIPremium = Convert.ToDecimal(row["NetAmount"].ToString());
        //                //claimDetail.PayableAmount = Convert.ToDecimal(row["PayableAmount"].ToString());
        //                //claimDetail.UnpaidLoanPremium = Convert.ToDecimal(row["UnpaidLoanPremium"].ToString());
        //                //claimDetail.UnpaidPolicyPremium = Convert.ToDecimal(row["UnpaidPolicyPremium"].ToString());
        //                //claimDetail.BonusAmount = Convert.ToDecimal(row["BonusAmount"].ToString());

        //                ///TODO: Add additional fields for maturity claims

        //                MIODclaimDetails.MIOwnDamageClaimDetails.Add(claimDetail);
        //            }
        //        }

        //        //if (dsClaims.Tables[1].Rows.Count > 0 && claimEmployeeDetail.ClaimDetails.Count > 0)
        //        //{
        //        //    claimEmployeeDetail.EmpName = dsClaims.Tables[1].Rows[0]["EmpName"].ToString();
        //        //    claimEmployeeDetail.EmpDesignation = dsClaims.Tables[1].Rows[0]["EmpDesignation"].ToString();
        //        //    claimEmployeeDetail.EmpDepartment = dsClaims.Tables[1].Rows[0]["EmpDepartment"].ToString();
        //        //}

        //        //if (dsClaims.Tables[2].Rows.Count > 0)
        //        //{
        //        //    foreach (DataRow row in dsClaims.Tables[2].Rows)
        //        //    {
        //        //        VM_ClaimDocument claimDocument = new VM_ClaimDocument();
        //        //        var filePath = row["DocumentPath"].ToString();
        //        //        claimDocument.DocumentFileName = Path.GetFileNameWithoutExtension(filePath);
        //        //        claimDocument.DocumentPath = filePath;
        //        //        claimDocument.DocumentType = row["DocumentType"].ToString();
        //        //        claimEmployeeDetail.ClaimDocuments.Add(claimDocument);
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return MIODclaimDetails;
        //}

        //Surveyor Workflow
        public VM_ODClaimSurveyorVerificationDetails GetEmployeeDetailsForSurveyorVerificationDLL(long EmpId)
        {
            VM_ODClaimSurveyorVerificationDetails verificationDetails = new VM_ODClaimSurveyorVerificationDetails();
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",EmpId)
                };
                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_ODClaimAppDetails_Surveyor");

                var EmployeeVerification = dsDDO.Tables[0].AsEnumerable().Select(dataRow => new ApplicantVerificationDetailMIODClaim
                {
                    EmployeeCode = dataRow.Field<long?>("odca_proposer_id"),
                    //Name = dataRow.Field<string>("employee_name"),
                    ApplicationNumber = Convert.ToString(dataRow.Field<long>("odca_claim_app_no")),
                    VehicleNumber = dataRow.Field<string>("odca_vehicle_number"),
                    ApplicationId = dataRow.Field<long>("odca_id"),
                    CategoryId = dataRow.Field<string>("odca_category_id"),
                    Status = dataRow.Field<string>("AppStatus")
                }).ToList();

                verificationDetails.ApplicantVerificationDetails = EmployeeVerification;
                //verificationDetails.ApprovedEmployeeStatus = ApprovedStatus;
                if (dsDDO.Tables[1].Rows.Count > 0)
                {
                    if (dsDDO.Tables[1].Rows.Count == 1)
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[1].Rows[0]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = 0;
                    }
                    else
                    {
                        verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[1].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[1].Rows[1]["ApplicationCount"]);
                        verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[1].Rows[1]["ApplicationCount"]);
                    }
                    //verificationDetails.TotalReceived = Convert.ToInt64(dsDDO.Tables[2].Rows[0]["ApplicationCount"]) + Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.ForwardedApplications = Convert.ToInt64(dsDDO.Tables[1].Rows[0]["FORWAREDED"]);
                    //verificationDetails.SentBackApplication = Convert.ToInt64(dsDDO.Tables[2].Rows[1]["ApplicationCount"]);
                    verificationDetails.AssignedApplications = Convert.ToInt64(dsDDO.Tables[1].Rows[0]["ASSIGNED"]);

                }
            }
            catch (Exception ex)
            {

            }
            return verificationDetails;
        }

        //View Approved Applcations
        public VM_ODClaimApprovedApplicationDetails GetApprovedApplicationListDLL(long EmpID, string Category)
        {
            VM_ODClaimApprovedApplicationDetails ResultAppDetails = new VM_ODClaimApprovedApplicationDetails();
            //IList<VM_MIODClaimWorkFlowDetails> workflowDetails = null;
            //IList<VM_ODClaimApplicationDetails> applicationdetails = null;
            try
            {
                DataSet dsDDO = new DataSet();
                SqlParameter[] sqlparam =
                {
                    new SqlParameter("@employee_id",EmpID),
                    new SqlParameter("@category_id",Category)
                };

                dsDDO = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_ODClaim_Approved_App_List");

                if (dsDDO.Tables.Count > 0 && dsDDO.Tables[0].Rows.Count > 0)
                {
                    //workflowDetails = new List<VM_MIODClaimWorkFlowDetails>();
                    foreach (DataRow dr in dsDDO.Tables[0].Rows)
                    {
                        ApprovedApplicationDetails AprvdAppDetail = new ApprovedApplicationDetails();
                        AprvdAppDetail.ApplicationId = Convert.ToInt64(dr["odca_id"]);
                        AprvdAppDetail.ApplicationRefNo = Convert.ToInt64(dr["odca_claim_app_no"]);
                        AprvdAppDetail.EmpolyeeId = Convert.ToInt64(dr["odca_proposer_id"]);
                        AprvdAppDetail.CategoryId = Convert.ToInt64(dr["odca_category_id"]);
                        AprvdAppDetail.DamageCost = Convert.ToDecimal(dr["odca_damage_cost"]);
                        AprvdAppDetail.ApprovedDamageCost = Convert.ToDecimal(dr["micw_approved_damage_cost"]);
                        AprvdAppDetail.VehicleNo = dr["odca_vehicle_number"].ToString();
                        AprvdAppDetail.MIPolicyNo = dr["odca_policy_number"].ToString();
                        AprvdAppDetail.ApplicationStatus= dr["asm_status_desc"].ToString();
                        ResultAppDetails.ApprovedAppDetails.Add(AprvdAppDetail);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ResultAppDetails;
        }
        public VM_ODClaimWorkOrderDetails GetODClaimAprvdAppDetailsDLL(long EmployeeCode, string PolicyNumber,string Category)
        {
            VM_ODClaimWorkOrderDetails objPD = new VM_ODClaimWorkOrderDetails();
            try
            {
                DataSet dsPD = new DataSet();
                SqlParameter[] sqlparam =
                {
                new SqlParameter("@employee_id",EmployeeCode),
                 new SqlParameter("@referenceid",PolicyNumber),
                 new SqlParameter("@category",Category)
                };
                dsPD = _Conn.ExeccuteDataset(sqlparam, "sp_kgid_select_MI_ODClaim_WorkOrderDetails");
                //bool OtherData = false;
                if (dsPD.Tables.Count > 0)
                {
                    if (dsPD.Tables[0].Rows.Count > 0)
                    {
                        objPD.Odca_claim_app_no = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_claim_app_no"]);
                        objPD.Odca_claim_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_claim_id"]);
                        objPD.Odca_proposer_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_proposer_id"]);
                        objPD.Odca_category_id = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_category_id"]);
                        objPD.Odca_vehicle_number = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_vehicle_number"]);
                        objPD.Odca_policy_number = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_policy_number"]);
                        objPD.Odca_date_time_of_accident = Convert.ToDateTime(dsPD.Tables[0].Rows[0]["odca_date_time_of_accident"]);
                        objPD.Odca_accident_cause_id = Convert.ToInt64(dsPD.Tables[0].Rows[0]["odca_accident_cause_id"]);
                        objPD.Odca_place_of_accident = Convert.ToString(dsPD.Tables[0].Rows[0]["odca_place_of_accident"]);
                        objPD.Odca_district_name = Convert.ToString(dsPD.Tables[0].Rows[0]["dm_name_english"]);
                        objPD.Odca_taluka_name = Convert.ToString(dsPD.Tables[0].Rows[0]["tm_englishname"]);
                        objPD.Odca_damage_cost = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["odca_damage_cost"]);
                        objPD.micw_approved_damage_cost = Convert.ToDecimal(dsPD.Tables[0].Rows[0]["micw_approved_damage_cost"]);
                        objPD.WorkOrderDate= Convert.ToDateTime(dsPD.Tables[0].Rows[0]["WorkOrderDate"]);
                        objPD.vy_vehicle_year = Convert.ToString(dsPD.Tables[0].Rows[0]["vy_vehicle_year"]);
                        objPD.ProposerName = Convert.ToString(dsPD.Tables[0].Rows[0]["ProposerName"]);
                        objPD.ProposerAddress = Convert.ToString(dsPD.Tables[0].Rows[0]["ProposerAddress"]);
                        objPD.DdoOffice = Convert.ToString(dsPD.Tables[0].Rows[0]["DdoOffice"]);
                    }
                    
                    if (dsPD.Tables[1].Rows.Count > 0)
                    {
                        var myData = dsPD.Tables[1].AsEnumerable().Select(r => new ApprovedClaimComponentList
                        {
                            ID = Convert.ToString(r.Field<long>("odcap_cost_component_id")),
                            Type = r.Field<string>("odcc_description"),
                            Value = r.Field<string>("odcap_component_cost_approved")
                        });
                        var list = myData.ToList();
                        objPD.ClaimComponentListDetails = list;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objPD;
        }


        #region Master Data Dist&Taluka&Components
        //Dist List
        public List<tbl_district_master> GetDistListDLL()
        {
            return (from distList in _db.tbl_district_master
                    select distList).ToList();
        }
        //Taluka List
        public List<tbl_taluka_master> GetTalukaListDLL(int DistId)
        {
            return (from TalukaList in _db.tbl_taluka_master where TalukaList.tm_distid == DistId select TalukaList).ToList();
        }
        //Component List
        public List<tbl_od_cost_component_master> GetComponentListDLL()
        {
            return (from ComponentList in _db.tbl_od_cost_component_master
                    select ComponentList).ToList();
        }
        #endregion
    }
}
