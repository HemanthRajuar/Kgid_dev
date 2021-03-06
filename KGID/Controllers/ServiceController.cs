using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using KGID_Models.KGIDMotorInsurance;
using BLL.KGIDMotorInsurance;




namespace KGID.Controllers
{
    public class ServiceController : Controller
    {


        private readonly IMotorInsuranceVehicleDetailsBll _IMotorInsuranceVehicleDetailsBll;


        public ServiceController()
        {

            this._IMotorInsuranceVehicleDetailsBll = new MotorInsuranceVehicleDetailsBll();

        }
        public ActionResult TestDLL()
        {
            //si
            //SignFile 
            return View();
        }
        // GET: Service
        public ActionResult Index()
        {
            //XMLInput();
            //VahanXMLInput();
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
      + "<chasis_no>ME3U3S5F2LA831540</chasis_no>"
      + "<Engine_no>U3S5F1LA974596</Engine_no>"
      + "<Webservice_id>0001</Webservice_id>"
    + "</Basic_vehicle_details>"
  + "</soap:Body>"
+ "</soap:Envelope>";
            //string responseStr = "{\"VahanResponse\": [{\"OFFICECODE\": \"63\",\"CHASINUMBER\": \"ME3U3S5F2LA831540\",\"ENGINENUMBER\": \"U3S5F1LA974596\"}]}";
            //string responseStr = "{\"VahanResponse\": [{\"OFFICECODE\": \"63\",\"CHASINUMBER\": \"ME3U3S5F2LA831540\", \"ENGINENUMBER\": \"U3S5F1LA974596\",\"TYPEOFVEHICLE\": \"M-Cycle/Scooter\",\"MAKEOFVEHICLE\": \"ROYAL-ENFIELD (UNIT OF EICHER LTD)\",\"MODELOFVEHICLE\": \"ARE000AAON0378M6A01A\",\"MONTHOFMANUFACTURE\": \"1\",\"YEAROFMANUFACTURE\": \"2020\",\"CUBICCAPACITY\": \"346.36\", \"SEATINGCAPACITY\": \"2\",\"LADENWEIGHT\": \"350\",\"RequestRefNo\": \"kgid0001\"}]}";
            string responseStr = "{\"Vahan Response\": [{\"OFFICE_CODE\": \"63\",\"CHASI_NUMBER\": \"ME3U3S5F2LA831540\", \"ENGINE_NUMBER\": \"U3S5F1LA974596\",\"TYPE_OF_VEHICLE\": \"M-Cycle/Scooter\",\"MAKE_OF_VEHICLE\": \"ROYAL-ENFIELD (UNIT OF EICHER LTD)\",\"MODEL_OF_VEHICLE\": \"ARE000AAON0378M6A01A\",\"MONTH_OF_MANUFACTURE\": \"1\",\"YEAR_OF_MANUFACTURE\": \"2020\",\"CUBIC_CAPACITY\": \"346.36\", \"SEATING_CAPACITY\": \"2\",\"LADEN_WEIGHT\": \"350\",\"Request_Ref_No\": \"kgid0001\"}]}";

            //HttpWebRequest request;
            //request = (HttpWebRequest)WebRequest.Create(URL);
            //byte[] bytes;
            ////bytes = System.Text.Encoding.ASCII.GetBytes(rxml);
            //bytes = System.Text.Encoding.UTF8.GetBytes(rxml);
            ////request.ContentType = "text/xml; encoding='utf-8'";
            ////request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "text/xml";
            //request.ContentLength = bytes.Length;
            //request.Method = "POST";

            //Stream requestStream = request.GetRequestStream();
            //requestStream.Write(bytes, 0, bytes.Length);
            //requestStream.Close();
            //XmlDocument doc = new XmlDocument();
            //HttpWebResponse response;
            //response = (HttpWebResponse)request.GetResponse();
            //if (response.StatusCode == HttpStatusCode.OK)
            //{
            //    Stream responseStream = response.GetResponseStream();
            //    responseStr = new StreamReader(responseStream).ReadToEnd();
            //    //return Content(responseStr);

            //    doc.LoadXml(responseStr);
            //    //XmlNode node = doc.SelectSingleNode("Basic_vehicle_detailsResult");
            //    //string jsonText = JsonConvert.SerializeXmlNode(doc);

            //    // To convert JSON text contained in string json into an XML node
            //    //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
            //    ViewBag.Response = responseStr;
            //    string jsonText = JsonConvert.SerializeXmlNode(doc);
            //    ViewBag.Response1 = doc.HasChildNodes;
            //}
            VM_MotorInsuranceVehicleDetails obj = new VM_MotorInsuranceVehicleDetails();
            dynamic result = JsonConvert.DeserializeObject(responseStr);


            foreach (var item in result)
            {
                obj.mivd_chasis_no = item.Value[0].CHASI_NUMBER.Value;
                obj.mivd_engine_no = item.Value[0].ENGINE_NUMBER.Value;
                obj.VehicleMakedesc = item.Value[0].MAKE_OF_VEHICLE.Value;
                obj.VehicleCategoryType = item.Value[0].TYPE_OF_VEHICLE.Value;
                obj.mivd_type_of_model = item.Value[0].MODEL_OF_VEHICLE.Value;
                obj.yeardesc = item.Value[0].YEAR_OF_MANUFACTURE.Value;
                obj.mivd_cubic_capacity = Convert.ToInt32(Math.Round(Convert.ToDecimal(item.Value[0].CUBIC_CAPACITY.Value)));
                obj.mivd_seating_capacity_including_driver = Convert.ToInt32(item.Value[0].SEATING_CAPACITY.Value);
                obj.mivd_vehicle_weight = Convert.ToInt32(item.Value[0].LADEN_WEIGHT.Value);
            }

            return View();
        }
        // string responseStr = "{\"Vahan Response\": [{\"OFFICE_CODE\": \"63\",\"CHASI_NUMBER\": \"ME3U3S5F2LA83154\", \"ENGINE_NUMBER\": \"U3S5F1LA974596\",\"TYPE_OF_VEHICLE\": \"M-Cycle/Scooter\",\"MAKE_OF_VEHICLE\": \"ROYAL-ENFIELD (UNIT OF EICHER LTD)\",\"MODEL_OF_VEHICLE\": \"ARE000AAON0378M6A01A\",\"MONTH_OF_MANUFACTURE\": \"12\",\"YEAR_OF_MANUFACTURE\": \"2018\",\"CUBIC_CAPACITY\": \"346.36\", \"SEATING_CAPACITY\": \"2\",\"LADEN_WEIGHT\": \"350\",\"Request_Ref_No\": \"kgid0001\"}]}";

