using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
  public  class tbl_GIS_Subcription_details
    {
        [Key]
     public long  gpd_id                   {get;set;}
     public long  gpd_empId                {get;set;}
     public long gpd_GISPolicyNum         { get;set;}
     public int  gpd_appliedforGis        {get;set;}
     public int  gdp_activeStatus         {get;set;}
     public long  gdp_updated_by           {get;set;}
     public long  gdp_created_by           {get;set;}
     public DateTime  gdp_creation_datetime    {get;set;}
     public DateTime gdp_updation_datetime    { get; set; }
   public long gpd_ApplicationId { get; set; }

        
    }
}
