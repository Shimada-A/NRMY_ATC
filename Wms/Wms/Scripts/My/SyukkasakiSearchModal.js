

var txtId;
var txtName;
var txtClass;
var txtPref;
var chks = false;
var Pop_SFlag = true;
setStorage();
setCheckboxs();
$(document).on('click', "#tb_pop tbody tr input[type=checkbox]", function () {
    var obj = $(this);
    if (!chks) {
        if (obj.is(":checked")) {
            $("#tb_pop tbody tr input[type = checkbox]").prop('checked', false);
            obj.prop('checked', true);
        }

    }
    setCheckboxs('set');
});

$(document).on('click', "#pop_div_page ul li a", function () {
    var rel = $(this).attr("rel");
    var relherf = $(this).attr("herf");
    var page_now = $("#Page").val();

    if (rel !== 'prev' && rel !== 'next') {
        $("#Page").val($(this).text());
    }
    else {
        if (rel === 'prev' && typeof (relherf) !== "undefined") {
            $("#Page").val(page_now - 1);
        }
        else if (rel === 'next' && typeof (relherf) !== "undefined") {
            $("#Page").val(page_now + 1);
        }
        else {
            return false;
        }
    }
    //$("#Page").val($(this).text());
    Pop_SFlag = false;
    //$('#frmPopSearch').submit();
});

$(document).on('click', '#btnPopSelect', function () {
    //$(this).attr("data-dismiss", "modal");
    if (window.localStorage) {

        var storage = window.localStorage;
        var pkeys = JSON.parse(storage.getItem('Pop_PKeys'));
        var ids = [];
        var names = [];
        var classs = [];
        var pref = [];
        $.each(pkeys, function (key) {
            if (pkeys[key]['checked']) {
                ids.push(pkeys[key]['id']);
                names.push(pkeys[key]['name']);
                classs.push(pkeys[key]['class']);
                pref.push(pkeys[key]['pref']);
            };
        });

        $(txtId).val(ids.toString());
        $(txtName).val(names.join('、'));
        $(txtClass).val(classs.join('、'));
        $(txtPref).val(pref.join('、'));
        //if (ids.length > 0) {
        //    $(txtId).val(ids.toString());
        //    $(txtName).val(names.join('、'));
        //}
        //else {
        //    //$(this).removeAttr("data-dismiss");
        //}
    }
    //else {
    //    //$(this).removeAttr("data-dismiss");
    //}
});

$('#SyukkasakiModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    txtId = button.data('id');
    txtName = button.data('name');
    txtClass = button.data('class');
    txtPref = button.data('pref');
    var flag = button.data('flag');
    if (typeof (flag) !== "undefined") {
        chks = true;
    }

    // 出荷先と配送業者検索のモーダルダイアログからの呼び出しに限り、z-index値を調整
    var x = $('#syukkasakiTransporterSearchModalDialog').length;
    if (x == 1) {
        // オーバレイ->モーダル->オーバーレイ->モーダル->オーバーレイと交互に重なるようにする
        var zIndex = 3000; //　親モーダルオーバーレイのzIndex
        setTimeout(function () {
            $('.modal-backdrop')
                .not('.modal-stack')
                .css('z-index', zIndex + 1)
                .addClass('modal-stack');
        }, 0);

        $('#SyukkasakiModal').css('z-index', zIndex + 2);
    }

    $("#txtPopCenterId").val($(txtId).val());
    setCheckboxs();

});

$(document).on('click', '#btnPopSearch', function () {
    Pop_SFlag = true;
});



function setStorage() {
    if (Pop_SFlag) {
        if (window.localStorage) {
            var storage = window.localStorage;
            storage.removeItem('Pop_PKeys');

            var data = {
                'StoreClass': $("#txtPopStoreClass").val(),
                'StoreId': $("#txtPopStoreId").val(),
                'StoreName': $("#txtPopStoreName").val(),
                'SortKey': $("#ddlPopSortKey").val(),
                'Sort': $("#ddlPopSort").val(),
                'Page': $("#txtLocId").val(),
                'AreaItem[0].IsCheck': $("input[name='AreaItem[0].IsCheck']").is(':checked'),
                'AreaItem[1].IsCheck': $("input[name='AreaItem[1].IsCheck']").is(':checked'),
                'AreaItem[2].IsCheck': $("input[name='AreaItem[2].IsCheck']").is(':checked'),
                'AreaItem[3].IsCheck': $("input[name='AreaItem[3].IsCheck']").is(':checked'),
                'AreaItem[4].IsCheck': $("input[name='AreaItem[4].IsCheck']").is(':checked'),
                'AreaItem[5].IsCheck': $("input[name='AreaItem[5].IsCheck']").is(':checked'),
                'AreaItem[6].IsCheck': $("input[name='AreaItem[6].IsCheck']").is(':checked'),
                'AreaItem[7].IsCheck': $("input[name='AreaItem[7].IsCheck']").is(':checked'),
                'AreaItem[8].IsCheck': $("input[name='AreaItem[8].IsCheck']").is(':checked'),
                'AreaItem[9].IsCheck': $("input[name='AreaItem[9].IsCheck']").is(':checked'),
                'AreaItem[0].AreaId': $("input[name='AreaItem[0].AreaId']").val(),
                'AreaItem[1].AreaId': $("input[name='AreaItem[1].AreaId']").val(),
                'AreaItem[2].AreaId': $("input[name='AreaItem[2].AreaId']").val(),
                'AreaItem[3].AreaId': $("input[name='AreaItem[3].AreaId']").val(),
                'AreaItem[4].AreaId': $("input[name='AreaItem[4].AreaId']").val(),
                'AreaItem[5].AreaId': $("input[name='AreaItem[5].AreaId']").val(),
                'AreaItem[6].AreaId': $("input[name='AreaItem[6].AreaId']").val(),
                'AreaItem[7].AreaId': $("input[name='AreaItem[7].AreaId']").val(),
                'AreaItem[8].AreaId': $("input[name='AreaItem[8].AreaId']").val(),
                'AreaItem[9].AreaId': $("input[name='AreaItem[9].AreaId']").val()
            };

            var urlAction = $("#MyUrl").val();
            var pageUrl = '';
            if (urlAction.indexOf('?') !== -1) {
                pageUrl = urlAction.replace('?', '/GetPKey?');
            } else {
                pageUrl = urlAction + '/GetPKey';
            }

            $.ajax({
                type: "post",
                url: pageUrl,
                data: data,
                async: false,
                success: function (data) {
                    var pkeys = {};
                    $.each(data, function (n, value) {
                        pkeys[value] = { 'checked': false, 'id': '', 'name': '', 'class': '', 'pref': '' };
                    });

                    var storage = window.localStorage;
                    storage.setItem('Pop_PKeys', JSON.stringify(pkeys));

                }
            });

        }
    }
}

