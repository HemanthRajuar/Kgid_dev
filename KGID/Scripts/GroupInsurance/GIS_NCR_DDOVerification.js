
var ColOneLoaded = false;
var ColTwoLoaded = false;

var ColFourLoaded = false;

var medSupportingDocs = [];
var today = '';
var minDate = '';
var dates = [];
var rowno = 0;
$(document).ready(function () {
    // Add minus icon for collapse element which is open by default
    $(".collapse.show").each(function () {
        $(this).prev(".card-header").find(".fa").addClass("fa-minus").removeClass("fa-plus");
    });

    // Toggle plus minus icon on show hide of collapse element
    $(".collapse").on('show.bs.collapse', function () {
        $(this).prev(".card-header").find(".fa").removeClass("fa-plus").addClass("fa-minus");
    }).on('hide.bs.collapse', function () {
        $(this).prev(".card-header").find(".fa").removeClass("fa-minus").addClass("fa-plus");
    });

    $("#spnEName").text($("#spnProposerName").text());
    $("#spnRNo").text($("#hdnBReferanceNo").val());
    $('#tblDDOWorkflow').DataTable({
        paging: false,
        info: false,
        searching: false,
        "ordering": false
    });

    $(".nav-item").click(function (e) {
        $("#viewSidebar")[0].style.width = "0";
        $("#applicationFormTab")[0].style.marginRight = "0";
    });

    $.ajax({
        type: 'POST',
        url: '/VerifyDetails/GetRemarkComments',
        data: JSON.stringify({ 'RemarkID': $("#ddlRemarks").val() }),
        async: false,
        contentType: 'application/json; charset=utf-8',
        processData: false,
        cache: false,
        success: function (data) {
         
            $("#txtComments").val(data);
            if ($("#ddlRemarks option:selected").text() == "No Correction Found") {
                $("#lblBackToEmp").hide();
                $("#lblfrwftoddo").show();
                $("#rbtnForward").prop("checked", true);
                $("#rbtnBackToEmployee").prop("checked", false);
                $("#errrbtn").attr("hidden", true);

            } else {
                $("#lblBackToEmp").show();
                $("#lblfrwftoddo").hide();
                $("#rbtnForward").prop("checked", false);
                $("#rbtnBackToEmployee").prop("checked", true);
                $("#errrbtn").attr("hidden", true);
            }
        }
    });
   
});

function ShowApplicationDetails(id) {
    $("#divBDetails").hide();
    $("#divFNDetails").hide();
    $("#divPMLDetails").hide();
    if (id == 1) {
        $("#divBDetails").show();
    } else if (id == 2) {
        $("#divFNDetails").show();
    } else if (id == 3) {
        $("#divPMLDetails").show();
    }
    $("#mdBasicDetails").modal("show");
}
$("#btnSubmit").click(function (e) {
    debugger;

    $(".err").hide();

    $(".chk-req").each(function () {
        if (!$(this).is(":checked")) {
            $(this).siblings(".err").show();
        }
    });

    var isStatusSelected = true;
    if ($("input[name='ApplicationStatus']:checked").length === 0) {
        $("input[name='ApplicationStatus']").parent().siblings(".err").show();
        isStatusSelected = false;
    }

    var isCheckboxchecked = true;
    if ($(".err:visible").length > 0) {
        isCheckboxchecked = false;
    }

    var isSendBack = true;
    if ($("#ddlRemarks").val() == null || $("#ddlRemarks").val() == null) {
        $("#errRemarksReq").show();
        isSendBack = false;
    }
    if ($('#chkFamNomD').prop('checked') && $('#chkAppD').prop('checked') )
    {
        if (isStatusSelected && isCheckboxchecked && isSendBack) {
            alertify.confirm(($("input[name='ApplicationStatus']:checked").val() === "2") ? "Are you sure you want to send back application to employee?" : "Are you sure you want to approve the application?", function () {
                $(".mederr").hide();
                // $("#frmDDOVerDetails").submit();
                //url: '/SaveDDOVData/',
                var objVerifyDetails = new FormData($("#frmDDOVerDetails").get(0));
                $.ajax({
                    type: 'POST',
                    /*url: '/GISSaveDDOVData/',*/
                    url: '/GIS_NCR_SaveDDOVData/',
                    data: objVerifyDetails,
                    async: false,
                    cache: false,
                    contentType: false,
                    processData: false,
                    //success: function (result) {
                    //    if (result == "1") {
                    //        window.location.href = "/GIS-NomineeChange-ddo/";
                    //        //if ($("input[name='ApplicationStatus']:checked").val() === "2") {
                    //        //    window.location.href = "/kgid-ddo/";
                    //        //}
                    //        //else {
                    //        //    alertify.alert("Forwarded to KGID department.", function () {
                    //        //        window.location.href = "/kgid-ddo/";
                    //        //    }).setHeader("Attention");
                    //        //}
                    //    }
                    //    //else if (result == "7") {
                    //    //    alertify.alert("There is no Caseworker mapped for this district.", function () {
                    //    //        window.location.href = "/kgid-ddo/";
                    //    //    }).setHeader("Warning");
                    //    //}

                    //}

                    success: function (result) {
                        if (result == "1") {
                            if ($("input[name='ApplicationStatus']:checked").val() === "2") {
                                /* window.location.href = "/kgid-ddo/";*/
                                /*  window.location.href = "/gis-view-app/"; GIS - ddo*/
                                window.location.href = "/GIS-NomineeChange-ddo/";
                            }
                            //else {
                            //    alertify.alert("Forwarded to KGID department.",
                            //        function () {
                            //        window.location.href = "/kgid-ddo/";
                            //    }).setHeader("Attention");
                            //}
                        }
                        window.location.href = "/GIS-NomineeChange-ddo/";
                        //else if (result == "7") {
                        //    alertify.alert("There is no Caseworker mapped for this district.", function () {
                        //        window.location.href = "/kgid-ddo/";
                        //    }).setHeader("Warning");
                        //}

                    }
                });
            }).setHeader("Confirm changes?");
        }
    }
    else
    {
        if ($('#chkFamNomD').prop('checked') == false) { $("#errchkFamNomD").removeAttr('hidden'); }
        if ($('#chkAppD').prop('checked') == false) { $("#errchkAppD").removeAttr('hidden'); }
    }
    e.preventDefault();
});

