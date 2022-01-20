using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.NBApplication
{
    public class VM_ApplicationDetail
    {
        public string ApplicationNumber { get; set; }
        public long ApplicationId { get; set; }
        public string QRCode { get; set; }
        public int ApplicationCount { get; set; }
        public int SentBackAppliaction { get; set; }
        public int RestrictApplyingPolicy { get; set; }
        public int Remarks { get; set; }
        public long PaymentStatus { get; set; }
        public string appSubmittedDate { get; set; }
        public int cd_active_status { get; set; }
        public long cd_challan_id { get; set; }
        public string cd_challan_ref_no { get; set; }
       
        public int? amount { get; set; }
        public string Employe_name { get; set; }
        public long challan_status_id { get; set; }
        public long Emp_Id { get; set; }

        public string kawt_remarks { get; set; }
        public string kawt_comments { get; set; }
        public long  kawt_verified_by { get; set; }
        public long kawt_assigned_to { get; set; }
        public long kawt_workflow_id { get; set; }
        public int kawt_application_status { get; set; }
        public long first_kgid_policy_no { get; set; }

        public string kawt_remarks_discription { get; set; }
        public string kawt_AssignedTo_Name{ get; set; }
        public string kawt_verifiedBy_Name { get; set; }
        public string kawt_applStatus_discription { get; set; }
        public long kawt_AssignedTo_KgidNo { get; set; }
        public long kawt_verifiedBy_KgidNo { get; set; }

        public int roleid { get; set; }

        public string Challen_TransactionNo { get; set; }
 
        public VM_ApplicationDetail()
        {
            listTrackDetails = new List<VM_trackDetails>();
           
        }
        public List<VM_trackDetails> listTrackDetails { get; set; }
        
    }
    public class VM_trackDetails
    {
        public string application_no { get; set; }
        public string kgid_policy_number { get; set; }
        public string districtNames { get; set; }
        public string assigned_date { get; set; }
        public string application_status { get; set; }

        
        public string RM_Remarks_Desc { get; set; }
        public string RM_Comments { get; set; }

        public string active_status { get; set; }
        public string kawt_application_id { get; set; }

        public string employee_id { get; set; }


    }
   
}