function setCheckboxs(flag) {
    var storage = window.localStorage;
    var rowids = storage.getItem('Pop_PKeys');
    if (rowids !== null) {
        var cnt = 0;
        var jrid = JSON.parse(rowids);
        var record = $("#tb_pop tbody tr");

        record.each(function () {

            var key = $(this).find("[name='SHIP_TO_STORE_ID']").val() + $(this).find("[name='SHIP_TO_STORE_CLASS']").val();
            var chk = $(this).find("input[type=checkbox]");

            if (flag === 'set') {
                if (chk.is(':checked')) {
                    if (!chks) {
                        $('#TempStoreId').val($(this).find("[name='SHIP_TO_STORE_ID']").val());

                        $.each(jrid, function (n, value) {
                            jrid[n] = { 'checked': false, 'id': '', 'name': '', 'class': '', 'pref': '' };
                        });
                    } else {
                        if ($('#TempStoreId').val().indexOf($(this).find("[name='SHIP_TO_STORE_ID']").val()) === -1) {
                            $('#TempStoreId').val($('#TempStoreId').val() + "," + $(this).find("[name='SHIP_TO_STORE_ID']").val());
                        }
                    }
                    jrid[key] = {
                        'checked': true,
                        'id': $(this).find("[name='SHIP_TO_STORE_ID']").val(),
                        'name': $(this).find("[name='SHIP_TO_STORE_NAME']").val(),
                        'class': $(this).find("[name='SHIP_TO_STORE_CLASS']").val(),
                        'pref': $(this).find("[name='PREF_NAME']").val()
                    };
                    chk.prop('checked', jrid[key]['checked']);
                    $('#pop_div_page ul li a').each(function (index, item) {
                        if ($(item).attr('href') !== undefined) {
                            var href = $(item).attr('href').toString().substr(0, $(item).attr('href').toString().indexOf("&TempStoreId="));
                            $(item).attr("href", href + "&TempStoreId=" + $('#TempStoreId').val());
                        }
                    });
                }
                else {
                    jrid[key] = { 'checked': false, 'id': '', 'name': '', 'class': '', 'pref': '' };
                    if ($('#TempStoreId').val() !== "") {
                        var tempstores = $('#TempStoreId').val().split(',');
                        if (tempstores.indexOf($(this).find("[name='SHIP_TO_STORE_ID']").val()) !== -1) {
                            var tempstore = $('#TempStoreId').val().replace($(this).find("[name='SHIP_TO_STORE_ID']").val(), "").replace(",,", ",");
                            if (tempstore.substr(0, 1) === ',') {
                                tempstore = tempstore.substr(1);
                            }
                            $('#TempStoreId').val(tempstore);
                            $('#pop_div_page ul li a').each(function (index, item) {
                                if ($(item).attr('href') !== undefined) {
                                    var href = $(item).attr('href').toString().substr(0, $(item).attr('href').toString().indexOf("&TempStoreId="));
                                    $(item).attr("href", href + "&TempStoreId=" + $('#TempStoreId').val());
                                }
                            });
                        }
                    }
                }
            } else if ($('#TempStoreId').val() !== "") {
                var arr = $('#TempStoreId').val().split(',');
                if (arr.indexOf($(this).find("[name='SHIP_TO_STORE_ID']").val()) !== -1) {
                    jrid[key] = {
                        'checked': true,
                        'id': $(this).find("[name='SHIP_TO_STORE_ID']").val(),
                        'name': $(this).find("[name='SHIP_TO_STORE_NAME']").val(),
                        'class': $(this).find("[name='SHIP_TO_STORE_CLASS']").val(),
                        'pref': $(this).find("[name='PREF_NAME']").val()
                    };
                    chk.prop('checked', jrid[key]['checked']);
                }
                else {
                    jrid[key] = { 'checked': false, 'id': '', 'name': '', 'class': '', 'pref': '' };
                }
            }
            else {
                if (typeof (jrid[key]) !== "undefined") {
                    var obj = jrid[key];
                    chk.prop('checked', obj['checked']);
                }
            }
        });
        if (flag === 'set' || ($('#TempStoreId').val() !== "")) {
            storage.setItem('Pop_PKeys', JSON.stringify(jrid));
        }
    }
}