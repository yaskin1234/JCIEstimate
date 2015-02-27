$(function () {
    $(".filter").change(function () {
        var selectedValue = $("#filter").val();
        var url = "/WarrantyIssues/Index";
        document.location = url + "?filterId=" + selectedValue;
    })

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

    $(":text").labelify({ labelledClass: "titleTextBox" });

    $("#toggleControlLocationIssues").click(function () {

        $header = $(this);
        $count = document.getElementById("locationIssueCount");
        //getting the next element
        $content = $header.next();
        //open up the content needed - toggle the slide- if visible, slide up, if not slidedown.
        $content.slideToggle(500, function () {
            //execute this after slideToggle is done
            //change text of header based on visibility of content div
            $header.text(function () {
                //change text based on condition
                return $content.is(":visible") ? "Hide Location Issues(" + $count.innerHTML + ")" : "Show Location Issues(" + $count.innerHTML + ")";
            });
        });

    });

    $("#toggleControlAttachments").click(function () {

        $header = $(this);
        $count = document.getElementById("attachmentCount");
        //getting the next element
        $content = $header.next();
        //open up the content needed - toggle the slide- if visible, slide up, if not slidedown.
        $content.slideToggle(500, function () {
            //execute this after slideToggle is done
            //change text of header based on visibility of content div
            $header.text(function () {
                //change text based on condition
                return $content.is(":visible") ? "Hide Attachments(" + $count.innerHTML + ")" : "Show Attachments(" + $count.innerHTML + ")";
            });
        });

    });

    $("#toggleControlComments").click(function () {

        $header = $(this);
        $count = document.getElementById("commentCount");
        //getting the next element
        $content = $header.next();
        //open up the content needed - toggle the slide- if visible, slide up, if not slidedown.
        $content.slideToggle(500, function () {
            //execute this after slideToggle is done
            //change text of header based on visibility of content div
            $header.text(function () {
                //change text based on condition
                return $content.is(":visible") ? "Hide Comments(" + $count.innerHTML + ")" : "Show Comments(" + $count.innerHTML + ")";
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