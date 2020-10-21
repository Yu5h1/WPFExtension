using System;
using System.Collections.Generic;
using System.Linq;

public struct FileTypeFilter
{
    public string DisplayName;
    public string[] types;
    public FileTypeFilter(string displayName, params string[] fileTypes)
    {
        DisplayName = displayName.Replace("*", "").Replace(".", "");
        types = fileTypes;
    }
    public override string ToString()
    {
        string typesTxT = string.Join("; ", types.Select(d => {
            var item = d;
            if (!item.StartsWith("*."))
            {
                if (item.StartsWith(".")) item = "*" + item;
                else item = "*." + item;
            }
            return item;
        }));
        return DisplayName + "(" + typesTxT + ")|" + typesTxT;
    }
}
public static class FileTypeFilterUtil
{
    public static string ToString(this IEnumerable<FileTypeFilter> filters, bool IncludeAllFile = true)
        => string.Join("|", filters.Select(d => d.ToString())) + (IncludeAllFile ? "" : "|All files (*.*)|*.*");
    public static string ToFiltersString(this string[] Filters, bool IncludeAllFile = true) => Filters.Select(d => new FileTypeFilter(d, d)).ToString(IncludeAllFile);
}
