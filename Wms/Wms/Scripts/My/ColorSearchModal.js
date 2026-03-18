/**
 * ブランド検索モーダル
 * _ColorSearchModal.cshtmlに必要
 */
var colorSearchModal = (function () {
    var current_scrollY;
    var id = '#colorSearchModalDialog';

    return {
        /**
         * モーダルダイアログの初期設定
         */
        defaultSetting: function () {
            var w = $(window).width();
            var h = $(window).height();
            $(id).dialog({
                autoOpen: false,
                width: w/2,
                height: h-50,
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
            $('.modalct .tb_scroll').css('height', h - 350);
        },
        resize: function () {
            var timer = false;
            if (timer !== false) {
                clearTimeout(timer);
            }
            timer = setTimeout(function () { colorSearchModal.adjust(); }, 200);
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
                $(this).prev().find(".ui-dialog-content").dialog("close");
            });
        },
        /**
         * 選択内容を仮保存する(確定ボタンが押されなかったら破棄する)
         */
        saveTemp: function () {
            var index = $("input[type='checkbox'][name$=IsCheck]").index(this);
            var id = $("input[type='hidden'][name$='Location.CenterId']").eq(index).val();

            if ($(this).prop("checked")) {
                //hiddenを作る
                $("<input>", {
                    type: "hidden",
                    name: "checkedColor",
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
            $("#selectedColor").empty();
            $("input[type='hidden'][name='checkedColor']").clone().appendTo("#selectedColor");
        },
        /**
         * モーダルダイアログを開く
         */
        open: function () {
            current_scrollY = $(window).scrollTop();
            $('.wrap').css({
                position: 'fixed',
                width: '100%',
                top: -1 * current_scrollY
            });

            //画面側の選択項目をコピー
            $("#checked").empty();
            $("#selectedColor input[type='hidden'][name='checkedColor']").clone().appendTo("#checked");

            //全てのチェックをOFF
            $("input[type='radio'][name$='IsCheck']").prop("checked", false);

            //選択項目でチェックをON
            //チェック済み(#checked)のhiddenを取得
            var checked = $("#checked input[type='hidden']");
            checked.each(function () {
                //各行hiddenから同じvalueを持つものを取得
                var target = $("input[type='hidden'][name$='Color.ItemColorId']").filter("[value='" + $(this).val() + "']");
                //それの兄弟要素のチェックボックスのチェックをオンにする
                $(target).siblings("input[type='radio']").prop("checked", true);
            });

            $(id).dialog("open");
        },
        /**
         * フッターのチェックボックスカウンターの設定
         */
        setCheckBoxCounter: function () {
            var checkBoxes = $("input[name$='IsCheck']");
            //checkBoxCounter.init(checkBoxes, null);
            $("#btnColorSearchFreeWord").on("click", this.ColorSearchChange);
        },
        /**
         * 再検索
         */
        ColorSearchChange: function () {
            $("#frmColorSearch").submit();
        },
        /**
         * 初期化
         */
        init: function () {
            this.defaultSetting();
            this.adjust();
            this.setCheckBoxCounter();
            this.setCloseEvent();
            $(window).on('resize', this.resize);
            $('#colorClose').on('click', this.close);
            $('#colorSelect').on('click', this.close);
            $("#colorSelect").on("click", this.saveChanges);
            $("input[type='checkbox'][name$=IsCheck]").on("change", this.saveTemp);
        }
    };
})();
