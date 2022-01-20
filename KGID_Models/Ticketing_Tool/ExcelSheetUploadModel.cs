using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KGID_Models.Ticketing_Tool
{
    public partial class ExcelSheetUploadModel
    {
        public ExcelSheetBind _ExcelSheetBind { get; set; }
    }

    public partial class ExcelSheetBind
    {
        public HttpPostedFileBase _excelFile { get; set; }
        public string _excelFilePath { get; set; }
        public SelectList _masterTable { get; set; }
        public int _masterTableId { get; set; }
        public int _uploadType { get; set; }
        public string status_message { get; set; }
        public string excelType { get; set; }
    }
}
