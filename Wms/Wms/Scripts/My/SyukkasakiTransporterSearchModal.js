/**
 * 出荷先配送業者ダイアログ検索モーダル
 * _SyukkasakiTransporterSearchModal.cshtmlに必要
 */
var syukkasakiTransporterSearchModal = (function () {
    var current_scrollY;
    var id = '#syukkasakiTransporterSearchModalDialog';

    return {

        /**
         * モーダルダイアログの初期設定
         */
        defaultSetting: function () {
            var w = $(window).width();
            var h = $(window).height();
            $(id).dialog({
                autoOpen: false,
                width: w / 2,
                position: { my: 'center top+30', at: 'center top', of: window },
                height: h - 50,
                modal: true,
                dialogClass: 'modalct',
                close: function () {
                    $('.wrap').attr({ style: '' });
                    $('html, body').prop({ scrollTop: current_scrollY });
                }
            });
        },
        adjust: function () {
            var w = $(window).width();
            var h = $(window).height();
            $('.modalct').css('width', w / 2);
            $('.modalct .tb_scroll').css('height', h - 430);
            $(id).dialog("option", "position", { my: 'center top+30', at: 'center top', of: window });
        },
        resize: function () {
            var timer = false;
            if (timer !== false) {
                clearTimeout(timer);
            }
            timer = setTimeout(function () { syukkasakiTransporterSearchModal.adjust(); }, 200);
        },
        /**
         * ウィンドウを閉じる(キャンセル)
         */
        close: function () {
            $('.wrap').attr({ style: '' });
            $('html, body').prop({ scrollTop: current_scrollY });
            $(id).dialog("close");
            document.activeElement.blur(); /* フォーカス解除 IE11 */
        },
        /**
         * ウィンドウの外をクリックしたときにウィンドウを閉じる
         */
        setCloseEvent: function () {
            //Attach cancel button.
            $(id).find("button.btn_close").on("click", function () {
                $(id).dialog("close");
            });

            //Attach outside of dialog area.
            $(document).on("click", ".ui-widget-overlay", function () {
                $(".ui-dialog-content").dialog("close");
            });
        },
        /**
         * 選択内容を仮保存する(確定ボタンが押されなかったら破棄する)
         */
        saveTemp: function () {
            var index = $("input[type='checkbox'][name$=IsCheck]").index(this);
            var id = $("input[type='hidden'][name$='SyukkasakiTransporter.CenterId']").eq(index).val();

            if ($(this).prop("checked")) {
                //hiddenを作る
                $("<input>", {
                    type: "hidden",
                    name: "checkedSyukkasakiTransporters",
                    value: id
                }).appendTo("#checked");
            } else {
                $("#checked input[type='hidden']").filter("[value='" + id + "']").remove();
            }
        },
        /**
         * 選択内容を保持する(確定)
         */
        saveChanges: function () {
            $("#selectedSyukkasakiTransporter").empty();
            $("input[type='hidden'][name='checkedSyukkasakiTransporters']").clone().appendTo("#selectedSyukkasakiTransporter");
        },
        /**
         * モーダルダイアログを開く
         */
        open: function (option) {
            current_scrollY = $(window).scrollTop();
            $('.wrap').css({
                position: 'fixed',
                width: '100%',
                top: -1 * current_scrollY
            });

            //初期値設定
            if (option) {
                if (option.centerId != undefined) {
                    $("#hidCenterId").val(option.centerId);
                }
                if (option.centerName != undefined) {
                    $("#lblCenterName").val(option.centerName);
                }
                if (option.brandId != undefined) {
                    $("#lblBrandId").val(option.brandId);
                }
                if (option.brandName != undefined) {
                    $("#lblBrandName").val(option.brandName);
                }
                if (option.isCenterOnly != undefined) {
                    $("#hidIsCenterOnly").val(option.isCenterOnly);

                    if (option.isCenterOnly) {
                        $("#txtStoreClass").val("8");
                        $("#txtStoreClass").prop("disabled", true);
                        $("#hidStoreClass").val("8");
                        $("#hidStoreClass").prop("disabled", false);
                    } else {
                        $("#txtStoreClass").val("");
                        $("#txtStoreClass").prop("disabled", false);
                        $("#hidStoreClass").prop("disabled", true);
                    }
                }
            }

            //画面側の選択項目をコピー
            $("#checked").empty();
            $("#selectedSyukkasakiTransporter input[type='hidden'][name='checkedSyukkasakiTransporters']").clone().appendTo("#checked");

            //全てのチェックをOFF
            $("input[type='checkbox'][name$='IsCheck']").prop("checked", false);

            //選択項目でチェックをON
            //チェック済み(#checked)のhiddenを取得
            var checked = $("#checked input[type='hidden']"); 
            checked.each(function () {
                //各行hiddenから同じvalueを持つものを取得
                var target = $("input[type='hidden'][name$='SyukkasakiTransporter.CenterId']").filter("[value='" + $(this).val() + "']");
                //それの兄弟要素のチェックボックスのチェックをオンにする
                $(target).siblings("input[type='checkbox']").prop("checked", true);
            });

            //検索処理実行
            if (option.search) $("#SyukkasakiTransporterSearch").click();

            $(id).dialog("open");
        },
        /**
         * 初期化
         */
        init: function () {
            this.defaultSetting();
            this.adjust();
            this.setCloseEvent();
            $(window).on('resize', this.resize);
            $('#syukkasakiTransporterClose').on('click', this.close);
            $('#syukkasakiTransporterSelect').on('click', this.close);
            $("#syukkasakiTransporterSelect").on("click", this.saveChanges);
            $("input[type='checkbox'][name$=IsCheck]").on("change", this.saveTemp);
        }
    };
})();
