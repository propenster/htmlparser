using System;
using System.Collections.Generic;
using System.Text;

namespace HtmlParser
{
    /// <summary>
    /// Selector Kind enum used by the By expressor for locating/finding HtmlElements in parse Tree...
    /// </summary>
    public enum Selector
    {
        Href,
        ID,
        ClassName,
        ElementTag,
        XPath,

    }
}
