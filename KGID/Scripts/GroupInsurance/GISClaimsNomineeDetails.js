//Variable Declarations
var alldead = true;
var nomineeDetails = new Array();
var guardianRelation = "";
var table;
var previoussharevalue = 0;
var guardianList = new Array();
var rowNumber = 0;
var selectedRelation;
var filepath;
$(document).ready(function () {
    var today = new Date();
    $('#txtNomineeDOB').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        maxDate: 0,
        minDate: new Date(today.getFullYear() - 60, today.getMonth(), today.getDate()),
        yearStart: today.getFullYear() - 60,
        yearEnd: today.getFullYear(),
        scrollMonth: false,
        scrollInput: false,
        autoclose: true,
        closeOnDateSelect: true
    });
    $('#txtEditNomineeDOB').datetimepicker({
        timepicker: false,
        format: 'd-m-Y',
        maxDate: 0,
        minDate: new Date(today.getFullYear() - 60, today.getMonth(), today.getDate()),
        yearStart: today.getFullYear() - 60,
        yearEnd: today.getFullYear(),
        scrollMonth: false,
        scrollInput: false,
        autoclose: true,
        closeOnDateSelect: true
    });
})

$(document).ready(function () {
    debugger;

    if ($.fn.dataTable.isDataTable('#tblNomineeDetails1')) {
        table = $('#tblNomineeDetails1').DataTable();
    }
    else {
        table = $('#tblNomineeDetails1').DataTable({
            paging: false,
            info: false,
            searching: false,
            "ordering": false
        });
    }

    $("#tblFamilyDetails TBODY TR").each(function () {
        var $row = $(this);
        var livingStatus = $row.find(".tdLivingStatus").text();
        var memberName = $row.find(".tdName").text();
        var relation = $row.find(".tdRelation").text();
        var famAge = $row.find(".tdAge").text();
        var memberId = $row.data("fd-id");

        if (livingStatus === "Alive") {
            isEmpOrphan = false;
            alldead = false;
            var familyDetails = {};
            familyDetails.memberId = memberId;
            familyDetails.name = memberName;
            familyDetails.relation = relation;
            familyDetails.age = famAge;
            nomineeDetails.push(familyDetails);
        }
    });

    var eachSumShare = 0;
    $('.val-share1').each(function () {
        eachSumShare = eachSumShare + parseInt($(this).text());
    });

    if (eachSumShare < 100) {
        $("#btnNomAdd").removeAttr("disabled");
    }
    else {
        $("#btnNomAdd").attr("disabled", true);
    }
    $("#txtNomineeName").attr("disabled", "true");
    $("#txtNomineeDOB").attr("disabled", "true");
    $("#txtNomineeShare").attr("disabled", "true");
    // ShowFields();
});

function ShowFields() {

    //if (type == "Orphan" || type == "Guardian") {
    //    $("#txtNomineeName").show();
    //    $("#txtEditNomineeName").show();
    //    $("#ddlNomineeName").hide();
    //    $("#ddlEditNomineeName").hide();
    //    $("#ddlNomineeRelation").removeAttr("disabled");
    //    $("#ddlEditGuardianRelation").removeAttr("disabled");
    //    $("#divAge").hide();
    //}
    //else {
    //    $("#txtNomineeName").hide();
    //    $("#txtEditNomineeName").hide();
    //    $("#ddlNomineeName").show();
    //    $("#ddlEditNomineeName").show();
    //    $("#ddlNomineeRelation").attr("disabled", "true");
    //    $("#ddlEditGuardianRelation").attr("disabled", "true");
    //    $("#divAge").show();
    //}

    $("#txtNomineeName").show();
    $("#txtEditNomineeName").hide();
    $("#ddlNomineeName").show();
    $("#ddlEditNomineeName").show();
    $("#ddlNomineeRelation").attr("disabled", "true");
    $("#ddlEditGuardianRelation").attr("disabled", "true");
    $("#divAge").show();
    $("#ddlGuardianRelation").attr("disabled", "true");
    $("#txtGuardianName").attr("disabled", "true");
    $("#txtNomineeAge").removeAttr("disabled");


}

$(document).on('change', '#txtNomineeName', function () {
    $("#txtNomineeDOB").removeAttr("disabled");
    $("#txtNomineeShare").removeAttr("disabled");
});

$(document).on('change', '#txtEditNomineeName', function () {
    $("#txtEditNomineeDOB").removeAttr("disabled");
    $("#txtEditNomineeShare").removeAttr("disabled");
});




//assign file path
function getpath(elem) {

    //  $(elem).attr('href',"")
    //  $(elem).attr('href', $(elem).attr('href') + '?FilePath=' + temp);
    $(elem).attr('href', $(elem).attr('href') + '?FilePath=' + filepath);




    //var oldUrl = $(elem).attr('href', $(elem).attr('href') + '?FilePath=' + temp);; // Get current url
    //var newUrl = oldUrl.replace($(elem).attr('href', $(elem).attr('href') + '?FilePath=' + temp), $(elem).attr('href', $(elem).attr('href') + '?FilePath=' + temp)); // Create new url
    //$(this).attr("href", newUrl); // Set herf value


    //  window.location href = "@Url.Action("ViewFilePath", "Home")" + "/" + selectedValue"
    // location. = '@Url.Action("ViewFilePath", "Home", new { FilePath ="' + filepath + '", target="_blank"})';

    //alert("hereqq");
    ////location.target = "_blank";
    //window.location.href = "Home/ViewFilePath?FilePath=" + filepath;


}

function EditNomineeDetails(rowNumber, Id) {
    var relationName;
    // $("#ddlEditNomineeList option[value=Select]").attr("selected", "selected");
    var relationID = 0;
    //  var $row = $("#divNominee tr[data-row-number=" + rowNumber + "]");
    var isOrphan = $(".rd-btn-orphan:checked").val();
    $("#hdnRowNumber").val(rowNumber);
    $("#hdnId").val(Id);
    var $row = $("#tblNomineeDetails1 tr[data-row-number=" + rowNumber + "]");
    var relation = $row.find("TD").eq(4).html();
    var relationID = $row.find("TD").eq(9).html();

    $("#ddlEditNomineeList option[value=" + relationID + "]").attr("selected", "selected");
    $("#ddlEditNomineeList").val(relationID);
    //$("#ddlEditNomineeList").text()
    //  var relation = $("#ddlEditNomineeList option:contains('" + relation + "')").attr("selected", "selected");
    var dob1 = $row.find("TD").eq(2).html();
    var nomineeAge = parseInt($row.find("TD").eq(3).html());
    relationName = $row.find("TD").eq(1).html();
    // $("#ddlEditNomineeList").text(relationName);
    //var gnd_contingencies1 = $row.find("TD").eq(7).html();
    //var gnd_predeceasing1 = $row.find("TD").eq(8).html();

    var gnd_contingencies1 = $row.find("TD").eq(6).html();
    var gnd_predeceasing1 = $row.find("TD").eq(7).html();
    //var SonDaughterAdoption_doc_path = $row.find("TD").eq(7).html();
    filepath = $row.find("TD").eq(8).html();


    if (relation == "Adopted Son" || relation == "Adopted Daughter") {
        $("#EditadoptionformuploadDiv").show();
        $("EditadoptionformuploadDiv").attr("style", "display:block")
    }
    else {
        $("#EditadoptionformuploadDiv").hide();
        $("EditadoptionformuploadDiv").attr("style", "display:none")
    }


    $("#txtEditNominecontingencies").val(gnd_contingencies1);
    $("#txtEditNomineepredeceasing").val(gnd_predeceasing1);
    //  $("#txtEditNomineeShare").val($row.find("TD").eq(5).html());

    $("#txtEditNomineeName").val("");
    $("#ddlEditNomineeName option").removeAttr("selected");
    $("#txtEditNomineeDOB").val("");
    $("#txtEditNomineeAge").val("");

    $("#txtEditNomineeShare").val("");
    $("#ddlEditNomineeRelation option").removeAttr("selected");


    //  $("#ddlEditNomineeList").val(relation);
    $("#txtEditNomineeName").val(relationName);
    if (relation == "Father") {
        $("#txtEditNomineeName").prop("disabled", true);
    }

    // Date of birth -- validation to show calendar starting date

    if (relation === "Father") {
        var today = new Date();
        var empdob = $("#txtBasicDateOfBirth").val();
        var dat;
        /*var dat = empdob.split("-");*/
        if (empdob.includes('/')) { dat = empdob.split("/"); }
        if (empdob.includes('-')) { dat = empdob.split("-") }
        if (empdob.includes('.')) { dat = empdob.split(".") }

        var d = dat[1] + "-" + dat[0] + "-" + dat[2];
        var dob = new Date(d);
        $("#txtEditNomineeName").prop("disabled", true);
        if ($('#father_name').val() != "") {
            // $('#txtEditNomineeName').val($('#father_name').val());
            //  $("#txtEditNomineeName").prop("disabled", true);
            $("#txtEditNomineeDOB").removeAttr("disabled");
            $("#txtEditNomineeShare").removeAttr("disabled");
        }

        var options = {
            timepicker: false,
            format: 'd-m-Y',
            maxDate: new Date((dob.getFullYear() - 10), 11, 31),
            minDate: new Date(today.getFullYear() - 100, 0, 1),
            defaultDate: new Date((dob.getFullYear() - 10), dob.getMonth(), dob.getDate()),
            yearStart: today.getFullYear() - 100,
            yearEnd: dob.getFullYear() - 10,
            scrollMonth: false,
            scrollInput: false,
            autoClose: true,
            closeOnDateSelect: true
        };

        $('#txtEditNomineeDOB').datetimepicker("destroy");
        $('#txtEditNomineeDOB').datetimepicker(options);

    }
    //

    //else {// $("#txtEditNomineeName").removeAttr("disabled");
    //}
    //   $("#ddlEditNomineeList option:equals(" + relation + ")").attr("selected", "selected");
    //commented now
    //$("#ddlEditNomineeList option[value=" + relationID +"]").attr("selected", "selected");
    //alert($("#ddlEditNomineeList").val())
    //alert(relationID);
    //$("#ddlEditNomineeList option[value=" + relationID + "]").attr("selected", "selected");
    //commented now
    $("#txtEditNomineeAge").val(nomineeAge);
    $("#txtEditNomineeDOB").val(dob1);
    $("#txtEditNomineeShare").val($row.find("TD").eq(5).html());

    previoussharevalue = $("#txtEditNomineeShare").val();
    //if (relation != "" && relation != undefined && nomineeDetails.length > 0) {
    //    var member = nomineeDetails.find(x => x.relation == relation);
    //    $('#ddlEditNomineeRelation').empty();
    //    $('#ddlEditNomineeRelation').append(new Option(member.relation, member.relation));
    //  //  $("#ddlEditNomineeRelation option:contains(" + member.relation + ")").attr("selected", "selected");
    //     $('#ddlEditNomineeRelation option:contains(" + relation + ")').val();
    //   // $("#ddlEditNomineeList option[value=" + member.relation + "]").attr("selected", "selected");
    //}
    //else {
    //    var rel = $('#ddlEditNomineeRelation option:contains(" + relation + ")').val();
    //    $("#ddlEditNomineeRelation").val(rel);
    //}
    $("#mdUpdateNominee").modal("show");
}



