var BserverResponse = false;
var MrtStatus;
var isFamilyDetailsAdded = false;
var isNomineeDetailsAdded = false;

$(document).ready(function () {
  
    const urlParams = new URLSearchParams(window.location.search);
    const myParam = urlParams.get('pay');   
    $(".number").hide();
    $("#imgQRCode").attr('src', $("#hdnQRCode").val());
    alertify.set('notifier', 'delay', 2);



});
$(document).ready(function () {
   
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    if (mm == 12) {
        mm = '01';
        yyyy = yyyy + 1;
    } else {
        mm = parseInt(mm) + 1;
        mm = String(mm).padStart(2, '0');
    }
    var timerdate = dd + '/' + mm + '/' + yyyy;
    //// For demo preview end
    //$("#ApplicationTab ul[aria-label=Pagination] li:nth-child(1)").after('<li><a id="btnASave" role="menuitem" onclick="ApplicationDetailsSave()">Save</a></li>');
    ////$("#PaymentTab ul[aria-label=Pagination] li:nth-child(1)").after('<li><a id="btnPSave" role="menuitem" onclick="PaymentDetailsSave()">Save</a></li>');
    //$("#MedicalTab ul[aria-label=Pagination] li:nth-child(1)").after('<li><a id="btnMSave" role="menuitem" onclick="MedicalDetailsSave()">Save</a></li>');

    //$("#txtdateofpayment").val(timerdate);
    //$("#txtdateofpayment1").val(timerdate);
    //if ($("#txtServiceType").val().toLowerCase() === "temporary") {
    //    $("a[href*='next']").parent("li").addClass("disabled");
    //    $("a[href*='next']").parent("li").attr("aria-disabled", true);
    //    $("a[href*='next']").removeAttr("href");
    //    $("a[href*='previous']").removeAttr("href");
    //}

    //var eid = $("#divApplication").data("eid");

    //$("#spnName").text($("#txtProposerName").val());

    //ShowNomineeType();
});

