using System;
using System.ComponentModel.DataAnnotations;

namespace KGID_Models.KGID_GroupInsurance
{
      
   public  class tbl_GIS_nominee_details
    {
        [Key]
      public long gnd_nominee_id       { get; set; }
      public long gnd_emp_id           { get; set; }
      public long gnd_application_id    { get; set; }
      public int gnd_relation_id       { get; set; }
      public string  gnd_name_of_nominee   { get; set; }
      public string gnd_name_of_guardian  { get; set; }
    //  public long gnd_guardian_relation_id { get; set; }
      public  int gnd_percentage_of_share{ get; set; }
     // public long  gnd_family_id         { get; set; }
      public int  gnd_active            { get; set; }
      public DateTime gnd_creation_datetime { get; set; }
      public long  gnd_created_by       { get; set; }
      public DateTime gnd_updation_datetime { get; set; }
      public long gnd_updated_by        { get; set; }
      public int  gnd_guardian_age      { get; set; }
        public int gnd_Nominee_age { get; set; }
        public string gnd_contingencies { get; set; }
        public string gnd_predeceasing { get; set; }
        public Nullable<DateTime> gnd_nomineeDob  { get; set; }
        public string gnd_AdoptionFile_path { get; set; }
        
    }
}
