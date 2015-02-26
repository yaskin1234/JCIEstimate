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

    $("#toggleControlAttachments").click(function () {

        $header = $(this);
        //getting the next element
        $content = $header.next();
        //open up the content needed - toggle the slide- if visible, slide up, if not slidedown.
        $content.slideToggle(500, function () {
            //execute this after slideToggle is done
            //change text of header based on visibility of content div
            $header.text(function () {
                //change text based on condition
                return $content.is(":visible") ? "Hide attachments" : "Show attachments";
            });
        });

    });

    $("#toggleControlComments").click(function () {

        $header = $(this);
        //getting the next element
        $content = $header.next();
        //open up the content needed - toggle the slide- if visible, slide up, if not slidedown.
        $content.slideToggle(500, function () {
            //execute this after slideToggle is done
            //change text of header based on visibility of content div
            $header.text(function () {
                //change text based on condition
                return $content.is(":visible") ? "Hide comments" : "Show comments";
            });
        });

    });
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