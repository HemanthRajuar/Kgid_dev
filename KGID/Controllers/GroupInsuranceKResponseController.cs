using BLL.DashboardBLL;
using BLL.KGIDGroupInsuranceBLL;
using BLL.NewEmployeeBLL;
using com.sun.org.apache.xml.@internal.security.c14n;
using Common;
using DLL.DBConnection;
using java.io;
using javax.crypto;
using javax.crypto.spec;
using javax.xml.stream;
using javax.xml.stream.events;
using KGID_Models.KGID_GroupInsurance;
using KGID_Models.KGID_Master;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using System.Xml.Linq;

namespace KGID.Controllers
{
    public class GroupInsuranceKResponseController : Controller
    {
        // GET: GroupInsuranceKResponse
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        private IDashboardBLL dashboardBLL;
        private readonly IGroupInsuranceBLL _IGroupInsuranceBLL;
        private readonly INBApplicationBll _INBApplicationbll;

        public GroupInsuranceKResponseController()
        {
            dashboardBLL = new DashboardBLL();
            this._IGroupInsuranceBLL = new GroupInsuranceBLL();
            this._INBApplicationbll = new NBApplicationBll();

        }
        public ActionResult Index()
        {
            return View();
        }

        #region KII Return Method
        public void Return(string encdata, string dept_code, string urlId, string connect)
        {
            //encdata = "VhrkkSQ5YXM%2BZJ49L439AKHwoDFvcWL%2FokKk%2FlK83Eu1k2jJymStl2t%2FjWf7ZEDsQLH5B9LREgfC%0D%0A%2BhEOJNysLMx1RxxwRUoP1GvbIHhDg%2FS6z3XEeEPRtTziy3QVHxNV7HMhV%2FpgjJrwW2neteGxnGAD%0D%0AWgzoJnFPeDwYADWFAeC18AARNogWt5DMMdkCgR%2BF8c6PAjO9yGMLqbLqbmPgOUKFaL9g6R9s9WP1%0D%0AW%2BdF0vPN6F9DBBlccG88gsOHN2OtyoSG1R%2BdIAaufRr9oL6Bn2TOaP9jF0IKOpMpgd0A1jFdKber%0D%0AAUnQtXfeL5qzCMsb";
            Logger.LogMessage(TracingLevel.INFO, "KII Return Enter");
            Logger.LogMessage(TracingLevel.INFO, "KII Return encdata: " + encdata);
            _IGroupInsuranceBLL.SaveLogs("KII Return Enter,KII Return encdata: " + encdata, Convert.ToInt64(Session["UID"]));
            //string testdataenc = "VhrkkSQ5YXM%2BZJ49L439AKCEJZzsN5PNVD1WMGXbOu0XTy60BJSDp8LJDWviKwGTPX0we7NfLgxa%0D%0AN7iabx7f837vvVNeNjp4dfUJTBLRN5wAxuqTLDbXRylnuM%2F2e8hZiGjY4LDeafJ53cab7dai6XIf%0D%0Axcp5gxMg1TmYN4DmpadwzNCOsKF8W8g8A7FUeW05%2F3w35rFXH1XmmWW45AVevd8Y3dDikSZlX1%2BG%0D%0AQpm9ZE%2BGf4gbQgmxP4CKObQ7W6epxpazPTxnTD30FKeMOiRKfAY9ByYLgG48QKRIrBVHZcGvZq58%0D%0A0y7MM5ZEW3rB5EMg";
            //var dec = HttpUtility.UrlDecode(encdata);
            //dec.Replace(" ", "+");
            //Logger.LogMessage(TracingLevel.INFO, "Dec: " + dec);
            var resdecData = SymmetricDecrypt(encdata, "EdZUiBM0d8C46PEZ2Yn9Gg==");
            Logger.LogMessage(TracingLevel.INFO, "KII Return resdecData: " + resdecData);
            _IGroupInsuranceBLL.SaveLogs("KII Return resdecData: " + resdecData, Convert.ToInt64(Session["UID"]));
            //  VM_PaymentDetails objPaymentDetails = GetTranDataFromString(resdecData); -- dd 
            VM_GISPaymentDetails objPaymentDetails = GetTranDataFromString(resdecData);


            Logger.LogMessage(TracingLevel.INFO, "objPaymentDetails :" + objPaymentDetails);
            Logger.LogMessage(TracingLevel.INFO, "KII_Home_Return_UID_Session :" + Convert.ToInt64(Session["UID"]));
            _IGroupInsuranceBLL.SaveLogs("KII_Home_Return_UID_Session :", Convert.ToInt64(Session["UID"]));

            //  long result = _INBApplicationbll.SaveNBChallanStatusDll(objPaymentDetails); --dd
            long result = _IGroupInsuranceBLL.GISSaveChallanStatusDll(objPaymentDetails);


            Logger.LogMessage(TracingLevel.INFO, "KIIReturn DB Save:" + result);
            _IGroupInsuranceBLL.SaveLogs("KIIReturn DB Save:" + result, Convert.ToInt64(Session["UID"]));
            //
            //HttpCookie cookieKIIReturn = HttpContext.Request.Cookies.Get("k2return");
            //HttpCookie cookieUID = HttpContext.Request.Cookies.Get("empid");
            //string KIIReturn = cookieKIIReturn.Value;
            //string UID = cookieUID.Value;
            //Logger.LogMessage(TracingLevel.INFO, "KIIReturn Cookie:" + KIIReturn);
            //Logger.LogMessage(TracingLevel.INFO, "UID Cookie:" + UID);
            //if (KIIReturn == "NBPay")
            //{
            //    Response.Redirect(Url.Action("ApplicationForm", "Employee", new { empId = UID, pay = "true" }));
            //}
            //
            string K2ReturnSession = Session["KIIReturn"].ToString();
            Logger.LogMessage(TracingLevel.INFO, "KIIReturn Session:" + K2ReturnSession);
            _IGroupInsuranceBLL.SaveLogs("KIIReturn Session:" + K2ReturnSession, Convert.ToInt64(Session["UID"]));
            if (Session["KIIReturn"].ToString() == "GISPay")   //--dd to be changed
            {
                Logger.LogMessage(TracingLevel.INFO, "GISPay Enter");
                Logger.LogMessage(TracingLevel.INFO, "UID Session:" + Session["UID"].ToString());
                _IGroupInsuranceBLL.SaveLogs("GISPay Enter-UID Session:" + Session["UID"].ToString(), Convert.ToInt64(Session["UID"]));
                //return RedirectToAction("/kgid-app?empId=" + Session["UID"] + "&pay=true", "Employee");
                //return RedirectToAction("kgid-app", new { empId = Session["UID"], pay = "true" });

                //Response.Redirect(Url.Action("ApplicationForm", "Employee", new { empId = Session["UID"], pay = "true" }));  //--dd to be changed

                Response.Redirect(Url.Action("GISApplicationForEmployee", "GroupInsurance", new { empId = Session["UID"], pay = "true" }));  //--dd to be changed
            }
            else
            {
                Logger.LogMessage(TracingLevel.INFO, "GISPay Enter"); // --dd to be changed
                _IGroupInsuranceBLL.SaveLogs("GISPay Enter", Convert.ToInt64(Session["UID"]));
                //return RedirectToAction("/kgid-app?empId=" + Session["UID"] + "&pay=true", "Employee");
                //Response.Redirect(Url.Action("ApplicationForm", "Employee", new { empId = Session["UID"], pay = "true" }));  //--dd to be changed
                Response.Redirect(Url.Action("GISApplicationForEmployee", "GroupInsurance", new { empId = Session["UID"], pay = "true" }));  //--dd to be changed
                // Response.Redirect(Url.Action("kgid-app", new { empId = Session["UID"], pay = "true" }));
            }
        }
        public static string SymmetricDecrypt(string text, string secretkey)
        {
            Cipher cipher;
            string encryptedString;
            byte[] encryptText = null;
            byte[] raw;
            SecretKeySpec skeySpec;
            try
            {
                //FileInputStream fileinputstream = new FileInputStream("D://KII//KGID_KHAJANE.key");
                //byte[] abyte = new byte[fileinputstream.available()];
                byte[] abyte = Encoding.UTF8.GetBytes(secretkey);
                //fileinputstream.read(abyte);
                //fileinputstream.close();
                byte[] keyBytes = new byte[16];
                int len = abyte.Length;
                if (len > keyBytes.Length) len = keyBytes.Length;
                //System.arraycopy(abyte, 0, keyBytes, 0, len);
                Array.Copy(abyte, 0, keyBytes, 0, len);
                //raw = Base64.decode(secretkey);
                skeySpec = new SecretKeySpec(keyBytes, "AES");
                //encryptText = System.Text.Encoding.UTF8.GetBytes(text);
                //encryptText = Base64.decode(text);
                encryptText = Convert.FromBase64String(text);

                IvParameterSpec ivSpec = new IvParameterSpec(keyBytes);
                cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
                cipher.init(Cipher.DECRYPT_MODE, skeySpec, ivSpec);
                //encryptedString = new String(cipher.doFinal(encryptText));
                //encryptedString = Convert.ToBase64String(cipher.doFinal(encryptText));
                encryptedString = Encoding.UTF8.GetString(cipher.doFinal(encryptText));
                return encryptedString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public VM_GISPaymentDetails GetTranDataFromString(string resdecData)
        {
            string[] dataArr = resdecData.Split('|');
            VM_GISPaymentDetails obj = new VM_GISPaymentDetails();
            if (Session["UID"] != null)
            {
                obj.EmpID = Convert.ToInt64(Session["UID"]);
            }
            foreach (var a in dataArr)
            {
                if (a.Split('=')[0] == "Bank_transaction_no")
                    obj.gcs_transaction_ref_no = (a.Split('=') != null) ? a.Split('=')[1].ToString() : "";
                if (a.Split('=')[0] == "challan_amount")
                    obj.gcs_amount = (a.Split('=') != null) ? Convert.ToInt32(a.Split('=')[1]) : 0;
                if (a.Split('=')[0] == "challan_ref_no")
                    obj.gcd_challan_ref_no = (a.Split('=') != null) ? a.Split('=')[1].ToString() : "";
                if (a.Split('=')[0] == "Status")
                    obj.gcs_status = (a.Split('=') != null) ? Convert.ToInt64(a.Split('=')[1]) : 0;
                if (a.Split('=')[0] == "trsn_timestamp")
                    obj.cs_transsaction_date = (a.Split('=') != null) ? a.Split('=')[1].ToString() : "";

            }
            Logger.LogMessage(TracingLevel.INFO, "GetTranDataFromString:" + obj.gcd_challan_ref_no);
            Logger.LogMessage(TracingLevel.INFO, "KII_Home_Return_UID_Session :" + obj.EmpID);
            return obj;
        }
        #endregion
        #region KII RTC 34
        public ActionResult KIIDoubleVerification(string ReqChallanRefNo)
        {
            Session.Timeout = Session.Timeout + 15;
            string ChallanRefNo = ReqChallanRefNo;
           // string URL = "https://khajane2.karnataka.gov.in/KhajaneWs/rct/rrpys/secbc/RctReceivePaymentStatusService?wsdl";
            //string URL = "https://preprodkhajane2.karnataka.gov.in/KhajaneWs/rct/rrvcs/secbc/RctReceiveValidateChlnService?wsdl";
            string URL = System.Configuration.ConfigurationManager.AppSettings["preprodkhajane2test"];

            //string URL = "https://117.239.56.125/KhajaneWs/rct/rrpys/RctReceivePaymentStatusService?wsdl";
            string transactiondate = DateTime.Now.ToString("ddMMyyyy");
            ByteArrayOutputStream fileWriter = null;
            StringBuilder content = null;
            string currPath = string.Empty;
            string SignedresultContent = string.Empty;
            string KIIsignresponse = string.Empty;
            string reqFile = TextFileCreate(ChallanRefNo);
            XMLInputFactory factory = XMLInputFactory.newInstance();
            //File fileLoc = new File(filePath);
            FileReader fileReader = new FileReader(reqFile);
            //XMLStreamReader reader = factory.createXMLStreamReader(fileReader);
            //content = new StringBuilder();
            // Parsing XML using stream reader and writing to a ByteArrayOutputStream
            string AsBase64String = string.Empty;
            byte[] AsBytes = System.IO.File.ReadAllBytes(reqFile);
            fileReader = new FileReader(reqFile);
            XMLStreamReader reader = factory.createXMLStreamReader(fileReader);
            content = new StringBuilder();
            // Parsing XML using stream reader and writing to a ByteArrayOutputStream
            while (reader.hasNext())
            {
                int eventType = reader.next();
                switch (eventType)
                {

                    case XMLEvent.START_ELEMENT:

                        currPath = currPath + "/" + reader.getLocalName();
                        //Instead
                        if (currPath.Contains("data"))
                        {
                            String startTag = "";
                            //Instead
                            if (reader.getLocalName().Equals("data"))
                            {
                                fileWriter = new ByteArrayOutputStream();
                                startTag = "<" + reader.getLocalName();
                                for (int k = 0; k < reader.getNamespaceCount(); k++)
                                {
                                    if (reader.getNamespacePrefix(k) != null)
                                        startTag = startTag + " xmlns:" + reader.getNamespacePrefix(k) + "=\"" + reader.getNamespaceURI(k) + "\"";
                                    else
                                        startTag = startTag + " xmlns=\"" + reader.getNamespaceURI(k) + "\"";
                                }
                                startTag = startTag + ">";
                            }
                            else
                            {
                                startTag = "<" + reader.getLocalName() + ">";
                            }

                            if (fileWriter != null)
                                fileWriter.write(Encoding.ASCII.GetBytes(startTag));
                        }
                        break;

                    case XMLStreamConstants.CHARACTERS:
                        if (fileWriter != null)
                        {
                            fileWriter.write(Encoding.ASCII.GetBytes(reader.getText()));
                        }
                        break;

                    case XMLStreamConstants.END_ELEMENT:
                        //Instead
                        if (currPath.Contains("data"))
                        {
                            string endTag = "</" + reader.getLocalName() + ">";

                            if (fileWriter != null)
                            {
                                fileWriter.write(Encoding.ASCII.GetBytes(endTag));
                            }
                        }
                        content = new StringBuilder();
                        //RemoveLasttag(currPath);
                        currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
                        break;

                    case XMLEvent.CDATA:
                        break;
                    case XMLEvent.SPACE:
                        break;

                }
            }
            com.sun.org.apache.xml.@internal.security.Init.init();
            Canonicalizer canon = Canonicalizer.getInstance(Canonicalizer.ALGO_ID_C14N_OMIT_COMMENTS);
            byte[] canonXmlBytes = canon.canonicalize(fileWriter.toByteArray());
            string beforesignedData = Convert.ToBase64String(canonXmlBytes);
            string beforecanonXmlData = Encoding.UTF8.GetString(AsBytes);
            string aftercanonXmlData = Encoding.UTF8.GetString(canonXmlBytes);

            //SIGN DATA WITH PFX FILE
            string xml_inBase64 = Convert.ToBase64String(AsBytes);
            string em = Encoding.UTF8.GetString(canonXmlBytes);

            Logger.LogMessage(TracingLevel.INFO, "10.10.31.231:8080/SignXmlData-Before convertion");
            //WebAPI Service Call
            using (var client = new HttpClient())
            {
                //string vSignXmlDataURI = System.Configuration.ConfigurationManager.AppSettings["SignXmlDataURI"];

                //client.BaseAddress = new Uri(vSignXmlDataURI);

                string vSignXmlDataURI1 = System.Configuration.ConfigurationManager.AppSettings["SignXmlDataURI"];

                client.BaseAddress = new Uri(vSignXmlDataURI1);

                // client.BaseAddress = new Uri("http://10.10.31.231:8080/SignXmlData/");


                object reqdata = new
                {
                    data = beforesignedData
                };
                var myContent = JsonConvert.SerializeObject(reqdata);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                /////////
                var resultsac = client.PostAsync("getSignforDataByte", byteContent).Result;
                SignedresultContent = resultsac.Content.ReadAsStringAsync().Result;
                client.CancelPendingRequests();
                //Console.WriteLine("about to dispose the client");
                client.Dispose();
                //return resultContent;
                Logger.LogMessage(TracingLevel.INFO, "RCT34-SignedResultData" + SignedresultContent);
                _IGroupInsuranceBLL.SaveLogs("RCT34-SignedResultData" + SignedresultContent, Convert.ToInt64(Session["UID"]));
            }
            Logger.LogMessage(TracingLevel.INFO, "10.10.31.231:8080/SignXmlData-After convertion");
            ////////
            if (SignedresultContent != null)
            {
                string xml2 = @"<?xml version='1.0' encoding='utf-8'?>"
    +
    "<soapenv:Envelope xmlns:soapenv=" + "'" + "http://schemas.xmlsoap.org/soap/envelope/" + "'" + " xmlns:ser=" + "'" + "http://service.receivepymntstatus.dept.rct.integration.ifms.gov.in/" + "'" + " xmlns:head=" + "'" + "http://header.ei.integration.ifms.gov.in/" + "'" + ">"
    + "\n"
    + "   <soapenv:Header>"
    + "\n"
    + "      <ser:Header>"
    + "\n"
    + "         <head:agencyCode>EA_KID</head:agencyCode>"
     + "\n"
    + "         <head:integrationCode>RCT034</head:integrationCode>"
     + "\n"
    + "         <head:uirNo>EA_KID-RCT034-" + transactiondate + "-" + ChallanRefNo + "</head:uirNo>"
                + "\n"
    + "      </ser:Header>"
     + "\n"
    + "   </soapenv:Header>"
     + "\n"
    + "   <soapenv:Body>"
     + "\n"
    + "      <ser:envelopedDataReq>"
     + "\n"
    + "         <Signature>" + SignedresultContent + "</Signature>"
    + "\n"
    + beforecanonXmlData
    + "      </ser:envelopedDataReq>"
     + "\n"
    + "   </soapenv:Body>"
     + "\n"
    + "</soapenv:Envelope>";

                string responseStr = string.Empty;
                string jsonText = string.Empty;
                //var _url = "https://preprodkhajane2.karnataka.gov.in/KhajaneWs/rct/rrvcs/secbc/";
                //var _action = "RctReceiveValidateChlnService?wsdl";

                //XmlDocument soapEnvelopeXml = CreateSoapEnvelope(signeddata, xml2);
                // WebRequest.DefaultCachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                //Log.Debug("Request for Signature Validation");
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(URL);
                byte[] bytes;
                bytes = Encoding.UTF8.GetBytes(xml2);
                request.ContentType = "text/xml";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                //request.SendChunked = false;
                // request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0, no-cache, no-store");
                //request.KeepAlive = true;
                //request.AllowWriteStreamBuffering = false;
                //request.ServicePoint.ConnectionLimit = 10;    // The default value of 2 within a ConnectionGroup caused me always a "Timeout exception" because a user's 1-3 concurrent WebRequests within a second.
                //request.ServicePoint.MaxIdleTime = 5 * 1000;  // (5 sec) default was 100000 (100 sec).  Max idle time for a connection within a ConnectionGroup for reuse before closing

                //Log.Debug("FileStream Rquesting for Pending Payment");
                Logger.LogMessage(TracingLevel.INFO, "RCT34-FileStream Rquesting for Pending Payment");
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                //Log.Debug("FileStream Rquest for Pending Payment Proceed");
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                Logger.LogMessage(TracingLevel.INFO, "RCT34-FileStream Rquest for Pending Payment Proceed");
                _IGroupInsuranceBLL.SaveLogs("RCT34-FileStream Rquest for Pending Payment Proceed", Convert.ToInt64(Session["UID"]));
                //Log.Debug("Response from KII Pending Payment Request:  " + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Logger.LogMessage(TracingLevel.INFO, "RCT34-HttpWebResponse:OK");
                    _IGroupInsuranceBLL.SaveLogs("RCT34-HttpWebResponse:OK", Convert.ToInt64(Session["UID"]));
                    Stream responseStream = response.GetResponseStream();
                    responseStr = new StreamReader(responseStream).ReadToEnd();
                    XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(responseStr));
                    var xmlWithoutNs = xmlDocumentWithoutNs.ToString();
                    Logger.LogMessage(TracingLevel.INFO, "RCT34 - XmlResultWithoutNs" + xmlWithoutNs);
                    _IGroupInsuranceBLL.SaveLogs ("RCT34 - XmlResultWithoutNs" + xmlWithoutNs, Convert.ToInt64(Session["UID"]));
                    //Log.Debug("Response KII Pending Request Data:  " + xmlWithoutNs);
                    //return Content(responseStr);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlWithoutNs);
                    //XmlNode node = doc.SelectSingleNode("Basic_vehicle_detailsResult");
                    //string jsonText = JsonConvert.SerializeXmlNode(doc);

                    // To convert JSON text contained in string json into an XML node
                    //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
                    ViewBag.Response = responseStr;
                    ViewBag.Response1 = doc.InnerText;
                    Logger.LogMessage(TracingLevel.INFO, "RCT34 - doc.InnerText" + doc.InnerText);
                    Logger.LogMessage(TracingLevel.INFO, "RCT34-Results" + responseStr);
                    _IGroupInsuranceBLL.SaveLogs("RCT34 - doc.InnerText" + doc.InnerText + " -- RCT34-Results" + responseStr, Convert.ToInt64(Session["UID"]));
                    //jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                    try
                    {
                        jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                        Logger.LogMessage(TracingLevel.INFO, "RCT34-ResultJsonConversion" + jsonText);
                        _IGroupInsuranceBLL.SaveLogs("RCT34-ResultJsonConversion" + jsonText, Convert.ToInt64(Session["UID"]));
                    }
                    catch (Exception ex)
                    {
                        string errtext = ex.Message.ToString();
                        Logger.LogMessage(TracingLevel.INFO, "RCT34-ResultJsonConversionError" + jsonText + "& ErrorMsg" + errtext);
                        _IGroupInsuranceBLL.SaveLogs("RCT34-ResultJsonConversionError" + jsonText + "& ErrorMsg" + errtext, Convert.ToInt64(Session["UID"]));
                    }

                    //jsonText = JsonConvert.SerializeXmlNode(doc);

                    //Status = "Payment pending at payment gateway";
                    //Status = jsonText;
                    DoubleVerificationDataResponseKII DVDataResponseK2 = new DoubleVerificationDataResponseKII();
                    DVDataResponseK2 = JsonConvert.DeserializeObject<DoubleVerificationDataResponseKII>(jsonText);
                    if (DVDataResponseK2.statusCode == "KII-RCTER-34")
                    {
                        DVDataResponseK2.status = "Department reference number not found";
                    }
                    else if (DVDataResponseK2.statusCode == "KII-RCTER-00")
                    {
                        if (DVDataResponseK2.pymntstatus == "10700066")
                        {
                            DVDataResponseK2.status = "Payment received at payment gateway";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700092")
                        {
                            DVDataResponseK2.status = "Payment pending at payment gateway";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700068")
                        {
                            DVDataResponseK2.status = "Payment failed at payment gateway";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700103")
                        {
                            DVDataResponseK2.status = "Transaction Failed";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700098")
                        {
                            DVDataResponseK2.status = "Challan Expired";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700072")
                        {
                            DVDataResponseK2.status = "Payment received at agency bank";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700070")
                        {
                            DVDataResponseK2.status = "Payment pending with agency bank";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720038")
                        {
                            DVDataResponseK2.status = "Cheque Bounced";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720072")
                        {
                            DVDataResponseK2.status = "Lost in transit";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720074")
                        {
                            DVDataResponseK2.status = "Mutilated";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720076")
                        {
                            DVDataResponseK2.status = "Signature Missing";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720078")
                        {
                            DVDataResponseK2.status = "Overwriting";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720080")
                        {
                            DVDataResponseK2.status = "Amount mismatch";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720082")
                        {
                            DVDataResponseK2.status = "Cheque/DD accepted, clearance awaited";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10700220")
                        {
                            DVDataResponseK2.status = "Payment not initiated at payment gateway";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720005")
                        {
                            DVDataResponseK2.status = "Challan Expired Reconcilation status";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720034")
                        {
                            DVDataResponseK2.status = "TTR in Progress";
                        }
                        else if (DVDataResponseK2.pymntstatus == "10720036")
                        {
                            DVDataResponseK2.status = "TTR Approved";
                        }
                    }
                    long EmpID = 0;
                    if (Session["UID"] != null)
                    {
                        EmpID = Convert.ToInt64(Session["UID"]);
                        Logger.LogMessage(TracingLevel.INFO, "RCT34-EmpID " + EmpID);
                        _IGroupInsuranceBLL.SaveLogs("RCT34-EmpID " + EmpID, Convert.ToInt64(Session["UID"]));
                    }
                    _IGroupInsuranceBLL.SaveLogs("GIS--Payment status" + DVDataResponseK2.pymntstatus, Convert.ToInt64(Session["UID"]));
                    Logger.LogMessage(TracingLevel.INFO, "GIS--Payment status" + DVDataResponseK2.pymntstatus);
                    string result = _IGroupInsuranceBLL.GISUpdateChallanStatusDll(ChallanRefNo, Convert.ToInt64(DVDataResponseK2.pymntstatus), EmpID);   //--dd  to be changed imp
                    Logger.LogMessage(TracingLevel.INFO, "GIS--RCT34-DBResult" + result);
                    _IGroupInsuranceBLL.SaveLogs("GIS--RCT34-DBResult" + result, Convert.ToInt64(Session["UID"]));
                    //return View(DVDataResponseK2);  
                    if (DVDataResponseK2.pymntstatus == "10700066")
                    {
                        try
                        {
                            Logger.LogMessage(TracingLevel.INFO, "GIS--SessionData Mobile&AppRefNo" + Session["MobileNo"].ToString() + Session["AppRefNo"].ToString());
                            _IGroupInsuranceBLL.SaveLogs("GIS--SessionData Mobile&AppRefNo" + Session["MobileNo"].ToString() + Session["AppRefNo"].ToString(), Convert.ToInt64(Session["UID"]));
                            //   var ApprefNo = (from ad in _db.tbl_kgid_application_details where ad.kad_emp_id == EmpID select ad.kad_kgid_application_number).ToList().LastOrDefault();
                            var ApprefNo = (from ad in _db.tbl_GIS_application_details where ad.gad_employee_id == EmpID select ad.gad_application_no).ToList().LastOrDefault();

                            //var mobile = (from eb in _db.tbl_employee_basic_details where eb.employee_id == EmpID select eb).FirstOrDefault();

                            //    string msg = "ನಿಮ್ಮ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ" + ApprefNo + "ರ ಪ್ರಾರಂಭಿಕ ಠೇವಣಿ ರೂ " + DVDataResponseK2.paidAmount + "ಗಳ ಪಾವತಿಯು ದಿನಾಂಕ " + DVDataResponseK2.currentTimeStamp + " ದಂದು ‌ಯಶಸ್ವಿಯಾಗಿದ್ದು, ಚಲನ್ ಸಂಖ್ಯೆ " + ChallanRefNo + " ಆಗಿರುತ್ತದೆ."

                            //+ "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                         //   var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587390729269); //--dd  changed
                            Logger.LogMessage(TracingLevel.INFO, "GIS--KIIDoubleVerification 10700066  1107161587390729269" );
                            _IGroupInsuranceBLL.SaveLogs("GIS--KIIDoubleVerification 10700066  1107161587390729269", Convert.ToInt64(Session["UID"]));

                            //msg = msg.Replace("#var1#", Session["AppRefNo"].ToString());
                            //msg = msg.Replace("#var2#", DVDataResponseK2.paidAmount.ToString());
                            //msg = msg.Replace("#var3#", DVDataResponseK2.currentTimeStamp.ToString());
                            //msg = msg.Replace("#var4#", ChallanRefNo);
                          //  Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification 10700066 replace of values" + msg.ToString());
                           //Logger.LogMessage(TracingLevel.INFO, "KIIDoubleVerification  10700066 mobile no." + Session["MobileNo"].ToString());

                           // AllCommon.sendUnicodeSMS(Session["MobileNo"].ToString(), msg, "1107161587390729269");
                            Logger.LogMessage(TracingLevel.INFO, "GIS--KIIDoubleVerification 10700066 SMS sent");
                            _IGroupInsuranceBLL.SaveLogs("GIS--KIIDoubleVerification 10700066 SMS sent", Convert.ToInt64(Session["UID"]));


                            string emailcontent = "Dear Proposer," +
                            "The challan reference number " + ChallanRefNo + " . having your Initial Deposit payment"
                            + "of Rs" + DVDataResponseK2.paidAmount + " . towards the proposal reference number" + Session["AppRefNo"].ToString() + ". has been "
                            + "Successfully received."
                            + "The Payment Receipt of Rs " + DVDataResponseK2.paidAmount + ". Is attached hereto. Please note the proposal reference number and quote the same in all your future correspondence "
                            + "Please note that the Initial deposit payment remitted by you is only for operational convenience and does not amount to automatic acceptance of the risk.Your proposal will be evaluated by the department and Policy bond will be issued once the same is approved."

                                + "In case of queries or assistance, Please call us on 080 - 22536189"

                                + "Looking forward and assuring of our best service."

                                    + "Warm Regards,"
                                    + "KGID, Official Branch";

                            Logger.LogMessage(TracingLevel.INFO, "GIS--KIIDoubleVerification 10700066  emailcontent" + emailcontent.ToString());
                            Logger.LogMessage(TracingLevel.INFO, "GIS--KIIDoubleVerification 10700066  email_id" + Session["EmailId"].ToString());

                            AllCommon objemail = new AllCommon();
                            //objemail.SendEmail(Session["EmailId"].ToString(), emailcontent, "Payment Successful");
                            Logger.LogMessage(TracingLevel.INFO, "GIS--KIIDoubleVerification 10700066 EMail sent");
                            _IGroupInsuranceBLL.SaveLogs("GIS--KIIDoubleVerification 10700066 EMail sent", Convert.ToInt64(Session["UID"]));

                        }
                        catch (Exception ex)
                        {
                            Logger.LogMessage(TracingLevel.INFO, "GIS--KIIDoubleVerification 10700066" + ex.Message.ToString());
                            _IGroupInsuranceBLL.SaveLogs("GIS--KIIDoubleVerification 10700066" + ex.Message.ToString(), Convert.ToInt64(Session["UID"]));
                        }


                        //   return RedirectToAction("ApplicationForm", "Employee", new RouteValueDictionary(new { pay = "true" }));  //  -- dd to be changed

                        return RedirectToAction("GISApplicationForEmployee", "GroupInsurance", new RouteValueDictionary(new { pay = "true" }));  //  -- dd to be changed
                    }
                    else
                    {

                        if (DVDataResponseK2.pymntstatus == "10700092" || DVDataResponseK2.pymntstatus == "10700070") //pending
                        {


                        }
                        if (DVDataResponseK2.pymntstatus == "10700068" || DVDataResponseK2.pymntstatus == "10700103" || DVDataResponseK2.pymntstatus == "10700098") //failed
                        {
                            try
                            {
                                Logger.LogMessage(TracingLevel.INFO, "GIS--SessionData Mobile&AppRefNo" + Session["MobileNo"].ToString() + Session["AppRefNo"].ToString());
                                //string msg = "ನಿಮ್ಮ ಪ್ರಸ್ತಾವನೆ ಸಂಖ್ಯೆ" + ApprefNo + " ರ ಪ್ರಾರಂಭಿಕ ಠೇವಣಿ ರೂ" + DVDataResponseK2.paidAmount + "ಗಳ ಪಾವತಿ ಪ್ರಯತ್ನ ಯಶಸ್ವಿಯಾಗಿರುವುದಿಲ್ಲ, https://kgidonline.karnataka.gov.in ನಲ್ಲಿ ಲಾಗಿನ್‌ ಆಗಿ ಮತ್ತೆ  ಪ್ರಯತ್ನಿಸಿ, ಹೆಚ್ಚಿನ ಮಾಹಿತಿಗಾಗಿ ಸಹಾಯವಾಣಿ ಸಂಖ್ಯೆ 080-22536189 ನ್ನು ಸಂಪರ್ಕಿಸಿ"
                                //           + "- ವಿಮಾ ಇಲಾಖೆ(KGID).";
                               // var msg = _INBApplicationbll.GetEmailSMSTemplate(1107161587458099452);   //--dd  changed
                                Logger.LogMessage(TracingLevel.INFO, "GIS--KIIDoubleVerification 10700068  1107161587458099452" );

                                //msg = msg.Replace("#var1#", Session["AppRefNo"].ToString());
                                //msg = msg.Replace("#var2#", DVDataResponseK2.paidAmount.ToString());
//
                                //Logger.LogMessage(TracingLevel.INFO, "GIS--KIIDoubleVerification 10700068  replace of values" + msg.ToString());
                                Logger.LogMessage(TracingLevel.INFO, "GIS--KIIDoubleVerification 10700068  replace of values" + Session["MobileNo"].ToString());
                                _IGroupInsuranceBLL.SaveLogs("GIS--KIIDoubleVerification 10700068  replace of values" + Session["MobileNo"].ToString(), Convert.ToInt64(Session["UID"]));

                               // AllCommon.sendUnicodeSMS(Session["MobileNo"].ToString(), msg, "1107161587458099452");
                                Logger.LogMessage(TracingLevel.INFO, "GIS--KIIDoubleVerification 10700068  SMS sent");
                                _IGroupInsuranceBLL.SaveLogs("GIS--KIIDoubleVerification 10700068  SMS sent", Convert.ToInt64(Session["UID"]));
                            }
                            catch (Exception ex)
                            {
                                Logger.LogMessage(TracingLevel.INFO, "GIS--KIIDoubleVerification 10700068" + ex.Message.ToString());
                                _IGroupInsuranceBLL.SaveLogs("GIS--KIIDoubleVerification 10700068" + ex.Message.ToString(), Convert.ToInt64(Session["UID"]));

                            }


                        }
                      //  return RedirectToAction("ApplicationForm", "Employee");  //-- dd to be changed
                        return RedirectToAction("GISApplicationForEmployee", "GroupInsurance");
                    }
                }
                else
                {
                    Logger.LogMessage(TracingLevel.INFO, "GIS--signed content error");
                    _IGroupInsuranceBLL.SaveLogs("GIS--signed content error", Convert.ToInt64(Session["UID"]));
                    // return RedirectToAction("ApplicationForm", "Employee");   //-- dd to be changed
                    return RedirectToAction("GISApplicationForEmployee", "GroupInsurance");
                }
            }
            //return RedirectToAction("ApplicationForm", "Employee");//-- dd to be changed
            return RedirectToAction("GISApplicationForEmployee", "GroupInsurance");
        }
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
        public string TextFileCreate(string ChallanRefNo)
        {
            Logger.LogMessage(TracingLevel.INFO, "TextFileCreate()");
            // KD0221801112345678
            //string dd = DateTime.Now.ToString("dd");
            //string MM = DateTime.Now.ToString("MM");
            //string yy = DateTime.Now.ToString("yy");
            //string ddHHmmss = DateTime.Now.ToString("ddHHmmss");
            //deptRefNum = "KD" + MM + yy + "8011" + ddHHmmss;
            //var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            string newFile = Server.MapPath("~/Documents/GISEmployeeDocuments/KIIRequest/DoubleVerification/" + ChallanRefNo + ".txt");
            //string fileName = @"D:\VenkatTXFITx.txt";
            System.IO.FileInfo fi = new System.IO.FileInfo(newFile);
            try
            {
                // Check if file already exists. If yes, delete it.     
                //if (fi.Exists)
                //{
                //    fi.Delete();
                //}

                // Create a new file     
                using (System.IO.StreamWriter strmwrtr = fi.CreateText())
                {
                    //strmwrtr.WriteLine("New file created: {0}", DateTime.Now.ToString());
                    //strmwrtr.WriteLine("Author: Venkatesh);
                    strmwrtr.WriteLine("<data>");
                    strmwrtr.WriteLine(" <RctReceivePaymentStatusRq>");
                    strmwrtr.WriteLine(" <deptRefNum>" + ChallanRefNo + "</deptRefNum>");
                    strmwrtr.WriteLine(" </RctReceivePaymentStatusRq>");
                    strmwrtr.WriteLine(" </data>");
                }

                Logger.LogMessage(TracingLevel.INFO, "Line catch" + newFile);
                return newFile;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        #endregion
        #region ContactUs
        [Route("GIS-kgid-cs")]
        public ActionResult ContactUs()
        {
            return View();
        }
        #endregion
        public ActionResult KIIDoubleVerificationTest(string ReqChallanRefNo, string Date)
        {
            string ChallanRefNo = ReqChallanRefNo;
            // string URL = "https://khajane2.karnataka.gov.in/KhajaneWs/rct/rrpys/secbc/RctReceivePaymentStatusService?wsdl";
            //  string URL = "https://preprodkhajane2.karnataka.gov.in/KhajaneWs/rct/rrvcs/secbc/RctReceiveValidateChlnService?wsdl";


            string URL = System.Configuration.ConfigurationManager.AppSettings["preprodkhajane2test"];

            string transactiondate = Date;
            ByteArrayOutputStream fileWriter = null;
            StringBuilder content = null;
            string currPath = string.Empty;
            string SignedresultContent = string.Empty;
            string KIIsignresponse = string.Empty;
            string reqFile = TextFileCreate(ChallanRefNo);
            XMLInputFactory factory = XMLInputFactory.newInstance();
            FileReader fileReader = new FileReader(reqFile);
            string AsBase64String = string.Empty;
            byte[] AsBytes = System.IO.File.ReadAllBytes(reqFile);
            fileReader = new FileReader(reqFile);
            XMLStreamReader reader = factory.createXMLStreamReader(fileReader);
            content = new StringBuilder();
            // Parsing XML using stream reader and writing to a ByteArrayOutputStream
            while (reader.hasNext())
            {
                int eventType = reader.next();
                switch (eventType)
                {

                    case XMLEvent.START_ELEMENT:

                        currPath = currPath + "/" + reader.getLocalName();
                        //Instead
                        if (currPath.Contains("data"))
                        {
                            String startTag = "";
                            //Instead
                            if (reader.getLocalName().Equals("data"))
                            {
                                fileWriter = new ByteArrayOutputStream();
                                startTag = "<" + reader.getLocalName();
                                for (int k = 0; k < reader.getNamespaceCount(); k++)
                                {
                                    if (reader.getNamespacePrefix(k) != null)
                                        startTag = startTag + " xmlns:" + reader.getNamespacePrefix(k) + "=\"" + reader.getNamespaceURI(k) + "\"";
                                    else
                                        startTag = startTag + " xmlns=\"" + reader.getNamespaceURI(k) + "\"";
                                }
                                startTag = startTag + ">";
                            }
                            else
                            {
                                startTag = "<" + reader.getLocalName() + ">";
                            }

                            if (fileWriter != null)
                                fileWriter.write(Encoding.ASCII.GetBytes(startTag));
                        }
                        break;

                    case XMLStreamConstants.CHARACTERS:
                        if (fileWriter != null)
                        {
                            fileWriter.write(Encoding.ASCII.GetBytes(reader.getText()));
                        }
                        break;

                    case XMLStreamConstants.END_ELEMENT:
                        //Instead
                        if (currPath.Contains("data"))
                        {
                            string endTag = "</" + reader.getLocalName() + ">";

                            if (fileWriter != null)
                            {
                                fileWriter.write(Encoding.ASCII.GetBytes(endTag));
                            }
                        }
                        content = new StringBuilder();
                        //RemoveLasttag(currPath);
                        currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
                        break;

                    case XMLEvent.CDATA:
                        break;
                    case XMLEvent.SPACE:
                        break;

                }
            }
            com.sun.org.apache.xml.@internal.security.Init.init();
            Canonicalizer canon = Canonicalizer.getInstance(Canonicalizer.ALGO_ID_C14N_OMIT_COMMENTS);
            byte[] canonXmlBytes = canon.canonicalize(fileWriter.toByteArray());
            string beforesignedData = Convert.ToBase64String(canonXmlBytes);
            string beforecanonXmlData = Encoding.UTF8.GetString(AsBytes);
            string aftercanonXmlData = Encoding.UTF8.GetString(canonXmlBytes);

            //SIGN DATA WITH PFX FILE
            string xml_inBase64 = Convert.ToBase64String(AsBytes);
            string em = Encoding.UTF8.GetString(canonXmlBytes);

            Logger.LogMessage(TracingLevel.INFO, "10.10.31.231:8080/SignXmlData-Before convertion");
            //WebAPI Service Call
            using (var client = new HttpClient())
            {
                string vSignXmlDataURI = System.Configuration.ConfigurationManager.AppSettings["SignXmlDataURI"];

                client.BaseAddress = new Uri(vSignXmlDataURI);

              //  client.BaseAddress = new Uri("http://10.10.31.231:8080/SignXmlData/");

                object reqdata = new
                {
                    data = beforesignedData
                };
                var myContent = JsonConvert.SerializeObject(reqdata);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                /////////
                var resultsac = client.PostAsync("getSignforDataByte", byteContent).Result;
                SignedresultContent = resultsac.Content.ReadAsStringAsync().Result;
                client.CancelPendingRequests();
                //Console.WriteLine("about to dispose the client");
                client.Dispose();
                //return resultContent;
                Logger.LogMessage(TracingLevel.INFO, "RCT34-SignedResultData" + SignedresultContent);
            }
            Logger.LogMessage(TracingLevel.INFO, "10.10.31.231:8080/SignXmlData-After convertion");
            _IGroupInsuranceBLL.SaveLogs("10.10.31.231:8080/SignXmlData-After convertion", Convert.ToInt64(Session["UID"]));
            ////////
            if (SignedresultContent != null)
            {
                string xml2 = @"<?xml version='1.0' encoding='utf-8'?>"
    +
    "<soapenv:Envelope xmlns:soapenv=" + "'" + "http://schemas.xmlsoap.org/soap/envelope/" + "'" + " xmlns:ser=" + "'" + "http://service.receivepymntstatus.dept.rct.integration.ifms.gov.in/" + "'" + " xmlns:head=" + "'" + "http://header.ei.integration.ifms.gov.in/" + "'" + ">"
    + "\n"
    + "   <soapenv:Header>"
    + "\n"
    + "      <ser:Header>"
    + "\n"
    + "         <head:agencyCode>EA_KID</head:agencyCode>"
     + "\n"
    + "         <head:integrationCode>RCT034</head:integrationCode>"
     + "\n"
    + "         <head:uirNo>EA_KID-RCT034-" + transactiondate + "-" + ChallanRefNo + "</head:uirNo>"
                + "\n"
    + "      </ser:Header>"
     + "\n"
    + "   </soapenv:Header>"
     + "\n"
    + "   <soapenv:Body>"
     + "\n"
    + "      <ser:envelopedDataReq>"
     + "\n"
    + "         <Signature>" + SignedresultContent + "</Signature>"
    + "\n"
    + beforecanonXmlData
    + "      </ser:envelopedDataReq>"
     + "\n"
    + "   </soapenv:Body>"
     + "\n"
    + "</soapenv:Envelope>";

                string responseStr = string.Empty;
                string jsonText = string.Empty;
                //Logger.LogMessage(TracingLevel.INFO, "RCT34-Request for Signature Validation");
                //Log.Debug("Request for Signature Validation");
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(URL);
                byte[] bytes;
                bytes = Encoding.UTF8.GetBytes(xml2);
                request.ContentType = "text/xml";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                //Log.Debug("FileStream Rquesting for Pending Payment");
                Logger.LogMessage(TracingLevel.INFO, "RCT34-FileStream Rquesting for Pending Payment");
                _IGroupInsuranceBLL.SaveLogs("RCT34-FileStream Rquesting for Pending Payment", Convert.ToInt64(Session["UID"]));
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                //Log.Debug("FileStream Rquest for Pending Payment Proceed");
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                Logger.LogMessage(TracingLevel.INFO, "RCT34-FileStream Rquest for Pending Payment Proceed");
                _IGroupInsuranceBLL.SaveLogs("RCT34-FileStream Rquest for Pending Payment Proceed", Convert.ToInt64(Session["UID"]));
                //Log.Debug("Response from KII Pending Payment Request:  " + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Logger.LogMessage(TracingLevel.INFO, "RCT34-HttpWebResponse:OK");
                    _IGroupInsuranceBLL.SaveLogs("RCT34-HttpWebResponse:OK", Convert.ToInt64(Session["UID"]));
                    Stream responseStream = response.GetResponseStream();
                    responseStr = new StreamReader(responseStream).ReadToEnd();
                    XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(responseStr));
                    var xmlWithoutNs = xmlDocumentWithoutNs.ToString();
                    Logger.LogMessage(TracingLevel.INFO, "RCT34 - XmlResultWithoutNs" + xmlWithoutNs);
                    _IGroupInsuranceBLL.SaveLogs("RCT34 - XmlResultWithoutNs" + xmlWithoutNs, Convert.ToInt64(Session["UID"]));
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlWithoutNs);
                    //XmlNode node = doc.SelectSingleNode("Basic_vehicle_detailsResult");
                    //string jsonText = JsonConvert.SerializeXmlNode(doc);

                    // To convert JSON text contained in string json into an XML node
                    //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
                    ViewBag.Response = responseStr;
                    ViewBag.Response1 = doc.InnerText;
                    Logger.LogMessage(TracingLevel.INFO, "RCT34 - doc.InnerText" + doc.InnerText);
                    Logger.LogMessage(TracingLevel.INFO, "RCT34-Results" + responseStr);
                    _IGroupInsuranceBLL.SaveLogs("RCT34 - doc.InnerText" + doc.InnerText + " --RCT34-Results" + responseStr, Convert.ToInt64(Session["UID"]));
                    //jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                    try
                    {
                        Logger.LogMessage(TracingLevel.INFO, "RCT34 -XML Doc To convert JSON Start");
                        jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0], Newtonsoft.Json.Formatting.None, true);
                        Logger.LogMessage(TracingLevel.INFO, "RCT34-ResultJsonConversion" + jsonText);
                        _IGroupInsuranceBLL.SaveLogs("RCT34-ResultJsonConversion" + jsonText, Convert.ToInt64(Session["UID"]));
                    }
                    catch (Exception ex)
                    {
                        string errtext = ex.Message.ToString();
                        Logger.LogMessage(TracingLevel.INFO, "RCT34-ResultJsonConversionError" + jsonText + "& ErrorMsg" + errtext);
                        _IGroupInsuranceBLL.SaveLogs("RCT34-ResultJsonConversionError" + jsonText + "& ErrorMsg" + errtext, Convert.ToInt64(Session["UID"]));
                    }

                    DoubleVerificationDataResponseKII DVDataResponseK2 = new DoubleVerificationDataResponseKII();
                    DVDataResponseK2 = JsonConvert.DeserializeObject<DoubleVerificationDataResponseKII>(jsonText);
                    Logger.LogMessage(TracingLevel.INFO, "RCT34-AfterJsonConversion" + jsonText + "& StatusCode: " + DVDataResponseK2.statusCode + "&&" + "PaymentStatus: " + DVDataResponseK2.pymntstatus);
                    _IGroupInsuranceBLL.SaveLogs("RCT34-AfterJsonConversion" + jsonText + "& StatusCode: " + DVDataResponseK2.statusCode + "&&" + "PaymentStatus: " + DVDataResponseK2.pymntstatus, Convert.ToInt64(Session["UID"]));
                    string TestResult = "StatusCode" + DVDataResponseK2.statusCode + "PaymentStatus:" + DVDataResponseK2.pymntstatus;
                    return Json(TestResult, JsonRequestBehavior.AllowGet);
                    //return View(DVDataResponseK2); 
                }
                else
                {
                    Logger.LogMessage(TracingLevel.INFO, "signed content error");
                    _IGroupInsuranceBLL.SaveLogs("signed content error", Convert.ToInt64(Session["UID"]));

                    string TestResultError = "signed content error";
                    return Json(TestResultError, JsonRequestBehavior.AllowGet);
                }
            }
            string TestResultHome = "Unable To Verify";
            return Json(TestResultHome, JsonRequestBehavior.AllowGet);
        }
    }
}