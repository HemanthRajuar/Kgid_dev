using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace KGID_Models.KGID_GroupInsurance
{
 public   class tbl_GIS_Subcription_Master
    {
        [Key]

        public  long GSM_Subcription_ID{ get; set; }
        public int  GSM_Group_ID { get; set; }
        public string GSM_Group_desc { get; set; }
        public int GSM_InsuranceFund { get; set; }
        public int GSM_SavingFund { get; set; }
        public int GSM_Total { get; set; }
        public int GSM_InsuranceCover { get; set; }
        public int GSM_Active_Status { get; set; }
        public DateTime? GSM_Creation_datetime { get; set; }
        public DateTime? GSM_Updation_datetime { get; set; }
        public long? GSM_created_by { get; set; }
        public long? GSM_Updated_by { get; set; }
    }
}
