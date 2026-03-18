using Share.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Wms.Common;
using Wms.Models;

namespace Wms.Helpers
{
    public static class DialogButtonHelper
    {
        public static IHtmlString ConfirmButtonFor<TModel>(
            this HtmlHelper<TModel> helper,
            DialogType dialogType = DialogType.Create,
            string form = null,
            string text = null,
            string validate = null,
            string target = null,
            object htmlAttributes = null)
        {
            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            // ボタン本体
            if (attrs.ContainsKey("id"))
            {
                id = attrs["id"].ToString();
            }
            else
            {
                throw new Exception("Id isn't exists.");
            }

            hiddenId = $"h_{id}";
            
            var button = CreateButton(text  ,htmlAttributes);
            var hiddenButton = CreateHiddenButton(dialogType,target);
            var script = CreateScript(validate,form);
            return MvcHtmlString.Create($"{button}\n{hiddenButton}\n{script}");
        }

        static string id;
        static string hiddenId;

        private static string GetPartialDialogView<TModel>(HtmlHelper<TModel> helper, DialogType dialogType, string message)
        {
            switch (dialogType)
            {
                case DialogType.Create:
                    return helper.Partial("_CreateConfirmMessageDialog", message ?? MessagesResource.QUE_CREATE).ToHtmlString();
                case DialogType.Update:
                    return helper.Partial("_UpdateConfirmMessageDialog", message ?? MessagesResource.QUE_UPDATE).ToHtmlString();
                case DialogType.Delete:
                    return helper.Partial("_DeleteConfirmMessageDialog", message ?? MessagesResource.QUE_DELETE).ToHtmlString();
                case DialogType.Other:
                    var dialog = new MessageDialog { Id = "dialog",Message=message};
                    return helper.Partial("_CondirmMessageDialog", dialog).ToHtmlString();
            }
            return default(string);
        }

        /// <summary>
        /// Get dialogs id.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static string GetTarget(DialogType t)
        {
            switch (t)
            {
                case DialogType.Create:
                    return "#newconfirm";
                case DialogType.Update:
                    return "#updconfirm";
                case DialogType.Delete:
                    return "#delconfirm";
                case DialogType.Csv:
                    return "#confirm_csv";
            }
            return default(string);
        }

        /// <summary>
        /// Display button.
        /// </summary>
        /// <param name="htmlAttr"></param>
        /// <returns></returns>
        private static string CreateButton(string text,object htmlAttr)
        {
            var button = new TagBuilder("button");
            button.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttr));
            button.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(
                new
                {
                    @onclick=id+"_click()"
                }));
            button.InnerHtml = text;
            return button.ToString(TagRenderMode.Normal);
        }

        private static string CreateScript(string validate,string form)
        {
            var script = new TagBuilder("script");
            script.InnerHtml = $@"
                function {id}_click(){{
                    {(string.IsNullOrEmpty(validate) ? "" : $"if(!{validate}) return;")}
                    {(string.IsNullOrEmpty(form) ? "" : $"if(!validateForm('#{form}')) return;")}
                    $('#{hiddenId}').click();
                }}
            ";
            return script.ToString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Hidden button.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static string CreateHiddenButton(DialogType t,string target = "")
        {
            var button = new TagBuilder("button");

            if (string.IsNullOrEmpty(target))
                target =GetTarget(t);
            // ボタン属性
            button.MergeAttributes(
                HtmlHelper.AnonymousObjectToHtmlAttributes(
                    new { @id=hiddenId,@hidden=true, @type = "button", @data_toggle = "modal", @data_target = target }));

            return button.ToString(TagRenderMode.Normal);
        }
    }
}