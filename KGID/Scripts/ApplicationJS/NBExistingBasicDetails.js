$(document).ready(function () {
    debugger;
    $('#txtEAddress').keypress(function (e) {
        var key = e.keyCode;
        if (key == 13)
            e.preventDefault();
    });
    var orphons = $("#hdnOrphan").val();
    if ($("#txtEmployeeFullName").val() != "" && $("#txtEmployeeFullNameKN").val() == "") {
        $("#txtEmployeeFullNameKN").attr("disabled", false);
        $("#txtEmployeeFullNameKN").removeAttr("readonly");
    }
    if ($("#txtFatherName").val() != "" && $("#txtFatherNameKN").val() == "") {
        $("#txtFatherNameKN").attr("disabled", false);
        $("#txtFatherNameKN").removeAttr("readonly");
    }
    if ($("#txtFatherName").val() == "" && $("#txtFatherNameKN").val() == "" && orphons == "false") {
        if (orphons == "false") {
            $("#divOrphanOptions").hide();
            $("#rbtnOrphan").prop("checked", false)
            $("#rbtnNotOrphan").prop("checked", true)
            $(".rd-btn-orphan:checked").prop("checked", false);
        }
        //else {
        //    $("#divOrphanOptions").show();
        //    $("#rbtnOrphan").prop("checked", false)
        //    $("#rbtnNotOrphan").prop("checked", true)
        //    $(".rd-btn-orphan:checked").prop("checked", false);
        //}

        $("#txtFatherNameKN").attr("disabled", false);
        $("#txtFatherNameKN").removeAttr("readonly");
    }
    if ($(".rd-btn-mstatus:checked").val() === "True") {
      /*  alert("JKBKJBJBVJH");*/
        $("#divMarriedOptions").show();
        
        $("#rbtnUnmarriedd").show();
        $("#divOrphanOptions").hide();
        $("#txtSpouseName").removeAttr("readonly");
        $("#txtSpouseNameKN").removeAttr("readonly");
       
        $("#rbtnMarried").prop("checked", true);
      /*  $("#rbtnUnmarried").attr("disabled", true);*/
       

        //$(".rd-btn-orphan:checked").prop("checked", false);
        //$(".rd-btn-orphan:checked").prop("checked", false);
        $("#divOrphanOptions").hide();
/*        $("#rbtnUnmarriedd").hide();*/
        $("#rbtnOrphan").prop("checked", false)
        $("#rbtnNotOrphan").prop("checked", true)
        $(".rd-btn-orphan").attr("disabled", false);
       /* $(".rd-btn-mstatus").attr("disabled", false);*/
        $("#ddlDRType").removeAttr("disabled");
    }
    else {
      /*  alert("jjvhjvvbv");*/
        $("#rbtnUnmarriedd").show();
        $("#rbtnUnmarried").prop("checked", true);
        $("#txtSpouseName").attr("readonly", "readonly").val("");
        $("#txtSpouseNameKN").attr("readonly", "readonly").val("");
        $(".rd-btn-govt:checked").prop("checked", false);
        $("#txtSpouseKGIDNumber").val("");
        $("#divSpouseKGID").hide();
        $("#divIDNumber").hide();
        $("#divMarriedOptions").hide();
        $("#divOrphanOptions").hide();
        $("#rbtnOrphan").prop("checked", false);
    
        $("#rbtnNotOrphan").prop("checked", true);
        $(".rd-btn-orphan").attr("disabled", false);
        $(".rd-btn-mstatus").attr("disabled", false);
        $("#ddlDRType").attr("disabled", true);
    }

    var isMarried = $("#rbtnMarried").is(":checked");
    if (!isMarried && $("#txtFatherName").val() != "") {
       /* alert("rbtnUnmarriedd");*/
        $("#rbtnUnmarriedd").show();
        $("#rbtnUnmarried").prop("checked", true);
        $("#txtSpouseName").attr("readonly", "readonly").val("");
        $("#txtSpouseNameKN").attr("readonly", "readonly").val("");
        $("#hdnOrphan").val("false");
      /*  $("#rbtnUnmarriedd").hide();*/
        $("#divOrphanOptions").hide();
        $("#rbtnOrphan").prop("checked", false);
        $("#rbtnNotOrphan").prop("checked", true)
      
    }
    else if (!isMarried && $("#txtFatherName").val() == "") {

        $("#hdnOrphan").val("true");
        $("#rbtnOrphan").prop("checked", false)
        $("#rbtnOrphan").prop("checked", true);
        //$("#rbtnNotOrphan").prop("checked", false);
        //$(".rd-btn-orphan").attr("disabled", false);
    }
    if ($(".rd-btn-govt:checked").val() === "True") {
        $("#divIDNumber").show();
        $("#divOrphanOptions").hide();
        $("#rbtnUnmarriedd").hide();
        $("#rbtnOrphan").prop("checked", false);
        $("#rbtnNotOrphan").prop("checked", true)
        $("#rbtnUnmarriedd").hide();
        $("#divSpouseKGID").show();
    }
    else {
        $("#divSpouseKGID").hide();
        $("#divIDNumber").hide();
    }
    if ($(".rd-btn-kgidpan:checked").val() === "True") {
        $("#divOrphanOptions").hide();
        $("#rbtnUnmarriedd").hide();
        $("#rbtnOrphan").prop("checked", false);
        $("#rbtnNotOrphan").prop("checked", true)
        $("#divPANNumber").show();
        $("#divKGIDNumber").hide();
    }
    else {
        $("#divOrphanOptions").hide();
        $("#rbtnOrphan").prop("checked", false);
        $("#rbtnNotOrphan").prop("checked", true)
        $("#divKGIDNumber").show();
        $("#divPANNumber").hide();
    }
    if ($("#ddlDRType").val() == 2) {
        $("#divOrphanOptions").hide();
        $("#rbtnUnmarriedd").hide();
        
        $("#rbtnOrphan").prop("checked", false);
        $("#rbtnNotOrphan").prop("checked", true)
        $("#divCSpouse").show();
        $("#divdrSuppDoc").hide();
        $("input[name='eod_spouse_govt_emp']").removeAttr("disabled");
    } else if ($("#ddlDRType").val() == 1) {
        $("#divCSpouse").hide();
        $("#divdrSuppDoc").show();
        $("input[name='eod_spouse_govt_emp']").attr("disabled", true);
    } else {
        $("#divCSpouse").hide();
        $("#divdrSuppDoc").hide();
        $("input[name='eod_spouse_govt_emp']").removeAttr("disabled");
    }
    $('#txtEPin').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
        $("#txtEPin").attr('maxlength', '6');
    });

    $('#txtSpouseKGIDNumber').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
        $("#txtSpouseKGIDNumber").attr('maxlength', '15');
    });
    //if ($("#hdnfirstpolno").val() == null || $("#hdnfirstpolno").val() == "") {
    //    $("#ddlDRType option:contains('N/A')").prop("selected", true);
    //    $("#ddlDRType option:contains('N/A')").attr("disabled", true);
    //}
});
$('input[type=radio][name=eod_emp_orphan]').change(function () {
    if (this.value == "True") {
        $("#hdnOrphan").val(true);
    }
    else {
        $("#hdnOrphan").val(false);
    }
});
$(".rd-btn-mstatus").change(function () {
    if ($(this).val() === "True") {
     /*   alert("fdfGDRGDF");*/
        $("#divMarriedOptions").show();
        $("#rbtnUnmarriedd").show();
        //$(".rd-btn-orphan:checked").prop("checked", false);
        $("#divOrphanOptions").hide();
        $("#rbtnOrphan").prop("checked", false)
        $("#rbtnNotOrphan").prop("checked", true);
        //$(".rd-btn-orphan:checked").prop("checked", false);
        $(".rd-btn-govt").removeAttr("disabled");
        $("#txtSpouseName").removeAttr("readonly");
        $("#txtSpouseNameKN").removeAttr("readonly");
        if ($("#txtSpouseName").val() != "" || $("#txtSpouseNameKN").val() != "") {
            $("#ddlDRType").removeAttr("disabled");
        }
        $.ajax({
            url: '/Employee/GetNomineeList',
            data: JSON.stringify({ AppID: $("#hdnAppID").val(), IsMarried: "Married" }),
            async: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result == "0") {
                    $(".rd-btn-mstatus").prop("checked", false);
                    alertify.alert("Please remove member from nominee list before editing.").setHeader("Warning!!!");
                    return false;
                }
            }, error: function (result) {
                alertify.error("Could not edit family member");
            }
        });
    }
    else {
   /*     alert("tertertretjjvghfghertrhjvvbv");*/
        $("#rbtnUnmarriedd").show();
        $("#rbtnUnmarried").prop("checked", true);
         
       /* $("#rbtnUnmarriedd").hide();*/
        $("#divMarriedOptions").hide();
        $("#txtSpouseName").attr("readonly", "readonly").val("");
        $("#txtSpouseNameKN").attr("readonly", "readonly").val("");
        //$("#txtSpouseName").removeAttr("readonly");
        //$("#txtSpouseNameKN").removeAttr("readonly");
        $("#divOrphanOptions").hide();
        $("#rbtnOrphan").prop("checked", false);
        $("#rbtnNotOrphan").prop("checked", true);
       /* $(".rd-btn-orphan").attr("disabled", false);*/

        //$("#divOrphanOptions").show();
        //$(".rd-btn-orphan:checked").prop("checked", false);

        //$("#rbtnOrphan").prop("checked", false);
        //$("#rbtnNotOrphan").prop("checked", true);
        //$(".rd-btn-orphan").attr("disabled", false);
        $(".rd-btn-govt:checked").prop("checked", false);
        $("#txtSpouseKGIDNumber").val("");
        $("#divSpouseKGID").hide();
        $("#divMarriedOptions").hide();

        $("#divIDNumber").hide();
        //$("#txtSpouseName").removeAttr("readonly") ;
        //$("#txtSpouseNameKN").removeAttr("readonly") ;
        $("#txtSpouseName").attr("readonly", "readonly").val("");
        $("#txtSpouseNameKN").attr("readonly", "readonly").val("");
        $("#ddlDRType").attr("disabled", true);
        $("#ddlDRType").val("");
        $("#divCSpouse").hide();
        $("#divSuppDoc").hide();
    }

    var isMarried = $("#rbtnMarried").is(":checked");
    if (!isMarried && $("#txtFatherName").val() != "") {
        $("#hdnOrphan").val("false");
        $("#rbtnNotOrphan").prop("checked", true);
    /*    $(".rd-btn-orphan").attr("disabled", true);*/
    }
    else if (!isMarried && $("#txtFatherName").val() == "") {
        $("#hdnOrphan").val("true");
        //$("#rbtnOrphan").prop("checked", true);
        //$(".rd-btn-orphan").attr("disabled", false);

        $("#divOrphanOptions").show();
        $("#rbtnOrphan").prop("checked", true);
        $("#rbtnNotOrphan").prop("checked", false);
        $(".rd-btn-orphan").attr("disabled", false);
    }
});

