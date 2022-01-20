using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_Master
{
  public   class tbl_ErrorLog
    {
        [Key]
        public long ErrorLogID { get; set; }
        public string ErrorDesc { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
