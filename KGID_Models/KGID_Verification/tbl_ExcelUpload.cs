using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Verification
{
  public  class tbl_ExcelUpload
    {[Key]
        public int ExcelID { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Registrationno { get; set; }
        public string Remarks { get; set; }
    }
}