//// Add & Update in nominee grid




function addRow(result) {
    if ($("#divNominee").find(".dataTables_empty").length === 1) {
        $("#divNominee").find(".dataTables_empty").parent("tr").remove();
    }
    else {
        skipSumShareValidation = false;
    }
    var nomineeName = nomineeName = $("#txtNomineeName").val();
    var NName = nomineeName;
    var NAge = $("#txtNomineeAge").val();
    var Relation = $("#ddlNomineeList option:selected").text();
    var NShare = $("#txtNomineeShare").val();
    var NDOB = $("#txtNomineeDOB").val();
    var RelationID = $("#ddlNomineeList").val();

    var contingencies = $("#txtNomineecontingencies").val();
    var Nomineepredeceasing = $("#txtNomineepredeceasing").val();
    var path = $("#txtAdoptionformDoc").val();
    var newClass;
    if ($("#divNominee tr[data-row-number=" + $("#divNominee tr[data-row-number]").length + "]").hasClass("even")) {
        newClass = "odd";
    }
    else {
        newClass = "even";
    }
    if (rowNumber == 0)
        rowNumber = $("#tblNomineeDetails1 TBODY TR").length + 1;
    else
        rowNumber = rowNumber + 1;

    //  var AddRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'> <td hidden>" + result + "</td> <td class='nName'>" + NName + "</td><td class='nDOB'>" + NDOB + "</td><td>" + NAge + "</td> <td class='nRelation'>" + Relation + "</td><td class='val-share1'>" + NShare + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditNomineeDetails(" + rowNumber + "," + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteNomineeDetails(" + result + "," + rowNumber + ");'>Delete</a></td></tr>";

    // var AddRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'> <td hidden>" + result + "</td> <td class='nName'>" + NName + "</td><td class='nDOB'>" + NDOB + "</td><td>" + NAge + "</td> <td class='nRelation'>" + Relation + "</td><td class='val-share1'>" + NShare + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditNomineeDetails(" + rowNumber + "," + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteNomineeDetails(" + result + "," + rowNumber + ");'>Delete</a></td>" + "<td class='ncontingencies'>" + contingencies + "</td>" + "<td class='nNomineepredeceasing'>" + Nomineepredeceasing + "</td> " + " < td class='npath' > " + path + "</td> " + " < td class='relationID' > " + RelationID + "</td></tr>";

    var AddRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'> <td hidden>" + result + "</td> <td class='nName'>" + NName + "</td><td class='nDOB'>" + NDOB + "</td><td>" + NAge + "</td> <td class='nRelation'>" + Relation + "</td><td class='val-share1'>" + NShare + "</td><td class='ncontingencies'>" + contingencies + "</td>" + "<td class='nNomineepredeceasing'>" + Nomineepredeceasing + "</td> " + " < td class='npath' > " + path + "</td> " + " < td class='relationID' > " + RelationID + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditNomineeDetails(" + rowNumber + "," + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteNomineeDetails(" + result + "," + rowNumber + ");'>Delete</a></td></tr>";

    $("#tblNomineeDetails1 tbody").append(AddRow);

}

function UpdateNomineeDetails() {

    debugger;
    var i = 0;
    var theTotal = 0;
    var share = 0;
    var diff = 0;

    share = $("#txtEditNomineeShare").val();

    var table = $('#tblNomineeDetails1').DataTable();

    table
        .column(5)
        .data()
        .each(function (value, index) {
            //console.log('Data in index: ' + index + ' is: ' + value);
            //alert('Data in index: ' + index + ' is: ' + value);

            //  var val = this.innerText;
            theTotal += parseInt(value);
        });


    //$("#tblNomineeDetails1 tr td:nth-child(5)").each(function () {
    //    var val = this.innerText;
    //    theTotal += parseInt(val);
    //});

    theTotal -= previoussharevalue;
    diff = 100 - parseInt(theTotal);

    if (share > diff && diff > 0) {
        i = 2;
        alertify.alert("Percentage share can not be more than " + diff + "%.").setHeader("warning!!!");;
        return false;
    }
    debugger;
    //   var result = validateEditNomineeFields();

    var valresult;
    if ($("#ddlEditNomineeList").val() == 14) { valresult = true }
    else { valresult = validateEditNomineeFields(); }



    if (valresult == true) {
        //var viewModelNominee = {
        //    'Id': $("#hdnId").val(),
        //    'EmpId': $("#hdnPReferanceNo").val(),
        //    'NameOfNominee': $("#txtEditNomineeName").val(),
        //    'Relation': $("#ddlEditNomineeList").val(),
        //    'PercentageShare': $("#txtEditNomineeShare").val(),
        //    'Age': $("#txtEditNomineeAge").val(),// guardianAge
        //    'nomineeDOB': $("#txtEditNomineeDOB").val()
        //};
        var formData = new FormData();
        var file = document.getElementById("txtEditAdoptionformDoc").files[0];

        if ($("#txtEditAdoptionformDoc").val().length > 0) { formData.append("SonDaughterAdoption_doc", file); }
        else { formData.append("SonDaughterAdoption_doc_path", filepath); }
        formData.append("Id", $("#hdnId").val());
        formData.append("EmpId", $("#hdnPReferanceNo").val());
        formData.append("NameOfNominee", $("#txtEditNomineeName").val());
        formData.append("Relation", $("#ddlEditNomineeList").val());
        formData.append("PercentageShare", $("#txtEditNomineeShare").val());
        formData.append("Age", $("#txtEditNomineeAge").val());
        formData.append("nomineeDOB", $("#txtEditNomineeDOB").val());
        // formData.append("SonDaughterAdoption_doc", $("#txtEditAdoptionformDoc").val());

        formData.append("SonDaughterAdoption_doc_path", file);

        formData.append("gnd_contingencies", $("#txtEditNominecontingencies").val());
        formData.append("gnd_predeceasing", $("#txtEditNomineepredeceasing").val());


        {
            if (i != 2) {
                $.ajax({
                    ////  url: '/Employee/InsertNomineeDetails',
                    //url: '/GroupInsurance/GISInsertNomineeDetails',
                    //data: JSON.stringify(viewModelNominee),
                    //async: false,
                    //type: 'POST',
                    //contentType: 'application/json; charset=utf-8',

                    url: '/GroupInsurance/GISInsertNomineeDetails',
                    data: formData,
                    async: false,
                    type: 'POST',
                    cache: false,
                    contentType: false,
                    /*contentType: 'application/json; charset=utf-8',*/
                    processData: false,

                    success: function (result) {
                        //    $("#mdUpdateNominee").modal("hide");

                        //   NserverResponse = true;
                        //  updateRow(result);

                        // GetNomineeDetails();

                        //  $("#btnNomineeSave").click();

                        $("#mdUpdateNominee").modal("hide");
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();
                        $('.modal-backdrop').remove();
                        GetNomineeDetails();
                        alertify.success("Nominee details updated successfully");
                    }, error: function (result) {
                        alertify.error("Could not updated nominee details");
                    }
                });
            }
        }
    }
    else {
        alertify.alert('Percentage share for nominees should be 100 %.').setHeader("Warning!!!");
    }
    // return NserverResponse;
}

function updateRow(result) {
    if ($("#divEditNominee").find(".dataTables_empty").length === 1) {
        $("#divEditNominee").find(".dataTables_empty").parent("tr").remove();
    }
    else {
        skipSumShareValidation = false;
    }
    var nomineeName = nomineeName = $("#txtEditNomineeName").val();
    var NName = nomineeName;
    var NAge = $("#txtEditNomineeAge").val();
    var Relation = $("#ddlEditNomineeList option:selected").text();
    var NShare = $("#txtEditNomineeShare").val();
    var NDOB = $("#txtEditNomineeDOB").val();
    var RelationID = $("#ddlEditNomineeList").val();
    //  var relationID = $row.find("TD").eq(10).html();
    var contingencies = $("#txtEditNomineecontingencies").val();
    var Nomineepredeceasing = $("#txtEditNomineepredeceasing").val();
    var path = $("#txtEditAdoptionformDoc").val();
    filepath = path;

    var newClass;

    var newClass;
    if ($("#divNominee tr[data-row-number=" + $("#divNominee tr[data-row-number]").length + "]").hasClass("even")) {
        newClass = "odd";
    }
    else {
        newClass = "even";
    }
    var rowNumber = $("#hdnRowNumber").val();
    var newClass = $("#tblNomineeDetails1 tr[data-row-number=" + rowNumber + "]").attr("class");
    // var updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'> <td hidden>" + result + "</td>  <td class='nName'>" + NName + "</td> <td class='nDOB'>" + NDOB + "</td><td>" + NAge + "</td> <td class='nRelation'>" + Relation + "</td><td class='val-share1'>" + NShare + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditNomineeDetails(" + rowNumber + "," + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteNomineeDetails(" + result + "," + rowNumber + ");'>Delete</a></td></tr>";
    //  var updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'> <td hidden>" + result + "</td> <td class='nName'>" + NName + "</td><td class='nDOB'>" + NDOB + "</td><td>" + NAge + "</td> <td class='nRelation'>" + Relation + "</td><td class='val-share1'>" + NShare + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditNomineeDetails(" + rowNumber + "," + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteNomineeDetails(" + result + "," + rowNumber + ");'>Delete</a></td><td class='ncontingencies' style='display:none'>" + contingencies + "</td><td class='nNomineepredeceasing' style='display:none'>" + Nomineepredeceasing + "</td>< td class='npath' style='display:none' > " + path + "</td></tr>";

    var updatedRow = "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'> <td hidden>" + result + "</td> <td class='nName'>" + NName + "</td><td class='nDOB'>" + NDOB + "</td><td>" + NAge + "</td> <td class='nRelation'>" + Relation + "</td><td class='val-share1'>" + NShare + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditNomineeDetails(" + rowNumber + "," + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteNomineeDetails(" + result + "," + rowNumber + ");'>Delete</a></td>" + "<td class='ncontingencies' style='display:none'>" + contingencies + "</td>" + "<td class='nNomineepredeceasing'>" + Nomineepredeceasing + "</td> " + " < td class='npath' > " + path + "</td> " + " < td class='relationID' > " + RelationID + "</td></tr>";
    //   var AddRow =     "<tr data-row-number='" + rowNumber + "' role='row' class='" + newClass + "'> <td hidden>" + result + "</td> <td class='nName'>" + NName + "</td><td class='nDOB'>" + NDOB + "</td><td>" + NAge + "</td> <td class='nRelation'>" + Relation + "</td><td class='val-share1'>" + NShare + "</td><td><a href='javascript: void(0);' class='btn-sm btn-primary' onclick='EditNomineeDetails(" + rowNumber + "," + result + ");'>Edit</a>&nbsp;<a href='javascript: void(0);' class='btn-sm btn-danger' onclick='DeleteNomineeDetails(" + result + "," + rowNumber + ");'>Delete</a></td>" + "<td class='ncontingencies' style='display:none'>" + contingencies + "</td>" + "<td class='nNomineepredeceasing'>" + Nomineepredeceasing + "</td> " + " < td class='npath' > " + path + "</td> " + " < td class='relationID' > " + RelationID + "</td></tr>";
    $("#tblNomineeDetails1 tr[data-row-number=" + rowNumber + "]").replaceWith(updatedRow);
    // resetEditNomineeFields();
    $("#mdUpdateNominee").modal("hide");
    GetNomineeDetails();
}

//// Delete Nominee
function DeleteNomineeRow(rowNumber) {

    $("#tblNomineeDetails1 tr[data-row-number='" + rowNumber + "']").remove();

    if ($("#tblNomineeDetails1").find("tbody").find("tr").length === 0) {
        var AddRow = "<tr class='odd'><td valign='top' colspan='8' class='dataTables_empty'>No data available in table</td></tr>";
        $("#tblNomineeDetails1 tbody").append(AddRow);
    }

    var eachSumShare = 0;
    $('.val-share1').each(function () {
        eachSumShare = eachSumShare + parseInt($(this).text());
    });

    if (eachSumShare < 100) {
        $("#btnNomAdd").removeAttr("disabled");
    }
    else {
        $("#btnNomAdd").attr("disabled", true);
    }
}

function DeleteNomineeDetails(Id, rowNumber) {

    var viewModelNominee = {
        'Id': Id,
    };

    $.ajax({
        url: '/GroupInsurance/DeleteNomineeDetails',
        data: JSON.stringify(viewModelNominee),
        async: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result == 1) {
                alertify.success("Nominee details deleted successfully");
                // NserverResponse = true;
                DeleteNomineeRow(rowNumber);
            }
            else {
                alertify.error("Could not delete nominee details");
            }

        }, error: function (result) {
            alertify.error("Could not delete nominee details");
        }
    });
    //  return NserverResponse;
}

