using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
 public   class tbl_year_master
    {
        [Key]
        public int ym_year_id { get; set; }
        public string ym_year_desc { get; set; }
        public int ym_active_status { get; set; }
        public Nullable<DateTime> ym_creation_datetime { get; set; }
        public Nullable<DateTime> ym_updation_datetime { get; set; }
        public Nullable<int> ym_created_by { get; set; }
        public Nullable<int> ym_updated_by { get; set; }
    }
}
