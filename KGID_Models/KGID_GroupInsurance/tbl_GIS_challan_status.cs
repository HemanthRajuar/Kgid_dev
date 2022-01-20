using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
  
    public class tbl_GIS_challan_status
    {
  [Key]
        public long  gcs_challan_status_id   { get; set; }
        public long  gcs_challan_id          { get; set; }
        public string  gcs_transaction_ref_no  { get; set; }
        public int  gcs_amount              { get; set; }
        public DateTime gcs_date_of_transaction { get; set; }
        public long  gcs_status              { get; set; }
        public int  gcs_active_status       { get; set; }
        public DateTime gcs_creation_datetime   { get; set; }
        public DateTime  gcs_updation_datetime   { get; set; }
        public long  gcs_created_by          { get; set; }
        public long  gcs_updated_by          { get; set; }
        public string gcs_service_response    { get; set; }
    }
}
