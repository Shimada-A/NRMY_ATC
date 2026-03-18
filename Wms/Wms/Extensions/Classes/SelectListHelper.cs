using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Wms.Extensions.Classes
{
    public static class SelectListHelper
    {
        public static List<SelectListItem> CreateSelectListFromEnum(Type t,object value) 
        {
            var list = new List<SelectListItem>();
            foreach(var item in t.GetEnumValues())
            {
                var itemString = item.ToString();
                var field = t.GetField(itemString);
                var displayAttr = field.GetCustomAttribute<DisplayAttribute>();
                var name = displayAttr.GetName();
                if (item.Equals(value))
                {
                    list.Add(new SelectListItem { Value = itemString, Text = name,Selected = true });
                }
                else 
                {
                    list.Add(new SelectListItem { Value = itemString, Text = name });
                }
            }

            return list;
        }
    }
}