$("#txtSpouseName,#txtSpouseNameKN").keyup(function () {
    if ($("#txtSpouseName").val() != "") {
        $("#ddlDRType").removeAttr("disabled");
    }
    if ($("#txtSpouseNameKN").val() != "") {
        $("#ddlDRType").removeAttr("disabled");
    }
})

$(".rd-btn-govt").change(function () {
    if ($(this).val() === "True") {
        $("#rbtnUnmarriedd").hide();
        $("#divOrphanOptions").hide();
        $("#divSpouseKGID").show();
        $("#divIDNumber").show();
        
        $('#txtSpousePanNumber').val("");
      /*  $('#txtSpouseKGIDNumber').val("");*/
        $('#errSpousePANNumberReq').attr("hidden", true);
    }
    else {
        $("#divSpouseKGID").hide();
        $("#divIDNumber").hide();
    }
});

$(".rd-btn-kgidpan").change(function () {
    if ($(this).val() === "True") {
        $("#rbtnUnmarriedd").hide();
        $("#rbtnUnmarriedd").hide();
        $("#divOrphanOptions").hide();
        var KGIDNO = $("#txtSpouseKGIDNumber").val();
        $("#divKGIDNumber").hide();
        $("#divPANNumber").show();
    }
    else {
        $("#divPANNumber").hide();
        $("#divKGIDNumber").show();
    }
});
$(".rd-btn-orphan").change(function () {
    var orphon = $("#hdnOrphan").val();
    var fathname = $("#txtFatherName").val();
    var fathernameiKN = $("#txtFatherNameKN").val();
    if (orphon == "true" && fathname != "" && fathernameiKN != "") {
        alertify.alert("Please remove the father name")

    }

});

