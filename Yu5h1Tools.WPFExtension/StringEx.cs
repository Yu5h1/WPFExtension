using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Yu5h1Tools.WPFExtension
{
    public static class StringEx
    {
        public static bool Match(this string txt, string searchText, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (searchText == "" || searchText == null) return true;
            return txt.ToLower().IndexOf(searchText.ToLower(), stringComparison) >= 0;
        }
        public static bool Match(this string txt, StringComparison stringComparison, params string[] args)
        {
            if (args == null || args.Length == 0) return true;
            foreach (var arg in args) if (txt.Match(arg, stringComparison)) return true;
            return false;
        }
        public static bool Match(this string txt,params string[] args)
                                            => txt.Match(StringComparison.OrdinalIgnoreCase, args);
        public static bool IsFileTypeHasAny(this string txt, params string[] types)
        {
            if (txt == "") return false;
            string ext = Path.GetExtension(txt).ToLower();
            foreach (var t in types)
            {
                string CONDITION = t.ToLower();
                if (!CONDITION.StartsWith(".")) CONDITION = "." + CONDITION;
                if (ext.Equals(CONDITION)) return true;
            }
            return false;
        }
        public static string Join<T>(this IEnumerable<T> objs, string separator = "") => string.Join(separator, objs.ToArray());

        public static string RemoveSuffixFrom(this string txt, params string[] filter)
        {
            foreach (var item in filter)
            {
                var lastIndexOf = txt.LastIndexOf(item);
                if (lastIndexOf > -1) txt = txt.Remove(lastIndexOf);
            }
            return txt;
        }
        public static string RemoveSuffixNumber(this string txt)
        {
            for (int i = txt.Length - 1; i >= 0; i--)
            {
                if (!int.TryParse(txt[i].ToString(), out _))
                {
                    if (i < txt.Length - 1)
                        return txt.Remove(i+1);
                    break;
                }
            }
            return txt;
        }
        public static int? ParseSuffixNumber(this string txt)
        {
            for (int i = txt.Length - 1; i >= 0; i--)
            {
                if (!int.TryParse(txt[i].ToString(), out _))
                {
                    if (i == txt.Length - 1) return null;
                    if (i < txt.Length - 1) return int.Parse(txt.Substring(i + 1));
                }
            }
            return null;
        }
        public static string GetUniqueStringWithSuffixNumber<T>(this string txt, IEnumerable<T> list,Func<T,string> getCompareString)
        {
            if (list.ToList().Exists(d=> getCompareString(d).Equals(txt)))
            {
                var index = txt.ParseSuffixNumber();
                if (index == null) txt = (txt + "1").GetUniqueStringWithSuffixNumber(list, getCompareString);
                else txt = (txt.RemoveSuffixNumber() + (index + 1).ToString()).GetUniqueStringWithSuffixNumber(list, getCompareString);
            }
            return txt;
        }
    }
}
