using System;
using System.Collections.Generic;
using System.Text;

namespace HtmlParser
{
    public interface IHtmlElement
    {
        string Id { get; set; }
        string ClassName { get; set; }
        string Href { get; set; }
        string Identifier { get; set; }
        string Content { get; set; }
        string Text { get; set; }
        IList<HtmlElement> Children { get; set; }
        bool HasChildren { get; }
        Dictionary<string, string> Attributes { get; set; }
        bool HasAttributes { get; }

        IHtmlElement FindElement(By by);
        IEnumerable<IHtmlElement> FindElements(By by);

        IHtmlElement Parse(string text);
    }
}