//function ValidateBasicDetails() {
//    $('.err').attr('hidden', true);
//    var isMarried = $("#rbtnMarried").is(":checked");
//    var IsMarriedRadioSelected = $("input[name='eod_emp_married']:checked").length;
//    var IsGovtEmpRadioSelected = $("input[name='eod_spouse_govt_emp']:checked").length;
//    var IsOrphanSelected = $("input[name='eod_emp_orphan']:checked").length;
//    var IsKGIDPANNum = $("input[name='isKGIDPAN']:checked").length;

//    if (IsMarriedRadioSelected === 0) {
//        $("#errMarritalStatusReq").removeAttr("hidden");
//    }

//    if (isMarried && IsGovtEmpRadioSelected === 0) {
//        $("#errGovtEmpStatusReq").removeAttr("hidden");
//    }

//    if (isMarried && ($("#txtSpouseName").val() == "" && $("#txtSpouseNameKN").val() == "")) {
//        $("#errSpouseName").removeAttr("hidden");
//    }

//    if ($("#rbtnGovtEmp").is(":checked") && $("#txtSpouseKGIDNumber").val() === "") {
//        $("#errSpouseKGIDNumberReq").removeAttr("hidden");
//    }

//    if (!isMarried && IsOrphanSelected === 0) {
//        $("#errOrphanStatusReq").removeAttr("hidden");
//    }
//    if ($("#rbtnGovtEmp").is(":checked")) {
//        if ($("#rbtnPAN").is(":checked")) {
//            if ($("#txtSpousePanNumber").val() == "") {
//                $("#errSpousePANNumberReq").removeAttr("hidden");
//            }
//            var panVal = $('#txtSpousePanNumber').val();
//            var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
//            if (regpan.test(panVal)) {
//                $("#errSpousePANNumberReq").attr("hidden");
//            } else {
//                $("#errSpousePANNumberReq").text("Please enter valid PAN Number.");
//                $("#errSpousePANNumberReq").removeAttr("hidden");
//            }
//        } else {
//            if ($("#txtSpouseKGIDNumber").val() == "") {
//                $("#errSpouseKGIDNumberReq").removeAttr("hidden");
//            }
//        }
//    }
//    if ($("#ddlDRType").val() == 2) {
//        if ($("#txtCS").val() == "") {
//            $("#errCurrMarritalStatusReq").removeAttr("hidden");
//        }
//    }

