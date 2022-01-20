using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
   public class tbl_GIS_NR_upload_form
    {
        [Key]
        public long App_Id { get; set; }
         public long App_Employee_Code { get; set; }
        public long App_gnwt_workflow_id { get; set; }
        public string App_Form6 { get; set; }
        public string App_Form7 { get; set; }
        public int App_Active { get; set; }
        public DateTime App_Creation_Date { get; set; }
        public long App_Created_By { get; set; }
        public long? App_ApplicationID { get; set; }
    }
}
