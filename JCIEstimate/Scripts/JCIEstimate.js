$(function () {
    $(".filter").change(function () {
        var selectedValue = $("#filter").val();
        var url = "/WarrantyIssues/Index";
        document.location = url + "?filterId=" + selectedValue;
    })

    $("#lnkFilter").click(function () {
        var searchVal = $("#txtSearchLocation").val();q
        var url = "/WarrantyIssues/Index";
        document.location = url + "?location=" + searchVal;
    })

    $("#locationUid").change(function () {
        var loc = $(this).val();
        $.getJSON("/WarrantyIssues/GetUnits?locationUid=" + loc, function (result) {
            var options = $("#warrantyUnitUid");
            //don't forget error handling!
            options.empty();
            options.append($("<option />").val("00000000-0000-0000-0000-000000000000").text("-- Choose --"));
            $.each(result, function (item) {
                options.append($("<option />").val(this.id).text(this.name));
            });
        });


        $("#RoomAndLocation").show("slow", function () {
            // Animation complete.
        });
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