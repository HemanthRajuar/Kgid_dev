using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KGID_Models.Attrebute;
using KGID_Models.KGIDNBApplication;

namespace KGID_Models.KGID_GroupInsurance
{
    public class GIS_Ledger
    {

        public GIS_Ledger()
        {
            listgisledgerdet = new List<GIS_Ledger_det>();
        }
        public long employee_id { get; set; }
        public string employee_name { get; set; }
        public string father_name { get; set; }
        public string spouse_name { get; set; }
        public List<GIS_Ledger_det> listgisledgerdet { get; set; }       

    }

    public class GIS_Ledger_det
    {
        public long employee_id { get; set; }
        public long LedgerId { get; set; }
        public Nullable<decimal> SR { get; set; }
        public string MNYEAR { get; set; }
        public string EMPCODE { get; set; }
        public Nullable<decimal> CURR_OPN { get; set; }
        public Nullable<decimal> CONT_GIS { get; set; }
        public Nullable<decimal> CURR_CLS { get; set; }
        public Nullable<decimal> INT_AMT { get; set; }
        public Nullable<decimal> INT_RATE { get; set; }
        public Nullable<decimal> MON { get; set; }
        public Nullable<decimal> YR { get; set; }
        public Nullable<decimal> MON_INT_AMT { get; set; }


        public Nullable<decimal> SAVING_FUND { get; set; }
        public Nullable<decimal> INSURANCE_FUND { get; set; }

        public string grp { get; set; }
        public string remark { get; set; }

        public int  Total { get; set; }
    }


}
