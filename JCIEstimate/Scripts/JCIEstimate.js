$(function () {
    $(".filter").change(function () {
        var selectedValue = $("#filter").val();
        var url = "/WarrantyIssues/Index";
        document.location = url + "?filterId=" + selectedValue;
    })
});

$(function () {
    $("#locationUid").change(function () {
        var unitDropDown = $("#warrantyUnitUid");
        var selectedValue = $("#locationUid").val();
        var url = "/WarrantyIssues/GetUnits?locationUid=" + selectedValue;
        $.post(url, { locationUid: selectedValue }, function (data) {
            $("#document").html(data);
            });
        //$.get(url, null, function (data) {
        //    $.each(data, function () {
        //        unitDropDown.append($("<option />").val(this.warrantyUnitUid).text(this.warrantyUnit1));
        //    });
        //});
    })
});





//$(function () {
//    $('#locationUid').change(function () {
//        $.ajax({
//            url: "/WarrantyIssues/EquipmentForLocation",            
//            type: "POST",
//            data: { locationUid: $(this).val() },
//            dataType: "json",
//            success: function (data) {
//                alert("check " + data);
//            }
//        }).done(function () {
//            alert("done");
//        }).fail(function () {
//            alert("fail");
//        }).always(function () {
//            alert("always");
//        })
//    });
//});