$(document).ready(function () {
 
    debugger;
    if ($("#hdnSentBackAppliaction").val() != 0) {

        if ($("#hdnRmrks").val() == 4) {

        }
        if ($("#hdnRmrks").val() == 3) {

            $('#nav-Application').removeClass("show active");
            $('#nav-Medical').addClass("show active");

            $("#nav-Application").removeClass("active");
            $("#nav-Medical").addClass("active");

            sessionStorage.setItem('IsMedicalRequired', medicalRequiredStatus);
            var value = sessionStorage.getItem("IsMedicalRequired");

            $('#hdfIsMedicalRequired').val(value);

        }
        if ($("#hdnRmrks").val() == 2) {

            $('#nav-Application').addClass("show active");
            $('#nav-Medical').removeClass("show active");

            $("#nav-Application").addClass("active");
            $("#nav-Medical").removeClass("active");
        }
    }
    else {
        if ($("#hdnPaymentStatus").val() == 1) {
            //$('#nav-Application-tab').addClass("disabled").removeClass("active");
            //$('#nav-Payment-tab').removeClass("disabled").addClass("active");
            //$('#nav-Medical-tab').addClass("disabled").removeClass("active");
            //$('#nav-Application,#nav-Medical').removeClass("show active");
            //$('#nav-Payment').addClass("show active");
            //$("#nav-Payment-tab").click();
            $('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "none");
        }
        else if ($("#hdnPaymentStatus").val() == 0 || $("#hdnPaymentStatus").val() == 2) {
            debugger;
            $('#nav-Application-tab').addClass("disabled").removeClass("active");
            $('#nav-Payment-tab').removeClass("disabled").addClass("active");
            $('#nav-Medical-tab').addClass("disabled").removeClass("active");
            $('#nav-Application,#nav-Medical').removeClass("show active");
            $('#nav-Payment').addClass("show active");
            $("#nav-Payment-tab").click();
            $('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "none");
        }
        else if ($("#hdnPaymentStatus").val() == 3) {
            $('#nav-Application-tab').addClass("disabled").removeClass("active");
            $('#nav-Payment-tab').removeClass("disabled").addClass("active");
            $('#nav-Medical-tab').addClass("disabled").removeClass("active");
            $('#nav-Application,#nav-Medical').removeClass("show active");
            $('#nav-Payment').addClass("show active");
            $("#nav-Payment-tab").click();
            $('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
        }
    }
});
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

function printform() {
    //GetNomineeDetails();
    var Applicationcontents = "";

    Applicationcontents = $('#divPrintForm').html();
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>KGID Details</h2></div><div class="form-group col-4"></div></div>' + $("#divKGID").html() ;
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html();
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + '<div class="html2pdf__page-break"></div>';
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintPD").html();
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html() + '<input type="checkbox" value="true" checked /><label>I agree to the terms and condition mentioned above.</label>' + "<hr />";

    var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();

    //html2pdf(Applicationcontents, {
    //    margin: 0.3,
    //    filename: "ApplicationForm" + "_" + date,
    //    image: { type: 'jpeg', quality: 1 },
    //    html2canvas: { scale: 0, letterRendering: true },
    //    jsPDF: { unit: 'in', format: 'letter', orientation: 'Portrait' },
    //    pagebreak: { mode: 'legacy' }
    //});
    let mywindow = window.open('', 'PRINT', 'height=650,width=900,top=100,left=150');

    mywindow.document.write('<html><head><title>ApplicationForm' + date + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(Applicationcontents);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();

    return true;
    $("#divHeightChart").show();
    $("#divShowQRCode").hide();
    $(".action").show();
}


// devika 



$("#btnNomineeNext").click(function (e) {
  
    var result;

    var rowCount = $("#tblNomineeDetails1 tbody tr[data-row-number]").length
    if (rowCount > 0) {
        debugger;
      
        if (validateNoneForNext() == true) { result = true; }
        else { result = validatesumofshare(); }


        if (result == true) {
              GisGetNomineeDetails();
              $("#nav-Nominee").removeClass("show active");      
              $("#nav-Declaration").addClass("show active");
              $("#nav-Upload").removeClass("show active");
            

              $("#nav-Nominee-tab").removeClass("active");      
              $("#nav-Declaration-tab").addClass("active");
              $("#nav-Upload-tab").removeClass("active");
            
              $("#nav-Declaration-tab").removeClass("disabled");
              $("#nav-Declaration").removeClass("disabled");
        }
        else {
            alertify.alert('Percentage share for nominees should be 100 %.').setHeader("Warning!!!");
        }
    }
    else { alertify.alert('Please enter nominee details.').setHeader("Warning!!!"); }
});



$("#btnempNextDeclaration").click(function (e) {

    debugger;

    //
    if ($("#chkDeclaration").prop("checked") == false) {
        alertify.alert("Please agree to the terms and condition.").setHeader("Warning !!!");
        return false;
    }
    else {
       // GisNCRSaveDeclaration()
       
      
      


        $("#nav-Nominee").removeClass("show active");
        $("#nav-Declaration").removeClass("show active");
        $("#nav-Upload").addClass("show active");


        $("#nav-Nominee-tab").removeClass("active");
        $("#nav-Declaration-tab").removeClass("active");
        $("#nav-Upload-tab").addClass("active");



        GisGetBasicDetails();
        GisGetNomineeDetails();       
        GisGetDeclaration();
    

         printform6(); 

       printform7(); 

        //setTimeout(function () { printform1(); }, 5000);
        
        //setTimeout(function () { printform6(); }, 5000);

        //setTimeout(function () { printform7(); }, 5000);

        //var EmpId = $('#hdnempidGIS').val();
    

        
       
        
    }
  


});

$("#btnempPreDeclaration").click(function (e) {

    debugger;


    $("#nav-Nominee").addClass("show active");
    $("#nav-Declaration").removeClass("show active");
    $("#nav-Upload").removeClass("show active");




    $("#nav-Nominee-tab").addClass("active");
    $("#nav-Declaration-tab").removeClass("active");
    $("#nav-Upload-tab").removeClass("active");

    $("#nav-Nominee-tab").removeClass("disabled");
    $("#nav-Nominee").removeClass("disabled");

    GisGetNomineeDetails();
    GisGetDeclaration();

    //$("#nav-Upload-tab").removeClass("disabled");
    //$("#nav-Upload").removeClass("disabled");
});
$("#nav-Nominee-tab").click(function (e) {
  
        $("#nav-Nominee").addClass("show active");
        $("#nav-Declaration").removeClass("show active");
        $("#nav-Upload").removeClass("show active");


        $("#nav-Nominee-tab").addClass("active");
        $("#nav-Declaration-tab").removeClass("active");
        $("#nav-Upload-tab").removeClass("active");


        //$("#nav-Upload-tab").removeClass("disabled");
        //$("#nav-Upload").removeClass("disabled");

});

function GisGetPaymentDetails() {
    $.ajax({
        /* url: '/Employee/PaymentDetailsToView',*/
        url: '/GroupInsurance/GISInitialPayment',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#PaymentTab").html(data);
            EnglishToKannada();
        }
    });
}
function GisGetNomineeDetails() {

    //  $("#nomneeform")[0].reset();
    //$("#tblNomineeDetails").children().remove();
    $.ajax({
        url: '/GISNomineeChangeReq/GISNomineeDetails',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#NomineeTab").html(data);

            //$("#divNRNominee").append(data);
            //$("#tblNomineeDetails tbody").html(data);
            //$("#tblNomineeDetails").append(data);
            //$("#tblNomineeDetails > tbody").append(data);
            EnglishToKannada();
        }
    });

}
function GisGetBasicDetails() {
    $.ajax({
        //url: '/Employee/BasicDetailsToView',
      //  url: '/GroupInsurance/GISEmployeeBasicDetails',
        url: '/GISNomineeChangeReq/GIS_NCR_EmployeeBasicDetailsToView',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#ApplicationTab").html(data);
            EnglishToKannada();

        }
    });
}
function GisGetDeclaration() {
    $.ajax({
        //url: '/Employee/BasicDetailsToView',
      //  url: '/GroupInsurance/GISDeclaration',
        url: '/GISNomineeChangeReq/GIS_NCR_Declaration',
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#DeclarationTab").html(data);
            EnglishToKannada();

        }
    });
}


