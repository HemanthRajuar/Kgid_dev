using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGID_Models.KGID_GroupInsurance
{
    public class VM_GISPaymentDetails
    {
        public int gcd_challan_id { get; set; }
        public string gcd_challan_ref_no { get; set; }
        public int gcd_purpose_id { get; set; }
        public int gcd_sub_purpose_id { get; set; }
        public int gcd_amount { get; set; }
        public long gcd_application_id { get; set; }
        public string gcd_date_of_generation { get; set; }
        public string gcs_transaction_ref_no { get; set; }
        public int gcs_amount { get; set; }
        public long gcs_status { get; set; }
        //public bool cs_status { get; set; }
        public int receipttypeid { get; set; }
        public string ddo_code { get; set; }
        public int hoa_id { get; set; }
        public long EmpID { get; set; }
        public int gcs_challan_id { get; set; }
        public string cs_transsaction_date { get; set; }
        public string hoa { get; set; }
        public string purpose_desc { get; set; }
        public string sub_purpose_desc { get; set; }
        public string receipt_type_desc { get; set; }
        public string PayStatus { get; set; }
        public string EmpName { get; set; }

        public long  ddo_code_id { get; set; }
        public int gcd_insurance_amount { get; set; }
        public int gcd_savings_amount { get; set; }


        public List<SelectListItem> PurposeTypes { get; set; }
        public List<SelectListItem> SubPurposeTypes { get; set; }
        public List<SelectListItem> ReceiptTypes { get; set; }
        public List<SelectListItem> HOATypes { get; set; }

        public string hoa1 { get; set; }
        public int gcd_purpose_id1 { get; set; }
        public int gcd_sub_purpose_id1 { get; set; }

        public string purpose_desc1 { get; set; }
        public string sub_purpose_desc1 { get; set; }
    }
}
