using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
    public class tbl_claim_subtype
    {
        [Key]
        public long cst_claim_subtype_id { get; set; }
        public string cst_claim_subtype_desc { get; set; }
        public long cst_claim_type_id { get; set; }
        public int cst_status { get; set; }
        public string cst_created_by { get; set; }
        public DateTime? cst_creation_datetime { get; set; }
        public string cst_updated_by { get; set; }
        public DateTime? cst_updation_datetime { get; set; }
    }
}
