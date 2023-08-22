namespace HtmlParser.Tests
{
    public class UnitTest1
    {
        private string _htmlText;
        private IHtmlDocument _htmlDocument;
        private IHtmlParser _htmlParser;
        public UnitTest1()
        {
            _htmlText =
            @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Document</title>
</head>
<body>

    <p id=""paragraph1"">Lorem ipsum, dolor sit amet consectetur adipisicing elit. Odit veritatis, assumenda quibusdam et deserunt architecto nulla eligendi quod recusandae vitae doloremque dicta quam? Asperiores, aut? Autem doloribus voluptatum itaque maiores?</p>
    <p id=""paragraph2"">Lorem ipsum, dolor sit amet consectetur adipisicing elit. Odit veritatis, assumenda quibusdam et deseru?</p>
    <p id=""paragraph3"">Lorem ipsum, dolor sit amet consectetur adipisicing elit. Odit veritatis, assumenda quibusdam et deserunt aaque maiores?</p>
    <p id=""paragraph4"">Lorem ipsum, dolor sit amet consectetur adipisicing elit. Odit veritatis, assumenda quibusdam et deserunt architecto nulla eligendi quod recusandae vitae doloremque dicta quam? Asperiores, aut? Autem doloribus voluptatum itaque maiores?</p>
    <p>My name is Faith</p>
    <h2 class=""Header2"">Welcome to HTMLParser C# by propenster</h2>
    <p>
        This is another paragraph... 
        Loaded 'C:\MinGW\bin\libgcc_s_dw2-1.dll'. Symbols loaded.
        Loaded 'C:\MinGW\bin\libstdc++-6.dll'. Symbols loaded.

    </p>

    <div id=""myDiv"">
       <p>Paragraph under div</p>
        <p>Another paragraph under div </p>
    </div>

</body>
</html>";

            _htmlParser = new HTMLParser();
            _htmlDocument = _htmlParser.Parse(_htmlText);


        }

        [Fact]
        public void Test_HtmlDocument_Gets_Parsed_Correctly()
        {
            IHtmlElement paragraph1 = _htmlDocument.FindElement(By.Id("paragraph1"));
            IHtmlElement headerElement = _htmlDocument.FindElement(By.ClassName("Header2"));
            Assert.Equal("Lorem ipsum, dolor sit amet consectetur adipisicing elit. Odit veritatis, assumenda quibusdam et deserunt architecto nulla eligendi quod recusandae vitae doloremque dicta quam? Asperiores, aut? Autem doloribus voluptatum itaque maiores?",
                paragraph1?.Text);
            Assert.StartsWith("Lorem ipsum,", paragraph1?.Text);

            Assert.Equal("<h2 class=\"Header2\">Welcome to HTMLParser C# by propenster</h2>", headerElement?.Content);
            Assert.Equal("Welcome to HTMLParser C# by propenster", headerElement?.Text);
        }
        [Fact]
        public void Test_Find_Elements_Under_Element_Parses_Successfully()
        {
            //get div...
            IHtmlElement myDiv = _htmlDocument.FindElement(By.Id("myDiv"));
            var paragraphsUnderMyDiv = myDiv.FindElements(By.ElementTag("p"));

            Assert.NotNull(myDiv);
            Assert.NotNull(paragraphsUnderMyDiv);

            Assert.True(myDiv.HasChildren);
            Assert.Equal(2, myDiv.Children.Count);
            Assert.Equal(paragraphsUnderMyDiv.Count(), myDiv.Children.Count());

        }
        [Fact]
        public void Test_Invalid_HTML_Text_For_Parser_Throws_Format_Exception()
        {           
            Assert.Throws<FormatException>(() => _htmlParser.Parse("This is not a valid HTML document"));
            Assert.Throws<ArgumentNullException>(() => _htmlParser.Parse(""));
            Assert.Throws<ArgumentNullException>(() => _htmlParser.Parse(null));
            Assert.Throws<ArgumentNullException>(() => _htmlParser.Parse(" "));
            Assert.Throws<ArgumentNullException>(() => _htmlParser.Parse(string.Empty));
        }
        [Fact]
        public void Test_Find_Elements_Without_SearchBy_Parameter_Expression_Throws_Correct_Exceptions()
        {
            Assert.Throws<ArgumentNullException>(() => _htmlDocument.FindElement(null));
            IHtmlElement myDiv = _htmlDocument.FindElement(By.Id("myDiv"));
            Assert.Throws<ArgumentNullException>(() => myDiv.FindElement(null));
            Assert.Throws<ArgumentNullException>(() => myDiv.FindElements(null));

        }
        [Fact]
        public void Test_Find_Elements_With_Invalid_Empty_SearchBy_Parameter_Expression_Throws_Correct_Exceptions()
        {
            Assert.Throws<ArgumentNullException>(() => _htmlDocument.FindElement(By.Id(null)));
            IHtmlElement myDiv = _htmlDocument.FindElement(By.Id("myDiv"));
            Assert.Throws<ArgumentNullException>(() => myDiv.FindElement(By.ClassName("")));
            Assert.Throws<ArgumentNullException>(() => myDiv.FindElement(By.Href(" ")));
            Assert.Throws<ArgumentNullException>(() => myDiv.FindElements(By.ElementTag(string.Empty)));

        }
        [Fact]
        public void Test_SearchBy_Parameter_Expression_Escapes_Css_Selectors()
        {
            IHtmlElement myDiv = _htmlDocument.FindElement(By.Id("#myDiv"));
            var paragraphsUnderMyDiv = myDiv.FindElements(By.ElementTag("p"));
            var headerElement = _htmlDocument.FindElement(By.ClassName(".Header2"));

            Assert.NotNull(myDiv);
            Assert.NotNull(paragraphsUnderMyDiv);
            Assert.NotNull(headerElement);
            Assert.NotNull(headerElement?.Text);

            Assert.True(myDiv.HasChildren);
            Assert.Equal(2, myDiv.Children.Count);
            Assert.Equal(paragraphsUnderMyDiv.Count(), myDiv.Children.Count());

        }
    }
}