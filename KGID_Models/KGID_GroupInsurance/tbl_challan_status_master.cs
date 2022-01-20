using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
    public class tbl_challan_status_master
    {
        [Key]
        public int csm_status_id {get;set;}
        public long csm_status_code { get;set;}
        public string csm_status { get;set;}
        public int csm_active_status {get;set;}
        public DateTime csm_created_date {get;set;}
        public long csm_created_by {get;set;}
        public DateTime csm_updated_date {get;set;}
        public long csm_updated_by { get;set;}
    }
}
