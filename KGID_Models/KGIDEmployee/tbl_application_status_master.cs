using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KGID_Models.KGIDEmployee
{
    public class tbl_application_status_master
    {
        [Key]
        public int asm_status_id { get; set; }
        public string asm_status_desc { get; set; }
        public int asm_module_id { get; set; }
        public int asm_active { get; set; }
     
    }
}