//    if ($("#ddlDRType").val() == 1) {
//        if ($("#flDivSupportingDoc").get(0).files[0] == "" || $("#flDivSupportingDoc").get(0).files[0] == undefined) {
//            if ($("#divdrDocUpload:visible").length === 0) {
//                $("#errCurrMarritalStatusReq").text("Please upload supporting document");
//                $("#errCurrMarritalStatusReq").removeAttr("hidden");
//            }
//        }
//    }

//    var msg = "";

//    if ($('#txtEmployeeFullNameKN').val() == "") {

//        msg += "Proposer Name Kannada, ";
//    }

//    if ($('#txtFatherName').val() != "") {
//        if ($('#txtFatherNameKN').val() == "") {
//            msg += "Father Name (Kannada), ";
//        }
//    }

//    if ($('#txtSpouseName').val() != "") {
//        if ($('#txtSpouseNameKN').val() == "") {
//            msg += "Spouse Name (Kannada), ";
//        }
//    }

//    if (msg != "") {

//        var lastChar = msg.slice(-1);
//        if (lastChar == ',') {
//            msg = msg.slice(0, -1);
//        }
//        alertify.alert("Please contact DDO to update the following feilds " + msg)
//        return false;
//    }
//    else {
//        return true;
//    }

//}



function ValidateBasicDetails() {
    debugger;
    $('.err').attr('hidden', true);
    var errCount = 0;
    var isMarried = $("#rbtnMarried").is(":checked");
    var IsMarriedRadioSelected = $("input[name='eod_emp_married']:checked").length;
    var IsGovtEmpRadioSelected = $("input[name='eod_spouse_govt_emp']:checked").length;
    var IsOrphanSelected = $("input[name='eod_emp_orphan']:checked").length;
    var IsKGIDPANNum = $("input[name='isKGIDPAN']:checked").length;
    var mob = $("#txtMobileNumber").val();
    var kgidno = $("#txtSpouseKGIDNumber").val();
    var proposername = $("#txtEmployeeFullNameKN").val();
    //var fathname = $("#txtFatherName").val();
    //var fathernameiKN = $("#txtFatherNameKN").val();
    var PlaceOfbirth =$("#txtPlaceofbirth").val().trim();
    var Email = $("#txtEmailId").val();
    var pan = $("#txtPanNumber").val();
    var adress = $("#txtEAddress").val();
    var pincode = $("#txtEPin").val();

    if (mob == "") { $("#errMblNum").removeAttr('hidden'); }
    if (proposername == "") { $("#errEmpNamekn").removeAttr('hidden'); }
    if (PlaceOfbirth == "") { $("#errPOB").removeAttr('hidden'); }
    if (Email == "") { $("#errMailID").removeAttr('hidden'); }
    if (pan == "") { $("#errPANNum").removeAttr('hidden'); }
    //if (fathname == "") { $("#errFatherName").removeAttr('hidden'); }
    //if (fathernameiKN == "") { $("#errFatherNameKN").removeAttr('hidden'); }
    if (adress == "") { $("#errAddress").removeAttr('hidden'); }
    if (pincode == "") { $("#errtxtEPin").removeAttr('hidden'); }
    if (IsMarriedRadioSelected === 0) {
        $("#errMarritalStatusReq").removeAttr("hidden");
        errCount++;
    }
    if ($("#rbtnOrphan").is(":checked")) {
        debugger;

        if ($("#txtFatherName").val() == "" && $("#txtFatherNameKN").val() == "") {
            $("#errFatherName").attr("hidden", true);
            $("#errFatherNameKN").attr("hidden", true);

        }
        //else
        //    $("#errFatherName").removeAttr("hidden");
    }
    if ($("#rbtnNotOrphan").is(":checked")) {
        if ($("#txtFatherName").val() != "") {
            if ($("#txtFatherNameKN").val() == "") {
                /*   $("#errFatherName").removeAttr("hidden");*/
                $("#errFatherNameKN").removeAttr("hidden");

                errCount++;
                
                var cursorsfathernamek = 1;
            }
        }
        else {
            var cursorfathername = 1;
            $("#errFatherName").attr("hidden", true);
            $("#errFatherNameKN").attr("hidden", true);}
        
    }

    if ($("#txtSpouseName").val() !="") {
      
        if ($("#txtSpouseNameKN").val() == "") {
             
            $("#errSpouseNameKan").removeAttr("hidden");
            $("#errSpouseNameKan").text("Enter names in Kannada");
            errCount++;
            var cursorsspousenamek = 1;
        }
        else {
          
            $("#errSpouseNameKan").attr("hidden", true);
         /*   $("#errSpouseNameKan").removeAttr("hidden");*/
        }
    }
    if ($("#txtEmployeeFullNameKN").val() == "") {
        $("#errEmpNamekn").removeAttr("hidden");
        errCount++;
        var cursorsempnamek = 1;
    }
    if ($("#txtEmployeeFullName").val() == "") {
        $("#errEmployeeName").removeAttr("hidden");
        errCount++;
        var cursorempname = 1;
    }
    //if ($("#txtFatherName").val() == "") {
    //    $("#errFatherName").removeAttr("hidden");
    //    errCount++;
    //}
    if ($("#txtBasicDateOfBirth").val() == "") {
        $("#errDOB").removeAttr("hidden");
        errCount++;
        var cursordob = 1;
    }
    if ($("#txtPlaceofbirth").val() == "") {
        $("#errPOB").removeAttr("hidden");
        errCount++;
        var cursorpob = 1;
    }
    if ($("#txtMobileNumber").val() == "") {
        $("#errMNumber").removeAttr("hidden");
        errCount++;
        var cursormobileno = 1;
    }
    if ($("#txtEmailId").val() == "") {
        $("#errMailID").removeAttr("hidden");
        errCount++;
        var cursoremail = 1;
    }
    else if ($("#txtEmailId").val() != "" || $("#txtEmailId").val() != null) {
        var input_eval = $("#txtEmailId").val();
        var inputeRGEX = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        var inputeResult = inputeRGEX.test(input_eval);
        if (!(inputeResult)) {
            $('#errEMail').removeAttr("hidden");
            $('#errEMail').show();
            errCount++;
            var cursoremail = 1;
            //return false;
        } else {
            $('#errEMail').hide();
        }
        //return true;
    }
    if ($("#txtPanNumber").val() == "") {
        $("#errPANNum").removeAttr("hidden");
        errCount++;
        var cursorpanno = 1;
    }
    else if ($("#txtPanNumber").val() != "" || $("#txtPanNumber").val() != null) {
        debugger;
        var input_pval = $("#txtPanNumber").val();
        var inputpRGEX = /[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
        var inputpResult = inputpRGEX.test(input_pval);
        if (!(inputpResult)) {
            $('#errPNum').removeAttr("hidden");
            $('#errPNum').show();
            errCount++;
            var cursorpanno = 1;
            // return false;
        } else {
            $('#errPNum').hide();
        }
        //return true;
    }
    //if ($("#txtPanNumber").val() == "") {
    //    $("#errpanno").removeAttr("hidden");
    //    errCount++;
    //} 
    if ($("#txtEAddress").val() == "") {
        $("#errAddress").removeAttr("hidden");
        errCount++;
        var cursoraddress = 1;
    }

    if ($("#txtEPin").val() == 0) {
        $("#errtxtEPin").removeAttr("hidden");
        errCount++;
        var cursorpinno = 1;
    }
    if (isMarried && IsGovtEmpRadioSelected === 0) {
        $("#errGovtEmpStatusReq").removeAttr("hidden");
        errCount++;
    }

    if (isMarried && ($("#txtSpouseName").val() == "" && $("#txtSpouseNameKN").val() == "")) {
        $("#errSpouseName").removeAttr("hidden");
        errCount++;
        var cursorspousename = 1;
    }

    if ($("#rbtnGovtEmp").is(":checked") && $("#txtSpouseKGIDNumber").val() === "") {
        $("#errSpouseKGIDNumberReq").removeAttr("hidden");
        errCount++;
        var cursorspousekgidno = 1;
    }

    if (!isMarried && IsOrphanSelected === 0) {
        $("#errOrphanStatusReq").removeAttr("hidden");
        errCount++;
    }
    if ($("#rbtnGovtEmp").is(":checked")) {
        if ($("#rbtnPAN").is(":checked")) {
            if ($("#txtSpousePanNumber").val() == "") {
                $("#errSpousePANNumberReq").removeAttr("hidden");
                errCount++;
                var cursorspousepanno = 1;
            }
            var panVal = $('#txtSpousePanNumber').val();
            var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
            if (regpan.test(panVal)) {
                $("#errSpousePANNumberReq").attr("hidden");
            } else {
                $("#errSpousePANNumberReq").text("Please enter valid PAN Number.");
                $("#errSpousePANNumberReq").removeAttr("hidden");
                errCount++;
                var cursorspousepanno = 1;
            }
        } else {
            if ($("#txtSpouseKGIDNumber").val() == "") {
                $("#errSpouseKGIDNumberReq").removeAttr("hidden");
                errCount++;
                var cursorspousekgidno = 1;
            }
            else if (kgidno.length < 5) {
                $("#errSpouseKGIDNumberReq").text("Please Enter valid KGID Number");
                $("#errSpouseKGIDNumberReq").removeAttr("hidden");
                errCount++;
                var cursorspousekgidno = 1;
            }
        }
    }
    if ($("#ddlDRType").val() == 2) {
        $("#errCurrMarritalStatusReq").text("");
        if ($("#txtCS").val() == "") {
            $("#errCurSpouseName").removeAttr("hidden");
            errCount++;
        }
        if ($("#txtCSKN").val() == "") {
            $("#errCurSpouseName").removeAttr("hidden");
            errCount++;
            var cursorspusenamekan = 1;
        }
        if ($("#txtCS").val() != "") {
            if ($("#txtCSKN").val() == "") {
                $("#errCurSpouseNameKan").removeAttr("hidden");
                errCount++;
                var cursorspusenamekan = 1;
            }
        }
    }

    if ($("#ddlDRType").val() == 1) {
        if ($("#flDivSupportingDoc").get(0).files[0] == "" || $("#flDivSupportingDoc").get(0).files[0] == undefined) {
            if ($("#divdrDocUpload:visible").length === 0) {
                $("#errCurrMarritalStatusReq").text("Please upload supporting document");
                $("#errCurrMarritalStatusReq").removeAttr("hidden");
                errCount++;
            }
        }
    }

    var msg = "";
    if ($('#txtEmployeeFullName').val() != "") {
        if ($('#txtEmployeeFullNameKN').val() != "") {
            //msg += "Proposer Name Kannada, ";
            var panVal = $('#txtEmployeeFullNameKN').val();
            //var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
            //if (regpan.test(panVal)) {
            //    $("#errSpousePANNumberReq").attr("hidden");

            //}
            var isKannada = /^(([\u0C80-\u0CFF])|([ ])|([\u0C80-\u0CFF]))*$/g;
            if (!isKannada.test(panVal)) {
                $("#errEmpNamekn").removeAttr("hidden");
                $("#errEmpNamekn").text("Enter names in Kannada");
                errCount++;
                var cursorsempnamek = 1;
            }
        }
        else
            $("#errEmpNamekn").attr("hidden");
        //$("#errEmployeeNameKan").removeAttr("hidden");       
    }


    if ($('#txtFatherName').val() != "") {
        if ($('#txtFatherNameKN').val() != "") {
            var panVal = $('#txtFatherNameKN').val();
            // msg += "Father Name (Kannada), "; 
            var isKannada = /^(([\u0C80-\u0CFF])|([ ])|([\u0C80-\u0CFF]))*$/g;
            if (!isKannada.test(panVal)) {
                $("#errFatherNameKN").removeAttr("hidden");
                $("#errFatherNameKN").text("Enter names in Kannada");
                errCount++;
                var cursorsfathernamek = 1;
            }
        }
        else {
            $("#errFatherNameKN").removeAttr("hidden");
          //  $("#errFatherNameKan").removeAttr("hidden");
        }
    }
    var SN = 0;
    if ($('#txtSpouseName').val() != "") {
        if ($('#txtSpouseNameKN').val() != "") {
            var panVal = $('#txtSpouseNameKN').val();
            var isKannada = /^(([\u0C80-\u0CFF])|([ ])|([\u0C80-\u0CFF]))*$/g;
            if (!isKannada.test(panVal)) {
                $("#errSpouseNameKan").removeAttr("hidden");
                $("#errSpouseNameKan").text("Enter names in Kannada");
                errCount++;
                var cursorsspousenamek = 1;
            }
        }
        else {
            $("#errSpouseNameKan").removeAttr("hidden");
   
        }
        //SN = 1;
        //msg += "Spouse Name (Kannada), "; 
    }
    if ($('#txtCS').val() != "") {
        if ($('#txtCSKN').val() != "") {
            var panVal = $('#txtCSKN').val();
            var isKannada = /^(([\u0C80-\u0CFF])|([ ])|([\u0C80-\u0CFF]))*$/g;
            if (!isKannada.test(panVal)) {
                $("#errCurrMarritalStatusReq").removeAttr("hidden");
                $("#errCurrMarritalStatusReq").text("Enter names in Kannada");
                errCount++;
                var cursorscurspousernamek = 1;
            }
        }
        else
            $("#errCurrMarritalStatusReq").attr("hidden");
        //$("#errCurrMarritalStatusReq").removeAttr("hidden");
        //errCount++;        
    }

    if (cursorempname == 1) {
        var value = $("#txtEmployeeFullName").val();
        $("#txtEmployeeFullName").focus().val('').val(value);
    }
    else if (cursorsempnamek == 1) {
        var value = $('#txtEmployeeFullNameKN').val();
        $("#txtEmployeeFullNameKN").focus().val('').val(value);
    }
    else if (cursorfathername == 1) {
        var value = $("#txtFatherName").val();
        $("#txtFatherName").focus().val('').val(value);
    }
    else if (cursorsfathernamek == 1) {
        var value = $('#txtFatherNameKN').val();
        $("#txtFatherNameKN").focus().val('').val(value);
    }
    else if (cursordob == 1) {
        var value = $("#txtEAddress").val();
        $("#txtEAddress").focus().val('').val(value);
    }
    else if (cursorpob == 1) {
        var value = $("#txtPlaceofbirth").val();
        $("#txtPlaceofbirth").focus().val('').val(value);
    }
    else if (cursormobileno == 1) {
        var value = $("#txtMobileNumber").val();
        $("#txtMobileNumber").focus().val('').val(value);
    }
    else if (cursoremail == 1) {
        var value = $("#txtEmailId").val();
        $("#txtEmailId").focus().val('').val(value);
    }
    else if (cursorpanno == 1) {
        var value = $("#txtPanNumber").val();
        $("#txtPanNumber").focus().val('').val(value);
    }
    else if (cursoraddress == 1) {
        var value = $("#txtEAddress").val();
        $("#txtEAddress").focus().val('').val(value);
    }
    else if (cursorpinno == 1) {
        var value = $("#txtEPin").val();
        $("#txtEPin").focus().val('').val(value);
    }
    else if (cursorsspousenamek == 1) {
        var value = $('#txtSpouseNameKN').val();
        $("#txtSpouseNameKN").focus().val('').val(value);S
    }
    else if (cursorspusenamekan == 1) {
        var value = $("#txtCSKN").val();
        $("#txtCSKN").focus().val('').val(value);
    }
    else if (cursorspousekgidno == 1) {
        var value = $("#txtSpouseKGIDNumber").val();
        $("#txtSpouseKGIDNumber").focus().val('').val(value);
    }
    else if (cursorspousepanno == 1) {
        var value = $('#txtSpousePanNumber').val();
        $("#txtSpousePanNumber").focus().val('').val(value);
    }
    else if (cursorspousename == 1) {
        var value = $("#txtSpouseName").val();
        $("#txtSpouseName").focus().val('').val(value);
    }
    else if (cursorscurspousernamek == 1) {
        var value = $('#txtCSKN').val();
        $("#txtCSKN").focus().val('').val(value);
    }
    else if (cursorpinno == 1) {
        var value = $("#txtEPin").val();
        $("#txtEPin").focus().val('').val(value);
    }
    else if (cursoraddress == 1) {
        var value = $("#txtEAddress").val();
        $("#txtEAddress").focus().val('').val(value);
    }
    else if (cursorpanno == 1) {
        var value = $("#txtPanNumber").val();
        $("#txtPanNumber").focus().val('').val(value);
    }
    else {
        return true;
    }

}

function ValidatePancardNumber(panVal) {
    var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
    if (regpan.test(panVal)) {
        $("#errSpousePANNumberReq").attr("hidden");
    } else {
        $("#errSpousePANNumberReq").removeAttr("hidden");
    }
}

function MShandleClick(sts) {
    if (sts == 1) {
        $("#hdnMStatus").val(true);
    } else {
        $("#hdnMStatus").val(false);
    }
}
function ChangeCurrentMarriedSts() {
    if ($("#hdnmarriedstatus").val() == 1 || $("#hdnmarriedstatus").val() == 2) {
        if ($("#ddlDRType").val() == 0 || $("#ddlDRType").val() == "") {
            alertify.error("You cannot select this option");
            $("#ddlDRType").val($("#hdnmarriedstatus").val());
            return false;
        }
    }
    $("#hdnDRStatus").val($("#ddlDRType").val());
    if ($("#ddlDRType").val() == 2) {
        $.ajax({
            url: '/Employee/GetNomineeList',
            data: JSON.stringify({ AppID: $("#hdnAppID").val(), IsMarried: "Remarried" }),
            async: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                debugger;
                if (result == "0") {
                    $("#ddlDRType").val("");
                    alertify.alert("Please remove member from nominee list before editing.").setHeader("Warning!!!");
                    $("#txtCS").val("");
                    $("#divCSpouse").hide();
                    $("#divdrSuppDoc").hide();
                    $("#divdrDocUpload").hide();
                    return false;
                }
                else {
                    $("#txtCS").val("");
                    $("#divCSpouse").show();
                    $("#divdrSuppDoc").hide();
                    $("#divdrDocUpload").hide();
                    $("input[name='eod_spouse_govt_emp']").removeAttr("disabled");
                }
            }, error: function (result) {
                alertify.error("Could not edit family member");
            }
        });
    }
    else if ($("#ddlDRType").val() == 1) {
        $.ajax({
            url: '/Employee/GetNomineeList',
            data: JSON.stringify({ AppID: $("#hdnAppID").val(), IsMarried: "Divorced" }),
            async: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                debugger;
                if (result == "0") {
                    $("#ddlDRType").val("");
                    alertify.alert("Please remove member from nominee list before editing.").setHeader("Warning!!!");
                    $("#txtCS").val("");
                    $("#divCSpouse").hide();
                    $("#divdrSuppDoc").hide();
                    $("#divdrDocUpload").hide();
                    return false;
                }
                else {
                    $("#divCSpouse").hide();
                    $("#divdrSuppDoc").show();
                    //$("#divdrDocUpload").show();
                    $("input[name='eod_spouse_govt_emp']").attr("disabled", true);
                }
            }, error: function (result) {
                alertify.error("Could not edit family member");
            }
        });
    }
    else {
        $("#txtCS").val("");
        $("#divCSpouse").hide();
        $("#divdrSuppDoc").hide();
        $("#divdrDocUpload").hide();
        $("input[name='eod_spouse_govt_emp']").removeAttr("disabled");
    }
}

