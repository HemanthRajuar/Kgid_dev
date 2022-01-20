function GenerateChallan() {
    if ($('#txtPayAmount').val()) {
        $.getJSON("../employee/GenerateChallan", { amount: $('#txtPayAmount').val() },
            function (data) {
                $('#txtpaymentrefno').val(data);
            });
    }
    else {
        bootbox.alert("<p class='bootbox-alert'>Please enter amount</p>");
    }
}
//function SaveGISChallanDetails() {
//    if (!$('#txtPayAmount').val()) {
//        bootbox.alert("<p class='bootbox-alert'>Please enter amount</p>");
//    }
//    else if (!$('#txtdateofpayment').val()) {
//        bootbox.alert("<p class='bootbox-alert'>Please enter date of challan</p>");
//    }
//    else if (!$('#txtpaymentrefno').val()) {
//        bootbox.alert("<p class='bootbox-alert'>Please enter reference no</p>");
//    }
//    else {
//        $.ajax({
//            url: '/Employee/AddChallanDetails',
//            data: { amount: $('#txtPayAmount').val(), challanNo: $('#txtpaymentrefno').val(), challanDate: $('#txtdateofpayment').val() },
//            type: 'POST',
//            success: function (result) {GISSaveChallanDetails
//                console.log(result);
//                $('#txtpaymentrefno1').val(result.cd_challan_ref_no);
//                $('#txtamount1').val(result.cd_amount);
//                $('#txttransactionno1').val('113001150541');
//                var date = new Date(parseInt(result.cd_datetime_of_challan.substr(6)));
//                $('#txtdateofpayment1').val(date);
//                bootbox.alert("<p class='bootbox-alert'>Challan Details Successfully Added.</p>");
//            }
//        });
//    }

//}

function GISSaveChallanDetails(eid) {

  
    //var viewModel = {
    //    'gcd_system_emp_code': eid, 'gcd_referance_number': $("#spnReferanceNo").text(), 'gcd_challan_ref_no': $("#txttransactionno1").val(),//$("#txtpaymentrefno").val(),
    //    'gcd_dept_code': $("#txtDistrictOfffice").val(), 'gcd_ddo_code': $("#txtDistrictOfffice").val(), 'gcd_purpose_code': $("#txtPurpose").val(), 'gcd_subpurpose_code': $("#txtSubPurpose").val(),
    //    'gcd_head_of_account': $("#txtHOA").val(), 'gcd_amount': $("#txtPayAmount").val(), 'gcd_datetime_of_challan': new Date(), 'gcd_status': '1', 'gcd_creation_datetime': new Date()
    //};
    var viewModel = {
        'EmpID': eid,
        'gcd_challan_ref_no': $('#txtChallanRefNo').val(),//$("#txtpaymentrefno").val(),
        'ddo_code_id': $('#txtddocodeid').val(),
        'ddo_code': $('#txtDDOCode').val(),
        'receipttypeid': $('#ddlReceiptType').val(),
        'gcd_purpose_code': $("#txtPurpose").val(),
        'gcd_subpurpose_code': $("#txtSubPurpose").val(),
        'gcd_purpose_id': $("#txtPurposeCodeId").val(),
        'gcd_sub_purpose_id': $("#txtSubpurposeId").val(),
        'gcd_head_of_account': $("#txtHOA").val(),
        'gcd_amount': $("#txtPDAmount").val(),
        'gcd_datetime_of_challan': new Date(),
        'gcd_status': '1',
        'gcd_creation_datetime': new Date(),
        'gcd_application_id': $("#txtAppID").val(),
        'gcd_insurance_amount': $("#hdninsurance_amount").val(),
        'gcd_savings_amount': $("#hdngcd_savings_amount").val(),
        
    };
    $.ajax({
       // url: '/Employee/InsertChallanDetails',
        url: '/GroupInsurance/GISInsertChallanDetails',
        data: JSON.stringify(viewModel),
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $("#txtRefNo").val($("#spnReferanceNo").text());
            $("#btnempInitialPaySave").hide();
            alertify.success("Challan details saved successfully");
            CserverResponse = true;
            GISSaveChallanStatus(eid);
         //   console.log(result);
//                $('#txtpaymentrefno1').val(result.cd_challan_ref_no);
//                $('#txtamount1').val(result.cd_amount);
//                $('#txttransactionno1').val('113001150541');
//                var date = new Date(parseInt(result.cd_datetime_of_challan.substr(6)));
//                $('#txtdateofpayment1').val(date);
            //   bootbox.alert("<p class='bootbox-alert'>Challan Details Successfully Added.</p>");
        }, error: function (result) {
            alertify.error("Could not save Challan details");
        }
    });

    return CserverResponse;



  
}


function GISSaveChallanStatus(eid) {

  
   // var PaymentDetails = {};
   // PaymentDetails.gcs_transaction_ref_no = $("#txttransactionno1").val();
   // PaymentDetails.gcs_amount = $("#txtPayAmount").text();
   // PaymentDetails.gcs_status = 1;
   // PaymentDetails.EmpID = $("#hdnEmployeeID").val();
   // PaymentDetails.gcd_application_id = $("#hdnAppID").val();
   ///* PaymentDetails.gcs_challan_id = $("#hdnChallanID").val();*/
   // PaymentDetails.gcd_challan_ref_no = $("#txttransactionno1").text();
  
    var viewModel = {
        'EmpID': eid,
        'gcs_transaction_ref_no ': "MHAF32432",//$("#txtpaymentrefno").val(),
        'gcs_amount': $("#txtPDAmount").val(),
        'gcs_status': "2",       
        'gcs_active_status': "1",
        'gcd_application_id': $("#hdnApplicationID").val(),
    };
  

    $.ajax({
        // url: '/Employee/InsertChallanDetails',
        //url: '/GroupInsurance/PaymentDetails',
        url: '/GroupInsurance/InsertChalanStatusDetails',
        data: JSON.stringify(viewModel),
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $("#txtRefNo").val($("#spnReferanceNo").text());
            $("#btnempInitialPayFinish").hide();
            alertify.success("Payment details saved successfully");
            CserverResponse = true;



            GisGetBasicDetails();
            GisGetNomineeDetails();
            GisGetPaymentDetails();
            GisGetDeclaration();

            printform1();
            printform6();
            printform7();
            /*setTimeout(function () { printform1(); }, 5000);*/
            $("#nav-Upload-tab").removeClass("disabled");
            $("#nav-Upload").removeClass("disabled");
            //setTimeout(function () { printform6(); }, 5000);

            //setTimeout(function () { printform7(); }, 5000);


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
          

            GisGetBasicDetails();
            GisGetNomineeDetails();
            GisGetPaymentDetails();
            GisGetDeclaration();
         //  PrintChallanDetails();
         
        }, error: function (result) {
            alertify.error("Could not save Payment details");
        }
    });
 
   // printApplicationForm();
    return CserverResponse;




}

function PrintChallanDetails() {
    var empid = $('#hdnEmployeeID').val();
    var appid = $('#hdnAppID').val();
   
   // console.log($('#txtpaymentrefno1').val());
    $.ajax({
        url: '/GroupInsurance/PrintChallanDetails',
        data: { EmpId: empid, AppId: appid, challanNo: $('#txtChallanRefNo').val() },
        type: 'POST',
        success: function (result) {
            window.location = "/GroupInsurance/PrintChallanDetails?challanNo=" + $('#txtpaymentrefno1').val();
           // $('#PaymentTab ul[aria-label=Pagination] li a[href="#finish"]').css("display", "block");
        }
    });
}

//PAYMENT 

