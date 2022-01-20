using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.Ticketing_Tool
{
    public partial class VehicalMappingToDDO
    {
        public int mivd_vehicle_details_id { get; set; }
        public string mivd_registration_no { get; set; }
        public string rto_desc { get; set; }
        public string mivd_type_of_model { get; set; }
        public string year_desc { get; set; }
        public int employee_id { get; set; }
        public string mivd_chasis_no { get; set; }
        public string mia_owner_of_the_vehicle { get; set; }
    }
}
