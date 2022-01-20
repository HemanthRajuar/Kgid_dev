using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGID_GroupInsurance
{
   public class VM_GIS_Upload_EmployeeForm
    {

        public long App_Employee_Code { get; set; }
        public HttpPostedFileBase ApplicationFormDoc { get; set; }
        public HttpPostedFileBase Form6Doc { get; set; }
        public HttpPostedFileBase Form7Doc { get; set; }
        public string ApplicationFormDocName { get; set; }
        public string Form6DocName { get; set; }
        public string Form7DocName { get; set; }
        public long App_ApplicationID { get; set; }
     
    }
}