function printApplicationForm() {
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Basic Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintBasicDetails").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPersonal").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html();
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head><title>Application Form</title>');
    frameDoc.document.write('</head><body>');
    //Append the external CSS file.
    //frameDoc.document.write('<link href="/Content/Custom/sb-admin-2.min.css" rel="stylesheet" />');
    frameDoc.document.write('<link href="/Scripts/DataTables/Bootstrap-4-4.1.1/css/bootstrap.min.css" rel="stylesheet" />');
    //Append the DIV contents.
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 500);

}
function printPaymentDetails() {
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Payment Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPaymentDetails").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Payment Status</h2></div><div class="form-group col-4"></div></div>' + $("#divPaymentStatus").html() + "<hr />";
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head><title>Payment Details</title>');
    frameDoc.document.write('</head><body>');
    //Append the external CSS file.
    //frameDoc.document.write('<link href="/Content/Custom/sb-admin-2.min.css" rel="stylesheet" />');
    frameDoc.document.write('<link href="/Scripts/DataTables/Bootstrap-4-4.1.1/css/bootstrap.min.css" rel="stylesheet" />');
    //Append the DIV contents.
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 500);

}
function printMedicalReport() {

    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Basic Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHBasicDetails").html() + "<hr />";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Physical Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHPhysicalDetails").html() + "<hr />";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Other Details</h3></div><div class="form-group col-4"></div></div>' + $("#divOtherDetails").html() + "<hr />";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Health Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHealthDetails").html() + "<hr />";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Doctor Details</h3></div><div class="form-group col-4"></div></div>' + $("#divDoctorDetails").html(); + "<hr />";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Doctor Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHDeclaration").html();
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head><title>Medical Report</title>');
    frameDoc.document.write('</head><body>');
    //Append the external CSS file.
    frameDoc.document.write('<link href="/Scripts/DataTables/Bootstrap-4-4.1.1/css/bootstrap.min.css" rel="stylesheet" />');
    //Append the DIV contents.
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 500);
}
function ChangeVerificationStatus(destid, srcid) {
    var verificationstatus = $("#" + srcid).prop("checked");
    $("#" + destid).prop("checked", verificationstatus);
}
$("#btnClear").click(function (e) {
    $("#ddlRemarks").val("");
    $("#txtComments").val("");
    $("#rbtnBackToEmployee").prop("checked", false);
    $("#rbtnForward").prop("checked", false);
    $(".verify").prop("checked", false);
    e.preventDefault();
});
function showtab(btnid, divid) {
    $("#applicationForm").removeClass("fade").addClass("active show");
    $("#nav-tab-Form").attr("aria-selected", "true");
    $("#scrutiny").removeClass("active show").addClass("fade");
    $("#nav-tab-Scrutiny").attr("aria-selected", "false");
    $("#nav-tab-Form").addClass("active show");
    $("#nav-tab-Scrutiny").removeClass("active show");
    $(".collapse").removeClass("show");
    $(".card-header > h2 > button").attr("aria-expanded", "false");
    $(".card-header > h2 > button > i").removeClass("fa-minus").addClass("fa-plus");
    $("#" + btnid > button).attr("aria-expanded", "true").removeClass("collapsed");
    $("#divid").addClass("show");
}
function closeNav() {
    $("#viewSidebar")[0].style.width = "0";
    $("#applicationFormTab")[0].style.marginRight = "0";
}