function DocFileChange(id, errLbl) {
    if ($("#" + id).get(0).files[0] != undefined) {
        var fileType = $("#" + id).get(0).files[0].type;
        if (fileType == 'application/pdf') {
            $("#" + errLbl).attr("hidden", true);
        }
        else {
            alert('Wrong type!! Upload Pdf file only..')
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

$('.ClAlphaNum').keyup(function () {
    var input_val = $(this).val();
    var inputRGEX = /^[a-zA-Z0-9]*$/;
    var inputResult = inputRGEX.test(input_val);
    if (!(inputResult)) {
        this.value = this.value.replace(/[^a-z0-9\s]/gi, '');
    }
});
$('.ClAlpha').keyup(function () {
    var input_val = $(this).val();
    var inputRGEX = /^[a-zA-Z]*$/;
    var inputResult = inputRGEX.test(input_val);
    if (!(inputResult)) {
        this.value = this.value.replace(/[^a-zA-Z\s]/gi, '');
    }
});
$('.ClNum').keyup(function () {
    var input_val = $(this).val();
    var inputRGEX = /^[0-9]*$/;
    var inputResult = inputRGEX.test(input_val);
    if (!(inputResult)) {
        this.value = this.value.replace(/[^0-9\s]/gi, '');
    }

});
$('.tocaps').keyup(function (e) {


    var input = $(this);
    var start = input[0].selectionStart;
    $(this).val(function (_, val) {
        return val.toUpperCase();
    });
    input[0].selectionStart = input[0].selectionEnd = start;

});

$(".preventSpace").keypress(function (e) {
    if (e.which === 32)
        return false;
});
