using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class tbl_mi_renewal_policy_details
    {[Key]
        public long rp_mi_renewal_policy_id { get; set; }
        public string rp_mi_policy_number { get; set; }
        public long rp_mi_emp_id { get; set; }
        public double? p_mi_premium { get; set; }
        //public float MIPremium { get; set; }
        public long rp_mi_renewal_application_id { get; set; }
        public string rp_mi_category_id { get; set; }
    }
}
