using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGID_GroupInsurance
{
    public class VM_GIS_Upload_Documents
    {
        public int IsDocType { get; set; }
        public long App_Proposer_ID { get; set; }
        public long GIS_App_Reference_ID { get; set; }
        public string GISPAgetype { get; set; }

        #region Death
        public HttpPostedFileBase Form3 { get; set; }
        public string Form3_filename { get; set; }
        public HttpPostedFileBase OtherDoc { get; set; }
        public string OtherDoc_filename { get; set; }
        
        #endregion

        #region Regular
        public HttpPostedFileBase ProposalDocDonatedVehicle { get; set; }
        public string ProposalDocDonatedVehicle_filename { get; set; }
        public HttpPostedFileBase DonationDoc { get; set; }
        public string DonationDoc_filename { get; set; }
        public HttpPostedFileBase SaleCertificateDoc { get; set; }
        public string SaleCertificateDoc_filename { get; set; }
        public HttpPostedFileBase TaxInvoiceDoc { get; set; }
        public string TaxInvoiceDoc_filename { get; set; }
        public HttpPostedFileBase DonatedEmissionCertificate { get; set; }
        public string DonatedEmissionCertificate_filename { get; set; }
        #endregion
        


    }
}
