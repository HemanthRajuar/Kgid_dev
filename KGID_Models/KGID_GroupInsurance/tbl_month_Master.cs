using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
 public class tbl_month_Master
    {
        [Key]
        public int mm_month_id { get; set; }
        public string mm_month_desc { get; set; }
        public int mm_active_status { get; set; }
        public Nullable<DateTime> mm_creation_datetime { get; set; }
        public Nullable<DateTime> mm_updation_datetime { get; set; }
        public Nullable<int> mm_created_by { get; set; }
        public Nullable<int> mm_updated_by { get; set; }
    }
}
