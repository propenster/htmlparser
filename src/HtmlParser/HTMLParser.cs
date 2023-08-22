using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HtmlParser
{
    public interface IHtmlParser
    {
        IHtmlDocument Parse(string text);
    }
    public class HTMLParser : IHtmlParser
    {

        public IEnumerable<IHtmlElement> Children { get; set; }

        public bool HasChildren => throw new NotImplementedException();

        public IEnumerable<HTMLHead> HTMLHead { get; set; }
        public HTMLBody HTMLBody { get; set; }

        public IHtmlDocument Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException("text", "Unparseable invalid HTML text provided. Text cannot be null");

            var htmlObject = new HtmlDocument();
            string pattern = @"<(?<opener>[^>]+)(?:[^>]*)(?:(?<content>.*?)<\/\k<opener>>|\/>)";
            var matches = Regex.Matches(text, pattern, RegexOptions.Singleline);
            if (matches.Count == 0) throw new FormatException("Unparseable invalid HTML text provided.");


            var html = matches.Cast<Match>().Select(x => x.Groups["content"].Value).FirstOrDefault();
            html = html.Substring(1); //remove the illegal >

            string openerCloserContentPattern = @"<(?<opener>[^>]+)>(?<content>.*?)<\/\k<opener>>";

            matches = Regex.Matches(html, openerCloserContentPattern, RegexOptions.Singleline);
            if (matches.Count == 0) throw new FormatException("Unparseable invalid HTML text provided.");
            //select parent matches...
            var parentGroupIds = new string[] { "head", "body" };
            var mainMatches = matches.Cast<Match>().Where(x => parentGroupIds.Contains(x.Groups["opener"].Value)).ToList();

            foreach (Match match in mainMatches)
            {
                switch (match.Groups["opener"].Value)
                {
                    case "head":
                        var heads = ParseHead(match.Groups["content"].Value);
                        //HTMLHead = heads;
                        htmlObject.HTMLHead = heads;
                        break;

                    case "body":
                        var content = match.Groups[0].Value;
                        HTMLBody body = ParseBody(match.Groups["content"].Value);
                        body.Text = content;
                        body.Content = match.Groups["content"].Value;
                        htmlObject.HTMLBody = body;
                        break;
                }

            }
            return htmlObject;
        }

        private HTMLBody ParseBody(string bodyText)
        {
            var body = new HTMLBody();
            string tagPattern = @"<(?<opener>[^\s>]+)(?:\s+(?<attribute>\w+)\s*=\s*""(?<value>[^""]*)""\s*)*(?:\s*\/?)>(?<content>.*?)<\/\k<opener>>";

            var matches = Regex.Matches(bodyText, tagPattern, RegexOptions.Singleline);

            if (matches.Count == 0) return body;

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
                body.Children.Add(elem);
            }
            return body;

        }
        /// <summary>
        /// Accepts the content of the HTMLHead element...
        /// </summary>
        /// <param name="headText"></param>
        /// <returns></returns>
        private HTMLHead ParseHead(string headText)
        {
            var head = new HTMLHead();
            string openerCloserContentPattern = @"<(?<opener>[^>]+)(?:[^>]*)(?:(?<content>.*?)<\/\k<opener>>|\/?>)";
            var matches = Regex.Matches(headText, openerCloserContentPattern, RegexOptions.Singleline);

            if (matches.Count == 0) return head;

            foreach (Match match in matches)
            {
                var opener = match.Groups["opener"].Value;
                var content = match.Groups[0].Value;

                //get attrs
                string attributePattern = @"(?<attribute>\w+)\s*=\s*""(?<value>[^""]*)""";
                var attributes = Regex.Matches(content, attributePattern, RegexOptions.Singleline).Cast<Match>().ToDictionary(x => x.Groups["attribute"].Value, x => x.Groups["value"].Value);


                if (match.Groups["opener"].Value.ToLowerInvariant().StartsWith("meta"))
                {
                    head.Metas.Add(new HtmlElement(match.Groups["content"].Value, content, attributes));

                }
                else if (match.Groups["opener"].Value.ToLowerInvariant().StartsWith("link"))
                {
                    head.LinkHrefs.ToList().Add(new HtmlElement(match.Groups["content"].Value, content, attributes));
                }
                else
                {
                    //it's a title...
                    if (!string.IsNullOrWhiteSpace(head?.Title?.Content)) throw new FormatException("Unparseable invalid HTML document. THere can only be one Title per HTMLDocument");
                    head.Title = new HtmlElement(match.Groups["content"].Value, content, attributes);

                }
            }
            return head;

        }


    }
}

