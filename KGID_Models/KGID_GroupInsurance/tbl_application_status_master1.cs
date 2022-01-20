using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
    public class tbl_application_status_master1
    {
        [Key]
        public long asm_status_id           {get;set;}       
        public string asm_status_desc { get;set;}
        public int asm_module_id           {get;set;}
        public int  asm_active              {get;set;}
        public DateTime asm_creation_datetime   {get;set;}
        public long asm_created_by          {get;set;}
        public DateTime asm_updation_datetime   {get;set;}
        public long asm_updated_by         { get; set; }
    }
}
