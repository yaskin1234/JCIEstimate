$(function () {

    $body = $("body");

    $(document).on({
        ajaxStart: function () { $body.addClass("loading"); },
        ajaxStop: function () { $body.removeClass("loading"); }
    });

    $(".filter").change(function () {
        var selectedValue = $("#filter").val();
        var url = "/WarrantyIssues/Index";
        $("#tblWarrantyIssues").load("/WarrantyIssues/IndexPartial?filterId=" + escape(selectedValue));        
    })

    $("#txtFilter").keyup(function () {
        $val = $("#txtFilter").val();
        if ($val.length + 1 > 2) {
            $("#tblWarrantyIssues").load("/WarrantyIssues/IndexPartial?location=" + escape($val));
        }
    })

    $("#ecms").change(function () {
        $val = $("#cbNewOrRepalcement").val();        
        $("#dvNewOrReplacement").show();        
    })

    $("#cbNewOrRepalcement").change(function () {
        $val = $("#cbNewOrRepalcement").val();
        if ($val == "New") {
            $("#dvDetails").show();
            $("#dvReplacement").hide();
        }
        if ($val == "Replacement") {
            $("#dvReplacement").show();
            $("#dvDetails").show();
        }
    })

    $("#btnEquipmentGridFilter").click(function () {
        $val = $("#equipmentFilter").val();
        $("#tbEquipment").load("/Equipments/GridEditPartial?filter=" + escape($val));
    })

    $("#equipmentFilter").keyup(function () {
        if (event.keyCode == 13) {
            $("#btnEquipmentGridFilter").click();
        }
    })

    $(".gridFilter").change(function () {
        var selectedValue = $("#gridFilter").val();
        var url = "/Equipments/GridEdit";
        document.location = url + "?filterId=" + escape(selectedValue);
    })

    $(".tdEquipmentForEcm").click(function () {                
        $val = this.id.split("_")[1];
        if ($("#tdEquipmentForEcm_" + $val).html().trim() != "") {
            $("#tdEquipmentForEcm_" + $val).empty();
        }
        else {
            $("#tdEquipmentForEcm_" + $val).load("/Equipments/IndexPartial?ecmUid=" + escape($val));
        }
        
    })

    

    $("#lnkFilter").click(function () {
        var searchVal = $("#txtSearchLocation").val();
        var url = "/WarrantyIssues/Index";
        document.location = url + "?location=" + escape(searchVal);
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

        currentySelectedEquipment = $td;
    });

    $(".toggleEquipmentAttributes").click(function () {

        var $td = $(this);
        $(".selectedEquipmentItem").removeClass("selectedEquipmentItem");
        //getting the next element        
        var selectedRow = $td.parent().parent();
        var equipmentUid = $td.next().text().trim();

        var $toHideId = "tdattr|" + $td.next().text().trim();
        var $toHide = $(document.getElementById($toHideId));
        if (selectedRow.attr("class") != "selectedEquipmentItem") {
            selectedRow.addClass("selectedEquipmentItem")
        } else {
            selectedRow.addClass("unselectedEquipmentItem")
        }
        //open up the content needed - toggle the slide- if visible, slide up, if not slidedown.        
        $toHide.slideToggle(100, function () {
            //execute this after slideToggle is done            
        });        
    });

    $(".toggleEquipmentTasks").click(function () {

        var $td = $(this);
        $(".selectedEquipmentItem").removeClass("selectedEquipmentItem");
        //getting the next element
        var equipmentUid = $td.next().next().text().trim();
        var $toHideId = "tdtasks|" + equipmentUid;
        var selectedRow = $td.parent().parent();
        if (selectedRow.attr("class") != "selectedEquipmentItem") {
            selectedRow.addClass("selectedEquipmentItem")
        } else {
            selectedRow.addClass("unselectedEquipmentItem")
        }
        var $toHide = $(document.getElementById($toHideId));
        //open up the content needed - toggle the slide- if visible, slide up, if not slidedown.        
        $toHide.slideToggle(100, function () {
            //execute this after slideToggle is done
            
        });
    });

    //$("#filter").kendoComboBox();

    //$("#gridFilter").kendoComboBox();

    var $table = $('#GridTable');
    $table.floatThead();

    $(".gridCheckBox").click(function () {
        $.ajax({
            url: "/EquipmentToDoes/SaveCheckedBox",
            type: "POST",
            data: {
                chkBoxName: $(this).attr("name"),
                value: this.checked
            },
            dataType: "json",
            success: function (data) {
                alert("check " + data);
            }
        })
    });

    $(".equipmentAttributeValue").focusout(function () {
        $.ajax({
            url: "/EquipmentAttributeValues/SaveValue",
            type: "POST",
            data: {                
                identifiers: $(this).attr("id"),
                value: ($(this).attr("type") == "text") ? $(this).val() : this.checked
            },
            dataType: "json",
            success: function (data) {
                alert("check " + data);
            }
        }).done(function () {            
        })
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