//$(function () {
//    $('#locationUid').change(function () {
//        $.ajax({
//            url: '/WarrantyIssues/EquipmentForLocation',
//            type: 'POST',
//            data: { locationUid: $(this).val() },
//            datatype: 'json',
//            success: function (data) {
//                var options = '';
//                $.each(data, function () {
//                    options += '<option value="' + this.equipmentUid + '">' + this.equipment + '</option>';
//                });
//                $('#equipmentUid').prop('disabled', false).html(options);
//            }
//        });
//    });
//});

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