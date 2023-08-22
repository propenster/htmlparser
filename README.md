# SimpleHTMLParser
A simple and full-feature HTML Parser in C#

### What is SimpleHTMLParser?
SimpleHTMLParser is a simple to use, efficient, and full-featured HTML Document Parser for C#. You can parse an HTML text or document and retrieve any element(s) in it.

### Where can I get it?

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [SimpleHTMLParser](https://www.nuget.org/packages/simplehtmlparser/) from the package manager console:

```
PM> Install-Package SimpleHTMLParser
```
Or from the .NET CLI as:
```
dotnet add package SimpleHTMLParser
```

```csharp

var htmlText =
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
        <span class=""mySpan"">This is a span under this DIV</span>
    </div>

</body>
</html>";

        var htmlParser = new HTMLParser();
        var htmlDocument = htmlParser.Parse(htmlText);

        //get a P Tag with a particular Attribute...
        IHtmlElement paragraph1 = htmlDocument.FindElement(By.Id("paragraph1"));

        Console.WriteLine("Paragraph1 Text >>> {0}", paragraph1?.Text ?? string.Empty);

        IHtmlElement headerElement = htmlDocument.FindElement(By.ClassName("Header2"));
        Console.WriteLine("Header2 Text >>> {0}", headerElement?.Text);

        //get div...
        IHtmlElement myDiv = htmlDocument.FindElement(By.Id("myDiv"));
        // get other elements under this DIV
        var paragraphsUnderMyDiv = myDiv.FindElements(By.ElementTag("p"));
        var spanUnderDivWithClassMySpan = myDiv.FindElements(By.ClassName("mySpan"));
        Console.WriteLine("This is the text from our Span under the DIV >>> {0}", spanUnderDivWithClassMySpan?.Text);
        Console.WriteLine("There are {0} paragraph tags under DIV myDiv", paragraphsUnderMyDiv.Count());
        




```

### You can also check [Examples](https://github.com/propenster/simplehtmlparser/tree/main/examples) folder for code samples.

### Do you have an issue?

If you're facing some problems using the package, file an [issue](https://github.com/propenster/simplehtmlparser/issues) above.

### License, etc.
SimpleHTMLParser is Copyright &copy; 2023 [Faith Olusegun](https://github.com/propenster) and other contributors under the [MIT license](https://github.com/propenster/simplehtmlparser/blob/main/LICENSE).

Contributions, Issues submissions, PRs etc are in order. Please communicate if you any issues or want to contribute.







