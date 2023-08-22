using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HtmlParser
{
    public interface IHtmlDocument
    {
        IEnumerable<IHtmlElement> Children { get; set; }
        bool HasChildren { get; }

        HTMLHead HTMLHead { get; set; }
        HTMLBody HTMLBody { get; set; }
        IHtmlElement FindElement(By by);
        IEnumerable<IHtmlElement> FindElements(By by);
    }
}
