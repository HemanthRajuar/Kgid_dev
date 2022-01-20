using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
   public  class VM_GISWorkflowDetail
    {
        public string ApplicationRefNo { get; set; }
        public string NameOfApplicant { get; set; }
        public string VerifiedBy { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime CreationDateTime { get; set; }


        public string From { get; set; }
        public string To { get; set; }

        public long orderid { get; set; }

    }
}
