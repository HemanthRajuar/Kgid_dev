using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
    public class tbl_GIS_application_details
    {
        [Key]
        public long gad_application_id { get; set; }
        public long gad_application_no { get; set; }
        public long gad_employee_id { get; set; }
        public int gad_active { get; set; }
        public long gad_created_by { get; set; }
        public DateTime? gad_creation_date { get; set; }
        public long gad_updated_by { get; set; }
        public DateTime? gad_updated_date { get; set; }
       
    }
}

