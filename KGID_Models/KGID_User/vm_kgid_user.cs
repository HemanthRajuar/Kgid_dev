using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_User
{
    public class vm_kgid_user
    {
        public int um_user_id { get; set; }
        public string um_user_name { get; set; }
        public string um_user_password { get; set; }
        public string um_user_New_password { get; set; }
        public string um_user_Confirm_password { get; set; }
        public string um_kgid_number { get; set; }
        public int? um_status { get; set; }
        public string Message { get; set; }
        public string um_role { get; set; }
        public bool um_agency_status { get; set; }
        public long um_agency_id { get; set; }
        public DateTime um_creation_datetime { get; set; }
        public int um_created_by { get; set; }
		public string um_mobile_no { get; set; }
        public string um_email_id { get; set; }
        public bool um_MB_value { get; set; }
    }
}
