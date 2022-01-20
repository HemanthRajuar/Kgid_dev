using KGID_Models.KGID_Master;
using KGID_Models.KGID_User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
    public class VM_ClaimLedger
    {

        public VM_ClaimLedger()
        {
            ledger1 = new List<GIS_Ledger_det>();
            ledgerList = new List<VM_ClaimLedger>();
            monthlist=new  List<tbl_month_master>();
            yearlist = new List<tbl_year_master>();
            grouplist = new List<tbl_employee_group_master>();
        }
    
        public IList<GIS_Ledger_det> ledger1 { get; set; }

      

        public IList<tbl_month_master> monthlist { get; set; }
        public IList<tbl_year_master> yearlist { get; set; }
        public IList<tbl_employee_group_master> grouplist { get; set; }
        //public IList<PrescriptionModel> PrescriptionModel { get; set; }
        //public PrescriptionModel newPrescriptionModel { get; set; }

        public IList<VM_ClaimLedger> ledgerList { get; set; }
        public VM_ClaimLedger newledger { get; set; }

        public long employee_id { get; set; }
        public string employee_name { get; set; }
        public string father_name { get; set; }
        public string spouse_name { get; set; }
        public List<GIS_Ledger_det> listgisledgerdet { get; set; }

        //
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
        public Nullable<decimal> Total { get; set; }
        public string remark { get; set; }
        public class GIS_Ledger_det
        {
            public long  LedgerId { get; set; }
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

        }

        
    }
}
