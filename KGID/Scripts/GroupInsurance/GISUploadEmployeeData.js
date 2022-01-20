﻿
$(document).ready(function () {
    $("#btnSubmit").on('click', function () {
        var EmpId = $('#hdnUFEmpId').val();
        var AppId = $('#hdnUFAppId').val();
      /*  alert(EmpId + "        " + AppId)*/
    //    var IsApplicationRequired = ($("#txtApplicationFormDoc").val() != undefined) ? true : false;

    //    if ($("#txtApplicationFormDoc").val().length == 0 && $('#applnformLink').is(':hidden')) {
           
    //            alertify.alert("Please upload application form").setHeader("warning!!!");
    //            return false;
           
    //}
      

       
    //    if ($("#txtForm6Doc").val().length == 0 && $('#Form6Link').is(':hidden')) {
    //            alertify.alert("Please upload  form6").setHeader("warning!!!");
    //            return false;
    //    }

    //    if ($("#txtForm7Doc").val().length == 0 && $('#Form7Link').is(':hidden')) {
    //        alertify.alert("Please upload  form7").setHeader("warning!!!");
    //        return false;
    //    }

       /* var IsApplicationRequired = ($("#txtApplicationFormDoc").val() != undefined) ? true : false;*/
        var IsApplicationRequired = ($("#txtAFDoc").val() != undefined) ? true : false;
        var IsForm6Required = ($("#txtForm6Doc").val() != undefined) ? true : false;
        var IsForm7Required = ($("#txtForm7Doc").val() != undefined) ? true : false;


        if (IsApplicationRequired) {
           
            if ($("#txtAFDoc").val().length == 0 && ($("#UplodedAF").val() == null )) {
                alertify.alert("Please upload application form").setHeader("warning!!!");
                return false;
            }
        }

        if (IsForm6Required) {
            if ($("#txtForm6Doc").val().length == 0 && ($("#UplodedF6").val() == null) ) {
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
           
            var objUF = new FormData($("#frmUploadDocs").get(0));
            objUF.append("Id", $("#hdnUFEmpId").val());
            objUF.append("EmpId", $("#hdnUFAppId").val());
            debugger;

            var viewModel = {
                /* 'EmpID': 0,*/
                'gcd_challan_ref_no': 0,//$("#txaymentrefno").val(),
                'ddo_code_id': 0,
                'ddo_code': 0,
                'receipttypeid': 0,
                'gcd_purpose_code': 0,
                'gcd_subpurpose_code': 0,
                'gcd_purpose_id': 0,
                'gcd_sub_purpose_id': 0,
                'gcd_head_of_acunt': 0,
                'gcd_amount': 0,
                'gcd_datetime_of_challan': new Date(),
                'gcd_status': '1',
                'gcd_creation_datetime': new Date(),
                'gcd_applicati_id': 0,
                'gcd_insurance_aunt': 0,
                'gcd_insurance_amount': 0,
            };
           
            $.ajax({
                type: 'POST',               
                url: '/gis-upload-form-data/',
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

                       // alertify.alert("There is no DDO available for this DDO code!", function () {
                        alertify.alert("Error in uploading File", function () {
                            //window.location.href = result.RedirectUrl;
                            $("#txtAFDoc").val("");
                            $("#txtAFDoc").text("");
                            $("#txtForm6Doc").val("");
                            $("#txtForm6Doc").text("");
                            $("#txtForm7Doc").val("");
                            $("#txtForm7Doc").text("");
                        }).setHeader("Attention");
                    }
                    else if (result.Result == 1 || result.Result == 2)
                    {
                      
                        $.ajax({
                            // url: '/Employee/InsertChallanDetai',
                            url: '/GroupInsurance/GISSaveDeclaration',
                            data: JSON.stringify({ EmpId: EmpId, AppId: AppId }),
                            type: 'POST',
                            async: false,
                            contentType: 'application/json; charset=utf-8',
                            success: function (result) {
                              
                                if (result == 3) { alertify.error("There is no DDO available for this DDO code!"); }
                                else if (result == 2) {
                                    setTimeout(function () { alertify.success("Successfully submitted to DDO"); }, 5000);
                                    /* alertify.success("Successfully submitted to DDO");*/
                                    window.location.href = "/gis-view-app/";
                                }
                                //          window.location.href = "/gis-view-app/";
                                //alertify.success("Successfully submitted to DDO");
                            }, error: function (sult) {
                                alertify.error("Error in saving workflow details");


                            }
                        });
                       

                        //
                    }
                    //else {
                    //    window.location.href = result.RedirectUrl;
                    //}
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


function saveDeclaration()// save workflow
{
   
}
