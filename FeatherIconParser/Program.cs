using Ocluse.LiquidSnow.Core.Extensions;
using System.Xml.Linq;

Console.WriteLine("Hello, World!");
string? path = Console.ReadLine();

if(string.IsNullOrWhiteSpace(path))
{
    Console.WriteLine("Please enter a path");
    return;
}
var files = Directory.GetFiles(path.Trim('"'), "*.svg");

Dictionary<string, string> elements = new();

foreach (var iconFile in files)
{
    //get the fileName
    var fileName = Path.GetFileNameWithoutExtension(iconFile);

    //icon name should be the pascal version of file name:
    var iconName = fileName.ToTitleCase().Replace("-", "");

    //load the file to an XDocument
    var doc = XDocument.Load(iconFile);

    //get the svg element
    var svg = doc.Root;
    if (svg != null && svg.Name.LocalName == "svg")
    {
        var svgContent = string.Concat(svg.Nodes()).Replace(@"xmlns=""http://www.w3.org/2000/svg""", "").Replace("  ", " ");
        elements.Add(iconName, svgContent);
    }
}

File.WriteAllLines("output.txt", elements.Select(x => $"public const string {x.Key} = @\"{x.Value.Replace("\"","\"\"")}\";"));