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





function EmployeeSave() {
   
    $("#hdnmarriedstatus").val($("#ddlDRType").val());
    $("#lblEmployeeName").html($("#txtEmployeeFullName").val())

    var formData = new FormData($("#frmAppBasicDetails").get(0));

    $.ajax({
        url: '/GroupInsurance/GISInsertBasicDetails',
        /*url: '/Employee/InsertBasicDetails',*/
        data: formData,
        async: false,
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            GetBasicDetails();
            $("#hdnBReferanceNo").val(result);
            BserverResponse = true;
        }, error: function (result) {
            alertify.error("Could not save basic details");
        }
    });

    return BserverResponse;
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

function printform() {
    //GetNomineeDetails();
    var Applicationcontents = "";

    Applicationcontents = $('#divPrintForm').html();
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

$("#btnempNext").click(function (e) {
    debugger;
    var result = false
    if (ValidateBasicDetailsForGis() == true && validatePincode() == true)
    {
     result = SaveBasicDetails123();
   
    if (result == true) {
        $("#nav-Application").removeClass("show active");
        $("#nav-Nominee").addClass("show active");
        $("#nav-Payment").removeClass("show active");
        $("#nav-Declaration").removeClass("show active");
        $("#nav-Upload").removeClass("show active");
       
       

        $("#nav-Application-tab").removeClass("active");
        $("#nav-Nominee-tab").addClass("active");
        $("#nav-Payment-tab").removeClass("active");
        $("#nav-Declaration-tab").removeClass("active");
        $("#nav-Upload-tab").removeClass("active");
       
        GisGetBasicDetails();
        GisGetNomineeDetails();
       
        $("#nav-Nominee").removeClass("disabled");
        $("#nav-Nominee-tab").removeClass("disabled");
        }
    }
    //LoadUploadedDocuments();
});

$("#btnNomineePrevious").click(function (e) {
 
    $("#nav-Application").addClass("show active");
      $("#nav-Nominee").removeClass("show active");   
    $("#nav-Payment").removeClass("show active");
    $("#nav-Declaration").removeClass("show active");
    $("#nav-Upload").removeClass("show active");



    $("#nav-Application-tab").addClass("active");
    $("#nav-Nominee-tab").removeClass("active");    
    $("#nav-Payment-tab").removeClass("active");
    $("#nav-Declaration-tab").removeClass("active");
    $("#nav-Upload-tab").removeClass("active");

    GisGetBasicDetails();
    GisGetNomineeDetails();
    $("#nav-Nominee").removeClass("disabled");
    $("#nav-Nominee-tab").removeClass("disabled");
    
});

$("#btnNomineeNext").click(function (e) {
  
    var result;
   
    var rowCount = $("#tblNomineeDetails1 tbody tr[data-row-number]").length
    if (rowCount > 0) {
        debugger;
      
        if (validateNoneApp() == true) { result = true; }
        else { result = validatesumofshare(); }
       
       
        if (result == true) {
            
             $("#nav-Application").removeClass("show active");
             $("#nav-Nominee").removeClass("show active");
             $("#nav-Payment").removeClass("show active");
            $("#nav-Declaration").addClass("show active");
             $("#nav-Upload").removeClass("show active");



             $("#nav-Application-tab").removeClass("active");
             $("#nav-Nominee-tab").removeClass("active");
             $("#nav-Payment-tab").removeClass("active");
             $("#nav-Declaration-tab").addClass("active");
             $("#nav-Upload-tab").removeClass("active");


           
            $("#nav-Declaration").removeClass("disabled");
            $("#nav-Declaration-tab").removeClass("disabled");
            GisGetBasicDetails();
            GisGetNomineeDetails();
            GisGetDeclaration();
           
        }
        else {
            alertify.alert('Percentage share for nominees should be 100 %.').setHeader("Warning!!!");
        }
    }
    else { alertify.alert('Please enter nominee details.').setHeader("Warning!!!"); }
});

$("#btnempInitialPayPre").click(function (e) {

    debugger;
 
        $("#nav-Application").removeClass("show active");
        $("#nav-Nominee").removeClass("show active");
        $("#nav-Payment").removeClass("show active");
        $("#nav-Declaration").addClass("show active");
        $("#nav-Upload").removeClass("show active");



        $("#nav-Application-tab").removeClass("active");
        $("#nav-Nominee-tab").removeClass("active");
        $("#nav-Payment-tab").removeClass("active");
        $("#nav-Declaration-tab").addClass("active");
        $("#nav-Upload-tab").removeClass("active");

    GisGetBasicDetails();
    GisGetNomineeDetails();
        GisGetPaymentDetails();
    $("#nav-Payment-tab").removeClass("disabled");
    $("#nav-Payment").removeClass("disabled");
});

$("#btnempInitialPayNext").click(function (e) {

    debugger;


   
    GisGetBasicDetails();
    GisGetNomineeDetails();
    GisGetPaymentDetails();
    GisGetDeclaration();
    
    
    $("#nav-Application").removeClass("show active");
    $("#nav-Nominee").removeClass("show active");
    $("#nav-Payment").removeClass("show active");
    $("#nav-Declaration").removeClass("show active");
    $("#nav-Upload").addClass("show active");



    $("#nav-Application-tab").removeClass("active");
    $("#nav-Nominee-tab").removeClass("active");
    $("#nav-Payment-tab").removeClass("active");
    $("#nav-Declaration-tab").removeClass("active");
        $("#nav-Upload-tab").addClass("active");
    

   
    
    printform1();
    printform6();
    printform7();
   
  /* setTimeout(function () { printform1(); }, 5000);*/


    //setTimeout(function () { printform6(); }, 5000);
    //setTimeout(function () { printform7(); }, 5000);
    $("#nav-Upload-tab").removeClass("disabled");
    $("#nav-Upload").removeClass("disabled");
});

$("#btnempNextDeclaration").click(function (e) {

    debugger;

    //
    if ($("#chkDeclaration").prop("checked") == false) {
        alertify.alert("Please agree to the terms and condition.").setHeader("Warning !!!");
        return false;
    }
    else {

        var EmpId = $('#hdnempidGIS').val();
        var AppId = $('#hdnApplnGIS').val();

      




        $("#nav-Application").removeClass("show active");
        $("#nav-Nominee").removeClass("show active");
        $("#nav-Declaration").removeClass("show active");
        $("#nav-Payment").addClass("show active");
        $("#nav-Upload").removeClass("show active");



        $("#nav-Application-tab").removeClass("active");
        $("#nav-Nominee-tab").removeClass("active");
        $("#nav-Declaration-tab").removeClass("active");
        $("#nav-Payment-tab").addClass("active");
        $("#nav-Upload-tab").removeClass("active");


       


    }
  

  
});

$("#btnempPreDeclaration").click(function (e) {

    debugger;

    $("#nav-Application").removeClass("show active");
    $("#nav-Nominee").addClass("show active");
    $("#nav-Payment").removeClass("show active");
    $("#nav-Declaration").removeClass("show active");
    $("#nav-Upload").removeClass("show active");



    $("#nav-Application-tab").removeClass("active");
    $("#nav-Nominee-tab").addClass("active");
    $("#nav-Payment-tab").removeClass("active");
    $("#nav-Declaration-tab").removeClass("active");
    $("#nav-Upload-tab").removeClass("active");

    GisGetBasicDetails();
 
    GisGetPaymentDetails();
    GisGetDeclaration();
    
    $("#nav-Declaration-tab").removeClass("disabled");
    $("#nav-Declaration").removeClass("disabled");
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

  
    //$("#tblNomineeDetails").children().remove();
    $.ajax({
        url: '/GroupInsurance/GISNomineeDetails',
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
        url: '/GroupInsurance/GISEmployeeBasicDetails',
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
        url: '/GroupInsurance/GISDeclaration',
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
        filename: $("#hdnApplnGISNumber").val()+"ApplicationForm" + "_" + date,
        image: { type: 'jpeg', quality: 1 },
        html2canvas: { scale: 0, letterRendering: true },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'Portrait' },
        pagebreak: { mode: 'legacy' }
    });
    let mywindow = window.open('', 'SAVE', 'height=650,width=900,top=100,left=150');

    mywindow.document.write('<html><head><title>ApplicationForm' + date + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(Applicationcontents);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    //mywindow.print()
    //mywindow.close();
    //$(".action").show();
    //$(".action").print();

  //  return true;
    //$("#divHeightChart").show();
    //$("#divShowQRCode").hide();
    //$(".action").show();
    //alert("   printform6();")
    //printform6();
}

function printform7() {
   
    var Applicationcontents1 = "";
    Applicationcontents1 = $('#divPrintForm7').html();
  
   
    var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();
 
    html2pdf(Applicationcontents1, {
        margin: 0.3,
        filename: $("#hdnApplnGISNumber").val()+"Form-7" + "_" + date,
        image: { type: 'jpeg', quality: 1 },
        html2canvas: { scale: 0, letterRendering: true },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'Portrait' },
        pagebreak: { mode: 'legacy' }
    });
    let mywindow = window.open('', 'SAVE', 'height=650,width=900,top=100,left=150');

    mywindow.document.write('<html><head><title>Form7' + date + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(Applicationcontents1);
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

function printform6() {
    var Applicationcontents11 = "";
    Applicationcontents11 = $('#divPrintForm6').html();

    var date = new Date().getDate() + '_' + (parseInt(new Date().getMonth()) + parseInt(1)) + '_' + new Date().getFullYear();

    html2pdf(Applicationcontents11, {
        margin: 0.3,
        filename: $("#hdnApplnGISNumber").val()+"Form-6" + "_" + date,
        image: { type: 'jpeg', quality: 1 },
        html2canvas: { scale: 0, letterRendering: true },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'Portrait' },
        pagebreak: { mode: 'legacy' }
    });
    let mywindow = window.open('', 'SAVE', 'height=650,width=900,top=100,left=150');

    mywindow.document.write('<html><head><title>Form6' + date + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(Applicationcontents11);
    mywindow.document.write('</body></html>');
  
    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    //mywindow.print();
    //mywindow.close();
   //  mywindow.document.save();

    // return true;
    //$("#divHeightChart").show();
    //$("#divShowQRCode").hide();
    //$(".action").show();
}

function printApplicationForm() {
 
    var contents = '<div class="row"><div class="form-group col-5"></div><div class="form-group col-3"><h3>Basic Details</h3></div><div class="form-group col-4"></div></div>' + $("#divPrintForm").html() + "<hr />";
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


function validateNoneApp() {
         
    var isNoneAdded = $(".nRelation:contains('None')").length;
 
    if (isNoneAdded > 0) {
        return true;
    }
    else { return false; }
}

    