        public ActionResult GetVehicleDetails(string chasis)
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
            //bytes = System.Text.Encoding.ASCII.GetBytes(rxml);
            bytes = System.Text.Encoding.UTF8.GetBytes(rxml);
            //request.ContentType = "text/xml; encoding='utf-8'";
            //request.ContentType = "application/x-www-form-urlencoded";
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
                //return Content(responseStr);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseStr);
                //XmlNode node = doc.SelectSingleNode("Basic_vehicle_detailsResult");
                //string jsonText = JsonConvert.SerializeXmlNode(doc);

                // To convert JSON text contained in string json into an XML node
                //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
                ViewBag.Response = responseStr;
                ViewBag.Response1 = doc.InnerText;
            }

            VM_MotorInsuranceVehicleDetails obj = new VM_MotorInsuranceVehicleDetails();
            dynamic result = JsonConvert.DeserializeObject(ViewBag.Response1);

            obj = _IMotorInsuranceVehicleDetailsBll.BindVahanResponseDetailstoModel(result);

            if (obj.mivd_chasis_no != null)
            {
                if (obj.mivd_chasis_no.Length > 25)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        VahanDetails = obj,

                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        VahanDetails = obj,

                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    IsSuccess = false,
                    VahanDetails = obj,

                }, JsonRequestBehavior.AllowGet);
            }

        }
        #region VAHAN
        public ActionResult VahanXMLInput()
        {
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
      + "<chasis_no>ME3U3S5F2LA831540</chasis_no>"
      + "<Engine_no>U3S5F1LA974596</Engine_no>"
      + "<Webservice_id>0001</Webservice_id>"
    + "</Basic_vehicle_details>"
  + "</soap:Body>"
+ "</soap:Envelope>";
            string responseStr = string.Empty;
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] bytes;
            //bytes = System.Text.Encoding.ASCII.GetBytes(rxml);
            bytes = System.Text.Encoding.UTF8.GetBytes(rxml);
            //request.ContentType = "text/xml; encoding='utf-8'";
            //request.ContentType = "application/x-www-form-urlencoded";
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
                return Content(responseStr);
            }
            return Content(responseStr);

        }
        public void InvokeService()
        {
            //Calling CreateSOAPWebRequest method  
            HttpWebRequest request = CreateSOAPWebRequest();

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request  
            //<soap:Header>
            //  <UserCredentials xmlns='http://localhost/'>
            //    <userName>kgid</userName>
            //    <password>Kg!d$123</password>
            //</UserCredentials>
            //</soap:Header>
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
            <soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
           
            <soap:Body>
            <Basic_vehicle_details xmlns='http://localhost/'>
                <Request_ref_no>kgid0001</Request_ref_no>
                <chasis_no>ME3U3S5F2LA831540</chasis_no>
                <Engine_no>U3S5F1LA974596</Engine_no>
                <Webservice_id>0001</Webservice_id>
            </Basic_vehicle_details>
            </soap:Body>
        </soap:Envelope>");


            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request  
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream  
                    var ServiceResult = rd.ReadToEnd();
                    //writting stream result on console  
                    Console.WriteLine(ServiceResult);
                    Console.ReadLine();
                }
            }
        }

        public HttpWebRequest CreateSOAPWebRequest()
        {
            //Making Web Request  
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"http://rtoeservices.karnataka.gov.in/kgidservices/kgid.asmx?op=Basic_vehicle_details");
            //SOAPAction  
            Req.Headers.Add(@"<UserCredentials xmlns='http://localhost/'>
                <userName>kgid</userName>
                <password>Kg!d$123</password>
            </UserCredentials>");
            //Content_type  
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method  
            Req.Method = "POST";
            //return HttpWebRequest  
            return Req;
        }
        #endregion
        public ActionResult XMLInput()
        {
            //string URL = "http://localhost:8080/KaveriResponse";
            //string URL = "http://localhost:60354/KaveriResponse";string rxml = "<bdajslip><propertyid>117139810577189276</propertyid><nameofowner1>ಕುಮಾರಸ್ವಾಮಿ ಬಡಾವಣೆ Mysore Vasudev</nameofowner1><fathernameowner1>ಕೋ Vasudeva</fathernameowner1><addressofowner1>hghghj</addressofowner1><addresspinowner1>0</addresspinowner1><mobileowner1></mobileowner1></bdajslip>";
            string URL = "https://117.239.56.125/KhajaneWs/rct/rrvcs/secbc/RctReceiveValidateChlnService?wsdl";
            string rxml = //"<?xml version='1.0' encoding='UTF-8'?>
"<ser:envelopedData xmlns:ser='https://service.receivevalidatechallan.dept.rct.integration.ifms.gov.in/'>"
  + "<data>"
    + "<RctReceiveValidateChlnRq>"
      + "<chlnDate>01/12/2020</chlnDate>"
      + "<deptCode>37B</deptCode>"
      + "<ddoCode>20154O</ddoCode>"
      + "<deptRefNum>FBIS1606803955504</deptRefNum>"
      + "<rctReceiveValidateChlnDtls>"
        + "<amount>2160</amount>"
        + "<deptPrpsId>2</deptPrpsId>"
        + "<prpsName>0230~00~104~0~00~000</prpsName>"
        + "<subPrpsName>FACT RGN 02</subPrpsName>"
        + "<subDeptRefNum>323456789577A</subDeptRefNum>"
      + "</rctReceiveValidateChlnDtls>"
      + "<rmtrName>Ptest Factory</rmtrName>"
      + "<totalAmount>2160</totalAmount>"
      + "<trsryCode>572A</trsryCode>"
    + "</RctReceiveValidateChlnRq>"
  + "</data>"
+ "</ser:envelopedData>";
            string responseStr = string.Empty;
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] bytes;
            //bytes = System.Text.Encoding.ASCII.GetBytes(rxml);
            bytes = System.Text.Encoding.UTF8.GetBytes(rxml);
            request.ContentType = "text/xml; encoding='utf-8'";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "text/xml";
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
                return Content(responseStr);
            }
            return Content(responseStr);

        }
        public ActionResult JSONInput()
        {
            //string URL = "http://localhost:8080/KaveriResponse";
            string URL = "http://localhost:60354/KaveriResponse";
            string rxml = "{ name: 'John', age: 31, city: 'New York' }";
            var responseStr = string.Empty;
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(URL);
            byte[] bytes;
            //bytes = System.Text.Encoding.ASCII.GetBytes(rxml);
            bytes = System.Text.Encoding.UTF8.GetBytes(rxml);
            request.ContentType = "application/json; encoding='utf-8'";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "text/xml";
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
                return Json(responseStr, JsonRequestBehavior.AllowGet);
            }
            return Json(responseStr, JsonRequestBehavior.AllowGet);

        }
    }
}