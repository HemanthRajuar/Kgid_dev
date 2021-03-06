var ColOneLoaded = false;
var ColTwoLoaded = false;
var ColThreeLoaded = false;
var ColFourLoaded = false;
var ColFiveLoaded = false;
$(document).ready(function () {
    $("#tblSIApprovedData").DataTable({
        paging: false,
        info: false,
        searching: false,
        "ordering": false
    });
    $("#spnEName").text($("#spnProposerName").text());
    $("#spnRNo").text($("#hdnBReferanceNo").val());

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
        }
    })
    $("#divMedLeaveDetails").hide();
    $("#divMedLeave").show();
    if ($.fn.dataTable.isDataTable('#tblMedicalLeaveDetails')) {
        table = $('#tblMedicalLeaveDetails').DataTable();
    }
    else {
        table = $('#tblMedicalLeaveDetails').DataTable({
            paging: false,
            info: false,
            searching: false,
            "columnDefs": [
                {
                    "orderable": false,
                    "targets": [0, 1, 2, 3, 4, 5]
                }
            ]
        });
    }
});

$("#btnSubmit").click(function (e) {
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
    if ($("#ddlRemarks").val() == "" || $("#ddlRemarks").val() == null) {
        $("#errRemarksReq").show();
        isSendBack = false;
    }

    if (isStatusSelected && isCheckboxchecked && isSendBack) {
        alertify.confirm("Are you sure you want to forward application to DIO?", function () {
           // $("#frmSIVerDetails").submit();
            var caseWorkerVerifiedDetails = new FormData($("#frmSIVerDetails").get(0));
            $.ajax({
                type: 'POST',
                url: '/SaveSIVData/',
                data: caseWorkerVerifiedDetails,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                success: function (result) {
                    if (result == "1") {
                        alertify.alert("Forwarded to DIO.", function () {
                            window.location.href = "/kgid-si/";
                        }).setHeader("Attention");
                    } 
                    else if (result == "7") {
                        alertify.alert("There is no DIO mapped for this district.", function () {
                            window.location.href = "/kgid-si/";
                        }).setHeader("Warning");
                    }
                }
            });
        }).setHeader("Confirm changes?");
    }

    e.preventDefault();
});


function printApplicationForm() {
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Basic Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintBasicDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPersonal").html() + "<hr/>";
    //contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html();
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
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Payment Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPaymentDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Payment Status</h2></div><div class="form-group col-4"></div></div>' + $("#divPaymentStatus").html() + "<hr/>";
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

    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Basic Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHBasicDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Physical Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHPhysicalDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Other Details</h3></div><div class="form-group col-4"></div></div>' + $("#divOtherDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Health Details</h3></div><div class="form-group col-4"></div></div>' + $("#divHealthDetails").html() + "<hr/>";
    contents = contents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Doctor Details</h3></div><div class="form-group col-4"></div></div>' + $("#divDoctorDetails").html(); + "<hr/>";
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
    $("#rdBtnSendBackToCW").prop("checked", false);
    $("#rdBtnForwardToDIO").prop("checked", false);
    $(".verify").prop("checked", false);
    e.preventDefault();
})
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
        //$.ajax({
        //    url: '/ViewDataToVerify/BasicDetailsToView',
        //    data: JSON.stringify({}),
        //    type: 'POST',
        //    async: false,
        //    contentType: 'application/json; charset=utf-8',
        //    success: function (data) {
        //        $("#divBD").html(data);
        //    }
        //});
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
            url: '/ViewDataToVerify/FamilyDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divFD").html(data);
                EnglishToKannada();
            }
        });
        $.ajax({
            url: '/ViewDataToVerify/NomineeDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divND").html(data);
                EnglishToKannada();
            }
        });
        ColTwoLoaded = true;
    }
})
$("#btncollapseThree,#a_colthree").click(function () {
    if (!ColThreeLoaded) {
        $.ajax({
            url: '/ViewDataToVerify/PersonalDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divPD").html(data);
                EnglishToKannada();
            }
        });
        ColThreeLoaded = true;
    }
})
$("#btncollapseFour,#a_colfour").click(function () {
    
    var empid = $('#hdnAddEmpCode').val();
    var appid = $('#hdnAppCode').val();
    if (!ColFourLoaded) {
        $.ajax({
            url: '/ViewDataToVerify/PaymentDetailsToView',
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
$("#btncollapseFive,#a_colfive").click(function () {
    if (!ColFiveLoaded) {
        $.ajax({
            url: '/ViewDataToVerify/HPhysicalDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divHPD").html(data);
                EnglishToKannada();
            }
        });
        $.ajax({
            url: '/ViewDataToVerify/HOtherDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divHOD").html(data);
                EnglishToKannada();
            }
        });
        $.ajax({
            url: '/ViewDataToVerify/HHealthDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divHHD").html(data);
                EnglishToKannada();
            }
        });
        $.ajax({
            url: '/ViewDataToVerify/HDoctorDetailsToView',
            data: JSON.stringify({}),
            type: 'POST',
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#divHDD").html(data);
                EnglishToKannada();
                if (!$("#rbtnKMCDr").prop("checked"))
                    $("#divDctrBnkDetails").show();
                else
                    $("#divDctrBnkDetails").hide();
            }
        });
        ColFiveLoaded = true;
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
});
$("#btnAFNext").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").addClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").addClass("active");
    $("#nav-tab-Documents").removeClass("active");
});
$("#btnSPrevious").click(function (e) {
    $("#workflow").removeClass("show active");
    $("#applicationForm").addClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").removeClass("active");
    $("#nav-tab-Form").addClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").removeClass("active");
});
$("#btnAFPrevious").click(function (e) {
    $("#workflow").addClass("show active");
    $("#applicationForm").removeClass("show active");
    $("#scrutiny").removeClass("show active");
    $("#uploadeddocuments").removeClass("show active");

    $("#nav-tab-Workflow").addClass("active");
    $("#nav-tab-Form").removeClass("active");
    $("#nav-tab-Scrutiny").removeClass("active");
    $("#nav-tab-Documents").removeClass("active");
});
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