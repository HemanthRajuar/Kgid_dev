using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDEmployee
{
    public class tbl_role_mapping
    {
        [Key]
        public long rm_id { get; set; }
        public long rm_emp_id { get; set; }
        public int rm_ddo_id { get; set; }
        public int rm_category_id { get; set; }
        public int rm_primary_charge { get; set; }
        public int rm_isactive { get; set; }
    }
}
