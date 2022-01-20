using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGID_GroupInsurance
{
  public  class VM_GISNomineeDetails
    {
        public VM_GISNomineeDetails()
        {
            NomineeDetails = new List<VM_GISNomineeDetail>();
        }

        public long EmployeeId { get; set; }
        public long ApplicationId { get; set; }
        public IList<VM_GISNomineeDetail> NomineeDetails { get; set; }
        public IList<VM_GISNomineeDetail> ddlNomineeList { get; set; }
    }

    public class VM_GISNomineeDetail
    {
        public long gnd_nominee_id { get; set; }
        public long gnd_emp_id { get; set; }
        public long gnd_application_id { get; set; }
        public long gnd_relation_id { get; set; }
        public string gnd_name_of_nominee { get; set; }
        public string gnd_name_of_guardian { get; set; }
        public long gnd_guardian_relation_id { get; set; }
        public long gnd_percentage_of_share { get; set; }
        public long gnd_family_id { get; set; }
        public int gnd_active { get; set; }
        public DateTime gnd_creation_datetime { get; set; }
        public long gnd_created_by { get; set; }
        public DateTime gnd_updation_datetime { get; set; }
        public long gnd_updated_by { get; set; }
        public int gnd_guardian_age { get; set; }
        public int gnd_Nominee_age { get; set; }
        public string gnd_relation_Desc { get; set; }

    }
    public class VM_GISDropDownList
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class GISBindDropDownModel
    {
        public List<VM_GISDropDownList> GuardianList { get; set; }
        public List<VM_GISDropDownList> NomineeList { get; set; }

    }
}
