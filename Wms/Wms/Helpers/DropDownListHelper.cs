using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Wms.Extensions.Classes;

namespace Wms.Helpers
{
    public static class DropDownListHelper
    {
        public static IHtmlString EnumDropDownInSortArea<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> exp,
            string optionLabel = null,
            object htmlAttributes = null)
        {
            var dropDown = CreateEnumDropDown(helper, exp, optionLabel, htmlAttributes);
            var validation = ValidationExtensions.ValidationMessageFor(helper, exp);
            return MvcHtmlString.Create(
                string.Format("<div class=\"sort\">{0}</div>",
                string.Format("<div class=\"sort_inner\">{0}</div>{1}",
                dropDown.ToHtmlString(),
                validation.ToHtmlString()))
            );
        }

        public static IHtmlString CreateEnumDropDown<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> exp,
            string optionLabel = null,
            object htmlAttributes = null)
        {
            if (helper == null)
                throw new ArgumentNullException("helper");

            var model = ModelMetadata.FromLambdaExpression(exp, helper.ViewData);
            var name =  ExpressionHelper.GetExpressionText(exp); 
            var t = model.ModelType;
            if (t.IsGenericType)
            {
                t = t.GenericTypeArguments[0];
            }
            var list = SelectListHelper.CreateSelectListFromEnum(t,model.Model);
            var selctList = new SelectList(list, "Value", "Text",model.Model);
            if (optionLabel == null)
            {
                return SelectExtensions.DropDownListFor(helper, exp, selctList, htmlAttributes);
            }
            else
            {
                return SelectExtensions.DropDownListFor(helper, exp, selctList, optionLabel, htmlAttributes);
            }
        }

        internal static IHtmlString SelectInternal<TModel>(HtmlHelper<TModel> helper,ModelMetadata metadata,string name, object htmlAttributes,string optionLabel,SelectList selectList)
        {
            // Convert each ListItem to an <option> tag and wrap them with <optgroup> if requested.
            StringBuilder listItemBuilder = BuildItems(optionLabel, selectList);
            
            string fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            TagBuilder tagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes( htmlAttributes));
            tagBuilder.MergeAttribute("name", fullName, true /* replaceExisting */);
            tagBuilder.GenerateId(fullName);

            // If there are any errors for a named field, we add the css attribute.
            ModelState modelState;
            if (helper.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            tagBuilder.MergeAttributes(helper.GetUnobtrusiveValidationAttributes(name, metadata));

            return MvcHtmlString.Create( tagBuilder.ToString(TagRenderMode.Normal));
        }

        internal static string ListItemToOption(SelectListItem item)
        {
            TagBuilder builder = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode(item.Text)
            };
            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }
            if (item.Selected)
            {
                builder.Attributes["selected"] = "selected";
            }
            if (item.Disabled)
            {
                builder.Attributes["disabled"] = "disabled";
            }
            return builder.ToString(TagRenderMode.Normal);
        }

        private static StringBuilder BuildItems(string optionLabel, IEnumerable<SelectListItem> selectList)
        {
            StringBuilder listItemBuilder = new StringBuilder();

            // Make optionLabel the first item that gets rendered.
            if (optionLabel != null)
            {
                listItemBuilder.AppendLine(ListItemToOption(new SelectListItem()
                {
                    Text = optionLabel,
                    Value = String.Empty,
                    Selected = false
                }));
            }

            // Group items in the SelectList if requested.
            // Treat each item with Group == null as a member of a unique group
            // so they are added according to the original order.
            IEnumerable<IGrouping<int, SelectListItem>> groupedSelectList = selectList.GroupBy<SelectListItem, int>(
                i => (i.Group == null) ? i.GetHashCode() : i.Group.GetHashCode());
            foreach (IGrouping<int, SelectListItem> group in groupedSelectList)
            {
                SelectListGroup optGroup = group.First().Group;

                // Wrap if requested.
                TagBuilder groupBuilder = null;
                if (optGroup != null)
                {
                    groupBuilder = new TagBuilder("optgroup");
                    if (optGroup.Name != null)
                    {
                        groupBuilder.MergeAttribute("label", optGroup.Name);
                    }
                    if (optGroup.Disabled)
                    {
                        groupBuilder.MergeAttribute("disabled", "disabled");
                    }
                    listItemBuilder.AppendLine(groupBuilder.ToString(TagRenderMode.StartTag));
                }

                foreach (SelectListItem item in group)
                {
                    listItemBuilder.AppendLine(ListItemToOption(item));
                }

                if (optGroup != null)
                {
                    listItemBuilder.AppendLine(groupBuilder.ToString(TagRenderMode.EndTag));
                }
            }

            return listItemBuilder;
        }

    }
}