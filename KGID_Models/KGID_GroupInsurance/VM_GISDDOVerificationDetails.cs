using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
  public  class VM_GISDDOVerificationDetails
    {

        public VM_GISDDOVerificationDetails()
        {
            EmployeeVerificationDetails = new List<GISEmployeeDDOVerificationDetail>();
            IEmployeeVerificationDetails = new List<GISEmployeeDDOVerificationDetail>();
            LastUpdatedStatusForEmployees = new List<GISEmployeeDDOVerificationDetail>();
            ApprovedEmployeeStatus = new List<GISEmployeeDDOVerificationDetail>();
        }
        public IList<GISEmployeeDDOVerificationDetail> EmployeeVerificationDetails { get; set; }

        public IList<GISEmployeeDDOVerificationDetail> IEmployeeVerificationDetails { get; set; }

        public IList<GISEmployeeDDOVerificationDetail> LastUpdatedStatusForEmployees { get; set; }

        public IList<GISEmployeeDDOVerificationDetail> ApprovedEmployeeStatus { get; set; }

        public long TotalReceived { get; set; }

      

        public long ForwardedApplications { get; set; }

        public long SentBackApplication { get; set; }

        public long PendingApplications { get; set; }

        public string Department { get; set; }

        public string Designation { get; set; }

        //public string employeename { get; set; }
        //public string groupDesc { get; set; }
        //public int SavingInsuranceAmt { get; set; }
        //public DateTime SubscriptionDate { get; set; }
        //public string deptName { get; set; }
        //public string amtinwords { get; set; }
    }

    public class GISEmployeeDDOVerificationDetail
    {
        public string Name { get; set; }

        public long? EmployeeCode { get; set; }

        public string ApplicationNumber { get; set; }

        public long ApplicationId { get; set; }

        public string Remarks { get; set; }

        public string Comments { get; set; }

        public string Status { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastUpdatedDateTime { get; set; }//Need to remove

        public string LastUpdatedDate { get; set; }

        public string Premium { get; set; }

        public string PolicyNumber { get; set; }

        public string SanctionedDate { get; set; }

        public int Priority { get; set; }

        public int RowNum { get; set; }

        public string District { get; set; }
        public string Department { get; set; }

        public string NBBondDocPath { get; set; }
        public string NBFSDocPath { get; set; }
        public string NBSignBondDocPath { get; set; }

        public string employeename { get; set; }
        public string groupDesc { get; set; }
        public int SavingInsuranceAmt { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public string deptName { get; set; }

        public string designation { get; set; }

        public DateTime currentdate { get; set; }
        public string amtinwords { get; set; }

        public long subcriptionNumber { get; set; }



    }
}