//Validations
$(".alphaonly").on("input", function () {
    var regexp = /[^a-z A-Z]/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});

$('.disable-first-zero').keypress(function (e) {
    if (this.value.length == 0 && e.which == 48) {
        return false;
    }
});

function setMaxNumberLength(event, control, numberOfCharacters) {
    if ((((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)) && $(control).val().length < numberOfCharacters)
        || event.keyCode === 8 || event.keyCode === 9 || event.keyCode === 13 || event.keyCode === 37 || event.keyCode === 39 || event.keyCode === 46) {
        return true;
    }
    return false;
}

function ValidateSumShare() {

    var sumShare = 0;
    var eachSumShare = 0;
    $('.val-share1').each(function () {
        eachSumShare = eachSumShare + parseInt($(this).text());
    });

    if (eachSumShare === 100) {
        return true;
    }
    return false;
}

function validateNomineeFields() {
    debugger;
    $('.err').attr('hidden', true);


    if ($("#ddlNomineeList").val() == "Select") {
        $("#errNomRelationReq").removeAttr('hidden');
        return false;
    }
    if ($("#txtNomineeName").val() == "") {
        $("#errNomNameReq").removeAttr('hidden');
        return false;
    }
    if ($("#txtNomineeDOB").val() == "") {
        $("#errDOB").removeAttr('hidden');
        return false;
    }
    if ($("#txtNomineeAge").val() == "") {
        $("#errNomineeAgeReq").removeAttr('hidden');
        return false;
    }
    if ($("#txtNomineeShare").val() == "") {
        $("#errNomineeShareReq").removeAttr('hidden');
        return false;
    }
    if (($("#ddlNomineeList").val() == 7) && $("#txtNomineeAge").val() > 18) {

        $('#errDOBAge').removeAttr("hidden");
        return false;
    }
    if (CheckDuplicateName() == true) {

        $("#errNomNameDupl").removeAttr('hidden');
        return false;
    }

    //debugger;
    //var table = $("#tblNomineeDetails1");
    //var firstTd = $("td:first", table);
    //var secondTd = firstTd.next();

    //if (firstTd.text() == $("#ddlNomineeList").val() && secondTd.text() == $("#txtNomineeName").val())
    //    alert("Error: You're trying to add the same entry");




    var txtShare = $("#txtNomineeShare").val();
    var sumShare = 0;
    var eachSumShare = 0;
    $('.val-share1').each(function () {
        eachSumShare = eachSumShare + parseInt($(this).text());
    });

    sumShare = eachSumShare + parseInt(txtShare);

    if (txtShare.trim() === '' || parseInt(txtShare) == 0) {
        $("#errNomineeShareReq").removeAttr('hidden');
        return false;
    }

    if (sumShare > 100) {
        $("#errNomineeSharePercent").removeAttr('hidden');
        return false;
    }



    /*  if ($("#ddlDRType").val() == 1) {*/
    //if ($("#ddlNomineeList").val() == "16" || $("#ddlNomineeList").val() == "17") {
    //    if ($("#flDivSupportingDoc").get(0).files[0] == "" || $("#flDivSupportingDoc").get(0).files[0] == undefined) {
    //        if ($("#divdrDocUpload:visible").length === 0) {
    //            $("#errCurrMarritalStatusReq").text("Please upload supporting document");
    //            $("#errCurrMarritalStatusReq").removeAttr("hidden");
    //        }
    //    }
    //}

    if ($("#ddlNomineeList").val() == "16" || $("#ddlNomineeList").val() == "15") {
        if ($("#txtAdoptionformDoc").val().length == 0) {
            $("#errAdoptionformDoc").removeAttr('hidden');
            //alertify.alert("Please upload  form").setHeader("warning!!!");
            return false;
        }
    }


    return true;

}

function validateEditNomineeFields() {
    debugger;
    $('.err').attr('hidden', true);


    if ($("#ddlEditNomineeList").val() == "Select") {
        $("#erEditrNomRelationReq").removeAttr('hidden');
        return false;
    }
    if ($("#txtEditNomineeName").val() == "") {
        $("#errEditNomNameReq").removeAttr('hidden');
        return false;
    }
    if ($("#txtEditNomineeDOB").val() == "") {
        $("#errEditDOB").removeAttr('hidden');
        return false;
    }
    if ($("#txtEditNomineeAge").val() == "") {
        $("#errEditNomineeAgeReq").removeAttr('hidden');
        return false;
    }

    if (($("#ddlEditNomineeList").val() == 7) && $("#txtEditNomineeAge").val() > 18) {

        $('#errEditDOBAge').removeAttr("hidden");
        return false;
    }
    if ($("#txtEditNomineeShare").val() == "") {
        $("#errEditNomineeShareReq").removeAttr('hidden');
        return false;
    }



    if (CheckDuplicateName() == true) {

        $("#errNomNameDupl").removeAttr('hidden');
        return false;
    }

    var txtShare = $("#txtEditNomineeShare").val();
    //if (txtShare == '' || parseInt(txtShare) == 0) {
    //    $("#errEditNomineeShareReq").removeAttr('hidden'); return false;
    //}
    //var txtShare = $("#txtEditNomineeShare").val();
    //var sumShare = 0;
    //var eachSumShare = 0;
    //$('.val-share').each(function () {
    //    eachSumShare = eachSumShare + parseInt($(this).text());
    //});

    //sumShare = eachSumShare + parseInt(txtShare);

    //if (txtShare.trim() === '' || parseInt(txtShare) == 0) {
    //    $("#errEditNomineeShareReq").removeAttr('hidden');
    //    return false;
    //}

    //if (sumShare > 100) {
    //    $("#errEditNomineeSharePercent").removeAttr('hidden');
    //    return false;
    //}

    //23 Novermber

    //var sumShare = 0;
    //var eachSumShare = 0;
    //$('.val-share1').each(function () {
    //    eachSumShare = eachSumShare + parseInt($(this).text());
    //});

    //sumShare = eachSumShare + parseInt(txtShare);

    //if (txtShare.trim() === '' || parseInt(txtShare) == 0) {
    //    $("#errNomineeEditShareReq").removeAttr('hidden');
    //    return false;
    //}

    //if (sumShare > 100) {
    //    $("#errNomineeEditSharePercent").removeAttr('hidden');
    //    return false;
    //}
    /*  if ($("#ddlDRType").val() == 1) {*/
    //if ($("#ddlNomineeList").val() == "16" || $("#ddlNomineeList").val() == "17") {
    //    if ($("#flDivSupportingDoc").get(0).files[0] == "" || $("#flDivSupportingDoc").get(0).files[0] == undefined) {
    //        if ($("#divdrDocUpload:visible").length === 0) {
    //            $("#errCurrMarritalStatusReq").text("Please upload supporting document");
    //            $("#errCurrMarritalStatusReq").removeAttr("hidden");
    //        }
    //    }
    //}

    if ($("#ddlEditNomineeList").val() == "16" || $("#ddlNomineeList").val() == "15") {
        if ($("#txtEditAdoptionformDoc").val().length == 0) {
            $("#errEditAdoptionformDoc").removeAttr('hidden');
            //alertify.alert("Please upload  form").setHeader("warning!!!");
            return false;
        }
    }



    return true;

}




