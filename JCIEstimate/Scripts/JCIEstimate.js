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

    $("#commissionFilter").change(function () {
        var selectedValue = $("#commissionFilter").val();        
        $("#tblCommissionIssues").load("/CommissionIssues/IndexPartial?filterId=" + escape(selectedValue));
    })

    $(".equipmentFilter").change(function () {
        var selectedValue = $("#equipmentFilter").val();
        var url = "/Equipments/Index";
        document.location = url + "?filterId=" + escape(selectedValue);
    })

    $("#recipientList").change(function () {
        var selectedValue = $("#recipientList").val();
        var url = "/UsersAdmin/MassEmail";
        document.location = url + "?filterId=" + escape(selectedValue);
    })

    $("#srchJCITag").click(function () {
        var selectedValue = $("#equipmentFilterJCITag").val();
        var url = "/Equipments/Index";
        document.location = url + "?jciTag=" + escape(selectedValue);
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

    $(".equipmentTaskDetailFilter").change(function () {
        var selectedValue = $("#equipmentTaskDetailFilter").val();
        var url = "/EquipmentTaskDetails/Index";
        document.location = url + "?filterId=" + escape(selectedValue);
    })

    $(".contractorNoteFilter").change(function () {
        var selectedValue = $("#contractorNoteFilter").val();
        var url = "/ContractorNotes/Index";
        document.location = url + "?filterId=" + escape(selectedValue);
    })

    $("#txtFilter").keyup(function () {
        $val = $("#txtFilter").val();
        if ($val.length + 1 > 2) {
            $("#tblWarrantyIssues").load("/WarrantyIssues/IndexPartial?location=" + escape($val));
        }
    })

    $("#txtCommissionFilter").keyup(function () {
        $val = $("#txtCommissionFilter").val();
        if ($val.length + 1 > 2) {
            $("#tblCommissionIssues").load("/CommissionIssues/IndexPartial?metasysNumber=" + escape($val));
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


    $("#equipmentFilterJCITag").keypress(function (e) {
        if (e.which == 13) {
            $("#srchJCITag").click();
            return false;
        }
    })

    
    $(".expandEquipmentForECM").click(function () {
        var ecmUid = this.id.split('_')[1];
        $("#tblEquipment_" + ecmUid).show();
        $("#collapse_" + ecmUid).show();
        $("#expand_" + ecmUid).hide();
    })

    $(".collapseEquipmentForECM").click(function () {
        var ecmUid = this.id.split('_')[1];
        $("#tblEquipment_" + ecmUid).hide();
        $("#collapse_" + ecmUid).hide();
        $("#expand_" + ecmUid).show();
    })

    $(".showOnScopeCheckbox").change(function () {
        var equipmentUid = this.id;
        
        $.ajax({
            url: "/Equipments/SetShowOnScope",
            type: "POST",
            data: {
                id: equipmentUid,
                value: this.checked
            },
            dataType: "json",
            success: function (data) {
            }
        }).done(function () {
        })
    })

    $(".showOnScopeForECM").change(function () {
        var ecmUid = this.id;

        $.ajax({
            url: "/Equipments/SetShowOnScopeForECM",
            type: "POST",
            data: {
                id: ecmUid,
                value: this.checked
            },
            dataType: "json",
            success: function (data) {
            }
        }).done(function () {
        })
    })

    
    

    $(".assignedToImage").click(function () {
        var id = this.id.split("_")[0];
        var span = $("#" + id + "__assignedToSpanID");
        var lnk = $("#" + id + "__assignedToLinkID");
        var img = $("#" + id + "__assignedToImageID");
        if ($(span).html().indexOf("select") == -1) {
            $(span).load("/ProjectTaskLists/GetAssignmentList?projectTaskListUid=" + escape(id));
            $(document).on('change', '.setTaskAssignment', function () {
                var assignment = $("#" + id + "__assignmentListID");
                var assignmentValue = assignment.val();
                $.ajax({
                    url: "/ProjectTaskLists/SaveAssignment",
                    type: "POST",
                    data: {
                        projectTaskListUid: id,
                        value: assignmentValue
                    },
                    dataType: "json",
                    success: function (data) {
                    }
                }).done(function () {
                })

            });
            $(span).css({ "style": "display:normal;" });
            if (img != undefined) {
                img.remove();
            }

        }
        
    })


    $(".addNewCalendarTask").click(function () {
        $val = this.id;        
        var td = document.getElementById($val + "|NewRow");
        $(td).load("/Calendars/GetNewCalendarTaskForm/" + escape($val));
        $(document).on('change', '.newTaskForm', function () {
            $tdval = this.id.split('|')[0];
            $val = $(this).val();
            var td = document.getElementById($tdval + "|TDlocationUid");
            $(td).load("/Calendars/GetLocationsForProject/" + escape($val));
            $(td).css({ "style": "display:normal;" });
                 
        });
        var btn = document.getElementById($val + '_btn');
        $(document).on('click', '#' + $val + '_btn', function () {
            var id = this.id.split('_')[0];
            $form = document.getElementById(id + '_Form');
            $form.submit();
        });        
        $(td).css({ "style": "display:normal;" });
    })

    $(".addNewProjectCalendarTask").click(function () {
        $val = this.id;
        var td = document.getElementById($val + "|NewRow");
        $(td).load("/ProjectCalendars/GetNewProjectCalendarTaskForm/" + escape($val));
        $(document).on('change', '.newProjectTaskFormProjectUid', function () {
            $tdval = this.id.split('|')[0];
            $val = $(this).val();
            var td = document.getElementById($tdval + "|TDlocationUid");
            $(td).load("/Calendars/GetLocationsForProject/" + escape($val));
            $(td).css({ "style": "display:normal;" });

        });
        var btn = document.getElementById($val + '_btn');
        $(document).on('click', '#' + $val + '_btn', function () {
            var id = this.id.split('_')[0];
            $form = document.getElementById(id + '_Form');
            $form.submit();
        });
        $(td).css({ "style": "display:normal;" });
    })


    $(".newTaskForm").change(function () {
        $val = this.id.split('|')[0];
        var td = document.getElementById($val + "|TDlocationUid");
        $(td).load("/Calendars/GetLocationsForProject/" + escape($val));
        $(td).css({ "style": "display:normal;" });
    })

    $(".newProjectTaskFormProjectUid").change(function () {
        $val = this.id.split('|')[0];
        var td = document.getElementById($val + "|TDlocationUid");
        $(td).load("/Calendars/GetLocationsForProject/" + escape($val));
        $(td).css({ "style": "display:normal;" });
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

    $("#commission_LocationUid").change(function () {
        var loc = $(this).val();
        $.getJSON("/CommissionIssues/GetEquipments?locationUid=" + loc, function (result) {
            var options = $("#commission_EquipmentUid");
            //don't forget error handling!
            options.empty();
            options.append($("<option />").val("00000000-0000-0000-0000-000000000000").text("-- Choose --"));
            $.each(result, function (item) {
                options.append($("<option />").val(this.id).text(this.name));
            });
        });


        $("#CommissionDetails").show("slow", function () {
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

    $("#toggleControlCommissionAttachments").click(function () {

        $header = $(this);
        $count = document.getElementById("commissionAttachmentCount");
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


    $("#toggleControlCommissionComments").click(function () {

        $header = $(this);
        $count = document.getElementById("commissionCommentCount");
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




    


    //var $table = $('#GridTable');
    //$table.floatThead();

    var $table1 = $('#tblEquipment');
    $table1.floatThead();

    var $table2 = $('#tblSalesTeamGrid');
    $table2.floatThead();

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

    $(".completionCheckBox").click(function () {
        $.ajax({
            url: "/LocationCompletionCategories/SaveCheckedBox",
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

    $(".calendarDayTask").change(function () {
        var id = this.id.split("_")[0];
        $.ajax({
            url: "/CalendarDayTasks/SaveCalendarDayTask",
            type: "POST",
            data: {
                id: id,
                value: $(this).val()
            },
            dataType: "json"
        })
        .always(function (data) {            
            var txtArea = document.getElementById(id + "_task");
            var txt = $(txtArea).val();            
            $("#" + id + "_div").show();
            $("#" + id + "_div").html(txt.replace(new RegExp("\n", "g"), "<br/>"));
            $("#" + id + "_divText").hide();
        });
    });

    $(".projectCalendarDayTask").change(function () {
        var id = this.id.split("_")[0];
        $.ajax({
            url: "/ProjectCalendarDayTasks/SaveProjectCalendarDayTask",
            type: "POST",
            data: {
                id: id,
                task: $(document.getElementById(id + "_task")).val(),
                taskStartDate: $(document.getElementById(id + "_taskStartDate")).val(),
                taskDuration: $(document.getElementById(id + "_taskDuration")).val()
            },
            dataType: "json"
        })        
    });


    $(".projectCalendarTaskEditLink").click(function () {
        var id = this.id.split("_")[0];
        $(".projectCalendarTaskSaveLink").show();
        $(".projectCalendarTaskEditLink").hide();
        $("#" + id + "_divText").show();
        $("#" + id + "_div").hide();
        $("#" + id + "_divStartDate").show();
        $("#" + id + "_taskStartDateSpan").hide();
        $("#" + id + "_divDuration").show();
        $("#" + id + "_spanDuration").hide();
    });

    $(".projectCalendarTaskSaveLink").click(function () {
        var id = this.id.split("_")[0];
        $(".projectCalendarTaskSaveLink").hide();
        $(".projectCalendarTaskEditLink").show();
        var txtArea = document.getElementById(id + "_task");
        var txt = $(txtArea).val();
        $("#" + id + "_div").show();
        $("#" + id + "_div").html(txt.replace(new RegExp("\n", "g"), "<br/>"));
        $("#" + id + "_divText").hide();
        $("#" + id + "_divStartDate").hide();
        $("#" + id + "_taskStartDateSpan").show();
        $("#" + id + "_divDuration").hide();
        $("#" + id + "_spanDuration").show();
        location.reload();
    });

    $(".calendarProjectFilter").change(function () {
        var selectedValue = $(this).val();
        var calendarUid = this.id.split("_")[0];
        window.location.replace("/Calendars/Edit/" + calendarUid + "?projectUid=" + selectedValue);
    });

    $(".projectCalendarProjectFilter").change(function () {
        var selectedValue = $(this).val();
        var calendarUid = this.id.split("_")[0];
        window.location.replace("/ProjectCalendars/Edit/" + calendarUid + "?projectUid=" + selectedValue);
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

    $(".OpportunityMilestones").click(function () {
        var d = new Date();
        var month = d.getMonth() + 1;
        var day = d.getDate();
        var output = (('' + month).length < 2 ? '0' : '') + month + '/' +
            (('' + day).length < 2 ? '0' : '') + day + '/' +
            d.getFullYear();

        if (this.checked == true) {
            if ($("#dateCompleted_" + $(this).attr("id").split('_')[1]).val() == '') {
                $("#dateCompleted_" + $(this).attr("id").split('_')[1]).val(output);                
            }            
        }
        else {
            $("#dateCompleted_" + $(this).attr("id").split('_')[1]).val("");
        }
        $("#dateCompleted_" + $(this).attr("id").split('_')[1]).triggerHandler('change');
        
        $.ajax({
            url: "/SalesOpportunityMilestones/SaveIsCompleted",
            type: "POST",
            data: {
                id: $(this).attr("id").split('_')[1],
                value: this.checked
            },
            dataType: "json",
            success: function (data) {
                alert("check " + data);
            }
        })
    });

    $(".SalesOpportunityTasks").click(function () {
        $.ajax({
            url: "/SalesOpportunityTasks/SaveIsCompleted",
            type: "POST",
            data: {
                id: $(this).attr("id").split('_')[1],
                value: this.checked
            },
            dataType: "json",
            success: function (data) {
                alert("check " + data);
            }
        })
    });


    $(".OpportunityMilestonesDate").change(function () {
        $.ajax({
            url: "/SalesOpportunityMilestones/SaveDateCompleted",
            type: "POST",
            data: {
                id: $(this).attr("id").split('_')[1],
                value: $(this).val()
            },
            dataType: "json",
            success: function (data) {
                alert("check " + data);
            }
        })
    });


    $("#addNewMasterTask").click(function () {

        $("#masterTaskNewTaskTable").show();
    });

    $("#addNewWeeklyTask").click(function () {

        $("#newWeeklyTaskTable").show();
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

    $("#lnkAddTasksToProject").click(function () {
        $.ajax({
            url: "/ProjectTaskLists/AddTasksForProject",
            type: "POST",
            data: {
                startDate: $("#startDate").val()
            },
            dataType: "json",
            success: function (data) {
                alert("check " + data);
            }
        }).done(function () {
        })
        location.reload();
    });


    $("#btnExportContractorNotes").click(function () {
        var dtltbl = $('#dvData').html();
        window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('#dvData').html()));
    }); 

    $("#btnExportEquipment").click(function () {
        var dtltbl = $('#dvEquipmentData').html();
        window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('#dvEquipmentData').html()));
    });

    $("#btnExportMassEmail").click(function () {        
        window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('#dvMassEmailData').html()));
    });
    
    $("#lnkExportCalendar").click(function () {
        $(".toRemoveForExport").remove();        
        $(".addNewCalendarTask").remove();
        $(".addNewProjectCalendarTask").remove();
        $(".addBorder").attr("style", "border:solid;border-width:thin;");
        
        window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('#calendarData').html()));
        location.reload();
    });

    $(".calendarTaskEditLink").click(function () {
        var id = this.id.split("_")[0];
        $("#" + id + "_divText").show();
        $("#" + id + "_div").hide();
    });

    $(".lnkAddProject").click(function () {
        var projectCalendarUid = this.id.split("_")[0];
        var projectUid = $("#projectUidAsAdd").val();
        var startDate = $("#taskStartDate").val();
        $.ajax({
            url: "/ProjectCalendars/AddNewProjectToCalendar",
            type: "POST",
            data: {
                projectCalendarUid: projectCalendarUid,
                projectUid: projectUid,
                startDate: startDate
            },
            dataType: "json",
            success: function (data) {
                alert("check " + data);
            }
        }).done(function () {
        })
        location.reload();
    });    

    $(".addNewProjectToCalendar").change(function () {
        $("#addProjectToCalendar").show();        
    });

    
    

    $(".form-control-shift").change(function () {        
        $.ajax({
            url: "/ContractorSchedules/SaveTask",
            type: "POST",
            data: {
                field: $(this).attr("id").split('_')[0],
                identifier: $(this).attr("id").split('_')[1],
                value: $(this).val()
            },
            dataType: "json",
            success: function (data) {
                alert("check " + data);
            }
        }).done(function () {
        })
    });

    $(".editMasterTask").change(function () {
        $.ajax({
            url: "/MasterSchedules/SaveMasterTask",
            type: "POST",
            data: {
                field: $(this).attr("id").split('_')[0],
                identifier: $(this).attr("id").split('_')[1],
                value: $(this).val()
            },
            dataType: "json",
            success: function (data) {
                alert("check " + data);
            }
        }).done(function () {
        })
    });

    $(".form-control-dates").change(function () {
        var id = $(this).attr("id").split('_')[1];
        $.ajax({
            url: "/ContractorSchedules/SaveTask",
            type: "POST",
            data: {
                field: $(this).attr("id").split('_')[0],
                identifier: id,
                value: $(this).val()
            },
            dataType: "json",
            success: function (data) {
                alert("check " + data);
            }
        }).done(function () {            
        })        
        var startDate = $("#taskStartDate_" + id).val();
        var endDate = $("#taskEndDate_" + id).val();        
        var daysBetween = DateDiff(startDate, endDate, "days");
        if (isNaN(daysBetween)) {
            $("#daysToComplete_" + id).text("Invalid dates");
        }
        else {
            $("#daysToComplete_" + id).text(daysBetween);
        }        
    });

    $(".form-control-dates").focus(function () {
        this.select();
    });

    $(".OpportunityMilestonesDate").focus(function () {
        this.select();
    });

    $('.img-zoom').hover(function () {
        $(this).addClass('transition');

    }, function () {
        $(this).removeClass('transition');
    });

    //$(function () {
    //    $(".form-control-dates").datepicker();
    //});

    $(function () {
        $(".datepicker").datepicker();
    });

    $(function () {
        $("#projectCalendarStartDate").datepicker();
    });

    $(function () {
        $("#userExpenseDate").datepicker();
    });

    $(function () {
        $("[name='startDate']").datepicker();
    });

    $(function () {
        $("[name='endDate']").datepicker();
    });

    

    $(".equipmentTaskDetailItemGrid").change(function () {
        var type = this.id.split("_")[0];
        var id = this.id.split("_")[1];
        var val = $(this).val();       
        
        $.ajax({
            url: "/EquipmentTaskDetailItems/SaveItem",
            type: "POST",
            data: {
                type: type,
                id: id,
                value: val
            },
            dataType: "json",
            success: function (data) {
            }
        }).done(function () {
        })
        
    });
    
    

    $(".taskChangeDate").click(function () {
        var id = this.id.split("_")[0];
        $("#" + id + "_changeDateText").show();
        $("#" + id + "_changeDateText").focus();
    });

    $(".projectTaskChangeDate").click(function () {
        var id = this.id.split("_")[0];
        $("#" + id + "_projectChangeDateText").show();
        $("#" + id + "_projectChangeDateText").focus();
    });


    $("[name='projectTaskList']").focus(function () {
        this.select();
    });

    $("[name='projectTaskList']").change(function () {
        var changedID = this.id.split("_")[0];
        var changedPredecessor = $("#" + changedID + "__predecessorID").val();

        if (changedPredecessor == "") {
            changedPredecessor = 0;
        }
        else {
            var predID;
            var predecessorEndDate;
            var newChangedStartDate;            
            $("span[id*='__sequence']").each(function () {
                if ($(this).html() == changedPredecessor) {
                    predID = this.id.split("_")[0];
                    predecessorEndDate = $("#" + predID + "__endDateID").html();
                    newChangedStartDate = addWeekdays(predecessorEndDate, 0);
                    $("#" + changedID + "__startDateID").val(newChangedStartDate);
                    return false;
                }
            })                        
        }

        var changedStartDate = $("#" + changedID + "__startDateID").val();
        var changedDuration = $("#" + changedID + "__durationID").val();
        var changedSeq = $("#" + changedID + "__sequence").html()
        var isCompleted;
        if ($("#" + changedID + "__isCompletedID").is(":checked")) {
            isCompleted = true;
        }
        else {
            isCompleted = false;
        }

        $.ajax({
            url: "/ProjectTaskLists/SaveTask",
            type: "POST",
            data: {
                projectTaskListUid: changedID,
                startDate: changedStartDate,
                duration: changedDuration,
                predecessor: changedPredecessor,
                isCompleted: isCompleted
            },
            dataType: "json",
            success: function (data) {                
            }
        }).done(function () {
        })
        $("#" + changedID + "__endDateID").html(addWeekdays(changedStartDate, changedDuration));
        var changedEndDate = $("#" + changedID + "__endDateID").html();
        var changedParentUid = $("#" + changedID + "__parent").attr("class");
        var changedParentSeq = $("#" + changedParentUid + "__sequence").html();

        $("[name='sequenceField']").each(function () {
            var thisID = this.id.split("_")[0];
            var thisSeq = $(this).html();
            var thisPredecessor = $("#" + thisID + "__predecessorID").val();
            var thisEndDate = $("#" + thisID + "__endDateID").val();
            var thisStartDate = $("#" + thisID + "__startDateID").val();            

            if (changedSeq == thisPredecessor) {
                var endDate = new Date(changedEndDate);
                var newStartDate = new Date(addWeekdays(endDate, 1));
                var newStartDateText = addWeekdays(newStartDate, 0);
                $("#" + thisID + "__startDateID").val(newStartDateText);
                $("#" + thisID + "__startDateID").change(); 
            }            

            if (thisPredecessor != "") {                
                $("#" + thisID + "__startDateID").prop('disabled', true);
            }
            else {
                $("#" + thisID + "__startDateID").prop('disabled', false);
            }

            ProcessParentTasks(thisID);

            var parentEndDate = $("#" + changedParentUid + "__endDateID").val();            

            if (changedParentSeq == thisPredecessor) {
                var pEndDate = new Date(parentEndDate);
                var newStartDate = new Date(addWeekdays(pEndDate, 1));
                var pStartDateText = addWeekdays(newStartDate, 0);
                $("#" + thisID + "__startDateID").val(pStartDateText);
                $("#" + thisID + "__startDateID").change();
            }
        });
    });

    $("[name='sequenceField']").each(function () {
        var thisID = this.id.split("_")[0];
        var thisSeq = $(this).html();
        var thisPredecessor = $("#" + thisID + "__predecessorID").val();        

        if (thisPredecessor != "") {
            $("#" + thisID + "__startDateID").prop('disabled', true);
        }
        else {
            $("#" + thisID + "__startDateID").prop('disabled', false);        }

        ProcessParentTasks(thisID);

    });

    $(".datepicker").change(function () {
        var id = this.id.split("_")[0];
        var type = this.id.split("_")[1];
        if (type == "projectChangeDateText") {
            $.ajax({
                url: "/ProjectCalendarDayTasks/SaveProjectCalendarDayTaskDate",
                type: "POST",
                data: {
                    id: id,
                    value: $(this).val()
                },
                dataType: "json"
            })
            .always(function (data) {
                $("#" + id + "_projectChangeDateText").hide();
                location.reload();
            })
        }
        if (type == "projectChangeDateText") {
            $.ajax({
                url: "/CalendarDayTasks/SaveCalendarDayTaskDate",
                type: "POST",
                data: {
                    id: id,
                    value: $(this).val()
                },
                dataType: "json"
            })
            .always(function (data) {
                $("#" + id + "_changeDateText").hide();
                location.reload();
            })
        }

    });    

});

function ProcessParentTasks(thisID) {
    var thisParentText = $("#" + thisID + "__parent").html();

    if (thisParentText > "") {
        var parentUid = $("#" + thisID + "__parentUid").val();
        var parentStartDate = new Date("12/31/2199");;
        var parentEndDate = new Date("1/1/1900");
        var currentStartDate;
        var currentEndDate;
        var element_Predecessor = $("#" + thisID + "__predecessorID");
        var thisDuration = $("#" + thisID + "__durationID");        
        var categoryDuration = 0;        

        $(element_Predecessor).remove();
        $("#" + thisID + "__startDateID").css("display", "none");
        $(thisDuration).remove();
        $("#" + thisID + "__endDateID").parent().css('background-color', 'lightblue');

        $("." + parentUid).each(function () {
            var childID = this.id.split("_")[0];
            var childStartDate = $("#" + childID + "__startDateID").val();
            var childEndDate = $("#" + childID + "__endDateID").html();
            currentStartDate = new Date(childStartDate);
            currentEndDate = new Date(childEndDate);
            if (currentStartDate < parentStartDate) {
                parentStartDate = currentStartDate;
            }
            if (currentEndDate > parentEndDate) {
                parentEndDate = currentEndDate;
            }
            categoryDuration += parseInt($("#" + childID + "__durationID").val());
        });
        $("#" + thisID + "__startDateID").val(addWeekdays(parentStartDate, 0));
        $("#" + thisID + "__endDateID").html(addWeekdays(parentEndDate, 0));
        $("#" + thisID + "__startDateID").prop('disabled', true);
        //$(thisDuration).prop('disabled', true);
        //$(thisDuration).val(categoryDuration);        
    }    
}

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
  

function DateDiff(date1,date2,interval) {
    var second=1000, minute=second*60, hour=minute*60, day=hour*24, week=day*7;
    date1 = new Date(date1);
    date2 = new Date(date2);
    var timediff = date2 - date1;
    if (isNaN(timediff)) return NaN;
    switch (interval) {
        case "years": return date2.getFullYear() - date1.getFullYear();
        case "months": return (
            ( date2.getFullYear() * 12 + date2.getMonth() )
            -
            ( date1.getFullYear() * 12 + date1.getMonth() )
        );
        case "weeks"  : return Math.floor(timediff / week);
        case "days"   : return Math.floor(timediff / day); 
        case "hours"  : return Math.floor(timediff / hour); 
        case "minutes": return Math.floor(timediff / minute);
        case "seconds": return Math.floor(timediff / second);
        default: return undefined;
    }
}

function addWeekdays(startDate, daysToAdd) {
    var i = 0;
    var endDate = new Date(startDate);
    while (i < daysToAdd) {
        endDate.setDate(endDate.getDate() + 1);
        if (!(isWeekend(endDate))){
            i++;
        }
    }
    return endDate.toDateString();//(endDate.getUTCMonth() + 1) + "/" + endDate.getUTCDate() + "/" + endDate.getUTCFullYear();
}

function isWeekend(date) {
    if (date.getDay() == 6 || date.getDay() == 0) {
        return true;
    }
    else {
        return false;
    }
}