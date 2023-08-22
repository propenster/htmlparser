using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace HtmlParser
{
    internal class HtmlDocument : IHtmlDocument
    {
        public IEnumerable<IHtmlElement> Children { get; set; }
        public bool HasChildren => Children.Any();
        public HTMLHead HTMLHead { get; set; }
        public HTMLBody HTMLBody { get; set; }
        /// <summary>
        /// e.g htmlObject.FindElement(By.Id("myTable"));
        /// </summary>
        /// <param name="by">expression to search the HTML Document with</param>
        /// <returns>HtmlElement</returns>
        public IHtmlElement FindElement(By by)
        {
            if(by == null) throw new ArgumentNullException(nameof(by), "Invalid search parameter. You must provide a search parameter exception");
            return FindElements(by).FirstOrDefault();
        }

        /// <summary>
        /// e.g htmlObject.FindElements(By.Id("myTable"));
        /// </summary>
        /// <param name="by">expression to search the HTML Document with</param>
        /// <returns>IEnumerable<HtmlElement></returns>
        public IEnumerable<IHtmlElement> FindElements(By by)
        {
            if (by == null) throw new ArgumentNullException(nameof(by), "Invalid search parameter. You must provide a search parameter exception");
            var token = by.FetchToken;
            switch (by.Selector)
            {
                case Selector.ID:
                case Selector.ClassName:
                case Selector.Href:
                    var safe = token.Split(new char[] { '='});
                    return HTMLBody.Children.Where(x => x.HasAttributes && x.Attributes.ContainsKey(safe.FirstOrDefault()) && x.Attributes[safe.FirstOrDefault()] == safe.LastOrDefault()).ToList();

                case Selector.ElementTag:
                    return HTMLBody.Children.Where(x => x.Content.StartsWith(token)).ToList();

                default:
                    return Enumerable.Empty<IHtmlElement>();

            }
        }

    }
}
