using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDNBApplication
{
    public class tbl_employee_address_details
    {
        [Key]
        public long ead_address_id { get; set; }
        //  public int ead_id { get; set; }
        // public Nullable<long> ead_sys_emp_code { get; set; }
        // [Required(ErrorMessage = "Please Enter Address")]
        //devika
        //public string ead_address { get; set; }
        [Required(ErrorMessage = "Please Enter Pincode")]
        //devika
        //  public string ead_pincode { get; set; }
        public Nullable<int> ead_pincode { get; set; }
        // public Nullable<System.DateTime> ead_date { get; set; }
        //public Nullable<bool> ead_active { get; set; }

        //devika
        //public Nullable<int> ead_created_by { get; set; }
        //public Nullable<int> ead_updated_by { get; set; }

        public Nullable<long> ead_created_by { get; set; }
        public Nullable<long> ead_updated_by { get; set; }
        public Nullable<System.DateTime> ead_creation_datetime { get; set; }
        public Nullable<System.DateTime> ead_updation_datetime { get; set; }

        //Added by devika
        //    public int ead_address_id { get; set; }        
        public long ead_emp_id { get; set; }
        public Nullable<bool> ead_active_status { get; set; }
        public long ead_application_id { get; set; }


        public string ead_address { get; set; }

    }
}
