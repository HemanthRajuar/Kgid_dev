using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
   public class tbl_GIS_NomineeReq_workflow_details
    {
        [Key]

        public long gnwt_workflow_id { get; set; }
        public long gnwt_application_id { get; set; }
        public long gnwt_verified_by { get; set; }
        public long gnwt_assigned_to { get; set; }
        public string gnwt_remarks { get; set; }
        public string gnwt_comments { get; set; }
        public int gnwt_nr_application_status { get; set; }
        public int gnwt_active_status { get; set; }
        public long gnwt_created_by { get; set; }
        public DateTime gnwt_creation_datetime { get; set; }
        public long gnwt_updated_by { get; set; }
        public DateTime gnwt_updation_datetime { get; set; }
    }
}