function ValidateEmpDOB(id) {
    debugger
    $('#errDOBAge').attr("hidden", true);
    $('#errDOB').attr("hidden", true);
    var endingDate = "";
    var txtDOB = $('#txtNomineeDOB').val();
    var ddlSelectedRelation = $("#ddlNomineeList").val();

    var splitdate = txtDOB.split('-');
    var date = splitdate[0];
    var month = splitdate[1] - 1;
    var year = splitdate[2];
    if (txtDOB != "") {
        var startDate = new Date(new Date(year + "-" + splitdate[1] + "-" + date).toISOString().substr(0, 10));
        if (!endingDate) {
            endingDate = new Date().toISOString().substr(0, 10);    // need date in YYYY-MM-DD format
        }
        var endDate = new Date(endingDate);
        if (startDate > endDate) {
            var swap = startDate;
            startDate = endDate;
            endDate = swap;
        }
        var startYear = startDate.getFullYear();
        var february = (startYear % 4 === 0 && startYear % 100 !== 0) || startYear % 400 === 0 ? 29 : 28;
        var daysInMonth = [31, february, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        var yearDiff = endDate.getFullYear() - startYear;
        var monthDiff = endDate.getMonth() - startDate.getMonth();
        if (monthDiff < 0) {
            yearDiff--;
            monthDiff += 12;
        }
        var dayDiff = endDate.getDate() - startDate.getDate();
        if (dayDiff < 0) {
            if (monthDiff > 0) {
                monthDiff--;
            } else {
                yearDiff--;
                monthDiff = 11;
            }
            dayDiff += daysInMonth[startDate.getMonth()];
        }

        // return yearDiff + 'Y ' + monthDiff + 'M ' + dayDiff + 'D';


        //if (yearDiff >= 18) {
        //    $('#errDOB').attr("hidden", true);

        //    //devika
        //    var dateString = $("#txtNomineeDOB").val();
        //    var dat = dateString.split("-");
        //    var d = dat[1] + "-" + dat[0] + "-" + dat[2];
        //    var myDate = new Date(d);
        //    var today = new Date();

        //    var age = Math.floor((today - myDate) / (365.25 * 24 * 60 * 60 * 1000));
        //    $("#txtNomineeAge").val(age);
        //    //
        //    return true;
        //}
        //else {
        //    $('#errDOB').removeAttr("hidden");
        //    $('#errDOB').text("Age cannot be less than 18.");
        //    return false;
        //}

        debugger;
        var dateString = $("#txtNomineeDOB").val();
        var dat = dateString.split("-");
        var d = dat[1] + "-" + dat[0] + "-" + dat[2];
        var myDate = new Date(d);
        var today = new Date();

        var age = Math.floor((today - myDate) / (365.25 * 24 * 60 * 60 * 1000));
        $("#txtNomineeAge").val(age);


        if ((ddlSelectedRelation == 7) && age > 18) {
            $('#errDOBAge').removeAttr("hidden");
            return false;
        }

    }
    else {
        $('#errDOB').removeAttr("hidden");
        return false;
    }
}

function EditValidateEmpDOB(id) {

    $('#errEditDOBAge').attr("hidden", true);
    $('#errEditDOB').attr("hidden", true);
    var endingDate = "";
    var txtDOB = $('#txtEditNomineeDOB').val();
    var EditddlSelectedRelation = $("#ddlNomineeList").val();
    var splitdate = txtDOB.split('-');
    var date = splitdate[0];
    var month = splitdate[1] - 1;
    var year = splitdate[2];
    if (txtDOB != "") {
        var startDate = new Date(new Date(year + "-" + splitdate[1] + "-" + date).toISOString().substr(0, 10));
        if (!endingDate) {
            endingDate = new Date().toISOString().substr(0, 10);    // need date in YYYY-MM-DD format
        }
        var endDate = new Date(endingDate);
        if (startDate > endDate) {
            var swap = startDate;
            startDate = endDate;
            endDate = swap;
        }
        var startYear = startDate.getFullYear();
        var february = (startYear % 4 === 0 && startYear % 100 !== 0) || startYear % 400 === 0 ? 29 : 28;
        var daysInMonth = [31, february, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        var yearDiff = endDate.getFullYear() - startYear;
        var monthDiff = endDate.getMonth() - startDate.getMonth();
        if (monthDiff < 0) {
            yearDiff--;
            monthDiff += 12;
        }
        var dayDiff = endDate.getDate() - startDate.getDate();
        if (dayDiff < 0) {
            if (monthDiff > 0) {
                monthDiff--;
            } else {
                yearDiff--;
                monthDiff = 11;
            }
            dayDiff += daysInMonth[startDate.getMonth()];
        }

        // return yearDiff + 'Y ' + monthDiff + 'M ' + dayDiff + 'D';

        //DEVIKA -- CODE TO CHECK  AGE RESTRICTION and calculate age
        //if (yearDiff >= 18) {
        //    $('#errEditDOB').attr("hidden", true);

        //    //devika
        //    var dateString = $("#txtEditNomineeDOB").val();
        //    var dat = dateString.split("-");
        //    var d = dat[1] + "-" + dat[0] + "-" + dat[2];
        //    var myDate = new Date(d);
        //    var today = new Date();

        //    var age = Math.floor((today - myDate) / (365.25 * 24 * 60 * 60 * 1000));
        //    $("#txtEditNomineeAge").val(age);
        //    //
        //    return true;
        //}
        //else {
        //    $('#errEditDOB').removeAttr("hidden");
        //    $('#errEditDOB').text("Age cannot be less than 18.");
        //    return false;
        //}

        var dateString = $("#txtEditNomineeDOB").val();
        var dat = dateString.split("-");
        var d = dat[1] + "-" + dat[0] + "-" + dat[2];
        var myDate = new Date(d);
        var today = new Date();

        var age = Math.floor((today - myDate) / (365.25 * 24 * 60 * 60 * 1000));
        $("#txtEditNomineeAge").val(age);

        if ((EditddlSelectedRelation == 7) && age > 18) {
            $('#errEditDOBAge').removeAttr("hidden");
            return false;
        }

    }
    else {
        $('#errEditDOB').removeAttr("hidden");
        return false;
    }
}

$('#ddlNomineeList').change(function (e) {


    if ($("#ddlNomineeList").val() == "16" || $("#ddlNomineeList").val() == "15") {

        $("#adoptionformuploadDiv").show();
        $("adoptionformuploadDiv").attr("style", "display:block")

    }
    else { //$("#adoptionformuploadDiv").hide(); 
        $("#adoptionformuploadDiv").hide();
        $("adoptionformuploadDiv").attr("style", "display:none")
    }


    $("#txtNomineeName").val("");
    $("#txtNomineeDOB").val("");
    $("#txtNomineeAge").val("");
    $("#txtNomineeShare").val("");
    $("#txtNomineeName").removeAttr("disabled");
    $("#txtNomineeDOB").attr("disabled", true);

    var today = new Date();

    // $('#txtNomineeName').prop("readonly", false);
    if ($("#errMember:visible").length > 0) {
        $("#errMember").attr("hidden", true);
    }

    var relation = $(this)[0].selectedOptions[0].innerHTML;

    // var isFatherAdded = $(".tdnRelation:contains('Father')").length;
    var isFatherAdded = $(".nRelation:contains('Father')").length;
    var isMotherAdded = $(".nRelation:contains('Mother')").length;
    var isSpouseAdded = $(".nRelation:contains('Spouse')").length;
    var isNoneAdded = $(".nRelation:contains('None')").length;


    var isMarried = $("#hdnIsMarried").val();
    var empdob = $("#txtBasicDateOfBirth").val();
    var dat;
    if (empdob.includes('/')) { dat = empdob.split("/"); }
    if (empdob.includes('-')) { dat = empdob.split("-") }
    if (empdob.includes('.')) { dat = empdob.split(".") }
    //var dat = empdob.split("-");

    var d = dat[1] + "-" + dat[0] + "-" + dat[2];
    var dob = new Date(d);
    /* $('#txtNomineeName').val("");*/
    if (relation == "None") {
        $("#txtNomineeName").attr("disabled", true);
        var isNoneAdded = $(".nRelation:contains('None')").length;
        if (isNoneAdded > 0) {
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
            return false;
        }
        if ($("tr[data-row-number]").length > 0) {
            $('#ddlRelation').val("");
            alertify.alert("Please remove the  member you have added previously.").setHeader("Warning!!!");
            return false;
        }
        $("#txtNomineeName").attr("disabled", true);
        $("#txtNomineeDOB").attr("disabled", true);
        $("#txtNomineeAge").attr("disabled", true);
        $("#txtNomineeShare").attr("disabled", true);

    }
    else {
        if (isNoneAdded > 0) {
            $('#ddlRelation').val("");
            alertify.alert("You cannot add other  members as you have added None in  details.").setHeader("Warning!!!");
            return false;
        }
        //$("#txtNomineeName").val("").attr("disabled", true);
        //$("#txtNomineeDOB").val("").attr("disabled", true);
        //$("#txtNomineeAge").val("").attr("disabled", true);
        //$("#txtNomineeShare").val("").attr("disabled", true);

    }
    if (relation === "-- Select --") { $("#txtNomineeName").attr("disabled", true); }
    if (relation === "Father") {

        if ($('#father_name').val() != "") {
            $('#txtNomineeName').val($('#father_name').val());
            /*  $("#father_name").attr("disabled", true);*/
            $("#txtNomineeName").prop("disabled", true);
            $("#txtNomineeDOB").removeAttr("disabled");
            $("#txtNomineeShare").removeAttr("disabled");
        }
        if (isFatherAdded > 0) {
            // $('#ddlRelation').val("");
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
        }
        else {
            var options = {
                timepicker: false,
                format: 'd-m-Y',
                maxDate: new Date((dob.getFullYear() - 10), 11, 31),
                minDate: new Date(today.getFullYear() - 100, 0, 1),
                defaultDate: new Date((dob.getFullYear() - 10), dob.getMonth(), dob.getDate()),
                yearStart: today.getFullYear() - 100,
                yearEnd: dob.getFullYear() - 10,
                scrollMonth: false,
                scrollInput: false,
                autoClose: true,
                closeOnDateSelect: true
            };

            $('#txtNomineeDOB').datetimepicker("destroy");
            $('#txtNomineeDOB').datetimepicker(options);
        }
    }
    if (relation === "Mother") {

        if (isMotherAdded > 0) {
            $('#ddlRelation').val("");
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
        }

        else {
            var options = {
                timepicker: false,
                format: 'd-m-Y',
                autoClose: true,
                maxDate: new Date((dob.getFullYear() - 10), 11, 31),
                minDate: new Date(today.getFullYear() - 100, 0, 1),
                defaultDate: new Date((dob.getFullYear() - 10), dob.getMonth(), dob.getDate()),
                yearStart: today.getFullYear() - 100,
                yearEnd: dob.getFullYear() - 10,
                scrollMonth: false,
                scrollInput: false,
                closeOnDateSelect: true
            };
            $('#txtNomineeDOB').datetimepicker("destroy");
            $('#txtNomineeDOB').datetimepicker(options);
        }
    }
    if (relation === "Spouse") {

        if ($('#txtSpouseName').val() != "") {
            if ($("#hdnDRStatus").val() == "2") {
                $('#txtNomineeName').val($('#txtCS').val());
                $('#txtNomineeName').prop("readonly", true);
            }
            else {
                $('#txtNomineeName').val($('#txtSpouseName').val());
                $('#txtNomineeName').prop("readonly", true);
            }
        }
        if (isSpouseAdded > 0) {
            //  $('#ddlRelation').val("");
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
        }


        else {
            if ($(".tdRelation:contains('Mother')").length > 0) {
                var dateOfBirthMother = $(".tdRelation:contains('Mother')").siblings(".tdDateOfBirth").text();
                /* var dateArray = dateOfBirthMother.split('-');*/
                var dateArray;

                if (dateOfBirthMother.includes('/')) { dateArray = dateOfBirthMother.split("/"); }
                if (dateOfBirthMother.includes('-')) { dateArray = dateOfBirthMother.split("-") }
                if (dateOfBirthMother.includes('.')) { dateArray = dateOfBirthMother.split(".") }
                var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
                var dateOfMotherBirth = new Date(date);

                var options = {
                    timepicker: false,
                    format: 'd-m-Y',
                    autoClose: true,
                    yearStart: today.getFullYear() - 100,
                    yearEnd: today.getFullYear() - 18,
                    defaultDate: new Date(dateOfMotherBirth.getFullYear(), dateOfMotherBirth.getMonth(), dateOfMotherBirth.getDate()),
                    maxDate: new Date((today.getFullYear() - 18), today.getMonth(), today.getDate()),
                    minDate: new Date(today.getFullYear() - 100, 0, 1),
                    scrollMonth: false,
                    scrollInput: false,
                    closeOnDateSelect: true
                };

                $('#txtNomineeDOB').datetimepicker("destroy");
                $('#txtNomineeDOB').datetimepicker(options);
            } else {
                var options = {
                    timepicker: false,
                    format: 'd-m-Y',
                    autoClose: true,
                    yearStart: today.getFullYear() - 100,
                    yearEnd: today.getFullYear() - 18,
                    defaultDate: new Date(today.getFullYear() - 18, today.getMonth(), today.getDate()),
                    maxDate: new Date((today.getFullYear() - 18), today.getMonth(), today.getDate()),
                    minDate: new Date(today.getFullYear() - 100, 0, 1),
                    scrollMonth: false,
                    scrollInput: false,
                    closeOnDateSelect: true
                };
                $('#txtNomineeDOB').datetimepicker("destroy");
                $('#txtNomineeDOB').datetimepicker(options);
            }
        }
    }
    if (relation === "Brother" || relation === "Sister") {



        /* if ($(".tdRelation:contains('Mother')").length > 0) {*/
        //var dateOfBirthMother = $(".tdRelation:contains('Mother')").siblings(".tdDateOfBirth").text();
        //var dateArray = dateOfBirthMother.split('-');
        //var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
        //var dateOfMotherBirth = new Date(date);
        //var dateOfBirthFather = $(".tdRelation:contains('Father')").siblings(".tdDateOfBirth").text();
        //var dateArray = dateOfBirthFather.split('-');
        //var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
        //var dateOfFatherBirth = new Date(date);
        //var startDate = '';
        //if (dateOfMotherBirth > dateOfFatherBirth)
        //    startDate = dateOfMotherBirth;
        //else
        //    startDate = dateOfFatherBirth;

        var options = {
            //    timepicker: false,
            //    format: 'd-m-Y',
            //    autoClose: true,
            //    yearStart: today.getFullYear() + 10,
            //    yearEnd: today.getFullYear(),
            //    minDate: new Date(today.getFullYear() + 10, 0, 1),
            //    maxDate: today,
            //    defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
            //    scrollMonth: false,
            //    scrollInput: false,
            //    closeOnDateSelect: true
            timepicker: false,
            format: 'd-m-Y',
            maxDate: 0,
            minDate: new Date(today.getFullYear() - 60, today.getMonth(), today.getDate()),
            yearStart: today.getFullYear() - 60,
            yearEnd: today.getFullYear(),
            scrollMonth: false,
            scrollInput: false,
            autoclose: true,
            closeOnDateSelect: true
        };

        $('#txtNomineeDOB').datetimepicker("destroy");
        $('#txtNomineeDOB').datetimepicker(options);
        //}

    }
    if (relation === "Daughter" || relation === "Son") {

        var divStatus = $("#hdnDRStatus").val();

        if ($(".tdRelation:contains('Spouse')").length > 0) {
            var dateOfBirthSpouse = $(".tdRelation:contains('Spouse')").siblings(".tdDateOfBirth").text();

            //var dateArray = dateOfBirthSpouse.split('-');
            var dateArray;
            if (dateOfBirthSpouse.includes('/')) { dateArray = dateOfBirthSpouse.split("/"); }
            if (dateOfBirthSpouse.includes('-')) { dateArray = dateOfBirthSpouse.split("-") }
            if (dateOfBirthSpouse.includes('.')) { dateArray = dateOfBirthSpouse.split(".") }


            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
            var dateOfSpouseBirth = new Date(date);
            var dateOfBirth = $("#txtBasicDateOfBirth").val();
            /*   var dateArray = dateOfBirth.split('-');*/
            var dateArray;
            if (dateArray.includes('/')) { dateArray = dateOfBirth.split("/"); }
            if (dateArray.includes('-')) { dateArray = dateOfBirth.split("-") }
            if (dateArray.includes('.')) { dateArray = dateOfBirth.split(".") }

            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
            var dateOfBirth = new Date(date);
            var startdate = '';
            if (dateOfBirth > dateOfSpouseBirth)
                startdate = dateOfBirth;
            else
                startdate = dateOfSpouseBirth;

            var options = {
                timepicker: false,
                format: 'd-m-Y',
                autoClose: true,
                yearStart: startdate.getFullYear() + 10,
                yearEnd: today.getFullYear(),
                minDate: new Date(startdate.getFullYear() + 10, 0, 1),
                maxDate: today,
                defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
                scrollMonth: false,
                scrollInput: false,
                closeOnDateSelect: true
            };

            $('#txtNomineeDOB').datetimepicker("destroy");
            $('#txtNomineeDOB').datetimepicker(options);
        }
        else {
            var dateOfBirth = $("#txtBasicDateOfBirth").val();
            var dateArray;

            if (dateOfBirth.includes('/')) { dateArray = dateOfBirth.split("/"); }
            if (dateOfBirth.includes('-')) { dateArray = dateOfBirth.split("-") }
            if (dateOfBirth.includes('.')) { dateArray = dateOfBirth.split(".") }



            /* var dateArray = dateOfBirth.split('-');*/
            //var dateArray = dateOfBirth.split('/');
            //var dateArray = dateOfBirth.split('.');
            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
            var dateOfBirth = new Date(date);
            var options = {
                timepicker: false,
                format: 'd-m-Y',
                autoClose: true,
                yearStart: dateOfBirth.getFullYear() + 10,
                yearEnd: today.getFullYear(),
                minDate: new Date(dateOfBirth.getFullYear() + 10, 0, 1),
                maxDate: today,
                defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
                scrollMonth: false,
                scrollInput: false,
                closeOnDateSelect: true
            };

            $('#txtNomineeDOB').datetimepicker("destroy");
            $('#txtNomineeDOB').datetimepicker(options);
        }

    }

});

//////$('#ddlEditNomineeList').change(function (e) {

//    if ($("#ddlEditNomineeList").val() == "15" || $("#ddlEditNomineeList").val() == "16") {

//        $("#EditadoptionformuploadDiv").show();
//        $("EditadoptionformuploadDiv").attr("style", "display:block")

//    }
//    else { //$("#adoptionformuploadDiv").hide(); 
//        $("#EditadoptionformuploadDiv").hide();
//        $("EditadoptionformuploadDiv").attr("style", "display:none")
//    }



//    $("#txtEditNomineeName").val("");
//    $("#txtEditNomineeDOB").val("");
//    $("#txtEditNomineeAge").val("");
//    $("#txtEditNomineeShare").val("");
//    $("#txtEditNomineeName").removeAttr("true");
//    $("#txtEditNomineeDOB").attr("disabled", true);

//    var today = new Date();

//    // $('#txtNomineeName').prop("readonly", false);
//    if ($("#errMember:visible").length > 0) {
//        $("#errMember").attr("hidden", true);
//    }

//    var relation = $(this)[0].selectedOptions[0].innerHTML;

//    // var isFatherAdded = $(".tdnRelation:contains('Father')").length;
//    var isFatherAdded = $(".nRelation:contains('Father')").length;
//    var isMotherAdded = $(".nRelation:contains('Mother')").length;
//    var isSpouseAdded = $(".nRelation:contains('Spouse')").length;
//    var isNoneAdded = $(".nRelation:contains('None')").length;


//    var isMarried = $("#hdnIsMarried").val();
//    var empdob = $("#txtBasicDateOfBirth").val();
//    var dat = empdob.split("-");
//    var d = dat[1] + "-" + dat[0] + "-" + dat[2];
//    var dob = new Date(d);

//    /* $('#txtNomineeName').val("");*/
//    if (relation == "None")
//    {
//        $("#txtEditNomineeName").attr("disabled", true);
//        $("#txtEditNomineeDOB").attr("disabled", true);
//        $("#txtEditNomineeAge").attr("disabled", true);
//        $("#txtEditNomineeShare").attr("disabled", true);
//        $("#txtEditNomineeName").attr("disabled", true);
//        var isNoneAdded = $(".nRelation:contains('None')").length;
//        if (isNoneAdded > 0) {
//            alertify.alert("Member Already Added.").setHeader("Warning!!!");           
//            return false;
//        }
//        if ($("tr[data-row-number]").length > 0) {
//            $('#ddlRelation').val("");

//            alertify.alert("Please remove the family member you have added previously.").setHeader("Warning!!!");
//            return false;
//        }


//    }
//    else {
//        if (isNoneAdded > 0) {
//            $('#ddlRelation').val("");
//            $("#txtEditNomineeName").attr("disabled", true);
//            $("#txtEditNomineeDOB").attr("disabled", true);
//            $("#txtEditNomineeAge").attr("disabled", true);
//            $("#txtEditNomineeShare").attr("disabled", true);
//            alertify.alert("You cannot add other family members as you have added None in family details.").setHeader("Warning!!!");
//            return false;
//        }
//        //$("#txtNomineeName").val("").attr("disabled", true);
//        //$("#txtNomineeDOB").val("").attr("disabled", true);
//        //$("#txtNomineeAge").val("").attr("disabled", true);
//        //$("#txtNomineeShare").val("").attr("disabled", true);

//    }
//    if (relation === "-- Select --") { $("#txtEditNomineeName").attr("disabled", true); }
//    if (relation === "Father") {

//        if ($('#father_name').val() != "") {
//            $('#txtEditNomineeName').val($('#father_name').val());
//            $("#txtEditNomineeName").prop("disabled", true);
//            $("#txtEditNomineeDOB").removeAttr("disabled");
//            $("#txtEditNomineeShare").removeAttr("disabled");
//        }
//        if (isFatherAdded > 0) {
//            // $('#ddlRelation').val("");
//            alertify.alert("Member Already Added.").setHeader("Warning!!!");
//        }
//        else {
//            var options = {
//                timepicker: false,
//                format: 'd-m-Y',
//                maxDate: new Date((dob.getFullYear() - 10), 11, 31),
//                minDate: new Date(today.getFullYear() - 100, 0, 1),
//                defaultDate: new Date((dob.getFullYear() - 10), dob.getMonth(), dob.getDate()),
//                yearStart: today.getFullYear() - 100,
//                yearEnd: dob.getFullYear() - 10,
//                scrollMonth: false,
//                scrollInput: false,
//                autoClose: true,
//                closeOnDateSelect: true
//            };

//            $('#txtEditNomineeDOB').datetimepicker("destroy");
//            $('#txtEditNomineeDOB').datetimepicker(options);
//        }
//    }
//    if (relation === "Mother") {

//        if (isMotherAdded > 0) {
//            $('#ddlRelation').val("");
//            alertify.alert("Member Already Added.").setHeader("Warning!!!");
//        }

//        else {
//            var options = {
//                timepicker: false,
//                format: 'd-m-Y',
//                autoClose: true,
//                maxDate: new Date((dob.getFullYear() - 10), 11, 31),
//                minDate: new Date(today.getFullYear() - 100, 0, 1),
//                defaultDate: new Date((dob.getFullYear() - 10), dob.getMonth(), dob.getDate()),
//                yearStart: today.getFullYear() - 100,
//                yearEnd: dob.getFullYear() - 10,
//                scrollMonth: false,
//                scrollInput: false,
//                closeOnDateSelect: true
//            };
//            $('#txtEditNomineeDOB').datetimepicker("destroy");
//            $('#txtEditNomineeDOB').datetimepicker(options);
//        }
//    }
//    if (relation === "Spouse") {

//        if ($('#txtSpouseName').val() != "") {
//            if ($("#hdnDRStatus").val() == "2") {
//                $('#txtEditNomineeName').val($('#txtCS').val());
//                $('#txtEditNomineeName').prop("readonly", true);
//            }
//            else {
//                $('#txtEditNomineeName').val($('#txtSpouseName').val());
//                $('#txtEditNomineeName').prop("readonly", true);
//            }
//        }
//        if (isSpouseAdded > 0) {
//            //  $('#ddlRelation').val("");
//            alertify.alert("Member Already Added.").setHeader("Warning!!!");
//        }


//        else {
//            if ($(".tdRelation:contains('Mother')").length > 0) {
//                var dateOfBirthMother = $(".tdRelation:contains('Mother')").siblings(".tdDateOfBirth").text();
//                var dateArray = dateOfBirthMother.split('-');
//                var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
//                var dateOfMotherBirth = new Date(date);

//                var options = {
//                    timepicker: false,
//                    format: 'd-m-Y',
//                    autoClose: true,
//                    yearStart: today.getFullYear() - 100,
//                    yearEnd: today.getFullYear() - 18,
//                    defaultDate: new Date(dateOfMotherBirth.getFullYear(), dateOfMotherBirth.getMonth(), dateOfMotherBirth.getDate()),
//                    maxDate: new Date((today.getFullYear() - 18), today.getMonth(), today.getDate()),
//                    minDate: new Date(today.getFullYear() - 100, 0, 1),
//                    scrollMonth: false,
//                    scrollInput: false,
//                    closeOnDateSelect: true
//                };

//                $('#txtEditNomineeDOB').datetimepicker("destroy");
//                $('#txtEditNomineeDOB').datetimepicker(options);
//            } else {
//                var options = {
//                    timepicker: false,
//                    format: 'd-m-Y',
//                    autoClose: true,
//                    yearStart: today.getFullYear() - 100,
//                    yearEnd: today.getFullYear() - 18,
//                    defaultDate: new Date(today.getFullYear() - 18, today.getMonth(), today.getDate()),
//                    maxDate: new Date((today.getFullYear() - 18), today.getMonth(), today.getDate()),
//                    minDate: new Date(today.getFullYear() - 100, 0, 1),
//                    scrollMonth: false,
//                    scrollInput: false,
//                    closeOnDateSelect: true
//                };
//                $('#txtEditNomineeDOB').datetimepicker("destroy");
//                $('#txtEditNomineeDOB').datetimepicker(options);
//            }
//        }
//    }
//    if (relation === "Brother" || relation === "Sister") {



//        /* if ($(".tdRelation:contains('Mother')").length > 0) {*/
//        //var dateOfBirthMother = $(".tdRelation:contains('Mother')").siblings(".tdDateOfBirth").text();
//        //var dateArray = dateOfBirthMother.split('-');
//        //var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
//        //var dateOfMotherBirth = new Date(date);
//        //var dateOfBirthFather = $(".tdRelation:contains('Father')").siblings(".tdDateOfBirth").text();
//        //var dateArray = dateOfBirthFather.split('-');
//        //var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
//        //var dateOfFatherBirth = new Date(date);
//        //var startDate = '';
//        //if (dateOfMotherBirth > dateOfFatherBirth)
//        //    startDate = dateOfMotherBirth;
//        //else
//        //    startDate = dateOfFatherBirth;

//        var options = {
//            //    timepicker: false,
//            //    format: 'd-m-Y',
//            //    autoClose: true,
//            //    yearStart: today.getFullYear() + 10,
//            //    yearEnd: today.getFullYear(),
//            //    minDate: new Date(today.getFullYear() + 10, 0, 1),
//            //    maxDate: today,
//            //    defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
//            //    scrollMonth: false,
//            //    scrollInput: false,
//            //    closeOnDateSelect: true
//            timepicker: false,
//            format: 'd-m-Y',
//            maxDate: 0,
//            minDate: new Date(today.getFullYear() - 60, today.getMonth(), today.getDate()),
//            yearStart: today.getFullYear() - 60,
//            yearEnd: today.getFullYear(),
//            scrollMonth: false,
//            scrollInput: false,
//            autoclose: true,
//            closeOnDateSelect: true
//        };

//        $('#txtEditNomineeDOB').datetimepicker("destroy");
//        $('#txtEditNomineeDOB').datetimepicker(options);
//        //}

//    }
//    if (relation === "Daughter" || relation === "Son") {

//        var divStatus = $("#hdnDRStatus").val();

//        if ($(".tdRelation:contains('Spouse')").length > 0) {
//            var dateOfBirthSpouse = $(".tdRelation:contains('Spouse')").siblings(".tdDateOfBirth").text();
//            var dateArray = dateOfBirthSpouse.split('-');
//            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
//            var dateOfSpouseBirth = new Date(date);
//            var dateOfBirth = $("#txtBasicDateOfBirth").val();
//            var dateArray = dateOfBirth.split('-');
//            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
//            var dateOfBirth = new Date(date);
//            var startdate = '';
//            if (dateOfBirth > dateOfSpouseBirth)
//                startdate = dateOfBirth;
//            else
//                startdate = dateOfSpouseBirth;

//            var options = {
//                timepicker: false,
//                format: 'd-m-Y',
//                autoClose: true,
//                yearStart: startdate.getFullYear() + 10,
//                yearEnd: today.getFullYear(),
//                minDate: new Date(startdate.getFullYear() + 10, 0, 1),
//                maxDate: today,
//                defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
//                scrollMonth: false,
//                scrollInput: false,
//                closeOnDateSelect: true
//            };

//            $('#txtEditNomineeDOB').datetimepicker("destroy");
//            $('#txtEditNomineeDOB').datetimepicker(options);
//        }
//        else {
//            var dateOfBirth = $("#txtBasicDateOfBirth").val();
//            var dateArray = dateOfBirth.split('-');
//            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
//            var dateOfBirth = new Date(date);
//            var options = {
//                timepicker: false,
//                format: 'd-m-Y',
//                autoClose: true,
//                yearStart: dateOfBirth.getFullYear() + 10,
//                yearEnd: today.getFullYear(),
//                minDate: new Date(dateOfBirth.getFullYear() + 10, 0, 1),
//                maxDate: today,
//                defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
//                scrollMonth: false,
//                scrollInput: false,
//                closeOnDateSelect: true
//            };

//            $('#txtEditNomineeDOB').datetimepicker("destroy");
//            $('#txtEditNomineeDOB').datetimepicker(options);
//        }

//    }

//});

// win merge edited




$('#ddlEditNomineeList').change(function (e) {

    if ($("#ddlEditNomineeList").val() == "15" || $("#ddlEditNomineeList").val() == "16") {

        $("#EditadoptionformuploadDiv").show();
        $("EditadoptionformuploadDiv").attr("style", "display:block")

    }
    else { //$("#adoptionformuploadDiv").hide(); 
        $("#EditadoptionformuploadDiv").hide();
        $("EditadoptionformuploadDiv").attr("style", "display:none")
    }



    $("#txtEditNomineeName").val("");
    $("#txtEditNomineeDOB").val("");
    $("#txtEditNomineeAge").val("");
    $("#txtEditNomineeShare").val("");
    $("#txtEditNomineeName").removeAttr("disabled");
    $("#txtEditNomineeDOB").attr("disabled", true);

    var today = new Date();

    // $('#txtNomineeName').prop("readonly", false);
    if ($("#errMember:visible").length > 0) {
        $("#errMember").attr("hidden", true);
    }

    var relation = $(this)[0].selectedOptions[0].innerHTML;

    // var isFatherAdded = $(".tdnRelation:contains('Father')").length;
    var isFatherAdded = $(".nRelation:contains('Father')").length;
    var isMotherAdded = $(".nRelation:contains('Mother')").length;
    var isSpouseAdded = $(".nRelation:contains('Spouse')").length;
    var isNoneAdded = $(".nRelation:contains('None')").length;


    var isMarried = $("#hdnIsMarried").val();
    var empdob = $("#txtBasicDateOfBirth").val();
    var dat;
    /*var dat = empdob.split("-");*/
    if (empdob.includes('/')) { dat = empdob.split("/"); }
    if (empdob.includes('-')) { dat = empdob.split("-") }
    if (empdob.includes('.')) { dat = empdob.split(".") }

    var d = dat[1] + "-" + dat[0] + "-" + dat[2];
    var dob = new Date(d);

    /* $('#txtNomineeName').val("");*/
    if (relation == "None") {
        // $("#txtEditNomineeName").attr("disabled", true);
        // $("#txtEditNomineeDOB").attr("disabled", true);
        // $("#txtEditNomineeAge").attr("disabled", true);
        //  $("#txtEditNomineeShare").attr("disabled", true);
        //  $("#txtEditNomineeName").attr("disabled", true);
        var isNoneAdded = $(".nRelation:contains('None')").length;
        if (isNoneAdded > 0) {
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
            return false;
        }
        if ($("tr[data-row-number]").length > 0) {
            $('#ddlRelation').val("");

            alertify.alert("Please remove the family member you have added previously.").setHeader("Warning!!!");
            return false;
        }
        $("#txtEditNomineeName").attr("disabled", true);
        $("#txtEditNomineeDOB").attr("disabled", true);
        $("#txtEditNomineeAge").attr("disabled", true);
        $("#txtEditNomineeShare").attr("disabled", true);

    }
    else {
        if (isNoneAdded > 0) {
            $('#ddlRelation').val("");
            //$("#txtEditNomineeName").attr("disabled", true);
            // $("#txtEditNomineeDOB").attr("disabled", true);
            // $("#txtEditNomineeAge").attr("disabled", true);
            // $("#txtEditNomineeShare").attr("disabled", true);
            alertify.alert("You cannot add other family members as you have added None in family details.").setHeader("Warning!!!");
            return false;
        }
        //$("#txtNomineeName").val("").attr("disabled", true);
        //$("#txtNomineeDOB").val("").attr("disabled", true);
        //$("#txtNomineeAge").val("").attr("disabled", true);
        //$("#txtNomineeShare").val("").attr("disabled", true);

    }
    if (relation === "-- Select --") { $("#txtEditNomineeName").attr("disabled", true); }
    if (relation === "Father") {
        $("#txtEditNomineeName").prop("disabled", true);
        if ($('#father_name').val() != "") {
            $('#txtEditNomineeName').val($('#father_name').val());
            $("#txtEditNomineeName").prop("disabled", true);
            $("#txtEditNomineeDOB").removeAttr("disabled");
            $("#txtEditNomineeShare").removeAttr("disabled");
        }
        if (isFatherAdded > 0) {
            // $('#ddlRelation').val("");
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
        }
        else {
            var options = {
                timepicker: false,
                format: 'd-m-Y',
                maxDate: new Date((dob.getFullYear() - 10), 11, 31),
                minDate: new Date(today.getFullYear() - 100, 0, 1),
                defaultDate: new Date((dob.getFullYear() - 10), dob.getMonth(), dob.getDate()),
                yearStart: today.getFullYear() - 100,
                yearEnd: dob.getFullYear() - 10,
                scrollMonth: false,
                scrollInput: false,
                autoClose: true,
                closeOnDateSelect: true
            };

            $('#txtEditNomineeDOB').datetimepicker("destroy");
            $('#txtEditNomineeDOB').datetimepicker(options);
        }
    }
    if (relation === "Mother") {

        if (isMotherAdded > 0) {
            $('#ddlRelation').val("");
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
        }

        else {
            var options = {
                timepicker: false,
                format: 'd-m-Y',
                autoClose: true,
                maxDate: new Date((dob.getFullYear() - 10), 11, 31),
                minDate: new Date(today.getFullYear() - 100, 0, 1),
                defaultDate: new Date((dob.getFullYear() - 10), dob.getMonth(), dob.getDate()),
                yearStart: today.getFullYear() - 100,
                yearEnd: dob.getFullYear() - 10,
                scrollMonth: false,
                scrollInput: false,
                closeOnDateSelect: true
            };
            $('#txtEditNomineeDOB').datetimepicker("destroy");
            $('#txtEditNomineeDOB').datetimepicker(options);
        }
    }
    if (relation === "Spouse") {

        if ($('#txtSpouseName').val() != "") {
            if ($("#hdnDRStatus").val() == "2") {
                $('#txtEditNomineeName').val($('#txtCS').val());
                $('#txtEditNomineeName').prop("readonly", true);
            }
            else {
                $('#txtEditNomineeName').val($('#txtSpouseName').val());
                $('#txtEditNomineeName').prop("readonly", true);
            }
        }
        if (isSpouseAdded > 0) {
            //  $('#ddlRelation').val("");
            alertify.alert("Member Already Added.").setHeader("Warning!!!");
        }


        else {
            if ($(".tdRelation:contains('Mother')").length > 0) {
                var dateOfBirthMother = $(".tdRelation:contains('Mother')").siblings(".tdDateOfBirth").text();
                /*  var dateArray = dateOfBirthMother.split('-');*/
                var dateArray;
                if (dateOfBirthMother.includes('/')) { dateArray = dateOfBirthMother.split("/"); }
                if (dateOfBirthMother.includes('-')) { dateArray = dateOfBirthMother.split("-") }
                if (dateOfBirthMother.includes('.')) { dateArray = dateOfBirthMother.split(".") }
                var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
                var dateOfMotherBirth = new Date(date);

                var options = {
                    timepicker: false,
                    format: 'd-m-Y',
                    autoClose: true,
                    yearStart: today.getFullYear() - 100,
                    yearEnd: today.getFullYear() - 18,
                    defaultDate: new Date(dateOfMotherBirth.getFullYear(), dateOfMotherBirth.getMonth(), dateOfMotherBirth.getDate()),
                    maxDate: new Date((today.getFullYear() - 18), today.getMonth(), today.getDate()),
                    minDate: new Date(today.getFullYear() - 100, 0, 1),
                    scrollMonth: false,
                    scrollInput: false,
                    closeOnDateSelect: true
                };

                $('#txtEditNomineeDOB').datetimepicker("destroy");
                $('#txtEditNomineeDOB').datetimepicker(options);
            } else {
                var options = {
                    timepicker: false,
                    format: 'd-m-Y',
                    autoClose: true,
                    yearStart: today.getFullYear() - 100,
                    yearEnd: today.getFullYear() - 18,
                    defaultDate: new Date(today.getFullYear() - 18, today.getMonth(), today.getDate()),
                    maxDate: new Date((today.getFullYear() - 18), today.getMonth(), today.getDate()),
                    minDate: new Date(today.getFullYear() - 100, 0, 1),
                    scrollMonth: false,
                    scrollInput: false,
                    closeOnDateSelect: true
                };
                $('#txtEditNomineeDOB').datetimepicker("destroy");
                $('#txtEditNomineeDOB').datetimepicker(options);
            }
        }
    }
    if (relation === "Brother" || relation === "Sister") {



        /* if ($(".tdRelation:contains('Mother')").length > 0) {*/
        //var dateOfBirthMother = $(".tdRelation:contains('Mother')").siblings(".tdDateOfBirth").text();
        //var dateArray = dateOfBirthMother.split('-');
        //var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
        //var dateOfMotherBirth = new Date(date);
        //var dateOfBirthFather = $(".tdRelation:contains('Father')").siblings(".tdDateOfBirth").text();
        //var dateArray = dateOfBirthFather.split('-');
        //var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
        //var dateOfFatherBirth = new Date(date);
        //var startDate = '';
        //if (dateOfMotherBirth > dateOfFatherBirth)
        //    startDate = dateOfMotherBirth;
        //else
        //    startDate = dateOfFatherBirth;

        var options = {
            //    timepicker: false,
            //    format: 'd-m-Y',
            //    autoClose: true,
            //    yearStart: today.getFullYear() + 10,
            //    yearEnd: today.getFullYear(),
            //    minDate: new Date(today.getFullYear() + 10, 0, 1),
            //    maxDate: today,
            //    defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
            //    scrollMonth: false,
            //    scrollInput: false,
            //    closeOnDateSelect: true
            timepicker: false,
            format: 'd-m-Y',
            maxDate: 0,
            minDate: new Date(today.getFullYear() - 60, today.getMonth(), today.getDate()),
            yearStart: today.getFullYear() - 60,
            yearEnd: today.getFullYear(),
            scrollMonth: false,
            scrollInput: false,
            autoclose: true,
            closeOnDateSelect: true
        };

        $('#txtEditNomineeDOB').datetimepicker("destroy");
        $('#txtEditNomineeDOB').datetimepicker(options);
        //}

    }
    if (relation === "Daughter" || relation === "Son") {

        var divStatus = $("#hdnDRStatus").val();

        if ($(".tdRelation:contains('Spouse')").length > 0) {
            var dateOfBirthSpouse = $(".tdRelation:contains('Spouse')").siblings(".tdDateOfBirth").text();
            /* var dateArray = dateOfBirthSpouse.split('-');*/
            var dateArray;
            if (dateOfBirthSpouse.includes('/')) { dateArray = dateOfBirthSpouse.split("/"); }
            if (dateOfBirthSpouse.includes('-')) { dateArray = dateOfBirthSpouse.split("-") }
            if (dateOfBirthSpouse.includes('.')) { dateArray = dateOfBirthSpouse.split(".") }


            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
            var dateOfSpouseBirth = new Date(date);
            /*  var dateOfBirth = $("#txtBasicDateOfBirth").val();*/
            var dateOfBirth;
            /*   var dateArray = dateOfBirth.split('-');*/
            if (dateArray.includes('/')) { dateArray = dateOfBirth.split("/"); }
            if (dateArray.includes('-')) { dateArray = dateOfBirth.split("-") }
            if (dateArray.includes('.')) { dateArray = dateOfBirth.split(".") }
            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
            var dateOfBirth = new Date(date);
            var startdate = '';
            if (dateOfBirth > dateOfSpouseBirth)
                startdate = dateOfBirth;
            else
                startdate = dateOfSpouseBirth;

            var options = {
                timepicker: false,
                format: 'd-m-Y',
                autoClose: true,
                yearStart: startdate.getFullYear() + 10,
                yearEnd: today.getFullYear(),
                minDate: new Date(startdate.getFullYear() + 10, 0, 1),
                maxDate: today,
                defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
                scrollMonth: false,
                scrollInput: false,
                closeOnDateSelect: true
            };

            $('#txtEditNomineeDOB').datetimepicker("destroy");
            $('#txtEditNomineeDOB').datetimepicker(options);
        }
        else {
            var dateOfBirth = $("#txtBasicDateOfBirth").val();
            /* var dateArray = dateOfBirth.split('-');*/

            var dateOfBirth;
            if (dateOfBirth.includes('/')) { dateArray = dateOfBirth.split("/"); }
            if (dateOfBirth.includes('-')) { dateArray = dateOfBirth.split("-") }
            if (dateOfBirth.includes('.')) { dateArray = dateOfBirth.split(".") }

            var date = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
            var dateOfBirth = new Date(date);
            var options = {
                timepicker: false,
                format: 'd-m-Y',
                autoClose: true,
                yearStart: dateOfBirth.getFullYear() + 10,
                yearEnd: today.getFullYear(),
                minDate: new Date(dateOfBirth.getFullYear() + 10, 0, 1),
                maxDate: today,
                defaultDate: new Date(today.getFullYear(), today.getMonth(), today.getDate()),
                scrollMonth: false,
                scrollInput: false,
                closeOnDateSelect: true
            };

            $('#txtEditNomineeDOB').datetimepicker("destroy");
            $('#txtEditNomineeDOB').datetimepicker(options);
        }

    }

});





//




function validatesumofshare() {

    var result = false;
    var eachSumShare = 0;
    $('.val-share1').each(function () {
        eachSumShare = eachSumShare + parseInt($(this).text());
    });
    if (eachSumShare == 100) { return true; }
}

//$("#btnNomineeNext").click(function (e) {
//    //var valresult;
//    //if ($("#ddlNomineeList").val() == 14) { result = true }
//    //else { result = validatesumofshare(); }
//    //debugger;
//    //var result = EmployeeSave();

//  //  var result = validatesumofshare();
//    if ($(".tdRelation:contains('None')")) {
//        result = true
//    }
//    else { result = validatesumofshare(); }
//    if (result == true) {
//        $("#nav-Application").removeClass("show active");
//        $("#nav-Nominee").removeClass("show active");
//        $("#uploadeddocuments").removeClass("show active");
//        $("#nav-Payment").addClass("show active");
//        //   $("#uploadeddocuments").removeClass("show active");


//        $("#nav-Application-tab").removeClass("active");
//        $("#nav-Nominee-tab").removeClass("active");
//        $("#nav-Payment-tab").addClass("active");

//        // $("#nav-tab-Documents").removeClass("active"); 
//        // alertify.success("Nominee details saved successfully");
//    }
//    else { alertify.alert('Percentage share for nominees should be 100 %.').setHeader("Warning!!!"); }

//    //LoadUploadedDocuments();
//});


$("#btnNomineeSave").click(function (e) {
    var valresult;
    //if ($("#ddlNomineeList").val() == 14) { result = true }
    //else { result = validatesumofshare(); }
    //debugger;
    //var result = EmployeeSave();

    //   var result = validatesumofshare();

    //  var rowCount = $("#tblNomineeDetails1 tbody tr").length;  //$("tr[data-row-number]").length > 0
    var rowCount = $("#tblNomineeDetails1 tbody tr[data-row-number]").length
    if (rowCount > 0) {
        result = validatesumofshare();
        if (result == true) {

            //$("#nav-Application").removeClass("show active");
            //$("#nav-Nominee").addClass("show active");
            //$("#nav-Declaration").removeClass("show active");
            //$("#nav-Payment").removeClass("show active");
            //$("#nav-Upload").removeClass("show active");



            //$("#nav-Application-tab").removeClass("active");
            //$("#nav-Nominee-tab").addClass("active");
            //$("#nav-Declaration-tab").removeClass("active");
            //$("#nav-Payment-tab").removeClass("active");
            //$("#nav-Upload-tab").removeClass("active");


            alertify.success('Nominee details added successfully');

        }
        else {
            alertify.alert('Percentage share for nominees should be 100 %.').setHeader("Warning!!!");
        }
    }
    else { alertify.alert('Please enter nominee details.').setHeader("Warning!!!"); }


    ////if ($(".tdRelation:contains('None')") == true) {

    ////   result = true
    ////}
    //// else { result = validatesumofshare();  }
    //if (result == true) {

    //    //$("#nav-Application").removeClass("show active");
    //    //$("#nav-Nominee").removeClass("show active");
    //    //$("#uploadeddocuments").removeClass("show active");
    //    //$("#nav-Payment").addClass("show active");
    //    ////   $("#uploadeddocuments").removeClass("show active");


    //    //$("#nav-Application-tab").removeClass("active");
    //    //$("#nav-Nominee-tab").removeClass("active");
    //    //$("#nav-Payment-tab").addClass("active");

    //    $("#nav-Application").removeClass("show active");
    //    $("#nav-Nominee").addClass("show active");
    //    $("#nav-Payment").removeClass("show active");
    //    $("#nav-Declaration").removeClass("show active");
    //    $("#nav-Upload").removeClass("show active");



    //    $("#nav-Application-tab").removeClass("active");
    //    $("#nav-Nominee-tab").addClass("active");
    //    $("#nav-Payment-tab").removeClass("active");
    //    $("#nav-Declaration-tab").removeClass("active");
    //    $("#nav-Upload-tab").removeClass("active");
    //    alertify.success("Nominee details saved successfully");
    //}
    //else {

    //    alertify.alert('Percentage share for nominees should be 100 %.').setHeader("Warning!!!");
    //}

});

function GetNomineeDetails() {


    //$("#tblNomineeDetails1").children().remove();
    $.ajax({

        url: '/GroupInsuranceClaim/GISClaimsNomineeDetails',
        /* url: '/GISNomineeChangeReq/GIS_NCR_NomineeDetails',*/
        data: JSON.stringify({}),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            debugger;
            $("#NomineeTab").html(data);
            //$("#divNRNominee").append(data);
            //$("#tblNomineeDetails1 tbody").html(data);
            //$("#tblNomineeDetails1").append(data);
            //$("#tblNomineeDetails1 > tbody").append(data);
            EnglishToKannada();
        }
    });

}

function addNomineeDetails() {
    // CheckDuplicateName();
    alert("devika");
    if ($("#ddlNomineeList").val() == 14) {
        // var valresult = validateNomineeFields();
        var valresult = validateNone();
        alert(valresult + "  in add function")
        if (valresult == true) {
            var formData = new FormData();
            var file = document.getElementById("txtAdoptionformDoc").files[0];
            formData.append("EmpId", $("#hdnPReferanceNo").val());
            formData.append("NameOfNominee", $("#txtNomineeName").val());
            formData.append("Relation", $("#ddlNomineeList").val());
            formData.append("PercentageShare", $("#txtNomineeShare").val());
            formData.append("Age", $("#txtNomineeAge").val());
            formData.append("nomineeDOB", $("#txtNomineeDOB").val());
            formData.append("SonDaughterAdoption_doc", $("#txtAdoptionformDoc").val());
            formData.append("SonDaughterAdoption_doc", file);
            formData.append("SonDaughterAdoption_doc_path", file);

            formData.append("gnd_contingencies", $("#txtNomineecontingencies").val());
            formData.append("gnd_predeceasing", $("#txtNomineepredeceasing").val());

            formData.append("gnd_bank_account_number", $("#txtBnkAcNo").val());
            formData.append("gnd_ifsc", $("#txtifscCode").val());
            formData.append("gnd_micr", $("#txtMicr").val());
          

            if (valresult == true) {
                //var viewModelNominee = {
                //    'EmpId': $("#hdnPReferanceNo").val(), 
                //    'NameOfNominee': $("#txtNomineeName").val(),
                //    'Relation': $("#ddlNomineeList").val(),
                //    'PercentageShare': $("#txtNomineeShare").val(),
                //    'Age': $("#txtNomineeAge").val(),// guardianAge
                //    'nomineeDOB': $("#txtNomineeDOB").val(),
                //    'SonDaughterAdoption_doc': $("#txtAdoptionformDoc").val(),
                //    //'SonDaughterAdoption_doc': fileData,
                //    'SonDaughterAdoption_doc_path': $("#txtAdoptionformDoc").val(), 
                //};

                $.ajax({
                    /* url: '/Employee/InsertNomineeDetails',*/
                    url: '/GroupInsuranceClaim/GISInsertNomineeDetails',
                    data: formData,
                    async: false,
                    type: 'POST',
                    cache: false,
                    contentType: false,
                    processData: false,
                    //success: function (result) {

                    //url: '/GroupInsurance/GISInsertNomineeDetails',
                    //data: formData,
                    //async: false,
                    //type: 'POST',
                    //cache: false,
                    //contentType: false,
                    //processData: false,

                    success: function (result) {

                        alertify.success("Nominee details saved successfully");
                        //  NserverResponse = true;
                        //  addRow(result);                
                        GetNomineeDetails();
                        $("#txtNomineeName").val("");
                        $("#txtNomineeDOB").val("");
                        $("#txtNomineeShare").val("");
                        $("#txtNomineeAge").val("");

                        $("#ddlNomineeList").val("Select").attr("selected", "selected");

                        $("#txtNomineeName").attr("disabled", true);
                        $("#txtNomineeDOB").attr("disabled", true);
                        $("#txtNomineeAge").attr("disabled", true);
                        $("#txtNomineeShare").attr("disabled", true);
                    }, error: function (result) {
                        alertify.error("Error in  saving nominee details");
                    }
                });
                ////
            }
        } else { alertify.alert('Memeber already Added').setHeader("Warning!!!"); }
    }
    else {

        var rowCount = $("#tblNomineeDetails1 tbody tr[data-row-number]").length + 1

        if (rowCount <= 5) {
            debugger;
            $('.err').attr("hidden", true);
            var i = 0;

            //var valresult;
            //if ($("#ddlNomineeList").val() == 14) { valresult = true }
            //else { valresult = validateNomineeFields();}
            var valresult = validateNomineeFields();

            var formData = new FormData();
            var file = document.getElementById("txtAdoptionformDoc").files[0];
            formData.append("EmpId", $("#hdnPReferanceNo").val());
            formData.append("NameOfNominee", $("#txtNomineeName").val());
            formData.append("Relation", $("#ddlNomineeList").val());
            formData.append("PercentageShare", $("#txtNomineeShare").val());
            formData.append("Age", $("#txtNomineeAge").val());
            formData.append("nomineeDOB", $("#txtNomineeDOB").val());
            formData.append("SonDaughterAdoption_doc", $("#txtAdoptionformDoc").val());
            formData.append("SonDaughterAdoption_doc", file);
            formData.append("SonDaughterAdoption_doc_path", file);
            formData.append("gnd_contingencies", $("#txtNomineecontingencies").val());
            formData.append("gnd_predeceasing", $("#txtNomineepredeceasing").val());
            formData.append("gnd_bank_account_number", $("#txtBnkAcNo").val());
            formData.append("gnd_ifsc", $("#txtifscCode").val());
            formData.append("gnd_micr", $("#txtMicr").val());

            if (valresult == true) {
                //var viewModelNominee = {
                //    'EmpId': $("#hdnPReferanceNo").val(), 
                //    'NameOfNominee': $("#txtNomineeName").val(),
                //    'Relation': $("#ddlNomineeList").val(),
                //    'PercentageShare': $("#txtNomineeShare").val(),
                //    'Age': $("#txtNomineeAge").val(),// guardianAge
                //    'nomineeDOB': $("#txtNomineeDOB").val(),
                //    'SonDaughterAdoption_doc': $("#txtAdoptionformDoc").val(),
                //    //'SonDaughterAdoption_doc': fileData,
                //    'SonDaughterAdoption_doc_path': $("#txtAdoptionformDoc").val(), 
                //};

                $.ajax({
                    /* url: '/Employee/InsertNomineeDetails',*/
                    url: '/GroupInsuranceClaim/GISInsertNomineeDetails',
                    data: formData,
                    async: false,
                    type: 'POST',
                    cache: false,
                    contentType: false,
                    processData: false,
                    //success: function (result) {

                    //url: '/GroupInsurance/GISInsertNomineeDetails',
                    //data: formData,
                    //async: false,
                    //type: 'POST',
                    //cache: false,
                    //contentType: false,
                    //processData: false,

                    success: function (result) {

                        alertify.success("Nominee details saved successfully");
                        //  NserverResponse = true;
                        //  addRow(result);                
                        GetNomineeDetails();
                        $("#txtNomineeName").val("");
                        $("#txtNomineeDOB").val("");
                        $("#txtNomineeShare").val("");
                        $("#txtNomineeAge").val("");

                        $("#ddlNomineeList").val("Select").attr("selected", "selected");

                        $("#txtNomineeName").attr("disabled", true);
                        $("#txtNomineeDOB").attr("disabled", true);
                        $("#txtNomineeAge").attr("disabled", true);
                        $("#txtNomineeShare").attr("disabled", true);
                    }, error: function (result) {
                        alertify.error("Error in  saving nominee details");
                    }
                });
                ////
            }

        }
        else { alertify.alert('Cannot add more than 5 nominees').setHeader("Warning!!!"); }
        ////return NserverResponse;
        //$("#txtNomineeName").val("");
        //$("#txtNomineeDOB").val("");
        //$("#txtNomineeShare").val("");
        //$("#txtNomineeAge").val("");

        //$("#ddlNomineeList").val("Select").attr("selected", "selected");

        //$("#txtNomineeName").attr("disabled", true);
        //$("#txtNomineeDOB").attr("disabled", true);
        //$("#txtNomineeAge").attr("disabled", true);
        //$("#txtNomineeShare").attr("disabled", true);

    }
}



//function CheckDuplicateName()
//{
//    debugger;
//    var result = false;
//    $('#tblNomineeDetails1').DataTable().rows().iterator('row', function (context, index)
//    {
//        var $row = $("#tblNomineeDetails1 tr[data-row-number=" + index + "]");
//        var relation = $row.find("TD").eq(4).html();        
//        var relationName = $row.find("TD").eq(1).html();
//        if (relation == $("#ddlNomineeList option:selected").text() && relationName == $("#txtNomineeName").val())
//        { return result= true; }

//    });    
//   return result 
//}

function CheckDuplicateName() {

    var result = false;
    tbl = $('#tblNomineeDetails1').DataTable();

    tbl.rows().every(function () {
        var data = this.data();

        if (data[4] == $("#ddlNomineeList option:selected").text() && data[1] == $("#txtNomineeName").val()) { return result = true;; }

    });

    return result
    //  debugger;
    //var result = false;
    //$('#tblNomineeDetails1').DataTable().rows().iterator('row', function (context, index) {
    //    var $row = $("#tblNomineeDetails1 tr[data-row-number=" + index + "]");
    //    var relation = $row.find("TD").eq(4).html();
    //    var relationName = $row.find("TD").eq(1).html();
    //    if (relation == $("#ddlNomineeList option:selected").text() && relationName == $("#txtNomineeName").val()) { return result = true; }

    //});
    //return result
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

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}


$(txtNomineeName).keyup(function (event) {

    $("#errNomNameReq").attr("hidden", true);;
    $("#errNomNameDupl").attr("hidden", true);;
});

$(txtNomineeDOB).keyup(function (event) {

    $("#errDOB").attr("hidden", true);;
    $("#errDOBAge").attr("hidden", true);;

});
$(txtNomineeShare).keyup(function (event) {

    $("#errNomineeShareReq").attr("hidden", true);;
    $("#errNomineeSharePercent").attr("hidden", true);;

});

$(txtEditNomineeName).keyup(function (event) {

    $("#errEditNomNameReq").attr("hidden", true);;
    $("#errEditNomNameDupl").attr("hidden", true);;
});

$(txtEditNomineeDOB).keyup(function (event) {

    $("#errEditDOB").attr("hidden", true);;
    $("#errEditDOBAge").attr("hidden", true);;

});
$(txtEditNomineeShare).keyup(function (event) {

    $("#errEditNomineeShareReq").attr("hidden", true);;
    $("#errEditNomineeSharePercent").attr("hidden", true);;

});


function validateNone() {
    var isNoneAdded = $(".nRelation:contains('None')").length;
    alert(isNoneAdded);
    if (isNoneAdded > 0) {
        return false;
    }
    else { return true; }
}