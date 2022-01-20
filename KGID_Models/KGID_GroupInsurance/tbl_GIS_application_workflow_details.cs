using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
    public class tbl_GIS_application_workflow_details
    {
        [Key]
      public long gawt_workflow_id  {get;set;}
      public long gawt_application_id {get;set;}
      public int gawt_InsuranceAmt {get;set;}
      public int gawt_savingAmt {get;set;}
      public DateTime gawt_InsuranceDate {get;set;}
      public DateTime gawt_savingDate {get;set;}
      public long gawt_verified_by {get;set;}
      public long gawt_assigned_to {get;set;}
      public string gawt_remarks  {get;set;}
      public string gawt_comments {get;set;}
      public int gawt_application_status {get;set;}
      public int gawt_active_status {get;set;}
      public long gawt_created_by {get;set;}
      public DateTime gawt_creation_datetime {get;set;}
      public long gawt_updated_by{ get; set; }
      public DateTime gawt_updation_datetime { get; set; }
    }
}
