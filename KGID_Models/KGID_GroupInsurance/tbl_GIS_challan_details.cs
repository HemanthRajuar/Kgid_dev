using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
    public class tbl_GIS_challan_details
    {
        [Key]
public long gcd_challan_id { get; set; }
public string gcd_challan_ref_no{ get; set; }
public int gcd_purpose_id { get; set; }
public int gcd_sub_purpose_id{ get; set; }
public int gcd_amount{ get; set; }
public long gcd_application_id{ get; set; }
public long gcd_emp_id{ get; set; }
public long gcd_dio_id{ get; set; }
public DateTime gcd_date_of_generation{ get; set; }
public long gcd_agency_ddo_id{ get; set; }
public int gcd_active_status{ get; set; }
public DateTime gcd_creation_datetime{ get; set; }
public DateTime gcd_updation_datetime { get; set; }
public long gcd_created_by { get; set; }
public long gcd_updated_by { get; set; }
public int gcd_user_category { get; set; }
public int gcd_module_id { get; set; }
public string gcd_file_name_xml { get; set; }
public string gcd_ack_status_flag { get; set; }
public string gcd_ack_status_msg { get; set; }
public string gcd_voucher_no { get; set; }
public string gcd_request_sent { get; set; }
public string gcd_ddo_code { get; set; }
public string gcd_hoa_code { get; set; }
 public int gcd_insurance_amount { get; set; }
 public int gcd_savings_amount { get; set; }

    }
}
