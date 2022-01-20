using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
    public class TBL_GIS_FUND_MONTHWISE
    {
      [Key]
      public long   LedgerId            {get;set;}
      public long   employee_id         {get;set;}
      public int   YR                  {get;set;}
      public int   MON                 {get;set;}
      public decimal   SAVING_FUND         {get;set;}
      public decimal INSURANCE_FUND      {get;set;}
      public string   grp                 {get;set;}
      public string   remark              {get;set;}
      public long   gfm_created_by      {get;set;}
      public DateTime   gfm_creation_datetime {get;set;}
      public long   gfm_updated_by      {get;set;}
      public DateTime   gfm_updation_datetime { get; set; }
        public decimal Total { get; set; }
        public int gfm_active_Status { get; set; }
    }
}
