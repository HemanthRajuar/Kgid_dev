using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_assign_application
    {
        [Key]
        public int ModuleType { get; set; }
        public long ap_emp_id { get; set; }
        public bool ap_active_status { get; set; }
        public int ap_category_id { get; set; }
        public int ap_ddocode_id { get; set; }
        public int ap_dist_id { get; set; }
        public DateTime ap_updation_datetime { get; set; }
        public DateTime ap_creation_datetime { get; set; }
        public long ap_created_by { get; set; }
        public long ap_updated_by { get; set; }
        public long? ap_no_of_application { get; set; }
        public DateTime? ap_assign_date { get; set; }
        public int ap_assign_id { get; set; }
    } 
}

    
