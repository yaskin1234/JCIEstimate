$(function () {

    $body = $("body");

    $(document).on({
        ajaxStart: function () { $body.addClass("loading"); },
        ajaxStop: function () { $body.removeClass("loading"); }

    })

    $(".filter").change(function () {
        var selectedValue = $("#filter").val();
        var url = "/WarrantyIssues/Index";
        $("#tblWarrantyIssues").load("/WarrantyIssues/IndexPartial?filterId=" + escape(selectedValue));        
    })

    $(".equipmentFilter").change(function () {
        var selectedValue = $("#equipmentFilter").val();
        var url = "/Equipments/Index";
        document.location = url + "?filterId=" + escape(selectedValue);
    })

    $(".equipmentGridFilter").change(function () {
        var selectedValue = $("#equipmentGridFilter").val();
        var url = "/Equipments/GridEdit";
        document.location = url + "?filterId=" + escape(selectedValue);
    })

    $(".estimateFilter").change(function () {
        var selectedValue = $("#estimateFilter").val();
        var url = "/Estimates/Index";
        document.location = url + "?filterId=" + escape(selectedValue);
    })

    $("#txtFilter").keyup(function () {
        $val = $("#txtFilter").val();
        if ($val.length + 1 > 2) {
            $("#tblWarrantyIssues").load("/WarrantyIssues/IndexPartial?location=" + escape($val));
        }
    })

    $("#equipmentUidAsReplaced").change(function () {
        var selectedValue = $(this).val();
        if (selectedValue == "00000000-0000-0000-0000-000000000000") {
            $("#useReplacement").attr("disabled", true);
        }
        else {
            $("#useReplacement").removeAttr("disabled");
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

    $("#btnJCITagSearch").click(function () {
        $val = $("#jciTag").val();
        document.location = "/Equipments/AddPicturesForEquipment?jciTag=" + escape($val);
    })
    
    $("#btnJCITagSearch").click(function () {
        $val = $("#jciTag").val();
        document.location = "/Equipments/AddPicturesForEquipment?jciTag=" + escape($val);
    })

    


    $("#jciTag").keypress(function (e) {
        if (e.which == 13) {
            $("#btnJCITagSearch").click();
            return false;
        }
    })

    

    $("#equipmentAttributeTypeUid").change(function () {
        $val = $("#equipmentAttributeTypeUid").val();

        $("#engineerCreateAttributes").load("/Equipments/GetAttributesForType?equipmentAttributeTypeUid=" + escape($val));
        $("#engineerCreateAttributes").css({ "style": "display:normal;" });
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
        if ($("#getEquipment_" + $val).html().trim() != "") {
            $("#getEquipment_" + $val).empty();
            $(this.parentNode).css({ "style": "display:none;" });
        }
        else {
            $("#getEquipment_" + $val).load("/Equipments/IndexPartial?ecmUid=" + escape($val));
            $(this.parentNode).css({ "style": "display:normal;" });
        }
        
    })

    

    $("#lnkFilter").click(function () {
        var searchVal = $("#txtSearchLocation").val();
        var url = "/WarrantyIssues/Index";
        document.location = url + "?location=" + escape(searchVal);
    })

    $(".NewEquipmentTasks").change(function () {
        var uid = $(this).val();
        var newTasks = document.getElementById("newTasks");
        if (newTasks.value.indexOf(uid) == -1) {
            newTasks.value = newTasks.value + uid + ",";
        }
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

    $(".toggleReplacedEquipment").click(function () {

        var $td = $(this);
        $td.removeClass("selectedEquipmentItem");
        //getting the next element
        var equipmentUid = $td.next().next().next().text().trim();
        var $toHideId = "trreplace|" + equipmentUid;
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

    var $table1 = $('#tblEquipment');
    $table1.floatThead();

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

    $(".contractorSchedule").change(function () {        
        $.ajax({
            url: "/ContractorDraws/SaveDrawScheduleAmount",            
            type: "POST",
            data: {
                id: $(this).attr("name"),
                value: $(this).val()
            },
            dataType: "json"
        })
        .always(function (data) {
            var ele = document.getElementById("runningTotal");
            $(ele).text(data);
        });
    });

    $(".expenseConstructionSchedule").change(function () {
        $.ajax({
            url: "/ExpenseConstructionDraws/SaveExpenseConstructionDrawScheduleAmount",
            type: "POST",
            data: {
                id: $(this).attr("name"),
                value: $(this).val()
            },
            dataType: "json"
        })
        .always(function (data) {            
            var aryData = data.split('|');
            var ele;
            ele = document.getElementById("expenseAmount");
            $(ele).text(aryData[0]);
            ele = document.getElementById("expenseConstructionDrawRunningTotal");
            $(ele).text(aryData[1]);
            ele = document.getElementById("expenseConstructionAmountRemaining");
            $(ele).text(aryData[2]);

            ele = document.getElementById("expenseAmount2");
            $(ele).text(aryData[0]);
            ele = document.getElementById("expenseConstructionDrawRunningTotal2");
            $(ele).text(aryData[1]);
            ele = document.getElementById("expenseConstructionAmountRemaining2");
            $(ele).text(aryData[2]);
        });
    });

    
    
    $(".estimateActiveChk").click(function () {
        $.ajax({
            url: "/Estimates/SaveIsActive",
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
        
        var estimateUid = $(this).attr("name");
        var activeAmountSpan = document.getElementById(estimateUid + "_ActiveAmount");
        var amountSpan = document.getElementById(estimateUid + "_Amount");
        var amount = Number(amountSpan.innerText.replace("$", "").replace(",", ""));
        var bidTotalSpan = document.getElementById("bidTotalSpan");
        var activeTotalSpan = document.getElementById("activeTotalSpan");
        var bidTotal = Number(bidTotalSpan.innerText.replace("$", "").replace(/,/g, ""));
        var activeTotal = Number(activeTotalSpan.innerText.replace("$", "").replace(/,/g, ""));
        if (this.checked) {
            activeAmountSpan.innerText = amountSpan.innerText;            
            activeTotal = activeTotal + amount;
        }
        else {
            activeAmountSpan.innerText = "$0";
            activeTotal = activeTotal - amount;
            
        }        
        activeTotalSpan.innerText = "$" + activeTotal.formatMoney(0, '.', ',');
        if (this.checked) {
            $(this.parentNode.parentNode).css({ "background-color": "#ddffdd" });
        } else {
            $(this.parentNode.parentNode).css({ "background-color": "#ffbbbb" });
        }
    });    

    $(".chkEngineerUseReplacement").click(function () {
        $.ajax({
            url: "/Equipments/SaveUseReplacement",
            type: "POST",
            data: {
                id: $(this).attr("name"),
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

Number.prototype.formatMoney = function (c, d, t) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};
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