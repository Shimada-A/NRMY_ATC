/**
 * ファイルアップロード
 * _FileUploadModal.cshtmlに必要
 * https://www.dropzonejs.com/
 */
var fileUploadModal = (function () {
    var id = '#fileUploadModalDialog';
    var myDropzone;
    var successCallback = function (data) { };

    return {
        /**
         * モーダルダイアログの初期設定
         */
        defaultSetting: function () {
            var w = $(window).width();
            $(id).dialog({
                autoOpen: false,
                modal: true,
                dialogClass: 'modalct',
                close: function () {
                    $('.wrap').attr({ style: '' });
                }
            });
        },
        adjust: function () {
            var w = $(window).width();
            var h = $(window).height();
            $('.modalct').css('width', w - 120);
        },
        resize: function () {
            var timer = false;
            if (timer !== false) {
                clearTimeout(timer);
            }
            timer = setTimeout(function () { fileUploadModal.adjust(); }, 200);
        },
        /**
         * ウィンドウを閉じる(キャンセル)
         */
        close: function () {
            $('.wrap').attr({ style: '' });
            $(id).dialog('close');
            document.activeElement.blur(); /* フォーカス解除 IE11 */
        },
        /**
         * ウィンドウの外をクリックしたときにウィンドウを閉じる
         */
        setCloseEvent: function () {
            //Attach cancel button.
            $(id).find('button.btn_close').on('click', function () {
                $(id).dialog('close');
            });

            //Attach outside of dialog area.
            $(document).on('click', '.ui-widget-overlay', function () {
                $(this).prev().find('.ui-dialog-content').dialog('close');
            });
        },
        /**
         * モーダルダイアログを開く
         */
        open: function () {
            $(id).dialog('open');
        },
        /**
         * 初期化
         */
        init: function () {
            this.defaultSetting();
            this.adjust();
            this.setCloseEvent();
            this.createDropzone();
            $(id).on('click', '#btnFileUploadComplete', this.close);
        },
        createDropzone: function () {
            //Dropzoneの設定
            myDropzone = new Dropzone("div#myDropzone", {
                url: "#",
                autoProcessQueue: true,
                success: function (file, data) {
                    //console.log(file);
                    //console.log(data);
                    // 全てアップロード完了したらサムネイル画像を削除
                    if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                        myDropzone.removeAllFiles();
                        successCallback(data);
                        fileUploadModal.close();
                    }
                },
                error: function (file, message) {
                    alert(message);
                    myDropzone.removeFile(file);
                },
                //TODO 多言語対応
                //https://stackoverflow.com/questions/17702394/how-do-i-change-the-default-text-in-dropzone-js
                dictFallbackMessage: "ブラウザがドラッグ＆ドロップに対応していません",
                dictFallbackText: "Please use the fallback form below to upload your files like in the olden days.",
                dictFileTooBig: "ファイルサイズが{{maxFilesize}}MBをオーバーしています。({{filesize}}MB)",
                dictInvalidFileType: "ファイルの種類が正しくありません",
                dictResponseError: "サーバーエラーが発生しました。ステータスコード: {{statusCode}}",
                dictCancelUpload: "キャンセル",
                dictCancelUploadConfirmation: "アップロードをキャンセルしますか？",
                dictRemoveFile: "削除",
                dictMaxFilesExceeded: "ファイル数が多すぎます",
            });
        },
        setting: function (args) {
            //Dropzoneの設定
            myDropzone.options.url = args.url || "#";
            myDropzone.options.acceptedFiles = args.acceptedFiles || ".csv";
            myDropzone.options.maxFiles = args.maxFiles || 1;
            myDropzone.options.maxFilesize = args.maxFilesize || 1; // MB
            $frm = $('form');
            if (args.form != undefined) $frm = args.form;
            myDropzone.on('sending', function (file, xhr, formData) {
                params = $frm.serializeArray();
                $.each(params, function (i, val) {
                    formData.append(val.name, val.value);
                });
            });

            //画面表示設定
            if (args.title != undefined) { $("#fileUploadTitle").text(args.title); }

            //アップロード成功後の処理
            if (args.successCallback != undefined) {
                successCallback = args.successCallback;
            }
        }
    };
})();