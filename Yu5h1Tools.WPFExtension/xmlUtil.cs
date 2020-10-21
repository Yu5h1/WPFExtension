using System;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Yu5h1Tools.WPFExtension
{
    public static class xmlUtil
    {
        public static string GetValue(this XElement node, string path) {
            var target = node.XPathSelectElement(path);
            if (target != null) return target.Value;
            return "";
        }
        public static IEnumerable<string> GetValues(this XElement node, string path) => node.XPathSelectElements(path).Select(d=>d.Value);

        public static void SetValue(this XElement node, string path, object value) {
            var target = node.XPathSelectElement(path);
            if (target == null) {
                target = node.AddHierarchiesIfNull(path.Split('\\'));
            }
            target.Value = value.ToString();
        }
        public static void AddValue(this XElement node, string path, object value)
        {
            if (path == "") return;
            if (!path.Contains('\\')) {
                node.Add(new XElement(path, value));
                return;
            }
            var parentNode = AddHierarchiesIfNull(node,path.Remove(path.LastIndexOf('\\')).Split('\\'));
            parentNode.Add(new XElement(Path.GetFileNameWithoutExtension(path), value));
        }
        public static void RemoveAll(this XElement node, string path) {
            var elements = node.XPathSelectElements(path).ToArray();
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].Remove();
            }
        }
        public static XElement AddHierarchiesIfNull(this XElement node,params string[] hierarchies) {
            var result = node;
            foreach (var item in hierarchies)
            {
                var next = result.Element(item);
                if (next == null) {
                    next = new XElement(item);
                    result.Add(next);
                }
                result = next;
            }
            return result;
        }
        public static XElement CreateHierarchies(params string[] hierarchies)
        {
            XElement result = new XElement(hierarchies[0]);
            if (hierarchies.Length > 1) {
                var current = result;
                for (int i = 1; i < hierarchies.Length; i++)
                {
                    var next = new XElement(hierarchies[i]);
                    current.Add(next);
                    current = next;
                }
            }
            return result;
        }
        public static void SetValue(this XDocument xdoc,string xPath,object value)
        {
            var rootName = xPath.Split('\\')[0];
            xPath = xPath.Remove(0, xPath.IndexOf('\\') + 1);
            if (xdoc.Root.Name != rootName)
            {
                xdoc.Root.Name = rootName;
                xdoc.Root.RemoveNodes();
            }
            xdoc.Root.SetValue(xPath, value);
        }
        public static void AddValue(this XDocument xdoc, string xPath, object value)
        {
            var rootName = xPath.Split('\\')[0];

            if (xPath.IndexOf('\\') >= 0) {
                xPath = xPath.Remove(0, xPath.IndexOf('\\') + 1);
            }
            
            if (xdoc.Root.Name != rootName)
            {
                xdoc.Root.Name = rootName;
                xdoc.Root.RemoveNodes();
            }
            xdoc.Root.AddValue(xPath, value);
        }
        public static void RemoveAll(this XDocument xdoc, string xPath)
        {
            var rootName = xPath.Split('\\')[0];
            if (xPath.IndexOf('\\') >= 0)
            {
                xPath = xPath.Remove(0, xPath.IndexOf('\\') + 1);
            }
            if (xdoc.Root.Name == rootName)
                xdoc.Root.RemoveAll(xPath);
        }
        public static string GetValue(this XDocument xdoc, string xPath)
        {
            var rootName = xPath.Split('\\')[0];
            xPath = xPath.Remove(0, xPath.IndexOf('\\') + 1);
            if (xdoc.Root.Name == rootName) return xdoc.Root.GetValue(xPath);
            return "";
        }
        public static IEnumerable<string> GetValues(this XDocument xdoc, string xPath)
        {
            var rootName = xPath.Split('\\')[0];
            xPath = xPath.Remove(0, xPath.IndexOf('\\') + 1);
            if (xdoc.Root.Name == rootName) return xdoc.Root.GetValues(xPath);
            return null;
        }
        public static void AddValue(string docPath, string xPath, object value)
        {
            if (File.Exists(docPath))
            {
                try
                {
                    var xDoc = XDocument.Load(docPath);
                    xDoc.AddValue(xPath, value);
                    xDoc.Save(docPath);
                }
                catch (Exception e)
                {
                    e.Message.PromptWarnning();
                    throw;
                }
            }
            else
            {
                var node = CreateHierarchies(xPath);
                node.Value = value.ToString();
                new XDocument(node).Save(docPath);
            }
        }
        public static void SetValue(string docPath, string xPath,object value) {
            if (File.Exists(docPath)) {
                try
                {
                    var xDoc = XDocument.Load(docPath);
                    xDoc.SetValue(xPath,value);
                    xDoc.Save(docPath);
                }
                catch (Exception e)
                {
                    e.Message.PromptWarnning();
                    throw;
                }
            }
            else
            {
                var node = CreateHierarchies(xPath);
                node.Value = value.ToString();
                new XDocument(node).Save(docPath);
            }
        }
        public static string GetValue(string docPath, string xPath)
        {
            if (File.Exists(docPath))
            {
                try
                {
                    var xDoc = XDocument.Load(docPath);
                    return xDoc.GetValue(xPath);
                }
                catch (Exception) { return ""; }
            }
            return "";
        }
        public static IEnumerable<string> GetValues(string docPath, string xPath)
        {
            if (File.Exists(docPath))
            {
                try
                {
                    var xDoc = XDocument.Load(docPath);
                    return xDoc.GetValues(xPath);
                }
                catch (Exception) { return null; }
            }
            return null;
        }
        public static void RemoveAll(string docPath, string xPath)
        {
            if (File.Exists(docPath))
            {
                try
                {
                    var xDoc = XDocument.Load(docPath);
                    xDoc.RemoveAll(xPath);
                    xDoc.Save(docPath);
                }
                catch (Exception e)
                {
                    e.Message.PromptWarnning();
                    throw;
                }
            }
        }
    }
}
