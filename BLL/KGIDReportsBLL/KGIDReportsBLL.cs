using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.KGIDReportsDLL;
using KGID_Models.KGID_Report;

namespace BLL.KGIDReportsBLL
{
    public class KGIDReportsBLL : IKGIDReportsBLL
    {
        private readonly IKGIDReportsDLL reportsDLL;

        public KGIDReportsBLL()
        {
            reportsDLL = new KGIDReportsDLL();
        }

        public VM_KGIDApplicationReports GetKGIDApplicationsReport(long loggedInUserId, VM_KGIDApplicationReportDetails applicationReportDetails, int empType)
        {
            return reportsDLL.GetKGIDApplicationsReport(loggedInUserId, applicationReportDetails, empType);
        }

        public VM_KGIDGSTReports GetKGIDGSTReport(long loggedInUserId, VM_KGIDGSTReportDetails applicationReportDetails, int empType)
        {
            return reportsDLL.GetKGIDGSTReport(loggedInUserId, applicationReportDetails, empType);
        }
    }
}
