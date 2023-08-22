using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace HtmlParser
{
    public class By
    {
        public string FetchToken { get; set; } = string.Empty;
        public Selector Selector { get; set; }

        public static By ElementTag(string elementTagToFind)
        {
            if (string.IsNullOrWhiteSpace(elementTagToFind))
            {
                throw new ArgumentNullException(nameof(elementTagToFind), "Cannot find elements when the elementTagToFind is null.");
            }
            var by = new By();
            by.FetchToken = string.Format("<{0}",elementTagToFind);
            by.Selector = Selector.ElementTag;
            return by;
        }

        public static By Href(string href)
        {
            if (string.IsNullOrWhiteSpace(href))
            {
                throw new ArgumentNullException(nameof(href), "Cannot find elements when the href is null.");
            }
            var by = new By();
            by.FetchToken = string.Format("{0}={1}", nameof(href).ToLowerInvariant(), href);
            by.Selector = Selector.Href;
            return by;
        }
        public static By Id(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Cannot find elements when the id is null.");
            }
            id = EscapeCssSelector(id);
            var by = new By();
            by.FetchToken = string.Format("{0}={1}", nameof(id).ToLowerInvariant(), id);
            by.Selector = Selector.ID;
            return by;
        }
        public static By ClassName(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentNullException(nameof(className), "Cannot find elements when the className is null.");
            }
            className = EscapeCssSelector(className);
            var by = new By();
            by.FetchToken = string.Format("{0}={1}", "class", className);
            by.Selector = Selector.ClassName;
            return by;
        }
        public static By XPath(string xpath)
        {
            if (string.IsNullOrWhiteSpace(xpath))
            {
                throw new ArgumentNullException(nameof(xpath), "Cannot find elements when the XPath expression is null.");
            }

            var by = new By();
            by.FetchToken = string.Format("{0}={1}", "xpath", xpath);
            by.Selector = Selector.XPath;
            return by;
        }

        internal static string EscapeCssSelector(string selector)
        {
            string result = Regex.Replace(selector, @"[#\.\[\]:,>+~=\s]+", string.Empty);
            return result;
        }

    }
}
