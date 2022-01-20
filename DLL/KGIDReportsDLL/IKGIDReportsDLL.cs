using KGID_Models.KGID_Report;

namespace DLL.KGIDReportsDLL
{
    public interface IKGIDReportsDLL
    {
        VM_KGIDApplicationReports GetKGIDApplicationsReport(long loggedInUserId, VM_KGIDApplicationReportDetails applicationReportDetails, int empType);

        VM_KGIDGSTReports GetKGIDGSTReport(long loggedInUserId, VM_KGIDGSTReportDetails applicationReportDetails, int empType);
    }
}