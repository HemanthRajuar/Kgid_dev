using KGID_Models.KGIDMotorInsurance;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Xml;
using BLL.KGIDMotorInsurance;
using System.Data;
using DLL.DBConnection;
using System.Data.SqlClient;
using System;
using System.Linq;

namespace KGID.Controllers
{
    
    public class ICTController : Controller
    {

        private string qry = "";

        private readonly IMotorInsuranceVehicleDetailsBll _IMotorInsuranceVehicleDetailsBll;
        private readonly Common_Connection _dal = new Common_Connection();
        private readonly DbConnectionKGID _db = new DbConnectionKGID();

        public ICTController()
        {

            this._IMotorInsuranceVehicleDetailsBll = new MotorInsuranceVehicleDetailsBll();

        }
        // GET: ICT
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadOldVehicles()
        {
            // 1. sp_kgid_saveMIReferenceNo

            string RefNo = "";
            long result = 0;

            DataSet ds = new DataSet();
            ds = _dal.ExeccuteDataset("select * From tbl_old_Vehicle_data where mia_application_ref_no='x' order by mivd_registration_no");

            if (ds.Tables.Count==0)
            {
                //_dal.ExeccuteNonQuery("");
                return View();
            }
            SqlCommand cmd = new SqlCommand();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //Step -1
                    RefNo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace("-", "").Replace(" ", "").Replace(":", "");
                    SqlParameter[] sqlparam =
                    {
                        new SqlParameter("@reference_no",RefNo),
                        new SqlParameter("@employee_id", ds.Tables[0].Rows[i]["mivd_proposer_id"].ToString()),
                        new SqlParameter("@PageType","New"),
                        new SqlParameter("@Category","2"), //DDO
                        new SqlParameter("@address",ds.Tables[0].Rows[i]["Proposer_address"].ToString()),
                        new SqlParameter("@email",ds.Tables[0].Rows[i]["Email"].ToString()),
                        new SqlParameter("@ownerofvehicle",ds.Tables[0].Rows[i]["Proposer_full_name"].ToString()),
                        new SqlParameter("@faxno",""),
                        new SqlParameter("@pincode",ds.Tables[0].Rows[i]["Pincode"].ToString()),

                    };
                    result = Convert.ToInt64(_dal.ExecuteCmd(sqlparam, "sp_kgid_saveMIReferenceNo"));
                    

                    if (result > 0)
                    {
                        //Step -2

                        bool isSuccess = false;
                        string message = string.Empty;
                        decimal OwnDamageV = 0;
                        decimal PremiumLiabilityV = 0;
                        int MinValue = 0;
                        int Depreciation = 0;
                        string Zone = "";
                        //string Pagetype = "New";
                        decimal AdditionalAmt = 0;
                        decimal govDiscount = 0;
                        decimal PLgovDiscount = 0;
                        decimal PLDriverAmt = 0;
                        decimal PLPassengerAmt = 0;
                        string regdate = ds.Tables[0].Rows[i]["mivd_date_of_registration"].ToString() ;


                        VM_MotorInsuranceVehicleDetails vmVehicleDetails = new VM_MotorInsuranceVehicleDetails();
                        vmVehicleDetails = GetVehicleDetails(ds.Tables[0].Rows[i]["mivd_chasis_no"].ToString(), ds.Tables[0].Rows[i]["mivd_type_of_cover"].ToString());
                        string result1;

                        if (vmVehicleDetails.mivd_chasis_no==null)
                        {
                            SqlParameter[] sqlparam1 =
                              {
                   
                                new SqlParameter("@referenceno", RefNo),
                                new SqlParameter("@employee_id", ds.Tables[0].Rows[i]["mivd_proposer_id"].ToString()),
                                new SqlParameter("@registrationno", ds.Tables[0].Rows[i]["mivd_registration_no"].ToString()),
                                new SqlParameter("@regauthorityandLocation",ds.Tables[0].Rows[i]["mivd_registration_authority_and_location"].ToString()),

                                new SqlParameter("@dateofregistration",regdate),

                                new SqlParameter("@Rtoid", ds.Tables[0].Rows[i]["mivd_rto_id"].ToString()),
                                new SqlParameter("@chassisno", ds.Tables[0].Rows[i]["mivd_chasis_no"].ToString()),
                                new SqlParameter("@engineno", ds.Tables[0].Rows[i]["mivd_engine_no"].ToString()),

                                new SqlParameter("@cubiccapacity", ds.Tables[0].Rows[i]["mivd_cubic_capacity"].ToString()),
                                new SqlParameter("@seatingcapacity", ds.Tables[0].Rows[i]["mivd_seating_capacity_including_driver"].ToString()),
                                new SqlParameter("@vehicleweight", ds.Tables[0].Rows[i]["mivd_vehicle_weight"].ToString()),

                                new SqlParameter("@vehiclecategorytype", ds.Tables[0].Rows[i]["mivd_vehicle_category_type_id"].ToString()),
                                new SqlParameter("@makeofvehicle",ds.Tables[0].Rows[i]["mivd_make_of_vehicle"].ToString()),

                                new SqlParameter("@vehiclemodel", ds.Tables[0].Rows[i]["mivd_type_of_model"].ToString()),

                                new SqlParameter("@year", ds.Tables[0].Rows[i]["mivd_year_of_manufacturer"].ToString()),
                                new SqlParameter("@dateofmanufacture",ds.Tables[0].Rows[i]["mivd_date_of_manufacturer"].ToString()),

                                new SqlParameter("@fueltype",ds.Tables[0].Rows[i]["mivd_vehicle_fuel_type"].ToString()),
                                new SqlParameter("@vehicletype", ds.Tables[0].Rows[i]["mivd_vehicle_type_id"].ToString()),

                                new SqlParameter("@vehiclesubtype", ds.Tables[0].Rows[i]["mivd_vehicle_subtype"].ToString()),
                                new SqlParameter("@vehiclecategory",ds.Tables[0].Rows[i]["mivd_vehicle_category_type_id"].ToString()),
                                new SqlParameter("@vehicleclass", 1),
                                new SqlParameter("@TypeofCoverID",ds.Tables[0].Rows[i]["mipd_type_of_cover_id"].ToString()),
                                new SqlParameter("@Vaahanidv", ds.Tables[0].Rows[i]["mivd_vahanidvamount"].ToString())
                                };
                   
                            result1 = Convert.ToString(_dal.ExecuteCmd(sqlparam1, "sp_kgid_SaveMIVehicleDetails"));

                        }
                        else
                        {

                            regdate = vmVehicleDetails.mivd_date_of_registration.ToString().Split(' ')[0];
                            regdate = regdate.Split('/')[1] + '/' + regdate.Split('/')[0].PadLeft(2, '0') + '/' + regdate.Split('/')[2];

                            SqlParameter[] sqlparam1 =
                              {
                                new SqlParameter("@referenceno", RefNo),
                                new SqlParameter("@employee_id", ds.Tables[0].Rows[i]["mivd_proposer_id"].ToString()),
                                new SqlParameter("@registrationno", vmVehicleDetails.mivd_registration_no),
                                new SqlParameter("@regauthorityandLocation",ds.Tables[0].Rows[i]["mivd_registration_authority_and_location"].ToString()),
                                new SqlParameter("@dateofregistration",vmVehicleDetails.mivd_date_of_registration==""?"":  regdate),
                                new SqlParameter("@Rtoid", vmVehicleDetails.mivd_vehicle_rto_id),
                                new SqlParameter("@chassisno", vmVehicleDetails.mivd_chasis_no),
                                new SqlParameter("@engineno", vmVehicleDetails.mivd_engine_no),

                                new SqlParameter("@cubiccapacity", vmVehicleDetails.mivd_cubic_capacity),
                                new SqlParameter("@seatingcapacity", vmVehicleDetails.mivd_seating_capacity_including_driver),
                                new SqlParameter("@vehicleweight", vmVehicleDetails.mivd_vehicle_weight),

                                new SqlParameter("@vehiclecategorytype", vmVehicleDetails.mivd_vehicle_category_type_id),
                                new SqlParameter("@makeofvehicle", vmVehicleDetails.mivd_make_of_vehicle),
                                new SqlParameter("@vehiclemodel", vmVehicleDetails.mivd_type_of_model),
                                new SqlParameter("@year", vmVehicleDetails.mivd_year_of_manufacturer),
                                new SqlParameter("@dateofmanufacture",vmVehicleDetails.mivd_manufacturer_month),
                                new SqlParameter("@fueltype", vmVehicleDetails.mivd_vehicle_fuel_type),
                                new SqlParameter("@vehicletype", vmVehicleDetails.mivd_vehicle_type_id),
                                new SqlParameter("@vehiclesubtype", vmVehicleDetails.mivd_vehicle_subtype_id),
                                new SqlParameter("@vehiclecategory", vmVehicleDetails.mivd_vehicle_category_id),
                                new SqlParameter("@vehicleclass", vmVehicleDetails.mivd_vehicle_class_id),
                                new SqlParameter("@Vaahanidv", vmVehicleDetails.VehicleIDVAmount==null?"": vmVehicleDetails.VehicleIDVAmount),
                                new SqlParameter("@TypeofCoverID", vmVehicleDetails.mipd_type_of_cover_id)

                                };

                            string param = "";
                            for (int l = 0; l < sqlparam1.Length; l++)
                            {

                                param += sqlparam1[l].Value + ",";
                            }

                            result1 = Convert.ToString(_dal.ExecuteCmd(sqlparam1, "sp_kgid_SaveMIVehicleDetails"));

                        }  //(vmVehicleDetails.mivd_chasis_no=="") end Condition
                       

                        result1 = result1.Trim().Replace("\r\n", string.Empty);
                        string[] res = result1.Split(',');
                        if (res.Length > 9)
                        {
                            isSuccess = true;
                            message = "Vehicle details saved successfully";

                            OwnDamageV = Convert.ToDecimal(res[0]);
                            PremiumLiabilityV = Convert.ToDecimal(res[1]);
                            MinValue = Convert.ToInt32(res[2]);
                            Depreciation = Convert.ToInt32(res[3]);
                            Zone = res[4];
                            AdditionalAmt = Convert.ToDecimal(res[5]);
                            govDiscount = Convert.ToDecimal(res[6]);
                            PLgovDiscount = Convert.ToDecimal(res[7]);
                            PLDriverAmt = Convert.ToDecimal(res[8]);
                            PLPassengerAmt = Convert.ToDecimal(res[9]);


                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(result1))
                            {
                                Zone = result1;
                                isSuccess = false;
                                message = "Cannot proceed,User belong to " + result + " .Please select other category";
                            }
                            if (string.IsNullOrEmpty(result1))
                            {
                                Zone = result1;
                                isSuccess = false;
                                message = "Cannot proceed further,please try again later.";
                            }
                            // return Json(new { IsSuccess = isSuccess, Message = message, Zonetype = res.Length }, JsonRequestBehavior.AllowGet);


                            qry = " update tbl_old_Vehicle_data set error ='" + message + "' where mivd_chasis_no='" + ds.Tables[0].Rows[i]["mivd_chasis_no"].ToString() + "'";
                            _dal.ExeccuteNonQuery(qry);

                        }

                       
                        long appid = 0;

                        appid =  Convert.ToInt32(_dal.GetScalerValue("select * From tbl_motor_insurance_application where mia_application_ref_no='" + RefNo.ToString() + "'"));
                       
                        qry = "update tbl_mi_policy_details set p_mi_policy_number = '" + ds.Tables[0].Rows[i]["policy_no"].ToString() + "'";
                        qry += ", p_mi_premium =" + ds.Tables[0].Rows[i]["Premium"].ToString();
                        qry += ", p_mi_from_date ='" + ds.Tables[0].Rows[i]["od_from_date"].ToString() + "'";
                        qry += ", p_mi_to_date ='" + ds.Tables[0].Rows[i]["od_from_date"].ToString() + "'";
                        qry += ", p_mi_tpfrom_date ='" + ds.Tables[0].Rows[i]["tp_from_date"].ToString() + "'";
                        qry += ", p_mi_tpto_date ='" + ds.Tables[0].Rows[i]["tp_to_date"].ToString() + "'";
                        qry += ", p_mi_active_status=0 ";
                        qry += " where p_mi_application_id =" + appid + " and p_mi_emp_id = " + ds.Tables[0].Rows[i]["mivd_proposer_id"].ToString();
                        _dal.ExeccuteNonQuery(qry);

                        qry = "update tbl_motor_insurance_application set mia_active=0";
                        qry += " where mia_motor_insurance_app_id =" + appid + " and mia_proposer_id = " + ds.Tables[0].Rows[i]["mivd_proposer_id"].ToString();
                        _dal.ExeccuteNonQuery(qry);

                        qry = " update tbl_old_Vehicle_data set mia_application_ref_no =" +  result + " where mivd_chasis_no='" + ds.Tables[0].Rows[i]["mivd_chasis_no"].ToString() + "'";
                        _dal.ExeccuteNonQuery(qry);


                    }
                }
            }
            return View();
        }


        public VM_MotorInsuranceVehicleDetails GetVehicleDetails(string chasis,string typeofcover)
        {

            //
            //string responseStr ="{\"Vahan Response\": [{\"OFFICE_CODE\": \"1\",\"CHASI_NUMBER\": \"ME3U3S5F2LA831540\", \"ENGINE_NUMBER\": \"U3S5F1LA974596\",\"TYPE_OF_VEHICLE\": \"M-Cycle/Scooter\",\"MAKE_OF_VEHICLE\": \"ROYAL-ENFIELD (UNIT OF EICHER LTD)\",\"MODEL_OF_VEHICLE\": \"ARE000AAON0378M6A01A\",\"MONTH_OF_MANUFACTURE\": \"12\",\"YEAR_OF_MANUFACTURE\": \"2018\",\"CUBIC_CAPACITY\": \"346.36\", \"SEATING_CAPACITY\": \"2\",\"LADEN_WEIGHT\": \"350\",\"Request_Ref_No\": \"kgid0001\"}]}";
            string URL = "http://rtoeservices.karnataka.gov.in/kgidservices/kgid.asmx?op=Basic_vehicle_details";
            string rxml = //"<?xml version='1.0' encoding='UTF-8'?>
@"<?xml version='1.0' encoding='utf-8'?>"
+ "<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>"
            + "<soap:Header>"
              + "<UserCredentials xmlns='http://localhost/'>"
      + "<userName>kgid</userName>"
      + "<password>Kg!d$123</password>"
    + "</UserCredentials>"
  + "</soap:Header>"
  + "<soap:Body>"
    + "<Basic_vehicle_details xmlns='http://localhost/'>"
      + "<Request_ref_no>kgid0001</Request_ref_no>"
      + "<chasis_no>" + chasis + "</chasis_no>"
      + "<Engine_no>U3S5F1LA974596</Engine_no>"
      + "<Webservice_id>0001</Webservice_id>"
    + "</Basic_vehicle_details>"
  + "</soap:Body>"
+ "</soap:Envelope>";
            string responseStr = string.Empty;
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] bytes;
            bytes = System.Text.Encoding.UTF8.GetBytes(rxml);
            request.ContentType = "text/xml";
            request.ContentLength = bytes.Length;
            request.Method = "POST";

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                responseStr = new StreamReader(responseStream).ReadToEnd();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseStr);
                ViewBag.Response = responseStr;
                ViewBag.Response1 = doc.InnerText;
            }

            VM_MotorInsuranceVehicleDetails obj = new VM_MotorInsuranceVehicleDetails();

            dynamic result = JsonConvert.DeserializeObject(ViewBag.Response1);

           
                obj = BindResponsetoModel(result, typeofcover);
           
            return obj;
        }

        public VM_MotorInsuranceVehicleDetails BindResponsetoModel(dynamic responseStr,string typeofcover)
        {
            DataSet dsVD1 = new DataSet();
            VM_MotorInsuranceVehicleDetails obj = new VM_MotorInsuranceVehicleDetails();
            SqlParameter[] parms1 = {

            };
            dsVD1 = _dal.ExeccuteDataset(parms1, "sp_kgid_getMultipleVehicleDetailsList");

            if (obj.VehicleCategoryTypeList.Count == 0)
            {
                var VehicleTypeOfCategoryList = dsVD1.Tables[0].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vct_vehicle_category_type_desc"),
                    Value = dataRow.Field<int>("vct_vehicle_category_type_id").ToString()
                }).ToList();
                obj.VehicleCategoryTypeList = VehicleTypeOfCategoryList;
            }
            if (obj.VehicleTypeOfMakeList.Count == 0)
            {
                var VehicleMakeList = dsVD1.Tables[1].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vm_vehicle_make_desc"),
                    Value = dataRow.Field<int>("vm_vehicle_make_id").ToString()
                }).ToList();
                obj.VehicleTypeOfMakeList = VehicleMakeList;
            }
            if (obj.VehicleManufacturerYearList == null)
            {
                var VehicleyearList = dsVD1.Tables[2].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vy_vehicle_year"),
                    Value = dataRow.Field<int>("vy_vehicle_year_id").ToString()
                }).OrderByDescending(a => a.Text).ToList();
                obj.VehicleManufacturerYearList = VehicleyearList;
            }
            if (obj.VehicleFuelList.Count == 0)
            {
                var VehicleFuelList = dsVD1.Tables[3].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vf_vehicle_fuel_type_desc"),
                    Value = dataRow.Field<int>("vf_vehicle_fuel_type_id").ToString()
                }).ToList();
                obj.VehicleFuelList = VehicleFuelList;
            }

            if (obj.VehicleTypeList == null)
            {
                var VehicleTypeList = dsVD1.Tables[4].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vht_vehicle_type_desc"),
                    Value = dataRow.Field<long>("vht_vehicle_type_id").ToString()
                }).ToList();
                obj.VehicleTypeList = VehicleTypeList;
            }

            if (obj.VehicleSubTypeList.Count == 0)
            {
                var VehicleSubTypeList = dsVD1.Tables[5].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vst_vehicle_subtype_desc"),
                    Value = dataRow.Field<long>("vst_vehicle_subtype_id").ToString()
                }).ToList();
                obj.VehicleSubTypeList = VehicleSubTypeList;
            }

            if (obj.VehicleCategoryList == null || obj.VehicleCategoryList.Count == 0)
            {
                var VehicleCategoryList = dsVD1.Tables[6].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vc_vehicle_category_desc"),
                    Value = dataRow.Field<int>("vc_vehicle_category_id").ToString()
                }).ToList();
                obj.VehicleCategoryList = VehicleCategoryList;
            }
            if (obj.VehicleTypeOfClassList.Count == 0)
            {
                var VehicleTypeOfClassList = dsVD1.Tables[8].AsEnumerable().Select(dataRow => new SelectListItem()
                {
                    Text = dataRow.Field<string>("vcm_desc"),
                    Value = dataRow.Field<int>("vcm_id").ToString()
                }).ToList();
                obj.VehicleTypeOfClassList = VehicleTypeOfClassList;
            }
            var TypeofCoverList = (from n in _db.tbl_motor_insurance_type_of_cover
                                   select new SelectListItem { Value = n.mitoc_type_cover_id.ToString(), Text = n.mitoc_type_cover_name }
                                                              ).ToList();

            typeofcover = typeofcover.Trim().ToUpper();

            switch (typeofcover)
            {
                case "LIABILITY ONLY POLICY":
                    obj.mipd_type_of_cover = typeofcover;
                    obj.mipd_type_of_cover_id = 1;
                    break;
                case "PACKAGE POLICY":
                    obj.mipd_type_of_cover = typeofcover;
                    obj.mipd_type_of_cover_id = 2;
                    break;
                case "BUNDLE POLICY":
                    obj.mipd_type_of_cover = typeofcover;
                    obj.mipd_type_of_cover_id = 3;
                    break;
            }

            


            foreach (var item in responseStr)
            {
                if (item.Value[0].ContainsKey("chasis_no"))
                {
                    obj.mivd_chasis_no = item.Value[0].chasis_no.Value;
                }
                else
                {


                    obj.mivd_vehicle_rto_id = Convert.ToInt64(item.Value[0].OFFICE_CODE.Value);
                    obj.mivd_chasis_no = item.Value[0].CHASI_NUMBER.Value;
                    obj.mivd_engine_no = item.Value[0].ENGINE_NUMBER.Value;
                    obj.mivd_registration_no = item.Value[0].Registration_Number.Value;
                    

                    obj.mivd_year_of_manufacturer = Convert.ToInt32( obj.VehicleManufacturerYearList.Where(m => m.Text == item.Value[0].YEAR_OF_MANUFACTURE.Value).First().Value);
                    obj.yeardesc = item.Value[0].YEAR_OF_MANUFACTURE.Value;
                    obj.mivd_manufacturer_month = item.Value[0].MONTH_OF_MANUFACTURE.Value;

                    obj.VehicleMakedesc = item.Value[0].MAKE_OF_VEHICLE.Value;
                    obj.mivd_make_of_vehicle = Convert.ToInt32( obj.VehicleTypeOfMakeList.Where(m => m.Text == item.Value[0].MAKE_OF_VEHICLE.Value).First().Value);

                    

                    obj.VehicleCategoryTypedesc = item.Value[0].Vehicle_category_type.Value;
                    obj.mivd_vehicle_category_type_id = Convert.ToInt32(obj.VehicleCategoryTypeList.Where(m => m.Text == item.Value[0].Vehicle_category_type.Value).First().Value);

                    
                    obj.mivd_type_of_model = item.Value[0].MODEL_OF_VEHICLE.Value;
                   
                    

                    obj.VehicleFueldesc = item.Value[0].Veh_Fuel_Type.Value;
                    obj.mivd_vehicle_fuel_type = Convert.ToInt32(obj.VehicleFuelList.Where(m => m.Text == item.Value[0].Veh_Fuel_Type.Value).First().Value); 


                    obj.VehicleTypedesc = item.Value[0].Vehicle_Type.Value;
                    obj.mivd_vehicle_type_id = obj.VehicleTypeList.Where(m => m.Text == item.Value[0].Vehicle_Type.Value).First().Value;
                    

                    string VaahanVehicleCategoryType = item.Value[0].Vehicle_category_type.Value;

                    if (VaahanVehicleCategoryType == "TWO WHEELER(NT)" || VaahanVehicleCategoryType == "TWO WHEELER(T)" || VaahanVehicleCategoryType == "TWO WHEELER (Invalid Carriage)")
                    {
                        obj.VehicleSubTypedesc = "Two wheeler";
                    }
                    else
                    {
                        obj.VehicleSubTypedesc = "Non-two wheeler";
                    }
                    obj.mivd_vehicle_subtype_id = obj.VehicleSubTypeList.Where(m => m.Text == obj.VehicleSubTypedesc).First().Value;


                    string VaahanTypeofvehicle = item.Value[0].TYPE_OF_VEHICLE.Value;
                    if (VaahanTypeofvehicle == "Motor Cycle/Scooter-Used For Hire")
                    {
                        obj.VehicleCategorydesc = "Two wheeler(Public)";
                    }
                    else if (VaahanTypeofvehicle == "M-Cycle/Scooter" || VaahanTypeofvehicle == "Moped" || VaahanTypeofvehicle == "Motorised Cycle (CC > 25cc)" || VaahanTypeofvehicle == "Motor Cycle/Scooter-With Trailer" || VaahanTypeofvehicle == "Invalid Carriage")
                    {
                        if (obj.VehicleSubTypedesc == "Non-two wheeler")
                        {
                            obj.VehicleCategorydesc = "Car/Jeep";
                        }
                        else
                        {
                            obj.VehicleCategorydesc = "Two wheeler(Private)";
                        }

                    }
                    else if (VaahanTypeofvehicle == "M-Cycle/Scooter-With Side Car" || VaahanTypeofvehicle == "Motor Cycle/Scooter-SideCar(T)")
                    {
                        obj.VehicleCategorydesc = "Two wheeler + side car";
                    }
                    else if (VaahanTypeofvehicle == "Goods Carrier")
                    {
                        obj.VehicleCategorydesc = "Goods carrying vehicle (Public)-A1";
                    }
                    else if (VaahanTypeofvehicle == "e-Rickshaw with Cart (G)" || VaahanTypeofvehicle == "Three Wheeler (Goods)")
                    {
                        obj.VehicleCategorydesc = "Goods carrying vehicle(3-wheeler)(Public)-A3";
                    }
                    else if (VaahanTypeofvehicle == "e-Rickshaw(P)")
                    {
                        obj.VehicleCategorydesc = "Goods carrying vehicle(3-wheeler)(Private)-A4";
                    }
                    else if (VaahanTypeofvehicle == "Three Wheeler (Personal)" || VaahanTypeofvehicle == "Three Wheeler (Passenger)")
                    {
                        obj.VehicleCategorydesc = "3 wheeler passenger";
                    }
                    else if (VaahanTypeofvehicle == "Quadricycle (Commercial)")
                    {
                        obj.VehicleCategorydesc = "4 wheeler upto 6 passengers(Public)";

                    }
                    else if (VaahanTypeofvehicle == "Motor Cab" || VaahanTypeofvehicle == "Maxi Cab" || VaahanTypeofvehicle == "Luxury Cab")
                    {
                        if (Convert.ToInt32(item.Value[0].SEATING_CAPACITY.Value) <= 6)
                        {
                            obj.VehicleCategorydesc = "4 wheeler upto 6 passengers(Public)";
                        }
                        else
                        {
                            obj.VehicleCategorydesc = "4 wheeler and higher(more than 6 passengers)-C2";
                        }
                    }

                    else if (VaahanTypeofvehicle == "Omni Bus (Private Use)" || VaahanTypeofvehicle == "Omni Bus(Private Use)" || VaahanTypeofvehicle == "Bus" || VaahanTypeofvehicle == "Educational Institution Bus" || VaahanTypeofvehicle == "Omni Bus")
                    {
                        obj.VehicleCategorydesc = "4 wheeler and higher(more than 6 passengers)-C2";
                    }
                    else if (VaahanTypeofvehicle == "Motor Car" || VaahanTypeofvehicle == "Private Service Vehicle (Individual Use)" || VaahanTypeofvehicle == "Quadricycle (Private)" || VaahanTypeofvehicle == "Cash Van")
                    {
                        obj.VehicleCategorydesc = "Car/Jeep";

                    }
                    else if (VaahanTypeofvehicle == "Trailer (Agricultural)" || VaahanTypeofvehicle == "Trailer For Personal Use" || VaahanTypeofvehicle == "Camper Van / Trailer" || VaahanTypeofvehicle == "Auxiliary Trailer" || VaahanTypeofvehicle == "Trailer(Commercial)" || VaahanTypeofvehicle == "Tractor - Trolley(Commercial)" || VaahanTypeofvehicle == "Semi - Trailer(Commercial)" || VaahanTypeofvehicle == "Trailer (Commercial)")
                    {
                        obj.VehicleCategorydesc = "Trailers";
                    }
                    else
                    {
                        obj.VehicleCategorydesc = "Special type of vehicles (Class D)";
                    }
                    obj.mivd_vehicle_category_id = obj.VehicleCategoryList.Where(m => m.Text == obj.VehicleCategorydesc).First().Value;


                   
                    obj.VehicleClassdesc = item.Value[0].TYPE_OF_VEHICLE.Value;
                    obj.mivd_vehicle_class_id = Convert.ToInt32( obj.VehicleTypeOfClassList.Where(m => m.Text == obj.VehicleClassdesc).First().Value);



                    obj.mivd_cubic_capacity = Convert.ToInt32(Math.Round(Convert.ToDecimal(item.Value[0].CUBIC_CAPACITY.Value)));
                    obj.mivd_seating_capacity_including_driver = Convert.ToInt32(item.Value[0].SEATING_CAPACITY.Value);
                    obj.mivd_vehicle_weight = Convert.ToInt32(item.Value[0].LADEN_WEIGHT.Value);

                    obj.mivd_date_of_registration = (item.Value[0].Registration_date.Value);
                    obj.VehicleIDVAmount = item.Value[0].IDV_Details.Value;
                    obj.VehicleSaleAmount = item.Value[0].Sale_Amount.Value;
                    obj.mivd_vehicle_reg_no = item.Value[0].Registration_Number.Value;
                    obj.mipd_type_of_cover_list = TypeofCoverList;





                }
            }
            return obj;
        }
    }
}