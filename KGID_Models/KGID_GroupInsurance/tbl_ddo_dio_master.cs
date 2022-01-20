using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
    public class tbl_ddo_dio_master
    {
        [Key]
        public long dd_dio_id             {get;set;}
        public string dd_dio_office         {get;set;}
        public int dd_ddo_id             {get;set;}
        public int dd_district_id        {get;set;}
        public bool dd_status             {get;set;}
        public Nullable<DateTime> dd_creation_datetime  {get;set;}
        public Nullable<DateTime> dd_updation_datetime  {get;set;}
        public Nullable<long> dd_created_by         {get;set;}
        public Nullable<long> dd_updated_by         {get;set;}
        public int dm_taluka_id          { get; set; }
}
}
