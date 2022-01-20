using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
    public class tbl_GIS_family_type_master
    {
        [Key]
        public int  gftm_family_type_id {get;set;}
        public string gftm_family_type { get; set; }
        public DateTime? gftm_creation_datetime { get; set; }
        public DateTime? gftm_updation_datetime { get; set; }
        public long? gftm_created_by { get; set; }
        public long? gftm_updated_by { get; set; }
    }
}