function printform1() {

    var Applicationcontents = "";

    Applicationcontents = $('#divPrintForm').html();
    
    var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();

    html2pdf(Applicationcontents, {
        margin: 0.3,
        filename: "Application Form" + "_" + date,
        image: { type: 'jpeg', quality: 1 },
        html2canvas: { scale: 0, letterRendering: true },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'Portrait' },
        pagebreak: { mode: 'legacy' }
    });
    let mywindow = window.open('', 'PRINT', 'height=650,width=900,top=100,left=150');

    mywindow.document.write('<html><head><title>ApplicationForm' + date + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(Applicationcontents);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    //mywindow.print();
    //mywindow.close();

    //  return true;
    //$("#divHeightChart").show();
    //$("#divShowQRCode").hide();
    //$(".action").show();
    //alert("   printform6();")
    //printform6();
}

function printform6() {

    var Applicationcontents1 = "";
    Applicationcontents1 = $('#divPrintForm6').html();

    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>KGID Details</h2></div><div class="form-group col-4"></div></div>' + $("#divKGID").html() ;
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html();
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + '<div class="html2pdf__page-break"></div>';
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintPD").html();
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html() + '<input type="checkbox" value="true" checked /><label>I agree to the terms and condition mentioned above.</label>' + "<hr />";

    var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();

    html2pdf(Applicationcontents1, {
        margin: 0.3,
        filename: $('#hdnApplnGISNCRnumber').val()+"Form 6" + "_" + date,
        image: { type: 'jpeg', quality: 1 },
        html2canvas: { scale: 0, letterRendering: true },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'Portrait' },
        pagebreak: { mode: 'legacy' }
    });
    let mywindow = window.open('', 'PRINT', 'height=650,width=900,top=100,left=150');

    mywindow.document.write('<html><head><title>Form7' + date + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(Applicationcontents1);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    //mywindow.print();
    //mywindow.close();

    // return true;
    //$("#divHeightChart").show();
    //$("#divShowQRCode").hide();
    //$(".action").show();
}

function printform7() {
  
    var Applicationcontents11 = "";
    Applicationcontents11 = $('#divPrintForm7').html();

    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>KGID Details</h2></div><div class="form-group col-4"></div></div>' + $("#divKGID").html() ;
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Family Details</h2></div><div class="form-group col-4"></div></div>' + $("#divFamily").html();
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Nominee Details</h2></div><div class="form-group col-4"></div></div>' + $("#divNominee").html() + '<div class="html2pdf__page-break"></div>';
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Personal Details</h2></div><div class="form-group col-4"></div></div>' + $("#divPrintPD").html();
    //Applicationcontents = Applicationcontents + '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h2>Declaration</h2></div><div class="form-group col-4"></div></div>' + $("#divDeclaration").html() + '<input type="checkbox" value="true" checked /><label>I agree to the terms and condition mentioned above.</label>' + "<hr />";

    var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();

    html2pdf(Applicationcontents11, {
        margin: 0.3,
        filename:+ $('#hdnApplnGISNCRnumber').val() +"Form 7" + "_" + date,
        image: { type: 'jpeg', quality: 1 },
        html2canvas: { scale: 0, letterRendering: true },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'Portrait' },
        pagebreak: { mode: 'legacy' }
    });
    let mywindow = window.open('', 'PRINT', 'height=650,width=900,top=100,left=150');

    mywindow.document.write('<html><head><title>Form7' + date + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(Applicationcontents11);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    //mywindow.print();
    mywindow.close();

    // return true;
    //$("#divHeightChart").show();
    //$("#divShowQRCode").hide();
    //$(".action").show();
}

function GisNCRSaveDeclaration() {

    var EmpId = $('#hdnempidGIS').val();
    var AppId = $('#hdnApplnGIS').val();

   
    $.ajax({
      

        url: '/GISNomineeChangeReq/GIS_NR_SaveDeclaration',
        data: { EmpId: $('#hdnempidGIS').val(), AppId: $('#hdnApplnGIS').val(), },
        type: 'POST',
        success: function (result) {          
            if (result == 3) { alertify.error("There is no DDO available for this DDO code!"); }
            else { window.location.href = "/gis-ncr-view-app/";}

        }, error: function (result) {
            //alertify.error("");
        }
    });

}

function validateNoneForNext() {
    var isNoneAdded = $(".nRelation:contains('None')").length;
    if (isNoneAdded > 0) {
        return true;
    }
    else { return false; }
}



