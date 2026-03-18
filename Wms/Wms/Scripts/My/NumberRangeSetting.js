//RangeSlider

$(document).on("click", ".slider_open", function () {
    $(".slider_dialog").dialog("close");
});

var rangeSliderClosed = function () { };

var rangeSliderClick = function () {
    var $start = $(this).parent().parent().children(".rs_input").find(".slider_start");
    var $end = $(this).parent().parent().children(".rs_input").find(".slider_end");
    var dialogId = $(this).attr("data-dialog");
    var $sliderDialog = $("#" + dialogId);

    var $slider = $sliderDialog.find(".slider");
    var prevStartVal;
    var prevEndVal;

    $sliderDialog.dialog({
        draggable: false,
        dialogClass: 'slider_modal1',
        width: 500,
        buttons: {
            'OK': function () {
                var slider = $slider.data("ionRangeSlider");
                $start.val(slider["old_from"]);
                $end.val(slider["old_to"]);
                $(this).dialog('close');
            }
        },
        position: { my: 'right top', at: 'right bottom+5', of: $start },
        open: function () {
            setTimeout(function () {
                $(".wrap").addClass("slider_open");
            }, 200);
            $slider.ionRangeSlider({
                type: "double",
                min: $start.attr('data-min'),
                max: $end.attr('data-max'),
                grid: false,
                grid_snap: false,
                hide_min_max: true,
                prettify_enabled: false,
                onFinish: function (data) {
                    range_start = data['from'];
                    range_end = data['to'];
                }
            });
            var slider_start = $start.val();
            if ($end.val() === '') {
                slider_start = $start.attr('data-min');
            }
            var slider_end = $end.val();
            if ($end.val() === '') {
                slider_end = $end.attr('data-max');
            }
            $slider.data("ionRangeSlider").update({
                from: slider_start,
                to: slider_end
            });
            prevStartVal = slider_start;
            prevEndVal = slider_end;
        },
        close: function () {
            $('.rs-container').remove();
            $('.slider_open').removeClass("slider_open");
            if (prevStartVal != $start.val() || prevEndVal != $end.val()) {
                rangeSliderClosed();
            }
        }
    });
};


$(".slider_start,.slider_end").on("click", rangeSliderClick);