$("#btncollapseOne,#a_colone").click(function () {
    if (!ColOneLoaded) {
        $.ajax({
            url: '/ViewDataToVerify/KGIDDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divKD").html(data);
                EnglishToKannada();
            }
        });
        ColOneLoaded = true;
    }
})
$("#btncollapseTwo,#a_coltwo").click(function () {
    if (!ColTwoLoaded) {
        $.ajax({

            url: '/GroupInsurance/GISNomineeDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divND").html(data);
                EnglishToKannada();
            }
        })
        ColTwoLoaded = true;
    }
})
$("#btncollapseFour,#a_colfour").click(function () {

    var empid = $('#hdnAddEmpCode').val();
    var appid = $('#hdnAppCode').val();
    if (!ColFourLoaded) {
        $.ajax({
            /* url: '/ViewDataToVerify/PaymentDetailsToView', PaymentDetailsToView*/
            url: '/GroupInsurance/GISPaymentDetailsToView',
            data: JSON.stringify({ EmpId: empid, AppId: appid }),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divPAD").html(data);

                EnglishToKannada();
            }
        });
        ColFourLoaded = true;
    }
})

$("#ddlRemarks").change(function () {
    $.ajax({
        type: 'POST',
        url: '/VerifyDetails/GetRemarkComments',
        data: JSON.stringify({ 'RemarkID': this.selectedOptions[0].value }),
        async: false,
        contentType: 'application/json; charset=utf-8',
        processData: false,
        cache: false,
        success: function (data) {
            $("#txtComments").val(data);
            if ($("#ddlRemarks option:selected").text() == "No Correction Found") {
                $("#lblBackToEmp").hide();
                $("#lblfrwftoddo").show();              
                $("#rbtnForward").prop("checked", true);
                $("#rbtnBackToEmployee").prop("checked", false);
                $("#errrbtn").attr("hidden", true);
              
            } else {
                $("#lblBackToEmp").show();                
                $("#lblfrwftoddo").hide();
                $("#rbtnForward").prop("checked", false);
                $("#rbtnBackToEmployee").prop("checked", true);
                $("#errrbtn").attr("hidden", true);
            }
        }
    });
});

$("#btnWFNext").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").addClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").addClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").removeClass("active");
})
$("#btnAFNext").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").addClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").addClass("active");
    $("#nav-tab-Documents").removeClass("active");
})
$("#btnSPrevious").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").addClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").addClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").removeClass("active");
})
$("#btnAFPrevious").click(function (e) {
    $("#workflow").addClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").addClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").removeClass("active");
})
$("#btnSNext").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").addClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").addClass("active");

    //LoadUploadedDocuments();
})

$("#btnUPrevious").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").addClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").addClass("active");
    $("#nav-tab-Documents").removeClass("active");
})

$("#nav-tab-Documents").click(function () {
    //LoadUploadedDocuments();
})

function LoadUploadedDocuments() {
    $.ajax({
        url: '/VerifyDetails/LoadUploadedDocuments',
        async: false,
        type: 'POST',
        contentType: false,
        processData: false,
        success: function (result) {
            var htmlData = '';
            for (var i = 0; i < result.length; i++) {
                htmlData = '<div class="form-group col-6"><label class="control-label text-justify col-6">' + result[i].UploaddocType + '</label><a class="viewuploadeddoc col-6" href="javascript:;" data-path=' + result[i].UploaddocPath + '>Click Here</a></div>'
            }
            $("#divLoadUploadedDocuments").html(htmlData);
        }
    });
}



function FileListItems(files) {
    var b = new ClipboardEvent("").clipboardData || new DataTransfer()
    for (var i = 0, len = files.length; i < len; i++) b.items.add(files[i])
    return b.files
}




function EnglishToKannada() {
    var _knLan = $('.knlan');
    var _EnLan = $('.Enlan');
    if (localStorage.ChangeLang == '0') {
        _EnLan.hide();
        _knLan.show();
        $('#changeLan').val('A');
    }
    else {
        _knLan.hide();
        _EnLan.show();
        $('#changeLan').val('ಕ');
    }
}

function DocFileChange(id, errLbl) {
    debugger;
    if ($("#" + id).get(0).files[0] != undefined) {
        var fileType = $("#" + id).get(0).files[0].type;
        if (fileType == 'application/pdf') {
            if (id == "flEditMedSupportingDoc") {
                $("#hdnMedSupportingDoc").val($("#" + id).get(0).files[0].name);
            } else {
                $("#hdnReimbursedDocument").val($("#" + id).get(0).files[0].name);
            }
            $("#" + errLbl).attr("hidden", true);
        }
        else {
            $("#" + errLbl).removeAttr("hidden");
            $("#" + errLbl).text("Please upload document in pdf format");
            $("#" + id).val("");
            return false;
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

$('#rbtnBackToEmployee').change(function () {
    $("#errrbtn").attr("hidden", true);;
});

$('#rbtnForward').change(function () {
    $("#errrbtn").attr("hidden", true);;
});

$('#chkAppD').change(function () {
    $("#errchkAppD").attr("hidden", true);;
});

$('#chkFamD').change(function () {
    $("#errchkFamNomD").attr("hidden", true);;
});