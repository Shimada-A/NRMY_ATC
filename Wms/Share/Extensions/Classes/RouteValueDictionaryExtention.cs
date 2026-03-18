namespace Share.Extensions.Classes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;

    /// <summary>
    /// RouteValueDictionaryの拡張メソッド
    /// </summary>
    public static class RouteValueDictionaryExtention
    {
        /// <summary>
        /// 複合型とコレクションを含むRouteValueDictionaryを返します。
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns>複合型とコレクションを含むRouteValueDictionary</returns>
        public static RouteValueDictionary WithComplexType(this RouteValueDictionary routeValues)
        {
            var newRouteValues = new RouteValueDictionary();
            SetRouteValues(ref newRouteValues, string.Empty, routeValues.ToDictionary(r => r.Key, r => r.Value));

            return newRouteValues;
        }

        private static void SetRouteValues(ref RouteValueDictionary rvd, string parentKey, Dictionary<string, object> dict)
        {
            var pKey = string.Empty;
            if (!string.IsNullOrWhiteSpace(parentKey))
            {
                pKey = parentKey + ".";
            }

            foreach (var d in dict)
            {
                if (d.Value == null)
                    continue;

                // Valueが基本型のもの（objectを除く）
                if (d.Value is string || d.Value is decimal
                    || d.Value is DateTime || d.Value is DateTimeOffset
                    || d.Value.GetType().IsPrimitive || d.Value.GetType().IsEnum)
                {
                    rvd.Add(pKey + d.Key, d.Value);
                }

                // コレクション型(命名規則を変更する)
                else if (d.Value is IEnumerable)
                {
                    var index = 0;
                    foreach (var val in (IEnumerable)d.Value)
                    {
                        if (val is string || val.GetType().IsPrimitive)
                        {
                            // rvd.Add(pKey + d.Key, d.Value);
                            rvd.Add(pKey + d.Key + "[" + index + "]", val);
                        }
                        else
                        {
                            // rvd.Add(parentKey + "[" + index + "]" + "." + d.Key, d.Value);
                            var properties = val.GetType().GetProperties();
                            var props = properties.ToDictionary(p => p.Name, p => p.GetValue(val));

                            SetRouteValues(ref rvd, pKey + d.Key + "[" + index + "]", props);
                        }

                        index++;
                    }
                }

                // 複合型
                else
                {
                    var properties = d.Value.GetType().GetProperties();
                    var props = properties.ToDictionary(p => p.Name, p => p.GetValue(d.Value));

                    SetRouteValues(ref rvd, pKey + d.Key, props);
                }
            }
        }
    }
}