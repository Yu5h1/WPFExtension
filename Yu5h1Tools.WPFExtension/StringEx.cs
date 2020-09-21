using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Yu5h1Tools.WPFExtension
{
    public static class StringEx
    {
        public static bool Match(this string txt, string searchText,  StringComparison stringComparison )
        {            
            if (searchText == "" || searchText == null) return true;
            return txt.ToLower().IndexOf(searchText.ToLower(), stringComparison) >= 0;
        }
        public static bool IsFileTypeMatches(this string txt, params string[] types)
        {
            if (txt == "") return false;
            //if (!System.Uri.IsWellFormedUriString(path,System.UriKind.Absolute)) {
            //    return false;
            //}
            string ext = Path.GetExtension(txt).ToLower();
            while (ext.StartsWith(".")) ext = ext.Substring(1);
            foreach (var t in types)
            {
                string CONDITION = t.ToLower();
                while (CONDITION.StartsWith(".")) CONDITION = CONDITION.Substring(1);
                if (ext.Equals(CONDITION)) return true;
            }
            return false;
        }
        public static string Join(this IEnumerable<string> strings,string separator = "") => string.Join(separator, strings);
        public static string RemoveSuffixFrom(this string txt, params string[] filter)
        {
            foreach (var item in filter)
            {
                var lastIndexOf = txt.LastIndexOf(item);
                if (lastIndexOf > -1) txt = txt.Remove(lastIndexOf);
            }
            return txt;
        }
    }
}
