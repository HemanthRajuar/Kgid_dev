using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
    public class tbl_claim_type_master
    {
        [Key]
        public long ctm_claim_type_id { get; set; }
        public string ctm_claim_type { get; set; }
        public int ctm_status { get; set; }
        public string ctm_created_by { get; set; }
        public DateTime? ctm_creation_datetime { get; set; }
        public string ctm_updated_by { get; set; }
        public DateTime? ctm_updation_datetime { get; set; }
    }
}
