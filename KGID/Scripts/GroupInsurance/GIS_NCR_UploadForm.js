
$(document).ready(function () {
    var EmpId = $('#hdnUFEmpId1').val();
    var AppId = $('#hdnUFAppId1').val();
 
    $("#btnSubmit").on('click', function () {
        //var EmpId = $('#hdnUFEmpId').val();
        //var AppId = $('#hdnUFAppId').val();
        var IsApplicationRequired = ($("#txtApplicationFormDoc").val() != undefined) ? true : false;

        //if (IsApplicationRequired) {
        //    if ($("#txtApplicationFormDoc").val().length == 0) {
        //        alertify.alert("Please upload application form").setHeader("warning!!!");
        //        return false;
        //    }
        //}


       // var IsApplicationRequired = ($("#txtAFDoc").val() != undefined) ? true : false;
        var IsForm6Required = ($("#txtForm6Doc").val() != undefined) ? true : false;
        var IsForm7Required = ($("#txtForm7Doc").val() != undefined) ? true : false;


        //if (IsApplicationRequired) {

        //    if ($("#txtAFDoc").val().length == 0 && ($("#UplodedAF").val() == null)) {
        //        alertify.alert("Please upload application form").setHeader("warning!!!");
        //        return false;
        //    }
        //}

        if (IsForm6Required) {
            if ($("#txtForm6Doc").val().length == 0 && ($("#UplodedF6").val() == null)) {
                alertify.alert("Please upload  form 6").setHeader("warning!!!");
                return false;
            }
        }

        if (IsForm7Required) {
            if ($("#txtForm7Doc").val().length == 0 && ($("#UplodedF7").val() == null)) {
                alertify.alert("Please upload  form 7").setHeader("warning!!!");
                return false;
            }
        }

        alertify.confirm("Are you sure you want to upload documents ?", function () {
            //alert($("#hdnempidGIS").val());
            //alert($("#hdnApplnGIS").val());
            //alert($("#hdnUFEmpId").val());
            //var objUF = new FormData($("#frmUploadDocs").get(0));
            var objUF = new FormData($("#frmUploadDocs").get(0));            
            objUF.append("App_Employee_Code", EmpId);
            objUF.append("App_ApplicationID", AppId);
            debugger;

            $.ajax({
                type: 'POST',
                // url: '/gis-upload-form-data/',
                url: '/gis-noiminee-change-upload-form/',
                data: objUF,
                async: false,
                cache: false,
                contentType: false,
                processData: false,

                //  type: 'POST',
                //   url: '/gis-upload-form-data/',
                ///*  url: '/GroupInsurance/GISUploadEmployeeData',*/
                // /*  type: 'POST',*/
                ///*  data: JSON.stringify({ objUF }),   */  
                //  data: objUF,
                //  async: false,
                //  contentType: 'application/json; charset=utf-8',
                //data: objUF,
                //async: false,
                //cache: false,
                //contentType: false,
                //processData: false,
                success: function (result) {
                    debugger;                   
                    if (result.Result == 0) {

                        alertify.alert("error in uploading files", function () {
                            //window.location.href = result.RedirectUrl;
                            $("#txtApplicationFormDoc").val("");
                            $("#txtApplicationFormDoc").text("");
                            $("#txtForm6Doc").val("");
                            $("#txtForm6Doc").text("");
                            $("#txtForm7Doc").val("");
                            $("#txtForm7Doc").text("");
                         
                        }).setHeader("Attention");
                    }
                    else {
                        GisNCRSaveDeclaration();
                       // window.location.href = result.RedirectUrl;
                    }
                }
            });
        }).setHeader("Confirm changes?");
    });

});
function ApplicationFormUpload() {
    var status = false;
    alertify.confirm('Application Upload !!!', 'Are you sure want to upload documents?', function () { alertify.success('Uploaded'); status = true; }
        , function () { alertify.error('Canceled'); status = false; }); return status;
}
function DocFileChange(id, errLbl) {
    if ($("#" + id).get(0).files[0] != undefined) {
        var fileType = $("#" + id).get(0).files[0].type;
        if (fileType == 'application/pdf') {
            $("#" + errLbl).attr("hidden", true);
        }
        else {
            $("#" + errLbl).removeAttr("hidden");
            $("#" + errLbl).text("Please upload document in pdf format");
            $("#" + id).val("");
        }
        const fsize = $("#" + id).get(0).files[0].size;
        const maxAllowedSize = 5 * 1024 * 1024;
        // The size of the file.
        if (fsize > maxAllowedSize) {
            $("#" + errLbl).removeAttr("hidden");
            $("#" + errLbl).text("File too Big, please select a file less than 5 MB");
            $("#" + id).val("");
        }
    }
}