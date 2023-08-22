using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HtmlParser
{
    public class HTMLHead : IHtmlElement
    {
        public IList<HtmlElement> Metas { get; set; } = new List<HtmlElement>();
        public IList<HtmlElement> LinkHrefs { get; set; } = new List<HtmlElement>();
        public HtmlElement Title { get; set; }
        public string Id { get; set; }
        public string ClassName { get; set; }
        public string Href { get; set; }
        public string Identifier { get; set; }
        public string Content { get; set; }
        public IList<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        public bool HasChildren => Children.Any();
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public bool HasAttributes => Attributes.Any();

        public string Text { get; set; }

        public HTMLHead()
        {
            Identifier = Guid.NewGuid().ToString();
        }

        public IHtmlElement FindElement(By by)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IHtmlElement> FindElements(By by)
        {
            throw new NotImplementedException();
        }

        public IHtmlElement Parse(string text)
        {
            throw new NotImplementedException();
        }
    }
}
