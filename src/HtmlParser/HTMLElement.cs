using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HtmlParser
{
    public class HtmlElement : IHtmlElement
    {
        public string Id { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string Href { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public IList<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        public bool HasChildren => Children.Any();
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public bool HasAttributes => Attributes.Any();

        public string Text { get; set; }

        public HtmlElement()
        {
            Identifier = Guid.NewGuid().ToString();
        }
        public HtmlElement(string text, string content)
        {
            Text = text;
            Content = content;
        }
        public HtmlElement(string text, string content, Dictionary<string, string> attributes)
        {
            Text = text;
            Content = content;
            Attributes = attributes;
        }

        /// <summary>
        /// e.g htmlObject.FindElement(By.Id("myTable"));
        /// </summary>
        /// <param name="by">expression to search the HTML Document with</param>
        /// <returns>HtmlElement</returns>
        public IHtmlElement FindElement(By by)
        {
            if (by == null) throw new ArgumentNullException(nameof(by), "Invalid search parameter. You must provide a search parameter exception");
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
            if (Content == null) return Enumerable.Empty<IHtmlElement>();
            var element = Parse(this.Text);        

            var token = by.FetchToken;
            if(element != null && element.Children.Any())
            {
                switch (by.Selector)
                {
                    case Selector.ID:
                    case Selector.ClassName:
                    case Selector.Href:

                        var safe = token.Split(new char[] { '=' });
                        return element.Children.Where(x => x.HasAttributes && x.Attributes.ContainsKey(safe.FirstOrDefault()) && x.Attributes[safe.FirstOrDefault()] == safe.LastOrDefault()).ToList();

                    case Selector.ElementTag:
                        return element.Children.Where(x => x.Content.StartsWith(token)).ToList();

                    default:
                        return Enumerable.Empty<IHtmlElement>();

                }
            }

            return Enumerable.Empty<IHtmlElement>();
        }

        public IHtmlElement Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException("text", "Unparseable invalid HTML text provided. Text cannot be null");
            string tagPattern3 = @"<(?<opener>[^\s>]+)(?:\s+(?<attribute>\w+)\s*=\s*""(?<value>[^""]*)""\s*)*(?:\s*\/?)>(?<content>.*?)<\/\k<opener>>";
            var matches = Regex.Matches(text, tagPattern3, RegexOptions.Singleline);
            if (matches.Count == 0) return this;

            foreach (Match match in matches)
            {
                var opener = match.Groups["opener"].Value;
                var content = match.Groups[0].Value;

                var elem = new HtmlElement(match.Groups["content"].Value, content);
                Group attributeGroup = match.Groups["attribute"];
                Group valueGroup = match.Groups["value"];

                if (attributeGroup.Success && valueGroup.Success)
                {
                    CaptureCollection attributes = attributeGroup.Captures;
                    CaptureCollection values = valueGroup.Captures;

                    for (int i = 0; i < attributes.Count; i++)
                    {
                        string attribute = attributes[i].Value;
                        string value = values[i].Value;

                        Console.WriteLine($"Attribute: {attribute}, Value: {value}");
                        elem.Attributes.Add(attribute, value);
                    }
                }
                Children.Add(elem);
            }
            return this;
        }
    }
}
