using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGIDMotorInsurance
{
    class VM_MI_Upload_Documentsexcel
    {

        public string OtherDocument1_filename { get; set; }

        public HttpPostedFileBase OtherDocument2 { get; set; }
        public string OtherDocument2_filename { get; set; }
    }
}
