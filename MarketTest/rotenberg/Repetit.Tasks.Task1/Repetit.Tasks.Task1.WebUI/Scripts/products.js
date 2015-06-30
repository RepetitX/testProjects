var waiting = function () {
    $("img#loading").show();
}

var waiting_complete = function () {
    $("img#loading").hide();
}
$(function () {
    $("#tree").treeTable();
    $("a.a-edit-item").click(function (e) {
        e.preventDefault();
        waiting();
        $("#remote_call_result_edit").empty();
        $("#panel_addProduct").hide();
        $("#remote_call_result_edit").load($(this).attr("href"), function () { $("td#product select[name='ProductID']").attr("disabled", "disabled").addClass("form-control");
            waiting_complete();
            $("#popup_edit_auto").fadeIn();
        });
        
    });
    $("a.edit-option").click(function (e) {
        e.preventDefault();
        waiting();
        $("#panel_addProduct").hide();
        $("#remote_call_result_edit").empty();
        $("#remote_call_result_edit").load($(this).attr("href"), function () {
            $("td#editoption select[name='OptionID']").addClass("form-control");
            waiting_complete();
            $("#popup_edit_auto").fadeIn();
        });

    });
    
    $.datepicker.setDefaults($.extend($.datepicker.regional["ru"]));

   $.validator.methods.date = function (value, element) {
        return (/^\d{2}.\d{2}.\d{4}$/.test(value) || jQuery.trim(value) == '');
    }

    $.validator.methods.number = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    };

    $("input.date").datepicker({ dateFormat: 'dd.mm.yy', buttonImage: '/Content/img/calendar-blue.gif', changeYear: true, showOn: 'button', showOtherMonths: true, yearRange: '2014:2015', defaultDate: +0, buttonImageOnly: false });
    //$("#newProduct").click(function(e) {
    //    e.preventDefault();
    //    var form = $(this).parents("form:fist");
    //    $.ajax(form.attr("action"), {
    //        type: "POST",
    //        data: form.serialize(),
    //        success:function(data) {
    //            if (data && data.result) {
    //                window.location.href = "/Orders/OrderOptions?OrderID=" + $("#OrderID").val();
    //            } else {
    //                alert(data.result);
    //            }
    //        }
    //        }
    //    );
    //});
});
var addOption = function (OrderItemID, OrderID, ProductID) {
    waiting();
    
    $("#remote_call_result_edit").load("/Orders/AddOption?OrderID=" + OrderID + "&OrderItemID=" + OrderItemID + "&ProductID=" + ProductID,
        function() {
            waiting_complete();
          
            $("#popup_edit_auto").fadeIn();
        }
    );
}