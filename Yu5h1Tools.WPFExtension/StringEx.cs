using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
